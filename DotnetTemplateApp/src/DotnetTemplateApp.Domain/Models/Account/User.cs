namespace DotnetTemplateApp.Domain.Models.Account
{

    public partial class User : Model
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Middlename { get; set; }
        public string? UserDisplayName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Guid UserAccountId { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
    }
}
