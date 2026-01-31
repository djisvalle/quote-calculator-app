using AutoMapper;
using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.Mappings
{
    public class ApplicantMappingProfile : Profile
    {
        public ApplicantMappingProfile()
        {
            CreateMap<CreateLoanRequestDto, Applicant>();
        }
    }
}
