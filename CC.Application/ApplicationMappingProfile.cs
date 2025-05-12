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
    /// <summary>
    /// AutoMapper profile for mapping between domain, DTOs, and contract models.
    /// </summary>
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            #region General Contracts
            CreateMap(typeof(ResultContract<>), typeof(ResponseContract<>));
            #endregion

            #region Conversion: Request Mappings
            CreateMap<GetLatestExRateRequestContract, GetLatestExRateRequestDto>();
            CreateMap<ConvertLatestRequestContract, ConvertRequestDto>();
            CreateMap<GetRateHistoryRequestContract, GetRateHistoryRequestDto>();
            #endregion

            #region Conversion: Result to ResponseContract Mappings
            CreateMap<ResultContract<GetLatestExRateResultDto>, ResponseContract<GetLatestExRateResponseContract>>();
            CreateMap<ResultContract<GetRateHistoryResultDto>, ResponseContract<GetRateHistoryResponseContract>>();
            CreateMap<ResultContract<ConvertLatestResultDto>, ResponseContract<ConvertLatestResponseContract>>();
            #endregion

            #region Conversion: DTO to Contract Mappings
            CreateMap<GetLatestExRateResultDto, GetLatestExRateResponseContract>();
            CreateMap<ConvertLatestResultDto, ConvertLatestResponseContract>();
            CreateMap<GetRateHistoryResultDto, GetRateHistoryResponseContract>();
            #endregion

            #region Account: Signup
            CreateMap<SignupRequestContract, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.NormalizedUsername, opt => opt.MapFrom(src => src.Username.ToUpper()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new PasswordEncryption().EncryptPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, _, _) => dest.Username))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom((src, dest, _, _) => DateTime.UtcNow.Ticks))
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom((_, _, _, _) => false))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom((_, _, _, _) => true));

            CreateMap<User, SignupResponseContract>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
            #endregion

            #region Account: Signin
            CreateMap<User, SigninResponseContract>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            #endregion
        }
    }
}
