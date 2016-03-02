using System.Linq;

using LeagueSharp.SDK;

using Settings = Cait.Config.Modes.LaneClear;

namespace Cait.Modes
{
    using System.Collections.Generic;

    using SharpDX;

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

                    var collision = Q.GetCollision(
                        (Vector2)GameObjects.Player.Position,
                        new List<Vector2> { (Vector2)prediction.UnitPosition });
                    foreach (var collisions in collision)
                    {
                        if (collision.Count() >= Settings.minions)
                        {
                            if (collision.Last().Distance(GameObjects.Player.Position)
                                - collision[0].Distance(GameObjects.Player.Position) < 600
                                && collision[0].Distance(GameObjects.Player.Position) < 500) Q.Cast(collisions);
                        }
                    }
                }
            }
        }
    }
}