using System;
using System.Collections.Generic;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.IoC;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    public class ConfigurationServicesProvider : IConfigurationServicesProvider
    {
        public virtual IEnumerable<(Type TService, Type TImplementation)> GetScopedServices()
        {
            return null;            
        }

        public virtual IEnumerable<(Type TService, Type TImplementation)> GetSingletonServices()
        {
            return null;
        }

        public virtual IEnumerable<(Type TService, Type TImplementation)> GetTransientServices()
        {
            var services = new List<(Type, Type)>();
            
            services.Add((typeof(IDbObjectProvider), typeof(SqliteDbObjectProvider)));
            services.Add((typeof(ISqlProvider), typeof(SqliteSqlProvider)));
            services.Add((typeof(IReaderService), typeof(ReaderService)));            

            return services;
        }
    }
}
