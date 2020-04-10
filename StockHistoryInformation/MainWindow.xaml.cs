using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace StockHistoryInformation
{
    /*Question 1

    Implement a WPF app to load stock information from StockData.csv, 
    allow user to Search specific company’s stock history information, 
    and display the search result in a DataGrid, 
    and the result should be sorted according to the date. 
    Clean the data by removing those rows that the price contains negative value 
    while the data is loading. Add Progress bar to the status bar 
    to indicate the progress of file loading. 

    Your app should also include a synchronous method to calculate factorial number.  

     */
    public partial class MainWindow : Window
    {
        List<StockInfo> StockInfos = new List<StockInfo>();
        string path = "C:\\Software Engineering\\2020 Winter\\Programming3\\Assignment\\Lab5\\Lab05\\";
        string filename = "stockData";
        System.Windows.Data.CollectionViewSource stockInfoViewSource;

        public MainWindow()
        {
            InitializeComponent();
            ProgressBarControl("hidden");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stockInfoViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stockInfoViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            
            ReadCSV("stockData");

            stockInfoViewSource.Source = StockInfos.OrderBy(s => s.Date);
        }

        public void ReadCSV(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(path + filename + ".csv");

                foreach (string line in lines.Skip(1))
                {
                    var regex = new Regex("\\\"(.*?)\\\"");
                    var output = regex.Replace(line, m => m.Value.Replace(',', '_'));
                    string[] data = output.Split(',');

                    double open, high, low, close;

                    Double.TryParse(Regex.Replace(data[2], "[^\\d.-]", ""), out open);
                    Double.TryParse(Regex.Replace(data[3], "[^\\d.-]", ""), out high);
                    Double.TryParse(Regex.Replace(data[4], "[^\\d.-]", ""), out low);
                    Double.TryParse(Regex.Replace(data[5], "[^\\d.-]", ""), out close);

                    if (open < 0 || high < 0 || low < 0 || close < 0)
                        continue;
                    else
                        StockInfos.Add(new StockInfo(data[0], DateTime.Parse(data[1]), open, high, low, close));
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            LabelAsyncResult.Content = "Reading...";

            string key = SearchKeyTextBox.Text;
            Task<IEnumerable<StockInfo>> searchTask = Task.Run(() => Search(key));
            await searchTask;
            stockInfoViewSource.Source = searchTask.Result;

            LabelAsyncResult.Content = $"{searchTask.Result.Count()} results are found.";
        }

        private IEnumerable<StockInfo> Search(string key)
        {
            List<StockInfo> result = new List<StockInfo>();
            int total = StockInfos.Count();
            this.Dispatcher.Invoke(() =>
            {
                ProgressBarControl("visible");
                Progress.Value = 0;
            });

            for (int i = 0; i < total; i++)
            {
                Thread.Sleep(10);
                if (StockInfos.ElementAt(i).CompanyName.Contains(key))
                {
                    result.Add(StockInfos.ElementAt(i));
                    this.Dispatcher.Invoke(() =>
                    {
                        Progress.Value = (i * 100 / total);
                    });
                    
                }
            }

            this.Dispatcher.Invoke(() =>
            {
                ProgressBarControl("hidden");
            });
 
            return result;
        }

        private void ProgressBarControl(string status)
        {
            if (status == "visible")
            {
                Progress.Visibility = Visibility.Visible;
                ProgressText.Visibility = Visibility.Visible;
            }
            
            else if (status == "hidden")
            {
                Progress.Visibility = Visibility.Hidden;
                ProgressText.Visibility = Visibility.Hidden;
            }
        }

        private async void ButtonFactorial_Click(object sender, RoutedEventArgs e)
        {
            TextBlockFactorialResult.Text = "Calculating...";
            int num = 0;
            if(Int32.TryParse(TextBoxInput.Text, out num)){
                Task<int> factorialTask = Task.Run(() => Factorial(num));
                await factorialTask;
                TextBlockFactorialResult.Text = factorialTask.Result.ToString();
            }

        }
        private int Factorial(int num)
        {
            Thread.Sleep(1000);
            if (num <= 1)
                return num;
            return num * Factorial(num - 1);
        }
    }
}
