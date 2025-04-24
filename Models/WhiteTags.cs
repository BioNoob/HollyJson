using PropertyChanged;
namespace HollyJson
{
    public enum Tags
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
                        Tagtype = Tags.Romance;
                        break;
                    case "DRAMA":
                        Tagtype = Tags.Drama;
                        break;
                    case "THRILLER":
                        Tagtype = Tags.Thriller;
                        break;
                    case "COMEDY":
                        Tagtype = Tags.Comedy;
                        break;
                    case "DETECTIVE":
                        Tagtype = Tags.Detective;
                        break;
                    case "ACTION":
                        Tagtype = Tags.Action;
                        break;
                    case "HISTORICAL":
                        Tagtype = Tags.Historical;
                        break;
                    case "ADVENTURE":
                        Tagtype = Tags.Adventure;
                        break;
                    case "INDOOR":
                        Tagtype = Tags.Indoor;
                        break;
                    case "OUTDOOR":
                        Tagtype = Tags.Outdoor;
                        break;
                    case "COM":
                        Tagtype = Tags.COM;
                        break;
                    case "ART":
                        Tagtype = Tags.ART;
                        break;
                    default:
                        Tagtype = Tags.ELSE;
                        break;

                }

            }
        }
        public DateTime dateAdded { get; set; }
        public int movieId { get; set; }
        public double value { get; set; }
        public bool IsOverall { get; set; }
        public Tags Tagtype { get; set; }
    }
}