using System;
using System.Collections.Concurrent;
using System.Linq;
using Blitzkrieg.Mcts.GameTrees;
using Blitzkrieg.Mcts.Persistence.Contracts;

namespace Blitzkrieg.Mcts.Persistence.Repositories
{
    public class InMemoryDocumentRepository<T>: IDefaultDocumentContract<T> where T: class, IMctsNode, new()
    {
        private static readonly Lazy<ConcurrentDictionary<string,T>> InMemoryLazyStore = new Lazy<ConcurrentDictionary<string,T>>();
        private static ConcurrentDictionary<string, T> InMemoryStore => InMemoryLazyStore.Value;

        public void Dispose() {}

        public bool Store(T entity)
        {
            return InMemoryStore.TryAdd(entity.Id, entity);
        }

        public bool Delete(T entity)
        {
            T removedItem;
            return InMemoryStore.TryRemove(entity.Id, out removedItem);
        }

        public bool Delete(string entityId)
        {
            T removedItem;
            return InMemoryStore.TryRemove(entityId, out removedItem);
        }

        public bool Update(T entity)
        {
            var previousValue = Get(entity.Id);
            var update = InMemoryStore.TryUpdate(entity.Id, entity, previousValue);
            return previousValue != null && update;
        }

        public T Get(string entityId)
        {
            T entity;
            InMemoryStore.TryGetValue(entityId, out entity);
            return entity;
        }

        public IQueryable<T> AllEntities()
        {
            return InMemoryStore.Values.AsQueryable();
        }

        public void CleanAll()
        {
            InMemoryStore.Clear();
        }
    }
}
