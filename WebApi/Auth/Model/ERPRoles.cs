namespace WebApi.Auth.Model
{
    public static class ERPRoles
    {
        public const string Admin = nameof(Admin);
        public const string Representative = nameof(Representative);
        public const string Worker = nameof(Worker);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, Representative, Worker };
    }
}
