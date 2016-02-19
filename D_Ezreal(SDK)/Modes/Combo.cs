﻿
using System.Collections.Generic;
using System.Linq;

using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;

using SharpDX;

using Collision = LeagueSharp.SDK.Collision;
using Settings = D_Ezreal_SDK_.Config.Modes.Combo;
/*internal static bool UseQ => _useQ.Value;

internal static bool UseW => _useW.Value;

internal static bool UseR => _useR.Value;

internal static bool UseRE => _useRE.Value;

internal static int Mintargets => _minTargets.Value;

internal static int Minrange => _minrange.Value;

internal static int Maxrange => _maxrange.Value;*/

namespace D_Ezreal_SDK_.Modes
{
    using System;
    using System.Runtime.Remoting.Messaging;

    using LeagueSharp.SDK.Core.Wrappers.Damages;

    internal sealed class Combo : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return Config.Keys.ComboActive;
        }

        internal override void Execute()
        {
            var ti = Variables.TargetSelector.GetTarget(Q);
            if (Items.HasItem(3146) && Items.CanUseItem(3146) && Config.Modes.Items.Offensive.Hextech
                && ti.IsValidTarget(700) && ti.HealthPercent <= Config.Modes.Items.Offensive.HextechEnemyhp)
            {
                Items.UseItem(3146, ti);
            }

            if (Items.HasItem(3142) && Items.CanUseItem(3142) && Config.Modes.Items.Offensive.Youmuu
                && ti.IsValidTarget(500))
            {
                Items.UseItem(3142);
            }

            if ((Items.HasItem(3144) && Items.CanUseItem(3144) || Items.HasItem(3153) && Items.CanUseItem(3153))
                && Config.Modes.Items.Offensive.Blade && ti.IsValidTarget(450)
                && (ti.HealthPercent <= Config.Modes.Items.Offensive.BladeEnemyhp
                    || GameObjects.Player.HealthPercent <= Config.Modes.Items.Offensive.Blademyhp))
            {
                Items.UseItem(3144, ti);
                Items.UseItem(3153, ti);
            }

            if (Q.IsReady() && Settings.UseQ)
            {
                var target = Variables.TargetSelector.GetTarget(Q);
                if (target != null)
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

            if (W.IsReady() && Settings.UseW)
            {
                var target = Variables.TargetSelector.GetTarget(W);
                if (target != null)
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
            if (R.IsReady() && Settings.UseR)
            {
                var target = Variables.TargetSelector.GetTarget(R);
                if (target != null)
                {
                    var prediction =
                        Movement.GetPrediction(
                            new PredictionInput
                            {
                                Unit = target,
                                Delay = R.Delay,
                                Radius = R.Width,
                                Speed = R.Speed,
                                Range = R.Range
                            });

                    {
                        if (Q.IsReady() && W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQW(true))
                            && target.IsValidTarget(Q.Range))
                            return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true))
                            && target.IsValidTarget(Q.Range)) return;
                        if (W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithW(true))
                            && target.IsValidTarget(W.Range)) return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQAuto(true))
                           && target.IsValidTarget(Q.Range))
                            return;
                        if (W.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithWAuto(true))
                            && target.IsValidTarget(W.Range))
                            return;
                        if (target.DistanceToPlayer() < target.GetRealAutoAttackRange()
                            && target.Health <= GameObjects.Player.GetAutoAttackDamage(target)) return;
                        if (prediction.Hitchance >= HitChance.High
                            && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithR(true)) && !target.IsDead && target.DistanceToPlayer() > Config.Modes.Combo.Minrange) R.Cast(prediction.CastPosition);
                    }
                }
            }
        }
    }
}

    
