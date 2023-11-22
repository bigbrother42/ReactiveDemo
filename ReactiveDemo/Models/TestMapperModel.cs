using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models
{
    public class TestMapperModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Code}, {Name}, {Description}";
        }
    }
}
