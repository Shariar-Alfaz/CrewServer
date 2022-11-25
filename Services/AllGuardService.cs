using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Services
{
    public class AllGuardService
    {
        private readonly AllGuardRepo _allGuardRepo;
        private readonly TokenRepo TokenRepo;

        public AllGuardService(AllGuardRepo allGuardRepo, TokenRepo tokenRepo)
        {
            _allGuardRepo = allGuardRepo;
            TokenRepo = tokenRepo;
        }

        public async Task<Token> GetToken(string token)
        {
            var find = await _allGuardRepo.GetToken(token);
            if (find != null)
            {
                if (find.ExpiredAt > DateTime.Now)
                {
                    return find;
                }
            }

            return null;
        }

        public async Task<Admin?> GetAdmin(string token)
        {
            return await TokenRepo.GetAdmin(token);
        }

        public async Task<Teacher?> GetTeacher(string token)
        {
            return await TokenRepo.GetTeacher(token);
        }

        public async Task<Student?> GetStudent(string token)
        {
            return await TokenRepo.GetStudent(token);
        }

        public bool UpdateToken(Token token)
        {
            token.ExpiredAt= DateTime.Now;
            return _allGuardRepo.updateToken(token);
        }
    }
}

