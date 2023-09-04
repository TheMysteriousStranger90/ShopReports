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
            var productCategoryLines = shopContext.Categories
                .Select(category => new ProductCategoryReportLine
                {
                    CategoryId = category.Id, CategoryName = category.Name,
                    // You can add more properties as needed.
                })
                .ToList();

            // Create a ProductCategoryReport using the retrieved data.
            var report = new ProductCategoryReport(productCategoryLines, DateTime.Now);

            return report;
        }

        public ProductReport GetProductReport()
        {
            var productLines = shopContext.Products
                .Select(product => new ProductReportLine
                {
                    ProductId = product.Id,
                    ProductTitle = product.Title.Title,
                    Manufacturer = product.Manufacturer.Name,
                })
                .ToList();

            var report = new ProductReport(productLines, DateTime.Now);

            return report;
        }

        public FullProductReport GetFullProductReport()
        {
            var fullProductLines = shopContext.Products
                .Join(
                    shopContext.Categories,
                    product => product.Id,
                    category => category.Id,
                    (product, category) => new FullProductReportLine
                    {
                        ProductId = product.Id,
                        Name = product.Title.Title,
                        CategoryId = category.Id,
                        Category = category.Name,
                        Manufacturer = product.Manufacturer.Name,
                    })
                .ToList();

            var report = new FullProductReport(fullProductLines, DateTime.Now);

            return report;
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
        {
            // TODO Implement the service method.
            throw new NotImplementedException();
        }
    }
}
