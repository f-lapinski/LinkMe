﻿using LinkMe.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Domain.Entities
{
    public class User : DomainEntity
    {
        public required string Email { get; set; }

        public required string HsahedPassword { get; set; }

        public DateTimeOffset RegisterDate { get; set; }

        public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
    }
}
