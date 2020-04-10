using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHistoryInformation
{
    public class StockInfo
    {
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }

        public StockInfo(string companyName, DateTime date, double open, double high, double low, double close)
        {
            CompanyName = companyName;
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }
    }
}
