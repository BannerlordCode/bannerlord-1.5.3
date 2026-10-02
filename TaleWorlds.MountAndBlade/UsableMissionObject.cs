using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000387 RID: 903
	public abstract class UsableMissionObject : SynchedMissionObject, IFocusable, IUsable, IVisible
	{
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x000D6ED2 File Offset: 0x000D50D2
		public virtual FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Item;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x000D6ED5 File Offset: 0x000D50D5
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x000D6ED8 File Offset: 0x000D50D8
		// (set) Token: 0x06003407 RID: 13319 RVA: 0x000D6EE0 File Offset: 0x000D50E0
		public Agent UserAgent
		{
			get
			{
				return this._userAgent;
			}
			private set
			{
				if (this._userAgent != value)
				{
					this.PreviousUserAgent = this._userAgent;
					this._userAgent = value;
					base.SetScriptComponentToTickMT(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003408 RID: 13320 RVA: 0x000D6F0A File Offset: 0x000D510A
		// (set) Token: 0x06003409 RID: 13321 RVA: 0x000D6F12 File Offset: 0x000D5112
		public Agent PreviousUserAgent { get; private set; }

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x000D6F1B File Offset: 0x000D511B
		// (set) Token: 0x0600340B RID: 13323 RVA: 0x000D6F23 File Offset: 0x000D5123
		public GameEntityWithWorldPosition GameEntityWithWorldPosition { get; private set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x000D6F2C File Offset: 0x000D512C
		// (set) Token: 0x0600340D RID: 13325 RVA: 0x000D6F34 File Offset: 0x000D5134
		public virtual Agent MovingAgent { get; private set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x000D6F3D File Offset: 0x000D513D
		// (set) Token: 0x0600340F RID: 13327 RVA: 0x000D6F45 File Offset: 0x000D5145
		public List<Agent> DefendingAgents { get; private set; }

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x000D6F4E File Offset: 0x000D514E
		public bool HasDefendingAgent
		{
			get
			{
				return this.DefendingAgents != null && this.GetDefendingAgentCount() > 0;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x000D6F63 File Offset: 0x000D5163
		public virtual bool DisableCombatActionsOnUse
		{
			get
			{
				return !this.IsInstantUse;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003412 RID: 13330 RVA: 0x000D6F6E File Offset: 0x000D516E
		// (set) Token: 0x06003413 RID: 13331 RVA: 0x000D6F76 File Offset: 0x000D5176
		public virtual bool LockUserFrames { get; set; }

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x000D6F7F File Offset: 0x000D517F
		// (set) Token: 0x06003415 RID: 13333 RVA: 0x000D6F87 File Offset: 0x000D5187
		public virtual bool LockUserPositions { get; set; }

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x000D6F90 File Offset: 0x000D5190
		// (set) Token: 0x06003417 RID: 13335 RVA: 0x000D6F98 File Offset: 0x000D5198
		public bool IsInstantUse { get; protected set; }

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x000D6FA1 File Offset: 0x000D51A1
		// (set) Token: 0x06003419 RID: 13337 RVA: 0x000D6FAC File Offset: 0x000D51AC
		public bool IsDeactivated
		{
			get
			{
				return this._isDeactivated;
			}
			set
			{
				if (value != this._isDeactivated)
				{
					this._isDeactivated = value;
					if (this._isDeactivated && !GameNetwork.IsClientOrReplay)
					{
						Agent userAgent = this.UserAgent;
						if (userAgent != null)
						{
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
						bool flag = false;
						while (this.HasAIMovingTo)
						{
							this.MovingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							flag = true;
						}
						while (this.HasDefendingAgent)
						{
							this.DefendingAgents[0].StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							flag = true;
						}
						if (flag)
						{
							base.SetScriptComponentToTick(this.GetTickRequirement());
						}
					}
				}
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x000D7034 File Offset: 0x000D5234
		// (set) Token: 0x0600341B RID: 13339 RVA: 0x000D703C File Offset: 0x000D523C
		public bool IsDisabledForPlayers
		{
			get
			{
				return this._isDisabledForPlayers;
			}
			set
			{
				if (value != this._isDisabledForPlayers)
				{
					this._isDisabledForPlayers = value;
					if (this._isDisabledForPlayers && !GameNetwork.IsClientOrReplay && this.UserAgent != null && !this.UserAgent.IsAIControlled)
					{
						this.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000D708A File Offset: 0x000D528A
		public virtual WeakGameEntity InteractionEntity
		{
			get
			{
				return base.GameEntity;
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x000D7092 File Offset: 0x000D5292
		public bool HasAIUser
		{
			get
			{
				return this.HasUser && this.UserAgent.IsAIControlled;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000D70A9 File Offset: 0x000D52A9
		public bool HasUser
		{
			get
			{
				return this.UserAgent != null;
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x0600341F RID: 13343 RVA: 0x000D70B4 File Offset: 0x000D52B4
		public virtual bool HasAIMovingTo
		{
			get
			{
				return this.MovingAgent != null;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000D70C0 File Offset: 0x000D52C0
		// (set) Token: 0x06003421 RID: 13345 RVA: 0x000D70DC File Offset: 0x000D52DC
		public bool IsVisible
		{
			get
			{
				return base.GameEntity.IsVisibleIncludeParents();
			}
			set
			{
				base.GameEntity.SetVisibilityExcludeParents(value);
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000D70F8 File Offset: 0x000D52F8
		protected UsableMissionObject(bool isInstantUse = false)
		{
			this._components = new List<UsableMissionObjectComponent>();
			this.IsInstantUse = isInstantUse;
			this.GameEntityWithWorldPosition = null;
			this._needsSingleThreadTickOnce = false;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000D7136 File Offset: 0x000D5336
		public virtual void OnUserConversationStart()
		{
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000D7138 File Offset: 0x000D5338
		public virtual void OnUserConversationEnd()
		{
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000D713A File Offset: 0x000D533A
		public void SetAreUserPositionsUpdatedInTheMachineTick(bool value)
		{
			this._areUserPositionsUpdatedInTheMachineTick = value;
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000D7143 File Offset: 0x000D5343
		public bool GetIsUserPositionsUpdatedInTheMachineTick()
		{
			return this._areUserPositionsUpdatedInTheMachineTick;
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000D714B File Offset: 0x000D534B
		public void SetIsDeactivatedSynched(bool value)
		{
			if (this.IsDeactivated != value)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetUsableMissionObjectIsDeactivated(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this.IsDeactivated = value;
			}
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000D7181 File Offset: 0x000D5381
		public void SetIsDisabledForPlayersSynched(bool value)
		{
			if (this.IsDisabledForPlayers != value)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetUsableMissionObjectIsDisabledForPlayers(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this.IsDisabledForPlayers = value;
			}
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000D71B7 File Offset: 0x000D53B7
		public virtual bool IsDisabledForAgent(Agent agent)
		{
			return this.IsDeactivated || agent.MountAgent != null || (this.IsDisabledForPlayers && !agent.IsAIControlled) || !agent.IsAbleToUseMachine();
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000D71E4 File Offset: 0x000D53E4
		public void AddComponent(UsableMissionObjectComponent component)
		{
			this._components.Add(component);
			component.OnAdded(base.Scene);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000D720A File Offset: 0x000D540A
		public void RemoveComponent(UsableMissionObjectComponent component)
		{
			component.OnRemoved();
			this._components.Remove(component);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000D722B File Offset: 0x000D542B
		public T GetComponent<T>() where T : UsableMissionObjectComponent
		{
			return this._components.Find((UsableMissionObjectComponent c) => c is T) as T;
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000D7261 File Offset: 0x000D5461
		private void CollectChildEntities()
		{
			this.CollectChildEntitiesAux(base.GameEntity);
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000D7270 File Offset: 0x000D5470
		private void CollectChildEntitiesAux(WeakGameEntity entity)
		{
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				this.CollectChildEntity(weakGameEntity);
				if (weakGameEntity.GetScriptCount() == 0)
				{
					this.CollectChildEntitiesAux(weakGameEntity);
				}
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000D72D0 File Offset: 0x000D54D0
		public void RefreshGameEntityWithWorldPosition()
		{
			this.GameEntityWithWorldPosition = new GameEntityWithWorldPosition(base.GameEntity);
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000D72E3 File Offset: 0x000D54E3
		protected virtual void CollectChildEntity(WeakGameEntity childEntity)
		{
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000D72E5 File Offset: 0x000D54E5
		protected virtual bool VerifyChildEntities(ref string errorMessage)
		{
			return true;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000D72E8 File Offset: 0x000D54E8
		protected internal override void OnInit()
		{
			base.OnInit();
			this.CollectChildEntities();
			this.LockUserFrames = !this.IsInstantUse;
			this.RefreshGameEntityWithWorldPosition();
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000D730B File Offset: 0x000D550B
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.CollectChildEntities();
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000D731C File Offset: 0x000D551C
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnMissionReset();
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000D7374 File Offset: 0x000D5574
		public virtual void OnFocusGain(Agent userAgent)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnFocusGain(userAgent);
			}
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000D73C8 File Offset: 0x000D55C8
		public virtual void OnFocusLose(Agent userAgent)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnFocusLose(userAgent);
			}
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000D741C File Offset: 0x000D561C
		public virtual TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000D7423 File Offset: 0x000D5623
		public virtual void SetUserForClient(Agent userAgent)
		{
			Agent userAgent2 = this.UserAgent;
			if (userAgent2 != null)
			{
				userAgent2.SetUsedGameObjectForClient(null);
			}
			this.UserAgent = userAgent;
			if (userAgent != null)
			{
				userAgent.SetUsedGameObjectForClient(this);
			}
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000D7448 File Offset: 0x000D5648
		public virtual void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				if (!userAgent.IsAIControlled && this.HasAIUser)
				{
					this.UserAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				if (this.IsAIMovingTo(userAgent))
				{
					Formation formation = userAgent.Formation;
					if (formation != null)
					{
						formation.Team.DetachmentManager.RemoveAgentAsMovingToDetachment(userAgent);
					}
					this.RemoveMovingAgent(userAgent);
					base.SetScriptComponentToTick(this.GetTickRequirement());
				}
				while (this.HasAIMovingTo && !this.IsInstantUse)
				{
					this.MovingAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
				{
					usableMissionObjectComponent.OnUse(userAgent);
				}
				this.UserAgent = userAgent;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new UseObject(userAgent.Index, base.Id));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					return;
				}
			}
			else
			{
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
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000D75A0 File Offset: 0x000D57A0
		public virtual void OnAIMoveToUse(Agent userAgent, IDetachment detachment)
		{
			this.AddMovingAgent(userAgent);
			Formation formation = userAgent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.AddAgentAsMovingToDetachment(userAgent, detachment);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000D75D4 File Offset: 0x000D57D4
		public virtual void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnUseStopped(userAgent, isSuccessful);
			}
			this.UserAgent = null;
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000D7630 File Offset: 0x000D5830
		public virtual void OnMoveToStopped(Agent movingAgent)
		{
			Formation formation = movingAgent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.RemoveAgentAsMovingToDetachment(movingAgent);
			}
			this.RemoveMovingAgent(movingAgent);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000D7661 File Offset: 0x000D5861
		public virtual int GetMovingAgentCount()
		{
			if (this.MovingAgent == null)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000D766E File Offset: 0x000D586E
		public virtual Agent GetMovingAgentWithIndex(int index)
		{
			return this.MovingAgent;
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000D7676 File Offset: 0x000D5876
		public virtual void RemoveMovingAgent(Agent movingAgent)
		{
			this.MovingAgent = null;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000D767F File Offset: 0x000D587F
		public virtual void AddMovingAgent(Agent movingAgent)
		{
			this.MovingAgent = movingAgent;
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000D7688 File Offset: 0x000D5888
		public void OnAIDefendBegin(Agent agent, IDetachment detachment)
		{
			this.AddDefendingAgent(agent);
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.AddAgentAsDefendingToDetachment(agent, detachment);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000D76BA File Offset: 0x000D58BA
		public void OnAIDefendEnd(Agent agent)
		{
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.RemoveAgentAsDefendingToDetachment(agent);
			}
			this.RemoveDefendingAgent(agent);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000D76EB File Offset: 0x000D58EB
		public void InitializeDefendingAgents()
		{
			if (this.DefendingAgents == null)
			{
				this.DefendingAgents = new List<Agent>();
			}
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000D7700 File Offset: 0x000D5900
		public int GetDefendingAgentCount()
		{
			return this.DefendingAgents.Count;
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000D770D File Offset: 0x000D590D
		public void AddDefendingAgent(Agent agent)
		{
			this.DefendingAgents.Add(agent);
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000D771B File Offset: 0x000D591B
		public void RemoveDefendingAgent(Agent agent)
		{
			this.DefendingAgents.Remove(agent);
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x000D772A File Offset: 0x000D592A
		public bool IsAgentDefending(Agent agent)
		{
			return this.DefendingAgents.Contains(agent);
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x000D7738 File Offset: 0x000D5938
		public virtual void SimulateTick(float dt)
		{
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000D773C File Offset: 0x000D593C
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (this.HasUser || this.HasAIMovingTo)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2;
			}
			if (this.HasDefendingAgent)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
			}
			using (List<UsableMissionObjectComponent>.Enumerator enumerator = this._components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsOnTickRequired())
					{
						return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
					}
				}
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000D77D0 File Offset: 0x000D59D0
		protected internal override void OnTickParallel2(float dt)
		{
			for (int i = this.GetMovingAgentCount() - 1; i >= 0; i--)
			{
				if (!this.GetMovingAgentWithIndex(i).IsActive())
				{
					this._needsSingleThreadTickOnce = true;
				}
			}
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000D7808 File Offset: 0x000D5A08
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnTick(dt);
			}
			if (!this._areUserPositionsUpdatedInTheMachineTick && this.HasUser && this.HasUserPositionsChanged(this.UserAgent))
			{
				if (this.LockUserFrames)
				{
					WorldFrame userFrameForAgent = this.GetUserFrameForAgent(this.UserAgent);
					Agent userAgent = this.UserAgent;
					Vec2 asVec = userFrameForAgent.Origin.AsVec2;
					userAgent.SetTargetPositionAndDirection(in asVec, in userFrameForAgent.Rotation.f);
				}
				else if (this.LockUserPositions)
				{
					this.UserAgent.SetTargetPosition(this.GetUserFrameForAgent(this.UserAgent).Origin.AsVec2);
				}
			}
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				for (int i = this.GetMovingAgentCount() - 1; i >= 0; i--)
				{
					Agent movingAgentWithIndex = this.GetMovingAgentWithIndex(i);
					if (!movingAgentWithIndex.IsActive())
					{
						Formation formation = movingAgentWithIndex.Formation;
						if (formation != null)
						{
							formation.Team.DetachmentManager.RemoveAgentAsMovingToDetachment(movingAgentWithIndex);
						}
						this.RemoveMovingAgent(movingAgentWithIndex);
						base.SetScriptComponentToTick(this.GetTickRequirement());
					}
				}
			}
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000D7954 File Offset: 0x000D5B54
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnEditorTick(dt);
			}
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000D79AC File Offset: 0x000D5BAC
		protected internal override void OnEditorValidate()
		{
			base.OnEditorValidate();
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnEditorValidate();
			}
			string text = null;
			if (!this.VerifyChildEntities(ref text))
			{
				MBDebug.ShowWarning(text);
			}
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000D7A14 File Offset: 0x000D5C14
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnRemoved();
			}
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000D7A6C File Offset: 0x000D5C6C
		public virtual WorldFrame GetUserFrameForAgent(Agent agent)
		{
			return this.GameEntityWithWorldPosition.WorldFrame;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000D7A7C File Offset: 0x000D5C7C
		public override string ToString()
		{
			string text = base.GetType() + " with Components:";
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				text = string.Concat(new object[] { text, "[", usableMissionObjectComponent, "]" });
			}
			return text;
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000D7B00 File Offset: 0x000D5D00
		public virtual bool IsAIMovingTo(Agent agent)
		{
			return this.MovingAgent == agent;
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000D7B0C File Offset: 0x000D5D0C
		public virtual bool HasUserPositionsChanged(Agent agent)
		{
			return base.GameEntity.GetHasFrameChanged();
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000D7B28 File Offset: 0x000D5D28
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.IsDeactivated);
			GameNetworkMessage.WriteBoolToPacket(this.IsDisabledForPlayers);
			GameNetworkMessage.WriteBoolToPacket(this.UserAgent != null);
			if (this.UserAgent != null)
			{
				GameNetworkMessage.WriteAgentIndexToPacket(this.UserAgent.Index);
			}
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x000D7B77 File Offset: 0x000D5D77
		public virtual bool IsUsableByAgent(Agent userAgent)
		{
			return true;
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000D7B7A File Offset: 0x000D5D7A
		public void SetCustomLocalFrame(in MatrixFrame customLocalFrame)
		{
			this.GameEntityWithWorldPosition.SetCustomLocalFrame(in customLocalFrame);
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x000D7B88 File Offset: 0x000D5D88
		public override void OnEndMission()
		{
			this.UserAgent = null;
			for (int i = this.GetMovingAgentCount() - 1; i >= 0; i--)
			{
				this.RemoveMovingAgent(this.GetMovingAgentWithIndex(i));
			}
			if (this.HasDefendingAgent)
			{
				for (int j = this.GetDefendingAgentCount() - 1; j >= 0; j--)
				{
					this.DefendingAgents.RemoveAt(j);
				}
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x000D7BF0 File Offset: 0x000D5DF0
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			UsableMissionObject.UsableMissionObjectRecord usableMissionObjectRecord = (UsableMissionObject.UsableMissionObjectRecord)synchedMissionObjectReadableRecord.Item2;
			this.IsDeactivated = usableMissionObjectRecord.IsDeactivated;
			this.IsDisabledForPlayers = usableMissionObjectRecord.IsDisabledForPlayers;
			if (usableMissionObjectRecord.IsUserAgentExists)
			{
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(usableMissionObjectRecord.AgentIndex, false);
				if (agentFromIndex != null)
				{
					this.SetUserForClient(agentFromIndex);
				}
			}
		}

		// Token: 0x06003458 RID: 13400
		public abstract TextObject GetDescriptionText(WeakGameEntity gameEntity);

		// Token: 0x04001614 RID: 5652
		private Agent _userAgent;

		// Token: 0x04001619 RID: 5657
		private bool _areUserPositionsUpdatedInTheMachineTick;

		// Token: 0x0400161A RID: 5658
		private readonly List<UsableMissionObjectComponent> _components;

		// Token: 0x0400161B RID: 5659
		[EditableScriptComponentVariable(false, "")]
		public TextObject DescriptionMessage = TextObject.GetEmpty();

		// Token: 0x0400161C RID: 5660
		[EditableScriptComponentVariable(false, "")]
		public TextObject ActionMessage = TextObject.GetEmpty();

		// Token: 0x0400161D RID: 5661
		private bool _needsSingleThreadTickOnce;

		// Token: 0x04001621 RID: 5665
		private bool _isDeactivated;

		// Token: 0x04001622 RID: 5666
		private bool _isDisabledForPlayers;

		// Token: 0x02000663 RID: 1635
		[DefineSynchedMissionObjectType(typeof(UsableMissionObject))]
		public struct UsableMissionObjectRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AE8 RID: 2792
			// (get) Token: 0x06004134 RID: 16692 RVA: 0x000FCEC7 File Offset: 0x000FB0C7
			// (set) Token: 0x06004135 RID: 16693 RVA: 0x000FCECF File Offset: 0x000FB0CF
			public bool IsDeactivated { get; private set; }

			// Token: 0x17000AE9 RID: 2793
			// (get) Token: 0x06004136 RID: 16694 RVA: 0x000FCED8 File Offset: 0x000FB0D8
			// (set) Token: 0x06004137 RID: 16695 RVA: 0x000FCEE0 File Offset: 0x000FB0E0
			public bool IsDisabledForPlayers { get; private set; }

			// Token: 0x17000AEA RID: 2794
			// (get) Token: 0x06004138 RID: 16696 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
			// (set) Token: 0x06004139 RID: 16697 RVA: 0x000FCEF1 File Offset: 0x000FB0F1
			public bool IsUserAgentExists { get; private set; }

			// Token: 0x17000AEB RID: 2795
			// (get) Token: 0x0600413A RID: 16698 RVA: 0x000FCEFA File Offset: 0x000FB0FA
			// (set) Token: 0x0600413B RID: 16699 RVA: 0x000FCF02 File Offset: 0x000FB102
			public int AgentIndex { get; private set; }

			// Token: 0x0600413C RID: 16700 RVA: 0x000FCF0B File Offset: 0x000FB10B
			public UsableMissionObjectRecord(bool isDeactivated, bool isDisabledForPlayers, bool isUserAgentExists, int agentIndex)
			{
				this.IsDeactivated = isDeactivated;
				this.IsDisabledForPlayers = isDisabledForPlayers;
				this.IsUserAgentExists = isUserAgentExists;
				this.AgentIndex = agentIndex;
			}

			// Token: 0x0600413D RID: 16701 RVA: 0x000FCF2A File Offset: 0x000FB12A
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.IsDeactivated = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.IsDisabledForPlayers = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.IsUserAgentExists = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				if (this.IsUserAgentExists)
				{
					this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref bufferReadValid);
				}
				return bufferReadValid;
			}
		}
	}
}
