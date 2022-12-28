using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_ConsoleApp
{
    internal class FileGenerator
    {
        private DateTime startDate;
            private DateTime endDate;
        private Random random = new Random();
        public void GenerateFile(string path)
        { 
            
            File.AppendAllLines(path, FileData(10));

        }
       public IEnumerable<string> FileData (int numberOfLines)
        {
            for(int i=0; i< numberOfLines; i++)
            {
                yield return GenerateDataString();
            }
        }
        public string GenerateRandomDate()
        {
            endDate = DateTime.Today;
            startDate = endDate.AddYears(-5);
            return startDate.AddDays(random.Next((endDate - startDate).Days)).ToString("dd.MM.yyyy");
        }
        public string GenerateLetterString(int length, int unicodeStart, int unicodeEnd, int gapStart, int gapEnd)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int symbolCode;
            for (int i = 0; i < length; i++)
            {
                symbolCode = random.Next(unicodeStart, unicodeEnd+1);
                if (symbolCode >= gapStart && symbolCode <= gapEnd)
                {
                    i--;
                }
                else
                {
                    stringBuilder.Append(Convert.ToChar(symbolCode).ToString());
                }
            }
            return stringBuilder.ToString();
        }
        public string GenerateDataString()
        {
            return ($"{GenerateRandomDate()} || {GenerateLetterString(10, 65, 122, 91, 96)} ||" +
                $"{GenerateLetterString(10, 1040, 1104, 0, 0)} || " +//"ё" excluded 
                $"{random.Next(1, 100000000) & ~1} || " +
                $"{random.Next(100000000, 2000000000)/100000000.0}");          
        }     
    }
}
