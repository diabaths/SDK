using LeagueSharp;
using LeagueSharp.SDK;

namespace Cait
{
    using LeagueSharp.Data.Enumerations;
    using LeagueSharp.SDK.Enumerations;

    internal static class SpellManager
    {
        internal static readonly Spell Q, W, E, R;
        internal static readonly PredictionInput QCollisionInput;

        static SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 1250f);
            W = new Spell(SpellSlot.W, 800f);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 2000);

            Q.SetSkillshot(0.25f, 60f, 2000f, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(1.00f, 100f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, 80f, 1600f, true, SkillshotType.SkillshotLine);

            QCollisionInput = new PredictionInput
            {
                Delay = Q.Delay,
                Radius = Q.Width,
                Speed = Q.Speed,
                Range = Q.Range,
                CollisionObjects = CollisionableObjects.Minions
            };
        }

        internal static void Initialize()
        {

        }
    }
}