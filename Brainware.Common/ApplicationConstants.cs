using System;

namespace Brainware.Common
{
    public static class ApplicationConstants
    {
        public static string SqlMdf = @"C:\Users\Lenovo\Downloads\BrainWare-master\BrainWare-master\Web\App_Data\BrainWare.mdf";
        public static string SqlConnectionString = $"Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BrainWAre;Integrated Security=SSPI;AttachDBFilename={SqlMdf}";

        //Sql queries
        //Order Queries
        public static string GetOrderQuery = "SELECT c.name, o.description, o.order_id FROM company c INNER JOIN [order] o on c.company_id=o.company_id";
        public static string GetOrderProductQuery = "SELECT op.price, op.order_id, op.product_id, op.quantity, p.name, p.price FROM orderproduct op INNER JOIN product p on op.product_id=p.product_id";
    }
}
