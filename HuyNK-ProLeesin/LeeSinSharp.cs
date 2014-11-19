using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;
using SharpDX;
using Color = System.Drawing.Color;

/*
 * ToDo:
 * 
 * */


namespace LeeSinSharp
{
    internal class LeeSinSharp
    {
        public static string[] testSpells = { "RelicSmallLantern", "RelicLantern", "SightWard", "wrigglelantern", "ItemGhostWard", "VisionWard",
                                     "BantamTrap", "JackInTheBox","CaitlynYordleTrap", "Bushwhack"};


        

        public const string CharName = "LeeSin";

        public static Menu Config;

        public static Map map;

        public static Obj_AI_Hero target;


        public LeeSinSharp()
        {
            /* CallBAcks */
            CustomEvents.Game.OnGameLoad += onLoad;

        }

        private static void onLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != CharName) return;
            map = new Map();

            Game.PrintChat("HuyNK- Pro LeeSin");

            try
            {

                Config = new Menu("LeeSin", "HuyNK-Pro LeeSin", true);
                var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
                SimpleTs.AddToMenu(targetSelectorMenu);
                Config.AddSubMenu(targetSelectorMenu);

                Config.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
                LeeSin.orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalker"));
                Config.AddSubMenu(new Menu("Combo", "Combo"));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Dung Q")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Dung E")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Dung R")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo And Insec!").SetValue(new KeyBind(32, KeyBindType.Press)));
                Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo1", "QERQE!").SetValue((new KeyBind("S".ToCharArray()[0], KeyBindType.Press, false))));

                //Misc
                Config.AddSubMenu(new Menu("Misc", "Misc"));
                Config.SubMenu("Misc").AddItem(new MenuItem("KhienRadun", "Khien Radun").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("AutoTiamat", "Tiamax").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("Automangxa", "Mang xa").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("AutoBOTRK", "Vo Danh").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("AutoYoumuu", "Kiem Ma Yomun").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("AutoIgnite", "Thieu Dot").SetValue(true));

                Config.AddSubMenu(new Menu("Harass", "Harass"));
                Config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Ria Mau!").SetValue((new KeyBind("C".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("Insec", "Insec"));
                Config.SubMenu("Insec").AddItem(new MenuItem("ActiveInsec", "Insec!").SetValue((new KeyBind("T".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("KillSteal", "KillSteal"));
                Config.SubMenu("KillSteal").AddItem(new MenuItem("UseR", "KS R")).SetValue(true);

                Config.AddSubMenu(new Menu("WardJump", "WardJump"));
                Config.SubMenu("WardJump").AddItem(new MenuItem("ActiveWard", "Luot W").SetValue((new KeyBind("G".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("Drawings", "Drawings"));
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Vong Q")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Vong E")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Vong W")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Vong R")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawInsec", "Huong Insec")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Vong Tron").SetValue(new Slider(100, 100, 10)));
                Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Do day").SetValue(new Slider(1, 10, 1)));
                Config.AddToMainMenu();
                Drawing.OnDraw += onDraw;
                Game.OnGameUpdate += OnGameUpdate;

                GameObject.OnCreate += OnCreateObject;
                GameObject.OnDelete += OnDeleteObject;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;

                LeeSin.setSkillShots();
            }
            catch
            {
            }

        }

        private static void OnGameUpdate(EventArgs args)
        {
            LeeSin.loaidraw();
            LeeSin.CastR_kill();
            target = SimpleTs.GetTarget(1500, SimpleTs.DamageType.Physical);
            LeeSin.checkLock(target);
            LeeSin.orbwalker.SetAttack(true);
            if (Config.Item("ActiveWard").GetValue<KeyBind>().Active)
            {
                LeeSin.wardJump(Game.CursorPos.To2D());
            }

            if (Config.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                LeeSin.doHarass();
            }


            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                LeeSin.combo();
            }
            if (Config.Item("ActiveCombo1").GetValue<KeyBind>().Active)
            {
                LeeSin.combo2();
            }
            if (Config.Item("ActiveInsec").GetValue<KeyBind>().Active)
            {
                LeeSin.useinsec();
            }

            if (LeeSin.orbwalker.ActiveMode.ToString() == "LaneClear")
            {
                
            }
        }

        private static void onDraw(EventArgs args)
        {
            if (Config.Item("DrawQ").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 900, System.Drawing.Color.Chartreuse,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawW").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 700, Color.FromArgb(137, 24, 242),
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawE").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 350, System.Drawing.Color.Fuchsia,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawR").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 375, System.Drawing.Color.Magenta,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawInsec").GetValue<bool>() && LeeSin.R.IsReady())
            {
                if (!LeeSin.loaidraw())
                {
                    Vector2 heroPos = Drawing.WorldToScreen(LeeSin.LockedTarget.Position);
                    Vector2 diempos = Drawing.WorldToScreen(LeeSin.getward1(LeeSin.LockedTarget));
                    Drawing.DrawLine(heroPos[0], heroPos[1], diempos[0], diempos[1], 1, System.Drawing.Color.White);
                }
                else
                {
                    Vector2 heroPos = Drawing.WorldToScreen(LeeSin.LockedTarget.Position);
                    Vector2 diempos = Drawing.WorldToScreen(LeeSin.getward3(LeeSin.LockedTarget));
                    Drawing.DrawLine(heroPos[0], heroPos[1], diempos[0], diempos[1], 1, System.Drawing.Color.White);
                }
            }
        }

        private static void OnCreateObject(GameObject sender, EventArgs args)
        {
            if (sender.Name.Contains("Missile") || sender.Name.Contains("Minion"))
                return;
        }

        private static void OnDeleteObject(GameObject sender, EventArgs args)
        {
          
        }

        public static void OnProcessSpell(LeagueSharp.Obj_AI_Base obj, LeagueSharp.GameObjectProcessSpellCastEventArgs arg)
        {
            if (testSpells.ToList().Contains(arg.SData.Name))
            {
                LeeSin.testSpellCast = arg.End.To2D();
                Polygon pol;
                if ((pol = map.getInWhichPolygon(arg.End.To2D())) != null)
                {
                    LeeSin.testSpellProj = pol.getProjOnPolygon(arg.End.To2D());
                }
            }
        }

     
    }
}
