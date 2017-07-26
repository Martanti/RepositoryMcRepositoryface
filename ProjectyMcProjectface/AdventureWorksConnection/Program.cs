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
            CopyDatabaseSMO(@".\SQLExpress", "AdventureWorks2014", @".\SQLExpress", "Northwind_Internal", "Eziukas");

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
            createUserDefinedDataTypes(originalDB, destinationDB);
            createXMLSchemaCollections(originalDB, destinationDB);
            createUserDefinedTypes(originalDB, destinationDB);

            Console.WriteLine("All tables: ");
            foreach (Table table in originalDB.Tables)
            {
                
                Console.WriteLine(table.Schema + "."+table.Name);
                createTable(table, schemaName, destinationServer, destinationDB);
            }
            Table testTable = originalDB.Tables[0];
            Console.WriteLine("Valio");

        }

        private static void copyTabledata(string sourceServer, string sourceDatabase, string sourceTable,
            string destinationServer, string destinationDatabase, string destinationTable, string schemaName, string originalSchemaName)
        {
            SqlConnection sourceConn = new SqlConnection("Data Source=" + sourceServer +
                ";Database=" + sourceDatabase + ";Integrated Security=True;MultipleActiveResultSets=true");
            SqlConnection destinationConn = new SqlConnection("Data Source=" + destinationServer +
                ";Database=" + destinationDatabase + ";Integrated Security=True;MultipleActiveResultSets=true");

            SqlCommand selectData = new SqlCommand("SELECT * FROM "+originalSchemaName+".[" + sourceTable + "]", sourceConn);
            sourceConn.Open();
            destinationConn.Open();
            SqlDataReader reader = selectData.ExecuteReader();
            
            using (SqlBulkCopy bulk = new SqlBulkCopy(destinationConn))
            {
                bulk.DestinationTableName = schemaName + ".[" + destinationTable + "]";
                bulk.WriteToServer(reader);
                reader.Close();
            }

            
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

            try
            {
                copiedtable.Create();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        private static void createUserDefinedDataTypes(Database originalDB, Database destinationDB)
        {
            foreach (UserDefinedDataType dt in originalDB.UserDefinedDataTypes)
            {
                Schema schema = destinationDB.Schemas[dt.Schema];
                if (schema == null)
                {
                    schema = new Schema(destinationDB, dt.Schema);
                    schema.Create();
                }
                UserDefinedDataType t = new UserDefinedDataType(destinationDB, dt.Name);
                t.SystemType = dt.SystemType;
                t.Length = dt.Length;
                t.Schema = dt.Schema;
                try
                {
                    t.Create();
                }
                catch(Exception ex)
                {
                    throw (ex);
                }

            }

        }
       
        private static void createUserDefinedTypes(Database originalDB, Database destinationDB)
        {
            foreach (UserDefinedDataType dt in originalDB.UserDefinedTypes)
            {
                Schema schema = destinationDB.Schemas[dt.Schema];
                if (schema == null)
                {
                    schema = new Schema(destinationDB, dt.Schema);
                    schema.Create();
                }
                UserDefinedDataType t = new UserDefinedDataType(destinationDB, dt.Name);
                t.SystemType = dt.SystemType;
                t.Length = dt.Length;
                t.Schema = dt.Schema;
                try
                {
                    t.Create();
                }
                catch(Exception ex)
                {
                    throw (ex);
                }

            }

        }
        private static void createXMLSchemaCollections(Database originalDB, Database destinationDB)
        {
            foreach (XmlSchemaCollection col in originalDB.XmlSchemaCollections)
            {
                Schema schema = destinationDB.Schemas[col.Schema];
                if (schema == null)
                {
                    schema = new Schema(destinationDB, col.Schema);
                    schema.Create();
                }
                XmlSchemaCollection c = new XmlSchemaCollection(destinationDB, col.Name);

                c.Text = col.Text;
                c.Schema = col.Schema;


                try
                {
                    c.Create();
                }
                catch(Exception ex)
                {
                    throw (ex);
                }

            }

        }
        private static void createColumns(Table sourcetable, Table copiedtable)
        {
            Server server = sourcetable.Parent.Parent;

            foreach (Column source in sourcetable.Columns)
            {
                
                Column column = new Column(copiedtable, source.Name, source.DataType);

                column.Nullable = source.Nullable;
                
                copiedtable.Columns.Add(column);
            }
        }
        
    }
}
