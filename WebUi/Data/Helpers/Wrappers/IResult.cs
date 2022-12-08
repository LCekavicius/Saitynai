namespace WebUi.Data.Helpers.Wrappers
{
    public interface IResult
    {
        bool IsSuccessful { get; set; }
        List<string> Messages { get; set; }
    }
}