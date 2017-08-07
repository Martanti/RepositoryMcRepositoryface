using System;
using System.Collections.Generic;

namespace Dto
{
    public class Database : BaseModel
    {
        public string Name { get; set; }
        public string InternalName { get; set; }
        public string OriginalConnectionString { get; set; }
        public string InternalConnectionString { get; set; }
        public List<Table> Tables { get; set; }
        public int TableCount { get; set; }
    }

    public class Table : BaseModel
    { 
        public Table()
        {
            Columns = new List<string>();
            Rows = new List<Row>();
        }
        public string Name { get; set; }
        public string Schema { get; set; }
        public List<string> Columns { get; set; }
        public List<Row> Rows { get; set; }
        public int RowCount { get; set; }
    }
    public class Row : BaseModel
    {
        public Row()
        {
            Values = new List<string>();
        }
        public List<string> Values { get; set; }

    }

}
