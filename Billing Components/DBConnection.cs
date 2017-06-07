using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing_Components.DB
{
    public class DBConnection
    {
        public class Member
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool Contributes { get; set; }
            public bool Active { get; set; }

            
            public Member(DataRow dataRow)
            {
                Id = Convert.ToInt32(dataRow["Id"]);
                FirstName = dataRow["FirstName"].ToString();
                LastName = dataRow["LastName"].ToString();
                Contributes = Convert.ToBoolean(dataRow["Contributes"]);
                Active = Convert.ToBoolean(dataRow["Active"]);
            }

            public string GetName()
            {
                return FirstName + " " + LastName;
            }
        }
        public class Payment
        {
            public int Id { get; set; }
            public int MemberId { get; set; }
            public int BillId { get; set; }
            public string Currency { get; set; }
            public string amount
            {
                get
                {
                    switch (Currency)
                    {
                        case "HRK":
                            return String.Format("{0:f2} kn", Amount);
                        case "EUR":
                            return String.Format("€ {0:f2}", Amount);
                        default:
                            return String.Format("{0:f2} kn", Amount);
                    }
                }
            }
            public double Amount { get; set; }
            public double AmountHRK
            {
                get
                {
                    switch (Currency)
                    {
                        case "HRK":
                            return Amount;
                        case "EUR":
                            return Amount * Properties.Settings.Default.EURRate;
                        default:
                            return Amount;
                    }
                }
            }

            public Payment(DataRow dataRow)
            {
                Id = Convert.ToInt32(dataRow["Id"]);
                MemberId = Convert.ToInt32(dataRow["MemberId"]);
                BillId = Convert.ToInt32(dataRow["BillId"]);
                Currency = dataRow["Currency"].ToString();
                Amount = Convert.ToDouble(dataRow["Amount"]);
            }

            public Payment(int MemberId, int BillId, string Currency, double Amount)
            {
                this.Id = -1;
                this.MemberId = MemberId;
                this.BillId = BillId;
                this.Currency = Currency;
                this.Amount = Amount;
            }

        }
        public class Bill
        {
            public int Id { get; set; }
            public string GUID { get; set; }
            public string Payer { get; set; }
            public string Category { get; set; }
            public string Recipient { get; set; }
            public string Currency { get; set; }

            public string amount
            {
                get
                {
                    switch(Currency)
                    {
                        case "HRK":
                            return String.Format("{0:f2} kn", Amount);
                        case "EUR":
                            return String.Format("€ {0:f2}", Amount);
                        default:
                            return String.Format("{0:f2} kn", Amount);
                    }
                }
            }
            public double Amount { get; set; }

            public double AmountHRK
            {
                get
                {
                    switch (Currency)
                    {
                        case "HRK":
                            return Amount;
                        case "EUR":
                            return Amount * Properties.Settings.Default.EURRate;
                        default:
                            return Amount;
                    }
                }
            }

            public string IBANOfRecipient { get; set; }
            public string Model { get; set; }
            public string ReferenceNumber { get; set; }
            public string Description { get; set; }

            public DateTime DueDate { get; set; }
            public int DueDateYear
            {
                get
                {
                    return DueDate.Year;
                }
                set
                {
                    DueDate = new DateTime(value, DueDate.Month, DueDate.Day);
                }
            }
            public int DueDateMonth
            {
                get
                {
                    return DueDate.Month;
                }
                set
                {
                    DueDate = new DateTime(DueDate.Year, value, DueDate.Day);
                }
            }
            public int DueDateDay
            {
                get
                {
                    return DueDate.Day;
                }
                set
                {
                    DueDate = new DateTime(DueDate.Year, DueDate.Month, value);
                }
            }

            public DateTime ForMonth { get; set; }
            public int ForMonthYear
            {
                get
                {
                    return ForMonth.Year;
                }
                set
                {
                    ForMonth = new DateTime(value, ForMonth.Month, ForMonth.Day);
                }
            }
            public int ForMonthMonth
            {
                get
                {
                    return ForMonth.Month;
                }
                set
                {
                    ForMonth = new DateTime(ForMonth.Year, value, ForMonth.Day);
                }
            }


            public bool Paid { get; set; }
            public DateTime DatePaid { get; set; }

            public Bill(DataRow dataRow)
            {
                Id = Convert.ToInt32(dataRow["Id"]);
                Payer = dataRow["Payer"].ToString();
                Category = dataRow["Category"].ToString();
                Recipient = dataRow["Recipient"].ToString();
                Currency = dataRow["Currency"].ToString();
                Amount = Convert.ToDouble(dataRow["Amount"]);
                IBANOfRecipient = dataRow["IBANOfRecipient"].ToString();
                Model = dataRow["Model"].ToString();
                ReferenceNumber = dataRow["ReferenceNumber"].ToString();
                Description = dataRow["Description"].ToString();
                DueDate = Convert.ToDateTime(dataRow["DueDate"]);
                ForMonth = Convert.ToDateTime(dataRow["ForMonth"]);
                Paid = Convert.ToBoolean(dataRow["Paid"]);
                if (Paid)
                    DatePaid = Convert.ToDateTime(dataRow["DatePaid"]);
                else
                    DatePaid = default(DateTime);

                DueDateDay = DueDate.Day;
                DueDateMonth = DueDate.Month;
                DueDateYear = DueDate.Year;
            }

            public Bill()
            {
                GUID = String.Empty;
                Payer = String.Empty;
                Category = String.Empty;
                Recipient = String.Empty;
                Currency = String.Empty;
                Amount = 0;
                IBANOfRecipient = String.Empty;
                Model = String.Empty;
                ReferenceNumber = String.Empty;
                Description = String.Empty;
                DueDate = DateTime.Today;
                ForMonth = DateTime.Today;
                Paid = false;
                DatePaid = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);

                DueDateDay = DueDate.Day;
                DueDateMonth = DueDate.Month;
                DueDateYear = DueDate.Year;

                ForMonthMonth = ForMonth.Month;
                ForMonthYear = ForMonth.Year;
            }

        }
        public class BillPayment
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Currency { get; set; }
            public string amount
            {
                get
                {
                    switch (Currency)
                    {
                        case "HRK":
                            return String.Format("{0:f2} kn", Amount);
                        case "EUR":
                            return String.Format("€ {0:f2}", Amount);
                        default:
                            return String.Format("{0:f2} kn", Amount);
                    }
                }
            }
            public double Amount { get; set; }
            public double AmountHRK
            {
                get
                {
                    switch (Currency)
                    {
                        case "HRK":
                            return Amount;
                        case "EUR":
                            return Amount * Properties.Settings.Default.EURRate;
                        default:
                            return Amount;
                    }
                }
            }

            public BillPayment(DataRow dataRow)
            {
                Id = Convert.ToInt32(dataRow["Id"]);
                Name = dataRow["FirstName"].ToString() + " " + dataRow["LastName"].ToString();
                Currency = dataRow["Currency"].ToString();
                Amount = Convert.ToDouble(dataRow["Amount"]);
            }
        }
        public class BillTemplate
        {
            public string Payer { get; set; }
            public string Category { get; set; }
            public string Recipient { get; set; }
            public string Currency { get; set; }
            public string IBANOfRecipient { get; set; }
            public string Model { get; set; }

            public BillTemplate()
            {
                Payer = String.Empty;
                Category = "New";
                Recipient = String.Empty;
                Currency = String.Empty;
                IBANOfRecipient = String.Empty;
                Model = String.Empty;
            }
            public BillTemplate(Bill template)
            {
                Payer = template.Payer;
                Category = template.Category;
                Recipient = template.Recipient;
                Currency = template.Currency;
                IBANOfRecipient = template.IBANOfRecipient;
                Model = template.Model;
            }
        }

        public List<Bill> Bills { get; set; }
        public List<Bill> MonthBills { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Member> Members { get; set; }

        public Dictionary<string, BillTemplate> BillTemplates { get; set; }

        private int Monthly_Month;
        private int Monthly_Year;


        private List<Member> DB_GetMembers()
        {
            List<Member> results = new List<Member>();

            using (var conection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            {
                DataTable myDataTable = new DataTable();
                conection.Open();
                String query = "Select * From Members WHERE Members.Active = True";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);

                Member member;

                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    member = new DBConnection.Member(myDataTable.Rows[i]);
                    
                    results.Add(member);
                }
            }

            return results;
        }
        private List<Bill> DB_GetBills()
        {
            List<Bill> results = new List<Bill>();

            using (var conection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            {
                DataTable myDataTable = new DataTable();
                conection.Open();
                String query = "Select * From Bills WHERE Bills.Active = True";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);

                Bill bill;

                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    bill = new DBConnection.Bill(myDataTable.Rows[i]);
                    
                    results.Add(bill);
                }
            }

            return results;
        }
        private List<Payment> DB_GetPayments()
        {
            List<Payment> results = new List<Payment>();

            using (var conection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            {
                DataTable myDataTable = new DataTable();
                conection.Open();
                String query = "SELECT * FROM Payments";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);

                Payment bill;

                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    bill = new DBConnection.Payment(myDataTable.Rows[i]);

                    results.Add(bill);
                }
            }

            return results;
        }

        public List<BillPayment> DB_GetPaymentsForBill(int BillId)
        {
            List<BillPayment> results = new List<BillPayment>();

            using (var conection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            {
                DataTable myDataTable = new DataTable();
                conection.Open();
                String query = "SELECT Payments.Id, Members.FirstName, Members.LastName, Payments.Currency, Payments.Amount, Payments.BillId FROM Payments INNER JOIN Members ON Members.Id=Payments.MemberId WHERE Payments.BillId = "+BillId+";";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);

                BillPayment bill;

                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    bill = new DBConnection.BillPayment(myDataTable.Rows[i]);

                    results.Add(bill);
                }
            }

            return results;
        }
        public List<Bill> DB_GetPaidBills(int Month, int Year)
        {
            return (from item in Bills
                    where item.Paid && item.DueDateMonth == Month && item.DueDateYear == Year
                    select item).ToList<Bill>();
        }

        public double DB_GetMonthlyPaymentAmount(Member member, int Month, int Year)
        {
            double result = 0;
            double value;
            string currency;
            using (var conection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            {
                DataTable myDataTable = new DataTable();
                conection.Open();
                String query = "SELECT Payments.MemberId, Payments.Amount, Bills.Currency FROM Bills INNER JOIN Payments ON Bills.Id = Payments.BillId WHERE (((Format([Bills].[DueDate],\"mm-yyyy\"))= \"" + Month.ToString().PadLeft(2,'0') + "-" + Year.ToString() + "\") AND ((Payments.MemberId)=" + member.Id + ")) ";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);

                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    value = Convert.ToDouble(myDataTable.Rows[i]["Amount"]);
                    currency = myDataTable.Rows[i]["Currency"].ToString();
                    if(currency=="EUR")
                        value*=Properties.Settings.Default.EURRate;

                    result += value;
                }
            }






            //List<Bill> Paid = DB_GetPaidBills(Month, Year);

            //foreach(Bill bill in Paid)
            //{
            //    result += (from item in DB_GetPaymentsForBill(bill.Id) where item.Name == member.GetName() select item.AmountHRK).FirstOrDefault<double>();
            //}

            return result;
        }


        public int DB_UpdateBill(Bill bill)
        {
            using (var connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandType = CommandType.Text;

                command.CommandText = "UPDATE Bills SET Payer = @Payer, Category = @Category, Recipient = @Recipient, [Currency] = @Currency, Amount = @Amount, IBANOfRecipient = @IBANOfRecipient, [Model] = @Model, ReferenceNumber = @ReferenceNumber, Description = @Description, DueDate = @DueDate, ForMonth = @ForMonth, Paid = @Paid, DatePaid = @DatePaid WHERE Id = @Id";
                command.Parameters.Add("@Payer", OleDbType.VarChar).Value = bill.Payer;
                command.Parameters.Add("@Category", OleDbType.VarChar).Value = bill.Category;
                command.Parameters.Add("@Recipient", OleDbType.VarChar).Value = bill.Recipient;
                command.Parameters.Add("@Currency", OleDbType.VarChar).Value = bill.Currency;
                command.Parameters.Add("@Amount", OleDbType.Double).Value = bill.Amount;
                command.Parameters.Add("@IBANOfRecipient", OleDbType.VarChar).Value = bill.IBANOfRecipient;
                command.Parameters.Add("@Model", OleDbType.VarChar).Value = bill.Model;
                command.Parameters.Add("@ReferenceNumber", OleDbType.VarChar).Value = bill.ReferenceNumber;
                command.Parameters.Add("@Description", OleDbType.VarChar).Value = bill.Description;
                command.Parameters.Add("@DueDate", OleDbType.Date).Value = bill.DueDate.Date;
                command.Parameters.Add("@ForMonth", OleDbType.Date).Value = bill.ForMonth.Date;
                command.Parameters.Add("@Paid", OleDbType.Boolean).Value = bill.Paid;
                command.Parameters.Add("@DatePaid", OleDbType.Date).Value = bill.DatePaid.Date;
                command.Parameters.Add("@Id", OleDbType.Integer).Value = bill.Id;

                try
                {
                    int Rows = command.ExecuteNonQuery();
                    return Rows;
                }
                catch
                {
                    return -1;//for error
                }
            }
        }

        public int DB_RemoveBill(int BillId)
        {
            using (var connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandType = CommandType.Text;

                command.CommandText = "UPDATE Bills SET Active = @Active WHERE Id = @Id";
                command.Parameters.Add("@Active", OleDbType.Boolean).Value = false;
                command.Parameters.Add("@Id", OleDbType.Integer).Value = BillId;

                try
                {
                    int Rows = command.ExecuteNonQuery();
                    return Rows;
                }
                catch
                {
                    return -1;//for error
                }
            }
        }

        public int DB_InsertBill(Bill bill)
        {
            using (var connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandType = CommandType.Text;

                command.CommandText = "INSERT INTO Bills ([GUID], Payer, Category, Recipient, [Currency], Amount, IBANOfRecipient, [Model], ReferenceNumber, Description, DueDate, ForMonth) VALUES (@GUID, @Payer, @Category, @Recipient, @Currency, @Amount, @IBANOfRecipient, @Model, @ReferenceNumber, @Description, @DueDate, @ForMonth)";
                command.Parameters.Add("@GUID", OleDbType.VarChar).Value = bill.GUID;
                command.Parameters.Add("@Payer", OleDbType.VarChar).Value = bill.Payer;
                command.Parameters.Add("@Category", OleDbType.VarChar).Value = bill.Category;
                command.Parameters.Add("@Recipient", OleDbType.VarChar).Value = bill.Recipient;
                command.Parameters.Add("@Currency", OleDbType.VarChar).Value = bill.Currency;
                command.Parameters.Add("@Amount", OleDbType.Double).Value = bill.Amount;
                command.Parameters.Add("@IBANOfRecipient", OleDbType.VarChar).Value = bill.IBANOfRecipient;
                command.Parameters.Add("@Model", OleDbType.VarChar).Value = bill.Model;
                command.Parameters.Add("@ReferenceNumber", OleDbType.VarChar).Value = bill.ReferenceNumber;
                command.Parameters.Add("@Description", OleDbType.VarChar).Value = bill.Description;
                command.Parameters.Add("@DueDate", OleDbType.Date).Value = bill.DueDate.Date;
                command.Parameters.Add("@ForMonth", OleDbType.Date).Value = bill.ForMonth.Date;

                try
                {
                    int Rows = command.ExecuteNonQuery();
                    return Rows;
                }
                catch
                {
                    return -1; //for error
                }
            }
        }

        public DBConnection()
        {
            Bills = DB_GetBills();
            Monthly_Month = DateTime.Today.Month;
            Monthly_Year = DateTime.Today.Year;
            GetMonthOverview(Monthly_Month, Monthly_Year);
            Payments = DB_GetPayments();
            Members = DB_GetMembers();

            BillTemplates = CreateBillTemplates();

        }

        private Dictionary<string, BillTemplate> CreateBillTemplates()
        {
            Dictionary<string,BillTemplate> Template = new Dictionary<string,BillTemplate>();

            List<Bill> Distinct = (from item in Bills
                                   group item by new { item.Category } //or group by new {p.ID, p.Name, p.Whatever}
                                       into distinct
                                       select distinct.FirstOrDefault()).ToList<Bill>();

            foreach(Bill bill in Distinct)
            {
                Template.Add(bill.Category, new BillTemplate(bill));
            }

            Template.Add("New", new BillTemplate());
            return Template;
        }

        public int UpdateId(string GUID)
        {
            using (var conection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            {
                DataTable myDataTable = new DataTable();
                conection.Open();
                String query = "SELECT Id FROM Bills WHERE Bills.GUID = '" + GUID + "'";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);


                return Convert.ToInt32(myDataTable.Rows[0]["Id"]);
            }
        }

        public int CreateGUID(Bill bill)
        {
            bill.GUID = CreateMD5(bill);

            using (var connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandType = CommandType.Text;

                command.CommandText = "UPDATE Bills SET [GUID] = @GUID WHERE Id = @Id";
                command.Parameters.Add("@GUID", OleDbType.VarChar).Value = bill.GUID;
                command.Parameters.Add("@Id", OleDbType.Integer).Value = bill.Id;

                try
                {
                    int Rows = command.ExecuteNonQuery();
                    return Rows;
                }
                catch
                {
                    return -1;//for error
                }
            }
        }

        public void ReOrderMonthBills()
        {
            MonthBills = (from item in Bills where item.DueDateMonth == Monthly_Month && item.DueDateYear == Monthly_Year select item).ToList<Bill>();
        }

        public void GetMonthOverview(int Month, int Year)
        {
            Monthly_Year = Year;
            Monthly_Month = Month;
            MonthBills = (from item in Bills where item.DueDateMonth == Month && item.DueDateYear == Year select item).ToList<Bill>();
        }

        internal void RefreshTemplates()
        {
            BillTemplates.Clear();

            List<Bill> Distinct = (from item in Bills
                                   group item by new { item.Category } //or group by new {p.ID, p.Name, p.Whatever}
                                       into distinct
                                       select distinct.FirstOrDefault()).ToList<Bill>();

            foreach (Bill bill in Distinct)
            {
                BillTemplates.Add(bill.Category, new BillTemplate(bill));
            }

            BillTemplates.Add("New", new BillTemplate());
        }

        internal int DB_InsertPayment(double Amount, int BillId, string Currency, int MemberId)
        {
            using (var connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandType = CommandType.Text;

                command.CommandText = "INSERT INTO Payments (MemberId, BillId, [Currency], [Amount]) VALUES (@MemberId, @BillId, @Currency, @Amount)";
                command.Parameters.Add("@MemberId", OleDbType.Integer).Value = MemberId;
                command.Parameters.Add("@BillId", OleDbType.Integer).Value = BillId;
                command.Parameters.Add("@Currency", OleDbType.VarChar).Value = Currency;
                command.Parameters.Add("@Amount", OleDbType.Double).Value = Amount;

                try
                {
                    int Rows = command.ExecuteNonQuery();

                    Payments.Add(new Payment(MemberId, BillId, Currency, Amount));

                    return Rows;
                }
                catch
                {
                    return -1; //for error
                }
            }
        }

        internal int DB_MarkBillPaid(Bill bill)
        {
            using (var connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\BC.accdb"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandType = CommandType.Text;

                command.CommandText = "UPDATE Bills SET Paid = @Paid, DatePaid = @DatePaid WHERE Id = @Id";
                command.Parameters.Add("@Paid", OleDbType.Boolean).Value = bill.Paid;
                command.Parameters.Add("@DatePaid", OleDbType.Date).Value = bill.DatePaid;
                command.Parameters.Add("@Id", OleDbType.Integer).Value = bill.Id;

                try
                {
                    int Rows = command.ExecuteNonQuery();
                    return Rows;
                }
                catch
                {
                    return -1;//for error
                }
            }
        }

        public string CreateMD5(Bill Bill)
        {
            string input = String.Format("{0} - {1} - {2} - {3}", Bill.Category, Bill.AmountHRK, Bill.DueDate, Bill.ReferenceNumber);
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
