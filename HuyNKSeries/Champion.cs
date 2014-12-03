using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace HuyNKSeries
{
    class Champion
    {
        public Champion()
        {
            //Events
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPosibleToInterrupt;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            GameObject.OnCreate += GameObject_OnCreate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Game.OnGameSendPacket += Game_OnSendPacket;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        public Champion(bool load)
        {
            if (load)
                GameOnLoad(); ;
        }

        //Orbwalker instance
        public Orbwalking.Orbwalker Orbwalker;

        //Player instance
        public Obj_AI_Hero Player = ObjectManager.Player;
        public Obj_AI_Hero SelectedTarget = null;

        //Spells
        public List<Spell> SpellList = new List<Spell>();

        public Spell Q;
        public Spell W;
        public Spell E;
        public Spell R;
        public Spell _r2;
        public SpellDataInst qSpell = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q);
        public SpellDataInst wSpell = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W);
        public SpellDataInst rSpell = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R);



        public void GameOnLoad()
        {
           

            Menus.SetuptMenu();
            

        }

        //to create by champ
        public virtual void Drawing_OnDraw(EventArgs args)
        {
            //for champs to use
        }

        public virtual void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            //for champs to use
        }

        public virtual void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            //for champs to use
        }

        public virtual void Game_OnGameUpdate(EventArgs args)
        {
            //for champs to use
        }

        public virtual void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            //for champs to use
        }

        public virtual void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            //for champs to use
        }

        public virtual void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs args)
        {
            //for champ use
        }

        public virtual void Game_OnSendPacket(GamePacketEventArgs args)
        {
            //for champ use
        }
    }
}
