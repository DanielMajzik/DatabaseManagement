using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public class ReadExcelIntoModel
    {
        public ExcelFileHandler FileHandler { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<ProductModel> Products { get; set; }
        public List<WebshopModel> Webshops { get; set; }

        public ReadExcelIntoModel(string excelFilePath)
        {
            try
            {
                FileHandler = new();
                FileHandler.OpenWorkbook(excelFilePath);
            }
            catch (Exception)
            {
                FileHandler = null;
            }
        }

        public List<CategoryModel> ReadCategories()
        {
            List<List<string>> worksheetData = FileHandler.ReadWorksheet("Category");
            Categories = new();

            if (worksheetData != null)
            {
                foreach (var line in worksheetData)
                {
                    Categories.Add(new CategoryModel(line[0]));
                }
            }
            return Categories;
        }

        public List<ProductModel> ReadProducts()
        {
            List<List<string>> worksheetData = FileHandler.ReadWorksheet("Product");
            Products = new();

            if (worksheetData != null)
            {
                foreach (var line in worksheetData)
                {
                    Products.Add(new ProductModel(line[0], int.Parse(line[1])));
                }
            }

            return Products;
        }

        public List<WebshopModel> ReadWebshops()
        {
            List<List<string>> worksheetData = FileHandler.ReadWorksheet("Webshop");
            Webshops = new();

            if (worksheetData != null)
            {
                foreach (var line in worksheetData)
                {
                    Webshops.Add(new WebshopModel(line[0], line[1]));
                }
            }

            return Webshops;
        }

    }
}
