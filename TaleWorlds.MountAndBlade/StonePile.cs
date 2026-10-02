using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000384 RID: 900
	public class StonePile : UsableMachine, IDetachment
	{
		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06003362 RID: 13154 RVA: 0x000D3072 File Offset: 0x000D1272
		// (set) Token: 0x06003363 RID: 13155 RVA: 0x000D307A File Offset: 0x000D127A
		public int AmmoCount { get; protected set; }

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003364 RID: 13156 RVA: 0x000D3084 File Offset: 0x000D1284
		public bool HasThrowingPointUsed
		{
			get
			{
				foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
				{
					if (throwingPoint.StandingPoint.HasUser || throwingPoint.StandingPoint.HasAIMovingTo || (throwingPoint.WaitingPoint != null && (throwingPoint.WaitingPoint.HasUser || throwingPoint.WaitingPoint.HasAIMovingTo)))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06003365 RID: 13157 RVA: 0x000D3114 File Offset: 0x000D1314
		public virtual BattleSideEnum Side
		{
			get
			{
				return BattleSideEnum.Defender;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003366 RID: 13158 RVA: 0x000D3117 File Offset: 0x000D1317
		public override int MaxUserCount
		{
			get
			{
				return this._throwingPoints.Count;
			}
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000D3124 File Offset: 0x000D1324
		protected StonePile()
		{
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x000D314C File Offset: 0x000D134C
		protected void ConsumeAmmo()
		{
			int ammoCount = this.AmmoCount;
			this.AmmoCount = ammoCount - 1;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetStonePileAmmo(base.Id, this.AmmoCount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.UpdateAmmoMesh();
			this.CheckAmmo();
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x000D319F File Offset: 0x000D139F
		public void SetAmmo(int ammoLeft)
		{
			if (this.AmmoCount != ammoLeft)
			{
				this.AmmoCount = ammoLeft;
				this.UpdateAmmoMesh();
				this.CheckAmmo();
			}
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x000D31C0 File Offset: 0x000D13C0
		protected virtual void CheckAmmo()
		{
			if (this.AmmoCount <= 0)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.IsDeactivated = true;
				}
			}
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x000D321C File Offset: 0x000D141C
		protected internal override void OnInit()
		{
			base.OnInit();
			this._tickOccasionallyTimer = new Timer(0f, 0.5f + MBRandom.RandomFloat * 0.5f, true);
			this._givenItem = Game.Current.ObjectManager.GetObject<ItemObject>(this.GivenItemID);
			MBList<VolumeBox> mblist = base.GameEntity.CollectScriptComponentsIncludingChildrenRecursive<VolumeBox>();
			this._throwingPoints = new List<StonePile.ThrowingPoint>();
			this._volumeBoxTimerPairs = new List<StonePile.VolumeBoxTimerPair>();
			foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in base.StandingPoints.OfType<StandingPointWithWeaponRequirement>())
			{
				if (standingPointWithWeaponRequirement.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					standingPointWithWeaponRequirement.InitGivenWeapon(this._givenItem);
					standingPointWithWeaponRequirement.SetHasAlternative(true);
					standingPointWithWeaponRequirement.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
				}
				else if (standingPointWithWeaponRequirement.GameEntity.HasTag("throwing"))
				{
					standingPointWithWeaponRequirement.InitRequiredWeapon(this._givenItem);
					StonePile.ThrowingPoint throwingPoint = new StonePile.ThrowingPoint();
					throwingPoint.StandingPoint = standingPointWithWeaponRequirement as StandingPointWithVolumeBox;
					throwingPoint.AmmoPickUpPoint = null;
					throwingPoint.AttackEntity = null;
					throwingPoint.AttackEntityNearbyAgentsCheckRadius = 0f;
					List<StandingPointWithWeaponRequirement> list = standingPointWithWeaponRequirement.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPointWithWeaponRequirement>("wait_to_throw");
					if (list != null && list.Count > 0)
					{
						throwingPoint.WaitingPoint = list[0];
						throwingPoint.WaitingPoint.InitRequiredWeapon(this._givenItem);
					}
					else
					{
						throwingPoint.WaitingPoint = null;
					}
					bool flag = false;
					int num = 0;
					while (num < this._volumeBoxTimerPairs.Count && !flag)
					{
						if (this._volumeBoxTimerPairs[num].VolumeBox.GameEntity.HasTag(throwingPoint.StandingPoint.VolumeBoxTag))
						{
							throwingPoint.EnemyInRangeTimer = this._volumeBoxTimerPairs[num].Timer;
							flag = true;
						}
						num++;
					}
					if (!flag)
					{
						VolumeBox volumeBox = mblist.FirstOrDefault<VolumeBox>((VolumeBox vb) => vb.GameEntity.HasTag(throwingPoint.StandingPoint.VolumeBoxTag));
						StonePile.VolumeBoxTimerPair volumeBoxTimerPair = default(StonePile.VolumeBoxTimerPair);
						volumeBoxTimerPair.VolumeBox = volumeBox;
						volumeBoxTimerPair.Timer = new Timer(-3.5f, 0.5f, false);
						throwingPoint.EnemyInRangeTimer = volumeBoxTimerPair.Timer;
						this._volumeBoxTimerPairs.Add(volumeBoxTimerPair);
					}
					this._throwingPoints.Add(throwingPoint);
				}
			}
			this.EnemyRangeToStopUsing = 5f;
			this.AmmoCount = this.StartingAmmoCount;
			this.UpdateAmmoMesh();
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this._throwingTargets = base.Scene.FindEntitiesWithTag("throwing_target").ToList<GameEntity>();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x000D3518 File Offset: 0x000D1718
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.AmmoCount = this.StartingAmmoCount;
			this.UpdateAmmoMesh();
			foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
			{
				standingPoint.IsDeactivated = false;
			}
			foreach (StonePile.VolumeBoxTimerPair volumeBoxTimerPair in this._volumeBoxTimerPairs)
			{
				volumeBoxTimerPair.Timer.Reset(-3.5f);
			}
			foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
			{
				throwingPoint.AmmoPickUpPoint = null;
			}
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x000D360C File Offset: 0x000D180C
		public override void AfterMissionStart()
		{
			if (base.AmmoPickUpPoints != null)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.LockUserFrames = true;
				}
			}
			if (this._throwingPoints != null)
			{
				foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
				{
					throwingPoint.StandingPoint.IsDisabledForPlayers = true;
					throwingPoint.StandingPoint.LockUserFrames = false;
					throwingPoint.StandingPoint.LockUserPositions = true;
				}
			}
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x000D36CC File Offset: 0x000D18CC
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=jfcceEoE}{PILE_TYPE} Pile", null);
				textObject.SetTextVariable("PILE_TYPE", new TextObject("{=1CPdu9K0}Stone", null));
				return textObject;
			}
			return null;
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000D3714 File Offset: 0x000D1914
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (gameEntity.IsValid && gameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
				textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				return textObject;
			}
			return null;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000D3768 File Offset: 0x000D1968
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new StonePileAI(this);
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x000D3770 File Offset: 0x000D1970
		public override bool IsInRangeToCheckAlternativePoints(Agent agent)
		{
			float num = ((base.StandingPoints.Count > 0) ? (agent.GetInteractionDistanceToUsable(base.StandingPoints[0]) + 2f) : 2f);
			return base.GameEntity.GlobalPosition.DistanceSquared(agent.Position) < num * num;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x000D37CC File Offset: 0x000D19CC
		public override StandingPoint GetBestPointAlternativeTo(StandingPoint standingPoint, Agent agent)
		{
			if (base.AmmoPickUpPoints.Contains(standingPoint))
			{
				float num = standingPoint.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
				StandingPoint standingPoint2 = standingPoint;
				foreach (StandingPoint standingPoint3 in base.AmmoPickUpPoints)
				{
					float num2 = standingPoint3.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
					if (num2 < num && ((!standingPoint3.HasUser && !standingPoint3.HasAIMovingTo) || standingPoint3.IsInstantUse) && !standingPoint3.IsDeactivated && !standingPoint3.IsDisabledForAgent(agent))
					{
						num = num2;
						standingPoint2 = standingPoint3;
					}
				}
				return standingPoint2;
			}
			return standingPoint;
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x000D38A8 File Offset: 0x000D1AA8
		private void TickOccasionally()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.AmmoCount <= 0 && !this.HasThrowingPointUsed)
				{
					this.ReleaseAllUserAgentsAndFormations(BattleSideEnum.None, true);
					return;
				}
				if (this.IsDisabledForBattleSideAI(this.Side))
				{
					this.ReleaseAllUserAgentsAndFormations(this.Side, false);
					return;
				}
				bool flag = this._volumeBoxTimerPairs.Count == 0;
				foreach (StonePile.VolumeBoxTimerPair volumeBoxTimerPair in this._volumeBoxTimerPairs)
				{
					if (volumeBoxTimerPair.VolumeBox.HasAgentsInAttackerSide())
					{
						flag = true;
						if (volumeBoxTimerPair.Timer.ElapsedTime() > 3.5f)
						{
							volumeBoxTimerPair.Timer.Reset(Mission.Current.CurrentTime);
						}
						else
						{
							volumeBoxTimerPair.Timer.Reset(Mission.Current.CurrentTime - 0.5f);
						}
					}
				}
				MBReadOnlyList<Formation> userFormations = base.UserFormations;
				if (flag && userFormations.CountQ<Formation>((Formation f) => f.Team.Side == this.Side) == 0)
				{
					float minDistanceSquared = float.MaxValue;
					Formation bestFormation = null;
					foreach (Team team in Mission.Current.Teams)
					{
						if (team.Side == this.Side)
						{
							using (List<Formation>.Enumerator enumerator3 = team.FormationsIncludingEmpty.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									Formation formation = enumerator3.Current;
									if (formation.CountOfUnits > 0 && formation.CountOfUnitsWithoutLooseDetachedOnes >= this.MaxUserCount && formation.CountOfUnitsWithoutLooseDetachedOnes > 0)
									{
										formation.ApplyActionOnEachUnit(delegate(Agent agent)
										{
											float num = agent.Position.DistanceSquared(this.GameEntity.GlobalPosition);
											if (minDistanceSquared > num)
											{
												minDistanceSquared = num;
												bestFormation = formation;
											}
										}, null);
									}
								}
							}
						}
					}
					Formation bestFormation2 = bestFormation;
					if (bestFormation2 == null)
					{
						return;
					}
					bestFormation2.StartUsingMachine(this, false);
					return;
				}
				else if (!flag)
				{
					if (userFormations.Count > 0)
					{
						this.ReleaseAllUserAgentsAndFormations(BattleSideEnum.None, true);
						return;
					}
				}
				else
				{
					if (userFormations.All<Formation>((Formation f) => f.Team.Side == this.Side && f.UnitsWithoutLooseDetachedOnes.Count == 0))
					{
						if (base.StandingPoints.Count<StandingPoint>((StandingPoint sp) => sp.HasUser || sp.HasAIMovingTo) == 0)
						{
							this.ReleaseAllUserAgentsAndFormations(BattleSideEnum.None, true);
							return;
						}
					}
					this.UpdateThrowingPointAttackEntities();
				}
			}
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x000D3B50 File Offset: 0x000D1D50
		private void ReleaseAllUserAgentsAndFormations(BattleSideEnum sideFilterForAIControlledAgents, bool disableForNonAIControlledAgents)
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				Agent agent = (standingPoint.HasUser ? standingPoint.UserAgent : (standingPoint.HasAIMovingTo ? standingPoint.MovingAgent : null));
				if (agent != null)
				{
					if (agent.IsAIControlled)
					{
						if (sideFilterForAIControlledAgents == BattleSideEnum.None)
						{
							goto IL_006E;
						}
						Team team = agent.Team;
						if (team != null && team.Side == sideFilterForAIControlledAgents)
						{
							goto IL_006E;
						}
					}
					if (agent.IsAIControlled || !disableForNonAIControlledAgents)
					{
						continue;
					}
					IL_006E:
					if (agent.GetPrimaryWieldedItemIndex() == EquipmentIndex.ExtraWeaponSlot && agent.Equipment[EquipmentIndex.ExtraWeaponSlot].Item == this._givenItem)
					{
						agent.DropItem(EquipmentIndex.ExtraWeaponSlot, WeaponClass.Undefined);
					}
					base.Ai.StopUsingStandingPoint(standingPoint);
				}
			}
			MBReadOnlyList<Formation> userFormations = base.UserFormations;
			for (int i = userFormations.Count - 1; i >= 0; i--)
			{
				Formation formation = userFormations[i];
				if (formation.Team.Side == this.Side)
				{
					formation.StopUsingMachine(this, false);
				}
			}
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x000D3C78 File Offset: 0x000D1E78
		private void UpdateThrowingPointAttackEntities()
		{
			bool flag = false;
			List<WeakGameEntity> list = null;
			foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
			{
				if (throwingPoint.StandingPoint.HasAIUser)
				{
					if (!flag)
					{
						list = this.GetEnemySiegeWeapons();
						flag = true;
						if (list == null)
						{
							foreach (StonePile.ThrowingPoint throwingPoint2 in this._throwingPoints)
							{
								throwingPoint2.AttackEntity = null;
								throwingPoint2.AttackEntityNearbyAgentsCheckRadius = 0f;
							}
							if (this._throwingTargets.Count == 0)
							{
								break;
							}
						}
					}
					Agent userAgent = throwingPoint.StandingPoint.UserAgent;
					GameEntity attackEntity = throwingPoint.AttackEntity;
					if (attackEntity != null)
					{
						bool flag2 = false;
						if (!this.CanShootAtEntity(userAgent, attackEntity.WeakEntity, false))
						{
							flag2 = true;
						}
						else if (this._throwingTargets.Contains(attackEntity))
						{
							flag2 = !throwingPoint.CanUseAttackEntity();
						}
						else if (!list.Contains(attackEntity.WeakEntity))
						{
							flag2 = true;
						}
						if (flag2)
						{
							throwingPoint.AttackEntity = null;
							throwingPoint.AttackEntityNearbyAgentsCheckRadius = 0f;
						}
					}
					if (!(throwingPoint.AttackEntity == null))
					{
						continue;
					}
					bool flag3 = false;
					if (this._throwingTargets.Count > 0)
					{
						foreach (GameEntity gameEntity in this._throwingTargets)
						{
							if (attackEntity != gameEntity && this.CanShootAtEntity(userAgent, gameEntity.WeakEntity, true))
							{
								throwingPoint.AttackEntity = gameEntity;
								throwingPoint.AttackEntityNearbyAgentsCheckRadius = 1.31f;
								flag3 = true;
								break;
							}
						}
					}
					if (flag3 || list == null)
					{
						continue;
					}
					using (List<WeakGameEntity>.Enumerator enumerator4 = list.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							WeakGameEntity weakGameEntity = enumerator4.Current;
							if (attackEntity != weakGameEntity && this.CanShootAtEntity(userAgent, weakGameEntity, false))
							{
								throwingPoint.AttackEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
								throwingPoint.AttackEntityNearbyAgentsCheckRadius = 0f;
								break;
							}
						}
						continue;
					}
				}
				throwingPoint.AttackEntity = null;
			}
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x000D3F04 File Offset: 0x000D2104
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000D3F10 File Offset: 0x000D2110
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this._tickOccasionallyTimer.Check(Mission.Current.CurrentTime))
				{
					this.TickOccasionally();
				}
				StandingPoint.StackArray8StandingPoint stackArray8StandingPoint = default(StandingPoint.StackArray8StandingPoint);
				int num = 0;
				Agent.StackArray8Agent stackArray8Agent = default(Agent.StackArray8Agent);
				int num2 = 0;
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					if (standingPoint.HasUser)
					{
						ActionIndexCache currentAction = standingPoint.UserAgent.GetCurrentAction(1);
						if (!(currentAction == ActionIndexCache.act_pickup_boulder_begin))
						{
							if (currentAction == ActionIndexCache.act_pickup_boulder_end)
							{
								MissionWeapon missionWeapon = new MissionWeapon(this._givenItem, null, null, 1);
								Agent userAgent = standingPoint.UserAgent;
								userAgent.EquipWeaponToExtraSlotAndWield(ref missionWeapon);
								base.Ai.StopUsingStandingPoint(standingPoint);
								this.ConsumeAmmo();
								if (userAgent.IsAIControlled)
								{
									stackArray8Agent[num2++] = userAgent;
								}
							}
							else if (!standingPoint.UserAgent.SetActionChannel(1, in ActionIndexCache.act_pickup_boulder_begin, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
							{
								base.Ai.StopUsingStandingPoint(standingPoint);
							}
						}
					}
					if (standingPoint.HasAIUser || standingPoint.HasAIMovingTo)
					{
						stackArray8StandingPoint[num++] = standingPoint;
					}
				}
				StonePile.ThrowingPoint.StackArray8ThrowingPoint stackArray8ThrowingPoint = default(StonePile.ThrowingPoint.StackArray8ThrowingPoint);
				int num3 = 0;
				foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
				{
					throwingPoint.AmmoPickUpPoint = null;
					if (throwingPoint.AttackEntity != null || (throwingPoint.EnemyInRangeTimer.Check(Mission.Current.CurrentTime) && throwingPoint.EnemyInRangeTimer.ElapsedTime() < 3.5f))
					{
						if (!this.UpdateThrowingPointIfHasAnyInteractingAgent(throwingPoint))
						{
							stackArray8ThrowingPoint[num3++] = throwingPoint;
						}
					}
					else
					{
						throwingPoint.StandingPoint.IsDeactivated = true;
						if (throwingPoint.WaitingPoint != null)
						{
							throwingPoint.WaitingPoint.IsDeactivated = true;
						}
					}
				}
				for (int i = 0; i < num; i++)
				{
					if (num3 > i)
					{
						StandingPointWithWeaponRequirement standingPointWithWeaponRequirement = stackArray8StandingPoint[i] as StandingPointWithWeaponRequirement;
						stackArray8ThrowingPoint[i].AmmoPickUpPoint = standingPointWithWeaponRequirement;
					}
					else if (stackArray8StandingPoint[i].HasUser || stackArray8StandingPoint[i].HasAIMovingTo)
					{
						base.Ai.StopUsingStandingPoint(stackArray8StandingPoint[i]);
					}
				}
				for (int j = 0; j < num2; j++)
				{
					Agent agent = stackArray8Agent[j];
					StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(this.Side, agent, null, null);
					this.AssignAgentToStandingPoint(suitableStandingPointFor, agent);
				}
			}
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x000D4220 File Offset: 0x000D2420
		private bool ShouldStandAtWaitingPoint(StonePile.ThrowingPoint throwingPoint)
		{
			bool flag = false;
			if (throwingPoint.WaitingPoint != null)
			{
				flag = true;
				Vec2 asVec = throwingPoint.StandingPoint.GameEntity.GlobalPosition.AsVec2;
				if (AgentProximityMap.CanSearchRadius(this._givenItemRange))
				{
					AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, asVec, this._givenItemRange, false);
					while (proximityMapSearchStruct.LastFoundAgent != null)
					{
						if (proximityMapSearchStruct.LastFoundAgent.State == AgentState.Active && proximityMapSearchStruct.LastFoundAgent.Team != null && proximityMapSearchStruct.LastFoundAgent.Team.Side == BattleSideEnum.Attacker)
						{
							flag = false;
							break;
						}
						AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
					}
				}
				else
				{
					float num = this._givenItemRange * this._givenItemRange;
					if (Mission.Current.AttackerTeam != null)
					{
						MBReadOnlyList<Agent> activeAgents = Mission.Current.AttackerTeam.ActiveAgents;
						int count = activeAgents.Count;
						for (int i = 0; i < count; i++)
						{
							if (activeAgents[i].Position.AsVec2.DistanceSquared(asVec) <= num)
							{
								flag = false;
								break;
							}
						}
					}
					if (Mission.Current.AttackerAllyTeam != null)
					{
						MBReadOnlyList<Agent> activeAgents2 = Mission.Current.AttackerAllyTeam.ActiveAgents;
						int count2 = activeAgents2.Count;
						for (int j = 0; j < count2; j++)
						{
							if (activeAgents2[j].Position.AsVec2.DistanceSquared(asVec) <= num)
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x000D439C File Offset: 0x000D259C
		private bool UpdateThrowingPointIfHasAnyInteractingAgent(StonePile.ThrowingPoint throwingPoint)
		{
			Agent agent = null;
			StandingPoint standingPoint = null;
			throwingPoint.StandingPoint.IsDeactivated = false;
			if (throwingPoint.StandingPoint.HasAIMovingTo)
			{
				agent = throwingPoint.StandingPoint.MovingAgent;
				standingPoint = throwingPoint.StandingPoint;
			}
			else if (throwingPoint.StandingPoint.HasUser)
			{
				agent = throwingPoint.StandingPoint.UserAgent;
				standingPoint = throwingPoint.StandingPoint;
			}
			if (throwingPoint.WaitingPoint != null)
			{
				throwingPoint.WaitingPoint.IsDeactivated = false;
				if (throwingPoint.WaitingPoint.HasAIMovingTo)
				{
					agent = throwingPoint.WaitingPoint.MovingAgent;
					standingPoint = throwingPoint.WaitingPoint;
				}
				else if (throwingPoint.WaitingPoint.HasUser)
				{
					agent = throwingPoint.WaitingPoint.UserAgent;
					standingPoint = throwingPoint.WaitingPoint;
				}
			}
			bool flag = agent != null;
			if (flag && agent.Controller == AgentControllerType.AI)
			{
				EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
				if (primaryWieldedItemIndex == EquipmentIndex.None || agent.Equipment[primaryWieldedItemIndex].Item != this._givenItem)
				{
					base.Ai.StopUsingStandingPoint(standingPoint);
					throwingPoint.AttackEntity = null;
					return flag;
				}
				if (standingPoint == throwingPoint.WaitingPoint)
				{
					if (!this.ShouldStandAtWaitingPoint(throwingPoint))
					{
						base.Ai.StopUsingStandingPoint(standingPoint);
						this.AssignAgentToStandingPoint(throwingPoint.StandingPoint, agent);
						return flag;
					}
				}
				else if (agent.IsUsingGameObject && throwingPoint.AttackEntity != null)
				{
					if (throwingPoint.CanUseAttackEntity())
					{
						agent.SetScriptedTargetEntity(throwingPoint.AttackEntity.WeakEntity, Agent.AISpecialCombatModeFlags.None, true);
						return flag;
					}
					agent.DisableScriptedCombatMovement();
					throwingPoint.AttackEntity = null;
					return flag;
				}
				else if (this.ShouldStandAtWaitingPoint(throwingPoint))
				{
					base.Ai.StopUsingStandingPoint(standingPoint);
					this.AssignAgentToStandingPoint(throwingPoint.WaitingPoint, agent);
				}
			}
			return flag;
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x000D4532 File Offset: 0x000D2732
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x000D454C File Offset: 0x000D274C
		float? IDetachment.GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Item1.Team.Side;
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, null, candidates);
			if (suitableStandingPointFor == null)
			{
				match = null;
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			match = StonePileAI.GetSuitableAgentForStandingPoint(this, suitableStandingPointFor, candidates, new List<Agent>(), weightOfNextSlot.Value);
			if (match == null)
			{
				return null;
			}
			float num = 1f;
			float? num2 = weightOfNextSlot;
			float num3 = num;
			if (num2 == null)
			{
				return null;
			}
			return new float?(num2.GetValueOrDefault() * num3);
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000D45E4 File Offset: 0x000D27E4
		float? IDetachment.GetWeightOfAgentAtNextSlot(List<Agent> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Team.Side;
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, candidates, null);
			if (suitableStandingPointFor == null)
			{
				match = null;
				return null;
			}
			match = StonePileAI.GetSuitableAgentForStandingPoint(this, suitableStandingPointFor, candidates, new List<Agent>());
			if (match == null)
			{
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			float num = 1f;
			float? num2 = weightOfNextSlot;
			float num3 = num;
			if (num2 == null)
			{
				return null;
			}
			return new float?(num2.GetValueOrDefault() * num3);
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000D4670 File Offset: 0x000D2870
		protected override StandingPoint GetSuitableStandingPointFor(BattleSideEnum side, Agent agent = null, List<Agent> agents = null, List<ValueTuple<Agent, float>> agentValuePairs = null)
		{
			List<Agent> list = new List<Agent>();
			if (agents == null)
			{
				if (agent != null)
				{
					list.Add(agent);
					goto IL_005A;
				}
				if (agentValuePairs == null)
				{
					goto IL_005A;
				}
				using (List<ValueTuple<Agent, float>>.Enumerator enumerator = agentValuePairs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<Agent, float> valueTuple = enumerator.Current;
						list.Add(valueTuple.Item1);
					}
					goto IL_005A;
				}
			}
			list.AddRange(agents);
			IL_005A:
			bool flag = false;
			bool flag2 = false;
			StandingPoint standingPoint = null;
			int num = 0;
			while (num < this._throwingPoints.Count && (standingPoint == null || flag2))
			{
				StonePile.ThrowingPoint throwingPoint = this._throwingPoints[num];
				if (this.IsThrowingPointAssignable(throwingPoint))
				{
					StandingPoint standingPoint2 = throwingPoint.StandingPoint;
					bool flag3 = this.ShouldStandAtWaitingPoint(throwingPoint);
					if (flag3)
					{
						standingPoint2 = throwingPoint.WaitingPoint;
					}
					bool flag4 = false;
					int num2 = 0;
					while (!flag4 && num2 < list.Count)
					{
						flag4 = !standingPoint2.IsDisabledForAgent(list[num2]);
						num2++;
					}
					if (flag4)
					{
						flag2 = flag3;
						standingPoint = standingPoint2;
					}
					else
					{
						flag = true;
					}
				}
				num++;
			}
			int num3 = 0;
			while (num3 < base.StandingPoints.Count && standingPoint == null)
			{
				StandingPoint standingPoint3 = base.StandingPoints[num3];
				if (!standingPoint3.IsDeactivated && (standingPoint3.IsInstantUse || (!standingPoint3.HasUser && !standingPoint3.HasAIMovingTo)) && !standingPoint3.GameEntity.HasTag("throwing") && !standingPoint3.GameEntity.HasTag("wait_to_throw") && (flag || !standingPoint3.GameEntity.HasTag(this.AmmoPickUpTag)))
				{
					int num4 = 0;
					while (num4 < list.Count && standingPoint == null)
					{
						if (!standingPoint3.IsDisabledForAgent(list[num4]))
						{
							standingPoint = standingPoint3;
						}
						num4++;
					}
					if (list.Count == 0)
					{
						standingPoint = standingPoint3;
					}
				}
				num3++;
			}
			return standingPoint;
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000D486C File Offset: 0x000D2A6C
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			if (this.IsDisabledForBattleSideAI(side))
			{
				return float.MinValue;
			}
			this.UsableStandingPoints.Clear();
			int num = 0;
			foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
			{
				if (this.IsThrowingPointAssignable(throwingPoint))
				{
					num++;
				}
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = base.StandingPoints[i];
				if (standingPoint.GameEntity.HasTag(this.AmmoPickUpTag) && num > 0)
				{
					num--;
					if (standingPoint.IsUsableBySide(side))
					{
						if (!standingPoint.HasAIMovingTo)
						{
							if (!flag2)
							{
								this.UsableStandingPoints.Clear();
							}
							flag2 = true;
						}
						else if (flag2 || standingPoint.MovingAgent.Formation.Team.Side != side)
						{
							goto IL_00EC;
						}
						flag = true;
						this.UsableStandingPoints.Add(new ValueTuple<int, StandingPoint>(i, standingPoint));
					}
				}
				IL_00EC:;
			}
			this.AreUsableStandingPointsVacant = flag2;
			if (!flag)
			{
				return float.MinValue;
			}
			if (flag2)
			{
				return 1f;
			}
			if (!base.IsDetachmentRecentlyEvaluated)
			{
				return 0.1f;
			}
			return 0.01f;
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000D49BC File Offset: 0x000D2BBC
		protected virtual void UpdateAmmoMesh()
		{
			int num = 20 - this.AmmoCount;
			if (base.GameEntity.IsValid)
			{
				for (int i = 0; i < base.GameEntity.MultiMeshComponentCount; i++)
				{
					MetaMesh metaMesh = base.GameEntity.GetMetaMesh(i);
					for (int j = 0; j < metaMesh.MeshCount; j++)
					{
						metaMesh.GetMeshAtIndex(j).SetVectorArgument(0f, (float)num, 0f, 0f);
					}
				}
			}
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000D4A40 File Offset: 0x000D2C40
		private bool CanShootAtEntity(Agent agent, WeakGameEntity entity, bool canShootEvenIfRayCastHitsNothing = false)
		{
			bool flag = false;
			Vec3 eyeGlobalPosition = agent.GetEyeGlobalPosition();
			Vec3 globalPosition = entity.GlobalPosition;
			Vec3 vec = eyeGlobalPosition - globalPosition;
			float num = vec.Normalize();
			if (num > 1E-05f && MathF.Abs(vec.z) < MathF.Cos(0.2617994f) && num < this._givenItemRange)
			{
				float num2;
				WeakGameEntity parent;
				if (base.Scene.RayCastForClosestEntityOrTerrain(agent.GetEyeGlobalPosition(), entity.GlobalPosition, out num2, out parent, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags))
				{
					while (parent.IsValid)
					{
						if (parent == entity)
						{
							flag = true;
							break;
						}
						parent = parent.Parent;
					}
				}
				else
				{
					flag = canShootEvenIfRayCastHitsNothing;
				}
			}
			return flag;
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000D4AE4 File Offset: 0x000D2CE4
		private List<WeakGameEntity> GetEnemySiegeWeapons()
		{
			List<WeakGameEntity> list = null;
			if (Mission.Current.Teams.Attacker.TeamAI is TeamAISiegeComponent)
			{
				using (List<IPrimarySiegeWeapon>.Enumerator enumerator = ((TeamAISiegeComponent)Mission.Current.Teams.Attacker.TeamAI).PrimarySiegeWeapons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SiegeWeapon siegeWeapon;
						if ((siegeWeapon = enumerator.Current as SiegeWeapon) != null && siegeWeapon.GameEntity.GetFirstScriptOfType<DestructableComponent>() != null && siegeWeapon.IsUsed)
						{
							if (list == null)
							{
								list = new List<WeakGameEntity>();
							}
							list.Add(siegeWeapon.GameEntity);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x000D4B9C File Offset: 0x000D2D9C
		private bool IsThrowingPointAssignable(StonePile.ThrowingPoint throwingPoint)
		{
			return throwingPoint.AmmoPickUpPoint == null && !throwingPoint.StandingPoint.IsDeactivated && !throwingPoint.StandingPoint.HasUser && !throwingPoint.StandingPoint.HasAIMovingTo && (throwingPoint.WaitingPoint == null || (!throwingPoint.WaitingPoint.IsDeactivated && !throwingPoint.WaitingPoint.HasUser && !throwingPoint.WaitingPoint.HasAIMovingTo));
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000D4C10 File Offset: 0x000D2E10
		private bool AssignAgentToStandingPoint(StandingPoint standingPoint, Agent agent)
		{
			if (standingPoint == null || agent == null || !StonePileAI.IsAgentAssignable(agent))
			{
				return false;
			}
			int num = base.StandingPoints.IndexOf(standingPoint);
			if (num >= 0)
			{
				((IDetachment)this).AddAgent(agent, num, Agent.AIScriptedFrameFlags.None);
				if (agent.Formation != null)
				{
					agent.Formation.DetachUnit(agent, ((IDetachment)this).IsLoose);
					agent.Detachment = this;
					agent.SetDetachmentWeight(this.GetWeightOfStandingPoint(standingPoint));
					return true;
				}
			}
			return false;
		}

		// Token: 0x040015E9 RID: 5609
		private const string ThrowingTargetTag = "throwing_target";

		// Token: 0x040015EA RID: 5610
		private const string ThrowingPointTag = "throwing";

		// Token: 0x040015EB RID: 5611
		private const string WaitingPointTag = "wait_to_throw";

		// Token: 0x040015EC RID: 5612
		private const float EnemyInRangeTimerDuration = 0.5f;

		// Token: 0x040015ED RID: 5613
		private const float EnemyWaitTimeLimit = 3f;

		// Token: 0x040015EE RID: 5614
		private const float ThrowingTargetRadius = 1.31f;

		// Token: 0x040015EF RID: 5615
		public int StartingAmmoCount = 12;

		// Token: 0x040015F0 RID: 5616
		public string GivenItemID = "boulder";

		// Token: 0x040015F1 RID: 5617
		[EditableScriptComponentVariable(true, "")]
		private float _givenItemRange = 15f;

		// Token: 0x040015F2 RID: 5618
		private ItemObject _givenItem;

		// Token: 0x040015F3 RID: 5619
		private List<GameEntity> _throwingTargets;

		// Token: 0x040015F4 RID: 5620
		private List<StonePile.ThrowingPoint> _throwingPoints;

		// Token: 0x040015F5 RID: 5621
		private List<StonePile.VolumeBoxTimerPair> _volumeBoxTimerPairs;

		// Token: 0x040015F6 RID: 5622
		private Timer _tickOccasionallyTimer;

		// Token: 0x02000659 RID: 1625
		[DefineSynchedMissionObjectType(typeof(StonePile))]
		public struct StonePileRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AE7 RID: 2791
			// (get) Token: 0x0600411F RID: 16671 RVA: 0x000FCC3A File Offset: 0x000FAE3A
			// (set) Token: 0x06004120 RID: 16672 RVA: 0x000FCC42 File Offset: 0x000FAE42
			public int ReadAmmoCount { get; private set; }

			// Token: 0x06004121 RID: 16673 RVA: 0x000FCC4B File Offset: 0x000FAE4B
			public StonePileRecord(int readAmmoCount)
			{
				this.ReadAmmoCount = readAmmoCount;
			}

			// Token: 0x06004122 RID: 16674 RVA: 0x000FCC54 File Offset: 0x000FAE54
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.ReadAmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200065A RID: 1626
		private class ThrowingPoint
		{
			// Token: 0x06004123 RID: 16675 RVA: 0x000FCC6C File Offset: 0x000FAE6C
			public bool CanUseAttackEntity()
			{
				bool flag = true;
				if (this.AttackEntityNearbyAgentsCheckRadius > 0f)
				{
					float currentTime = Mission.Current.CurrentTime;
					if (currentTime >= this._cachedCanUseAttackEntityExpireTime)
					{
						this._cachedCanUseAttackEntity = Mission.Current.HasAnyAgentsOfSideInRange(this.AttackEntity.GlobalPosition, this.AttackEntityNearbyAgentsCheckRadius, BattleSideEnum.Attacker);
						this._cachedCanUseAttackEntityExpireTime = currentTime + 1f;
					}
					flag = this._cachedCanUseAttackEntity;
				}
				return flag;
			}

			// Token: 0x040021D5 RID: 8661
			private const float CachedCanUseAttackEntityUpdateInterval = 1f;

			// Token: 0x040021D6 RID: 8662
			public StandingPointWithVolumeBox StandingPoint;

			// Token: 0x040021D7 RID: 8663
			public StandingPointWithWeaponRequirement AmmoPickUpPoint;

			// Token: 0x040021D8 RID: 8664
			public StandingPointWithWeaponRequirement WaitingPoint;

			// Token: 0x040021D9 RID: 8665
			public Timer EnemyInRangeTimer;

			// Token: 0x040021DA RID: 8666
			public GameEntity AttackEntity;

			// Token: 0x040021DB RID: 8667
			public float AttackEntityNearbyAgentsCheckRadius;

			// Token: 0x040021DC RID: 8668
			private float _cachedCanUseAttackEntityExpireTime;

			// Token: 0x040021DD RID: 8669
			private bool _cachedCanUseAttackEntity;

			// Token: 0x020006D5 RID: 1749
			public struct StackArray8ThrowingPoint
			{
				// Token: 0x17000B37 RID: 2871
				public StonePile.ThrowingPoint this[int index]
				{
					get
					{
						switch (index)
						{
						case 0:
							return this._element0;
						case 1:
							return this._element1;
						case 2:
							return this._element2;
						case 3:
							return this._element3;
						case 4:
							return this._element4;
						case 5:
							return this._element5;
						case 6:
							return this._element6;
						case 7:
							return this._element7;
						default:
							return null;
						}
					}
					set
					{
						switch (index)
						{
						case 0:
							this._element0 = value;
							return;
						case 1:
							this._element1 = value;
							return;
						case 2:
							this._element2 = value;
							return;
						case 3:
							this._element3 = value;
							return;
						case 4:
							this._element4 = value;
							return;
						case 5:
							this._element5 = value;
							return;
						case 6:
							this._element6 = value;
							return;
						case 7:
							this._element7 = value;
							return;
						default:
							return;
						}
					}
				}

				// Token: 0x040023E2 RID: 9186
				private StonePile.ThrowingPoint _element0;

				// Token: 0x040023E3 RID: 9187
				private StonePile.ThrowingPoint _element1;

				// Token: 0x040023E4 RID: 9188
				private StonePile.ThrowingPoint _element2;

				// Token: 0x040023E5 RID: 9189
				private StonePile.ThrowingPoint _element3;

				// Token: 0x040023E6 RID: 9190
				private StonePile.ThrowingPoint _element4;

				// Token: 0x040023E7 RID: 9191
				private StonePile.ThrowingPoint _element5;

				// Token: 0x040023E8 RID: 9192
				private StonePile.ThrowingPoint _element6;

				// Token: 0x040023E9 RID: 9193
				private StonePile.ThrowingPoint _element7;

				// Token: 0x040023EA RID: 9194
				public const int Length = 8;
			}
		}

		// Token: 0x0200065B RID: 1627
		private struct VolumeBoxTimerPair
		{
			// Token: 0x040021DE RID: 8670
			public VolumeBox VolumeBox;

			// Token: 0x040021DF RID: 8671
			public Timer Timer;
		}
	}
}
