
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

            if (Settings.UseQ && Q.IsReady() && GameObjects.Player.ManaPercent > Settings.Mana)
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
                    if (prediction.Hitchance >= HitChance.High && target.IsValidTarget(Q.Range-150))
                    {
                        Q.Cast(prediction.CastPosition);
                    }
                }
            }
        }
    }
}

