using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade.DividableTasks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000163 RID: 355
	public abstract class RangedSiegeWeaponAi : UsableMachineAIBase
	{
		// Token: 0x0600127E RID: 4734 RVA: 0x00039DA0 File Offset: 0x00037FA0
		public RangedSiegeWeaponAi(RangedSiegeWeapon rangedSiegeWeapon)
			: base(rangedSiegeWeapon)
		{
			this._threatSeeker = new RangedSiegeWeaponAi.ThreatSeeker(rangedSiegeWeapon);
			((RangedSiegeWeapon)this.UsableMachine).OnReloadDone += this.FindNextTarget;
			this._delayTimer = this._delayDuration;
			this._targetEvaluationTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00039E0E File Offset: 0x0003800E
		public void ResetThreatSeeker()
		{
			this._threatSeeker.ResetTargetableObjects();
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00039E1C File Offset: 0x0003801C
		protected override void OnTick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			base.OnTick(agentToCompareTo, formationToCompareTo, potentialUsersTeam, dt);
			if (this.UsableMachine.PilotAgent != null && this.UsableMachine.PilotAgent.IsAIControlled)
			{
				RangedSiegeWeapon rangedSiegeWeapon = this.UsableMachine as RangedSiegeWeapon;
				if (rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.WaitingAfterShooting && rangedSiegeWeapon.PilotAgent != null && rangedSiegeWeapon.PilotAgent.IsAIControlled)
				{
					rangedSiegeWeapon.AiRequestsManualReload();
				}
				this.UpdateAim(rangedSiegeWeapon, dt);
			}
			this.AfterTick(agentToCompareTo, formationToCompareTo, potentialUsersTeam, dt);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00039E98 File Offset: 0x00038098
		protected virtual void UpdateAim(RangedSiegeWeapon rangedSiegeWeapon, float dt)
		{
			if (this._threatSeeker.UpdateThreatSeekerTask() && dt > 0f && this._target == null && rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.Idle)
			{
				if (this._delayTimer <= 0f)
				{
					this.FindNextTarget();
				}
				this._delayTimer -= dt;
			}
			if (this._target != null)
			{
				if (this._target.Agent != null && !this._target.Agent.IsActive())
				{
					this._target = null;
					return;
				}
				if (rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.Idle && rangedSiegeWeapon.UserCountNotInStruckAction > 0)
				{
					if (DebugSiegeBehavior.ToggleTargetDebug && this.UsableMachine.PilotAgent != null)
					{
						this._target.ComputeGlobalTargetingBoundingBoxMinMax();
						Vec3 targetingPosition = this._target.TargetingPosition;
					}
					if (this._targetEvaluationTimer.Check(Mission.Current.CurrentTime) && !((RangedSiegeWeapon)this.UsableMachine).CanShootAtThreat(this._target, 5))
					{
						this._cannotShootCounter++;
					}
					if (this._cannotShootCounter >= 4)
					{
						this._target = null;
						this.SetTargetingTimer();
						this._cannotShootCounter = 0;
						return;
					}
					if (rangedSiegeWeapon.AimAtThreat(this._target) && rangedSiegeWeapon.PilotAgent != null)
					{
						this._delayTimer -= dt;
						if (this._delayTimer <= 0f && rangedSiegeWeapon.CanShootAtThreat(this._target, 5))
						{
							rangedSiegeWeapon.AiRequestsShoot();
							this._target = null;
							this.SetTargetingTimer();
							this._cannotShootCounter = 0;
							this._targetEvaluationTimer.Reset(Mission.Current.CurrentTime);
							return;
						}
					}
				}
				else
				{
					this._targetEvaluationTimer.Reset(Mission.Current.CurrentTime);
				}
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0003A047 File Offset: 0x00038247
		private void SetTargetFromThreatSeeker()
		{
			this._target = this._threatSeeker.PrepareTargetFromTask();
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0003A05A File Offset: 0x0003825A
		public void FindNextTarget()
		{
			if (this.UsableMachine.PilotAgent != null && this.UsableMachine.PilotAgent.IsAIControlled)
			{
				this._threatSeeker.PrepareThreatSeekerTask(new Action(this.SetTargetFromThreatSeeker));
				this.SetTargetingTimer();
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0003A098 File Offset: 0x00038298
		private void AfterTick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			if ((dt <= 0f || (agentToCompareTo != null && this.UsableMachine.PilotAgent != agentToCompareTo) || (formationToCompareTo != null && (this.UsableMachine.PilotAgent == null || !this.UsableMachine.PilotAgent.IsAIControlled || this.UsableMachine.PilotAgent.Formation != formationToCompareTo))) && this.UsableMachine.PilotAgent == null)
			{
				this._threatSeeker.Release();
				this._target = null;
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0003A113 File Offset: 0x00038313
		private void SetTargetingTimer()
		{
			this._delayTimer = this._delayDuration + MBRandom.RandomFloat * 0.5f;
		}

		// Token: 0x04000483 RID: 1155
		private const float TargetEvaluationDelay = 0.5f;

		// Token: 0x04000484 RID: 1156
		private const int MaxTargetEvaluationCount = 4;

		// Token: 0x04000485 RID: 1157
		public const string ForceTargetEntityTag = "attackMe";

		// Token: 0x04000486 RID: 1158
		private readonly RangedSiegeWeaponAi.ThreatSeeker _threatSeeker;

		// Token: 0x04000487 RID: 1159
		private Threat _target;

		// Token: 0x04000488 RID: 1160
		private float _delayTimer;

		// Token: 0x04000489 RID: 1161
		private float _delayDuration = 1f;

		// Token: 0x0400048A RID: 1162
		private int _cannotShootCounter;

		// Token: 0x0400048B RID: 1163
		private readonly Timer _targetEvaluationTimer;

		// Token: 0x02000490 RID: 1168
		public class ThreatSeeker
		{
			// Token: 0x06003A0E RID: 14862 RVA: 0x000ECC54 File Offset: 0x000EAE54
			public ThreatSeeker(RangedSiegeWeapon weapon)
			{
				this.Weapon = weapon;
				this.WeaponPositions = new List<Vec3> { this.Weapon.GameEntity.GlobalPosition };
				this._targetAgent = null;
				this._getMostDangerousThreat = new FindMostDangerousThreat(null);
			}

			// Token: 0x06003A0F RID: 14863 RVA: 0x000ECCA8 File Offset: 0x000EAEA8
			public void ResetTargetableObjects()
			{
				IEnumerable<MissionObject> enumerable = Mission.Current.ActiveMissionObjects.WhereQ<MissionObject>((MissionObject mo) => mo is ITargetable);
				this._potentialTargetObjects = (from to in enumerable.WhereQ<MissionObject>(delegate(MissionObject to)
					{
						ITargetable targetable;
						return (targetable = to as ITargetable) != null && targetable.IsDestructable() && targetable.GetTargetEntity() != null;
					})
					select to as ITargetable).ToList<ITargetable>();
				this._referencePositions = enumerable.OfType<ICastleKeyPosition>().ToList<ICastleKeyPosition>();
			}

			// Token: 0x06003A10 RID: 14864 RVA: 0x000ECD4C File Offset: 0x000EAF4C
			public Threat PrepareTargetFromTask()
			{
				Agent agent2;
				this._currentThreat = this._getMostDangerousThreat.GetResult(out agent2);
				if (this._currentThreat != null && this._currentThreat.TargetableObject == null)
				{
					this._currentThreat.Agent = this._targetAgent;
					if (this._targetAgent == null || !this._targetAgent.IsActive() || this._targetAgent.Formation != this._currentThreat.Formation || !this.Weapon.CanShootAtAgent(this._targetAgent, 5))
					{
						this._targetAgent = agent2;
						float selectedAgentScore = float.MaxValue;
						Agent selectedAgent = this._targetAgent;
						Action<Agent> action = delegate(Agent agent)
						{
							float num = agent.Position.DistanceSquared(this.Weapon.GameEntity.GlobalPosition) * (MBRandom.RandomFloat * 0.2f + 0.8f);
							if (agent == this._targetAgent)
							{
								num *= 0.5f;
							}
							if (selectedAgentScore > num && this.Weapon.CanShootAtAgent(agent, 5))
							{
								selectedAgent = agent;
								selectedAgentScore = num;
							}
						};
						if (agent2.Detachment == null)
						{
							this._currentThreat.Formation.ApplyActionOnEachAttachedUnit(action);
						}
						else
						{
							this._currentThreat.Formation.ApplyActionOnEachDetachedUnit(action);
						}
						this._targetAgent = selectedAgent ?? this._currentThreat.Formation.GetUnitWithIndex(MBRandom.RandomInt(this._currentThreat.Formation.CountOfUnits));
						this._currentThreat.Agent = this._targetAgent;
					}
				}
				if (this._currentThreat != null && this._currentThreat.TargetableObject == null && this._currentThreat.Agent == null)
				{
					this._currentThreat = null;
				}
				return this._currentThreat;
			}

			// Token: 0x06003A11 RID: 14865 RVA: 0x000ECEB5 File Offset: 0x000EB0B5
			public bool UpdateThreatSeekerTask()
			{
				Agent targetAgent = this._targetAgent;
				if (targetAgent != null && !targetAgent.IsActive())
				{
					this._targetAgent = null;
				}
				return this._getMostDangerousThreat.Update();
			}

			// Token: 0x06003A12 RID: 14866 RVA: 0x000ECEE0 File Offset: 0x000EB0E0
			public void PrepareThreatSeekerTask(Action lastAction)
			{
				this._getMostDangerousThreat.Prepare(this.GetAllThreats(), this.Weapon);
				this._getMostDangerousThreat.SetLastAction(lastAction);
			}

			// Token: 0x06003A13 RID: 14867 RVA: 0x000ECF05 File Offset: 0x000EB105
			public void Release()
			{
				this._targetAgent = null;
				this._currentThreat = null;
			}

			// Token: 0x06003A14 RID: 14868 RVA: 0x000ECF18 File Offset: 0x000EB118
			public List<Threat> GetAllThreats()
			{
				List<Threat> list = new List<Threat>();
				for (int i = this._potentialTargetObjects.Count - 1; i >= 0; i--)
				{
					ITargetable targetable = this._potentialTargetObjects[i];
					UsableMachine usableMachine;
					MissionObject missionObject;
					if (((usableMachine = targetable as UsableMachine) != null && (usableMachine.IsDestroyed || usableMachine.IsDeactivated || !usableMachine.GameEntity.IsValid)) || ((missionObject = targetable as MissionObject) != null && missionObject.IsDisabled) || targetable.GetSide() == this.Weapon.Side)
					{
						this._potentialTargetObjects.RemoveAt(i);
					}
					else
					{
						Threat threat = new Threat
						{
							TargetableObject = targetable,
							ThreatValue = this.Weapon.ProcessTargetValue(targetable.GetTargetValue(this.WeaponPositions), targetable.GetTargetFlags()),
							ForceTarget = targetable.Entity().HasTag("attackMe")
						};
						list.Add(threat);
					}
				}
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.Side.GetOppositeSide() == this.Weapon.Side)
					{
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							if (formation.CountOfUnits > 0)
							{
								float targetValueOfFormation = RangedSiegeWeaponAi.ThreatSeeker.GetTargetValueOfFormation(formation, this._referencePositions);
								if (targetValueOfFormation != -1f)
								{
									list.Add(new Threat
									{
										Formation = formation,
										ThreatValue = this.Weapon.ProcessTargetValue(targetValueOfFormation, RangedSiegeWeaponAi.ThreatSeeker.GetTargetFlagsOfFormation()),
										ForceTarget = false
									});
								}
							}
						}
					}
				}
				return list;
			}

			// Token: 0x06003A15 RID: 14869 RVA: 0x000ED104 File Offset: 0x000EB304
			private static float GetTargetValueOfFormation(Formation formation, IEnumerable<ICastleKeyPosition> referencePositions)
			{
				if (formation.QuerySystem.LocalEnemyPower / formation.QuerySystem.LocalAllyPower > 0.5f)
				{
					return -1f;
				}
				float num = (float)formation.CountOfUnits * 3f;
				if (TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
				{
					num *= 3f;
				}
				num *= RangedSiegeWeaponAi.ThreatSeeker.GetPositionMultiplierOfFormation(formation, referencePositions);
				float num2 = MBMath.ClampFloat(formation.QuerySystem.LocalAllyPower / (formation.QuerySystem.LocalEnemyPower + 0.01f), 0f, 5f) / 5f;
				return num * num2;
			}

			// Token: 0x06003A16 RID: 14870 RVA: 0x000ED19B File Offset: 0x000EB39B
			public static TargetFlags GetTargetFlagsOfFormation()
			{
				return TargetFlags.None | TargetFlags.IsMoving | TargetFlags.IsFlammable | TargetFlags.IsAttacker;
			}

			// Token: 0x06003A17 RID: 14871 RVA: 0x000ED1A8 File Offset: 0x000EB3A8
			private static float GetPositionMultiplierOfFormation(Formation formation, IEnumerable<ICastleKeyPosition> referencePositions)
			{
				ICastleKeyPosition castleKeyPosition;
				float minimumDistanceBetweenPositions = RangedSiegeWeaponAi.ThreatSeeker.GetMinimumDistanceBetweenPositions(formation.GetMedianAgent(false, false, formation.GetAveragePositionOfUnits(false, false)).Position, referencePositions, out castleKeyPosition);
				bool flag = castleKeyPosition != null && castleKeyPosition.AttackerSiegeWeapon != null && castleKeyPosition.AttackerSiegeWeapon.HasCompletedAction();
				float num;
				if (formation.PhysicalClass.IsRanged())
				{
					if (minimumDistanceBetweenPositions < 20f)
					{
						num = 1f;
					}
					else if (minimumDistanceBetweenPositions < 35f)
					{
						num = 0.8f;
					}
					else
					{
						num = 0.6f;
					}
					return num + (flag ? 0.2f : 0f);
				}
				if (minimumDistanceBetweenPositions < 15f)
				{
					num = 0.2f;
				}
				else if (minimumDistanceBetweenPositions < 40f)
				{
					num = 0.15f;
				}
				else
				{
					num = 0.12f;
				}
				return num * (flag ? 7.5f : 1f);
			}

			// Token: 0x06003A18 RID: 14872 RVA: 0x000ED26C File Offset: 0x000EB46C
			private static float GetMinimumDistanceBetweenPositions(Vec3 position, IEnumerable<ICastleKeyPosition> referencePositions, out ICastleKeyPosition closestCastlePosition)
			{
				if (referencePositions != null && referencePositions.Count<ICastleKeyPosition>() != 0)
				{
					closestCastlePosition = referencePositions.MinBy<ICastleKeyPosition, float>((ICastleKeyPosition rp) => rp.GetPosition().DistanceSquared(position));
					return MathF.Sqrt(closestCastlePosition.GetPosition().DistanceSquared(position));
				}
				closestCastlePosition = null;
				return -1f;
			}

			// Token: 0x06003A19 RID: 14873 RVA: 0x000ED2C8 File Offset: 0x000EB4C8
			public static Threat GetMaxThreat(List<ICastleKeyPosition> castleKeyPositions)
			{
				List<ITargetable> list = new List<ITargetable>();
				List<Threat> list2 = new List<Threat>();
				foreach (WeakGameEntity weakGameEntity in Mission.Current.ActiveMissionObjects.Select<MissionObject, WeakGameEntity>((MissionObject amo) => amo.GameEntity))
				{
					ITargetable targetable;
					if ((targetable = weakGameEntity.GetFirstScriptOfType<UsableMachine>() as ITargetable) != null)
					{
						list.Add(targetable);
					}
				}
				list.RemoveAll((ITargetable um) => um.GetSide() == BattleSideEnum.Defender);
				list2.AddRange(list.Select<ITargetable, Threat>(delegate(ITargetable um)
				{
					Threat threat = new Threat();
					threat.TargetableObject = um;
					threat.ThreatValue = um.GetTargetValue(castleKeyPositions.Select<ICastleKeyPosition, Vec3>((ICastleKeyPosition c) => c.GetPosition()).ToList<Vec3>());
					threat.ForceTarget = um.Entity().HasTag("attackMe");
					return threat;
				}));
				return list2.MaxBy<Threat, float>((Threat t) => t.ThreatValue);
			}

			// Token: 0x04001B25 RID: 6949
			private FindMostDangerousThreat _getMostDangerousThreat;

			// Token: 0x04001B26 RID: 6950
			private const float SingleUnitThreatValue = 3f;

			// Token: 0x04001B27 RID: 6951
			private const float InsideWallsThreatMultiplier = 3f;

			// Token: 0x04001B28 RID: 6952
			private Threat _currentThreat;

			// Token: 0x04001B29 RID: 6953
			private Agent _targetAgent;

			// Token: 0x04001B2A RID: 6954
			public RangedSiegeWeapon Weapon;

			// Token: 0x04001B2B RID: 6955
			public List<Vec3> WeaponPositions;

			// Token: 0x04001B2C RID: 6956
			private List<ITargetable> _potentialTargetObjects;

			// Token: 0x04001B2D RID: 6957
			private List<ICastleKeyPosition> _referencePositions;
		}
	}
}
