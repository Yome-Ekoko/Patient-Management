﻿using Patient_Management.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Patient_Management.Core.Repository.Base
{
    public class RepositoryBase
    {
        public RepositoryBase(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected IServiceScopeFactory _serviceScopeFactory;

        public ApplicationDbContext GetDatabaseContext(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public void ClearChangeTracking()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                dbContext.ChangeTracker.Clear();
            }
        }
    }
}
