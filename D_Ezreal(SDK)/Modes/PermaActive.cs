using System.Linq;

using LeagueSharp.SDK;

using Settings = D_Ezreal_SDK_.Config.Modes.Misc;

namespace  D_Ezreal_SDK_.Modes
{
    using System;

    using LeagueSharp.SDK.Enumerations;
    using LeagueSharp.SDK.Utils;

    internal sealed class PermaActive : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return true;
        }

        internal override void Execute()
        {
            if (Q.IsReady())
            {
                var target = Variables.TargetSelector.GetTarget(Q);
                if (target.IsValidTarget(Q.Range))
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
                    if (prediction.Hitchance >= HitChance.High && Q.GetPrediction(target).CollisionObjects.Count == 0)
                    {
                        if (Settings.useQK && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true)))
                        {
                            Q.Cast(prediction.CastPosition);
                            Modes.Combo.castR = Environment.TickCount;
                        }
                    }

                    if (prediction.Hitchance == HitChance.Immobile
                        && Q.GetPrediction(target).CollisionObjects.Count == 0)
                        if (Settings.useQimmo)
                        {
                            Q.Cast(prediction.CastPosition);
                            Modes.Combo.castR = Environment.TickCount;
                        }

                    if (prediction.Hitchance == HitChance.Dashing && Q.GetPrediction(target).CollisionObjects.Count == 0)
                        if (Settings.useQdash)
                        {
                            Q.Cast(prediction.CastPosition);
                            Modes.Combo.castR = Environment.TickCount;
                        }
                }
            }

            if (W.IsReady())
            {
                var target = Variables.TargetSelector.GetTarget(W);
                if (target.IsValidTarget(W.Range))
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
                    if (prediction.Hitchance >= HitChance.High)
                    {
                        if (Settings.useQK && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithW(true)))
                        {
                            W.Cast(prediction.CastPosition);
                            Modes.Combo.castR = Environment.TickCount;
                        }
                    }
                }
            }

            if (R.IsReady() && Settings.UseRM)
            {
                var target = Variables.TargetSelector.GetTarget(1500);
                if (target.IsValidTarget(R.Range))
                {
                    var prediction =
                        Movement.GetPrediction(
                            new PredictionInput
                                {
                                    Unit = target,
                                    Delay = R.Delay,
                                    Radius = R.Width,
                                    Speed = R.Speed,
                                    Range = R.Range
                                });
                    if (prediction.Hitchance >= HitChance.High && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithR(true)))
                    {
                        if (Q.IsReady() && W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQW(true))
                            && target.IsValidTarget(Q.Range)) return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true))
                            && target.IsValidTarget(Q.Range)) return;
                        if (W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithW(true))
                            && target.IsValidTarget(W.Range)) return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQAuto(true))
                            && target.IsValidTarget(Q.Range)) return;
                        if (W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithWAuto(true))
                            && target.IsValidTarget(W.Range)) return;
                        if (target.DistanceToPlayer() < target.GetRealAutoAttackRange()
                            && target.Health <= GameObjects.Player.GetAutoAttackDamage(target)) return;
                        if (Environment.TickCount - Modes.Combo.castR > 500
                            && target.DistanceToPlayer() > Config.Modes.Combo.Minrange)
                        {
                            R.Cast(prediction.CastPosition);
                        }
                    }
                }
            }
        }
    }
}