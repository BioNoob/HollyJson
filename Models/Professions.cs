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
        public Profession GetProfession
        {
            get
            {
                if (Actor is not null)
                    return Profession.Actor;
                if (Scriptwriter is not null)
                    return Profession.Scriptwriter;
                if (Composer is not null)
                    return Profession.Composer;
                if (FilmEditor is not null)
                    return Profession.FilmEditor;
                if (Producer is not null)
                    return Profession.Producer;
                if (Cinematographer is not null)
                    return Profession.Cinematographer;
                if (Director is not null)
                    return Profession.Director;
                if (Agent is not null)
                    return Profession.Agent;
                return Profession.Else;
            }
        }
        public double? Actor { get; set; }
        public double? Scriptwriter { get; set; }
        public double? Composer { get; set; }
        public double? FilmEditor { get; set; }
        public double? Producer { get; set; }
        public double? Cinematographer { get; set; }
        public double? Director { get; set; }
        public double? Agent { get; set; }
        public double? SetterVal
        {
            get
            {
                switch (GetProfession)
                {
                    case Profession.Actor:
                        return Actor;
                    case Profession.Scriptwriter:
                        return Scriptwriter;
                    case Profession.Composer:
                        return Composer;
                    case Profession.FilmEditor:
                        return FilmEditor;
                    case Profession.Producer:
                        return Producer;
                    case Profession.Cinematographer:
                        return Cinematographer;
                    case Profession.Director:
                        return Director;
                    case Profession.Agent:
                        return Agent;
                }
                return null;
            }
            set
            {
                switch (GetProfession)
                {
                    case Profession.Actor:
                        Actor = value;
                        break;
                    case Profession.Scriptwriter:
                        Scriptwriter = value;
                        break;
                    case Profession.Composer:
                        Composer = value;
                        break;
                    case Profession.FilmEditor:
                        FilmEditor = value;
                        break;
                    case Profession.Producer:
                        Producer = value;
                        break;
                    case Profession.Cinematographer:
                        Cinematographer = value;
                        break;
                    case Profession.Director:
                        Director = value;
                        break;
                    case Profession.Agent:
                        Agent = value;
                        break;
                }
            }
        }
        public string ProfAsString
        {
            get
            {
                switch (GetProfession)
                {
                    case Profession.Actor:
                        return "Actor";
                    case Profession.Scriptwriter:
                        return "Scriptwriter";
                    case Profession.Composer:
                        return "Composer";
                    case Profession.FilmEditor:
                        return "FilmEditor";
                    case Profession.Producer:
                        return "Producer";
                    case Profession.Cinematographer:
                        return "Cinematographer";
                    case Profession.Director:
                        return "Director";
                    case Profession.Agent:
                        return "Agent";
                    case Profession.Else:
                        return "ELSE";
                }
                return "ELSE";
            }
        }
        public override string ToString()
        {
            return $"{ProfAsString}: {SetterVal?.ToString("0.0", CultureInfo.InvariantCulture)}";
        }

    }
}
