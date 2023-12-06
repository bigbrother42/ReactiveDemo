using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDemo.WebDto.Custom
{
    [JsonObject]
    [Serializable]
    public class NoteCategoryCustomWebDto
    {
        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("ContentId")]
        public int ContentId { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }
    }
}
