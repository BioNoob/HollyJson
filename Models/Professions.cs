using PropertyChanged;
using System.Globalization;

namespace HollyJson
{
    [AddINotifyPropertyChangedInterface]
    public class Professions
    {
        public enum Profession
        {
            Actor,
            Scriptwriter,
            Composer,
            FilmEditor,
            Producer,
            Cinematographer,
            Director,
            Agent,
            Else
        }
        public string Name { get; set; }
        public double Value { get; set; }
        public string ProfToDecode
        {
            get
            {
                string pr = "PROFESSION_";
                switch (Name)
                {
                    case "Actor":
                    case "Composer":
                    case "Scriptwriter":
                    case "FilmEditor":
                    case "Producer":
                    case "Director":
                    case "Agent":
                    case "LieutScript":
                    case "LieutPrep":
                    case "LieutProd":
                    case "LieutPost":
                    case "LieutRelease":
                    case "LieutSecurity":
                    case "LieutProducers":
                    case "LieutInfrastructure":
                    case "LieutTech":
                    case "LieutMuseum":
                    case "LieutEscort":
                    case "CptHR":
                    case "CptLawyer":
                    case "CptFinancier":
                    case "CptPR":
                        return pr + Name.ToUpper();
                    default:
                        return pr + "NONE";
                }
            }
        }
        public Profession GetProfession
        {
            get
            {
                //if (Actor is not null)
                //    return Profession.Actor;
                //if (Scriptwriter is not null)
                //    return Profession.Scriptwriter;
                //if (Composer is not null)
                //    return Profession.Composer;
                //if (FilmEditor is not null)
                //    return Profession.FilmEditor;
                //if (Producer is not null)
                //    return Profession.Producer;
                //if (Cinematographer is not null)
                //    return Profession.Cinematographer;
                //if (Director is not null)
                //    return Profession.Director;
                //if (Agent is not null)
                //    return Profession.Agent;
                return Profession.Else;
            }
        }
        //public double? Actor { get; set; }
        //public double? Scriptwriter { get; set; }
        //public double? Composer { get; set; }
        //public double? FilmEditor { get; set; }
        //public double? Producer { get; set; }
        //public double? Cinematographer { get; set; }
        //public double? Director { get; set; }
        //public double? Agent { get; set; }
        //public double? SetterVal
        //{
        //    get
        //    {
        //        switch (GetProfession)
        //        {
        //            case Profession.Actor:
        //                return Actor;
        //            case Profession.Scriptwriter:
        //                return Scriptwriter;
        //            case Profession.Composer:
        //                return Composer;
        //            case Profession.FilmEditor:
        //                return FilmEditor;
        //            case Profession.Producer:
        //                return Producer;
        //            case Profession.Cinematographer:
        //                return Cinematographer;
        //            case Profession.Director:
        //                return Director;
        //            case Profession.Agent:
        //                return Agent;
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        switch (GetProfession)
        //        {
        //            case Profession.Actor:
        //                Actor = value;
        //                break;
        //            case Profession.Scriptwriter:
        //                Scriptwriter = value;
        //                break;
        //            case Profession.Composer:
        //                Composer = value;
        //                break;
        //            case Profession.FilmEditor:
        //                FilmEditor = value;
        //                break;
        //            case Profession.Producer:
        //                Producer = value;
        //                break;
        //            case Profession.Cinematographer:
        //                Cinematographer = value;
        //                break;
        //            case Profession.Director:
        //                Director = value;
        //                break;
        //            case Profession.Agent:
        //                Agent = value;
        //                break;
        //        }
        //    }
        //}
        //public string PropertyProf { get; set; }
        //public string ProfToDecode
        //{
        //    get
        //    {
        //        const string pr = "PROFESSION_";
        //        switch (GetProfession)
        //        {
        //            case Profession.Actor:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Scriptwriter:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Composer:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.FilmEditor:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Producer:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Cinematographer:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Director:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Agent:
        //                return pr + ProfAsString.ToUpper();
        //            case Profession.Else:
        //            default:
        //                return pr + ProfAsString.ToUpper();
        //        }
        //    }
        //}
        //public string ProfAsString
        //{
        //    get
        //    {
        //        switch (GetProfession)
        //        {
        //            case Profession.Actor:
        //                return "Actor";
        //            case Profession.Scriptwriter:
        //                return "Scriptwriter";
        //            case Profession.Composer:
        //                return "Composer";
        //            case Profession.FilmEditor:
        //                return "FilmEditor";
        //            case Profession.Producer:
        //                return "Producer";
        //            case Profession.Cinematographer:
        //                return "Cinematographer";
        //            case Profession.Director:
        //                return "Director";
        //            case Profession.Agent:
        //                return "Agent";
        //            case Profession.Else:
        //                return "ELSE";
        //        }
        //        return "ELSE";
        //    }
        //}
        //public override string ToString()
        //{
        //    return $"{ProfAsString}: {SetterVal?.ToString("0.0", CultureInfo.InvariantCulture)}";
        //}

    }
}
