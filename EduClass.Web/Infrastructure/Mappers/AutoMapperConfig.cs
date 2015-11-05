using AutoMapper;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x => {
                x.AddProfile<PersonMapperProfiles>();
                x.AddProfile<MailMapperProfiles>();
                x.AddProfile<GroupMapperProfiles>();
            });

        
            
        }
    }
}