//C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\Test\file4.txt C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\TestTask-ConsoleApp\bin\Debug\TestFolder\file55.txt

//C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\TestTask-ConsoleApp\bin\Debug\TestFolder\mergefile248.txt
namespace TestTask_ConsoleApp_FileMerger
{
    internal class FileMerger
    {
       
      
        public void FileMerge(string[] filesPathes, string mergeFile, string deleteLine)
        {

            if (IsFileExists(filesPathes))//check the existanse of all files
            {
               
                using StreamWriter streamWriter = new StreamWriter(mergeFile);
                int skipedLinesCounter=0;
                foreach (string filePath in filesPathes)
                {
                    using StreamReader streamReader = new StreamReader(filePath);
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if ( IsSearchSymbolCombination(deleteLine, line))
                        {
                            skipedLinesCounter++;
                        }
                        else
                        {
                            streamWriter.WriteLine(line);
                        }
                    }
                }
                OutputSkipedLinesCounter(skipedLinesCounter);
            }
        }
        private void OutputSkipedLinesCounter(int skipedLinesCounter)
        {
            Console.WriteLine($"Lines skiped: {skipedLinesCounter}.");
        }
        private bool IsSearchSymbolCombination(string deleteLine, string line)
        {
            return line.Contains(deleteLine) ? true : false;
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
