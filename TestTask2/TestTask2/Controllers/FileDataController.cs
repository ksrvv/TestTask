using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace TestTask2.Controllers
{
    public class FileDataController : Controller
    {
        public IActionResult FileData(string altitude)
        {
            TempData["msg"] = $"File: {altitude}.";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string filename = @"C:\Users\апро\Desktop\TestTask\TestTask2\TestTask2\Upload\" + altitude;
            //using StreamWriter streamWriter = new StreamWriter(@"C:\Users\апро\Desktop\TestTask\TestTask-ConsoleApp\Test\file0.txt");
            //streamWriter.WriteLine(filename);
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\апро\\BalanceAccounts.mdf;Integrated Security=True";
            string sqlGetClassBalanceAccounts = $"SELECT CLASS, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES FROM ClassBalanceAccount WHERE [FILE]= N'{filename}'";
            string sqlGetQuarternaryBalanceAccounts = $"SELECT CLASS, QUARTERNARY_ACCOUNT_NUMBER, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES FROM QuarternaryBalanceAccount WHERE [FILE]= N'{filename}'";
            string sqlGetBinaryBalanceAccounts = $"SELECT BINARY_ACCOUNT_NUMBER, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES FROM BinaryBalanceAccount WHERE [FILE]= N'{filename}'";                 
            return View(GetDataTable( connectionString, sqlGetClassBalanceAccounts,  sqlGetQuarternaryBalanceAccounts,  sqlGetBinaryBalanceAccounts));
        }
        private DataTable GetDataTable(string connectionString, string sqlGetClassBalanceAccounts, string sqlGetQuarternaryBalanceAccounts, string sqlGetBinaryBalanceAccounts)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapterClassBA = new SqlDataAdapter(sqlGetClassBalanceAccounts, connection);
            SqlDataAdapter adapterQuarternaryBA = new SqlDataAdapter(sqlGetQuarternaryBalanceAccounts, connection);
            SqlDataAdapter adapterBinaryBA = new SqlDataAdapter(sqlGetBinaryBalanceAccounts, connection);
          
            DataSet dsClassBA = new DataSet();
            DataSet dsQuarternaryBA = new DataSet();
            DataSet dsBinaryBA = new DataSet();
            DataSet union = new DataSet();

            DataTable unionDT = new DataTable("DalanceAccounts");
            union.Tables.Add(unionDT);
            DataColumn Class = new DataColumn("CLASS", Type.GetType("System.String"));
            DataColumn AccountNumber = new DataColumn("AccountNumber", Type.GetType("System.Int32"));
            DataColumn openingBalanceAssets = new DataColumn("OPENING_BALANCE_ASSETS", Type.GetType("System.Double"));
            DataColumn openingBalanceLiabilities = new DataColumn("OPENING_BALANCE_LIABILITIES", Type.GetType("System.Double"));
            DataColumn turnoverDebit = new DataColumn("TURNOVER_DEBIT", Type.GetType("System.Double"));
            DataColumn turnoverCredit = new DataColumn("TURNOVER_CREDIT", Type.GetType("System.Double"));
            DataColumn closingBalanceAssets = new DataColumn("CLOSING_BALANCE_ASSETS", Type.GetType("System.Double"));
            DataColumn closingBalanceLiabilities = new DataColumn("CLOSING_BALANCE_LIABILITIES", Type.GetType("System.Double"));
            unionDT.Columns.Add(Class);
            unionDT.Columns.Add(AccountNumber);
            unionDT.Columns.Add(openingBalanceAssets);
            unionDT.Columns.Add(openingBalanceLiabilities);
            unionDT.Columns.Add(turnoverDebit);
            unionDT.Columns.Add(turnoverCredit);
            unionDT.Columns.Add(closingBalanceAssets);
            unionDT.Columns.Add(closingBalanceLiabilities);

            adapterClassBA.Fill(dsClassBA);
            adapterQuarternaryBA.Fill(dsQuarternaryBA);
            adapterBinaryBA.Fill(dsBinaryBA);

            CultureInfo myCultureInfo = new CultureInfo("ru-RU");

            DataTable dtQuarternaryBA = dsQuarternaryBA.Tables[0];//work with this table
            dtQuarternaryBA.CaseSensitive = true;
            dtQuarternaryBA.Locale = myCultureInfo;
            DataTable dtClassBA = dsClassBA.Tables[0];
            dtClassBA.CaseSensitive = true;
            dtQuarternaryBA.Locale = myCultureInfo;
            DataTable dtBinaryBA = dsBinaryBA.Tables[0];
            dtBinaryBA.CaseSensitive = true;
            dtQuarternaryBA.Locale = myCultureInfo;

            int currentQuarternaryA = Convert.ToInt32(dtQuarternaryBA.Rows[0][1].ToString().Remove(2));
            string currentClass = dtQuarternaryBA.Rows[0][0].ToString();
            for (int i = 0; i <= dtQuarternaryBA.Rows.Count - 1; i++)
            {

                DataRow importRow = dtQuarternaryBA.Rows[i];
                if (dtQuarternaryBA.Rows[i][0].ToString() == currentClass && Convert.ToInt32(dtQuarternaryBA.Rows[i][1].ToString().Remove(2)) == currentQuarternaryA)
                {
                    unionDT.Rows.Add(GetDataRow(unionDT, importRow[0], importRow[1], importRow[2], importRow[3], importRow[4], importRow[5], importRow[6], importRow[7]));
                }
                else if (dtQuarternaryBA.Rows[i][0].ToString() == currentClass && Convert.ToInt32(dtQuarternaryBA.Rows[i][1].ToString().Remove(2)) != currentQuarternaryA)
                {

                    DataRow importRowBinaryAccount = dtBinaryBA.Select($"BINARY_ACCOUNT_NUMBER ={currentQuarternaryA}")[0];
                    unionDT.Rows.Add(GetDataRow(unionDT, null , importRowBinaryAccount[0], importRowBinaryAccount[1], importRowBinaryAccount[2], importRowBinaryAccount[3], importRowBinaryAccount[4], importRowBinaryAccount[5], importRowBinaryAccount[6]));
                    unionDT.Rows.Add(GetDataRow(unionDT, importRow[0], importRow[1], importRow[2], importRow[3], importRow[4], importRow[5], importRow[6], importRow[7]));
                    currentQuarternaryA = Convert.ToInt32(dtQuarternaryBA.Rows[i][1].ToString().Remove(2));

                }
                else if (dtQuarternaryBA.Rows[i][0].ToString() != currentClass && Convert.ToInt32(dtQuarternaryBA.Rows[i][1].ToString().Remove(2)) == currentQuarternaryA)
                {
                    DataRow importRowClassAccount = dtClassBA.Select($"CLASS ='{currentClass}'")[0];                  
                    unionDT.Rows.Add(GetDataRow(unionDT, importRowClassAccount[0], currentQuarternaryA, importRowClassAccount[1], importRowClassAccount[2], importRowClassAccount[3], importRowClassAccount[4], importRowClassAccount[5], importRowClassAccount[6]));
                    unionDT.Rows.Add(GetDataRow(unionDT, importRow[0], importRow[1], importRow[2], importRow[3], importRow[4], importRow[5], importRow[6], importRow[7]));
                    currentClass = dtQuarternaryBA.Rows[i][0].ToString();
                }
                else
                {
                    DataRow importRowBinaryAccount = dtBinaryBA.Select($"BINARY_ACCOUNT_NUMBER ={currentQuarternaryA}")[0];                 
                    unionDT.Rows.Add(GetDataRow(unionDT, null, importRowBinaryAccount[0], importRowBinaryAccount[1], importRowBinaryAccount[2], importRowBinaryAccount[3], importRowBinaryAccount[4], importRowBinaryAccount[5], importRowBinaryAccount[6]));
                    DataRow importRowClassAccount = dtClassBA.Select($"CLASS ='{currentClass}'")[0];
                    unionDT.Rows.Add(GetDataRow(unionDT, "ПО КЛАССУ", null, importRowClassAccount[1], importRowClassAccount[2], importRowClassAccount[3], importRowClassAccount[4], importRowClassAccount[5], importRowClassAccount[6]));
                    unionDT.Rows.Add(GetDataRow(unionDT, importRow[0], importRow[1], importRow[2], importRow[3], importRow[4], importRow[5], importRow[6], importRow[7]));
                    currentClass = dtQuarternaryBA.Rows[i][0].ToString();
                    currentQuarternaryA = Convert.ToInt32(dtQuarternaryBA.Rows[i][1].ToString().Remove(2));
                }
                if(i== dtQuarternaryBA.Rows.Count - 1)
                {
  DataRow importRowBinaryAccountLast = dtBinaryBA.Select($"BINARY_ACCOUNT_NUMBER ={currentQuarternaryA}")[0];
                unionDT.Rows.Add(GetDataRow(unionDT, null, importRowBinaryAccountLast[0], importRowBinaryAccountLast[1], importRowBinaryAccountLast[2], importRowBinaryAccountLast[3], importRowBinaryAccountLast[4], importRowBinaryAccountLast[5], importRowBinaryAccountLast[6]));
                    DataRow importRowClassAccount = dtClassBA.Select($"CLASS ='{currentClass}'")[0];
                    unionDT.Rows.Add(GetDataRow(unionDT, "ПО КЛАССУ", null, importRowClassAccount[1], importRowClassAccount[2], importRowClassAccount[3], importRowClassAccount[4], importRowClassAccount[5], importRowClassAccount[6]));
                }
              

            }
            return unionDT;
        }
        private DataRow GetDataRow ( DataTable dataTable, params object[] cells)
        {
            DataRow row = dataTable.NewRow();                   
                row.ItemArray = cells;             
             return row;
        }
        public IActionResult DownloadFile()
        {
            //add data to server data file
            UpdateServerDataFile();
            //download server data file
            var memory = DownloadStringFile("ServerData.txt", "wwwroot\\ServerData");
            return File(memory.ToArray(), "text/plain", "ServerData.txt");
        }
        private void UpdateServerDataFile()
        {

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\апро\\BalanceAccounts.mdf;Integrated Security=True";
            string sqlGetClassBalanceAccounts = $"SELECT CLASS, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES FROM ClassBalanceAccount";
            string sqlGetQuarternaryBalanceAccounts = $"SELECT CLASS, QUARTERNARY_ACCOUNT_NUMBER, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES FROM QuarternaryBalanceAccount";
            string sqlGetBinaryBalanceAccounts = $"SELECT BINARY_ACCOUNT_NUMBER, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES FROM BinaryBalanceAccount";
            // new data table with serer data 
            DataTable serverData = GetDataTable(connectionString, sqlGetClassBalanceAccounts, sqlGetQuarternaryBalanceAccounts, sqlGetBinaryBalanceAccounts);
            //uploading data from datatable into server data file 
            using StreamWriter streamWriter = new StreamWriter("wwwroot\\ServerData\\ServerData.txt");
           
            for (int i = 0; i <= serverData.Rows.Count - 1; i++)
            {
                DataRow currentRow = serverData.Rows[i];
                StringBuilder row = new StringBuilder();
                for(int j = 0; j < 8; j++)
                {
                    row.Append(currentRow[j].ToString()+"||");
                }
                row.Append("\n");
                streamWriter.WriteLine(row.ToString());
            }
        }
        private MemoryStream DownloadStringFile ( string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }
       
    }
}
