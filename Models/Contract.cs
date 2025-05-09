using HollyJson.ViewModels;
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
        [JsonIgnore]
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
                if (!IsInit)
                {
                    if (!lockamount)
                    {
                        //менять кол-во дней
                        IsInit = true;
                        DaysLeft = (int)Math.Ceiling((dateOfEnding - dateOfNow).TotalDays);
                        if (DaysLeft < 1)
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
        [JsonConverter(typeof(DoubleJsonConverter))]
        public double initialFee { get; set; }
        [JsonConverter(typeof(DoubleJsonConverter))]
        public double monthlySalary { get; set; }
        [JsonConverter(typeof(DoubleJsonConverter))]
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
        [JsonConverter(typeof(DoubleJsonConverter))]
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

                    var daysbeforenow = (dateOfNow - dateOfSigning).TotalDays; //1089
                    var daysafternow = (dateOfEnding - dateOfNow).TotalDays; //6
                    var differernce = value - DaysLeft; //val = 2560 left = 6 dif = 2554
                    if (differernce > 0) //увеличиваем кол-во дней
                    {
                        if (differernce > daysbeforenow) //не осталось для смещения вперед
                        {
                            //докинули год лет
                            lockamount = true;
                            amount += (int)Math.Ceiling((differernce - daysbeforenow) / 365.2425); //3->8
                            lockamount = false;
                            //сколько всего должно получится дней
                            //var im_need = differernce + daysbeforenow; // 3643
                            //пересчитали новую концову
                            daysafternow = (dateOfSigning.AddYears(amount) - dateOfNow).TotalDays; //1883
                            //определили сколько должно быть до начала контракта


                            var to_move = daysafternow - value; //-1810
                            //var daystoremovefromstart = to_move - daysbeforenow; //-2899
                            dateOfSigning = dateOfSigning.AddDays(-1 * to_move);
                        }
                        else
                        {
                            dateOfSigning = dateOfSigning.AddDays(differernce);
                        }
                    }
                    else //Уменьшаем кол-во дней //-340
                    {
                        if ((daysafternow - differernce) > 0) //10
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
                    if (dateOfEnding <= dateOfNow)
                    {
                        var tt = dateOfNow - dateOfEnding;
                        dateOfSigning = dateOfSigning.AddDays(tt.TotalDays + 1);
                    }
                    if (dateOfSigning >= dateOfNow)
                    {
                        var tt = dateOfSigning - dateOfNow;
                        dateOfSigning = dateOfSigning.AddDays((-1 * tt.TotalDays) - 1);
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
        [JsonConstructor]
        public Contract()
        {
            IsInit = true;
        }
        public Contract(DateTime now, Professions.Profession prof)
        {
            IsInit = true;
            amount = 5;
            startAmount = 5;
            monthlySalary = 0;
            weightToSalary = 100;
            dateOfSigning = now != new DateTime() ? now.AddDays(-1) : now;
            dateOfNow = now;
            SetCalcDaysLeft();
            initialFee = 100;
            switch (prof)
            {
                case Professions.Profession.Agent:
                case Professions.Profession.Composer:
                case Professions.Profession.FilmEditor:
                case Professions.Profession.Scriptwriter:
                    contractType = 2;
                    break;
                case Professions.Profession.Else:
                default:
                    contractType = 0;
                    break;
            }
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
