using System.Data.Common;

namespace DotnetTemplateApp.Domain.Interfaces.Persistence
{
    public interface IDapperDbContext
    {
        Task<IEnumerable<T>> ExecuteFunctionAsync<T>(string functionName, object? param = null, DbConnection? conn = null);
    }
}
