using System.ComponentModel.DataAnnotations;

namespace EFDataModels
{
    public class ConnectionString
    {
        
        public int ConnectionId { get; set; }
        public int UserId { get; set; }
        public string String { get; set; }

        public string DataSource { get; set; }
        public string DatabaseName { get; set; }
        public virtual RegisteredUser User { get; set; }
    }
}