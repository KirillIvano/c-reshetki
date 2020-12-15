using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data.shared
{
    [Serializable]
    public class Storage<TEntity>
    {
       public Storage()
       {
            if (typeof(TEntity).IsSerializable) {
                Rows = new List<TEntity>();
                CurrentId = 0;
            } else
            {
                throw new NotSupportedException("Storage entity must be serializable");
            }
       }

        public Storage(int id, List<TEntity> rows)
        {
            if (typeof(TEntity).IsSerializable)
            {
                Rows = rows;
                CurrentId = id;
            }
            else
            {
                throw new NotSupportedException("Storage entity must be serializable");
            }
        }

        public int CurrentId { get; set; }

        public List<TEntity> Rows { get; set; }
    }
}
