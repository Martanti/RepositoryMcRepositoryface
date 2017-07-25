﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventureWorksConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            
            ReadAllTablesFromDatabase(@"Data Source=.\SQLEXPRESS;Database=AdventureWorks2014;Integrated Security=True;");
            

            Console.ReadLine();
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
        public static void PrintTableData(string tableName, SqlConnection con)
        {
            DataTable dataTable = new DataTable();

            string query = "select * from " + tableName;
            SqlCommand command = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(command);

            da.Fill(dataTable);

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
            List<string> tables = connection.GetSchema("Tables").AsEnumerable().Select(S => S[1].ToString() + "." + S[2].ToString()).ToList();
            return tables;
        }
        
    }
}
