using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace DB_Importer
{
    internal class DBImporter
    {
        private Queue<string> fileLines = new Queue<string>();
        (DateTime, string, string, int, double) sourse;
        private int lineAmountCounter = 0;
        private int linesImportedCounter = 0;

        public void GetFileLines(string filepath)
        {
            string line;           
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        line = streamReader.ReadLine();
                        fileLines.Enqueue(line);//add file lines in queue
                        lineAmountCounter++;//count the file lines amount
                    }
                }         
        }
        public (DateTime, string, string, int, double) GetDBRow(string queueLine)
        {
            var dbLine = queueLine.Split("||");
            return (Convert.ToDateTime(dbLine[0]), dbLine[1], dbLine[2], Convert.ToInt32(dbLine[3]), Convert.ToDouble(dbLine[4]));//the tuple wiht splitted file line data
        }
        public void Import(string filepath)
        {
            if (File.Exists(filepath))//check the existanse of all files
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                string connectionString = config.GetConnectionString("DefaultConnection");
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                var options = optionsBuilder.UseSqlServer(connectionString).Options;
                using (ApplicationContext db = new ApplicationContext(options))
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
                        Console.WriteLine($"Imported {linesImportedCounter}/{lineAmountCounter} lines.");
                    }
                    Console.WriteLine($"The numeric amount: {UseStoredProcedure("@numericTotal", System.Data.SqlDbType.BigInt, System.Data.ParameterDirection.Output, options, "CountNumericTotal @numericTotal OUT")}\n" +
                       $"The median of double numerics: {UseStoredProcedure("@median", System.Data.SqlDbType.Float, System.Data.ParameterDirection.Output, options, "GetMedian @median OUT")}");
                }
            }
            else Console.WriteLine($"File {filepath} doesnt exist!");
        }
        public Object UseStoredProcedure(string paramName, System.Data.SqlDbType type, System.Data.ParameterDirection direction, DbContextOptions<ApplicationContext> options, string storedProcedureString )//construction could be reused for both stored procedures
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
            SqlParameter param = new()
                {
                    ParameterName = paramName,
                    SqlDbType = type,
                    Direction = direction,
                };
                db.Database.ExecuteSqlRaw($"{storedProcedureString}", param);
               return param.Value;
            }
        }
    }
}
