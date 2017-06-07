using Billing_Components.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing_Components
{
    class SortOptions
    {
        string LastOption = "Id";
        bool Descending = false;

        public void SetProperty(string option)
        {
            LastOption = option;
        }
        public void SetProperty(bool option)
        {
            Descending = option;
        }

        public string GetLastOption()
        {
            return LastOption;
        }
        public bool GetDescending()
        {
            return Descending;
        }
    }


    public class MemberPayment
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public double PaymentAmount { get; set; }
        public double PaidAmount { get; set; }


        public MemberPayment(DBConnection _DBConnection, DBConnection.Bill Bill, DBConnection.Member Member)
        {
            MemberId = Member.Id;
            Name = Member.GetName();

            PaymentAmount =0;

            PaidAmount = _DBConnection.DB_GetMonthlyPaymentAmount(Member, Bill.DueDateMonth, Bill.DueDateYear);
        }
    }

    public class BillReview
    {
        public string Name { get; set; }
        public double Paid { get; set; }
        public string paid
        {
            get
            {
                return String.Format("{0:f2} kn", Paid);
            }
        }
        public double Remaining { get; set; }
        public string remaining
        {
            get
            {
                return String.Format("{0:f2} kn", Remaining);
            }
        }

        public BillReview(DBConnection _DBConnection, DBConnection.Member member, bool isMonthly = false)
        {
            if(isMonthly)
            {
                Name = member.GetName();
                Paid = 0;
                Remaining = 0;
                foreach (DBConnection.Bill bill in _DBConnection.MonthBills)
                {
                    Remaining += bill.AmountHRK;
                }

                Remaining = Math.Round(Remaining / _DBConnection.Members.Count, 2);

                List<DBConnection.Payment> MemberPayments = (from item in _DBConnection.Payments
                                                             where item.MemberId == member.Id
                                                             select item).ToList<DBConnection.Payment>();

                List<DBConnection.Bill> PaidMonthBills = (from item in _DBConnection.MonthBills
                                                          where item.Paid
                                                          select item).ToList<DBConnection.Bill>();

                foreach (DBConnection.Bill bill in PaidMonthBills)
                {
                    double amount = (from item in MemberPayments
                                     where item.BillId == bill.Id
                                     select item.AmountHRK).FirstOrDefault<double>();
                    Paid += amount;
                    Remaining -= amount;

                }
            }
            else
            {
                Name = member.GetName();
                Paid = 0;
                Remaining = 0;
                foreach (DBConnection.Bill bill in _DBConnection.Bills)
                {
                    Remaining += bill.AmountHRK;
                }

                Remaining = Math.Round(Remaining / (double)_DBConnection.Members.Count, 2);

                List<DBConnection.Payment> MemberPayments = (from item in _DBConnection.Payments
                                                             where item.MemberId == member.Id
                                                             select item).ToList<DBConnection.Payment>();

                foreach (DBConnection.Bill bill in _DBConnection.Bills)
                {
                    double amount = (from item in MemberPayments
                                     where item.BillId == bill.Id
                                     select item.AmountHRK).FirstOrDefault<double>();
                    Paid += amount;
                    Remaining -= amount;

                }
            }

        }

    }
}
