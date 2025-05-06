using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class CharXp
    {
        public int Id { get; set; }
        public int xpType { get; set; }
        public List<Dictionary<string,int>> xpForProfession { get; set; }
        public void SetXpMod(double coef)
        {
            foreach (var item in xpForProfession)
            {
                foreach (var prof in item)
                {
                    item[prof.Key] = (int)Math.Ceiling(prof.Value * coef);
                }
            }
        }
    }
}
