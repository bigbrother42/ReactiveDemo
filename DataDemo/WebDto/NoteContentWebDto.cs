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
    public class NoteContentWebDto : WebDtoBase
    {
        [JsonProperty("ContentId")]
        public int ContentId { get; set; }

        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }
    }
}
