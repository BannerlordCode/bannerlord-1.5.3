using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000361 RID: 865
	public class StandingPoint : UsableMissionObject
	{
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x000CB81C File Offset: 0x000C9A1C
		public virtual Agent.AIScriptedFrameFlags DisableScriptedFrameFlags
		{
			get
			{
				return Agent.AIScriptedFrameFlags.None;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x000CB81F File Offset: 0x000C9A1F
		public override bool DisableCombatActionsOnUse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060031DC RID: 12764 RVA: 0x000CB822 File Offset: 0x000C9A22
		// (set) Token: 0x060031DD RID: 12765 RVA: 0x000CB82A File Offset: 0x000C9A2A
		[EditableScriptComponentVariable(false, "")]
		public Agent FavoredUser { get; set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060031DE RID: 12766 RVA: 0x000CB833 File Offset: 0x000C9A33
		public virtual bool PlayerStopsUsingWhenInteractsWithOther
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x000CB836 File Offset: 0x000C9A36
		public bool UseOwnPositionInsteadOfWorldPosition
		{
			get
			{
				return this._useOwnPositionInsteadOfWorldPosition;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x000CB83E File Offset: 0x000C9A3E
		public float CustomPlayerInteractionDistance
		{
			get
			{
				return this._customPlayerInteractionDistance;
			}
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000CB848 File Offset: 0x000C9A48
		protected internal override void OnInit()
		{
			base.OnInit();
			this._cachedAgentDistances = new Dictionary<Agent, StandingPoint.AgentDistanceCache>();
			bool flag = base.GameEntity.HasTag("attacker");
			bool flag2 = base.GameEntity.HasTag("defender");
			if (flag && !flag2)
			{
				this.StandingPointSide = BattleSideEnum.Attacker;
			}
			else if (!flag && flag2)
			{
				this.StandingPointSide = BattleSideEnum.Defender;
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000CB8B8 File Offset: 0x000C9AB8
		public void OnParentMachinePhysicsStateChanged()
		{
			base.GameEntityWithWorldPosition.InvalidateWorldPosition();
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000CB8C5 File Offset: 0x000C9AC5
		public override bool IsDisabledForAgent(Agent agent)
		{
			return base.IsDisabledForAgent(agent) || (this.StandingPointSide != BattleSideEnum.None && agent.IsAIControlled && agent.Team != null && agent.Team.Side != this.StandingPointSide);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000CB903 File Offset: 0x000C9B03
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (!GameNetwork.IsClientOrReplay && base.HasUser)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel3;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000CB928 File Offset: 0x000C9B28
		private void TickAux(bool isParallel)
		{
			if (!GameNetwork.IsClientOrReplay && base.HasUser)
			{
				if (!base.UserAgent.IsActive() || this.DoesActionTypeStopUsingGameObject(MBAnimation.GetActionType(base.UserAgent.GetCurrentAction(0))))
				{
					if (isParallel)
					{
						this._needsSingleThreadTickOnce = true;
						return;
					}
					Agent userAgent = base.UserAgent;
					Agent.StopUsingGameObjectFlags stopUsingGameObjectFlags = Agent.StopUsingGameObjectFlags.None;
					if (this._autoAttachOnUsingStopped)
					{
						stopUsingGameObjectFlags |= Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject;
					}
					userAgent.StopUsingGameObject(false, stopUsingGameObjectFlags);
					Action<Agent, bool> onUsingStoppedAction = this._onUsingStoppedAction;
					if (onUsingStoppedAction == null)
					{
						return;
					}
					onUsingStoppedAction(userAgent, true);
					return;
				}
				else if (this.AutoSheathWeapons)
				{
					if (base.UserAgent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
					{
						if (isParallel)
						{
							this._needsSingleThreadTickOnce = true;
						}
						else
						{
							base.UserAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
						}
					}
					if (base.UserAgent.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
					{
						if (isParallel)
						{
							this._needsSingleThreadTickOnce = true;
							return;
						}
						base.UserAgent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
						return;
					}
				}
				else if (this.AutoWieldWeapons && base.UserAgent.Equipment.HasAnyWeapon() && base.UserAgent.GetPrimaryWieldedItemIndex() == EquipmentIndex.None && base.UserAgent.GetOffhandWieldedItemIndex() == EquipmentIndex.None)
				{
					if (isParallel)
					{
						this._needsSingleThreadTickOnce = true;
						return;
					}
					base.UserAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.Instant, Equipment.InitialWeaponEquipPreference.Any);
				}
			}
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000CBA4F File Offset: 0x000C9C4F
		protected internal override void OnTickParallel3(float dt)
		{
			this.TickAux(true);
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x000CBA58 File Offset: 0x000C9C58
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				this.TickAux(false);
			}
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000CBA77 File Offset: 0x000C9C77
		protected virtual bool DoesActionTypeStopUsingGameObject(Agent.ActionCodeType actionType)
		{
			return actionType == Agent.ActionCodeType.Jump || actionType == Agent.ActionCodeType.Kick || actionType == Agent.ActionCodeType.WeaponBash;
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x000CBA8C File Offset: 0x000C9C8C
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			if (!this._autoAttachOnUsingStopped && this.MovingAgent != null)
			{
				Agent movingAgent = this.MovingAgent;
				movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
				Action<Agent, bool> onUsingStoppedAction = this._onUsingStoppedAction;
				if (onUsingStoppedAction != null)
				{
					onUsingStoppedAction(movingAgent, false);
				}
			}
			base.OnUse(userAgent, agentBoneIndex);
			if (this.LockUserFrames)
			{
				WorldFrame userFrameForAgent = this.GetUserFrameForAgent(userAgent);
				Vec2 asVec = userFrameForAgent.Origin.AsVec2;
				userAgent.SetTargetPositionAndDirection(in asVec, in userFrameForAgent.Rotation.f);
				return;
			}
			if (this.LockUserPositions)
			{
				userAgent.SetTargetPosition(this.GetUserFrameForAgent(userAgent).Origin.AsVec2);
			}
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000CBB27 File Offset: 0x000C9D27
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (this.LockUserFrames || this.LockUserPositions)
			{
				userAgent.ClearTargetFrame();
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000CBB48 File Offset: 0x000C9D48
		public override WorldFrame GetUserFrameForAgent(Agent agent)
		{
			if (!Mission.Current.IsTeleportingAgents && !this.TranslateUser)
			{
				return agent.GetWorldFrame();
			}
			if (!Mission.Current.IsTeleportingAgents && (this.LockUserFrames || this.LockUserPositions))
			{
				return base.GetUserFrameForAgent(agent);
			}
			WorldFrame userFrameForAgent = base.GetUserFrameForAgent(agent);
			MatrixFrame lookFrame = agent.LookFrame;
			Vec2 vec = (lookFrame.origin.AsVec2 - userFrameForAgent.Origin.AsVec2).Normalized();
			Vec2 vec2 = userFrameForAgent.Origin.AsVec2 + agent.GetInteractionDistanceToUsable(this) * 0.5f * vec;
			Mat3 rotation = lookFrame.rotation;
			userFrameForAgent.Origin.SetVec2(vec2);
			userFrameForAgent.Rotation = rotation;
			return userFrameForAgent;
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000CBC0E File Offset: 0x000C9E0E
		public virtual bool HasAlternative()
		{
			return false;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000CBC14 File Offset: 0x000C9E14
		public virtual float GetUsageScoreForAgent(Agent agent)
		{
			WorldPosition origin = this.GetUserFrameForAgent(agent).Origin;
			WorldPosition worldPosition = agent.GetWorldPosition();
			float pathDistance = this.GetPathDistance(agent, ref origin, ref worldPosition);
			float num = ((pathDistance < 0f) ? float.MinValue : (-pathDistance));
			if (agent == this.FavoredUser)
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x000CBC68 File Offset: 0x000C9E68
		public virtual float GetUsageScoreForAgent(ValueTuple<Agent, float> agentPair)
		{
			float item = agentPair.Item2;
			float num = ((item < 0f) ? float.MinValue : (-item));
			if (agentPair.Item1 == this.FavoredUser)
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x000CBCA5 File Offset: 0x000C9EA5
		public void SetupOnUsingStoppedBehavior(bool autoAttach, Action<Agent, bool> action)
		{
			this._autoAttachOnUsingStopped = autoAttach;
			this._onUsingStoppedAction = action;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x000CBCB8 File Offset: 0x000C9EB8
		private float GetPathDistance(Agent agent, ref WorldPosition userPosition, ref WorldPosition agentPosition)
		{
			StandingPoint.AgentDistanceCache agentDistanceCache;
			float num;
			if (this._cachedAgentDistances.TryGetValue(agent, out agentDistanceCache))
			{
				if (agentDistanceCache.AgentPosition.DistanceSquared(agentPosition.AsVec2) < 1f && agentDistanceCache.StandingPointPosition.DistanceSquared(userPosition.AsVec2) < 1f)
				{
					num = agentDistanceCache.PathDistance;
				}
				else
				{
					if (!Mission.Current.Scene.GetPathDistanceBetweenPositions(ref userPosition, ref agentPosition, agent.Monster.BodyCapsuleRadius, out num))
					{
						num = float.MaxValue;
					}
					agentDistanceCache = new StandingPoint.AgentDistanceCache
					{
						AgentPosition = agentPosition.AsVec2,
						StandingPointPosition = userPosition.AsVec2,
						PathDistance = num
					};
					this._cachedAgentDistances[agent] = agentDistanceCache;
				}
			}
			else
			{
				if (!Mission.Current.Scene.GetPathDistanceBetweenPositions(ref userPosition, ref agentPosition, agent.Monster.BodyCapsuleRadius, out num))
				{
					num = float.MaxValue;
				}
				agentDistanceCache = new StandingPoint.AgentDistanceCache
				{
					AgentPosition = agentPosition.AsVec2,
					StandingPointPosition = userPosition.AsVec2,
					PathDistance = num
				};
				this._cachedAgentDistances[agent] = agentDistanceCache;
			}
			return num;
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x000CBDD7 File Offset: 0x000C9FD7
		public override void OnEndMission()
		{
			base.OnEndMission();
			this.FavoredUser = null;
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000CBDE6 File Offset: 0x000C9FE6
		protected internal virtual bool IsUsableBySide(BattleSideEnum side)
		{
			return !base.IsDeactivated && (base.IsInstantUse || !base.HasUser) && (this.StandingPointSide == BattleSideEnum.None || side == this.StandingPointSide);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000CBE16 File Offset: 0x000CA016
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000CBE1C File Offset: 0x000CA01C
		public override bool IsUsableByAgent(Agent userAgent)
		{
			switch (this._validControllerType)
			{
			case StandingPoint.ValidControllerType.None:
				return false;
			case StandingPoint.ValidControllerType.PlayerOnly:
				return userAgent.IsPlayerControlled;
			case StandingPoint.ValidControllerType.AIOnly:
				return userAgent.IsAIControlled;
			case StandingPoint.ValidControllerType.PlayerOrAI:
				return true;
			default:
				return true;
			}
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000CBE5B File Offset: 0x000CA05B
		public void SetUsableByAIOnly()
		{
			this._validControllerType = StandingPoint.ValidControllerType.AIOnly;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000CBE64 File Offset: 0x000CA064
		public void SetUsableByPlayerOnly()
		{
			this._validControllerType = StandingPoint.ValidControllerType.PlayerOnly;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000CBE6D File Offset: 0x000CA06D
		public void SetUsableByPlayerOrAI()
		{
			this._validControllerType = StandingPoint.ValidControllerType.PlayerOrAI;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000CBE76 File Offset: 0x000CA076
		public StandingPoint()
			: base(false)
		{
		}

		// Token: 0x04001506 RID: 5382
		public bool AutoSheathWeapons = true;

		// Token: 0x04001507 RID: 5383
		public bool AutoEquipWeaponsOnUseStopped;

		// Token: 0x04001508 RID: 5384
		private bool _autoAttachOnUsingStopped = true;

		// Token: 0x04001509 RID: 5385
		private Action<Agent, bool> _onUsingStoppedAction;

		// Token: 0x0400150A RID: 5386
		public bool AutoWieldWeapons;

		// Token: 0x0400150B RID: 5387
		public readonly bool TranslateUser = true;

		// Token: 0x0400150C RID: 5388
		public bool HasRecentlyBeenRechecked;

		// Token: 0x0400150E RID: 5390
		private Dictionary<Agent, StandingPoint.AgentDistanceCache> _cachedAgentDistances;

		// Token: 0x0400150F RID: 5391
		[EditableScriptComponentVariable(true, "")]
		private bool _useOwnPositionInsteadOfWorldPosition;

		// Token: 0x04001510 RID: 5392
		[EditableScriptComponentVariable(true, "")]
		private float _customPlayerInteractionDistance;

		// Token: 0x04001511 RID: 5393
		private bool _needsSingleThreadTickOnce;

		// Token: 0x04001512 RID: 5394
		private StandingPoint.ValidControllerType _validControllerType = StandingPoint.ValidControllerType.PlayerOrAI;

		// Token: 0x04001513 RID: 5395
		protected BattleSideEnum StandingPointSide = BattleSideEnum.None;

		// Token: 0x02000648 RID: 1608
		public struct StackArray8StandingPoint
		{
			// Token: 0x17000AE6 RID: 2790
			public StandingPoint this[int index]
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

			// Token: 0x0400217D RID: 8573
			private StandingPoint _element0;

			// Token: 0x0400217E RID: 8574
			private StandingPoint _element1;

			// Token: 0x0400217F RID: 8575
			private StandingPoint _element2;

			// Token: 0x04002180 RID: 8576
			private StandingPoint _element3;

			// Token: 0x04002181 RID: 8577
			private StandingPoint _element4;

			// Token: 0x04002182 RID: 8578
			private StandingPoint _element5;

			// Token: 0x04002183 RID: 8579
			private StandingPoint _element6;

			// Token: 0x04002184 RID: 8580
			private StandingPoint _element7;

			// Token: 0x04002185 RID: 8581
			public const int Length = 8;
		}

		// Token: 0x02000649 RID: 1609
		private struct AgentDistanceCache
		{
			// Token: 0x04002186 RID: 8582
			public Vec2 AgentPosition;

			// Token: 0x04002187 RID: 8583
			public Vec2 StandingPointPosition;

			// Token: 0x04002188 RID: 8584
			public float PathDistance;
		}

		// Token: 0x0200064A RID: 1610
		private enum ValidControllerType
		{
			// Token: 0x0400218A RID: 8586
			None,
			// Token: 0x0400218B RID: 8587
			PlayerOnly,
			// Token: 0x0400218C RID: 8588
			AIOnly,
			// Token: 0x0400218D RID: 8589
			PlayerOrAI
		}
	}
}
