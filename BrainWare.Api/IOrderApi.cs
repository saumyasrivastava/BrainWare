using Brainware.Api.Model;
using System.Data;
using static BrainWare.Api.OrderApi;

namespace BrainWare.Api
{
    public interface IOrderApi
    {
        public ApiResult<List<Order>> GetOrdersForCompany(int CompanyId);
    }
}