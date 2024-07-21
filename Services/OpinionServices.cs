using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<int> NoteCompany(CompanyInfo companyInfo)
        {

            List<Opinions> opinions = _context.Opinions.Where(c => c.Id_Company == companyInfo.Id_Company).ToList();


            try
            {
                int sum = 0;
                foreach (var opinion in opinions)
                {
                    sum += opinion.Score;
                }
                int average = (sum / opinions.Count);

                return average;

            }
            catch (Exception DivideByZeroException)
            {
                return -1;
            }

        }

        public async Task<List<Opinions>> Opinions(CompanyInfo companyInfo)
        {
            List<Opinions> opinions = _context.Opinions.Where(c => c.Id_Company == companyInfo.Id_Company).ToList();
            return opinions;
        }






        public async Task AddOpinions(int Id_Company, bool IsAnonymously, int OpinionScore, string OpinionText, UserModel userModel)
        {
            if(_context.Opinions.Where(c => c.Id_Company == Id_Company && c.Id_user == userModel.Id).Count() == 0) 
            {
                Opinions NewOpinion = new Opinions();

                NewOpinion.Id_Company = Id_Company;
                NewOpinion.Anonymously = IsAnonymously;
                NewOpinion.Id_user = userModel.Id;
                NewOpinion.Score = OpinionScore;
                NewOpinion.Text = OpinionText;
                NewOpinion.DateAdd = DateTime.Now;

               _context.Opinions.Add(NewOpinion);
               _context.SaveChanges();

            }
        }


        public async Task DeleteOpinions(int Id)
        {

            Opinions opinionTodelete = _context.Opinions.Find(Id);
       
            _context.Opinions.RemoveRange(opinionTodelete);
            _context.SaveChanges();

        }
        public async Task EditOpinions(int Id, bool IsAnonymously, int OpinionScore, string OpinionText)
        {
            Opinions opinion = _context.Opinions.Find(Id);

            opinion.Text = OpinionText;
            opinion.Anonymously = IsAnonymously;
            opinion.Score = OpinionScore;

            _context.SaveChanges();
        }



    }
}
