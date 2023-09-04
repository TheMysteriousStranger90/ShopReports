using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services
{
    public class CustomerReportService
    {
        private readonly ShopContext shopContext;

        public CustomerReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public CustomerSalesRevenueReport GetCustomerSalesRevenueReport()
        {
            var customerSalesRevenueLines = shopContext.Customers
                .Select(customer => new CustomerSalesRevenueReportLine
                {
                    CustomerId = customer.Id,
                    PersonFirstName = customer.Person.FirstName,
                    PersonLastName = customer.Person.LastName,
                    SalesRevenue = customer.Orders
                        .SelectMany(order => order.Details)
                        .Sum(detail => detail.PriceWithDiscount * detail.ProductAmount),
                    // You can add more properties as needed.
                })
                .ToList();

            // Create a CustomerSalesRevenueReport using the retrieved data.
            var report = new CustomerSalesRevenueReport(customerSalesRevenueLines, DateTime.Now);

            return report;
        }
    }
}
