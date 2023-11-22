using ReactiveDemo.Base.ProfileBase;
using ReactiveDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo
{
    public class ReactiveDemoMapperProfile : BaseProfile
    {
        public ReactiveDemoMapperProfile() : base(nameof(ReactiveDemoMapperProfile)) { 
            
        }
        protected override void CreateMaps()
        {
            CreateMap<TestMapperModel, TestMapperModel>();
        }
    }
}
