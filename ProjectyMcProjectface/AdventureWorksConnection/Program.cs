using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;


namespace AdventureWorksConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            //ReadAllTablesFromDatabase(@"Data Source=.\SQLEXPRESS;Database=InternalDB;Integrated Security=True;");
            //CopyDatabase("Northwind", @".\SQLEXPRESS", "Northwind_Internal", @".\SQLEXPRESS");
            CopyDatabaseSMO(@".\SQLExpress", "Northwind", @".\SQLExpress", "Northwind_Internal", "Eziukas");

            Console.ReadLine();
        }

        public static void CopyDatabaseSMO(string originalServerName, string originalDBName,
            string destinationServerName, string destinationDBName, string schemaName)
        {
            ServerConnection originalConn = new ServerConnection();
            originalConn.ServerInstance = originalServerName;
            originalConn.LoginSecure = true;
            Server originalServer = new Server(originalConn);

            ServerConnection destinationConn = new ServerConnection();
            destinationConn.ServerInstance = destinationServerName;
            destinationConn.LoginSecure = true;
            Server destinationServer = new Server(destinationConn);

            Database originalDB = originalServer.Databases[originalDBName];
            Database destinationDB = destinationServer.Databases[destinationDBName];

            Schema schema = destinationDB.Schemas[schemaName];
            if(schema == null)
            {
                schema = new Schema(destinationDB, schemaName);
                schema.Create();
            }

            Console.WriteLine("All tables: ");
            foreach (Table table in originalDB.Tables)
            {
                
                Console.WriteLine(table.Schema + "."+table.Name);
                createTable(table, schemaName, destinationServer, destinationDB);
                copyTabledata(originalServerName, originalDBName, table);
            }
            Table testTable = originalDB.Tables[0];
            Console.WriteLine("Valio");

        }
        private static void copyTabledata(string sourceServer, string sourceDatabase, string sourceTable,
            string destinationServer, string destinationDatabase, string destinationTable)
        {
            SqlConnection sourceConn = new SqlConnection("Data Source=" + sourceServer +
                ";Database=" + sourceDatabase + ";Integrated Security=True");
            SqlConnection destinationConn = new SqlConnection("Data Source=" + destinationServer +
                ";Database=" + destinationDatabase + ";Integrated Security=True");

            SqlCommand selectData = new SqlCommand("SELECT * FROM " + sourceTable, sourceConn);
            sourceConn.Open();
            SqlDataReader reader = selectData.ExecuteReader();

            using (SqlBulkCopy bulk = new SqlBulkCopy(sourceConn))
            {
                bulk.DestinationTableName = destinationTable;
                bulk.WriteToServer(reader);
            }

            reader.Close();
            sourceConn.Close();
            destinationConn.Close();
        }
        private static void createTable(Table sourcetable, string schema, Server destinationServer, 
            Database db)
        {
            Table copiedtable = new Table(db, sourcetable.Name, schema);

            createColumns(sourcetable, copiedtable);

            copiedtable.AnsiNullsStatus = sourcetable.AnsiNullsStatus;
            copiedtable.QuotedIdentifierStatus = sourcetable.QuotedIdentifierStatus;
            copiedtable.TextFileGroup = sourcetable.TextFileGroup;
            copiedtable.FileGroup = sourcetable.FileGroup;

            copiedtable.Create();
        }
        private static void createColumns(Table sourcetable, Table copiedtable)
        {
            Server server = sourcetable.Parent.Parent;

            foreach (Column source in sourcetable.Columns)
            {
                Column column = new Column(copiedtable, source.Name, source.DataType);
                column.Collation = source.Collation;
                column.Nullable = source.Nullable;
                column.Computed = source.Computed;
                column.ComputedText = source.ComputedText;
                column.Default = source.Default;

                if (source.DefaultConstraint != null)
                {
                    string tabname = copiedtable.Name;
                    string constrname = source.DefaultConstraint.Name;
                    column.AddDefaultConstraint(tabname + "_" + constrname);
                    column.DefaultConstraint.Text = source.DefaultConstraint.Text;
                }

                column.IsPersisted = source.IsPersisted;
                column.DefaultSchema = source.DefaultSchema;
                column.RowGuidCol = source.RowGuidCol;

                if (server.VersionMajor >= 10)
                {
                    column.IsFileStream = source.IsFileStream;
                    column.IsSparse = source.IsSparse;
                    column.IsColumnSet = source.IsColumnSet;
                }

                copiedtable.Columns.Add(column);
            }
        }

        public static void CopyDatabase(string OriginalDBName, string OriginalServerName,
                                        string DestinationDBName, string DestinationServerName)
        {
            SqlConnection originalCon = new SqlConnection("Data Source=" + OriginalServerName + 
                ";Database=" + OriginalDBName + ";Integrated Security=True");
            originalCon.Open();
            SqlConnection destinationServerCon = new SqlConnection("Data Source=" + DestinationServerName + 
                ";Database="+DestinationDBName+";Integrated Security=True");
            destinationServerCon.Open();

            /*Create Db
            string createDestinationDBQuery = "CREATE DATABASE " + DestinationDBName;
            SqlCommand createDestinationDBCommand = new SqlCommand(createDestinationDBQuery, destinationServerCon);
            createDestinationDBCommand.ExecuteNonQuery();

            Console.WriteLine("DB craeted successfully");*/

            //getting table list in database

            List<string> tables = GetTableList(originalCon);
            List<string> tablesWithSchemas = GetTableWithSchemaList(originalCon);
            if(tables != null)
            {
                string schemaName = "TestSchema";
                string createSchemaDBQuery = "create schema " + schemaName;
                SqlCommand createSchemaDBCommand = new SqlCommand(createSchemaDBQuery, destinationServerCon);
                createSchemaDBCommand.ExecuteNonQuery();

                for(int x = 0; x < tables.Count; x++)
                {
                    Console.WriteLine("Copying " + tablesWithSchemas[x]);
                    CopyTableTointernalDB(OriginalDBName,schemaName, tables[x], tablesWithSchemas[x], originalCon, destinationServerCon);
                    Console.WriteLine();
                }
            }

            originalCon.Close();
            destinationServerCon.Close();

        }

        public static void ReadAllTablesFromDatabase(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            List<string> tables = GetTableList(con);
            foreach(var table in tables)
            {
                Console.WriteLine(table);
                PrintTableData(table, con);
                Console.WriteLine();
            }

            con.Close();
        }
        public static void CopyTableTointernalDB(string originalDBName, string schemaName, string tableName, string originalTableWithSchema,
            SqlConnection originalCon, SqlConnection destinationCon)
        {
            

            string createDestinationTableQuery = "create table " + schemaName + ".[" + tableName + "](";
            DataTable ColumnsDT = new DataTable();
            string getTableColumnDataQuery = "SELECT * FROM "+originalDBName+".INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName +"'";
            SqlCommand getTableColumnDataCommand = new SqlCommand(getTableColumnDataQuery, originalCon);
            SqlDataAdapter TableDA = new SqlDataAdapter(getTableColumnDataCommand);
            TableDA.Fill(ColumnsDT);
            for (int x = 0; x < ColumnsDT.Rows.Count; x++)
            {
                createDestinationTableQuery += "[" + ColumnsDT.Rows[x].ItemArray[3].ToString() + "] " + "[" + ColumnsDT.Rows[x].ItemArray[7].ToString() + "], ";
                
            }

            createDestinationTableQuery = createDestinationTableQuery.Remove(createDestinationTableQuery.Length - 2);
            createDestinationTableQuery += " )";

            SqlCommand createDestinationTableCommand = new SqlCommand(createDestinationTableQuery, destinationCon);
            createDestinationTableCommand.ExecuteNonQuery();
            Console.WriteLine("Table " + schemaName + "." + tableName + " created succesfully!");

            //Copying Now!
            
            DataTable dataTable = new DataTable();

            string getTableDataquery = "select * from " + originalTableWithSchema;
            SqlCommand getTableDataCommand = new SqlCommand(getTableDataquery, originalCon);
            SqlDataAdapter da = new SqlDataAdapter(getTableDataCommand);

            da.Fill(dataTable);

            for (int x = 0; x < dataTable.Rows.Count; x++)
            {
                string insertQuery = "insert into " + schemaName + ".["+tableName+"](" ;
                string values = "VALUES(";

                for (int y = 0; y < dataTable.Columns.Count; y++)
                {
                    insertQuery += dataTable.Columns[y].ColumnName + ", ";
                    values += dataTable.Rows[x].ItemArray[y].ToString() + ", ";
                }
                insertQuery = insertQuery.Remove(insertQuery.Length - 2);
                insertQuery += " )";
                values = values.Remove(values.Length - 2);
                values += " )";
                insertQuery += " " + values;
                SqlCommand insertCommand = new SqlCommand(insertQuery, destinationCon);
                insertCommand.ExecuteNonQuery();
                
            }

            da.Dispose();
            
        }
        public static void PrintTableData(string tableName, SqlConnection con)
        {
            DataTable dataTable = new DataTable();

            string query = "select * from " + tableName;
            SqlCommand command = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(command);

            
            da.Fill(dataTable);

            for (int x = 0; x < dataTable.Rows.Count; x++)
            {
                string line = "";

                for (int y = 0; y < dataTable.Columns.Count; y++)
                {
                    line = line + dataTable.Columns[y].ColumnName + ": " + dataTable.Rows[x].ItemArray[y].ToString() + "; ";
                }
                Console.WriteLine(line);
            }
            da.Dispose();
        }
        public static List<string> GetTableList(SqlConnection connection)
        {
            List<string> tables = connection.GetSchema("Tables").AsEnumerable().Select(S => S[2].ToString()).ToList();
            return tables;
        }
        public static List<string> GetTableWithSchemaList(SqlConnection connection)
        {
            List<string> tables = connection.GetSchema("Tables").AsEnumerable().Select(S => S[1].ToString() + ".[" + S[2].ToString() + "]").ToList();
            
            return tables;
        }
    }
}
