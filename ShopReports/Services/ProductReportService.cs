using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services
{
    public class ProductReportService
    {
        private readonly ShopContext shopContext;

        public ProductReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public ProductCategoryReport GetProductCategoryReport()
        {
            /*
            var productCategoryLines = shopContext.Products
                .Include(p => p.Title.Category)
                .Select(pc => new ProductCategoryReportLine
                {
                    CategoryId = pc.Id, CategoryName = pc.Title.Category.Name
                })
                .ToList();

            // Get the current date as the report generation date
            var reportGenerationDate = DateTime.Now;

            // Create the product category report
            var productCategoryReport = new ProductCategoryReport(productCategoryLines, reportGenerationDate);

            return productCategoryReport;
            */


            var productCategoryLines = shopContext.Products
                .Include(p => p.Title.Category) // Include necessary relationships
                .GroupBy(p => new { CategoryId = p.Title.Category.Id, CategoryName = p.Title.Category.Name })
                .OrderBy(g => g.Key.CategoryName) // Order by category name
                .Select(g => new ProductCategoryReportLine
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName
                })
                .ToList();

            // Get the current date as the report generation date
            var reportGenerationDate = DateTime.Now;

            // Create the product category report
            var productCategoryReport = new ProductCategoryReport(productCategoryLines, reportGenerationDate);

            return productCategoryReport;
        }

        public ProductReport GetProductReport()
        {
            // TODO Implement the service method.
            throw new NotImplementedException();
        }

        public FullProductReport GetFullProductReport()
        {
            // TODO Implement the service method.
            throw new NotImplementedException();
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
        {
            // TODO Implement the service method.
            throw new NotImplementedException();
        }
    }
}
