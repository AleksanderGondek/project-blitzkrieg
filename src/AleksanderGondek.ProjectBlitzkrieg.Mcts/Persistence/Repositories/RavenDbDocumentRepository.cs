using System;
using System.Linq;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Contracts;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Repositories
{
    public class RavenDbDocumentRepository<T> : IDefaultDocumentContract<T> where T : class, IMctsNode, new()
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Store(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string entityId)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public T Get(string entityId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> AllEntities()
        {
            throw new NotImplementedException();
        }

        public void CleanAll()
        {
            throw new NotImplementedException();
        }
    }
}
