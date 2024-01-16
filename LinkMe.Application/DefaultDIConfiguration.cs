using FluentValidation;
using LinkMe.Application.Interfaces;
using LinkMe.Application.Logic.Abstractions;
using LinkMe.Application.Services;
using LinkMe.Application.Validators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMe.Application
{
    public static class DefaultDIConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(BaseQueryHandler));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
