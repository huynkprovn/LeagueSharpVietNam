using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using SharpDX;

using Color = System.Drawing.Color;

namespace HuyNKSeries.Champ
{
  internal class Kalista : Champion
    {
        public static Spell Q, W, E, R;
        public static readonly List<Spell> spellList = new List<Spell>();
        
        public Kalista()
        {
            SetSpells();
            LoadMenu();
        }

        private void LoadMenu()
        {
            // Create menu

           // Menus.menu = new Menu("[HuyNK] " + Menus.CHAMP_NAME, "huynks" + Menus.CHAMP_NAME, true);
           Menu key = new Menu("Key", "Key");
           
                key.AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
                key.AddItem(new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
                key.AddItem(new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind("N".ToCharArray()[0], KeyBindType.Toggle)));
                Menus.menu.AddSubMenu(key);
          

            // Combo
            Menu combo = new Menu("Combo", "combo");
            combo.AddItem(new MenuItem("comboUseQ", "Use Q").SetValue(true));
            combo.AddItem(new MenuItem("comboUseE", "Use E").SetValue(true));
            combo.AddItem(new MenuItem("comboNumE", "Stacks for E").SetValue(new Slider(5, 1, 20)));
            combo.AddItem(new MenuItem("comboUseItems", "Use items").SetValue(true));
            combo.AddItem(new MenuItem("comboUseIgnite", "Use Ignite").SetValue(true));
            combo.AddItem(new MenuItem("comboActive", "Combo active").SetValue(new KeyBind(32, KeyBindType.Press)));
            Menus.menu.AddSubMenu(combo);

            // Harass
            Menu harass = new Menu("Harass", "harass");
            harass.AddItem(new MenuItem("harassUseQ", "Use Q").SetValue(true));
            harass.AddItem(new MenuItem("harassMana", "Mana usage in percent (%)").SetValue(new Slider(30)));
            harass.AddItem(new MenuItem("harassActive", "Harass active").SetValue(new KeyBind('C', KeyBindType.Press)));
            Menus.menu.AddSubMenu(harass);

            // WaveClear
            Menu waveClear = new Menu("WaveClear", "waveClear");
            waveClear.AddItem(new MenuItem("waveUseQ", "Use Q").SetValue(true));
            waveClear.AddItem(new MenuItem("waveNumQ", "Minion kill number for Q").SetValue(new Slider(3, 1, 10)));
            waveClear.AddItem(new MenuItem("waveUseE", "Use E").SetValue(true));
            waveClear.AddItem(new MenuItem("waveNumE", "Minion kill number for E").SetValue(new Slider(2, 1, 10)));
            waveClear.AddItem(new MenuItem("waveBigE", "Always E big minions").SetValue(true));
            waveClear.AddItem(new MenuItem("waveMana", "Mana usage in percent (%)").SetValue(new Slider(30)));
            waveClear.AddItem(new MenuItem("waveActive", "WaveClear active").SetValue(new KeyBind('V', KeyBindType.Press)));
            Menus.menu.AddSubMenu(waveClear);

            // JungleClear
            Menu jungleClear = new Menu("JungleClear", "jungleClear");
            jungleClear.AddItem(new MenuItem("jungleUseE", "Use E").SetValue(true));
            jungleClear.AddItem(new MenuItem("jungleActive", "JungleClear active").SetValue(new KeyBind('V', KeyBindType.Press)));
            Menus.menu.AddSubMenu(jungleClear);

            // Flee
            Menu flee = new Menu("Flee", "flee");
            flee.AddItem(new MenuItem("fleeWalljump", "Try to jump over walls").SetValue(true));
            flee.AddItem(new MenuItem("fleeAA", "Smart usage of AA").SetValue(true));
            flee.AddItem(new MenuItem("fleeActive", "Flee active").SetValue(new KeyBind('T', KeyBindType.Press)));
            Menus.menu.AddSubMenu(flee);

            // Misc
            Menu misc = new Menu("Misc", "misc");
            misc.AddItem(new MenuItem("miscKillstealE", "Killsteal with E").SetValue(true));
            Menus.menu.AddSubMenu(misc);

            // Items
            Menu items = new Menu("Items", "items");
            items.AddItem(new MenuItem("itemsBotrk", "Use BotRK").SetValue(true));
            Menus.menu.AddSubMenu(items);

            // Drawings
            Menu drawings = new Menu("Drawings", "drawings");
            drawings.AddItem(new MenuItem("drawRangeQ", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.IndianRed))));
            drawings.AddItem(new MenuItem("drawRangeW", "W range").SetValue(new Circle(true, Color.FromArgb(150, Color.MediumPurple))));
            drawings.AddItem(new MenuItem("drawRangeE", "E range").SetValue(new Circle(true, Color.FromArgb(150, Color.DarkRed))));
            drawings.AddItem(new MenuItem("drawRangeR", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.Red))));
            Menus.menu.AddSubMenu(drawings);

            // Finalize menu
           // Menus.menu.AddToMainMenu();
        }

        public static void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 1150);
            W = new Spell(SpellSlot.W, 5000);
            E = new Spell(SpellSlot.E, 1000);
            R = new Spell(SpellSlot.R, 1500);

            // Add to spell list
            spellList.AddRange(new[] { Q, W, E, R });

            // Finetune spells
            Q.SetSkillshot(0.25f, 40, 1200, true, SkillshotType.SkillshotLine);
        }
        public  override void Game_OnGameUpdate(EventArgs args)
        {
            // Combo
            if (Menus.menu.SubMenu("combo").Item("comboActive").GetValue<KeyBind>().Active)
                Combo();
            // Harass
            if (Menus.menu.SubMenu("harass").Item("harassActive").GetValue<KeyBind>().Active)
                Harass();
            // WaveClear
            if (Menus.menu.SubMenu("waveClear").Item("waveActive").GetValue<KeyBind>().Active)
                WaveClear();
            // JungleClear
            if (Menus.menu.SubMenu("jungleClear").Item("jungleActive").GetValue<KeyBind>().Active)
                JungleClear();
            // Flee
            if (Menus.menu.SubMenu("flee").Item("fleeActive").GetValue<KeyBind>().Active)
                Flee();

            // Check killsteal
            if (E.IsReady() && Menus.menu.SubMenu("misc").Item("miscKillstealE").GetValue<bool>())
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValidTarget(E.Range)))
                {
                    if (Menus.player.GetSpellDamage(enemy, SpellSlot.E) > enemy.Health)
                    {
                        E.Cast();
                        break;
                    }
                }
            }
        }
        public static float GetComboDamage(Obj_AI_Hero target)
        {
            // Auto attack damage
            double damage = Menus.player.GetAutoAttackDamage(target);

            // Q damage
            if (Q.IsReady())
                damage += Menus.player.GetSpellDamage(target, SpellSlot.Q);

            // E stack damage
            if (E.IsReady())
                damage += Menus.player.GetSpellDamage(target, SpellSlot.E);

            return (float)damage;
        }

      public static void CastBasicSkillShot(Spell spell, float range, SimpleTs.DamageType type, HitChance hitChance)
      {
          var target = SimpleTs.GetTarget(range, type);

          if (target == null || !spell.IsReady())
              return;
          spell.UpdateSourcePosition();

          if (spell.GetPrediction(target).Hitchance >= hitChance)
              spell.Cast(target, HuyNkItems.packets());
      }
        public static void Combo()
        {
            bool useQ = Menus.menu.SubMenu("combo").Item("comboUseQ").GetValue<bool>();
            bool useE = Menus.menu.SubMenu("combo").Item("comboUseE").GetValue<bool>();

            Obj_AI_Hero target;

            if (useQ && Q.IsReady())
                target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
            else
                target = SimpleTs.GetTarget(Orbwalking.GetRealAutoAttackRange(Menus.player), SimpleTs.DamageType.Physical);

            if (target == null)
                return;

            // Item usage
            if (Menus.menu.SubMenu("combo").Item("comboUseItems").GetValue<bool>())
            {
                if (Menus.menu.SubMenu("items").Item("itemsBotrk").GetValue<bool>())
                {
                    bool foundCutlass = Items.HasItem(3144);
                    bool foundBotrk = Items.HasItem(3153);

                    if (foundCutlass || foundBotrk)
                    {
                        if (foundCutlass || Menus.player.Health + Menus.player.GetItemDamage(target, Damage.DamageItems.Botrk) < Menus.player.MaxHealth)
                            Items.UseItem(foundCutlass ? 3144 : 3153, target);
                    }
                }
            }

            // Spell usage
            if (useQ && Q.IsReady())
                CastBasicSkillShot(Q, Q.Range, SimpleTs.DamageType.Physical, HitChance.Collision);
                Q.Cast(target, HuyNkItems.packets());

            if (useE && E.IsReady())
            {
                if (target.HasBuff("KalistaExpungeMarker") && target.Buffs.FirstOrDefault(b => b.DisplayName == "KalistaExpungeMarker").Count >= Menus.menu.SubMenu("combo").Item("comboNumE").GetValue<Slider>().Value)
                    E.Cast(true);
                HuyNkItems.CastBasicSkillShot(Q, Q.Range, SimpleTs.DamageType.Physical, HitChance.Collision);
            }
        }
        public static void Harass()
        {
            if ((Menus.player.Mana / Menus.player.MaxMana) * 100 < Menus.menu.SubMenu("harass").Item("harassMana").GetValue<Slider>().Value)
                return;

            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
            if (target == null)
                return;

            bool useQ = Menus.menu.SubMenu("harass").Item("harassUseQ").GetValue<bool>();

            if (useQ && Q.IsReady())
                
                Q.Cast(target, HuyNkItems.packets());
                HuyNkItems.CastBasicSkillShot(Q, Q.Range, SimpleTs.DamageType.Physical, HitChance.Medium);
        }
        public static void getAvailableJumpSpots()
        {
            int size = 295;
            int n = 15;
            double x, y;
            Vector3 drawWhere;

            if (!Q.IsReady())
            {
                Drawing.DrawText(Drawing.Width * 0.44f, Drawing.Height * 0.80f, Color.Red,
                " SKILL Q HAVEN'T READY");
            }
            else
            {
                Drawing.DrawText(Drawing.Width * 0.39f, Drawing.Height * 0.80f, Color.RoyalBlue,
                    "CLICK TO JUMP");
            }
            Vector3 playerPosition = HuyNkItems.Player.Position;
            Drawing.DrawCircle(ObjectManager.Player.Position, size, Color.RoyalBlue);
            Obj_AI_Hero     qtarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
            for (int i = 1; i <= n; i++)
            {
                x = size * Math.Cos(2 * Math.PI * i / n);
                y = size * Math.Sin(2 * Math.PI * i / n);
                drawWhere = new Vector3((int)(playerPosition.X + x), (float)(playerPosition.Y + y), playerPosition.Z);
                if (!Utility.IsWall(drawWhere))
                {
                    if (Q.IsReady() && Game.CursorPos.Distance(drawWhere) <= 80f)
                    {
                        if (qtarget != null)
                            Q.Cast(qtarget);
                        else
                            Q.Cast(new Vector2(drawWhere.X, drawWhere.Y), true);

                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(drawWhere.X, drawWhere.Y)).Send();
                        return;
                    }
                  
                    Utility.DrawCircle(drawWhere, 20, Color.Red);
                    
                }
            }

        }
        public static void Flee()
        {
            bool useWalljump = Menus.menu.SubMenu("flee").Item("fleeWalljump").GetValue<bool>();
            if (useWalljump)
            {
                getAvailableJumpSpots();
            }
            bool useAA = Menus.menu.SubMenu("flee").Item("fleeAA").GetValue<bool>();

            if (useAA)
            {
                var dashObject = GetDashObject();
                if (dashObject != null)
                    Orbwalking.Orbwalk(dashObject, Game.CursorPos);
                else
                    Orbwalking.Orbwalk(null, Game.CursorPos);
            }
        }
        public static void JungleClear()
        {
            bool useE = Menus.menu.SubMenu("jungleClear").Item("jungleUseE").GetValue<bool>();

            if (useE && E.IsReady())
            {
                var minions = MinionManager.GetMinions(Menus.player.Position, E.Range, MinionTypes.All, MinionTeam.Neutral);

                // Check if a jungle mob can die with E
                foreach (var minion in minions)
                {
                    if (Menus.player.GetSpellDamage(minion, SpellSlot.E) > minion.Health)
                    {
                        E.Cast(true);
                        break;
                    }
                }
            }
        }
        public static void WaveClear()
        {
            // Mana check
            if ((Menus.player.Mana / Menus.player.MaxMana) * 100 < Menus.menu.SubMenu("waveClear").Item("waveMana").GetValue<Slider>().Value)
                return;

            bool useQ = Menus.menu.SubMenu("waveClear").Item("waveUseQ").GetValue<bool>();
            bool useE = Menus.menu.SubMenu("waveClear").Item("waveUseE").GetValue<bool>();
            bool bigE = Menus.menu.SubMenu("waveClear").Item("waveBigE").GetValue<bool>();

            // Q usage
            if (useQ && Q.IsReady())
            {
                int hitNumber = Menus.menu.SubMenu("waveClear").Item("waveNumQ").GetValue<Slider>().Value;

                // Get minions in range
                var minions = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.BaseSkinName.Contains("Minion") && m.IsValidTarget(Q.Range)).ToList();

                if (minions.Count >= hitNumber)
                {
                    // Sort by distance
                    minions.Sort((m1, m2) => m2.Distance(Menus.player, true).CompareTo(m1.Distance(Menus.player, true)));

                    // Helpers
                    int bestHitCount = 0;
                    PredictionOutput bestResult = null;

                    foreach (var minion in minions)
                    {
                        var prediction = Q.GetPrediction(minion);

                        // Get targets being hit with colliding Q
                        var targets = prediction.CollisionObjects;
                        // Sort them by distance
                        targets.Sort((t1, t2) => t1.Distance(Menus.player, true).CompareTo(t2.Distance(Menus.player, true)));

                        // Validate
                        if (targets.Count > 0)
                        {
                            // Loop through the next targets to see if they will die with the Q hitting
                            for (int i = 0; i < targets.Count; i++)
                            {
                                if (Menus.player.GetSpellDamage(targets[i], SpellSlot.Q) < targets[i].Health || i == targets.Count)
                                {
                                    // Can't kill this minion, check result so far
                                    if (i >= hitNumber && (bestResult == null || bestHitCount < i))
                                    {
                                        bestHitCount = i;
                                        bestResult = prediction;
                                    }

                                    // Break the loop cuz can't kill target
                                    break;
                                }
                            }
                        }
                    }

                    // Check if we have a valid target with enough targets being hit
                    if (bestResult != null)
                        Q.Cast(bestResult.CastPosition);
                }
            }

            // General E usage
            if (useE && E.IsReady())
            {
                int hitNumber = Menus.menu.SubMenu("waveClear").Item("waveNumE").GetValue<Slider>().Value;

                // Get surrounding
                var minions = MinionManager.GetMinions(Menus.player.Position, E.Range);

                if (minions.Count >= hitNumber)
                {
                    // Check if enough minions die with E
                    int conditionMet = 0;
                    foreach (var minion in minions)
                    {
                        if (Menus.player.GetSpellDamage(minion, SpellSlot.E) > minion.Health)
                            conditionMet++;
                    }

                    // Cast on condition met
                    if (conditionMet >= hitNumber)
                        E.Cast(true);
                }
            }

            // Always E on big minions
            if (bigE && E.IsReady())
            {
                // Get big minions
                var minions = MinionManager.GetMinions(Menus.player.Position, E.Range).Where(m => m.BaseSkinName.Contains("MinionSiege"));

                foreach (var minion in minions)
                {
                    if (Menus.player.GetSpellDamage(minion, SpellSlot.E) > minion.Health)
                    {
                        // On first big minion which can die with E, use E
                        E.Cast(true);
                        break;
                    }
                }
            }
        }
        public static Obj_AI_Base GetDashObject()
        {
            float realAArange = Orbwalking.GetRealAutoAttackRange(Menus.player);

            var objects = ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsValidTarget(realAArange));
            Vector2 apexPoint = Menus.player.ServerPosition.To2D() + (Menus.player.ServerPosition.To2D() - Game.CursorPos.To2D()).Normalized() * realAArange;

            Obj_AI_Base target = null;

            foreach (var obj in objects)
            {
                if (VectorHelper.IsLyingInCone(obj.ServerPosition.To2D(), apexPoint, Menus.player.ServerPosition.To2D(), realAArange))
                {
                    if (target == null || target.Distance(apexPoint, true) > obj.Distance(apexPoint, true))
                        target = obj;
                }
            }

            return target;
        }
        public static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "KalistaExpungeWrapper")
                    Utility.DelayAction.Add(250, Orbwalking.ResetAutoAttackTimer);
            }
        }
        public override void Drawing_OnDraw(EventArgs args)
        {
            // Spell ranges
            foreach (var spell in spellList)
            {
                var circleEntry = Menus.menu.SubMenu("drawings").Item("drawRange" + spell.Slot.ToString()).GetValue<Circle>();
                if (circleEntry.Active)
                    Utility.DrawCircle(Menus.player.Position, spell.Range, circleEntry.Color);
            }
        }

    }
}
