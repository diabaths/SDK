using System.Linq;

using LeagueSharp.SDK;

using Settings = Cait.Config.Modes.JungleClear;

namespace Cait.Modes
{
    internal sealed class JungleClear : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return Config.Keys.JungleClearActive;
        }

        internal override void Execute()
        {
            if (!Variables.Orbwalker.CanMove())
            {
                return;
            }

            if (Settings.UseQ && Q.IsReady() && GameObjects.Player.ManaPercent > Settings.MinMana)
            {
                Q.Cast(GameObjects.Jungle.OrderByDescending(x => x.MaxHealth).FirstOrDefault(x => x.IsValidTarget(Q.Range)));
            }
        }
    }
}
