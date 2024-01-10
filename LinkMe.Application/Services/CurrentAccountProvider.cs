using EFCoreSecondLevelCacheInterceptor;
using LinkMe.Application.Exceptions;
using LinkMe.Application.Interfaces;
using LinkMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Application.Services
{
    public class CurrentAccountProvider : ICurrentAccountProvider
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;

        public CurrentAccountProvider(IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider)
        {
            _applicationDbContext = applicationDbContext;
            _authenticationDataProvider = authenticationDataProvider;
        }
        public async Task<int?> GetAccountId()
        {
            var userId = _authenticationDataProvider.GetUserId();
            if (userId != null)
            {
                return await _applicationDbContext.AccountUsers
                    .Where(au => au.UserId == userId.Value)
                    .OrderBy(au => au.UserId)
                    .Select(au => (int?)au.UserId)
                    .Cacheable()
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<Account> GetAuthenticatedAccount()
        {
            var accountId = await GetAccountId();
            if (accountId == null) 
            { 
                throw new UnauthorizedAccessException();
            }

            var account = await _applicationDbContext.Accounts.Cacheable().FirstOrDefaultAsync(a => a.Id == accountId.Value);
            if (account == null)
            {
                throw new ErrorException("AccountDoesNotExist");
            }

            return account;
        }
    }
}
