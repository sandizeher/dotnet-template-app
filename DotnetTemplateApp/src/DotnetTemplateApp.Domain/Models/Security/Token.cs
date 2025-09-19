using DotnetTemplateApp.Domain.Models.Account;

namespace DotnetTemplateApp.Domain.Models.Security
{

    public partial class Token
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public string RefreshToken { get; set; } = default!;
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Expires { get; set; }
        public DateTimeOffset? Revoked { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? ReasonRevoked { get; set; }

        public virtual UserAccount? UserAccount { get; set; }
    }
}
