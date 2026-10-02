using System;
using System.Collections.Generic;
using SandBox.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace SandBox.Objects.AnimationPoints
{
	// Token: 0x02000052 RID: 82
	public class AnimationPoint : StandingPoint
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00010FED File Offset: 0x0000F1ED
		public override bool PlayerStopsUsingWhenInteractsWithOther
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00010FF0 File Offset: 0x0000F1F0
		// (set) Token: 0x060002FF RID: 767 RVA: 0x00010FF8 File Offset: 0x0000F1F8
		public bool IsArriveActionFinished { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00011001 File Offset: 0x0000F201
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0001100C File Offset: 0x0000F20C
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00011043 File Offset: 0x0000F243
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0001104C File Offset: 0x0000F24C
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000304 RID: 772 RVA: 0x00011083 File Offset: 0x0000F283
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0001108B File Offset: 0x0000F28B
		public bool IsActive { get; private set; } = true;

		// Token: 0x06000306 RID: 774 RVA: 0x00011094 File Offset: 0x0000F294
		public AnimationPoint()
		{
			this._greetingTimer = null;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00011141 File Offset: 0x0000F341
		public override bool DisableCombatActionsOnUse
		{
			get
			{
				return !base.IsInstantUse;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001114C File Offset: 0x0000F34C
		private void CreateVisualizer()
		{
			if (this.PairLoopStartActionCode != ActionIndexCache.act_none || this.LoopStartActionCode != ActionIndexCache.act_none)
			{
				this._animatedEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(base.GameEntity.Scene, false, true, true);
				this._animatedEntity.EntityFlags |= EntityFlags.DontSaveToScene;
				this._animatedEntity.Name = "ap_visual_entity";
				MBActionSet mbactionSet = MBActionSet.GetActionSetWithIndex(0);
				ActionIndexCache actionIndexCache = ActionIndexCache.act_none;
				int numberOfActionSets = MBActionSet.GetNumberOfActionSets();
				for (int i = 0; i < numberOfActionSets; i++)
				{
					MBActionSet actionSetWithIndex = MBActionSet.GetActionSetWithIndex(i);
					if (this.ArriveActionCode == ActionIndexCache.act_none || MBActionSet.CheckActionAnimationClipExists(actionSetWithIndex, in this.ArriveActionCode))
					{
						if (this.PairLoopStartActionCode != ActionIndexCache.act_none && MBActionSet.CheckActionAnimationClipExists(actionSetWithIndex, in this.PairLoopStartActionCode))
						{
							mbactionSet = actionSetWithIndex;
							actionIndexCache = this.PairLoopStartActionCode;
							break;
						}
						if (this.LoopStartActionCode != ActionIndexCache.act_none && MBActionSet.CheckActionAnimationClipExists(actionSetWithIndex, in this.LoopStartActionCode))
						{
							mbactionSet = actionSetWithIndex;
							actionIndexCache = this.LoopStartActionCode;
							break;
						}
					}
				}
				if (actionIndexCache == ActionIndexCache.act_none)
				{
					actionIndexCache = ActionIndexCache.act_jump_loop;
				}
				this._animatedEntity.CreateAgentSkeleton("human_skeleton", true, mbactionSet, "human", MBObjectManager.Instance.GetObject<Monster>("human"));
				this._animatedEntity.Skeleton.SetAgentActionChannel(0, in actionIndexCache, 0f, -0.2f, true, 0f);
				this._animatedEntity.AddMultiMeshToSkeleton(MetaMesh.GetCopy("roman_cloth_tunic_a", true, false));
				this._animatedEntity.AddMultiMeshToSkeleton(MetaMesh.GetCopy("casual_02_boots", true, false));
				this._animatedEntity.AddMultiMeshToSkeleton(MetaMesh.GetCopy("hands_male_a", true, false));
				this._animatedEntity.AddMultiMeshToSkeleton(MetaMesh.GetCopy("head_male_a", true, false));
				this._animatedEntityDisplacement = Vec3.Zero;
				if (this.ArriveActionCode != ActionIndexCache.act_none && (MBActionSet.GetActionAnimationFlags(mbactionSet, in this.ArriveActionCode) & AnimFlags.anf_displace_position) != (AnimFlags)0UL)
				{
					this._animatedEntityDisplacement = MBActionSet.GetActionDisplacementVector(mbactionSet, in this.ArriveActionCode);
				}
				this.UpdateAnimatedEntityFrame();
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00011378 File Offset: 0x0000F578
		private void UpdateAnimatedEntityFrame()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Mat3 identity = Mat3.Identity;
			MatrixFrame matrixFrame = new MatrixFrame(in identity, in this._animatedEntityDisplacement);
			matrixFrame = globalFrame.TransformToParent(in matrixFrame);
			globalFrame.origin = matrixFrame.origin;
			this._animatedEntity.SetFrame(ref globalFrame, true);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000113D0 File Offset: 0x0000F5D0
		protected override void OnEditModeVisibilityChanged(bool currentVisibility)
		{
			if (this._animatedEntity != null)
			{
				this._animatedEntity.SetVisibilityExcludeParents(currentVisibility);
				if (!base.GameEntity.IsGhostObject())
				{
					this._resyncAnimations = true;
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00011410 File Offset: 0x0000F610
		protected override void OnEditorTick(float dt)
		{
			if (this._animatedEntity != null)
			{
				if (this._resyncAnimations)
				{
					this.ResetAnimations();
					this._resyncAnimations = false;
				}
				bool flag = this._animatedEntity.IsVisibleIncludeParents();
				if (flag && !MBEditor.HelpersEnabled())
				{
					this._animatedEntity.SetVisibilityExcludeParents(false);
					flag = false;
				}
				if (flag)
				{
					this.UpdateAnimatedEntityFrame();
				}
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00011470 File Offset: 0x0000F670
		protected override void OnEditorInit()
		{
			this._itemsForBones = new List<AnimationPoint.ItemForBone>();
			this.SetActionCodes();
			this.InitParameters();
			if (!base.GameEntity.IsGhostObject())
			{
				this.CreateVisualizer();
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000114AC File Offset: 0x0000F6AC
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			if (this._animatedEntity != null && this._animatedEntity.Scene == base.GameEntity.Scene)
			{
				this._animatedEntity.Remove(removeReason);
				this._animatedEntity = null;
			}
			this.PairEntity = null;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00011508 File Offset: 0x0000F708
		protected void ResetAnimations()
		{
			ActionIndexCache actionIndexCache = ActionIndexCache.act_none;
			int numberOfActionSets = MBActionSet.GetNumberOfActionSets();
			for (int i = 0; i < numberOfActionSets; i++)
			{
				MBActionSet actionSetWithIndex = MBActionSet.GetActionSetWithIndex(i);
				if (this.ArriveActionCode == ActionIndexCache.act_none || MBActionSet.CheckActionAnimationClipExists(actionSetWithIndex, in this.ArriveActionCode))
				{
					if (this.PairLoopStartActionCode != ActionIndexCache.act_none && MBActionSet.CheckActionAnimationClipExists(actionSetWithIndex, in this.PairLoopStartActionCode))
					{
						actionIndexCache = this.PairLoopStartActionCode;
						break;
					}
					if (this.LoopStartActionCode != ActionIndexCache.act_none && MBActionSet.CheckActionAnimationClipExists(actionSetWithIndex, in this.LoopStartActionCode))
					{
						actionIndexCache = this.LoopStartActionCode;
						break;
					}
				}
			}
			if (actionIndexCache != ActionIndexCache.act_none)
			{
				this._animatedEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_jump_loop, 0f, -0.2f, true, 0f);
				this._animatedEntity.Skeleton.SetAgentActionChannel(0, in actionIndexCache, 0f, -0.2f, true, 0f);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000115FF File Offset: 0x0000F7FF
		protected override void OnEditorVariableChanged(string variableName)
		{
			if (this.ShouldUpdateOnEditorVariableChanged(variableName))
			{
				if (this._animatedEntity != null)
				{
					this._animatedEntity.Remove(91);
				}
				this.SetActionCodes();
				this.CreateVisualizer();
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00011631 File Offset: 0x0000F831
		public void RequestResync()
		{
			this._resyncAnimations = true;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0001163A File Offset: 0x0000F83A
		public override void AfterMissionStart()
		{
			if (Agent.Main != null && this.LoopStartActionCode != ActionIndexCache.act_none && !MBActionSet.CheckActionAnimationClipExists(Agent.Main.ActionSet, in this.LoopStartActionCode))
			{
				base.IsDisabledForPlayers = true;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00011673 File Offset: 0x0000F873
		protected virtual bool ShouldUpdateOnEditorVariableChanged(string variableName)
		{
			return variableName == "ArriveAction" || variableName == "LoopStartAction" || variableName == "PairLoopStartAction";
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0001169C File Offset: 0x0000F89C
		protected void ClearAssignedItems()
		{
			this.SetAgentItemsVisibility(false);
			this._itemsForBones.Clear();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000116B0 File Offset: 0x0000F8B0
		protected void AssignItemToBone(AnimationPoint.ItemForBone newItem)
		{
			if (!string.IsNullOrEmpty(newItem.ItemPrefabName) && !this._itemsForBones.Contains(newItem))
			{
				this._itemsForBones.Add(newItem);
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000116DC File Offset: 0x0000F8DC
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

		// Token: 0x06000316 RID: 790 RVA: 0x0001187C File Offset: 0x0000FA7C
		protected override void OnInit()
		{
			base.OnInit();
			this._itemsForBones = new List<AnimationPoint.ItemForBone>();
			this.SetActionCodes();
			this.InitParameters();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000118A8 File Offset: 0x0000FAA8
		private void InitParameters()
		{
			this._greetingTimer = null;
			this._pointRotation = Vec3.Zero;
			this._state = AnimationPoint.State.NotUsing;
			this._pairPoints = this.GetPairs(this.PairEntity);
			if (this.ActivatePairs)
			{
				this.SetPairsActivity(false);
			}
			this.LockUserPositions = true;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000118F8 File Offset: 0x0000FAF8
		protected virtual void SetActionCodes()
		{
			this.ArriveActionCode = ActionIndexCache.Create(this.ArriveAction);
			this.LoopStartActionCode = ActionIndexCache.Create(this.LoopStartAction);
			this.PairLoopStartActionCode = ActionIndexCache.Create(this.PairLoopStartAction);
			this.LeaveActionCode = ActionIndexCache.Create(this.LeaveAction);
			this.SelectedRightHandItem = this.RightHandItem;
			this.SelectedLeftHandItem = this.LeftHandItem;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011961 File Offset: 0x0000FB61
		protected override bool DoesActionTypeStopUsingGameObject(Agent.ActionCodeType actionType)
		{
			return false;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00011964 File Offset: 0x0000FB64
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.HasUser)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001197D File Offset: 0x0000FB7D
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.Tick(dt, false);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00011990 File Offset: 0x0000FB90
		private List<AnimationPoint> GetPairs(GameEntity entity)
		{
			List<AnimationPoint> list = new List<AnimationPoint>();
			if (entity != null)
			{
				if (entity.HasScriptOfType<AnimationPoint>())
				{
					AnimationPoint firstScriptOfType = entity.GetFirstScriptOfType<AnimationPoint>();
					list.Add(firstScriptOfType);
				}
				else
				{
					foreach (GameEntity gameEntity in entity.GetChildren())
					{
						list.AddRange(this.GetPairs(gameEntity));
					}
				}
			}
			if (list.Contains(this))
			{
				list.Remove(this);
			}
			return list;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00011A1C File Offset: 0x0000FC1C
		public override WorldFrame GetUserFrameForAgent(Agent agent)
		{
			WorldFrame userFrameForAgent = base.GetUserFrameForAgent(agent);
			float agentScale = agent.AgentScale;
			userFrameForAgent.Origin.SetVec2(userFrameForAgent.Origin.AsVec2 + (userFrameForAgent.Rotation.f.AsVec2 * -this.ForwardDistanceToPivotPoint + userFrameForAgent.Rotation.s.AsVec2 * this.SideDistanceToPivotPoint) * (1f - agentScale));
			return userFrameForAgent;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00011AA0 File Offset: 0x0000FCA0
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
				case AnimationPoint.State.NotUsing:
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
							if (base.UserAgent.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_displace_position))
							{
								base.UserAgent.ClearTargetFrame();
								this.LockUserFrames = false;
								this.LockUserPositions = false;
							}
						}
						this._state = AnimationPoint.State.StartToUse;
						return;
					}
					break;
				case AnimationPoint.State.StartToUse:
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
						this._state = AnimationPoint.State.Using;
						return;
					}
					break;
				case AnimationPoint.State.Using:
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
					if (this.IsArriveActionFinished && base.UserAgent != Agent.Main)
					{
						this.PairTick(isSimulation);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00011DC4 File Offset: 0x0000FFC4
		private void PairTick(bool isSimulation)
		{
			MBList<Agent> pairEntityUsers = this.GetPairEntityUsers();
			if (this.PairEntity != null)
			{
				bool flag = base.UserAgent != ConversationMission.OneToOneConversationAgent && pairEntityUsers.Count + 1 >= this.MinUserToStartInteraction;
				this.SetAgentItemsVisibility(flag);
			}
			if (this._pairState != AnimationPoint.PairState.NoPair && pairEntityUsers.Count < this.MinUserToStartInteraction)
			{
				this._pairState = AnimationPoint.PairState.NoPair;
				if (base.UserAgent != ConversationMission.OneToOneConversationAgent)
				{
					base.UserAgent.SetActionChannel(0, in this._lastAction, false, (AnimFlags)((long)Math.Min(base.UserAgent.GetCurrentActionPriority(0), 73)), 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					base.UserAgent.ResetLookAgent();
				}
				this._greetingTimer = null;
			}
			else if (this._pairState == AnimationPoint.PairState.NoPair && pairEntityUsers.Count >= this.MinUserToStartInteraction && this.IsRotationCorrectDuringUsage())
			{
				this._lastAction = base.UserAgent.GetCurrentAction(0);
				if (this._startPairAnimationWithGreeting)
				{
					this._pairState = AnimationPoint.PairState.BecomePair;
					this._greetingTimer = new Timer(Mission.Current.CurrentTime, (float)MBRandom.RandomInt(5) * 0.3f, true);
				}
				else
				{
					this._pairState = AnimationPoint.PairState.StartPairAnimation;
				}
			}
			else if (this._pairState == AnimationPoint.PairState.BecomePair && this._greetingTimer.Check(Mission.Current.CurrentTime))
			{
				this._greetingTimer = null;
				this._pairState = AnimationPoint.PairState.Greeting;
				Vec3 eyeGlobalPosition = pairEntityUsers.GetRandomElement<Agent>().GetEyeGlobalPosition();
				Vec3 eyeGlobalPosition2 = base.UserAgent.GetEyeGlobalPosition();
				Vec3 vec = eyeGlobalPosition - eyeGlobalPosition2;
				vec.Normalize();
				Mat3 rotation = base.UserAgent.Frame.rotation;
				if (Vec3.DotProduct(rotation.f, vec) > 0f)
				{
					ActionIndexCache greetingActionId = this.GetGreetingActionId(eyeGlobalPosition2, eyeGlobalPosition, rotation);
					base.UserAgent.SetActionChannel(1, in greetingActionId, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
			}
			else if (this._pairState == AnimationPoint.PairState.Greeting && base.UserAgent.GetCurrentAction(1) == ActionIndexCache.act_none)
			{
				this._pairState = AnimationPoint.PairState.StartPairAnimation;
			}
			if (this._pairState == AnimationPoint.PairState.StartPairAnimation)
			{
				this._pairState = AnimationPoint.PairState.Pair;
				base.UserAgent.SetActionChannel(0, in this.PairLoopStartActionCode, false, (AnimFlags)0UL, 0f, MBRandom.RandomFloatRanged(0.8f, this.ActionSpeed), isSimulation ? 0f : (-0.2f), 0.4f, isSimulation ? MBRandom.RandomFloatRanged(0f, 0.5f) : 0f, false, -0.2f, 0, true);
			}
			if (this._pairState == AnimationPoint.PairState.Pair && this.IsRotationCorrectDuringUsage())
			{
				base.UserAgent.SetActionChannel(0, in this.PairLoopStartActionCode, false, (AnimFlags)0UL, 0f, MBRandom.RandomFloatRanged(0.8f, this.ActionSpeed), isSimulation ? 0f : (-0.2f), 0.4f, isSimulation ? MBRandom.RandomFloatRanged(0f, 0.5f) : 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000120E0 File Offset: 0x000102E0
		private ActionIndexCache GetGreetingActionId(Vec3 userAgentGlobalEyePoint, Vec3 lookTarget, Mat3 userAgentRot)
		{
			Vec3 vec = lookTarget - userAgentGlobalEyePoint;
			vec.Normalize();
			float num = Vec3.DotProduct(userAgentRot.f, vec);
			if (num > 0.8f)
			{
				return AnimationPoint._greetingFrontActions[MBRandom.RandomInt(AnimationPoint._greetingFrontActions.Length)];
			}
			if (num <= 0f)
			{
				return ActionIndexCache.act_none;
			}
			if (Vec3.DotProduct(Vec3.CrossProduct(vec, userAgentRot.f), userAgentRot.u) <= 0f)
			{
				return AnimationPoint._greetingLeftActions[MBRandom.RandomInt(AnimationPoint._greetingLeftActions.Length)];
			}
			return AnimationPoint._greetingRightActions[MBRandom.RandomInt(AnimationPoint._greetingRightActions.Length)];
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00012184 File Offset: 0x00010384
		private MBList<Agent> GetPairEntityUsers()
		{
			MBList<Agent> mblist = new MBList<Agent>();
			if (base.UserAgent != ConversationMission.OneToOneConversationAgent)
			{
				foreach (AnimationPoint animationPoint in this._pairPoints)
				{
					if (animationPoint.HasUser && animationPoint._state == AnimationPoint.State.Using && animationPoint.UserAgent != ConversationMission.OneToOneConversationAgent)
					{
						mblist.Add(animationPoint.UserAgent);
					}
				}
			}
			return mblist;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00012210 File Offset: 0x00010410
		private void SetPairsActivity(bool isActive)
		{
			foreach (AnimationPoint animationPoint in this._pairPoints)
			{
				animationPoint.IsActive = isActive;
				if (!isActive)
				{
					if (animationPoint.HasAIUser)
					{
						animationPoint.UserAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					Agent movingAgent = animationPoint.MovingAgent;
					if (movingAgent != null)
					{
						movingAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00012290 File Offset: 0x00010490
		public override bool IsUsableByAgent(Agent userAgent)
		{
			return this.IsActive && base.IsUsableByAgent(userAgent);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000122A4 File Offset: 0x000104A4
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.OnUse(userAgent, agentBoneIndex);
			this._equipmentIndexMainHand = base.UserAgent.GetPrimaryWieldedItemIndex();
			this._equipmentIndexOffHand = base.UserAgent.GetOffhandWieldedItemIndex();
			this._state = AnimationPoint.State.NotUsing;
			if (this.ActivatePairs)
			{
				this.SetPairsActivity(true);
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000122F4 File Offset: 0x000104F4
		private void RevertWeaponWieldSheathState()
		{
			if (this._equipmentIndexMainHand != EquipmentIndex.None && this.AutoSheathWeapons)
			{
				base.UserAgent.Mission.AddTickActionMT(Mission.MissionTickAction.TryToWieldWeaponInSlot, base.UserAgent, (int)this._equipmentIndexMainHand, 0);
			}
			else if (this._equipmentIndexMainHand == EquipmentIndex.None && this.AutoWieldWeapons)
			{
				base.UserAgent.Mission.AddTickActionMT(Mission.MissionTickAction.TryToSheathWeaponInHand, base.UserAgent, 0, 0);
			}
			if (this._equipmentIndexOffHand != EquipmentIndex.None && this.AutoSheathWeapons)
			{
				base.UserAgent.Mission.AddTickActionMT(Mission.MissionTickAction.TryToWieldWeaponInSlot, base.UserAgent, (int)this._equipmentIndexOffHand, 0);
				return;
			}
			if (this._equipmentIndexOffHand == EquipmentIndex.None && this.AutoWieldWeapons)
			{
				base.UserAgent.Mission.AddTickActionMT(Mission.MissionTickAction.TryToSheathWeaponInHand, base.UserAgent, 1, 0);
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000123B8 File Offset: 0x000105B8
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
						float num = 0f;
						MBActionSet actionSet = userAgent.ActionSet;
						float currentActionProgress = userAgent.GetCurrentActionProgress(0);
						float actionBlendOutStartProgress = MBActionSet.GetActionBlendOutStartProgress(actionSet, in currentAction2);
						if (currentActionProgress < actionBlendOutStartProgress)
						{
							float num2 = (actionBlendOutStartProgress - currentActionProgress) / actionBlendOutStartProgress;
							num = MBActionSet.GetActionBlendOutStartProgress(actionSet, in this.LeaveActionCode) * num2;
						}
						base.UserAgent.SetActionChannel(0, in this.LeaveActionCode, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, num, false, -0.2f, 0, true);
					}
				}
			}
			this._pairState = AnimationPoint.PairState.NoPair;
			this._lastAction = ActionIndexCache.act_none;
			if (base.UserAgent.GetLookAgent() != null)
			{
				base.UserAgent.ResetLookAgent();
			}
			this.IsArriveActionFinished = false;
			this.LockUserFrames = true;
			this.LockUserPositions = true;
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (this.ActivatePairs)
			{
				this.SetPairsActivity(false);
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000125E7 File Offset: 0x000107E7
		public override void SimulateTick(float dt)
		{
			this.Tick(dt, true);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000125F1 File Offset: 0x000107F1
		public override bool HasAlternative()
		{
			return this.GroupId >= 0;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00012600 File Offset: 0x00010800
		public float GetRandomWaitInSeconds()
		{
			if (this.MinWaitinSeconds < 0f || this.MaxWaitInSeconds < 0f)
			{
				return -1f;
			}
			if (MathF.Abs(this.MinWaitinSeconds - this.MaxWaitInSeconds) >= 1E-45f)
			{
				return this.MinWaitinSeconds + MBRandom.RandomFloat * (this.MaxWaitInSeconds - this.MinWaitinSeconds);
			}
			return this.MinWaitinSeconds;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00012668 File Offset: 0x00010868
		public List<AnimationPoint> GetAlternatives()
		{
			List<AnimationPoint> list = new List<AnimationPoint>();
			IEnumerable<WeakGameEntity> children = base.GameEntity.Parent.GetChildren();
			if (children != null)
			{
				foreach (WeakGameEntity weakGameEntity in children)
				{
					AnimationPoint firstScriptOfType = weakGameEntity.GetFirstScriptOfType<AnimationPoint>();
					if (firstScriptOfType != null && firstScriptOfType.HasAlternative() && this.GroupId == firstScriptOfType.GroupId)
					{
						list.Add(firstScriptOfType);
					}
				}
			}
			return list;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000126F8 File Offset: 0x000108F8
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

		// Token: 0x0600032C RID: 812 RVA: 0x00012770 File Offset: 0x00010970
		private bool IsTargetReached()
		{
			float num = Vec2.DotProduct(base.UserAgent.GetTargetDirection().AsVec2, base.UserAgent.GetMovementDirection());
			return (base.UserAgent.Position.AsVec2 - base.UserAgent.GetTargetPosition()).LengthSquared < 0.040000003f && num > 0.99f;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000127DD File Offset: 0x000109DD
		public bool IsRotationCorrectDuringUsage()
		{
			return this._pointRotation.IsNonZero && Vec2.DotProduct(this._pointRotation.AsVec2, base.UserAgent.GetMovementDirection()) > 0.99f;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00012810 File Offset: 0x00010A10
		protected bool CanAgentUseItem(Agent agent)
		{
			return agent.GetComponent<CampaignAgentComponent>() != null && agent.GetComponent<CampaignAgentComponent>().AgentNavigator != null;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001282C File Offset: 0x00010A2C
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
						this.SetAgentItemVisibility(itemForBone, true);
					}
				}
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001294C File Offset: 0x00010B4C
		public override void OnUserConversationStart()
		{
			this._pointRotation = base.UserAgent.Frame.rotation.f;
			this._pointRotation.Normalize();
			if (this.KeepOldVisibility)
			{
				foreach (AnimationPoint.ItemForBone itemForBone in this._itemsForBones)
				{
					itemForBone.OldVisibility = itemForBone.IsVisible;
				}
				this.SetAgentItemsVisibility(false);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000129D8 File Offset: 0x00010BD8
		public override void OnUserConversationEnd()
		{
			base.UserAgent.ResetLookAgent();
			base.UserAgent.LookDirection = this._pointRotation;
			base.UserAgent.SetActionChannel(0, in this.LoopStartActionCode, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			if (this.KeepOldVisibility)
			{
				foreach (AnimationPoint.ItemForBone itemForBone in this._itemsForBones)
				{
					if (itemForBone.OldVisibility)
					{
						this.SetAgentItemVisibility(itemForBone, true);
					}
				}
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00012A90 File Offset: 0x00010C90
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

		// Token: 0x06000333 RID: 819 RVA: 0x00012AF4 File Offset: 0x00010CF4
		private void SetAgentItemVisibility(AnimationPoint.ItemForBone item, bool isVisible)
		{
			sbyte realBoneIndex = base.UserAgent.AgentVisuals.GetRealBoneIndex(item.HumanBone);
			base.UserAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SetPrefabVisibility(realBoneIndex, item.ItemPrefabName, isVisible);
			item.IsVisible = isVisible;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012B3C File Offset: 0x00010D3C
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

		// Token: 0x0400014F RID: 335
		private const string AlternativeTag = "alternative";

		// Token: 0x04000150 RID: 336
		private const float RangeThreshold = 0.2f;

		// Token: 0x04000151 RID: 337
		private const float RotationScoreThreshold = 0.99f;

		// Token: 0x04000152 RID: 338
		private const float ActionSpeedRandomMinValue = 0.8f;

		// Token: 0x04000153 RID: 339
		private const float AnimationRandomProgressMaxValue = 0.5f;

		// Token: 0x04000154 RID: 340
		private static readonly ActionIndexCache[] _greetingFrontActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_greeting_front_1,
			ActionIndexCache.act_greeting_front_2,
			ActionIndexCache.act_greeting_front_3,
			ActionIndexCache.act_greeting_front_4
		};

		// Token: 0x04000155 RID: 341
		private static readonly ActionIndexCache[] _greetingRightActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_greeting_right_1,
			ActionIndexCache.act_greeting_right_2,
			ActionIndexCache.act_greeting_right_3,
			ActionIndexCache.act_greeting_right_4
		};

		// Token: 0x04000156 RID: 342
		private static readonly ActionIndexCache[] _greetingLeftActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_greeting_left_1,
			ActionIndexCache.act_greeting_left_2,
			ActionIndexCache.act_greeting_left_3,
			ActionIndexCache.act_greeting_left_4
		};

		// Token: 0x04000157 RID: 343
		public string ArriveAction = "";

		// Token: 0x04000158 RID: 344
		public string LoopStartAction = "";

		// Token: 0x04000159 RID: 345
		public string PairLoopStartAction = "";

		// Token: 0x0400015A RID: 346
		public string LeaveAction = "";

		// Token: 0x0400015B RID: 347
		public int GroupId = -1;

		// Token: 0x0400015C RID: 348
		public string RightHandItem = "";

		// Token: 0x0400015D RID: 349
		public HumanBone RightHandItemBone = HumanBone.ItemR;

		// Token: 0x0400015E RID: 350
		public string LeftHandItem = "";

		// Token: 0x0400015F RID: 351
		public HumanBone LeftHandItemBone = HumanBone.ItemL;

		// Token: 0x04000160 RID: 352
		public GameEntity PairEntity;

		// Token: 0x04000161 RID: 353
		public int MinUserToStartInteraction = 1;

		// Token: 0x04000162 RID: 354
		public bool ActivatePairs;

		// Token: 0x04000163 RID: 355
		public float MinWaitinSeconds = 30f;

		// Token: 0x04000164 RID: 356
		public float MaxWaitInSeconds = 120f;

		// Token: 0x04000165 RID: 357
		public float ForwardDistanceToPivotPoint;

		// Token: 0x04000166 RID: 358
		public float SideDistanceToPivotPoint;

		// Token: 0x04000167 RID: 359
		private bool _startPairAnimationWithGreeting;

		// Token: 0x04000168 RID: 360
		protected float ActionSpeed = 1f;

		// Token: 0x04000169 RID: 361
		public bool KeepOldVisibility;

		// Token: 0x0400016B RID: 363
		private ActionIndexCache ArriveActionCode;

		// Token: 0x0400016C RID: 364
		protected ActionIndexCache LoopStartActionCode;

		// Token: 0x0400016D RID: 365
		protected ActionIndexCache PairLoopStartActionCode;

		// Token: 0x0400016E RID: 366
		private ActionIndexCache LeaveActionCode;

		// Token: 0x0400016F RID: 367
		protected ActionIndexCache DefaultActionCode;

		// Token: 0x04000170 RID: 368
		private bool _resyncAnimations;

		// Token: 0x04000171 RID: 369
		private string _selectedRightHandItem;

		// Token: 0x04000172 RID: 370
		private string _selectedLeftHandItem;

		// Token: 0x04000173 RID: 371
		private AnimationPoint.State _state;

		// Token: 0x04000174 RID: 372
		private AnimationPoint.PairState _pairState;

		// Token: 0x04000175 RID: 373
		private Vec3 _pointRotation;

		// Token: 0x04000176 RID: 374
		private List<AnimationPoint> _pairPoints;

		// Token: 0x04000177 RID: 375
		private List<AnimationPoint.ItemForBone> _itemsForBones;

		// Token: 0x04000178 RID: 376
		private ActionIndexCache _lastAction;

		// Token: 0x04000179 RID: 377
		private Timer _greetingTimer;

		// Token: 0x0400017A RID: 378
		private GameEntity _animatedEntity;

		// Token: 0x0400017B RID: 379
		private Vec3 _animatedEntityDisplacement = Vec3.Zero;

		// Token: 0x0400017C RID: 380
		private EquipmentIndex _equipmentIndexMainHand;

		// Token: 0x0400017D RID: 381
		private EquipmentIndex _equipmentIndexOffHand;

		// Token: 0x0200015F RID: 351
		public class ItemForBone
		{
			// Token: 0x06000E81 RID: 3713 RVA: 0x0006643B File Offset: 0x0006463B
			public ItemForBone(HumanBone bone, string name, bool isVisible)
			{
				this.HumanBone = bone;
				this.ItemPrefabName = name;
				this.IsVisible = isVisible;
				this.OldVisibility = isVisible;
			}

			// Token: 0x040006C4 RID: 1732
			public HumanBone HumanBone;

			// Token: 0x040006C5 RID: 1733
			public string ItemPrefabName;

			// Token: 0x040006C6 RID: 1734
			public bool IsVisible;

			// Token: 0x040006C7 RID: 1735
			public bool OldVisibility;
		}

		// Token: 0x02000160 RID: 352
		private enum State
		{
			// Token: 0x040006C9 RID: 1737
			NotUsing,
			// Token: 0x040006CA RID: 1738
			StartToUse,
			// Token: 0x040006CB RID: 1739
			Using
		}

		// Token: 0x02000161 RID: 353
		private enum PairState
		{
			// Token: 0x040006CD RID: 1741
			NoPair,
			// Token: 0x040006CE RID: 1742
			BecomePair,
			// Token: 0x040006CF RID: 1743
			Greeting,
			// Token: 0x040006D0 RID: 1744
			StartPairAnimation,
			// Token: 0x040006D1 RID: 1745
			Pair
		}
	}
}
