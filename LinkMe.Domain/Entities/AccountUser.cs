using LinkMe.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Domain.Entities
{
    public class AccountUser : DomainEntity
    {
        public int AccountId { get; set; }

        public required Account Account { get; set; } //lepiej uzyc = default; ale to sie dowiem pozniej dlaczego

        public int UserId { get; set; }

        public required User User { get; set; }

    }
}
