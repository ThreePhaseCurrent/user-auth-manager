using System;
using AutoMapper;
using UserAuthManager.API.Models;
using UserAuthManager.Core.Models;

namespace UserAuthManager.API.Mapping
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            MappingRegisterToApplicationUser();
            MappingApplicationUserToShortUserInfo();
        }
        
        private void MappingRegisterToApplicationUser()
        {
            CreateMap<Register, ApplicationUser>()
                .ForMember(x => x.NormalizedEmail,
                    q =>
                        q.MapFrom(x => x.Email.ToUpper()))

                .ForMember(x => x.NormalizedUserName,
                    q =>
                        q.MapFrom(x => x.UserName.ToUpper()))

                .ForMember(x => x.EmailConfirmed,
                    q =>
                        q.MapFrom(x => false))

                .ForMember(x => x.PhoneNumberConfirmed,
                    q =>
                        q.MapFrom(x => false))

                .ForMember(x => x.SecurityStamp,
                    q =>
                        q.MapFrom(x => Guid.NewGuid().ToString("D")));
        }

        private void MappingApplicationUserToShortUserInfo()
        {
            CreateMap<ApplicationUser, ShortUserInfo>()
                .ForMember(x => x.Email,
                    q =>
                        q.MapFrom(x => x.Email))
                
                .ForMember(x => x.UserName,
                    q =>
                        q.MapFrom(x => x.UserName));
        }
    }
}