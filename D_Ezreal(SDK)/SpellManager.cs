using LeagueSharp;
using LeagueSharp.Data.Enumerations;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;

namespace D_Ezreal_SDK_
{
    internal static class SpellManager
    {
        internal static readonly Spell Q, W, E, R;
        internal static readonly PredictionInput QCollisionInput;

        static SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 1180);
            W = new Spell(SpellSlot.W, 950);
            E = new Spell(SpellSlot.E, 475);
            R = new Spell(SpellSlot.R, 2000);

            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);


            QCollisionInput = new PredictionInput
            {
                Delay = Q.Delay,
                Radius = Q.Width,
                Speed = Q.Speed,
                Range = Q.Range,
                CollisionObjects = CollisionableObjects.Minions | CollisionableObjects.Heroes
            };
        }

        internal static void Initialize()
        {
            
        }
    }
}