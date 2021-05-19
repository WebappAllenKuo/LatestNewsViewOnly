using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Infra;

namespace LatestNewsWeb.Infra.Validate
{
    public class ValidatorFactory
    {
        private readonly IServiceProvider   _serviceProvider;
        private readonly MemoryCacheService _memoryCacheService;

        public ValidatorFactory(IServiceProvider   serviceProvider,
                                MemoryCacheService memoryCacheService)
        {
            _serviceProvider    = serviceProvider;
            _memoryCacheService = memoryCacheService;
        }

        public void Validate<T>(ModelStateDictionary modelState, T dto)
        {
            GetValidator<T>()?.Validate(modelState, dto);
        }

        private Validator<T> GetValidator<T>()
        {
            var type = Assembly.GetExecutingAssembly()
                               .GetTypes()
                               .FirstOrDefault(t =>
                                               {
                                                   if (t.BaseType is Type baseType
                                                    && baseType.IsGenericType
                                                    && baseType.GetGenericTypeDefinition() == typeof(Validator<>)
                                                    && baseType.GenericTypeArguments.Contains(typeof(T)))
                                                   {
                                                       return true;
                                                   }

                                                   return false;
                                               });
            return _serviceProvider.GetService(type) as Validator<T>;
        }
    }
}
