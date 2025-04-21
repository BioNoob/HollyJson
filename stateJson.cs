using PropertyChanged;
using System.Collections.ObjectModel;

namespace HollyJson
{
    //              stateJson.logs."timestamp": "1935-01-28T00:00:00",
    [AddINotifyPropertyChangedInterface]
    public class stateJson
    {
        public int budget { get; set; }
        public int cash { get; set; }
        public double reputation { get; set; }
        public int influence { get; set; }
        public string studioName { get; set; }
        public string timePassed { get; set; }
        public DateTime Now => !string.IsNullOrEmpty(timePassed) ? new DateTime(1929, 1, 1).AddDays(int.Parse(timePassed.Split('.')[0])) : new DateTime(1929, 1, 1);
        public ObservableCollection<Character> characters { get; set; }
        public ObservableCollection<Character> Mycharacters { get; set; }// => new ObservableCollection<Character>(characters.Where(t => t.studioId == "PL" && t.professions.GetProfession != Professions.Profession.Else).ToList());

    }
}
