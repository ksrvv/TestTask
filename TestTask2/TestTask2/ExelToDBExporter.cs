using ExcelDataReader;
using System.Data.SqlClient;
namespace TestTask2
{
    public class ExelToDBExporter
    {
        private readonly string connectgionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\апро\\BalanceAccounts.mdf;Integrated Security=True";
       
        public void Export(string filename)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var sqlConnection = new SqlConnection(connectgionString);
            sqlConnection.Open();
            string filepath = "C:\\Users\\апро\\Desktop\\TestTask\\TestTask2\\TestTask2\\Upload\\" + filename;
            using var insertData = new SqlCommand("AddData", sqlConnection);
            using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                string currentClass = "";
                while (reader.Read())
                {
                    var current = reader.GetValue(0);
                    if (current != null)
                    {//row with string data in [0] cell

                        if (current.ToString().StartsWith("КЛАСС"))
                        {//class name row
                            currentClass = current.ToString();
                        }
                        else if (current.ToString().StartsWith("ПО КЛАССУ"))
                        {//class ballance account row
                            double openingBalanceAssets, openingBalanceLiabilities, turnoverDebit, turnoverCredit, closingBalanceassets, closingBalanceLiabilities;
                            openingBalanceAssets = reader.GetDouble(1);
                            openingBalanceLiabilities = reader.GetDouble(2);
                            turnoverDebit = reader.GetDouble(3);
                            turnoverCredit = reader.GetDouble(4);
                            closingBalanceassets = reader.GetDouble(5);
                            closingBalanceLiabilities = reader.GetDouble(6);
                            string sqlExpression = String.Format("INSERT INTO ClassBalanceAccount (CLASS, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES, [FILE]) VALUES (@class, @openingBalanceAssets, @openingBalanceLiabilities, @turnoverDebit, @turnoverCredit, @closingBalanceassets, @closingBalanceLiabilities, @filename)");
                            using (SqlConnection connection = new SqlConnection(connectgionString))
                            {
                                connection.Open();
                                SqlCommand command = new SqlCommand(sqlExpression, connection);
                                SqlParameter classParam = new SqlParameter("@class", currentClass);
                                command.Parameters.Add(classParam);
                                SqlParameter openingBalanceAssetsParam = new SqlParameter("@openingBalanceAssets", openingBalanceAssets);
                                command.Parameters.Add(openingBalanceAssetsParam);
                                SqlParameter openingBalanceLiabilitiesParam = new SqlParameter("@openingBalanceLiabilities", openingBalanceLiabilities);
                                command.Parameters.Add(openingBalanceLiabilitiesParam);
                                SqlParameter turnoverDebitParam = new SqlParameter("@turnoverDebit", turnoverDebit);
                                command.Parameters.Add(turnoverDebitParam);
                                SqlParameter turnoverCreditParam = new SqlParameter("@turnoverCredit", turnoverCredit);
                                command.Parameters.Add(turnoverCreditParam);
                                SqlParameter closingBalanceassetsParam = new SqlParameter("@closingBalanceassets", closingBalanceassets);
                                command.Parameters.Add(closingBalanceassetsParam);
                                SqlParameter closingBalanceLiabilitiesParam = new SqlParameter("@closingBalanceLiabilities", closingBalanceLiabilities);
                                command.Parameters.Add(closingBalanceLiabilitiesParam);
                                SqlParameter filenameParam = new SqlParameter("@filename", filepath);
                                command.Parameters.Add(filenameParam);
                                int number = command.ExecuteNonQuery();
                            }

                        }
                        else if (int.TryParse(current.ToString(), out int temp))//row with numeric data in [0] cell
                        {
                            var currentBallaneAccount = Convert.ToInt32(current);
                            double openingBalanceAssets, openingBalanceLiabilities, turnoverDebit, turnoverCredit, closingBalanceassets, closingBalanceLiabilities;
                            openingBalanceAssets = reader.GetDouble(1);
                            openingBalanceLiabilities = reader.GetDouble(2);
                            turnoverDebit = reader.GetDouble(3);
                            turnoverCredit = reader.GetDouble(4);
                            closingBalanceassets = reader.GetDouble(5);
                            closingBalanceLiabilities = reader.GetDouble(6);
                            if (currentBallaneAccount >= 10 && currentBallaneAccount <= 99)//binary ballance account row
                            {
                                string sqlExpression = String.Format("INSERT INTO BinaryBalanceAccount (BINARY_ACCOUNT_NUMBER, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES, [FILE]) VALUES (@binaryAccountNumber, @openingBalanceAssets, @openingBalanceLiabilities, @turnoverDebit, @turnoverCredit, @closingBalanceassets, @closingBalanceLiabilities, @filename)");
                                using (SqlConnection connection = new SqlConnection(connectgionString))
                                {
                                    connection.Open();
                                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                                    SqlParameter binaryAccountNumberParam = new SqlParameter("@binaryAccountNumber", currentBallaneAccount);
                                    command.Parameters.Add(binaryAccountNumberParam);
                                    SqlParameter openingBalanceAssetsParam = new SqlParameter("@openingBalanceAssets", openingBalanceAssets);
                                    command.Parameters.Add(openingBalanceAssetsParam);
                                    SqlParameter openingBalanceLiabilitiesParam = new SqlParameter("@openingBalanceLiabilities", openingBalanceLiabilities);
                                    command.Parameters.Add(openingBalanceLiabilitiesParam);
                                    SqlParameter turnoverDebitParam = new SqlParameter("@turnoverDebit", turnoverDebit);
                                    command.Parameters.Add(turnoverDebitParam);
                                    SqlParameter turnoverCreditParam = new SqlParameter("@turnoverCredit", turnoverCredit);
                                    command.Parameters.Add(turnoverCreditParam);
                                    SqlParameter closingBalanceassetsParam = new SqlParameter("@closingBalanceassets", closingBalanceassets);
                                    command.Parameters.Add(closingBalanceassetsParam);
                                    SqlParameter closingBalanceLiabilitiesParam = new SqlParameter("@closingBalanceLiabilities", closingBalanceLiabilities);
                                    command.Parameters.Add(closingBalanceLiabilitiesParam);
                                    SqlParameter filenameParam = new SqlParameter("@filename", filepath);
                                    command.Parameters.Add(filenameParam);
                                    int number = command.ExecuteNonQuery();
                                }
                            }
                            else//Quarternary ballance account row
                            {
                               
                                string sqlExpression = String.Format("INSERT INTO QuarternaryBalanceAccount (CLASS, QUARTERNARY_ACCOUNT_NUMBER, OPENING_BALANCE_ASSETS, OPENING_BALANCE_LIABILITIES, TURNOVER_DEBIT, TURNOVER_CREDIT, CLOSING_BALANCE_ASSETS, CLOSING_BALANCE_LIABILITIES, [FILE]) VALUES (@class, @quarternaryAccountNumber, @openingBalanceAssets, @openingBalanceLiabilities, @turnoverDebit, @turnoverCredit, @closingBalanceassets, @closingBalanceLiabilities, @filename)");
                                using (SqlConnection connection = new SqlConnection(connectgionString))
                                {
                                    connection.Open();
                                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                                    SqlParameter classParam = new SqlParameter("@class", currentClass);
                                    command.Parameters.Add(classParam);
                                    SqlParameter quarternaryAccountNumberParam = new SqlParameter("@quarternaryAccountNumber", currentBallaneAccount);
                                    command.Parameters.Add(quarternaryAccountNumberParam);
                                    SqlParameter openingBalanceAssetsParam = new SqlParameter("@openingBalanceAssets", openingBalanceAssets);
                                    command.Parameters.Add(openingBalanceAssetsParam);
                                    SqlParameter openingBalanceLiabilitiesParam = new SqlParameter("@openingBalanceLiabilities", openingBalanceLiabilities);
                                    command.Parameters.Add(openingBalanceLiabilitiesParam);
                                    SqlParameter turnoverDebitParam = new SqlParameter("@turnoverDebit", turnoverDebit);
                                    command.Parameters.Add(turnoverDebitParam);
                                    SqlParameter turnoverCreditParam = new SqlParameter("@turnoverCredit", turnoverCredit);
                                    command.Parameters.Add(turnoverCreditParam);
                                    SqlParameter closingBalanceassetsParam = new SqlParameter("@closingBalanceassets", closingBalanceassets);
                                    command.Parameters.Add(closingBalanceassetsParam);
                                    SqlParameter closingBalanceLiabilitiesParam = new SqlParameter("@closingBalanceLiabilities", closingBalanceLiabilities);
                                    command.Parameters.Add(closingBalanceLiabilitiesParam);
                                    SqlParameter filenameParam = new SqlParameter("@filename", filepath);
                                    command.Parameters.Add(filenameParam);
                                    int number = command.ExecuteNonQuery();
                                }
                            }
                        }
                        else continue;
                    }
                }
            }
        }
    }
}

