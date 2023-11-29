using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDemo.WebDto
{
    [Serializable]
    public class WebDtoBase
    {
        [JsonProperty("CreateBy")]
        public int CreateBy { get; set; }

        [JsonProperty("CreateAt")]
        public DateTime CreateAt { get; set; }

        [JsonProperty("UpdateBy")]
        public int UpdateBy { get; set; }

        [JsonProperty("UpdateAt")]
        public DateTime UpdateAt { get; set; }
    }
}
