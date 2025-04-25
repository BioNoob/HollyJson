using Newtonsoft.Json;
using PropertyChanged;
using System.Globalization;
using System.Collections.ObjectModel;

namespace HollyJson
{
    [AddINotifyPropertyChangedInterface]
    public class Character
    {
        private int age;
        private string birthDate1;
        private string normalFirst1;
        private string normalLast1;
        private string customName1;
        private bool calcages = false;
        private string? myCustomName = null;
        public double limit { get; set; }
        public double mood { get; set; }
        public double attitude { get; set; }
        public int id { get; set; }
        public int portraitBaseId { get; set; }
        public string firstNameId { get; set; }
        public string normalFirst
        {
            get
            {
                if (string.IsNullOrWhiteSpace(normalFirst1))
                    return firstNameId;
                else
                    return normalFirst1;
            }
            set => normalFirst1 = value;
        }
        public string lastNameId { get; set; }
        public string normalLast
        {
            get
            {
                if (string.IsNullOrWhiteSpace(normalLast1))
                    return lastNameId;
                else
                    return normalLast1;
            }
            set => normalLast1 = value;
        }
        public string MyCustomName
        {
            get
            {
                if (myCustomName is null)
                    return customName;
                else
                    return myCustomName;
            }
            set
            {
                myCustomName = value;
            }
        }
        public string customName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(customName1))
                    return $"{normalFirst} {normalLast}";
                else
                    return customName1;
            }
            set { customName1 = value; }
        }
        public bool CustomNameWasSetted => MyCustomName != customName;
        public string birthDate
        {
            get => birthDate1;
            set
            {
                birthDate1 = value;
            }
        } //"10-04-1902",
        public DateTime GetBirthDate => DateTime.ParseExact(birthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        public int Age
        {
            get => age;
            set
            {
                if (!calcages)
                    birthDate = GetBirthDate.AddYears(age - value).ToString("dd-MM-yyyy");
                else
                    calcages = false;
                age = value;
            }
        }
        [JsonIgnore]
        public ObservableCollection<WhiteTag> whiteTagsNEW { get; set; }
        //ПРОВЕРИТЬ ЛОКАЛИЗАЦИЮ
        public List<string> aSins { get; set; }
        public List<string> labels { get; set; }

        public void SetFullAge(DateTime now)
        {
            var age = now.Year - GetBirthDate.Year;
            if (GetBirthDate.Date > now.AddYears(-age)) age--;
            calcages = true;
            Age = age;
        }
        public string? studioId { get; set; }

        public Contract? contract { get; set; }
        [JsonIgnore]
        public Professions professions { get; set; }
        public string JsonString { get; set; }
        public bool WasChanged
        {
            get
            {
                var q = JsonConvert.DeserializeObject<Character>(JsonString);
                bool t1 =
                    limit == q.limit &&
                    mood == q.mood &&
                    attitude == q.attitude &&
                    birthDate.Equals(q.birthDate);

                t1 &= !CustomNameWasSetted;

                bool t2 = true;
                    //professions.SetterVal == q.professions.SetterVal;
                var cntr = q.contract;
                if (q.contract is not null)
                {
                    bool t3 =
                        contract.weightToSalary == cntr.weightToSalary &&
                        contract.monthlySalary == cntr.monthlySalary &&
                        contract.amount == cntr.amount &&
                        contract.startAmount == cntr.startAmount &&
                        contract.dateOfSigning == cntr.dateOfSigning &&
                        contract.initialFee == cntr.initialFee;
                    return !(t1 && t2 && t3);
                }
                else
                    return !(t1 && t2);
            }
        }

    }
}
