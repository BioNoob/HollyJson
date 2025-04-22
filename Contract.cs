using PropertyChanged;

namespace HollyJson
{
    [AddINotifyPropertyChangedInterface]
    public class Contract
    {
        private int daysLeft;

        public int amount { get; set; }
        public int startAmount { get; set; }
        public double initialFee { get; set; }
        public double monthlySalary { get; set; }
        public double weightToSalary { get; set; }
        public DateTime dateOfSigning { get; set; }
        public int contractType { get; set; }
        public int DaysLeft
        {
            get => daysLeft; 
            set
            {
                if (!calceddays)
                    dateOfSigning = dateOfSigning.AddDays(value - daysLeft);//daysLeft - value);
                else
                    calceddays = false;
                daysLeft = value;
            }
        }
        bool calceddays = false;
        public void SetCalcDaysLeft(DateTime now)
        {
            var t = dateOfSigning.AddYears(amount);
            TimeSpan ts = t - now;//now - dateOfSigning;
            calceddays = true;
            DaysLeft = (int)ts.TotalDays;
        }
    }
}
