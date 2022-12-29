namespace DB_Importer
{
    internal class DBImporter
    {
        private Queue<string> fileLines = new Queue<string>();
        private int lineAmountCounter = 0;
        private int linesImportedCounter = 0;

        public void GetFileLines(string filepath)
        {
            string line;
            if (File.Exists(filepath))//check the existanse of all files
            {
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        line = streamReader.ReadLine();
                        fileLines.Enqueue(line);
                        lineAmountCounter++;
                    }
                }
            }
        }
        public (DateTime, string, string, int, double) GetDBRow(string queueLine)
        {
            var dbLine = queueLine.Split("||");
            return (Convert.ToDateTime(dbLine[0]), dbLine[1], dbLine[2], Convert.ToInt32(dbLine[3]), Convert.ToDouble(dbLine[4]));
        }
        public void Import(string filepath)
        {

            (DateTime, string, string, int, double) sourse;
            using (ApplicationContext db = new ApplicationContext())
            {
                GetFileLines(filepath);//get data from file in queue 
                //i use queue because its usage allows to save the lines order without any reverses
                foreach (string line in fileLines)
                {
                    sourse = GetDBRow(line);
                    DataModel data = new DataModel { Date = sourse.Item1, LatinSymbols = sourse.Item2, CyrillicSymbols = sourse.Item3, Numeric = sourse.Item4, DoubleNumeric = sourse.Item5 };
                    db.Data.Add(data);
                    db.SaveChanges();
                    linesImportedCounter++;
                    Console.WriteLine($"Imported {linesImportedCounter} lines. {lineAmountCounter - linesImportedCounter} lines left.");
                }
            }
        }
    }
}
