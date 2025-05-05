using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollyJson.Models
{
    public class Trigger
    {
        public string name { get; set; }
        public string value { get; set; }
        public int triggerType { get; set; }
        public int buffType { get; set; }
        public int duration { get; set; }
        public double probability { get; set; }
        public int delay { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class BuildingInfo
    {
        public int domain { get; set; }
        public string department { get; set; }
        public List<string> unlockedByPerks { get; set; }
        public List<string> dependsOnBuildings { get; set; }
        public bool hasHiddenObjects { get; set; }
        public int duration { get; set; }
        public int stuff { get; set; }
        public double electricity { get; set; }
        public double water { get; set; }
        public List<Trigger> triggers { get; set; }
        public int shapeType { get; set; }
        public int behaviour { get; set; }
        public string id { get; set; }
    }
}
