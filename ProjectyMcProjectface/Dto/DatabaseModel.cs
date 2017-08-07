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
    public class RowPage : BaseModel
    {
        public List<Row> Rows { get; set; }
        public readonly int RowsInPage = 20;
        public RowPage()
        {
            Rows = new List<Row>();
        }
    }
    public class Table : BaseModel
    { 
        public Table()
        {
            Columns = new List<string>();
            Pages = new List<RowPage>();
        }
        public string Name { get; set; }
        public string Schema { get; set; }
        public List<string> Columns { get; set; }
        public List<RowPage> Pages { get; set; }
        public int RowCount { get; set; }
    }
    public class Row : BaseModel
    {
        private Row()
        {
            Values = new List<string>();
        }
        public List<string> Values { get; set; }

    }

}
