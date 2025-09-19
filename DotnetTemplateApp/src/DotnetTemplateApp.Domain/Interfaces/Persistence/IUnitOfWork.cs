using DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account;

namespace DotnetTemplateApp.Domain.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit(bool clearTracker = false);
        /// <summary>
        /// Clear any changes in the change tracker to undo all uncommited changes.
        /// </summary>
        void ClearChanges();

        #region Repositories

        #region Account
        IUserAccountRepository UserAccountRepository { get; }
        IUserRepository UserRepository { get; }

        #endregion

        #endregion
    }
}
