//C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\TestTask-ConsoleApp\bin\Debug\TestFolder\file4.txt C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\TestTask-ConsoleApp\bin\Debug\TestFolder\file55.txt

//C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\TestTask-ConsoleApp\bin\Debug\TestFolder\mergefile248.txt
namespace TestTask_ConsoleApp_FileMerger
{
    internal class FileMerger
    {
        private int skipedLinesCounter = 0;
        private bool skipLines = true;
        public void FileMerge(string[] filesPathes, string mergeFile, string deleteLine)
        {

            if (IsFileExists(filesPathes))//chesck the existanse of all files
            {
                skipLines = deleteLine is null ? true : false; //if false any lines will be skipped and func  IsSearchSymbolCombination wont be used
                using StreamWriter streamWriter = new StreamWriter(mergeFile);
                foreach (string filePath in filesPathes)
                {
                    using StreamReader streamReader = new StreamReader(filePath);
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if (skipLines && IsSearchSymbolCombination(deleteLine, line))
                        {
                            skipedLinesCounter++;
                        }
                        else
                        {
                            streamWriter.WriteLine(line);
                        }
                    }
                }
                OutputSkipedLinesCounter();
            }
        }
        private void OutputSkipedLinesCounter()
        {
            Console.WriteLine($"Lines skiped: {skipedLinesCounter}.");
        }
        private bool IsSearchSymbolCombination(string deleteLine, string line)
        {
            return line.Contains(deleteLine, StringComparison.CurrentCulture) ? true : false;
        }
        private bool IsFileExists(string[] filesPathes)
        {
            foreach (string filePath in filesPathes)
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"{filePath} doesnt exist!");
                    return false;
                }
            }
            return true;
        }
    }
}
