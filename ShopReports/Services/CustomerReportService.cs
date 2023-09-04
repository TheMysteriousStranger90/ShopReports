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
                .Select(c => new
                {
                    c.Id,
                    c.Person.FirstName,
                    c.Person.LastName,
                    SalesRevenue = c.Orders
                        .SelectMany(o => o.Details)
                        .Sum(detail => detail.PriceWithDiscount)
                })
                .OrderByDescending(c => c.SalesRevenue)
                .Select(c => new CustomerSalesRevenueReportLine
                {
                    CustomerId = c.Id,
                    PersonFirstName = c.FirstName,
                    PersonLastName = c.LastName,
                    SalesRevenue = c.SalesRevenue
                }).Take(15)
                .ToList();

            // Get the current date as the report generation date
            var reportGenerationDate = DateTime.Now;

            // Create the customer sales revenue report
            var customerSalesRevenueReport =
                new CustomerSalesRevenueReport(customerSalesRevenueLines, reportGenerationDate);

            return customerSalesRevenueReport;
        }
    }
}
