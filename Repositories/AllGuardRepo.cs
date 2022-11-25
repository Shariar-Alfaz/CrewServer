using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class AllGuardRepo
    {
        private readonly DataContext _dataContext;
        public AllGuardRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Token> GetToken(string token)
        {
           var tk = await _dataContext.Tokens.FirstOrDefaultAsync(x => x.Key.Equals(token));
            return tk;
        }

        public bool updateToken(Token token)
        {
            var check=_dataContext.Tokens.Update(token);
            return check==null?false:true;
        }
    }
}
