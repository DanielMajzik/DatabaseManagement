using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace DatabaseManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SqlConnectionManager ConnectionManager = new();
        private ReadExcelIntoModel ExcelIntoModel = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SourceFileNameBox.Text = openFileDialog.FileName;
                ExcelIntoModel = new(SourceFileNameBox.Text);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectionManager.IsConnected())
            {
                ConnectionManager.CloseConnection();
            }
            Close();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExcelIntoModel != null)
            {
                if (ConnectionManager.IsConnected())
                {
                    if ((bool)WebshopButton.IsChecked)
                    {
                        string[] webshopFields = { "NAME", "URL" };
                        string[] webshopTypes = { "nvarchar(255)", "nvarchar(255)" };
                        string[] webshopValues = new string[2];
                        ConnectionManager.CreateTable("WEBSHOP", webshopFields, webshopTypes);
                        var webshops = ExcelIntoModel.ReadWebshops();

                        if (webshops.Count == 0)
                        {
                            MessageBoxResult result = MessageBox.Show(
                                    "Empty list of data. Overwrite existing data?",
                                    "Warning",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning,
                                    MessageBoxResult.No);
                            if (result == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        foreach (var webshop in webshops)
                        {
                            webshopValues[0] = webshop.Name;
                            webshopValues[1] = webshop.URL;
                            ConnectionManager.InsertRecord("WEBSHOP", webshopFields, webshopValues, webshopTypes);
                        }
                        MessageBox.Show(
                                    "Webshop data updated",
                                    "Success",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information,
                                    MessageBoxResult.OK);
                    }
                    else
                    {
                        if ((bool)CategoryButton.IsChecked)
                        {
                            string[] categoryFields = { "NAME" };
                            string[] categoryTypes = { "nvarchar(255)" };
                            string[] categoryValues = new string[1];
                            ConnectionManager.CreateTable("CATEGORY", categoryFields, categoryTypes);
                            var categories = ExcelIntoModel.ReadCategories();

                            if (categories.Count == 0)
                            {
                                MessageBoxResult result = MessageBox.Show(
                                        "Empty list of data. Overwrite existing data?",
                                        "Warning",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning,
                                        MessageBoxResult.No);
                                if (result == MessageBoxResult.No)
                                {
                                    return;
                                }
                            }

                            foreach (var category in categories)
                            {
                                categoryValues[0] = category.Name;
                                ConnectionManager.InsertRecord("CATEGORY", categoryFields, categoryValues, categoryTypes);
                            }

                            MessageBox.Show(
                                    "Category data updated",
                                    "Success",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information,
                                    MessageBoxResult.OK);
                        }
                        else
                        {
                            if ((bool)ProductButton.IsChecked)
                            {
                                string[] productFields = { "NAME", "CATEGORY_ID" };
                                string[] productTypes = { "nvarchar(255)", "int" };
                                string[] productValues = new string[2];
                                ConnectionManager.CreateTable("PRODUCT", productFields, productTypes);
                                var products = ExcelIntoModel.ReadProducts();

                                if (products.Count == 0)
                                {
                                    MessageBoxResult result = MessageBox.Show(
                                            "Empty list of data. Overwrite existing data?",
                                            "Warning",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning,
                                            MessageBoxResult.No);
                                    if (result == MessageBoxResult.No)
                                    {
                                        return;
                                    }
                                }

                                foreach (var product in products)
                                {
                                    productValues[0] = product.Name;
                                    productValues[1] = product.CategoryID.ToString();
                                    ConnectionManager.InsertRecord("PRODUCT", productFields, productValues, productTypes);
                                }

                                MessageBox.Show(
                                    "Product data updated",
                                    "Success",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information,
                                    MessageBoxResult.OK);
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Please, select the destionation table.",
                                    "Attention",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.OK);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                                "Please, connect to the database.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error,
                                MessageBoxResult.OK);
                }
            }
            else
            {
                MessageBox.Show(
                                "Please, select the spreadsheet.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error,
                                MessageBoxResult.OK);
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectButton.Content.Equals("Connect") && ConnectionManager.InitiateConnection("EGRC01066\\SQLEXPRESS", "SHOPSANDPRODUCTS", "CSAUTH", "AuthMe12."))
            {
                ConnectButton.Content = "Disconnect";
                return;
            }
            else
            {
                if (ConnectButton.Content.Equals("Disconnect") && ConnectionManager.CloseConnection())
                {
                    ConnectButton.Content = "Connect";
                }
            }
        }
    }
}