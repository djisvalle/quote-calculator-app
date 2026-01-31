using AutoMapper;
using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Application.DTOs.Loan.Response;
using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace QuoteCalculator.Application.Mappings
{
    public class LoanApplicationMappingProfile : Profile
    {
        public LoanApplicationMappingProfile()
        {
            CreateMap<CreateLoanRequestDto, LoanApplication>();

            CreateMap<LoanApplication, LoanApplicationDetailsResponseDto>()
                .ForMember(dest => dest.LoanApplicationPublicId, opt => opt.MapFrom(src => src.LoanApplicationPublicId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Applicant.Title))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Applicant.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Applicant.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Applicant.DateOfBirth))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.Applicant.MobileNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Applicant.Email))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType));

            CreateMap<LoanApplication, ApplicationDetailsResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Applicant.FirstName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Applicant.Email))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
