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
            DtoDatabaseModel.TableNames = new List<string>();

            foreach (Table table in targetDb.Tables)
            {
                DtoDatabaseModel.TableNames.Add(table.Schema + "." + table.Name);
            }

            return DtoDatabaseModel;
        }
    }
}
