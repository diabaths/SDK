﻿using System.Linq;
using System;
using LeagueSharp;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Wrappers.Damages;
using LeagueSharp.SDK;

using Settings = Cait.Config.Modes.Misc;

namespace Cait.Modes
{
    internal sealed class PermaActive : ModeBase
    {
        internal override bool ShouldBeExecuted()
        {
            return true;
        }

        internal override void Execute()
        {
            if (E.IsReady() && Settings._emouse.Active)
            {
                E.Cast(GameObjects.Player.Position.Extend(Game.CursorPos, -(E.Range / 2)));
               GameObjects.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }

            if (Q.IsReady())
            {
                var target = Variables.TargetSelector.GetTarget(Q);
                if (target.IsValidTarget(Q.Range))
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
                    if (prediction.Hitchance >= HitChance.High
                        && target.DistanceToPlayer() > target.GetRealAutoAttackRange())
                    {
                        if (Settings.useQK && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true)))
                        {
                            Q.Cast(prediction.CastPosition);
                            Modes.Combo.castR = Environment.TickCount;
                        }
                    }

                    if (Settings.useQimmo)
                        if (prediction.Hitchance == HitChance.Immobile
                            && target.DistanceToPlayer() > target.GetRealAutoAttackRange())
                        {
                            Q.Cast(prediction.CastPosition);
                            Modes.Combo.castR = Environment.TickCount;
                        }

                    if (Settings.autoq && (target.HasBuff("caitlynyordletrapinternal") || target.HasBuff("caitlynyordletrapdebuff"))
                        && target.DistanceToPlayer() > target.GetRealAutoAttackRange())
                    {
                        Q.Cast(target);
                        Modes.Combo.castR = Environment.TickCount;
                    }
                }
            }

            if (W.IsReady() && Settings.usews && Environment.TickCount - Modes.Combo.castW > 1300 && !GameObjects.Player.IsAttackingPlayer)
            {
                var target = Variables.TargetSelector.GetTarget(W);
                if (target.IsValidTarget(W.Range))
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
                    if (prediction.Hitchance >= HitChance.High
                        && (target.HasBuffOfType(BuffType.Suppression) && target.HasBuffOfType(BuffType.Stun)
                            || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup)))
                    {
                        W.Cast(prediction.CastPosition);
                        Modes.Combo.castR = Environment.TickCount;
                        Modes.Combo.castW = Environment.TickCount;
                    }
                }
            }

            if (R.IsReady() && Settings.useRM && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithR(true)) && Environment.TickCount - Modes.Combo.castR > 700)
            {
                var target = Variables.TargetSelector.GetTarget(R.Range);
                if (target.IsValidTarget(R.Range))
                {
                    var pred = R.GetPrediction(target);
                    if (!pred.CollisionObjects.Any(obj => obj is Obj_AI_Hero))
                    {
                        if (target.IsValidTarget(R.Range) && Settings._semiR.Active
                            && target.DistanceToPlayer() > 1200)
                        {
                            R.Cast(target);
                        }

                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQ(true))
                            && target.IsValidTarget(Q.Range)) return;
                        if (Q.IsReady() && GameObjects.EnemyHeroes.Any(x => x.IsKillableWithQAuto(true))
                            && target.IsValidTarget(Q.Range)) return;
                        if (target.DistanceToPlayer() < target.GetRealAutoAttackRange()
                            && target.Health <= GameObjects.Player.GetAutoAttackDamage(target)) return;
                        if (target.DistanceToPlayer() > 1200)
                        {
                            R.Cast(target);
                        }
                    }
                }
            }
        }
    }
}