using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;

namespace Bussiness
{
    public class DatabaseCopy : IDatabaseCopy
    {
        public void CopyDatabaseSMO(string sourceConnectionString, string destinationConnectionString, string userID)
        {
            SqlConnection sourceConn = new SqlConnection(sourceConnectionString);
            sourceConn.Open();
            SqlConnection targetConn = new SqlConnection(destinationConnectionString);
            targetConn.Open();

            ServerConnection originalConn = new ServerConnection();
            originalConn.ServerInstance = sourceConn.DataSource;
            originalConn.LoginSecure = true;
            Server originalServer = new Server(originalConn);

            ServerConnection destinationConn = new ServerConnection();
            destinationConn.ServerInstance = targetConn.DataSource;
            destinationConn.LoginSecure = true;
            Server destinationServer = new Server(destinationConn);

            Database originalDB = originalServer.Databases[sourceConn.Database];
            Database destinationDB = destinationServer.Databases[userID + "_" + sourceConn.Database];

            if (destinationDB == null)
            {
                destinationDB = new Database(destinationServer, userID + "_" + sourceConn.Database);
                destinationDB.Create();
            }

            foreach (Schema sch in originalDB.Schemas)
            {
                Schema schema = destinationDB.Schemas[sch.Name];
                if (schema == null)
                {
                    schema = new Schema(destinationDB, sch.Name);
                    schema.Create();
                }
            }

            createUserDefinedDataTypes(originalDB, destinationDB);
            createXMLSchemaCollections(originalDB, destinationDB);
            createUserDefinedTypes(originalDB, destinationDB);

            foreach (Table table in originalDB.Tables)
            {
                createTable(table, table.Schema, destinationServer, destinationDB);
                copyTabledata(sourceConn.DataSource, sourceConn.Database, table.Name, targetConn.DataSource,
                    destinationDB.Name, table.Name, table.Schema, table.Schema);
            }

            sourceConn.Close();
            targetConn.Close();
        }

        public void CopyDatabaseSMO(string originalServerName, string originalDBName,
            string destinationServerName, string userID)
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
            Database destinationDB = destinationServer.Databases[userID + "_" +originalDBName];

            if(destinationDB == null)
            {
                destinationDB = new Database(destinationServer, userID + "_" +originalDBName);
                destinationDB.Create();
            }

            foreach(Schema sch in originalDB.Schemas)
            {
                Schema schema = destinationDB.Schemas[sch.Name];
                if (schema == null)
                {
                    schema = new Schema(destinationDB, sch.Name);
                    schema.Create();
                }
            }
            
            createUserDefinedDataTypes(originalDB, destinationDB);
            createXMLSchemaCollections(originalDB, destinationDB);
            createUserDefinedTypes(originalDB, destinationDB);

            foreach (Table table in originalDB.Tables)
            {
                createTable(table, table.Schema, destinationServer, destinationDB);
                copyTabledata(originalServerName, originalDBName, table.Name, destinationServerName,
                    destinationDB.Name, table.Name, table.Schema, table.Schema);
            }

        }

        private void copyTabledata(string sourceServer, string sourceDatabase, string sourceTable,
            string destinationServer, string destinationDatabase, string destinationTable, string schemaName, string originalSchemaName)
        {
            SqlConnection sourceConn = new SqlConnection("Data Source=" + sourceServer +
                ";Database=" + sourceDatabase + ";Integrated Security=True;MultipleActiveResultSets=true");
            SqlConnection destinationConn = new SqlConnection("Data Source=" + destinationServer +
                ";Database=" + destinationDatabase + ";Integrated Security=True;MultipleActiveResultSets=true");

            SqlCommand selectData = new SqlCommand("SELECT * FROM " + originalSchemaName + ".[" + sourceTable + "]", sourceConn);
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

        private void createTable(Table sourcetable, string schema, Server destinationServer,
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
        private void createUserDefinedDataTypes(Database originalDB, Database destinationDB)
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
                catch (Exception ex)
                {
                    throw (ex);
                }

            }

        }

        private void createUserDefinedTypes(Database originalDB, Database destinationDB)
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
                catch (Exception ex)
                {
                    throw (ex);
                }

            }

        }
        private void createXMLSchemaCollections(Database originalDB, Database destinationDB)
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
                catch (Exception ex)
                {
                    throw (ex);
                }

            }

        }
        private void createColumns(Table sourcetable, Table copiedtable)
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
