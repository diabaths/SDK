
using System;
using System.Collections.Generic;
using System.Drawing;

using LeagueSharp;
using LeagueSharp.SDK;

namespace D_Ezreal_SDK_
{
    using LeagueSharp.SDK.Enumerations;
    using LeagueSharp.SDK.Utils;

    internal static class Program
    {
        internal static bool UseQ;

        private static int usemuranama;

        internal const int IgniteRange = 600;

        internal static SpellSlot Ignite;

        internal static Obj_AI_Hero Player;

        private const string ChampName = "Ezreal";

        public static object SharpDX { get; private set; }

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
                            "R",
                            () => Config.Modes.Drawings.RDamageColor,
                            unit =>
                            SpellManager.R.IsReady() ? (float)unit.GetRDamage() : 0f,
                            () => Config.Modes.Drawings.DrawRDamage)
                    },
                () => Config.Modes.Drawings.DamageIndicatorEnabled,
                () => Config.Modes.Drawings.HerosEnabled);

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += OnUpdate;
            Events.OnGapCloser += OnGapCloser;
            Logging.Write()(LogLevel.Info, "D-Ezreal Loaded successfully!");

            Notifications.Add(
                new Notification("D-Ezreal Loaded!", "Enjoy the Free elo Tnxkkbb!"));
        }

        private static void OnUpdate(EventArgs args)
        {
            usepotion();
        }

        private static void OnGapCloser(object oSender, Events.GapCloserEventArgs args)
        {
            var sender = args.Sender;
            if (Config.Modes.Misc.Gap_E && sender.Distance(GameObjects.Player.ServerPosition) <= 300)
            {
                if (args.IsDirectedToPlayer)
                {
                    if (SpellManager.E.IsReady())
                    {
                        SpellManager.E.Cast(Player.Position.Extend(sender.Position, -SpellManager.E.Range));
                    }
                }
            }
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

        private static void Drawing_OnDraw(EventArgs args)
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