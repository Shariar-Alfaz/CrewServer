using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Services.Tools
{
    public class TokenGenerator<T>
    {
        private T User;
        private string role;
        private string Token;

        public TokenGenerator( T enUser, string role)
        {
            this.User = enUser;
            this.role = role;
        }

        private void GenerateToken()
        {

        }
    }
}
