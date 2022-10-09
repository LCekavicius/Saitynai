namespace WebApi.Data.Dtos
{
    public record CompanyDto(int Id, string name, DateTime Creationdate);
    public record CreateCompanyDto(string name);
    public record UpdateCompanyDto(string name);
}
