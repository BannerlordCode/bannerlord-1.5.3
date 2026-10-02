using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.AnimationPoints
{
	// Token: 0x02000054 RID: 84
	public class DynamicObjectAnimationPoint : StandingPoint
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00012FE1 File Offset: 0x000111E1
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00012FE9 File Offset: 0x000111E9
		public bool IsArriveActionFinished { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00012FF2 File Offset: 0x000111F2
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00012FFC File Offset: 0x000111FC
		protected string SelectedRightHandItem
		{
			get
			{
				return this._selectedRightHandItem;
			}
			set
			{
				if (value != this._selectedRightHandItem)
				{
					AnimationPoint.ItemForBone itemForBone = new AnimationPoint.ItemForBone(this.RightHandItemBone, value, false);
					this.AssignItemToBone(itemForBone);
					this._selectedRightHandItem = value;
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00013033 File Offset: 0x00011233
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0001303C File Offset: 0x0001123C
		protected string SelectedLeftHandItem
		{
			get
			{
				return this._selectedLeftHandItem;
			}
			set
			{
				if (value != this._selectedLeftHandItem)
				{
					AnimationPoint.ItemForBone itemForBone = new AnimationPoint.ItemForBone(this.LeftHandItemBone, value, false);
					this.AssignItemToBone(itemForBone);
					this._selectedLeftHandItem = value;
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00013073 File Offset: 0x00011273
		public override bool PlayerStopsUsingWhenInteractsWithOther
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00013076 File Offset: 0x00011276
		public override bool DisableCombatActionsOnUse
		{
			get
			{
				return !base.IsInstantUse;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00013081 File Offset: 0x00011281
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00013089 File Offset: 0x00011289
		public bool IsActive { get; private set; } = true;

		// Token: 0x06000347 RID: 839 RVA: 0x00013092 File Offset: 0x00011292
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.Tick(dt, false);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000130A3 File Offset: 0x000112A3
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.HasUser)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000130BC File Offset: 0x000112BC
		protected override bool DoesActionTypeStopUsingGameObject(Agent.ActionCodeType actionType)
		{
			return false;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000130BF File Offset: 0x000112BF
		public override bool IsUsableByAgent(Agent userAgent)
		{
			return this.IsActive && base.IsUsableByAgent(userAgent);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000130D2 File Offset: 0x000112D2
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.OnUse(userAgent, agentBoneIndex);
			this._equipmentIndexMainHand = base.UserAgent.GetPrimaryWieldedItemIndex();
			this._equipmentIndexOffHand = base.UserAgent.GetOffhandWieldedItemIndex();
			this._state = DynamicObjectAnimationPoint.State.NotUsing;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00013108 File Offset: 0x00011308
		public override WorldFrame GetUserFrameForAgent(Agent agent)
		{
			WorldFrame userFrameForAgent = base.GetUserFrameForAgent(agent);
			float agentScale = agent.AgentScale;
			userFrameForAgent.Origin.SetVec2(userFrameForAgent.Origin.AsVec2 + (userFrameForAgent.Rotation.f.AsVec2 * -this.ForwardDistanceToPivotPoint + userFrameForAgent.Rotation.s.AsVec2 * this.SideDistanceToPivotPoint) * (1f - agentScale));
			return userFrameForAgent;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001318C File Offset: 0x0001138C
		public override bool IsDisabledForAgent(Agent agent)
		{
			if (base.HasUser && base.UserAgent == agent)
			{
				return !this.IsActive || base.IsDeactivated;
			}
			if (!this.IsActive || agent.MountAgent != null || base.IsDeactivated || !agent.IsAbleToUseMachine() || (!agent.IsAIControlled && (base.IsDisabledForPlayers || base.HasUser)))
			{
				return true;
			}
			WeakGameEntity parent = base.GameEntity.Parent;
			if (!parent.IsValid || !parent.HasScriptOfType<UsableMachine>() || !base.GameEntity.HasTag("alternative"))
			{
				return base.IsDisabledForAgent(agent);
			}
			if (agent.IsAIControlled && parent.HasTag("reserved"))
			{
				return true;
			}
			CampaignAgentComponent component = agent.GetComponent<CampaignAgentComponent>();
			string text = ((((component != null) ? component.AgentNavigator : null) != null) ? agent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag : string.Empty);
			if (!string.IsNullOrEmpty(text) && !parent.HasTag(text))
			{
				return true;
			}
			using (List<StandingPoint>.Enumerator enumerator = parent.GetFirstScriptOfType<UsableMachine>().StandingPoints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationPoint animationPoint;
					if ((animationPoint = enumerator.Current as AnimationPoint) != null && this.GroupId == animationPoint.GroupId && !animationPoint.IsDeactivated && (animationPoint.HasUser || (animationPoint.HasAIMovingTo && !animationPoint.IsAIMovingTo(agent))) && animationPoint.GameEntity.HasTag("alternative"))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001332C File Offset: 0x0001152C
		public override void SimulateTick(float dt)
		{
			this.Tick(dt, true);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00013336 File Offset: 0x00011536
		public override bool HasAlternative()
		{
			return this.GroupId >= 0;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00013344 File Offset: 0x00011544
		protected override void OnInit()
		{
			base.OnInit();
			this._itemsForBones = new List<AnimationPoint.ItemForBone>();
			this.SetActionCodes();
			this.InitParameters();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001336F File Offset: 0x0001156F
		protected override void OnEditorInit()
		{
			this._itemsForBones = new List<AnimationPoint.ItemForBone>();
			this.SetActionCodes();
			this.InitParameters();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00013388 File Offset: 0x00011588
		public override void OnUserConversationStart()
		{
			this._pointRotation = base.UserAgent.Frame.rotation.f;
			this._pointRotation.Normalize();
			if (!this.KeepOldVisibility)
			{
				foreach (AnimationPoint.ItemForBone itemForBone in this._itemsForBones)
				{
					itemForBone.OldVisibility = itemForBone.IsVisible;
				}
				this.SetAgentItemsVisibility(false);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00013414 File Offset: 0x00011614
		public override void OnUserConversationEnd()
		{
			base.UserAgent.ResetLookAgent();
			base.UserAgent.LookDirection = this._pointRotation;
			base.UserAgent.SetActionChannel(0, in this.LoopStartActionCode, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			foreach (AnimationPoint.ItemForBone itemForBone in this._itemsForBones)
			{
				if (itemForBone.OldVisibility)
				{
					this.SetAgentItemVisibility(itemForBone, true);
				}
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000134C4 File Offset: 0x000116C4
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			this.SetAgentItemsVisibility(false);
			this.RevertWeaponWieldSheathState();
			if (base.UserAgent.IsActive())
			{
				if (this.LeaveActionCode == ActionIndexCache.act_none)
				{
					base.UserAgent.SetActionChannel(0, in this.LeaveActionCode, false, (AnimFlags)((long)Math.Min(base.UserAgent.GetCurrentActionPriority(0), 73)), 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
				else if (this.IsArriveActionFinished)
				{
					ActionIndexCache currentAction = base.UserAgent.GetCurrentAction(0);
					if (currentAction != this.LeaveActionCode && !base.UserAgent.ActionSet.AreActionsAlternatives(in currentAction, in this.LeaveActionCode))
					{
						AnimFlags animFlags = (AnimFlags)((long)Math.Min(base.UserAgent.GetCurrentActionPriority(0), base.UserAgent.IsSitting() ? 94 : 73));
						base.UserAgent.SetActionChannel(0, in this.LeaveActionCode, false, animFlags, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					}
				}
				else
				{
					ActionIndexCache currentAction2 = userAgent.GetCurrentAction(0);
					if (currentAction2 == this.ArriveActionCode && this.ArriveActionCode != ActionIndexCache.act_none)
					{
						MBActionSet actionSet = userAgent.ActionSet;
						float currentActionProgress = userAgent.GetCurrentActionProgress(0);
						float actionBlendOutStartProgress = MBActionSet.GetActionBlendOutStartProgress(actionSet, in currentAction2);
						if (currentActionProgress < actionBlendOutStartProgress)
						{
							float num = (actionBlendOutStartProgress - currentActionProgress) / actionBlendOutStartProgress;
							MBActionSet.GetActionBlendOutStartProgress(actionSet, in this.LeaveActionCode);
						}
					}
				}
			}
			this._lastAction = ActionIndexCache.act_none;
			if (base.UserAgent.GetLookAgent() != null)
			{
				base.UserAgent.ResetLookAgent();
			}
			this.IsArriveActionFinished = false;
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001368C File Offset: 0x0001188C
		private void RevertWeaponWieldSheathState()
		{
			if (this._equipmentIndexMainHand != EquipmentIndex.None && this.AutoSheathWeapons)
			{
				base.UserAgent.TryToWieldWeaponInSlot(this._equipmentIndexMainHand, Agent.WeaponWieldActionType.WithAnimation, false);
			}
			else if (this._equipmentIndexMainHand == EquipmentIndex.None && this.AutoWieldWeapons)
			{
				base.UserAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
			}
			if (this._equipmentIndexOffHand != EquipmentIndex.None && this.AutoSheathWeapons)
			{
				base.UserAgent.TryToWieldWeaponInSlot(this._equipmentIndexOffHand, Agent.WeaponWieldActionType.WithAnimation, false);
				return;
			}
			if (this._equipmentIndexOffHand == EquipmentIndex.None && this.AutoWieldWeapons)
			{
				base.UserAgent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimation);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00013720 File Offset: 0x00011920
		public void SetAgentItemsVisibility(bool isVisible)
		{
			if (!base.UserAgent.IsMainAgent)
			{
				foreach (AnimationPoint.ItemForBone itemForBone in this._itemsForBones)
				{
					this.SetAgentItemVisibility(itemForBone, isVisible);
				}
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00013784 File Offset: 0x00011984
		private void SetAgentItemVisibility(AnimationPoint.ItemForBone item, bool isVisible)
		{
			sbyte realBoneIndex = base.UserAgent.AgentVisuals.GetRealBoneIndex(item.HumanBone);
			base.UserAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SetPrefabVisibility(realBoneIndex, item.ItemPrefabName, isVisible);
			item.IsVisible = isVisible;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000137CC File Offset: 0x000119CC
		private void Tick(float dt, bool isSimulation = false)
		{
			if (base.HasUser)
			{
				if (Game.Current != null && Game.Current.IsDevelopmentMode)
				{
					base.UserAgent.GetTargetPosition().IsNonZero();
				}
				ActionIndexCache currentAction = base.UserAgent.GetCurrentAction(0);
				switch (this._state)
				{
				case DynamicObjectAnimationPoint.State.NotUsing:
					if (this.IsTargetReached() && base.UserAgent.MovementVelocity.LengthSquared < 0.1f && base.UserAgent.IsAbleToUseMachine())
					{
						if (this.ArriveActionCode != ActionIndexCache.act_none)
						{
							Agent userAgent = base.UserAgent;
							int num = 0;
							bool flag = false;
							AnimFlags animFlags = (AnimFlags)0UL;
							float num2 = 0f;
							float num3 = (isSimulation ? 0f : (-0.2f));
							userAgent.SetActionChannel(num, in this.ArriveActionCode, flag, animFlags, num2, MBRandom.RandomFloatRanged(0.8f, 1f), num3, 0.4f, 0f, false, -0.2f, 0, true);
						}
						this._state = DynamicObjectAnimationPoint.State.StartToUse;
						return;
					}
					break;
				case DynamicObjectAnimationPoint.State.StartToUse:
					if (this.ArriveActionCode != ActionIndexCache.act_none && isSimulation)
					{
						this.SimulateAnimations(0.1f);
					}
					if (this.ArriveActionCode == ActionIndexCache.act_none || currentAction == this.ArriveActionCode || base.UserAgent.ActionSet.AreActionsAlternatives(in currentAction, in this.ArriveActionCode))
					{
						base.UserAgent.ClearTargetFrame();
						WorldFrame userFrameForAgent = this.GetUserFrameForAgent(base.UserAgent);
						this._pointRotation = userFrameForAgent.Rotation.f;
						this._pointRotation.Normalize();
						if (base.UserAgent != Agent.Main)
						{
							base.UserAgent.SetScriptedPositionAndDirection(ref userFrameForAgent.Origin, userFrameForAgent.Rotation.f.AsVec2.RotationInRadians, false, Agent.AIScriptedFrameFlags.DoNotRun);
						}
						this._state = DynamicObjectAnimationPoint.State.Using;
						return;
					}
					break;
				case DynamicObjectAnimationPoint.State.Using:
					if (isSimulation)
					{
						float num4 = 0.1f;
						if (currentAction != this.ArriveActionCode)
						{
							num4 = 0.01f + MBRandom.RandomFloat * 0.09f;
						}
						this.SimulateAnimations(num4);
					}
					if (!this.IsArriveActionFinished && (this.ArriveActionCode == ActionIndexCache.act_none || base.UserAgent.GetCurrentAction(0) != this.ArriveActionCode))
					{
						this.IsArriveActionFinished = true;
						this.AddItemsToAgent();
					}
					if (this.IsRotationCorrectDuringUsage())
					{
						base.UserAgent.SetActionChannel(0, in this.LoopStartActionCode, false, (AnimFlags)0UL, 0f, (this.ActionSpeed < 0.8f) ? this.ActionSpeed : MBRandom.RandomFloatRanged(0.8f, this.ActionSpeed), isSimulation ? 0f : (-0.2f), 0.4f, isSimulation ? MBRandom.RandomFloatRanged(0f, 0.5f) : 0f, false, -0.2f, 0, true);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00013A9C File Offset: 0x00011C9C
		private void SetActionCodes()
		{
			this.ArriveActionCode = ActionIndexCache.Create(this.ArriveAction);
			this.LoopStartActionCode = ActionIndexCache.Create(this.LoopStartAction);
			this.LeaveActionCode = ActionIndexCache.Create(this.LeaveAction);
			this.SelectedRightHandItem = this.RightHandItem;
			this.SelectedLeftHandItem = this.LeftHandItem;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00013AF4 File Offset: 0x00011CF4
		private void InitParameters()
		{
			this._pointRotation = Vec3.Zero;
			this._state = DynamicObjectAnimationPoint.State.NotUsing;
			this.LockUserPositions = true;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00013B0F File Offset: 0x00011D0F
		protected void AssignItemToBone(AnimationPoint.ItemForBone newItem)
		{
			if (!string.IsNullOrEmpty(newItem.ItemPrefabName) && !this._itemsForBones.Contains(newItem))
			{
				this._itemsForBones.Add(newItem);
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00013B38 File Offset: 0x00011D38
		public bool IsRotationCorrectDuringUsage()
		{
			return this._pointRotation.IsNonZero && Vec2.DotProduct(this._pointRotation.AsVec2, base.UserAgent.GetMovementDirection()) > 0.99f;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00013B6B File Offset: 0x00011D6B
		protected bool CanAgentUseItem(Agent agent)
		{
			return agent.GetComponent<CampaignAgentComponent>() != null && agent.GetComponent<CampaignAgentComponent>().AgentNavigator != null;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00013B88 File Offset: 0x00011D88
		protected void AddItemsToAgent()
		{
			if (this.CanAgentUseItem(base.UserAgent) && this.IsArriveActionFinished)
			{
				if (this._itemsForBones.Count != 0)
				{
					base.UserAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.HoldAndHideRecentlyUsedMeshes();
				}
				foreach (AnimationPoint.ItemForBone itemForBone in this._itemsForBones)
				{
					ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(itemForBone.ItemPrefabName);
					if (@object != null)
					{
						EquipmentIndex equipmentIndex = this.FindProperSlot(@object);
						if (!base.UserAgent.Equipment[equipmentIndex].IsEmpty)
						{
							base.UserAgent.DropItem(equipmentIndex, WeaponClass.Undefined);
						}
						ItemObject itemObject = @object;
						ItemModifier itemModifier = null;
						IAgentOriginBase origin = base.UserAgent.Origin;
						MissionWeapon missionWeapon = new MissionWeapon(itemObject, itemModifier, (origin != null) ? origin.Banner : null);
						base.UserAgent.EquipWeaponWithNewEntity(equipmentIndex, ref missionWeapon);
						base.UserAgent.TryToWieldWeaponInSlot(equipmentIndex, Agent.WeaponWieldActionType.Instant, false);
					}
					else
					{
						sbyte realBoneIndex = base.UserAgent.AgentVisuals.GetRealBoneIndex(itemForBone.HumanBone);
						base.UserAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SetPrefabVisibility(realBoneIndex, itemForBone.ItemPrefabName, true);
					}
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00013CD8 File Offset: 0x00011ED8
		private EquipmentIndex FindProperSlot(ItemObject item)
		{
			EquipmentIndex equipmentIndex = EquipmentIndex.Weapon3;
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 <= EquipmentIndex.Weapon3; equipmentIndex2++)
			{
				if (base.UserAgent.Equipment[equipmentIndex2].IsEmpty)
				{
					equipmentIndex = equipmentIndex2;
				}
				else if (base.UserAgent.Equipment[equipmentIndex2].Item == item)
				{
					return equipmentIndex2;
				}
			}
			return equipmentIndex;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00013D34 File Offset: 0x00011F34
		private void SimulateAnimations(float dt)
		{
			base.UserAgent.TickActionChannels(dt);
			Vec3 vec = base.UserAgent.ComputeAnimationDisplacement(dt);
			if (vec.LengthSquared > 0f)
			{
				base.UserAgent.TeleportToPosition(base.UserAgent.Position + vec);
			}
			base.UserAgent.AgentVisuals.GetSkeleton().TickAnimations(dt, base.UserAgent.AgentVisuals.GetGlobalFrame(), true);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00013DAC File Offset: 0x00011FAC
		private bool IsTargetReached()
		{
			float num = Vec2.DotProduct(base.UserAgent.GetTargetDirection().AsVec2, base.UserAgent.GetMovementDirection());
			return (base.UserAgent.Position.AsVec2 - base.UserAgent.GetTargetPosition()).LengthSquared < 0.040000003f && num > 0.99f;
		}

		// Token: 0x04000194 RID: 404
		private const float RangeThreshold = 0.2f;

		// Token: 0x04000195 RID: 405
		private const float RotationScoreThreshold = 0.99f;

		// Token: 0x04000196 RID: 406
		private const float ActionSpeedRandomMinValue = 0.8f;

		// Token: 0x04000197 RID: 407
		private const float AnimationRandomProgressMaxValue = 0.5f;

		// Token: 0x04000198 RID: 408
		private const string AlternativeTag = "alternative";

		// Token: 0x04000199 RID: 409
		private ActionIndexCache _lastAction;

		// Token: 0x0400019A RID: 410
		public string ArriveAction = "";

		// Token: 0x0400019B RID: 411
		public string LoopStartAction = "";

		// Token: 0x0400019C RID: 412
		public string LeaveAction = "";

		// Token: 0x0400019D RID: 413
		public float ActionSpeed = 1f;

		// Token: 0x0400019E RID: 414
		public bool KeepOldVisibility;

		// Token: 0x0400019F RID: 415
		private Vec3 _pointRotation;

		// Token: 0x040001A0 RID: 416
		private ActionIndexCache ArriveActionCode;

		// Token: 0x040001A1 RID: 417
		protected ActionIndexCache LoopStartActionCode;

		// Token: 0x040001A2 RID: 418
		private ActionIndexCache LeaveActionCode;

		// Token: 0x040001A3 RID: 419
		protected ActionIndexCache DefaultActionCode;

		// Token: 0x040001A4 RID: 420
		private DynamicObjectAnimationPoint.State _state;

		// Token: 0x040001A5 RID: 421
		public float ForwardDistanceToPivotPoint;

		// Token: 0x040001A6 RID: 422
		public float SideDistanceToPivotPoint;

		// Token: 0x040001A8 RID: 424
		private List<AnimationPoint.ItemForBone> _itemsForBones;

		// Token: 0x040001A9 RID: 425
		public string RightHandItem = "";

		// Token: 0x040001AA RID: 426
		public HumanBone RightHandItemBone = HumanBone.ItemR;

		// Token: 0x040001AB RID: 427
		public string LeftHandItem = "";

		// Token: 0x040001AC RID: 428
		public HumanBone LeftHandItemBone = HumanBone.ItemL;

		// Token: 0x040001AD RID: 429
		private EquipmentIndex _equipmentIndexMainHand;

		// Token: 0x040001AE RID: 430
		private EquipmentIndex _equipmentIndexOffHand;

		// Token: 0x040001AF RID: 431
		public int GroupId = -1;

		// Token: 0x040001B0 RID: 432
		private string _selectedRightHandItem;

		// Token: 0x040001B1 RID: 433
		private string _selectedLeftHandItem;

		// Token: 0x02000163 RID: 355
		private enum State
		{
			// Token: 0x040006D8 RID: 1752
			NotUsing,
			// Token: 0x040006D9 RID: 1753
			StartToUse,
			// Token: 0x040006DA RID: 1754
			Using
		}
	}
}
