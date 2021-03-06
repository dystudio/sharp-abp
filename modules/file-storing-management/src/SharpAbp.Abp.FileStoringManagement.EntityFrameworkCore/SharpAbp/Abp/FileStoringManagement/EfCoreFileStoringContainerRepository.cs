﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;


namespace SharpAbp.Abp.FileStoringManagement
{
    public class EfCoreFileStoringContainerRepository : EfCoreRepository<IFileStoringManagementDbContext, FileStoringContainer, Guid>,
         IFileStoringContainerRepository
    {
        public EfCoreFileStoringContainerRepository(IDbContextProvider<IFileStoringManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        /// <summary>
        /// Find container by name
        /// </summary>
        /// <param name="name">container name</param>
        /// <param name="includeDetails">include details</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<FileStoringContainer> FindAsync([NotNull] string name, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        /// <summary>
        /// Find container by name
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="name"></param>
        /// <param name="exceptId"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<FileStoringContainer> FindAsync(Guid? tenantId, string name, Guid? exceptId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(tenantId.HasValue, x => x.TenantId == tenantId.Value)
                .WhereIf(!name.IsNullOrWhiteSpace(), x => x.Name == name)
                .WhereIf(exceptId.HasValue, x => x.Id != exceptId.Value)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Override GetAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<FileStoringContainer> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<FileStoringContainer>> GetListAsync(int skipCount, int maxResultCount, string sorting = null, bool includeDetails = true, string name = "", string provider = "", CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!name.IsNullOrWhiteSpace(), item => item.Name == name)
                .WhereIf(!provider.IsNullOrWhiteSpace(), item => item.Provider == provider)
                .OrderBy(sorting ?? nameof(FileStoringContainer.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get count async
        /// </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(string name = "", string provider = "", CancellationToken cancellationToken = default)
        {
            return await DbSet
                  .WhereIf(!name.IsNullOrWhiteSpace(), item => item.Name == name)
                  .WhereIf(!provider.IsNullOrWhiteSpace(), item => item.Provider == provider)
                  .CountAsync(cancellationToken);
        }

    }
}
