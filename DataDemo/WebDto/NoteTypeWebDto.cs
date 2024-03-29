﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDemo.WebDto
{
    public class NoteTypeWebDto : WebDtoBase
    {
        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("TypeId")]
        public int TypeId { get; set; }

        [JsonProperty("TypeName")]
        public string TypeName { get; set; }

        [JsonProperty("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}
