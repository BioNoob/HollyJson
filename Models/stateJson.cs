using PropertyChanged;
using System.Collections.ObjectModel;

namespace HollyJson
{
    [AddINotifyPropertyChangedInterface]
    public class stateJson
    {
        public static DateTime GameStartTime => new DateTime(1929, 1, 1);
        public int budget { get; set; }
        public int cash { get; set; }
        public double reputation { get; set; }
        public int influence { get; set; }
        public string studioName { get; set; }
        public string timePassed { get; set; }
        public DateTime Now => !string.IsNullOrEmpty(timePassed) ? GameStartTime.AddDays(int.Parse(timePassed.Split('.')[0])) : GameStartTime;
        public ObservableCollection<Character> characters { get; set; }
        public Dictionary<string,DateTime> NextSpawnDays { get; set; }
        //public ObservableCollection<Character> Mycharacters { get; set; }// => new ObservableCollection<Character>(characters.Where(t => t.studioId == "PL" && t.professions.GetProfession != Professions.Profession.Else).ToList());

    }
}
