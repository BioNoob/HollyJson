using PropertyChanged;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Building
    {
        public string id { get; set; }
        public int baseDuration { get; set; }
        public int baseWater { get; set; }
        public int baseElectricity { get; set; }
        public int baseCost { get; set; }
        public int staff { get; set; }
    }
}
