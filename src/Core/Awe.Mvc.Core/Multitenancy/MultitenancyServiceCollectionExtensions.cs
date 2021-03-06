﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Awe.Mvc.Core.Multitenancy;
using Awe.Mvc.Core.Multitenancy.Internal;
using System.Reflection;
using Awe.Menu.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy<TTenant, TResolver>(this IServiceCollection services)
            where TResolver : class, ITenantResolver<TTenant>
            where TTenant : class
        {
            Ensure.Argument.NotNull(services, nameof(services));

            services.AddScoped<ITenantResolver<TTenant>, TResolver>();

            // No longer registered by default as of ASP.NET Core RC2
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddScoped<IAweMenuService, AweMenuService>();

            // Make Tenant and TenantContext injectable
            services.AddScoped(prov => prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>());
			services.AddScoped(prov => prov.GetService<TenantContext<TTenant>>()?.Tenant);

			// Make ITenant injectable for handling null injection, similar to IOptions
			services.AddScoped<ITenant<TTenant>>(prov => new TenantWrapper<TTenant>(prov.GetService<TTenant>()));

			// Ensure caching is available for caching resolvers
			var resolverType = typeof(TResolver);
            if (typeof(MemoryCacheTenantResolver<TTenant>).IsAssignableFrom(resolverType))
            {
                services.AddMemoryCache();
            }

            return services;
        }

        public static IServiceCollection AddDefaultMultitenancy<TTenantResolver>(this IServiceCollection services)
            where TTenantResolver : class, ITenantResolver<AweAppTenant>
        {
            return services.AddMultitenancy<AweAppTenant, TTenantResolver>();
        }

        public static IServiceCollection AddDefaultMultitenancy(this IServiceCollection services)
        {
            return services.AddMultitenancy<AweAppTenant, AweDefaultTenantResolver>();
        }

        public static IServiceCollection AddDefaultCachedMultitenancy(this IServiceCollection services)
        {
            return services.AddMultitenancy<AweAppTenant, AweCachingAppTenantResolver>();
        }
    }
}
