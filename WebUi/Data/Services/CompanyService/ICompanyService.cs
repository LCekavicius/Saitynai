using WebApi.Auth.Model;
using WebApi.Data.Dtos;
using WebUi.Data.Helpers.Wrappers;

namespace WebUi.Data.Services.UserService
{
    public interface ICompanyService
    {
        Task<Result<List<CompanyDto>>> GetCompanies();
        Task<Result<CompanyDto>> GetCompany(int companyId);
        Task<Result<CompanyDto>> InsertCompany(CreateCompanyDto dto);
        Task<Result<CompanyDto>> UpdateCompany(int companyId, UpdateCompanyDto dto);
        Task<Result> DeleteCompany(int companyId);

    }
}