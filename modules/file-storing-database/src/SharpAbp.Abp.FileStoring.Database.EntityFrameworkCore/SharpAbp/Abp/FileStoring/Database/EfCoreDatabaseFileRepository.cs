﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SharpAbp.Abp.FileStoring.Database
{
    public class EfCoreDatabaseFileRepository : EfCoreRepository<IFileStoringDbContext, DatabaseFile, Guid>,
         IDatabaseFileRepository
    {
        public EfCoreDatabaseFileRepository(IDbContextProvider<IFileStoringDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<DatabaseFile> FindAsync(
            Guid containerId,
            string name,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(
                x => x.ContainerId == containerId && x.Name == name,
                GetCancellationToken(cancellationToken)
            );
        }

        public virtual async Task<bool> ExistsAsync(
            Guid containerId,
            string name,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(
                x => x.ContainerId == containerId && x.Name == name,
                GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> DeleteAsync(
            Guid containerId,
            string name,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            //TODO: Should extract this logic to out of the repository and remove this method completely

            var blob = await FindAsync(containerId, name, cancellationToken);
            if (blob == null)
            {
                return false;
            }

            await base.DeleteAsync(blob, autoSave, cancellationToken: GetCancellationToken(cancellationToken));
            return true;
        }
    }
}
