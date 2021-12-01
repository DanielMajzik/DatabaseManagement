using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatabaseManagement
{
    public class ExcelFileHandler
    {
        public ExcelPackage Package { get; set; }
        public ExcelFileHandler()
        {
            Package = null;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public bool IsFileOpen()
        {
            return Package != null;
        }

        public void OpenWorkbook(string fileName)
        {
            try
            {
                FileInfo file = new(fileName);
                Package = new ExcelPackage(file);
                Package.LoadAsync(file);
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "File error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

            }

        }

        public List<List<string>> ReadWorksheet(string sheetName)
        {
            if (Package != null)
            {
                List<List<string>> records = new();
                ExcelWorksheet worksheet = Package.Workbook.Worksheets[sheetName];

                if (worksheet != null)
                {
                    for (int row = 1; !string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value?.ToString()); row++)
                    {
                        List<string> record = new();
                        for (int column = 1; !string.IsNullOrWhiteSpace(worksheet.Cells[row, column].Value?.ToString()); column++)
                        {
                            record.Add(worksheet.Cells[row, column].Value.ToString());
                        }
                        records.Add(record);
                    }

                    return records;
                }
            }
            MessageBox.Show($"Worksheet \"{sheetName}\" cannot be found", "Worksheet error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            return null;
        }
    }
}
