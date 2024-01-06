﻿using LinkMe.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Application.Logic.Abstractions
{
    public abstract class BaseQueryHandler
    {
        protected readonly ICurrentAccountProvider _currentAccountProvider;
        protected readonly IApplicationDbContext _applicationDbContext;

        public BaseQueryHandler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext)
        {
            _currentAccountProvider = currentAccountProvider;
            _applicationDbContext = applicationDbContext;
        }
    }
}
