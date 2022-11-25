﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTO;
using Microsoft.Identity.Client;
using Repositories;
using Services.Tools;

namespace Services
{
    public class AdminService
    {
        private readonly AdminRepo _adminRepo;
        public AdminService(AdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public async Task<Admin> SaveAdmin(AdminResDTO admin)
        {
            var newAdmin = this.ConvertAdmin(admin);
            var data= await _adminRepo.Save(newAdmin);
            if (data != null)
            {
                var hash = new Hash(admin.Password);
                var loginInfo = new LoginInformation()
                {
                    LoginEmail = data.Email,
                    LoginId = data.UserId,
                    PasswordHash = hash.GetHash(),
                    PasswordSalt = hash.GetSalt(),
                    RoleId = 1
                };
                if (await _adminRepo.SaveLoginInfo(loginInfo)) return data;
                return null;
            }

            return data;
        }

        private  Admin ConvertAdmin(AdminResDTO admin)
        {
            var hash = new Hash(admin.Password);

            var data =new Admin()
            {
                Email = admin.Email,
                Name = admin.Name,
                UserId = admin.UserId,
                BirthDate = Convert.ToDateTime(admin.Date),
            };
            return data;
        }

        public async Task<bool> CheckIfEmailExist(string email)
        {
            return await _adminRepo.CheckIfEmailExist(email);
        }

        public async Task<bool> CheckIfUserIdExist(string id)
        {
            return await _adminRepo.CheckIfUserIdExist(id);
        }

        public async Task<Admin> GetMe(string token)
        {
            return  await _adminRepo.GetMe(token);
        }

        public async Task<List<Admin?>> GetAll()
        {
            return await _adminRepo.GetAll();
        }

        public async Task<SendData<Admin>> UpdateAdmin(AdminResDTO admin)
        {
            var up = new Admin()
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                BirthDate = Convert.ToDateTime(admin.Date),
                UserId = admin.UserId
            };
            var res = await _adminRepo.Update(up);
            var data = new SendData<Admin>()
            {
                HasError = res == null,
                Message = res == null ? "Not saved" : "",
                Success = res != null,
                SingleData = res
            };
            return data;
        }

        public async Task<bool> DeleteAdmin(int id)
        {
            return  await _adminRepo.Delete(id);
        }
    }
}
