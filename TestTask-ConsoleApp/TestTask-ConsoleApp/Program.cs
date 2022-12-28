
//folder will be placed in users directory
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
//create 100 files .txt
var fileGenerator = new FileGenerator();
for (int i=0; i<5; i++)
{
    string filePath = $@"{path}\file{i}.txt";
    fileGenerator.GenerateFile(filePath);

}
