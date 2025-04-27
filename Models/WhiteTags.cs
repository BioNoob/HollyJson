using PropertyChanged;
namespace HollyJson.Models
{
    public enum Skills
    {
        Romance,
        Drama,
        Thriller,
        Comedy,
        Detective,
        Action,
        Historical,
        Adventure,
        Indoor,
        Outdoor,
        COM,
        ART,
        ELSE

    }

    [AddINotifyPropertyChangedInterface]
    public class OverallValue
    {
        public int movieId { get; set; }
        public int sourceType { get; set; }
        public double value { get; set; }
        public DateTime dateAdded { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class WhiteTag
    {
        private string id1;

        public List<OverallValue> overallValues { get; set; }
        public string id
        {
            get => id1;
            set
            {
                id1 = value;
                switch (value)
                {
                    case "ROMANCE":
                        Tagtype = Skills.Romance;
                        break;
                    case "DRAMA":
                        Tagtype = Skills.Drama;
                        break;
                    case "THRILLER":
                        Tagtype = Skills.Thriller;
                        break;
                    case "COMEDY":
                        Tagtype = Skills.Comedy;
                        break;
                    case "DETECTIVE":
                        Tagtype = Skills.Detective;
                        break;
                    case "ACTION":
                        Tagtype = Skills.Action;
                        break;
                    case "HISTORICAL":
                        Tagtype = Skills.Historical;
                        break;
                    case "ADVENTURE":
                        Tagtype = Skills.Adventure;
                        break;
                    case "INDOOR":
                        Tagtype = Skills.Indoor;
                        break;
                    case "OUTDOOR":
                        Tagtype = Skills.Outdoor;
                        break;
                    case "COM":
                        Tagtype = Skills.COM;
                        break;
                    case "ART":
                        Tagtype = Skills.ART;
                        break;
                    default:
                        Tagtype = Skills.ELSE;
                        break;

                }

            }
        }
        public DateTime dateAdded { get; set; }
        public int movieId { get; set; }
        public double value { get; set; }
        public bool IsOverall { get; set; }
        public Skills Tagtype { get; set; }
        public WhiteTag()
        {
            dateAdded = stateJson.GameStartTime;
            movieId = 0;
            value = 0d;
            IsOverall = false;
            overallValues = new List<OverallValue>();
            id = "";
        }
        //constr for add new
        public WhiteTag(string idd, double val) : base()
        {
            id = idd;
            value = val;
            overallValues.Add(new OverallValue() { movieId = 0, dateAdded = dateAdded, value = val, sourceType = 0 });
        }
    }
}