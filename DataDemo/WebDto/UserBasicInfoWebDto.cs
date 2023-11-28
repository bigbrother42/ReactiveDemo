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
    public class UserBasicInfoWebDto : WebDtoBase
    {
        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }
    }
}
