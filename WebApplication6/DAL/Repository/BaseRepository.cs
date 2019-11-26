using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class BaseRepository<T> where T : class
    {
        public virtual List<T> List()
        {
            var ctx = new ProjeHaberDbEntities();
            return ctx.Set<T>().ToList();
        }
        public virtual void Add(T entity)
        {
            var ctx = new ProjeHaberDbEntities();
            ctx.Set<T>().Add(entity);
            ctx.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            var ctx = new ProjeHaberDbEntities();
            ctx.Set<T>().Attach(entity);
            ctx.Entry(entity).State = EntityState.Modified;
            ctx.SaveChanges();
        }
        public virtual void Remove(int entityId)
        {
            var ctx = new ProjeHaberDbEntities();
            T Entity = ctx.Set<T>().Find(entityId);
            ctx.SaveChanges();
            if(ctx.Entry(Entity).State==EntityState.Detached)
            {
                ctx.Set<T>().Attach(Entity);
            }
            ctx.Set<T>().Remove(Entity);
            ctx.SaveChanges();
        }
        public virtual T FindById(int EntityId)
        {
            var ctx = new ProjeHaberDbEntities();
            return ctx.Set<T>().Find(EntityId);
        }
    }
}
