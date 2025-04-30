using HollyJson.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System.Diagnostics.Contracts;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Contract
    {
        private int daysLeft;

        public int contractType { get; set; }
        public int amount
        {
            get => amount1;
            set
            { 
                amount1 = value;
                startAmount = value;
                if(DaysLeft > value * 365)
                {
                    calceddays = true;
                    DaysLeft = value * 365;
                }
            }
        }
        public int startAmount { get; set; }
        public double initialFee { get; set; }
        public double monthlySalary { get; set; }
        public double weightToSalary { get; set; }
        public DateTime dateOfSigning { get; set; }

        #region ImNotUse
        public bool is5050 { get; set; }
        public bool payed5050 { get; set; }
        public bool raiseIgn { get; set; }
        public int raiseCool { get; set; }
        public int raiseBonus { get; set; }
        public int ultimatumCool { get; set; }
        public int leaveCool { get; set; }
        public List<object> offers { get; set; }
        public object extension { get; set; }
        public bool Is5050 { get; set; }
        public double FeeWith5050 { get; set; }
        public int SecondPay { get; set; }
        #endregion
        [JsonIgnore]
        public int DaysLeft
        {
            get => daysLeft;
            set
            {
                if(value > amount * 365)
                {
                    value = amount * 365;
                }
                if (!calceddays)
                    dateOfSigning = dateOfSigning.AddDays(value - daysLeft);
                else
                    calceddays = false;
                daysLeft = value;
            }
        }
        private bool calceddays = false;
        private int amount1;

        public void SetCalcDaysLeft(DateTime now)
        {
            var t = dateOfSigning.AddYears(amount);
            TimeSpan ts = t - now;
            calceddays = true;
            DaysLeft = (int)ts.TotalDays;
        }
        public static bool operator ==(Contract a, Contract b)
        {
            if (a is null) return b is null;
            if (b is null) return a is null;
            return
            b.amount == a.amount &
            b.startAmount == a.startAmount &
            b.initialFee == a.initialFee &
            b.monthlySalary == a.monthlySalary &
            b.weightToSalary == a.weightToSalary &
            b.dateOfSigning == a.dateOfSigning &
            b.contractType == a.contractType;
        }
        public static bool operator !=(Contract a, Contract b)
        {
            return !(a == b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not null)
            {
                return (obj as Contract)! == this;
            }
            else
                return false;
        }
        public Contract(DateTime now)
        {
            amount = 3;
            startAmount = 3;
            monthlySalary = 0;
            weightToSalary = 100;
            dateOfSigning = now != new DateTime() ? now.AddDays(-1) : now;
            SetCalcDaysLeft(dateOfSigning);
            initialFee = 100;
            contractType = 0;


            is5050 = false;
            payed5050 = false;
            raiseIgn = false;
            raiseCool = 0;
            raiseBonus = 0;
            ultimatumCool = 0;
            leaveCool = 0;
            offers = new List<object>();
            extension = null;
            Is5050 = false;
            FeeWith5050 = 100;
            SecondPay = 50;
        }
    }
}
