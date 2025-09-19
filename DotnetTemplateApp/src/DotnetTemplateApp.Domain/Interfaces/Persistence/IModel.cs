namespace DotnetTemplateApp.Domain.Interfaces.Persistence
{
    public interface IModel
    {
        Guid Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}

