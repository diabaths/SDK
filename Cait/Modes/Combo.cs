
using System;
using System.Linq;

using Cait;
using Cait.Modes;

using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;

using Settings =Cait.Config.Modes.Combo;

namespace Cait.Modes
{
    using System;

    using LeagueSharp;

    using SharpDX;


    internal sealed class Combo : ModeBase
    {
        public static int castR;

        public static int castW;

        internal override bool ShouldBeExecuted()
        {
            return Config.Keys.ComboActive;
        }

        internal override void Execute()
        {
            var ti = Variables.TargetSelector.GetTarget(Q);
            if (Settings.UseIgnitecombo && Program.Ignite.IsReady() && ti.IsValidTarget(Program.IgniteRange)
                && ti.HealthPercent < Settings.Ignitehealth)
            {
                GameObjects.Player.Spellbook.CastSpell(Program.Ignite, ti);
            }

            if ((Items.HasItem(3048) && Items.CanUseItem(3048) || Items.HasItem(3040) && Items.CanUseItem(3040))
                && Config.Modes.Items.Deffensive.Archangel && GameObjects.Player.CountEnemyHeroesInRange(800) > 0
                && GameObjects.Player.HealthPercent <= Config.Modes.Items.Deffensive.Archangelmyhp)
            {
                Items.UseItem(3048);
                Items.UseItem(3040);
            }

            if (Items.HasItem(3146) && Items.CanUseItem(3146) && Config.Modes.Items.Offensive.Hextech
                && ti.IsValidTarget(700) && ti.HealthPercent <= Config.Modes.Items.Offensive.HextechEnemyhp)
            {
                Items.UseItem(3146, ti);
            }

            if (Items.HasItem(3142) && Items.CanUseItem(3142) && Config.Modes.Items.Offensive.Youmuu
                && ti.IsValidTarget(500))
            {
                Items.UseItem(3142);
            }

            if ((Items.HasItem(3144) && Items.CanUseItem(3144) || Items.HasItem(3153) && Items.CanUseItem(3153))
                && Config.Modes.Items.Offensive.Blade && ti.IsValidTarget(450)
                && (ti.HealthPercent <= Config.Modes.Items.Offensive.BladeEnemyhp
                    || GameObjects.Player.HealthPercent <= Config.Modes.Items.Offensive.Blademyhp))
            {
                Items.UseItem(3144, ti);
                Items.UseItem(3153, ti);
            }

            if (Q.IsReady() && Settings.UseQ)
            {
                var target = Variables.TargetSelector.GetTarget(Q);
                if (target != null)
                {
                    var prediction =
                        Movement.GetPrediction(
                            new PredictionInput
                                {
                                    Unit = target,
                                    Delay = Q.Delay,
                                    Radius = Q.Width,
                                    Speed = Q.Speed,
                                    Range = Q.Range
                                });

                    if (prediction.Hitchance >= HitChance.VeryHigh
                        && target.DistanceToPlayer() > Program.Player.GetRealAutoAttackRange())
                    {
                        Q.Cast(prediction.CastPosition);
                        castR = Environment.TickCount;
                    }
                }
            }

            if (W.IsReady() && Settings.UseW)
            {
                var target = Variables.TargetSelector.GetTarget(W);
                if (target != null)
                {
                    var prediction =
                        Movement.GetPrediction(
                            new PredictionInput
                                {
                                    Unit = target,
                                    Delay = W.Delay,
                                    Radius = W.Width,
                                    Speed = W.Speed,
                                    Range = W.Range
                                });
                    if (target.IsMelee && target.IsFacing(GameObjects.Player) && target.DistanceToPlayer() < 300 && Environment.TickCount - castW > 1000)
                    {
                        W.Cast(GameObjects.Player);
                        castR = Environment.TickCount;
                        castW = Environment.TickCount;
                    }

                    if (prediction.Hitchance >= HitChance.VeryHigh && target.IsFacing(GameObjects.Player)
                        && Environment.TickCount - castW > 1000)
                    {
                        W.Cast(prediction.CastPosition);
                        castR = Environment.TickCount;
                        castW = Environment.TickCount;
                    }

                    if (!target.IsFacing(GameObjects.Player) && Environment.TickCount - castW > 2000)
                    {
                        var vector = target.ServerPosition - ObjectManager.Player.Position;
                        var Behind = W.GetPrediction(target).CastPosition + Vector3.Normalize(vector) * 100;
                        W.Cast(Behind);
                        castR = Environment.TickCount;
                        castW = Environment.TickCount;
                    }
                }
            }

            if (E.IsReady() && Settings.UseE)
            {
                var target = Variables.TargetSelector.GetTarget(1200);
                if (target != null)
                {
                    if (target.IsValidTarget(350))
                    {
                        E.Cast(target);
                        if (Q.IsReady()) DelayAction.Add(250, () => Q.Cast(Q.GetPrediction(target).CastPosition));
                        castR = Environment.TickCount;
                    }

                    if (target.IsValidTarget(1200) && GameObjects.Player.CountEnemyHeroesInRange(2000) <= 1)
                    {
                        if (
                            GameObjects.EnemyHeroes.Any(
                                x =>
                                x.IsKillableWithauto(true)
                                && target.DistanceToPlayer() > Program.Player.GetRealAutoAttackRange())
                            && !target.IsFacing(GameObjects.Player))
                        {
                            E.Cast(GameObjects.Player.Position.Extend(Game.CursorPos, -(E.Range / 2)));
                        }
                    }
                }
            }

            if (R.IsReady())
            {
                var target = Variables.TargetSelector.GetTarget(R);
                if (GameObjects.EnemyHeroes.Any(x => !x.IsKillableWithR(true))) return;
                if (target.IsValidTarget(R.Range) && Settings.UseR && !target.IsDead
                    && target.DistanceToPlayer() > Program.Player.GetRealAutoAttackRange()
                    && GameObjects.Player.CountEnemyHeroesInRange(R.Range) <= 1 && Environment.TickCount - castR > 700) R.Cast(target);
            }
        }
    }
}


