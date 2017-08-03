using System;
using System.Collections.Generic;

namespace Dto
{
    public class Database
    {
        public string Name { get; set; }
        public string InternalName { get; set; }
        public string OriginalConnectionString { get; set; }
        public string InternalConnectionString { get; set; }
        public List<string> TableNames { get; set; }
    }
    public class Table
    {
        private Table()
        {
            Columns = new List<Tuple<string, ValueType>>();
            Rows = new List<Row>();
        }
        public string Name { get; set; }
        public string Schema { get; set; }
        public List<Tuple<string, ValueType>> Columns { get; set; }
        public List<Row> Rows { get; set; }
    }
    public class Row
    {
        private Row()
        {
            Values = new List<string>();
        }
        public List<string> Values { get; set; }
    }

}
