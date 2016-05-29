
using LeagueSharp.SDK;
using Settings = Cait.Config.Modes.Harass;

namespace Cait.Modes
{
    internal sealed class Harass : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return Config.Keys.HarassActive;
        }

        internal override void Execute()
        {
            if (!Variables.Orbwalker.CanMove())
            {
                return;
            }

            if (Q.IsReady() && GameObjects.Player.ManaPercent > Settings.Mana)
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
                    if (prediction.Hitchance >= HitChance.VeryHigh && target.IsValidTarget(Q.Range-150))
                    {
                        Q.Cast(prediction.CastPosition);
                    }
                }
            }
        }
    }
}

