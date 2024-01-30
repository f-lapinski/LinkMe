using FluentValidation;
using LinkMe.Application.Exceptions;
using LinkMe.Application.Interfaces;
using LinkMe.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Application.Logic.User
{
    public static class LogoutCommand
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private readonly IPasswordManager _passwordManager;
            public Handler(
                ICurrentAccountProvider currentAccountProvider,
                IApplicationDbContext applicationDbContext,
                IPasswordManager passwordManager) : base(currentAccountProvider, applicationDbContext)
            {
            }


            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
            }
        }
    }
}
