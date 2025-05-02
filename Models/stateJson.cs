using PropertyChanged;
using System.Collections.ObjectModel;

namespace HollyJson.Models
{
    [AddINotifyPropertyChangedInterface]
    public class TagPool
    {
        public string Item1 { get; set; }
        public DateTime Item2 { get; set; }
        public TagPool()
        {
            Item1 = string.Empty;
            Item2 = new DateTime();
        }
        public TagPool(string item1, DateTime item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
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
        public DateTime Now
        {
            get
            {
                int days = 0;
                int.TryParse(timePassed.Split('.')[0], out days); //дни есть
                return !string.IsNullOrEmpty(timePassed) ? GameStartTime.AddDays(days) : GameStartTime;
            }
        }
        public ObservableCollection<Character> characters { get; set; }

        public Dictionary<string, DateTime> NextSpawnDays { get; set; }
        public ObservableCollection<Milestones> milestones { get; set; }
        private int valOfActivePolicy;
        public int ValOfActivePolicy
        {
            get
            {
                return valOfActivePolicy;
            }
            set
            {
                if (milestones is null)
                {
                    valOfActivePolicy = 0;
                }
                else
                {
                    var curr = milestones.Where(t => t.id.Contains(NameOfActivePolicy));
                    if (curr.Count() > 0)
                    {
                        var q = curr.Single(t => t.Inner_id == value);
                        q.finished = true;
                        q.progress = 1.0;
                        //если 0 - 1 - 2
                        if (value < 3)
                        {
                            curr.Single(t => t.Inner_id == value + 1).locked = false;
                            curr.Single(t => t.Inner_id == value + 1).progress = 0.0d;
                            curr.Single(t => t.Inner_id == value + 1).finished = false;
                        }
                        //для тех кто дальше одного шага вверх locked = true;
                        foreach (var item in curr.Where(t => t.Inner_id > value + 1))
                        {
                            item.locked = true;
                            item.progress = 0.0d;
                            item.finished = false;
                        }
                        valOfActivePolicy = value;
                    }
                    else
                        valOfActivePolicy = 0;
                }
            }
        }
        public bool HaveActivePolicy
        {
            get
            {
                if (milestones is not null)
                    return milestones.Where(t => t.finished).Count() > 0;
                else
                    return false;
            }
        }
        public string NameOfActivePolicy
        {
            get
            {
                if (milestones is not null)
                {
                    if (milestones.Where(t => t.finished).Count() > 0)
                    {
                        return milestones.MaxBy(t => t.finished)?.Inner_name!;
                    }
                    else
                        return "NONE";
                }
                else
                    return "NONE";
            }
        }
        public ObservableCollection<string> AvailablePerks { get; set; }

        public ObservableCollection<string> openedPerks { get; set; }
        /// <summary>
        /// Закрытые тэги
        /// </summary>
        public ObservableCollection<string> tagBank { get; set; }
        /// <summary>
        /// Открытые тэги
        /// </summary>
        public ObservableCollection<TagPool> tagPool { get; set; }
        public static List<string> PreGenPerks => new List<string>()
        {
            "BANK_LOAN",
            "BLDG_CONSTRUCTOR",
            "PROFITABLE_MOVIE_REP_2",
            "BLDG_ESCORT_DOMINION",
            "LEGAL_DEFENSE_1",
            "BLDG_WATER_TOWER_I",
            "BLDG_DISTRIBUTION",
            "POST_DIR_MONT_COMP_XP_1",
            "CASH_FLOW_1",
            "TAGS_RESEARCH",
            "GOOD_ATTITUDE_REP_1",
            "IMPROVEMENT_0_NO_SADNESS",
            "LEGAL_DEFENSE_2",
            "BLDG_POWERPLANT_I",
            "BLDG_ANALYTICS",
            "BLDG_LAB",
            "CASH_FLOW_2",
            "MOVIE_RELEASE_XP_1",
            "GOOD_ATTITUDE_REP_2",
            "HIRING_BONUSES",
            "LEGAL_DEFENSE_3",
            "REPAIR_TEAM_1",
            "ANALYSIS_GROUPS",
            "LAB_INHOUSE_IMPROVED",
            "BLDG_CASTING",
            "BANK_LOAN_EARLY_REPAYMENT",
            "PROD_DIR_CIN_ACT_XP_1",
            "NEW_TAG_BY_LT_1",
            "ICON_REP_1",
            "MOVIE_RELEASE_MOOD_BOOST",
            "CONTRACT_PAYMENTS_50_50",
            "IMPROVEMENT_I",
            "MOVIEGOERS_NUMBER_WIDE",
            "LAB_INHOUSE_TIME_1",
            "BLDG_WORKSHOP",
            "BANK_LOAN_INT_RATE_REDUCTION_1",
            "BLDG_LOGISTICS",
            "NEW_TAG_BY_LT_2",
            "SKILLED_ACTOR_REP",
            "PERSONNEL_X2",
            "CONTRACT_5_YEARS",
            "MOVIEGOERS_NUMBER_NARROW",
            "BLDG_SOUND",
            "PREPROD_PROD_DIR_CIN_XP_1",
            "TWO_PROJECTS",
            "BANK_LOAN_INT_RATE_REDUCTION_2",
            "BLDG_PAVILION_II",
            "PASSIVE_PROTECTION_1",
            "NEW_TAG_BY_LT_3",
            "LEGEND_REP_1",
            "BAD_ATTITUDE_NO_SADNESS",
            "CONTRACT_10_YEARS",
            "BLDG_MARKETING",
            "SOUND_INHOUSE_IMPROVED",
            "PROPS_QLT_2",
            "CONTRACT_WEIGHT",
            "BANK_LOAN_AMOUNT_1",
            "WG_WATCHES",
            "BLDG_LINE_PRODUCTION",
            "PASSIVE_PROTECTION_2",
            "EDITS_ON_GO",
            "CHARITY_TO_REP",
            "NOMINATION_LOSS_NO_SADNESS",
            "CONTRACT_TERMINATION_FEE_1",
            "SCANDAL_COVER_UP_MONEY",
            "SOUND_INHOUSE_TIME_1",
            "BANK_LOAN_AMOUNT_2",
            "SETS_QLT_2",
            "INSURANCE_PLUS",
            "SECOND_UNIT",
            "PASSIVE_PROTECTION_3",
            "TAGS_SLOTS_6",
            "ETHNIC_COMPOSITION",
            "CONTRACT_TERMINATION_FEE_2",
            "POSTRELEASE_ANALYSIS",
            "BLDG_CONCERT",
            "BANK_LOAN_TERM_1",
            "BLDG_SUPPLY",
            "HOUSEMAID",
            "URGENT_DOUBLE_SEARCH",
            "ACTIVE_PROTECTION",
            "SCREENPLAY_TIME_RED_1",
            "ILLEGAL_WORKERS",
            "CONTRACT_TERMINATION_FREE",
            "BLDG_PRINT",
            "CONCERT_INHOUSE_MPROVED",
            "BANK_LOAN_TERM_2",
            "PROPS_QLT_3",
            "HOTEL_SUITE",
            "FLEX_SCHEDULE",
            "ACTIVE_PROTECTION_XP_BONUS_1",
            "NEW_SCREENPLAY_XP_BONUS_1",
            "CHEAP_ILLEGALS",
            "CONTRACT_5_MOVIES",
            "PRINT_INHOUSE_QLT_1",
            "CONCERT_INHOUSE_TIME_1",
            "SETS_QLT_3",
            "CHEF",
            "TEAM_SERVICE_1",
            "CONTRACT_10_MOVIES",
            "ACTIVE_PROTECTION_XP_BONUS_2",
            "NEW_SCREENPLAY_XP_BONUS_2",
            "BUILDINGS_CONSERVATION",
            "PRINT_INHOUSE_QLT_2",
            "BLDG_RND_I",
            "SETS_TIME_RED_1",
            "ASSISTANT",
            "TEAM_SERVICE_2",
            "BLDG_SPIES",
            "NEW_SCREENPLAY_XP_BONUS_3",
            "SALARY_CUT",
            "WM_HOSPICE",
            "BLDG_RND_II",
            "SETS_TIME_RED_2",
            "SPOUSES_ASSISTANT",
            "URGENT_LOCATION_SEARCH",
            "SCREENPLAY_TIME_RED_2",
            "CONSERVATION_COOLDOWN",
            "ANALYSIS_VISION_DEPTH_1",
            "FAIL_NO_DISCLOSURE",
            "STUDIO_TECH",
            "PREPROD_PROD_DIR_CIN_XP_2",
            "BUTLER",
            "URGENT_EXTRAS_SEARCH",
            "SCREENPLAY_TIME_RED_3",
            "ANALYSIS_ENTIRE_CAST",
            "SPYING_LVL_2",
            "STUDIO_TECH_RED_TIME_1",
            "BLDG_SCOUT",
            "NANNY",
            "URGENT_CREW_SEARCH",
            "TAGS_SLOTS_7",
            "SCANDAL_COVER_UP_PP",
            "SPYING_XP_BONUS_1",
            "STUDIO_TECH_RED_TIME_2",
            "LOCATION_SEARCH_WORLD",
            "PENTHOUSE",
            "BLDG_PAVILION_III",
            "NEW_SCREENPLAY_PP_BONUS_1",
            "ANALYSIS_TAGS",
            "SPYING_XP_BONUS_2",
            "STUDIO_TECH_ADD_RND",
            "LOCATION_QLT_1",
            "PERSONAL_DRIVER",
            "BLDG_PAVILION_IV",
            "NEW_SCREENPLAY_PP_BONUS_2",
            "ANALYSIS_SCREENPLAY",
            "BLDG_SHENANIGANS",
            "BLDG_RND_III",
            "LOCATION_SEARCH_TIME_1",
            "PERSONAL_DRIVER_PREMIUM",
            "SCEN_IDEAS_STORAGE_1",
            "PRINT_EMERGENCY",
            "SHENANIGANS_KIDNAPPING",
            "BLDG_RND_IV",
            "LOCATION_SEARCH_TIME_2",
            "VILLA",
            "SCEN_IDEAS_GEN_AMT_1",
            "ANALYSIS_BUDGET",
            "SHENANIGANS_MURDER",
            "LOCATION_QLT_2",
            "WG_ALCOHOL",
            "SCEN_IDEAS_GEN_AMT_2",
            "ANALYSIS_VISION_DEPTH_2",
            "SETS_TIME_RED_3",
            "WG_CIGARS",
            "MOVIE_SEQUEL",
            "WM_ORPHANAGE",
            "EXTRAS_2",
            "WG_HAUTE_WARDROBE",
            "MOVIE_RELEASE_XP_2",
            "WM_WEDDING",
            "EXTRAS_3",
            "WG_SPORTCAR",
            "MOVIE_RELEASE_XP_3",
            "WM_HOMELESS",
            "EXTRAS_4",
            "MOVIE_SEQUEL_ORIGINALITY",
            "WM_DEBT",
            "MOVIE_SEQUEL_LEGACY",
            "TAGS_RESEARCH_DIRECTION",
            "TAGS_RESEARCH_TIME_RED_1",
            "TAGS_XP_BONUS_1",
            "TAGS_XP_BONUS_2",
            "TAGS_XP_BONUS_3",
            "TAGS_RESEARCH_TIME_RED_2",
            "TAGS_RESEARCH_TIME_RED_3",
            "TAGS_NEW_PP_BONUS",
            "BLDG_FREELANCE",
            "SCRIPT_DOCTORS",
            "SCRIPT_DOCTORS_CHEAPER",
            "SCRIPT_DOCTORS_FASTER",
            "SCRIPT_DOCTORS_RANGE",
            "SCRIPT_DOCTORS_SCORES",
            "TAGS_SLOTS_8",
            "TAGS_SLOTS_9",
            "TAGS_SLOTS_10",
            "BG_UNLOCK",
            "BM_UNLOCK",
            "BG_NARCOTICS",
            "BM_DROWNING",
            "BG_NARCOTICS_2",
            "BM_DRUNKARD",
            "BG_XXX",
            "BM_FIGHT",
            "BG_BRAINS",
            "BM_CRIMINAL",
            "BG_SAFARI",
            "BM_HOUSE_BURN",
            "BG_KILLING",
            "BG_CANNIBAL",
            "BG_UNDERAGE"
        };

    }
}
