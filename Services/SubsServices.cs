using SchiftPlanner.Models;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using SchiftPlanner.Controllers;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SchiftPlanner.Services
{
    public class SubsServices : ISubsServices
    {
        private readonly DatabaseContext _context;

        public SubsServices(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Subscriptions>> GetMySubs(UserModel user)
        {
            List<Subscriptions> subs = _context.Subscriptions
                .Where(s => s.Id_User == user.Id)
                .ToList();

            return subs;
        }

        public async Task<Subscriptions> AddNewSubscription(UserModel user, Type_Subscriptions type_Sub)
        {
            var subscription = new Subscriptions
            {
                Id_User = user.Id,
                Id_Sub = type_Sub.Id_Sub,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(type_Sub.DateLenght),
                AutoRenew = true,

                User = user,
                Type_Subscriptions = type_Sub
            };

            var company = new CompanyInfo
            {
                CompanyName = "Your Commpany name",
                Description = "There is your Description",
                LogoUrl = "Url to your Logo",

                Subscriptions = subscription
            };


            if(type_Sub.TypeCompany == 1)
            {
                var companyType = new Company_Type1
                {
                    Id_Company = company.Id_Company,
                    Id_user = user.Id,

                    Company_Info = company
                };

                _context.Subscriptions.Add(subscription);
                _context.CompanyInfo.Add(company);
                _context.Company_Type1.Add(companyType);
            }
            else if (type_Sub.TypeCompany == 2)
            {
                var companyType = new Company_Type2
                {
                    Id_Company = company.Id_Company,
                    Id_user = user.Id,

                    Company_Info = company
                };

                _context.Subscriptions.Add(subscription);
                _context.CompanyInfo.Add(company);
                _context.Company_Type2.Add(companyType);
            }

            _context.SaveChanges();




            return subscription;
        }


        public async Task<Subscriptions> RenewSubscription(UserModel user, string Id_Company)
        {
            Subscriptions MySub = _context.Subscriptions.Find(Id_Company);
            Type_Subscriptions type_Subscriptions = _context.Type_Subscriptions.Find(MySub.Id_Sub);
            MySub.EndDate.AddDays(type_Subscriptions.DateLenght);

            _context.SaveChanges();

            return MySub;
        }


        public async Task<Subscriptions> RenewAnotherSubscription(string Id_Company, Type_Subscriptions type_Subscriptions)
        {
            Subscriptions MySub = _context.Subscriptions.Find(Id_Company);
            MySub.Id_Sub = type_Subscriptions.Id_Sub;
            MySub.EndDate.AddDays(type_Subscriptions.DateLenght);

            _context.SaveChanges();

            return MySub;
        }

        public async Task<Subscriptions> ChangeAutoRenew(string Id_Company)
        {
            Subscriptions MySub = _context.Subscriptions.Find(Id_Company);

            if(MySub.AutoRenew)
            {
                MySub.AutoRenew = false;
            }
            else
            {
                MySub.AutoRenew = true;
            }
            
            _context.SaveChanges();

            return MySub;
        }





    }
}
