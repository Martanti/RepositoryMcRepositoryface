using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class DatabaseRegisterModel
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsConnectionSuccessfull { get; set; }
        public bool IsHttpGet { get; set; }
    }
}
