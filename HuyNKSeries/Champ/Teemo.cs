using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace HuyNKSeries.Champ
{
    class Teemo : Champion
    {
        public Teemo()
        {
            SetUpSpells();
            LoadMenu();
        }

        public void SetUpSpells()
        {
            Q = new Spell(SpellSlot.Q, 580);

            W = new Spell(SpellSlot.W);

            E = new Spell(SpellSlot.E);

            R = new Spell(SpellSlot.R);
        }

        public void LoadMenu()
        {
            Menus.menu = new Menu("[HuyNK] " + Menus.CHAMP_NAME, "huynks" + Menus.CHAMP_NAME, true);
            var key = new Menu("Key", "Key");
            {
                key.AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
                key.AddItem(new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
                key.AddItem(new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind("N".ToCharArray()[0], KeyBindType.Toggle)));
                Menus.menu.AddSubMenu(key);
            }

            var combo = new Menu("Combo", "Combo");
            {
                combo.AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
                combo.AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
                combo.AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
                combo.AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
                combo.AddItem(new MenuItem("Ignite", "Use Ignite").SetValue(true));
                Menus.menu.AddSubMenu(combo);
            }

            var harass = new Menu("Harass", "Harass");
            {
                harass.AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
                //AddManaManagertoMenu(harass, "Harass", 30);
                //add to menu
                Menus.menu.AddSubMenu(harass);
            }

            var miscMenu = new Menu("Misc", "Misc");
            {
                miscMenu.AddItem(new MenuItem("Get_Cord", "Get Coordinates").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
                //add to menu
                Menus.menu.AddSubMenu(miscMenu);
            }

            var drawMenu = new Menu("Drawing", "Drawing");
            {
                drawMenu.AddItem(new MenuItem("Draw_Disabled", "Disable All").SetValue(false));
                drawMenu.AddItem(new MenuItem("Draw_Q", "Draw Q").SetValue(true));
                drawMenu.AddItem(new MenuItem("Draw_W", "Draw W").SetValue(true));
                drawMenu.AddItem(new MenuItem("Draw_E", "Draw E").SetValue(true));
                drawMenu.AddItem(new MenuItem("Draw_R", "Draw R").SetValue(true));
                drawMenu.AddItem(new MenuItem("Draw_R_Killable", "Draw R Mark on Killable").SetValue(true));

                MenuItem drawComboDamageMenu = new MenuItem("Draw_ComboDamage", "Draw Combo Damage").SetValue(true);
                drawMenu.AddItem(drawComboDamageMenu);
                Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
                Utility.HpBarDamageIndicator.Enabled = drawComboDamageMenu.GetValue<bool>();
                drawComboDamageMenu.ValueChanged +=
                    delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
                    };

                Menus.menu.AddSubMenu(drawMenu);
            }
        }

        private float GetComboDamage(Obj_AI_Base target)
        {
            double comboDamage = 0;

            if (Q.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.Q);

            if (E.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.E) * 2;

            if (R.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.R);

            if (HuyNkItems.IgniteSlot != SpellSlot.Unknown && Player.SummonerSpellbook.CanUseSpell(HuyNkItems.IgniteSlot) == SpellState.Ready)
                comboDamage += Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            return (float)(comboDamage + Player.GetAutoAttackDamage(target) * 2);
        }

        private void Combo()
        {
            UseSpells(Menus.menu.Item("UseQCombo").GetValue<bool>(), Menus.menu.Item("UseWCombo").GetValue<bool>(),
                false, Menus.menu.Item("UseRCombo").GetValue<bool>(), "Combo");
           
                    HuyNkItems.Use_DFG(SelectedTarget);
            
        }

        private void Harass()
        {
            UseSpells(Menus.menu.Item("UseQHarass").GetValue<bool>(), false,
                false, false, "Harass");
        }

        private void UseSpells(bool useQ, bool useW, bool useE, bool useR, string source)
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);

            if (target != null)
            {
                if(useQ && Q.IsReady())
                    Q.CastOnUnit(target, HuyNkItems.packets());

                if(useW && W.IsReady())
                    W.Cast(HuyNkItems.packets());
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (Menus.menu.Item("Get_Cord").GetValue<KeyBind>().Active)
            {
                Game.PrintChat("X: " + Player.ServerPosition.X + " Y: " + Player.ServerPosition.Y + " Z: " + Player.ServerPosition.Z);
            }
            

            if (Menus.menu.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Menus.menu.Item("HarassActiveT").GetValue<KeyBind>().Active)
                    Harass();

                if (Menus.menu.Item("HarassActive").GetValue<KeyBind>().Active)
                    Harass();
            }
        }
    }
}
