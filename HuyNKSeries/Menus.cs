using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using LX_Orbwalker;
using SharpDX;

using Color = System.Drawing.Color;
namespace HuyNKSeries
{
   public class Menus
    {

       public static  Orbwalking.Orbwalker Orbwalker;
       
        public static Menu menu;
        public static Menu  orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");

        public static Obj_AI_Hero player = ObjectManager.Player;
        public static string CHAMP_NAME = player.ChampionName ;
        public static void SetuptMenu()
        {
            Game.PrintChat("=========================");
            Game.PrintChat("|   <font color = \"#FFB6C1\">HuyNK Serier</font> by <font color = \"#00FFFF\">HuyNK</font> |");
            Game.PrintChat("====================================================");
            Game.PrintChat("|<font color = \"#87CEEB\">Feel free to donate via Paypal to:</font> <font color = \"#FFFF00\">khachuyvk@gmail.com</font>|");
            Game.PrintChat("====================================================");



            Menus.menu = new Menu("[HuyNK Series] " + Menus.CHAMP_NAME, "huynks" + Menus.CHAMP_NAME, true);

            //Info
            menu.AddSubMenu(new Menu("Info", "Info"));
            menu.SubMenu("Info").AddItem(new MenuItem("[Author]", "[By HuyNK]"));
            menu.SubMenu("Info").AddItem(new MenuItem("[Paypal]", "[Donate: khachuyvk@gmail.com]"));

            //Target selector
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            menu.AddSubMenu(targetSelectorMenu);
            //Orbwalker
            orbwalkerMenu.AddItem(new MenuItem("Orbwalker_Mode", "Change Orbwalker(Reload)").SetValue(false));
            menu.AddSubMenu(orbwalkerMenu);
            chooseOrbwalker(menu.Item("Orbwalker_Mode").GetValue<bool>());
            //Autolevel
           
           
            //Packet Menu
            menu.AddSubMenu(new Menu("Packet Setting", "Packets"));
            menu.SubMenu("Packets").AddItem(new MenuItem("packet", "Use Packets").SetValue(false));
         // Autolevel.Autolv();
            menu.AddToMainMenu();

            try
            {
                if (Activator.CreateInstance(null, "HuyNKSeries.Champ." + player.ChampionName) != null)
                {
                    Game.PrintChat("<font color = \"#FFB6C1\">HUYNK Series " + player.ChampionName + " Loaded!</font>");
                }
            }
            catch
            {
                Game.PrintChat("HuyNK Religion => {0} Not Support !", player.ChampionName);
            }
        }

        public static void chooseOrbwalker(bool mode)
        {
           if (mode)
            {
                

                HuyNKOrbwalker.AddToMenu(orbwalkerMenu);
               
            }
            else
            {
                Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
                
            }
        }
        }
    }
    

