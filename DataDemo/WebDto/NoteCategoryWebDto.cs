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
    public class NoteCategoryWebDto : WebDtoBase
    {
        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("TypeId")]
        public int TypeId { get; set; }

        [JsonProperty("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }
    }
}
