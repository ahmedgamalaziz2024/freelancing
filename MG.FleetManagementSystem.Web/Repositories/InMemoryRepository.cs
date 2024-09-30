using System.Collections.Generic;
using System.Linq;

namespace MG.FleetManagementSystem.Web.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        protected readonly List<T> _entities = new List<T>();
        protected int _nextId = 1;

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public T GetById(int id)
        {
            return _entities.FirstOrDefault(e => (int)e.GetType().GetProperty("Id").GetValue(e) == id);
        }

        public void Add(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && (int)idProperty.GetValue(entity) == 0)
            {
                idProperty.SetValue(entity, _nextId++);
            }
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            var id = (int)entity.GetType().GetProperty("Id").GetValue(entity);
            var existingEntity = GetById(id);
            if (existingEntity != null)
            {
                _entities.Remove(existingEntity);
                _entities.Add(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }
    }
}