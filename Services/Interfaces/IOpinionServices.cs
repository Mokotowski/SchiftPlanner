using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IOpinionServices
    {
        public Task<int> NoteCompany(CompanyInfo companyInfo);
        public Task<List<Opinions>> Opinions(CompanyInfo companyInfo);
        public Task AddOpinions(int Id_Company, bool IsAnonymously, int OpinionScore, string OpinionText, UserModel userModel);
        public Task DeleteOpinions(int Id);
        public Task EditOpinions(int Id, bool IsAnonymously, int OpinionScore, string OpinionText);




    }
}
