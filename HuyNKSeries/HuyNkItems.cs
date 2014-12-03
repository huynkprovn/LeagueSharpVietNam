using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using SharpDX;

using Color = System.Drawing.Color;
namespace HuyNKSeries
{
  public   class HuyNkItems
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static Obj_AI_Hero SelectedTarget = null;
        //summoners
        public static SpellSlot IgniteSlot = ObjectManager.Player.GetSpellSlot("SummonerDot");
        //HuyNkItems
        public static LeagueSharp.Common.Items.Item DFG = Utility.Map.GetMap()._MapType == Utility.Map.MapType.TwistedTreeline ? new LeagueSharp.Common.Items.Item(3188, 750) : new LeagueSharp.Common.Items.Item(3128, 750);
        public static LeagueSharp.Common.Items.Item Botrk = new LeagueSharp.Common.Items.Item(3153, 450);
        public static LeagueSharp.Common.Items.Item Bilge = new LeagueSharp.Common.Items.Item(3144, 450);
        public static LeagueSharp.Common.Items.Item Hex = new LeagueSharp.Common.Items.Item(3146, 700);
        public static int lastPlaced;
        public Vector3 lastWardPos;
        public static void Use_DFG(Obj_AI_Hero target)
        {
            if (target != null && Player.Distance(target) < 750 && LeagueSharp.Common.Items.CanUseItem(DFG.Id))
                LeagueSharp.Common.Items.UseItem(DFG.Id, target);
        }

        public static void Use_Hex(Obj_AI_Hero target)
        {
            if (target != null && Player.Distance(target) < 450 && LeagueSharp.Common.Items.CanUseItem(Hex.Id))
                LeagueSharp.Common.Items.UseItem(Hex.Id, target);
        }
        public static  void Use_Botrk(Obj_AI_Hero target)
        {
            if (target != null && Player.Distance(target) < 450 && LeagueSharp.Common.Items.CanUseItem(Botrk.Id))
                LeagueSharp.Common.Items.UseItem(Botrk.Id, target);
        }

        public static void Use_Bilge(Obj_AI_Hero target)
        {
            if (target != null && Bilge.IsReady() && Player.Distance(target) < 450 && LeagueSharp.Common.Items.CanUseItem(Bilge.Id))
                LeagueSharp.Common.Items.UseItem(Bilge.Id, target);
        }
        public static void Use_Ignite(Obj_AI_Hero target)
        {
            if (target != null && IgniteSlot != SpellSlot.Unknown &&
                    Player.SummonerSpellbook.CanUseSpell(IgniteSlot) == SpellState.Ready && Player.Distance(target) < 650)
                Player.SummonerSpellbook.CastSpell(IgniteSlot, target);
        }

        public static  bool Ignite_Ready()
        {
            if (IgniteSlot != SpellSlot.Unknown && Player.SummonerSpellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                return true;
            return false;
        }
        public   static bool IsInsideEnemyTower(Vector3 position)
        {
            return ObjectManager.Get<Obj_AI_Turret>()
                                    .Any(tower => tower.IsEnemy && tower.Health > 0 && tower.Position.Distance(position) < 775);
        }
        public static  float GetManaPercent(Obj_AI_Hero unit = null)
        {
            if (unit == null)
                unit = Player;
            return (unit.Mana / unit.MaxMana) * 100f;
        }
        public static float GetHealthPercent(Obj_AI_Hero unit = null)
        {
            if (unit == null)
                unit = Player;
            return (unit.Health / unit.MaxHealth) * 100f;
        }
        public static bool HasBuff(Obj_AI_Base target, string buffName)
        {
            return target.Buffs.Any(buff => buff.Name == buffName);
        }
        public static bool IsWall(Vector2 pos)
        {
            return (NavMesh.GetCollisionFlags(pos.X, pos.Y) == CollisionFlags.Wall ||
                    NavMesh.GetCollisionFlags(pos.X, pos.Y) == CollisionFlags.Building);
        }
        public static Vector2 V2E(Vector3 from, Vector3 direction, float distance)
        {
            return from.To2D() + distance * Vector3.Normalize(direction - from).To2D();
        }
        public static bool IsPassWall(Vector3 start, Vector3 end)
        {
            double count = Vector3.Distance(start, end);
            for (uint i = 0; i <= count; i += 10)
            {
                Vector2 pos = V2E(start, end, i);
                if (IsWall(pos))
                    return true;
            }
            return false;
        }
        public static int countEnemiesNearPosition(Vector3 pos, float range)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>().Count(
                    hero => hero.IsEnemy && !hero.IsDead && hero.IsValid && hero.Distance(pos) <= range);
        }

        public static int countAlliesNearPosition(Vector3 pos, float range)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>().Count(
                    hero => hero.IsAlly && !hero.IsDead && hero.IsValid && hero.Distance(pos) <= range);
        }
        /*
        public bool manaCheck()
        {
            int totalMana = qMana[Q.Level] + wMana[W.Level] + eMana[E.Level] + rMana[R.Level];
            var checkMana = menu.Item("mana").GetValue<bool>();

            if (Player.Mana >= totalMana || !checkMana)
                return true;

            return false;
        }*/

        public static PredictionOutput GetP(Vector3 pos, Spell spell, Obj_AI_Base target, float delay, bool aoe)
        {
            return Prediction.GetPrediction(new PredictionInput
            {
                Unit = target,
                Delay = spell.Delay + delay,
                Radius = spell.Width,
                Speed = spell.Speed,
                From = pos,
                Range = spell.Range,
                Collision = spell.Collision,
                Type = spell.Type,
                RangeCheckFrom = Player.ServerPosition,
                Aoe = aoe,
            });
        }

        public static PredictionOutput GetP(Vector3 pos, Spell spell, Obj_AI_Base target, bool aoe)
        {
            return Prediction.GetPrediction(new PredictionInput
            {
                Unit = target,
                Delay = spell.Delay,
                Radius = spell.Width,
                Speed = spell.Speed,
                From = pos,
                Range = spell.Range,
                Collision = spell.Collision,
                Type = spell.Type,
                RangeCheckFrom = Player.ServerPosition,
                Aoe = aoe,
            });
        }

        public static PredictionOutput GetP2(Vector3 pos, Spell spell, Obj_AI_Base target, bool aoe)
        {
            return Prediction.GetPrediction(new PredictionInput
            {
                Unit = target,
                Delay = spell.Delay,
                Radius = spell.Width,
                Speed = spell.Speed,
                From = pos,
                Range = spell.Range,
                Collision = spell.Collision,
                Type = spell.Type,
                RangeCheckFrom = pos,
                Aoe = aoe,
            });
        }

        public static PredictionOutput GetPCircle(Vector3 pos, Spell spell, Obj_AI_Base target, bool aoe)
        {
            return Prediction.GetPrediction(new PredictionInput
            {
                Unit = target,
                Delay = spell.Delay,
                Radius = 1,
                Speed = float.MaxValue,
                From = pos,
                Range = float.MaxValue,
                Collision = spell.Collision,
                Type = spell.Type,
                RangeCheckFrom = pos,
                Aoe = aoe,
            });
        }

        public static Object[] VectorPointProjectionOnLineSegment(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            float cx = v3.X;
            float cy = v3.Y;
            float ax = v1.X;
            float ay = v1.Y;
            float bx = v2.X;
            float by = v2.Y;
            float rL = ((cx - ax) * (bx - ax) + (cy - ay) * (by - ay)) /
                       ((float)Math.Pow(bx - ax, 2) + (float)Math.Pow(by - ay, 2));
            var pointLine = new Vector2(ax + rL * (bx - ax), ay + rL * (by - ay));
            float rS;
            if (rL < 0)
            {
                rS = 0;
            }
            else if (rL > 1)
            {
                rS = 1;
            }
            else
            {
                rS = rL;
            }
            bool isOnSegment;
            if (rS.CompareTo(rL) == 0)
            {
                isOnSegment = true;
            }
            else
            {
                isOnSegment = false;
            }
            var pointSegment = new Vector2();
            if (isOnSegment)
            {
                pointSegment = pointLine;
            }
            else
            {
                pointSegment = new Vector2(ax + rS * (bx - ax), ay + rS * (by - ay));
            }
            return new object[3] { pointSegment, pointLine, isOnSegment };
        }

        public static void CastBasicSkillShot(Spell spell, float range, SimpleTs.DamageType type, HitChance hitChance)
        {
            var target = SimpleTs.GetTarget(range, type);

            if (target == null || !spell.IsReady())
                return;
            spell.UpdateSourcePosition();

            if (spell.GetPrediction(target).Hitchance >= hitChance)
                spell.Cast(target, packets());
        }

        public Obj_AI_Hero GetTargetFocus(float range)
        {
            var focusSelected = Menus.menu.Item("selected").GetValue<bool>();

            if (SimpleTs.GetSelectedTarget() != null)
                if (focusSelected && SimpleTs.GetSelectedTarget().Distance(Player.ServerPosition) < range + 100 && SimpleTs.GetSelectedTarget().Type == GameObjectType.obj_AI_Hero)
                {
                    //Game.PrintChat("Focusing: " + SimpleTs.GetSelectedTarget().Name);
                    return SimpleTs.GetSelectedTarget();
                }
            return null;
        }

        public static HitChance GetHitchance(string Source)
        {
            var hitC = HitChance.High;
            int qHit = Menus.menu.Item("qHit").GetValue<Slider>().Value;
            int harassQHit = Menus.menu.Item("qHit2").GetValue<Slider>().Value;

            // HitChance.Low = 3, Medium , High .... etc..
            if (Source == "Combo")
            {
                switch (qHit)
                {
                    case 1:
                        hitC = HitChance.Low;
                        break;
                    case 2:
                        hitC = HitChance.Medium;
                        break;
                    case 3:
                        hitC = HitChance.High;
                        break;
                    case 4:
                        hitC = HitChance.VeryHigh;
                        break;
                }
            }
            else if (Source == "Harass")
            {
                switch (harassQHit)
                {
                    case 1:
                        hitC = HitChance.Low;
                        break;
                    case 2:
                        hitC = HitChance.Medium;
                        break;
                    case 3:
                        hitC = HitChance.High;
                        break;
                    case 4:
                        hitC = HitChance.VeryHigh;
                        break;
                }
            }

            return hitC;
        }
        public static bool packets()
        {
            return Menus.menu.Item("packet").GetValue<bool>();
        }
    }
}
