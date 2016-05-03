using System;
using System.Linq;

using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using LeagueSharp.SDK.Utils;

using SharpDX;

using Settings = Cait.Config.Modes.Combo;

namespace Cait.Modes
{
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
                var targetq = Variables.TargetSelector.GetTarget(Q);
                if (targetq.IsValidTarget(Q.Range))
                {
                    var prediction =
                        Movement.GetPrediction(
                            new PredictionInput
                                {
                                    Unit = targetq,
                                    Delay = Q.Delay,
                                    Radius = Q.Width,
                                    Speed = Q.Speed,
                                    Range = Q.Range
                                });

                    if (prediction.Hitchance >= HitChance.VeryHigh
                        && targetq.DistanceToPlayer() > Program.Player.GetRealAutoAttackRange())
                    {
                        Q.Cast(prediction.CastPosition);
                        castR = Environment.TickCount;
                    }
                }
            }

            if (W.IsReady() && Settings.UseW && !GameObjects.Player.IsAttackingPlayer)
            {
                var targetw = Variables.TargetSelector.GetTarget(W);
                if (targetw.IsValidTarget(W.Range))
                { 
                    var prediction =
                        Movement.GetPrediction(
                            new PredictionInput
                                {
                                    Unit = targetw,
                                    Delay = W.Delay,
                                    Radius = W.Width,
                                    Speed = W.Speed,
                                    Range = W.Range
                                });
                    if (targetw.IsMelee && targetw.IsFacing(GameObjects.Player) && targetw.DistanceToPlayer() < 300 && Environment.TickCount - castW > 1300)
                    {
                        W.Cast(GameObjects.Player);
                        castR = Environment.TickCount;
                        castW = Environment.TickCount;
                    }

                    if (prediction.Hitchance >= HitChance.VeryHigh && targetw.IsFacing(GameObjects.Player)
                        && Environment.TickCount - castW > 1300)
                    {
                        W.Cast(prediction.CastPosition);
                        castR = Environment.TickCount;
                        castW = Environment.TickCount;
                    }

                    if (!targetw.IsFacing(GameObjects.Player) && Environment.TickCount - castW > 2000)
                    {
                        var vector = targetw.ServerPosition - ObjectManager.Player.Position;
                        var Behind = W.GetPrediction(targetw).CastPosition + Vector3.Normalize(vector) * 100;
                        W.Cast(Behind);
                        castR = Environment.TickCount;
                        castW = Environment.TickCount;
                    }
                }
            }

            if (E.IsReady() && Settings.UseE)
            {
                var targete = Variables.TargetSelector.GetTarget(1200);
                if (targete.IsValidTarget(E.Range))
                {
                    if (targete.IsValidTarget(350))
                    {
                        E.Cast(targete);
                        if (Q.IsReady()) DelayAction.Add(250, () => Q.Cast(Q.GetPrediction(targete).CastPosition));
                        castR = Environment.TickCount;
                    }

                    if (targete.IsValidTarget(1200) && GameObjects.Player.CountEnemyHeroesInRange(2000) <= 1)
                    {
                        if (
                            GameObjects.EnemyHeroes.Any(
                                x =>
                                x.IsKillableWithauto(true)
                                && targete.DistanceToPlayer() > Program.Player.GetRealAutoAttackRange())
                            && !targete.IsFacing(GameObjects.Player))
                        {
                            E.Cast(GameObjects.Player.Position.Extend(Game.CursorPos, -(E.Range / 2)));
                        }
                    }
                }
            }

            if (R.IsReady())
            {
                var target = Variables.TargetSelector.GetTarget(R);
                var pred = R.GetPrediction(target);
                if (!pred.CollisionObjects.Any(obj => obj is Obj_AI_Hero))
                {
                    if (GameObjects.EnemyHeroes.Any(x => !x.IsKillableWithR(true))) return;
                    if (target.IsValidTarget(R.Range) && Settings.UseR && !target.IsDead
                        && target.DistanceToPlayer() > 1200 && Environment.TickCount - castR > 700) R.Cast(target);
                }
            }
        }
    }
}


