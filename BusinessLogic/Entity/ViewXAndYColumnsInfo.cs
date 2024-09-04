using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace chart_report_api.BusinessLogic.Entity
{

    public class XColumn
    {
        [JsonIgnore]
        public string NameAlias => $"x_{Name}";
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
    public class YColumn
    {
        [JsonIgnore]
        public string NameAlias => $"y_{Name}";
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
    public class YColumns
    {
        public List<YColumn> Column { get; set; }
    }

    public class ViewXAndYColumnsInfo
    {
        public XColumn XColumn { get; set; }
        public List<YColumns> YColumn { get; set; }
    }
}