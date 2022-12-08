using System.Xml.Linq;

namespace WebApi.Data.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public DateTime Creationdate { get; set; }

        public CompanyDto(int id, string name, DateTime creationdate)
        {
            Id = id;
            this.name = name;
            this.Creationdate= creationdate;
        }

        public override bool Equals(object o)
        {
            var other = o as CompanyDto;
            return other?.name == name;
        }
        public override int GetHashCode() => name?.GetHashCode() ?? 0;
        public override string ToString() => name;

    }
    public record CreateCompanyDto(string name);
    public record UpdateCompanyDto(string name);
}
