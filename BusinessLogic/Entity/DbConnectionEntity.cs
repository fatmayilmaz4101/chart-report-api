
using chart_report_api.enums;

namespace chart_report_api.BusinessLogic.Entity
{
    public class DbConnectionEntity
    {
        public DbSystem System { get; set; }
        public required string Server { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Database { get; set; }

    }
}