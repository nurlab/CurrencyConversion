using AutoMapper;
using CC.Application.Contracts;
using CC.Application.Contracts.Account;
using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Application.Helper;
using CC.Application.Interfaces;
using CC.Domain.Contracts;
using CC.Domain.Contracts.Conversion;
using CC.Domain.Entities;

namespace CC.Application
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            #region General
            CreateMap(typeof(ResultContract<>), typeof(ResponseContract<>));
            #endregion

            #region Conversion
            CreateMap<GetLatestExRateRequestContract, GetLatestExRateRequestDto>();
            CreateMap<ConvertLatestRequestContract, ConvertRequestDto>();
            CreateMap<GetRateHistoryRequestContract, GetRateHistoryRequestDto>();

            //CreateMap<IResultContract<GetLatestExRateResultDto>, IResponseContract<GetLatestExRateResponseContract>>();
            //CreateMap<IResultContract<GetRateHistoryResultDto>, IResponseContract<GetRateHistoryResponseContract>>();
            //CreateMap<IResultContract<ConvertLatestResultDto>, IResponseContract<ConvertLatestResponseContract>>();

            CreateMap<ResultContract<GetLatestExRateResultDto>, ResponseContract<GetLatestExRateResponseContract>>();
            CreateMap<ResultContract<GetRateHistoryResultDto>, ResponseContract<GetRateHistoryResponseContract>>();
            CreateMap<ResultContract<ConvertLatestResultDto>, ResponseContract<ConvertLatestResponseContract>>();
            CreateMap<GetLatestExRateResultDto, GetLatestExRateResponseContract>();
            CreateMap<ConvertLatestResultDto, ConvertLatestResponseContract>();
            CreateMap<GetRateHistoryResultDto, GetRateHistoryResponseContract>();

            #endregion

            #region Account
            CreateMap<SignupRequestContract, User>()
                        .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                        .ForMember(dest => dest.NormalizedUsername, opt => opt.MapFrom(src => src.Username.ToUpper()))
                        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new PasswordEncryption().EncryptPassword(src.Password)))
                        .ForMember(dest => dest.Role, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => dest.Username))
                        .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow.Ticks))
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                        .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom((src, dest, member, context) => false))
                        .ForMember(dest => dest.IsActive, opt => opt.MapFrom((src, dest, member, context) => true));


            CreateMap<User, SignupResponseContract>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            CreateMap<User, SigninResponseContract>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            #endregion

        }
    }
}
