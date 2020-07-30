using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFramework<T> : ISqlRepository<T> where T : class, IModificationHistory
    {
        IQueryable<T> FindAll(bool asNoTracking);

        IQueryable<KeyValuePair<TKey, TValue>> FindKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>;

        IQueryable<T> FindByFilter(Converter<T, string> converter, string substring);

        IQueryable<T> FindPage(int page, int pageSize);

        IQueryable<T> FindByDateCreated(IQueryable<T> source, string dateF, string dateT);

        IQueryable<T> FindByDateCreated(IQueryable<T> source, DateTime? dateF, DateTime? dateT);

        IQueryable<T> FindByDateModified(IQueryable<T> source, string dateF, string dateT);

        IQueryable<T> FindByDateModified(IQueryable<T> source, DateTime? dateF, DateTime? dateT);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);


        T FindByKeyValues(params object[] keyValues);

        List<T> GetAll(bool asNoTracking);

        List<T> Get(Expression<Func<T, bool>> predicate);

        List<KeyValuePair<TKey, TValue>> GetKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>;

        List<T> GetByFilter(Converter<T, string> converter, string substring);

        List<T> GetPage(int page, int pageSize);

        int GetCount(Expression<Func<T, bool>> predicate);


        void Attach(T obj);

        void DeleteAndCommit(T entity);

        int CommitAsClientWins();

        int CommitAsDatabaseWins();

        int CommitWithOptimisticConcurrency();

        bool DeleteAndCommitWithOptimisticConcurrency(T entity, Action<string, string> writeError);
    }
}
