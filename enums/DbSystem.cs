
using chart_report_api.enums;

namespace chart_report_api.enums
{
    public enum DbSystem
    {
        PostgreSQL = 1,
        MongoDb,
        MsSQL
    }

}
public static class EnumExtensions
{
    public static string ToEnumString(this DbSystem system) => system.ToString();
}