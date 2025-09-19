using DotnetTemplateApp.Domain.Interfaces.Persistence;

namespace DotnetTemplateApp.Domain.Models
{

    public abstract class Model : IModel
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual DateTime? DateModified { get; set; }
    }
}
