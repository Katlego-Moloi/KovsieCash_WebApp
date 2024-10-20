using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public class BeneficiaryRepository : RepositoryBase<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
