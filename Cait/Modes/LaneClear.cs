using System.Linq;

using LeagueSharp.SDK;

using Settings = Cait.Config.Modes.LaneClear;

namespace Cait.Modes
{
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
                foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsValidTarget(Q.Range)))
                {
                    var prediction = Q.GetPrediction(minion);
                    
                    var killcount = 0;

                    foreach (var collisionMinion in prediction.CollisionObjects.OrderBy(x => x.IsKillableWithQ(true)))
                    {
                        if (collisionMinion.IsKillableWithQ())
                        {
                            killcount++;
                        }
                    }
                    if (killcount >= 1) Q.Cast(prediction.CastPosition);

                }
            }
        }
    }
}