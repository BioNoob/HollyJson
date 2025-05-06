using PropertyChanged;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    ///Studio politics
    public class Milestones
    {
        public string id { get; set; }
        public string group { get; set; }
        public bool finished { get; set; }
        public bool locked { get; set; }
        public double progress { get; set; }
        public List<object> chains { get; set; }
        public int Inner_id => int.Parse(id.Substring(id.Length - 1, 1));
        public string Inner_name => id.Remove(id.Length - 2, 2);
        public static bool operator ==(Milestones a, Milestones b)
        {
            if (a is null) return b is null;
            if (b is null) return a is null;
            return
            b.id == a.id &
            b.group == a.group &
            b.finished == a.finished &
            b.locked == a.locked &
            b.progress == a.progress;
            //b.chains == a.chains;
        }
        public static bool operator !=(Milestones a, Milestones b)
        {
            return !(a == b);
        }
        public override bool Equals(object? obj)
        {
            if (obj is not null)
            {
                return (obj as Milestones)! == this;
            }
            else
                return false;
        }
    }
}
