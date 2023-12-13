using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Services.Interfaces;
using System.Collections.Generic;
using System.Globalization;

namespace SchiftPlanner.Services
{
    public class OpinionServices : IOpinionServices
    {
        private readonly DatabaseContext _context;
        public OpinionServices(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<short> NoteCompany(CompanyInfo companyInfo)
        {

            List<short> opinions = _context.Opinions.Where(c => c.Id_Company == companyInfo.Id_Company).Select(o => o.Score).ToList();

            long sum = 0;
            foreach(var opinion in opinions)
            {
                sum += opinion;
            }
            short average = (short)(sum / opinions.Count);


            return average;
        }





        public async Task<List<Opinions>> Opinions(CompanyInfo companyInfo)
        {
            List<Opinions> opinions = _context.Opinions.Where(c => c.Id_Company == companyInfo.Id_Company).ToList();
            return opinions;
        }


        public async Task AddOpinions(CompanyInfo companyInfo, Opinions NewOpinion, UserModel userModel)
        {
            NewOpinion.Id_user = userModel.Id;
            NewOpinion.Id_Company = companyInfo.Id_Company;

            _context.Opinions.Add(NewOpinion);
            _context.SaveChanges();

        }



        public async Task DeleteOpinions(Opinions opinion)
        {

            Opinions opinionTodelete = _context.Opinions.Where(c => c.Id == opinion.Id).FirstOrDefault();
       
            _context.Opinions.RemoveRange(opinionTodelete);
            _context.SaveChanges();

        }
        public async Task EditOpinions(Opinions ActualOpinion, Opinions NewOpinion)
        {

            //sprawdzić czy działa
            ActualOpinion = ActualOpinion / NewOpinion;

            _context.SaveChanges();
        }



    }
}
