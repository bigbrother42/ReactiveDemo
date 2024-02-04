using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDemo.WebDto
{
    [JsonObject]
    [Serializable]
    public class SystemConfigWebDto : WebDtoBase
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("FunctionNo")]
        public string FunctionNo { get; set; }

        [JsonProperty("ItemNo")]
        public string ItemNo { get; set; }

        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        [JsonProperty("ConfigContent")]
        public string ConfigContent { get; set; }
    }
}
