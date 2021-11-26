using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly SqlConnectionManager ConnectionManager = new();
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

                ExcelFileHandler excelFileHandler = new();
                excelFileHandler.OpenWorkbook(SourceFileNameBox.Text);

                if (excelFileHandler.IsFileOpen())
                {
                    var worksheetData = excelFileHandler.ReadWorksheet("Product");

                    string listToWrite = "";

                    if (worksheetData != null)
                    {
                        foreach (var list in worksheetData)
                        {
                            foreach (var element in list)
                            {
                                listToWrite += element + '\t';
                            }
                            listToWrite += '\n';
                        }

                        MessageBox.Show(listToWrite, "Data Read from excel", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    }
                }
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
            if (!File.Exists(SourceFileNameBox.Text))
            {
                // TODO
                SourceFileNameBox.Text = "Error 404. You are an IDIOT! No such file exists.";
                return;
            }
            else
            {
                if ((bool)WebshopButton.IsChecked)
                {
                    // TODO
                    SourceFileNameBox.Text = "Webshop data updated";
                }
                else
                {
                    if ((bool)CategoryButton.IsChecked)
                    {
                        // TODO
                        SourceFileNameBox.Text = "Category data updated";
                    }
                    else
                    {
                        if ((bool)ProductButton.IsChecked)
                        {
                            // TODO
                            SourceFileNameBox.Text = "Item data updated";
                        }
                        else
                        {
                            // TODO
                            SourceFileNameBox.Text = "Please, select the destionation database.";
                        }
                    }
                }
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