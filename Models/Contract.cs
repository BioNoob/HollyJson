using Newtonsoft.Json;
using PropertyChanged;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Contract
    {
        private int daysLeft;
        private int amount1;
        private bool lockamount = false;
        public bool IsInit { get; set; }
        public int contractType { get; set; }
        public int amount
        {
            get => amount1;
            set
            {
                amount1 = value;
                startAmount = value;
                //имеет приоритет перед кол-во дней!
                if(!IsInit)
                {
                    if(!lockamount)
                    {
                        //менять кол-во дней
                        IsInit = true;
                        DaysLeft = (int)Math.Ceiling((dateOfEnding - dateOfNow).TotalDays);
                        if(DaysLeft < 1)
                        {
                            dateOfSigning = dateOfSigning.AddDays(Math.Abs(DaysLeft) + 1);//+ сколько то чтоб получился 1 день?
                            DaysLeft = 1;
                            //+1 на случай нуля
                        }
                        IsInit = false;
                    }
                }
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
                if (!IsInit)
                {

                    var daysbeforenow = (dateOfNow - dateOfSigning).TotalDays; //370
                    var daysafternow = (dateOfEnding - dateOfNow).TotalDays; //350
                    var differernce = value - DaysLeft; // 380 //30
                    if (differernce > 0) //увеличиваем кол-во дней
                    {
                        if (differernce > daysbeforenow) //не осталось для смещения вперед
                        {
                            //докинули год лет
                            lockamount = true;
                            amount += (int)Math.Ceiling((differernce - daysbeforenow) / 365.2425);
                            lockamount = false;
                            //сколько всего должно получится дней
                            var im_need = differernce + daysbeforenow; // 730
                                                                       //пересчитали новую концову
                            daysafternow = (dateOfSigning.AddYears(amount) - dateOfNow).TotalDays; //715
                                                                                                   //определили сколько должно быть до начала контракта
                            var to_move = daysafternow - im_need; //15
                            var daystoremovefromstart = to_move - daysbeforenow; //-355
                            dateOfSigning = dateOfSigning.AddDays(daystoremovefromstart);
                        }
                        else 
                        {
                            dateOfSigning = dateOfSigning.AddDays(differernce);
                        }
                    }
                    else //Уменьшаем кол-во дней //-340
                    {
                        if((daysafternow - differernce) > 0) //10
                        {
                            var to_move = differernce;
                            dateOfSigning = dateOfSigning.AddDays(differernce);
                        }
                        else // когда вылетели
                        {
                            value = 1;
                            dateOfSigning = dateOfSigning.AddDays(-1 * (daysafternow - 1));
                        }
                    }
                    //
                    //300 -> 400 = +100
                    //if (value - daysLeft > 0)
                    //{
                    //    DateTime calc_date = dateOfSigning.AddDays(value - daysLeft);
                    //    if (dateOfNow != new DateTime())
                    //    {
                    //        if (calc_date >= dateOfNow.AddDays(-1)) //если по дате
                    //                                                //подписания вылетим за текущую дату
                    //                                                //то увеличим кол-во лет контракта
                    //                                                //но установим дату подписнаия на вчера
                    //        {
                    //            var b = ((calc_date - dateOfNow).TotalDays) / 365.2425;
                    //            amount += (int)Math.Ceiling(b); //чтоб влезть от текущей даты
                    //            dateOfSigning = dateOfNow.AddDays(-1);
                    //        }
                    //    }
                    //}
                    ////-100
                    //else
                    //{
                    //    if(dateOfNow != new DateTime())
                    //    {
                    //        DateTime calc_date = dateOfSigning.AddDays(value - daysLeft);
                    //        DateTime end_date = calc_date.AddYears(amount);
                    //        //если окончание стало меньше чем сейчас.. то надо:
                    //        //уменьшить срок контракта.. если после этого все равно меньше то
                    //        //
                    //        if (end_date < dateOfNow.AddDays(1))
                    //        {
                                
                    //        }
                    //    }
                    //}
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
