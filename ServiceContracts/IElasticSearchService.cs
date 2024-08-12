using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IElasticSearchService
    {
        public string IndexUser(User user);

        public Task<List<User>> GetUser(string text);
    }
}
