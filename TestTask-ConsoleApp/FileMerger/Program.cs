
using TestTask_ConsoleApp_FileMerger;

Console.WriteLine("Enter file pathes using the following sample: path1 path2 ...  pathn");
string[] filesPathes= Console.ReadLine().Split(" ");
Console.WriteLine("Enter the merge-file path: ");
string mergeFile = Console.ReadLine();
Console.WriteLine("Enter the symbols combination: ");
string deleteLine = Console.ReadLine().Trim();
FileMerger fileMerger = new FileMerger();
fileMerger.FileMerge(filesPathes, mergeFile, deleteLine);
