using System.Linq;

using LeagueSharp.SDK;

using Settings = D_Ezreal_SDK_.Config.Modes.LaneClear;

namespace D_Ezreal_SDK_.Modes
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
                int countMinions = 0;

                foreach (var miniondie in
                    GameObjects.EnemyMinions.Where(
                        minion =>
                        (minion.IsKillableWithQ(true) || minion.IsKillableWithQAuto(true)) && minion.IsValidTarget(Q.Range)))
                {
                    countMinions++;
                   
                    var prediction = Q.GetPrediction(miniondie);
                    if (countMinions >= 1 && prediction.Hitchance >= HitChance.High
                        && Q.GetPrediction(miniondie).CollisionObjects.Count == 0)
                    {
                        Q.Cast(miniondie);
                    }
                }
            }

            if (Settings.UseW && W.IsReady() && GameObjects.Player.ManaPercent > Settings.MinMana)
            {
                var heroes = GameObjects.AllyHeroes;

                foreach (var hero in heroes.Where(hero => !hero.IsDead && hero.IsValidTarget(W.Range)))
                {
                    if (!hero.IsMe && hero.IsUnderEnemyTurret())
                    {
                        W.Cast(hero);
                    }
                }
            }
        }
    }
}
