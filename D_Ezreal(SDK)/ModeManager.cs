using System;
using System.Collections.Generic;

using D_Ezreal_SDK_.Modes;

using LeagueSharp.SDK;

namespace D_Ezreal_SDK_
{
    using LeagueSharp.SDK.Enumerations;
    using LeagueSharp.SDK.Utils;

    internal static class ModeManager
    {
        private static readonly List<ModeBase> Modes;

        static ModeManager()
        {
            Modes = new List<ModeBase>
            {
                new PermaActive(),
                new Combo(),
                new Harass(),
                new LaneClear(),
                new JungleClear(),

            };

            new TickOperation(0x42, () =>
            {
                if (GameObjects.Player.IsDead)
                {
                    return;
                }

                Modes.ForEach(mode =>
                {
                    try
                    {
                        if (mode.ShouldBeExecuted())
                        {
                            mode.Execute();
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Write()(LogLevel.Error, $"Error executing mode '{mode.GetType().Name}'\n{e}");
                    }
                });
            }).Start();
        }

        internal static void Initialize() { }
    }
}
