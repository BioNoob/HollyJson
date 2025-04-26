using PropertyChanged;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Professions
    {
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
        public double Value { get; set; }
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
    }
}
