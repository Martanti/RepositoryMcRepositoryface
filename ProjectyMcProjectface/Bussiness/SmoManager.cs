using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Bussiness
{
    public class SmoManager : ISmoManager
    {
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
    }
}
