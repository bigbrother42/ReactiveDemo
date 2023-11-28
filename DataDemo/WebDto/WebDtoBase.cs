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
        [JsonProperty("CreatedBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("CreateAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("UpdatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty("UpdateAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
