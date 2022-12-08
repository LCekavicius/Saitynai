namespace WebUi.Data.Helpers.Wrappers
{
    public class Result<T> : IResult
    {
        public T Data { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string> Messages { get; set; }

        public Result(T data, bool success, params string[] messages)
        {
            this.Data = data;
            this.IsSuccessful = success;
            this.Messages = new List<string>();
            foreach (var item in messages)
            {
                this.Messages.Add(item);
            }
        }
    }

    public class Result : IResult
    {
        public bool IsSuccessful { get; set; }
        public List<string> Messages { get; set; }

        public Result(bool success, params string[] messages)
        {
            this.IsSuccessful = success;
            this.Messages = new List<string>();
            foreach (var item in messages)
            {
                this.Messages.Add(item);
            }
        }
    }
}
