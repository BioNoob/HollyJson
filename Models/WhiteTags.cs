using Newtonsoft.Json;
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
        private double value1;

        public int movieId { get; set; }
        public int sourceType { get; set; }
        public double value { get => value1; set => value1 = Math.Round(value, 7, MidpointRounding.AwayFromZero); }
        public DateTime dateAdded { get; set; }
        public static bool operator ==(OverallValue a, OverallValue b)
        {
            return
            b.movieId == a.movieId &
            b.sourceType == a.sourceType &
            b.value == a.value &
            b.dateAdded == a.dateAdded;
        }
        public static bool operator !=(OverallValue a, OverallValue b)
        {
            return !(a == b);
        }
        public override bool Equals(object? obj)
        {
            if (obj is not null)
            {
                return (obj as OverallValue)! == this;
            }
            else
                return false;
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class WhiteTag
    {
        private string id1;
        private double value1;

        public List<OverallValue> overallValues { get; set; }
        [JsonIgnore]
        public OverallValue? ZeroPoint
        {
            get
            {
                if (overallValues is not null)
                    return overallValues.SingleOrDefault(t => t.movieId == 0 && t.sourceType == 0);
                else
                    return null;
            }
        }
        [JsonIgnore]
        public double FreeToOverAllZero => value - Math.Round(overallValues.Sum(t => t.value), 2, MidpointRounding.AwayFromZero);
        public static Skills GetEnumVal(string val)
        {
            Skills Tagtype = Skills.ELSE;
            switch (val)
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
            return Tagtype;
        }

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
        public double value
        {
            get => value1; set
            {
                value1 = value;
                if (ZeroPoint is not null)
                    ZeroPoint.value = FreeToOverAllZero;
            }
        }
        public bool IsOverall { get; set; }
        [JsonIgnore]
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
        public WhiteTag(string idd, double val)
        {
            id = idd;
            value = val;
            dateAdded = stateJson.GameStartTime;
            movieId = 0;
            overallValues = [new OverallValue() { movieId = 0, dateAdded = dateAdded, value = val, sourceType = 0 }];
        }
        public static bool operator ==(WhiteTag a, WhiteTag b)
        {
            if (a is null) return b is null;
            if (b is null) return a is null;
            return
            b.id == a.id &
            b.dateAdded == a.dateAdded &
            b.movieId == a.movieId &
            b.value == a.value &
            b.IsOverall == a.IsOverall &
            a.overallValues.SequenceEqual(b.overallValues);
        }
        public static bool operator !=(WhiteTag a, WhiteTag b)
        {
            return !(a == b);
        }
        public override bool Equals(object? obj)
        {
            if (obj is not null)
            {
                return (obj as WhiteTag)! == this;
            }
            else
                return false;
        }
    }
}