using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyNKSeries
{
    class ChampionPriority
    {
        public String[] Champs = 
        { 
            "Aatrox", "Ahri", "Akali", "Alistar", "Amumu", "Anivia", "Annie", "Ashe", "Blitzcrank", "Brand", "Braum", "Caitlyn", "Cassiopeia", "ChoGath", "Corki", 
            "Darius", "Diana", "DrMundo", "Draven", "Elise", "Evelynn", "Ezreal", "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Garen", "Gnar", "Gragas",
            "Graves", "Hecarim", "Heimerdinger", "Irelia", "Janna", "Jarvan IV", "Jax", "Jayce", "Jinx", "Karma", "Karthus", "Kassadin", "Katarina", "Kayle", "Kennen",
            "KhaZix", "KogMaw", "LeBlanc", "Lee Sin", "Leona", "Lissandra", "Lucian", "Lulu", "Lux", "Malphite", "Malzahar", "Maokai", "MasterYi", "MissFortune",
            "Mordekaiser", "Morgana", "Nami", "Nasus", "Nautilus", "Nidalee", "Nocturne", "Nunu", "Olaf", "Orianna", "Pantheon", "Poppy", "Quinn", "Rammus", "Renekton",
            "Rengar", "Riven", "Rumble", "Ryze", "Sejuani", "Shaco", "Shen", "Shyvana", "Singed", "Sion", "Sivir", "Skarner", "Sona", "Soraka", "Swain", "Syndra", "Talon",
            "Taric", "Teemo", "Thresh", "Tristana", "Trundle", "Tryndamere", "TwistedFate", "Twitch", "Udyr", "Urgot", "Varus", "Vayne", "Veigar", "VelKoz", "Vi", "Viktor",
            "Vladimir", "Volibear", "Warwick", "Wukong", "Xerath", "XinZhao", "Yasuo", "Yorick", "Zac", "Zed", "Ziggs", "Zilean", "Zyra" 
        };

        public static String[] mage = 
        {
            "ahri", "anivia", "annie", "brand", "cassiopeia", "elise", "fiddlesticks", "heimerdinger", "karma", "karthus", "kennen", "lissandra", "lux", "malzahar", "morgana", "orianna", "ryze", "swain", "syndra", "twistedfate", "veigar", "velkoz", "viktor", "vladimir", "xerath", "ziggs", "zyra"
        };

        public static String[] marksman = 
        {
            "ashe", "caitlyn", "corki", "draven", "ezreal", "graves", "jinx", "kogmaw", "lucian", "missfortune", "quinn", "sivir", "teemo", "tristana", "twitch", "urgot", "varus", "vayne"
        };

        public static String[] assassin = 
        {
            "akali", "evelynn", "fizz", "kassadin", "katarina", "khazix", "leblanc", "masteryi", "nidalee", "nocturne", "rengar", "shaco", "talon", "zed"
        };
    }
}
