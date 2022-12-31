
//C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\TestTask-ConsoleApp\bin\Debug
using System.Text;
using TestTask_ConsoleApp;
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Enter the directory path: ");
string path =Console.ReadLine();
DirectoryInfo dirInfo = new DirectoryInfo(path);
if (!dirInfo.Exists)
{
    dirInfo.Create();
}
//create filesAmount files .txt
var fileGenerator = new FileGenerator();
Console.WriteLine("Enter the files amount and its size (lines): ");
int filesAmount, linesAmount;
if(Int32.TryParse(Console.ReadLine(), out filesAmount)&& (Int32.TryParse(Console.ReadLine(), out linesAmount)))
{
for (int i=0; i< filesAmount; i++)
{
    string filePath = $@"{path}\file{i}.txt";
    fileGenerator.GenerateFile(filePath, linesAmount);

}
}
else
{
    Console.WriteLine("Incorrect input!");
}

