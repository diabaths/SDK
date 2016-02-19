using System.Linq;

using LeagueSharp.SDK;

using Settings = D_Ezreal_SDK_.Config.Modes.LaneClear;

namespace D_Ezreal_SDK_.Modes
{
    using LeagueSharp;
    using LeagueSharp.SDK.Core.Wrappers.Damages;

    internal sealed class LaneClear : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return Config.Keys.LaneClearActive;

        }

        internal override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && GameObjects.Player.ManaPercent > Settings.MinMana)
            {
                foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsKillableWithQ(true)))
                {
                    var prediction = Q.GetPrediction(minion);
                    if (prediction.Hitchance < HitChance.High && ObjectManager.Player.canttack())
                    {
                        continue;
                    }

                    var killcount = 0;

                    foreach (var collisionMinion in prediction.CollisionObjects.OrderBy(x => x.Distance(GameObjects.Player)))
                    {
                        if (collisionMinion.IsKillableWithQ())
                        {
                            killcount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (killcount < 1)
                    {
                        continue;
                    }

                    if (Q.Cast(prediction.CastPosition))
                    {
                        break;
                    }
                }
            }
        }
    }
}
