using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class NewsRepository:BaseRepository<HaberTbl>,IRepositoryNews
    {
        public override List<HaberTbl> List()
        {
            MemoryCache mCache = MemoryCache.Default;

            List<HaberTbl> model = new List<HaberTbl>();
            if(!mCache.Contains("newsList"))
            {
                model = new ProjeHaberDbEntities().Set<HaberTbl>().ToList();

                mCache.Add("newsList", model, DateTimeOffset.Now.AddSeconds(30));

            }
            else
            {
                model = (List<HaberTbl>)mCache.Get("newsList");
            }




            return model;
        }

        public void ReadCount(int id)
        {
            var ctx = new ProjeHaberDbEntities();
            HaberTbl haberTbl = ctx.HaberTbl.FirstOrDefault(c => c.Id == id);
            if(haberTbl.ReadCount==null)
            {
                haberTbl.ReadCount = 1;
               
            }
            else
            {
                haberTbl.ReadCount++;
            }
            ctx.SaveChanges();
        }

       
    }
}
