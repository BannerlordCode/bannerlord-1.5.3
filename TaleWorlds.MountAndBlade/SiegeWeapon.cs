using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035B RID: 859
	public abstract class SiegeWeapon : UsableMachine, ITargetable
	{
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003160 RID: 12640 RVA: 0x000C838A File Offset: 0x000C658A
		// (set) Token: 0x06003161 RID: 12641 RVA: 0x000C8392 File Offset: 0x000C6592
		[EditorVisibleScriptComponentVariable(false)]
		public bool ForcedUse { get; private set; }

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x000C839C File Offset: 0x000C659C
		public bool IsUsed
		{
			get
			{
				using (List<Formation>.Enumerator enumerator = base.UserFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Team.Side == this.Side)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x000C8400 File Offset: 0x000C6600
		public void SetForcedUse(bool value)
		{
			this.ForcedUse = value;
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x000C8409 File Offset: 0x000C6609
		public virtual BattleSideEnum Side
		{
			get
			{
				return BattleSideEnum.Attacker;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x000C840C File Offset: 0x000C660C
		public override TextObject HitObjectName
		{
			get
			{
				return GameTexts.FindText("str_siege_engine", this.GetSiegeEngineType().StringId);
			}
		}

		// Token: 0x06003166 RID: 12646
		public abstract SiegeEngineType GetSiegeEngineType();

		// Token: 0x06003167 RID: 12647 RVA: 0x000C8424 File Offset: 0x000C6624
		protected virtual bool CalculateIsSufficientlyManned(BattleSideEnum battleSide)
		{
			if (this.GetDetachmentWeightAux(battleSide) < 1f)
			{
				return true;
			}
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.Side == this.Side)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0 && base.IsUsedByFormation(formation) && (formation.Arrangement.UnitCount > 1 || (formation.Arrangement.UnitCount > 0 && !formation.HasPlayerControlledTroop)))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x000C8514 File Offset: 0x000C6714
		private bool HasNewMovingAgents()
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasAIMovingTo && standingPoint.PreviousUserAgent != standingPoint.MovingAgent)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000C8580 File Offset: 0x000C6780
		protected internal override void OnInit()
		{
			base.OnInit();
			this.ForcedUse = true;
			this._potentialUsingFormations = new List<Formation>();
			this._forcedUseFormations = new List<Formation>();
			base.GameEntity.SetAnimationSoundActivation(true);
			this._removeOnDeployEntities = Mission.Current.Scene.FindEntitiesWithTag(this.RemoveOnDeployTag).ToList<GameEntity>();
			this._addOnDeployEntities = Mission.Current.Scene.FindEntitiesWithTag(this.AddOnDeployTag).ToList<GameEntity>();
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (!(standingPoint is StandingPointWithWeaponRequirement))
				{
					standingPoint.AutoEquipWeaponsOnUseStopped = true;
				}
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
			WeakGameEntity firstChildEntityWithTag = base.GameEntity.GetFirstChildEntityWithTag("targeting_entity");
			if (firstChildEntityWithTag.IsValid)
			{
				Vec3 vec = base.GameEntity.ComputeGlobalPhysicsBoundingBoxCenter();
				this._targetingPositionOffset = new Vec3?(firstChildEntityWithTag.GlobalPosition - vec);
			}
			this.EnemyRangeToStopUsing = 5f;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000C86AC File Offset: 0x000C68AC
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents() && !GameNetwork.IsClientOrReplay)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x000C86E4 File Offset: 0x000C68E4
		private void TickAux(bool isParallel)
		{
			if (!GameNetwork.IsClientOrReplay && base.GameEntity.IsVisibleIncludeParents())
			{
				if (this.IsDisabledForBattleSide(this.Side))
				{
					using (List<StandingPoint>.Enumerator enumerator = base.StandingPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StandingPoint standingPoint = enumerator.Current;
							Agent userAgent = standingPoint.UserAgent;
							if (userAgent != null && !userAgent.IsPlayerControlled && userAgent.Formation != null && userAgent.Formation.Team.Side == this.Side)
							{
								if (isParallel)
								{
									this._needsSingleThreadTickOnce = true;
								}
								else
								{
									userAgent.Formation.StopUsingMachine(this, false);
									this._forcedUseFormations.Remove(userAgent.Formation);
									this._isValidated = false;
								}
							}
						}
						return;
					}
				}
				if (this.ForcedUse)
				{
					bool flag = false;
					foreach (Team team in Mission.Current.Teams)
					{
						if (team.Side == this.Side)
						{
							if (!this.CalculateIsSufficientlyManned(team.Side))
							{
								foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
								{
									if (formation.CountOfUnits > 0 && formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Retreat && (formation.Arrangement.UnitCount > 1 || (formation.Arrangement.UnitCount > 0 && !formation.HasPlayerControlledTroop)) && !formation.Detachments.Contains(this))
									{
										if (isParallel)
										{
											this._needsSingleThreadTickOnce = true;
										}
										else
										{
											this._potentialUsingFormations.Add(formation);
										}
									}
								}
								this._areMovingAgentsProcessed = false;
							}
							else if (this.HasNewMovingAgents())
							{
								if (!this._areMovingAgentsProcessed)
								{
									float num = float.MaxValue;
									Formation formation2 = null;
									foreach (Formation formation3 in team.FormationsIncludingSpecialAndEmpty)
									{
										if (formation3.CountOfUnits > 0 && formation3.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Retreat && (formation3.Arrangement.UnitCount > 1 || (formation3.Arrangement.UnitCount > 0 && !formation3.HasPlayerControlledTroop)))
										{
											WorldPosition cachedMedianPosition = formation3.CachedMedianPosition;
											Vec3 vec = base.GameEntity.GlobalPosition;
											float num2 = cachedMedianPosition.DistanceSquaredWithLimit(in vec, 10000f);
											if (num2 < num)
											{
												num = num2;
												formation2 = formation3;
											}
										}
									}
									if (formation2 != null && !base.IsUsedByFormation(formation2))
									{
										if (isParallel)
										{
											this._needsSingleThreadTickOnce = true;
										}
										else
										{
											this._potentialUsingFormations.Clear();
											this._potentialUsingFormations.Add(formation2);
											flag = true;
											this._areMovingAgentsProcessed = true;
										}
									}
									else
									{
										this._areMovingAgentsProcessed = true;
									}
								}
							}
							else
							{
								this._areMovingAgentsProcessed = false;
							}
							if (flag)
							{
								this._potentialUsingFormations[0].StartUsingMachine(this, !this._potentialUsingFormations[0].IsAIControlled);
								this._forcedUseFormations.Add(this._potentialUsingFormations[0]);
								this._potentialUsingFormations.Clear();
								this._isValidated = false;
								flag = false;
							}
							else if (this._potentialUsingFormations.Count > 0)
							{
								float num3 = float.MaxValue;
								Formation formation4 = null;
								foreach (Formation formation5 in this._potentialUsingFormations)
								{
									Vec2 cachedAveragePosition = formation5.CachedAveragePosition;
									Vec3 vec = base.GameEntity.GlobalPosition;
									float num4 = cachedAveragePosition.DistanceSquared(vec.AsVec2);
									if (num4 < num3)
									{
										num3 = num4;
										formation4 = formation5;
									}
								}
								int count = base.StandingPoints.Count;
								int num5 = 0;
								Formation formation6 = null;
								Vec2 vec2 = Vec2.Zero;
								for (int i = 0; i < count; i++)
								{
									Agent previousUserAgent = base.StandingPoints[i].PreviousUserAgent;
									if (previousUserAgent != null)
									{
										if (!previousUserAgent.IsActive() || previousUserAgent.Formation == null || (formation6 != null && previousUserAgent.Formation != formation6))
										{
											num5 = -1;
											break;
										}
										num5++;
										Vec2 vec3 = vec2;
										Vec3 vec = previousUserAgent.Position;
										vec2 = vec3 + vec.AsVec2;
										formation6 = previousUserAgent.Formation;
									}
								}
								Formation formation7 = formation4;
								if (num5 > 0 && this._potentialUsingFormations.Contains(formation6))
								{
									vec2 *= 1f / (float)num5;
									Vec3 vec = base.GameEntity.GlobalPosition;
									if (vec2.DistanceSquared(vec.AsVec2) < num3)
									{
										formation7 = formation6;
									}
								}
								formation7.StartUsingMachine(this, !formation7.IsAIControlled);
								this._forcedUseFormations.Add(formation7);
								this._potentialUsingFormations.Clear();
								this._isValidated = false;
							}
							else if (!this._isValidated)
							{
								if (!this.HasToBeDefendedByUser(team.Side) && this.GetDetachmentWeightAux(team.Side) == -3.4028235E+38f)
								{
									for (int j = this._forcedUseFormations.Count - 1; j >= 0; j--)
									{
										Formation formation8 = this._forcedUseFormations[j];
										if (formation8.Team.Side == this.Side && !this.IsAnyUserBelongsToFormation(formation8))
										{
											if (isParallel)
											{
												if (base.IsUsedByFormation(formation8))
												{
													this._needsSingleThreadTickOnce = true;
													break;
												}
												this._forcedUseFormations.Remove(formation8);
											}
											else
											{
												if (base.IsUsedByFormation(formation8))
												{
													formation8.StopUsingMachine(this, !formation8.IsAIControlled);
												}
												this._forcedUseFormations.Remove(formation8);
											}
										}
									}
									if (isParallel && this._needsSingleThreadTickOnce)
									{
										break;
									}
								}
								if (!isParallel)
								{
									this._isValidated = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x000C8D3C File Offset: 0x000C6F3C
		protected virtual bool IsAnyUserBelongsToFormation(Formation formation)
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.UserAgent != null && standingPoint.UserAgent.Formation == formation)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x000C8DA8 File Offset: 0x000C6FA8
		protected internal override void OnTickParallel(float dt)
		{
			this.TickAux(true);
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x000C8DB1 File Offset: 0x000C6FB1
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				this.TickAux(false);
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x000C8DD0 File Offset: 0x000C6FD0
		public void TickAuxForInit()
		{
			this.TickAux(false);
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000C8DDC File Offset: 0x000C6FDC
		protected internal virtual void OnDeploymentStateChanged(bool isDeployed)
		{
			foreach (GameEntity gameEntity in this._removeOnDeployEntities)
			{
				gameEntity.SetVisibilityExcludeParents(!isDeployed);
				StrategicArea firstScriptOfType = gameEntity.GetFirstScriptOfType<StrategicArea>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.OnParentGameEntityVisibilityChanged(!isDeployed);
				}
				else
				{
					foreach (StrategicArea strategicArea in from c in gameEntity.GetChildren()
						where c.HasScriptOfType<StrategicArea>()
						select c.GetFirstScriptOfType<StrategicArea>())
					{
						strategicArea.OnParentGameEntityVisibilityChanged(!isDeployed);
					}
				}
			}
			foreach (GameEntity gameEntity2 in this._addOnDeployEntities)
			{
				gameEntity2.SetVisibilityExcludeParents(isDeployed);
				MissionObject firstScriptOfType2 = gameEntity2.GetFirstScriptOfType<MissionObject>();
				if (firstScriptOfType2 != null)
				{
					firstScriptOfType2.SetAbilityOfFaces(isDeployed);
				}
				StrategicArea firstScriptOfType3 = gameEntity2.GetFirstScriptOfType<StrategicArea>();
				if (firstScriptOfType3 != null)
				{
					firstScriptOfType3.OnParentGameEntityVisibilityChanged(isDeployed);
				}
				else
				{
					foreach (StrategicArea strategicArea2 in from c in gameEntity2.GetChildren()
						where c.HasScriptOfType<StrategicArea>()
						select c.GetFirstScriptOfType<StrategicArea>())
					{
						strategicArea2.OnParentGameEntityVisibilityChanged(isDeployed);
					}
				}
			}
			if (this._addOnDeployEntities.Count > 0 || this._removeOnDeployEntities.Count > 0)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.RefreshGameEntityWithWorldPosition();
				}
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x000C9028 File Offset: 0x000C7228
		public override bool HasWaitFrame
		{
			get
			{
				return base.HasWaitFrame && (!(this is IPrimarySiegeWeapon) || !(this as IPrimarySiegeWeapon).HasCompletedAction());
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x000C904C File Offset: 0x000C724C
		public override bool IsDeactivated
		{
			get
			{
				return base.IsDisabled || !base.GameEntity.IsValid || !base.GameEntity.IsVisibleIncludeParents() || base.IsDeactivated;
			}
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x000C9089 File Offset: 0x000C7289
		public override bool ShouldAutoLeaveDetachmentWhenDisabled(BattleSideEnum sideEnum)
		{
			return this.AutoAttachUserToFormation(sideEnum);
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000C9092 File Offset: 0x000C7292
		public override bool AutoAttachUserToFormation(BattleSideEnum sideEnum)
		{
			return base.Ai.HasActionCompleted || !base.IsDisabledDueToEnemyInRange(sideEnum);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000C90AD File Offset: 0x000C72AD
		public override bool HasToBeDefendedByUser(BattleSideEnum sideEnum)
		{
			return !base.Ai.HasActionCompleted && base.IsDisabledDueToEnemyInRange(sideEnum);
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000C90C8 File Offset: 0x000C72C8
		protected float GetUserMultiplierOfWeapon()
		{
			int userCountIncludingInStruckAction = base.UserCountIncludingInStruckAction;
			if (userCountIncludingInStruckAction == 0)
			{
				return 0f;
			}
			return 0.7f + 0.3f * (float)userCountIncludingInStruckAction / (float)this.MaxUserCount;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x000C90FB File Offset: 0x000C72FB
		protected virtual float GetDistanceMultiplierOfWeapon(Vec3 weaponPos)
		{
			if (this.GetMinimumDistanceBetweenPositions(weaponPos) > 20f)
			{
				return 0.4f;
			}
			Debug.FailedAssert("Invalid weapon type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SiegeWeapon.cs", "GetDistanceMultiplierOfWeapon", 549);
			return 1f;
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x000C9130 File Offset: 0x000C7330
		protected virtual float GetMinimumDistanceBetweenPositions(Vec3 position)
		{
			return base.GameEntity.GlobalPosition.DistanceSquared(position);
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x000C9154 File Offset: 0x000C7354
		protected float GetHitPointMultiplierOfWeapon()
		{
			if (base.DestructionComponent != null)
			{
				return MathF.Max(1f, 2f - MathF.Log10(base.DestructionComponent.HitPoint / base.DestructionComponent.MaxHitPoint * 10f + 1f));
			}
			return 1f;
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x000C91A7 File Offset: 0x000C73A7
		public WeakGameEntity GetTargetEntity()
		{
			return base.GameEntity;
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x000C91AF File Offset: 0x000C73AF
		public Vec3 GetTargetingOffset()
		{
			if (this._targetingPositionOffset != null)
			{
				return this._targetingPositionOffset.Value;
			}
			return Vec3.Zero;
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x000C91CF File Offset: 0x000C73CF
		public BattleSideEnum GetSide()
		{
			return this.Side;
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000C91D8 File Offset: 0x000C73D8
		public Vec3 GetTargetGlobalVelocity()
		{
			IMoveableSiegeWeapon moveableSiegeWeapon = this as IMoveableSiegeWeapon;
			if (moveableSiegeWeapon != null)
			{
				return moveableSiegeWeapon.MovementComponent.Velocity;
			}
			return Vec3.Zero;
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000C9200 File Offset: 0x000C7400
		public bool IsDestructable()
		{
			return base.GameEntity.HasScriptOfType<DestructableComponent>();
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000C921B File Offset: 0x000C741B
		public WeakGameEntity Entity()
		{
			return base.GameEntity;
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x000C9224 File Offset: 0x000C7424
		public ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax()
		{
			return base.GameEntity.ComputeGlobalPhysicsBoundingBoxMinMax();
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x000C923F File Offset: 0x000C743F
		public virtual void OnShipCaptured(BattleSideEnum newDefaultSide)
		{
		}

		// Token: 0x06003182 RID: 12674
		public abstract TargetFlags GetTargetFlags();

		// Token: 0x06003183 RID: 12675
		public abstract float GetTargetValue(List<Vec3> weaponPos);

		// Token: 0x040014CD RID: 5325
		private const string TargetingEntityTag = "targeting_entity";

		// Token: 0x040014CE RID: 5326
		[EditableScriptComponentVariable(true, "")]
		internal string RemoveOnDeployTag = "";

		// Token: 0x040014CF RID: 5327
		[EditableScriptComponentVariable(true, "")]
		internal string AddOnDeployTag = "";

		// Token: 0x040014D0 RID: 5328
		private List<GameEntity> _addOnDeployEntities;

		// Token: 0x040014D2 RID: 5330
		protected bool _spawnedFromSpawner;

		// Token: 0x040014D3 RID: 5331
		private List<GameEntity> _removeOnDeployEntities;

		// Token: 0x040014D4 RID: 5332
		private List<Formation> _potentialUsingFormations;

		// Token: 0x040014D5 RID: 5333
		private List<Formation> _forcedUseFormations;

		// Token: 0x040014D6 RID: 5334
		private bool _needsSingleThreadTickOnce;

		// Token: 0x040014D7 RID: 5335
		private bool _areMovingAgentsProcessed;

		// Token: 0x040014D8 RID: 5336
		private bool _isValidated;

		// Token: 0x040014D9 RID: 5337
		private Vec3? _targetingPositionOffset;
	}
}
