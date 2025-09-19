using DotnetTemplateApp.Domain.Interfaces.Persistence;
using DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account;
using DotnetTemplateApp.Persistence.Contexts;
using DotnetTemplateApp.Persistence.Repositories.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DotnetTemplateApp.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        private IUserAccountRepository? _userAccountRepository;
        private IUserRepository? _userRepository;
        public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
            if (_context.ChangeTracker != null)
            {
                _context.ChangeTracker.Tracked += OnEntityTracked!;
                _context.ChangeTracker.StateChanged += OnEntityStateChanged!;
            }
        }

        public async Task Commit(bool clearTracker = false)
        {
            await SaveChangesAsync();
            if (clearTracker && _context.ChangeTracker != null)
            {
                _context.ChangeTracker.Clear();
            }
        }

        private async Task SaveChangesAsync()
        {
            var saved = false;
            while (!saved)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    saved = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Exception occurred while saving changes to the database");
                }
            }
        }

        /// <inheritdoc/>
        public void ClearChanges()
        {
            _context.ChangeTracker?.Clear();
        }

        public IUserAccountRepository UserAccountRepository
        {
            get
            {
                _userAccountRepository ??= new UserAccountRepository(_context);
                return _userAccountRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_context);
                return _userRepository;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is IModel addedModel)
            {
                addedModel.DateCreated = DateTime.UtcNow;
            }
            else if (e.Entry.State == EntityState.Modified && e.Entry.Entity is IModel updatedModel)
            {
                updatedModel.DateModified = DateTime.UtcNow;
                e.Entry.Property("DateModified").IsModified = true;
                e.Entry.Property("DateCreated").IsModified = false;
            }
        }

        void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            if (e.NewState == EntityState.Modified && e.Entry.Entity is IModel updatedModel)
            {
                updatedModel.DateModified = DateTime.UtcNow;
                e.Entry.Property("DateModified").IsModified = true;
                e.Entry.Property("DateCreated").IsModified = false;
            }
        }
    }
}
