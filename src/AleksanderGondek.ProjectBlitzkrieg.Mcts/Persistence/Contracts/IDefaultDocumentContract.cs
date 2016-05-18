using System;
using System.Linq;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Contracts
{
    public interface IDefaultDocumentContract<T> : IDisposable where T: class, IMctsNode, new()
    {
        bool Store(T entity);
        bool Delete(T entity);
        bool Delete(string entityId);
        bool Update(T entity);
        T Get(string entityId);
        IQueryable<T> AllEntities();

        void CleanAll();
    }
}
