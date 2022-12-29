using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Importer
{
    internal class DataModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string LatinSymbols { get; set; }
        public string CyrillicSymbols { get; set; } 
        public int Numeric { get; set; }
        public double DoubleNumeric { get; set; }
    }
}
