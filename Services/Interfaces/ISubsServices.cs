using SchiftPlanner.Models;
using SchiftPlanner.Models.Subs;

namespace SchiftPlanner.Services.Interfaces
{
    public interface ISubsServices
    {
        public Task<List<Subscriptions>> GetMySubs(UserModel user);


        public Task<Subscriptions> AddNewSubscription(UserModel user, Type_Subscriptions type_Sub);
        public Task<Subscriptions> RenewSubscription(UserModel user, string Id_Company);
        public Task<Subscriptions> RenewAnotherSubscription(string Id_Company, Type_Subscriptions type_Subscriptions);
        public Task<Subscriptions> ChangeAutoRenew(string Id_Company);






    }
}
