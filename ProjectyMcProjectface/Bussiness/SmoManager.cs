using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace Bussiness
{
    public class SmoManager : ISmoManager
    {
        public int GetTableCountByConnectionString(string connString)
        {
            SqlConnection targetConn = new SqlConnection(connString);
            ServerConnection serverConn = new ServerConnection();
            serverConn.ServerInstance = targetConn.DataSource;
            serverConn.LoginSecure = true;

            Server server = new Server(serverConn);
            Database targetDb = server.Databases[targetConn.Database];

            return targetDb.Tables.Count;
        }
        public Dto.Database GetDatabaseByInternalConnString(string connString, string customName="")
        {
            Dto.Database DtoDatabaseModel = new Dto.Database();

            SqlConnection targetConn = new SqlConnection(connString);
            ServerConnection serverConn = new ServerConnection();
            serverConn.ServerInstance = targetConn.DataSource;
            serverConn.LoginSecure = true;

            Server server = new Server(serverConn);
            Database targetDb = server.Databases[targetConn.Database];

            DtoDatabaseModel.InternalConnectionString = connString;
            DtoDatabaseModel.Name = customName;
            DtoDatabaseModel.InternalName = targetConn.Database;
            DtoDatabaseModel.Tables = new List<Dto.Table>();

            foreach (Table table in targetDb.Tables)
            {
                Dto.Table tableModel = new Dto.Table();
                foreach(Column column in table.Columns)
                {
                    tableModel.Columns.Add(column.Name);
                }
                tableModel.Name = table.Name;
                tableModel.Schema = table.Schema;
                tableModel.RowCount = (int)table.RowCount;

                DtoDatabaseModel.Tables.Add(tableModel);
            }

            return DtoDatabaseModel;
        }
        public Dto.Table GetCompleteTableData(string connString,string schema, string name)
        {
            Dto.Table tableModel = new Dto.Table();

            SqlConnection targetConn = new SqlConnection(connString);
            ServerConnection serverConn = new ServerConnection();
            serverConn.ServerInstance = targetConn.DataSource;
            serverConn.LoginSecure = true;

            Server server = new Server(serverConn);
            Database targetDb = server.Databases[targetConn.Database];

            DataSet dataSet = targetDb.ExecuteWithResults("SELECT * FROM " + schema + "." + name);
            tableModel.Name = name;
            tableModel.Schema = schema;
            foreach(DataTable sourceTable in dataSet.Tables)
            {
                foreach(DataColumn col in sourceTable.Columns)
                {
                    tableModel.Columns.Add(col.ColumnName);
                }
                for(int x = 0; x < sourceTable.Rows.Count; x++)
                {
                    Dto.Row row = new Dto.Row();
                    foreach(var item in sourceTable.Rows[x].ItemArray)
                    {
                        if (item == null)
                        {
                            row.Values.Add("NULL");
                        }
                        else
                        {
                            row.Values.Add(item.ToString());
                        }
                    }
                    tableModel.Rows.Add(row);
                }
            }

            return tableModel;
        }
    }
}
