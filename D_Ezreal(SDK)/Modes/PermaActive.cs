using System.Linq;

using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;

using Settings = D_Ezreal_SDK_.Config.Modes.Misc;

namespace D_Ezreal_SDK_.Modes
{
    using LeagueSharp.SDK.Core.Wrappers.Damages;

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
                    if (prediction.Hitchance >= HitChance.High && Q.GetPrediction(target).CollisionObjects.Count == 0)
                    {
                        if (Settings.useQK && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true)))
                        {
                            Q.Cast(prediction.CastPosition);
                        }
                    }

                    if (prediction.Hitchance == HitChance.Immobile
                        && Q.GetPrediction(target).CollisionObjects.Count == 0)
                        if (Settings.useQimmo)
                        {
                            Q.Cast(prediction.CastPosition);
                        }

                    if (prediction.Hitchance == HitChance.Dashing && Q.GetPrediction(target).CollisionObjects.Count == 0)
                        if (Settings.useQdash)
                        {
                            Q.Cast(prediction.CastPosition);
                        }
                }
            }

            if (W.IsReady())
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
                    if (prediction.Hitchance >= HitChance.High)
                    {
                        if (Settings.useQK && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithW(true)))
                        {
                            W.Cast(prediction.CastPosition);
                        }
                    }
                }
            }

            if (R.IsReady())
            {
                var target = Variables.TargetSelector.GetTarget(1500);
                if (target != null)
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
                    if (prediction.Hitchance >= HitChance.High)
                    {
                        if (Q.IsReady() && W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQW(true))
                               && target.IsValidTarget(Q.Range))
                            return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true))
                            && target.IsValidTarget(Q.Range))
                            return;
                        if (W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithW(true))
                            && target.IsValidTarget(W.Range))
                            return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQAuto(true))
                           && target.IsValidTarget(Q.Range))
                            return;
                        if (W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithWAuto(true))
                            && target.IsValidTarget(W.Range))
                            return;
                        if (target.DistanceToPlayer() < target.GetRealAutoAttackRange()
                            && target.Health <= GameObjects.Player.GetAutoAttackDamage(target))
                            return;
                        if (Settings.UseRM && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithR(true)) && target.DistanceToPlayer() > Config.Modes.Combo.Minrange)
                        {
                            R.Cast(prediction.CastPosition);
                        }
                    }
                }
            }
        }
    }
}