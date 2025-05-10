using AutoMapper;
using CC.Application.Contracts.Account;
using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Application.Interfaces;
using CC.Domain.Contracts.Conversion;
using CC.Domain.DTOs;
using CC.Domain.Entities;

namespace CC.Application
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {

            #region Conversion
            CreateMap<GetLatestExRateRequestContract, GetLatestExRateRequestDto>();
            CreateMap<ConvertLatestRequestContract, ConvertRequestDto>();
            CreateMap<GetRateHistoryRequestContract, GetRateHistoryRequestDto>();

            CreateMap<IResultContract<GetLatestExRateResultDto>, IResponseContract<GetLatestExRateResponseContract>>();
            CreateMap<IResultContract<GetRateHistoryResultDto>, IResponseContract<GetRateHistoryResponseContract>>();
            CreateMap<IResultContract<ConvertLatestResultDto>, IResponseContract<ConvertLatestResponseContract>>();
            #endregion

            #region Account
            CreateMap<SignupRequestContract, User>();
            CreateMap<User, SigninResponseContract>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            #endregion

        }
    }
}
