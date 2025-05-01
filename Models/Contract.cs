using Newtonsoft.Json;
using PropertyChanged;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Contract
    {
        private int daysLeft;
        private int amount1;
        public bool IsInit { get; set; }
        public int contractType { get; set; }
        public int amount
        {
            get => amount1;
            set
            {
                amount1 = value;
                startAmount = value;
                //if(DaysLeft > value * 365)
                //{
                //    calceddays = true;
                //    DaysLeft = value * 365;
                //}
            }
        }
        public int startAmount { get; set; }
        public double initialFee { get; set; }
        public double monthlySalary { get; set; }
        public double weightToSalary { get; set; }
        public DateTime dateOfSigning { get; set; }
        [JsonIgnore]
        public DateTime dateOfEnding => dateOfSigning.AddYears(amount);
        [JsonIgnore]
        public DateTime dateOfNow { get; set; }
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
                //if(value > amount * 365)
                //{
                //    value = amount * 365;
                //}
                //if (!calceddays)
                //    dateOfSigning = dateOfSigning.AddDays(value - daysLeft);
                //else
                //    calceddays = false;

                /*
                 * 1. увеличение дней.. двигаем дату подписания вперед. 
                 * Не может быть раньше чем сейчас - 1
                 * Если выходим за границу, увеличить кол-во лет контракта (амоунт) + 1
                 * 2. уменьшение дней.. двигаем дату подписания назад.
                 * Дата окончания не может быть раньше чем сейчас + 1
                 * (минимальный = 1 день)
                 * 
                 * 3. увеличение лет контракта. Пересчет оставшихся дней, но без изменения даты контракта
                 * 4. уменьшение лет контракта. Мин = 1 год. Пересчет дней, но без изменения подписания.
                 * 
                 * 0. При инициализации нельзя ниче двигать..
                 * OK _IsInit..
                 * но как его снять? после инициализации?
                 */
                //1.
                if (!IsInit)
                {
                    //
                    //300 -> 400 = +100
                    if (value - daysLeft > 0)
                    {
                        DateTime calc_date = dateOfSigning.AddDays(value - daysLeft);
                        if (dateOfNow != new DateTime())
                        {
                            if (calc_date >= dateOfNow.AddDays(-1)) //если по дате
                                                                    //подписания вылетим за текущую дату
                                                                    //то увеличим кол-во лет контракта
                                                                    //но установим дату подписнаия на вчера
                            {
                                var b = ((calc_date - dateOfNow).TotalDays) / 365.2425;
                                amount += (int)Math.Ceiling(b); //чтоб влезть от текущей даты
                                dateOfSigning = dateOfNow.AddDays(-1);
                            }
                        }
                    }
                    //-100
                    else
                    {
                        if(dateOfNow != new DateTime())
                        {
                            DateTime calc_date = dateOfSigning.AddDays(value - daysLeft);
                            DateTime end_date = calc_date.AddYears(amount);
                            //если окончание стало меньше чем сейчас.. то надо:
                            //уменьшить срок контракта.. если после этого все равно меньше то
                            //
                            if (end_date < dateOfNow.AddDays(1))
                            {
                                
                            }
                        }
                    }
                }


                daysLeft = value;
            }
        }

        public void SetCalcDaysLeft()//DateTime now)
        {
            var t = dateOfEnding;
            TimeSpan ts = t - dateOfNow;
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
        public Contract()
        {
            IsInit = true;
        }
        public Contract(DateTime now)
        {
            IsInit = true;
            amount = 3;
            startAmount = 3;
            monthlySalary = 0;
            weightToSalary = 100;
            dateOfSigning = now != new DateTime() ? now.AddDays(-1) : now;
            dateOfNow = now;
            SetCalcDaysLeft();
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
            IsInit = false;
        }
    }
}
