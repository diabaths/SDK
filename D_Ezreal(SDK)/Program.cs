﻿
using System;
using System.Collections.Generic;
using System.Drawing;

using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;

namespace D_Ezreal_SDK_
{
    using System.Linq;

    using D_Ezreal_SDK_.Modes;

    internal static class Program
    {
        internal static bool UseQ;
        private static int usemuranama;
        internal const int  IgniteRange = 600;
        internal static SpellSlot  Ignite;
        internal static Obj_AI_Hero Player;
        private const string ChampName = "Ezreal";

        internal static void Main(string[] args)
        {
            Events.OnLoad += Load_OnLoad;
        }

        private static void InitSummonerSpell()
        {
            Ignite = GameObjects.Player.GetSpellSlot("summonerdot");

        }

        private static void Load_OnLoad(object sender, EventArgs e)
        {
            if (ObjectManager.Player.ChampionName != ChampName)
            {
                return;
            }

            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            InitSummonerSpell();

            DamageIndicator.Initialize(
                new List<DamageIndicator.DamageInfo>
                    {
                        new DamageIndicator.DamageInfo(
                            "Q",
                            () => Config.Modes.Drawings.QDamageColor,
                            unit =>
                            SpellManager.Q.IsReady() ? (float)unit.GetQDamage() : 0f,
                            () => Config.Modes.Drawings.DrawQDamage),
                        new DamageIndicator.DamageInfo(
                            "W",
                            () => Config.Modes.Drawings.WDamageColor,
                            unit =>
                            SpellManager.W.IsReady() ? (float)unit.GetWDamage() : 0f,
                            () => Config.Modes.Drawings.DrawWDamage),
                        new DamageIndicator.DamageInfo(
                            "E",
                            () => Config.Modes.Drawings.RDamageColor,
                            unit =>
                            SpellManager.R.IsReady() ? (float)unit.GetRDamage() : 0f,
                            () => Config.Modes.Drawings.DrawRDamage)
                    },
                () => Config.Modes.Drawings.DamageIndicatorEnabled,
                () => Config.Modes.Drawings.HerosEnabled);

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += OnUpdate;

            Variables.Orbwalker.OnAction += Orbwalker_OnAction;
            Logging.Write()(LogLevel.Info, "D-Ezreal Loaded successfully!");

            Notifications.Add(
                new Notification("D-Ezreal Loaded!", "D-Ezreal was loaded!", "tnxkkbb!")
                    {
                        HeaderTextColor =
                            SharpDX.Color
                            .LightBlue,
                        BodyTextColor =
                            SharpDX.Color
                            .White,
                        Icon =
                            NotificationIconType
                            .Check,
                        IconFlash = true
                    });
        }

        private static void OnUpdate(EventArgs args)
        {
            usepotion();
        }

        private static void usepotion()
        {

            if (GameObjects.Player.InFountain() || GameObjects.Player.HasBuff("Recall")) return;
            if (GameObjects.Player.CountEnemyHeroesInRange(800) > 0)
            {
                if (Config.Modes.Items.Potions.UseHPpotion
                    && GameObjects.Player.HealthPercent < Config.Modes.Items.Potions.minHP
                    && !(GameObjects.Player.HasBuff("RegenerationPotion")
                         || GameObjects.Player.HasBuff("ItemMiniRegenPotion")
                         || GameObjects.Player.HasBuff("ItemCrystalFlask")
                         || GameObjects.Player.HasBuff("ItemCrystalFlaskJungle")
                         || GameObjects.Player.HasBuff("ItemDarkCrystalFlask")))
                {

                    if (Items.HasItem(2010) && Items.CanUseItem(2010))
                    {
                        Items.UseItem(2010);
                    }
                    if (Items.HasItem(2003) && Items.CanUseItem(2003))
                    {
                        Items.UseItem(2003);
                    }
                    if (Items.HasItem(2031) && Items.CanUseItem(2031))
                    {
                        Items.UseItem(2031);
                    }
                    if (Items.HasItem(2032) && Items.CanUseItem(2032))
                    {
                        Items.UseItem(2032);
                    }
                    if (Items.HasItem(2033) && Items.CanUseItem(2033))
                    {
                        Items.UseItem(2033);
                    }
                }
                if (Config.Modes.Items.Potions.UseMPpotion
                    && GameObjects.Player.ManaPercent < Config.Modes.Items.Potions.MinMP
                    && !(GameObjects.Player.HasBuff("ItemDarkCrystalFlask")
                         || GameObjects.Player.HasBuff("ItemMiniRegenPotion")
                         || GameObjects.Player.HasBuff("ItemCrystalFlaskJungle")
                         || GameObjects.Player.HasBuff("ItemCrystalFlask")))
                {
                    if (Items.HasItem(2041) && Items.CanUseItem(2041))
                    {
                        Items.UseItem(2041);
                    }
                    if (Items.HasItem(2010) && Items.CanUseItem(2010))
                    {
                        Items.UseItem(2010);
                    }
                    if (Items.HasItem(2032) && Items.CanUseItem(2032))
                    {
                        Items.UseItem(2032);
                    }
                    if (Items.HasItem(2033) && Items.CanUseItem(2033))
                    {
                        Items.UseItem(2033);
                    }
                }
            }
        }

        private static void Orbwalker_OnAction(object sender, OrbwalkingActionArgs e)
        {
            if (!e.Sender?.IsMe == false)
            {
                return;
            }
            if (Variables.Orbwalker.GetActiveMode() == OrbwalkingMode.LaneClear && e.Type == OrbwalkingType.AfterAttack)
            {
                UseQ = true;
            }
            if (Variables.Orbwalker.GetActiveMode() == OrbwalkingMode.Combo && e.Type == OrbwalkingType.BeforeAttack
                && Items.HasItem(3042) && Items.CanUseItem(3042) && Config.Modes.Items.Offensive.usemuramana
                && GameObjects.Player.Buffs.Count(buf => buf.Name == "Muramana") == 0
                && Environment.TickCount - usemuranama > 350)
            {
                if (GameObjects.Player.ManaPercent > Config.Modes.Items.Offensive.muramanamin) Items.UseItem(3042);
                usemuranama = Environment.TickCount;
            }
            else if (Items.HasItem(3042) && Items.CanUseItem(3042)
                     && GameObjects.Player.Buffs.Count(buf => buf.Name == "Muramana") == 1
                     && Environment.TickCount - usemuranama > 350
                     && (GameObjects.Player.ManaPercent < Config.Modes.Items.Offensive.muramanamin
                         || Variables.Orbwalker.GetActiveMode() != OrbwalkingMode.Combo))
            {
                Items.UseItem(3042);
                usemuranama = Environment.TickCount;
            }
        }
        

        //need to fix it!!!
        /*public static void Gapcloser_OnGapCloser(object sender, GapcloserEventArgs gapcloser)
            {
                string[] herogapcloser =
                {
                    "Braum", "Ekko", "Elise", "Fiora", "Kindred", "Lucian", "Yi", "Nidalee", "Quinn", "Riven", "Shaco", "Sion", "Vayne", "Yasuo", "Graves", "Azir", "Gnar", "Irelia", "Kalista"
                };
                if (sender.IsEnemy && sender.GetAutoAttackRange() >= ObjectManager.Player.Distance(gapcloser.End) && !herogapcloser.Any(sender.ChampionName.Contains))
                {
                    var diffGapCloser = gapcloser.End - gapcloser.Start;
                    SpellManager.E.Cast(ObjectManager.Player.ServerPosition + diffGapCloser);
                }
            }*/
               private static
               void Drawing_OnDraw(EventArgs args)
        {
            if (Config.Modes.Drawings.DrawQRange)
            {
                Drawing.DrawCircle(GameObjects.Player.Position, SpellManager.Q.Range, Color.BlueViolet);
            }

            if (Config.Modes.Drawings.DrawWRange)
            {
                Drawing.DrawCircle(GameObjects.Player.Position, SpellManager.W.Range, Color.BlueViolet);
            }

            if (Config.Modes.Drawings.DrawERange)
            {
                Drawing.DrawCircle(GameObjects.Player.Position, SpellManager.E.Range, Color.BlueViolet);
            }

            if (Config.Modes.Drawings.DrawRRange)
            {
                Drawing.DrawCircle(GameObjects.Player.Position, SpellManager.R.Range, Color.BlueViolet);
            }
        }
    }
}
