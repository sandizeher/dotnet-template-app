using DotnetTemplateApp.Domain.Enums;

namespace DotnetTemplateApp.Domain.Models.Account
{

    public partial class UserAccount : Model
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; }
        public bool IsRegistered { get; set; } = false;
        public DateTime? LastLogin { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberPrefix { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public virtual User? User { get; set; }
    }
}
