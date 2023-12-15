using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IOpinionServices
    {
        public Task<short> NoteCompany(CompanyInfo companyInfo);
        public Task<List<Opinions>> Opinions(CompanyInfo companyInfo);
        public Task AddOpinions(CompanyInfo companyInfo, Opinions NewOpinion, UserModel userModel);
        public Task DeleteOpinions(Opinions opinions);
        public Task EditOpinions(Opinions ActualOpinion, Opinions NewOpinion);




    }
}
