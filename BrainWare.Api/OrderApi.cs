using Brainware.Api.Model;
using Brainware.Common;
using System.Data;
using System.Data.Common;

namespace BrainWare.Api
{
    public class OrderApi : IOrderApi
    {
        private IDatabase _database = null;

        public IDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database();
                }

                return _database;
            }
        }

        public ApiResult<List<Order>> GetOrdersForCompany(int companyId)
        {
            var result = new ApiResult<List<Order>>();

            var orders = new List<Order>();

            if (companyId == null || companyId == 0)
            {
                result.ErrorMessages.Add("Invalid companyId");
                return result;
            }

            DbDataReader ordersDbReader = null;
            try
            {
                // Get the orders
                try
                {
                    ordersDbReader = _database.ExecuteReader(ApplicationConstants.GetOrderQuery);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add($"Error while retrieving data for orders. Details: {ex.Message}");
                    return result;
                }

                try
                {
                    FillOrderInfo(orders, ordersDbReader);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add("Order - Error while retrieving Integer/decimal data for each row data for orders. More details " + ex);
                    return result;
                }

                ordersDbReader.Close();
                GetOrderProductDetails(result);
            }
            catch (Exception ex)
            {
                result.ErrorMessages.Add("Order - Error while processing. More details " + ex);
                return result;
            }
            finally
            {
                ordersDbReader?.Close();
                _database?.Dispose();
            }

            return result;
        }

        #region Helper Methods
        private static void FillOrderInfo(List<Order> orders, DbDataReader ordersDbReader)
        {
            while (ordersDbReader.Read())
            {
                var order = (IDataRecord)ordersDbReader;

                orders.Add(new Order()
                {
                    CompanyName = order.GetString(0),
                    Description = order.GetString(1),
                    OrderId = order.GetInt32(2),
                    OrderProducts = new List<OrderProduct>()
                });
            }
        }

        private void GetOrderProductDetails(ApiResult<List<Order>> result)
        {
            if (result.Data.Count == 0)
            {
                return;
            }

            var orderProducts = new List<OrderProduct>();
            DbDataReader orderProductReader = null;
            try
            {
                try
                {
                    orderProductReader = _database.ExecuteReader(ApplicationConstants.GetOrderProductQuery);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add("OrderProduct - Error while retrieving data for order products. More details " + ex);
                    return;
                }

                try
                {
                    FillOrderProucts(orderProducts, orderProductReader);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add("OrderProduct - Error while retrieving Integer/decimal data for each row data for order products. More details " + ex);
                    return;
                }

                orderProductReader.Close();
                UpdateProductInfoInOrders(result, orderProducts);
            }
            finally
            {
                orderProductReader?.Close();
            }
        }

        private static void FillOrderProucts(List<OrderProduct> orderProducts, DbDataReader orderProductReader)
        {
            while (orderProductReader.Read())
            {
                var record2 = (IDataRecord)orderProductReader;

                orderProducts.Add(new OrderProduct()
                {
                    OrderId = record2.GetInt32(1),
                    ProductId = record2.GetInt32(2),
                    Price = record2.GetDecimal(0),
                    Quantity = record2.GetInt32(3),
                    Product = new Product()
                    {
                        Name = record2.GetString(4),
                        Price = record2.GetDecimal(5)
                    }
                });
            }
        }

        private static void UpdateProductInfoInOrders(ApiResult<List<Order>> result, List<OrderProduct> orderProducts)
        {
            foreach (var order in result.Data)
            {
                foreach (var orderproduct in orderProducts)
                {
                    if (orderproduct.OrderId != order.OrderId)
                        continue;

                    order.OrderProducts.Add(orderproduct);
                    order.OrderTotal = order.OrderTotal + (orderproduct.Price * orderproduct.Quantity);
                }
            }
        } 
        #endregion
    }
}