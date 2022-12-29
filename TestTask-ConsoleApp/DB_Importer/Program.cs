//in this task i will use Entity Framework and SQLite database.
//code first because there are any requrements in task about the database design (as at the task 2)
using DB_Importer;
Console.WriteLine("Enter the file path: ");
string filepath = Console.ReadLine();
DBImporter importer = new DBImporter();
importer.Import(filepath);
