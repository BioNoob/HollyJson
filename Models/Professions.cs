using HollyJson.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Professions
    {
        private double _value;

        public enum Profession
        {
            Actor,
            Composer,
            Scriptwriter,
            Cinematographer,
            FilmEditor,
            Producer,
            Director,
            Agent,
            LieutScript,
            LieutPrep,
            LieutProd,
            LieutPost,
            LieutRelease,
            LieutSecurity,
            LieutProducers,
            LieutInfrastructure,
            LieutTech,
            LieutMuseum,
            LieutEscort,
            CptHR,
            CptLawyer,
            CptFinancier,
            CptPR,
            Else
        }
        public string Name { get; set; }
        [JsonConverter(typeof(DoubleJsonConverter))]
        public double Value { get => _value; set { _value = value;  IsValChanged?.Invoke(_value); } }
        public delegate void ValChanged(double val);
        public event ValChanged IsValChanged;

        public string ProfToDecode
        {
            get
            {
                string pr = "PROFESSION_";
                if (GetProfession != Profession.Else)
                    return pr + Name.ToUpper();
                else
                    return pr + "NONE";
            }
        }
        public Profession GetProfession
        {
            get
            {
                switch (Name)
                {
                    case "Actor":
                        return Profession.Actor;
                    case "Composer":
                        return Profession.Composer;
                    case "Scriptwriter":
                        return Profession.Scriptwriter;
                    case "Cinematographer":
                        return Profession.Cinematographer;
                    case "FilmEditor":
                        return Profession.FilmEditor;
                    case "Producer":
                        return Profession.Producer;
                    case "Director":
                        return Profession.Director;
                    case "Agent":
                        return Profession.Agent;
                    case "LieutScript":
                        return Profession.LieutScript;
                    case "LieutPrep":
                        return Profession.LieutPrep;
                    case "LieutProd":
                        return Profession.LieutProd;
                    case "LieutPost":
                        return Profession.LieutPost;
                    case "LieutRelease":
                        return Profession.LieutRelease;
                    case "LieutSecurity":
                        return Profession.LieutSecurity;
                    case "LieutProducers":
                        return Profession.LieutProducers;
                    case "LieutInfrastructure":
                        return Profession.LieutInfrastructure;
                    case "LieutTech":
                        return Profession.LieutTech;
                    case "LieutMuseum":
                        return Profession.LieutMuseum;
                    case "LieutEscort":
                        return Profession.LieutEscort;
                    case "CptHR":
                        return Profession.CptHR;
                    case "CptLawyer":
                        return Profession.CptLawyer;
                    case "CptFinancier":
                        return Profession.CptFinancier;
                    case "CptPR":
                        return Profession.CptPR;
                    default:
                        return Profession.Else;
                }
            }

        }
        public bool IsTalent
        {
            get
            {
                switch (GetProfession)
                {
                    case Profession.Actor:
                    case Profession.Composer:
                    case Profession.Scriptwriter:
                    case Profession.Cinematographer:
                    case Profession.FilmEditor:
                    case Profession.Producer:
                    case Profession.Director:
                    case Profession.Agent:
                        return true;
                    case Profession.LieutScript:
                    case Profession.LieutPrep:
                    case Profession.LieutProd:
                    case Profession.LieutPost:
                    case Profession.LieutRelease:
                    case Profession.LieutSecurity:
                    case Profession.LieutProducers:
                    case Profession.LieutInfrastructure:
                    case Profession.LieutTech:
                    case Profession.LieutMuseum:
                    case Profession.LieutEscort:
                    case Profession.CptHR:
                    case Profession.CptLawyer:
                    case Profession.CptFinancier:
                    case Profession.CptPR:
                    case Profession.Else:
                    default:
                        return false;
                }
            }
        }

        public static bool operator ==(Professions a, Professions b)
        {
            if (a is null) return b is null;
            if (b is null) return a is null;
            return
            b.Name == a.Name &
            b.Value == a.Value;
        }
        public static bool operator !=(Professions a, Professions b)
        {
            return !(a == b);
        }
        public override bool Equals(object? obj)
        {
            if (obj is not null)
            {
                return (obj as Professions)! == this;
            }
            else
                return false;
        }
    }
}
