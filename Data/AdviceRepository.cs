using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public class AdviceRepository : RepositoryBase<Advice>, IAdviceRepository
    {
        public AdviceRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
