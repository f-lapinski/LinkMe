using LinkMe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Application.Interfaces
{
    public interface ICurrentAccountProvider
    {
        Task<int?> GetAccountId();

        Task<Account> GetAuthenticatedAccount();

        
    }
}
