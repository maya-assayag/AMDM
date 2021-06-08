using AMDM.Data;
using AMDM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AMDM.Services
{
    public class UserService
    {
        private readonly AMDMContext _context;
        public UserService(AMDMContext context)
        {
            _context = context;
        }

        public async void Signin(User account, HttpContext context)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
                , authProperties);

            string name="";
            string id="";
            if (account.Type==UserType.Trainee)
            {
                var t = _context.Trainee.FirstOrDefault(t =>
                    t.Email == account.Email);

                name = t.FirstName + ' ' + t.LastName;
                id = t.Id;
            }
            if (account.Type == UserType.Trainer)
            {
                var t = _context.Trainer.FirstOrDefault(t =>
                    t.Email == account.Email);

                name = t.FirstName + ' ' + t.LastName;
                id = t.Id;
            }

            context.Session.SetString("Name", name);
            context.Session.SetString("Id", id);
            context.Session.SetString("Type", account.Type.ToString());

        }


        public async Task<bool> Register(User user, HttpContext context)
        {
            var q = _context.User.FirstOrDefault(u => u.Email == user.Email);
            if (q == null)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();



                var u = _context.User.FirstOrDefault(u =>
                     u.Email == user.Email && u.Password == user.Password);

                if (user.Type != UserType.Trainer)
                {
                    Signin(u, context);
                }
                
                return true;

            }
            return false;
            //else
            //{
                
            //    //ViewData["Error"] = "This user already exists.";
            //}

        }
    }
}
