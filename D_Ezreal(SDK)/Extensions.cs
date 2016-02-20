using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Wrappers.Damages;

namespace D_Ezreal_SDK_
{
    using System.Linq;

    internal static class Extensions
    {
        internal static bool canttack(this Obj_AI_Base hero)
        {
            return hero.IsMe && hero.CanAttack;
        }

        internal static double GetQDamage(this Obj_AI_Base target)
        {
            return GameObjects.Player.GetSpellDamage(target, SpellSlot.Q);
        }

        internal static double GetWDamage(this Obj_AI_Base target)
        {
            return GameObjects.Player.GetSpellDamage(target, SpellSlot.W);
        }

        internal static double GetRDamage(this Obj_AI_Base target)
        {
            return GameObjects.Player.GetSpellDamage(target, SpellSlot.R);
        }

        internal static bool IsKillableWithauto(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.Q.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield < GameObjects.Player.GetAutoAttackDamage(target);
        }

        internal static bool IsKillableWithQ(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.Q.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield < target.GetQDamage();
        }

        internal static bool IsKillableWithW(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.W.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield < target.GetWDamage();
        }

        internal static bool IsKillableWithR(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.R.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield < 0.88 * target.GetRDamage();
        }

        internal static bool IsKillableWithQAuto(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.Q.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield
                   < target.GetQDamage() + GameObjects.Player.GetAutoAttackDamage(target);
        }

        internal static bool IsKillableWithWAuto(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.W.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield
                   < target.GetWDamage() + GameObjects.Player.GetAutoAttackDamage(target);
        }

        internal static bool IsKillableWithQW(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.W.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield
                   < target.GetWDamage() + target.GetQDamage();
        }

        internal static bool combodamage(this Obj_AI_Base target, bool rangeCheck = true)
        {
            return target.IsValidTarget(rangeCheck ? SpellManager.W.Range : float.MaxValue)
                   && target.Health + target.HPRegenRate + target.PhysicalShield
                   < target.GetWDamage() + target.GetQDamage() + target.GetRDamage();
        }
    }
}
