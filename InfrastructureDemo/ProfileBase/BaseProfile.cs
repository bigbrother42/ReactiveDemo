using AutoMapper;

namespace ReactiveDemo.Base.ProfileBase
{
    public abstract class BaseProfile : Profile
    {
        protected BaseProfile(string profileName)
        {
            ProfileName = profileName;

            // can add customer format
            //ForSourceType<DateTime>().AddFormatter<StandardDateTimeFormatter>();
            //ForSourceType<DateTime?>().AddFormatter<StandardDateTimeFormatter>();

            CreateMaps();
        }

        public override string ProfileName { get; }

        protected abstract void CreateMaps();
    }
}
