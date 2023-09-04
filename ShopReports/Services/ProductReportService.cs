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
            var productCategoryLines = shopContext.Products
                .Include(p => p.Title.Category) // Include necessary relationships
                .GroupBy(p => new { CategoryId = p.Title.Category.Id, CategoryName = p.Title.Category.Name })
                .OrderBy(g => g.Key.CategoryName) // Order by category name
                .Select(g => new ProductCategoryReportLine
                {
                    CategoryId = g.Key.CategoryId, CategoryName = g.Key.CategoryName
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
            var productReportLines = shopContext.Products
                .Include(p => p.Title)
                .Include(p => p.Manufacturer)
                .OrderByDescending(p => p.Title.Title) // Sort by product title
                .Select(p => new ProductReportLine
                {
                    ProductId = p.Id,
                    ProductTitle = p.Title.Title,
                    Manufacturer = p.Manufacturer.Name,
                    Price = p.UnitPrice
                })
                .ToList();

            var reportGenerationDate = DateTime.Now;
            var productReport = new ProductReport(productReportLines, reportGenerationDate);

            return productReport;
        }

        public FullProductReport GetFullProductReport()
        {
            var fullProductReportLines = shopContext.Products
                .Include(p => p.Title)
                .Include(p => p.Manufacturer)
                .OrderBy(p => p.Title.Title)
                .Select(p => new FullProductReportLine
                {
                    ProductId = p.Id,
                    Name = p.Title.Title,
                    CategoryId = p.Title.Category.Id,
                    Category = p.Title.Category.Name,
                    Manufacturer = p.Manufacturer.Name,
                    Price = p.UnitPrice
                })
                .ToList();

            var reportGenerationDate = DateTime.Now;
            var fullProductReport = new FullProductReport(fullProductReportLines, reportGenerationDate);

            return fullProductReport;
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
        {
            var productTitleSalesRevenueLines = shopContext.OrderDetails
                .Include(od => od.Product.Title)
                .GroupBy(od => od.Product.Title.Title)
                .Select(g => new ProductTitleSalesRevenueReportLine
                {
                    ProductTitleName = g.Key,
                    SalesRevenue = g.Sum(od => od.PriceWithDiscount),
                    SalesAmount = g.Sum(od => od.ProductAmount)
                })
                .OrderByDescending(line => line.SalesRevenue)
                .ToList();

            var reportGenerationDate = DateTime.Now;
            var productTitleSalesRevenueReport =
                new ProductTitleSalesRevenueReport(productTitleSalesRevenueLines, reportGenerationDate);

            return productTitleSalesRevenueReport;
        }
    }
}
