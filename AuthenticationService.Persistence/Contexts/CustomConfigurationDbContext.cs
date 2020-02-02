using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Persistence.Contexts
{
    /// <summary>
    /// DbContext for the IdentityServer configuration data.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="IdentityServer4.EntityFramework.Interfaces.IConfigurationDbContext" />
    public class CustomConfigurationDbContext : ConfigurationDbContext<CustomConfigurationDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public CustomConfigurationDbContext(DbContextOptions<CustomConfigurationDbContext> options,
                                            ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }

    /// <summary>
    /// DbContext for the IdentityServer configuration data.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="IdentityServer4.EntityFramework.Interfaces.IConfigurationDbContext" />
    public class CustomConfigurationDbContext<TContext> : DbContext, IConfigurationDbContext
        where TContext : DbContext, IConfigurationDbContext
    {
        private ConfigurationStoreOptions StoreOptions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public CustomConfigurationDbContext(DbContextOptions<TContext> options, ConfigurationStoreOptions storeOptions) : base(options)
            => this.StoreOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>
        /// The clients.
        /// </value>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the identity resources.
        /// </summary>
        /// <value>
        /// The identity resources.
        /// </value>
        /// 
        public DbSet<IdentityResource> IdentityResources { get; set; }

        /// <summary>
        /// Gets or sets the API resources.
        /// </summary>
        /// <value>
        /// The API resources.
        /// </value>
        public DbSet<ApiResource> ApiResources { get; set; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
            => base.SaveChangesAsync();

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureClientContext(this.StoreOptions);
            modelBuilder.ConfigureResourcesContext(this.StoreOptions);

            base.OnModelCreating(modelBuilder);
        }
    }
}
