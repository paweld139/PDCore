using PDCore.Interfaces;
using PDCoreNew.Context.IContext;
using PDCoreNew.Repositories.Repo;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using AutoMapper;

namespace PDCoreNew.Factories.Fac.Repository
{
    /// <summary>
    /// A maker of Repositories.
    /// </summary>
    /// <remarks>
    /// An instance of this class contains repository factory functions for different types.
    /// Each factory function takes an EF <see cref="IEntityFrameworkDbContext"/> and returns
    /// a repository bound to that DbContext.
    /// <para>
    /// Designed to be a "Singleton", configured at web application start with
    /// all of the factory functions needed to create any type of repository.
    /// Should be thread-safe to use because it is configured at app start,
    /// before any request for a factory, and should be immutable thereafter.
    /// </para>
    /// </remarks>
    public class RepositoryFactories
    {
        /// <summary>
        /// Get the dictionary of repository factory functions.
        /// </summary>
        /// <remarks>
        /// A dictionary key is a System.Type, typically a repository type.
        /// A value is a repository factory function
        /// that takes a <see cref="IEntityFrameworkDbContext"/> and <see cref="ILogger"/> arguments and returns
        /// a repository object. Caller must know how to cast it.
        /// </remarks>
        private readonly IDictionary<Type, Func<IEntityFrameworkDbContext, ILogger, IMapper, object>> _repositoryFactories;

        /// <summary>
        /// Constructor that initializes with runtime Code Camper repository factories
        /// </summary>
        public RepositoryFactories()
        {
            _repositoryFactories = GetFactories();
        }

        /// <summary>
        /// Constructor that initializes with an arbitrary collection of factories
        /// </summary>
        /// <param name="factories">
        /// The repository factory functions for this instance. 
        /// </param>
        /// <remarks>
        /// This ctor is primarily useful for testing this class
        /// </remarks>
        public RepositoryFactories(IDictionary<Type, Func<IEntityFrameworkDbContext, ILogger, IMapper, object>> factories)
        {
            _repositoryFactories = factories;
        }

        /// <summary>
        /// Return the runtime repository factory functions,
        /// each one is a factory for a repository of a particular type.
        /// </summary>
        /// <remarks>
        /// MODIFY THIS METHOD TO ADD CUSTOM FACTORY FUNCTIONS
        /// </remarks>
        protected virtual IDictionary<Type, Func<IEntityFrameworkDbContext, ILogger, IMapper, object>> GetFactories()
        {
            return new Dictionary<Type, Func<IEntityFrameworkDbContext, ILogger, IMapper, object>>();
        }

        /// <summary>
        /// Get the repository factory function for the type.
        /// </summary>
        /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
        /// <returns>The repository function if found, else null.</returns>
        /// <remarks>
        /// The type parameter, T, is typically the repository type 
        /// but could be any type (e.g., an entity type)
        /// </remarks>
        public Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetRepositoryFactory<T>()
        {
            _repositoryFactories.TryGetValue(typeof(T), out Func<IEntityFrameworkDbContext, ILogger, IMapper, object> factory);

            return factory;
        }

        /// <summary>
        /// Get the factory for <see cref="ISqlRepositoryEntityFramework{T}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="T">The root type of the repository, typically an entity type.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="ISqlRepositoryEntityFramework{T}"/>, given an EF <see cref="IEntityFrameworkDbContext"/>.
        /// </returns>
        /// <remarks>
        /// Looks first for a custom factory in <see cref="_repositoryFactories"/>.
        /// If not, falls back to the <see cref="GetDefaultEntityRepositoryFactory{T}"/>.
        /// You can substitute an alternative factory for the default one by adding
        /// a repository factory for type "T" to <see cref="_repositoryFactories"/>.
        /// </remarks>
        public Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetRepositoryFactoryForEntityType<T>() where T : class, IModificationHistory
        {
            return GetRepositoryFactory<T>() ?? GetDefaultEntityRepositoryFactory<T>();
        }

        public Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetRepositoryFactoryForEntityTypeConnected<T>() where T : class, IModificationHistory, new()
        {
            return GetRepositoryFactory<T>() ?? GetDefaultEntityRepositoryFactoryConnected<T>();
        }

        public Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetRepositoryFactoryForEntityTypeDisconnected<T>() where T : class, IModificationHistory
        {
            return GetRepositoryFactory<T>() ?? GetDefaultEntityRepositoryFactoryDisconnected<T>();
        }

        /// <summary>
        /// Default factory for a <see cref="ISqlRepositoryEntityFramework{T}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="T">Type of the repository's root entity</typeparam>
        protected virtual Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetDefaultEntityRepositoryFactory<T>() where T : class, IModificationHistory
        {
            return (dbContext, logger, mapper) => new SqlRepositoryEntityFramework<T>(dbContext, logger, mapper);
        }

        protected virtual Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetDefaultEntityRepositoryFactoryConnected<T>() where T : class, IModificationHistory, new()
        {
            return (dbContext, logger, mapper) => new SqlRepositoryEntityFrameworkConnected<T>(dbContext, logger, mapper);
        }

        protected virtual Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetDefaultEntityRepositoryFactoryDisconnected<T>() where T : class, IModificationHistory
        {
            return (dbContext, logger, mapper) => new SqlRepositoryEntityFrameworkDisconnected<T>(dbContext, logger, mapper);
        }

        public virtual Func<IEntityFrameworkDbContext, ILogger, IMapper, object> GetDefaultRepositoryFactory<T>()
        {
            return (dbContext, logger, mapper) => Activator.CreateInstance(typeof(T), dbContext, logger, mapper);
        }
    }
}
