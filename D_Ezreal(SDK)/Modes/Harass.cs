using LeagueSharp.SDK;

using Settings = D_Ezreal_SDK_.Config.Modes.Harass;

namespace D_Ezreal_SDK_.Modes
{
    using LeagueSharp.SDK.Enumerations;
    using LeagueSharp.SDK.UI;

    internal sealed class Harass : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return Config.Keys.HarassActive;
        }

        internal override void Execute()
        {
            //Vars.Menu["spells"]["e"]["whitelist"][Targets.Target.ChampionName.ToLower()].GetValue<MenuBool>().Value
            if (Settings.UseQ && Q.IsReady() && GameObjects.Player.ManaPercent > Settings.Mana)
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
                        Q.Cast(prediction.CastPosition);
                    }
                }
            }

            if (Settings.UseW && W.IsReady() && GameObjects.Player.ManaPercent > Settings.Mana)
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
                        W.Cast(prediction.CastPosition);
                    }
                }
            }
        }
    }
}