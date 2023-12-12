using DataDemo.WebDto;
using ReactiveDemo.Base.ProfileBase;
using ReactiveDemo.Models;
using ReactiveDemo.Models.Csv;
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
            CreateMap<NoteTypeWebDto, NoteTypeCsvModel>();
            CreateMap<NoteCategoryWebDto, NoteCategoryCsvModel>();
            CreateMap<NoteContentWebDto, NoteContentCsvModel>();
        }
    }
}
