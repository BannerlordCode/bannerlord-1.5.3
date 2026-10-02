using System;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B2 RID: 946
	public class MapState : GameState
	{
		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x000DF4ED File Offset: 0x000DD6ED
		// (set) Token: 0x06003707 RID: 14087 RVA: 0x000DF4F5 File Offset: 0x000DD6F5
		public Incident NextIncident
		{
			get
			{
				return this._nextIncident;
			}
			set
			{
				this._nextIncident = value;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003708 RID: 14088 RVA: 0x000DF4FE File Offset: 0x000DD6FE
		// (set) Token: 0x06003709 RID: 14089 RVA: 0x000DF506 File Offset: 0x000DD706
		public MenuContext MenuContext
		{
			get
			{
				return this._menuContext;
			}
			private set
			{
				this._menuContext = value;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x000DF50F File Offset: 0x000DD70F
		// (set) Token: 0x0600370B RID: 14091 RVA: 0x000DF520 File Offset: 0x000DD720
		public string GameMenuId
		{
			get
			{
				return Campaign.Current.MapStateData.GameMenuId;
			}
			set
			{
				Campaign.Current.MapStateData.GameMenuId = value;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x0600370C RID: 14092 RVA: 0x000DF532 File Offset: 0x000DD732
		public bool AtMenu
		{
			get
			{
				return this.MenuContext != null;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x0600370D RID: 14093 RVA: 0x000DF53D File Offset: 0x000DD73D
		public bool MapConversationActive
		{
			get
			{
				return this._mapConversationActive;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x0600370E RID: 14094 RVA: 0x000DF545 File Offset: 0x000DD745
		// (set) Token: 0x0600370F RID: 14095 RVA: 0x000DF54D File Offset: 0x000DD74D
		public IMapStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06003710 RID: 14096 RVA: 0x000DF556 File Offset: 0x000DD756
		public bool IsSimulationActive
		{
			get
			{
				return this._battleSimulation != null;
			}
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x000DF561 File Offset: 0x000DD761
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnIdleTick(dt);
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000DF57B File Offset: 0x000DD77B
		private void RefreshHandler()
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRefreshState();
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000DF58D File Offset: 0x000DD78D
		public void OnJoinArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000DF595 File Offset: 0x000DD795
		public void OnLeaveArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000DF59D File Offset: 0x000DD79D
		public void OnDispersePlayerLeadedArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000DF5A5 File Offset: 0x000DD7A5
		public void OnArmyCreated(MobileParty mobileParty)
		{
			this.RefreshHandler();
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000DF5AD File Offset: 0x000DD7AD
		public void StartIncident(Incident incident)
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnIncidentStarted(incident);
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000DF5C0 File Offset: 0x000DD7C0
		public void OnMainPartyEncounter()
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMainPartyEncounter();
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000DF5D4 File Offset: 0x000DD7D4
		public void ProcessTravel(CampaignVec2 moveTargetPoint)
		{
			MobileParty.MainParty.ForceAiNoPathMode = false;
			NavigationHelper.EmbarkDisembarkData embarkDisembarkData = NavigationHelper.EmbarkDisembarkData.Invalid;
			if (MobileParty.MainParty.HasNavalNavigationCapability)
			{
				Vec2 vec = (moveTargetPoint.ToVec2() - MobileParty.MainParty.Position.ToVec2()).Normalized();
				embarkDisembarkData = NavigationHelper.GetEmbarkAndDisembarkDataForPlayer(MobileParty.MainParty.Position, vec, moveTargetPoint, moveTargetPoint.IsOnLand);
				if (embarkDisembarkData.IsTargetingTheDeadZone)
				{
					moveTargetPoint = (MobileParty.MainParty.IsTransitionInProgress ? embarkDisembarkData.TransitionEndPosition : embarkDisembarkData.TransitionStartPosition);
				}
			}
			MobileParty.NavigationType navigationType;
			if (NavigationHelper.CanPlayerNavigateToPosition(moveTargetPoint, out navigationType))
			{
				MobileParty.MainParty.SetMoveGoToPoint(moveTargetPoint, navigationType);
			}
			if (MobileParty.MainParty.HasNavalNavigationCapability && !embarkDisembarkData.IsTargetingTheDeadZone && navigationType == MobileParty.NavigationType.Naval && MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.IsTransitionInProgress)
			{
				MobileParty.MainParty.CancelNavigationTransition();
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000DF6B4 File Offset: 0x000DD8B4
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (Campaign.Current.SaveHandler.IsSaving)
			{
				Campaign.Current.SaveHandler.SaveTick();
				return;
			}
			if (this._battleSimulation != null)
			{
				this._battleSimulation.Tick(dt);
			}
			else if (this.AtMenu)
			{
				this.OnMenuModeTick(dt);
			}
			this.OnMapModeTick(dt);
			if (!Campaign.Current.SaveHandler.IsSaving)
			{
				Campaign.Current.SaveHandler.CampaignTick();
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000DF735 File Offset: 0x000DD935
		private void OnMenuModeTick(float dt)
		{
			this.MenuContext.OnTick(dt);
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMenuModeTick(dt);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000DF754 File Offset: 0x000DD954
		private void OnMapModeTick(float dt)
		{
			if (this._closeScreenNextFrame)
			{
				Game.Current.GameStateManager.CleanStates(0);
				return;
			}
			if (this.Handler != null)
			{
				this.Handler.BeforeTick(dt);
			}
			if (Campaign.Current != null && base.GameStateManager.ActiveState == this)
			{
				Campaign.Current.RealTick(dt);
				IMapStateHandler handler = this.Handler;
				if (handler != null)
				{
					handler.Tick(dt);
				}
				IMapStateHandler handler2 = this.Handler;
				if (handler2 != null)
				{
					handler2.AfterTick(dt);
				}
				Campaign.Current.Tick();
				IMapStateHandler handler3 = this.Handler;
				if (handler3 == null)
				{
					return;
				}
				handler3.AfterWaitTick(dt);
			}
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000DF7F0 File Offset: 0x000DD9F0
		public void OnLoadingFinished()
		{
			if (!string.IsNullOrEmpty(this.GameMenuId))
			{
				this.EnterMenuMode();
			}
			this.RefreshHandler();
			if (Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu != null && Campaign.Current.CurrentMenuContext.GameMenu.IsWaitMenu)
			{
				Campaign.Current.CurrentMenuContext.GameMenu.StartWait();
			}
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnGameLoadFinished();
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000DF878 File Offset: 0x000DDA78
		public void OnMapConversationStarts(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			this._mapConversationActive = true;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMapConversationStarts(playerCharacterData, conversationPartnerData);
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000DF894 File Offset: 0x000DDA94
		public void OnMapConversationOver()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnMapConversationOver();
			}
			this._mapConversationActive = false;
			if (Game.Current.GameStateManager.ActiveState is MapState)
			{
				MenuContext menuContext = this.MenuContext;
				if (menuContext != null)
				{
					menuContext.Refresh();
				}
			}
			this.RefreshHandler();
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000DF8E6 File Offset: 0x000DDAE6
		internal void OnSignalPeriodicEvents()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSignalPeriodicEvents();
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000DF8F8 File Offset: 0x000DDAF8
		internal void OnHourlyTick()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnHourlyTick();
			}
			MenuContext menuContext = this.MenuContext;
			if (menuContext == null)
			{
				return;
			}
			menuContext.OnHourlyTick();
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000DF91B File Offset: 0x000DDB1B
		protected override void OnActivate()
		{
			base.OnActivate();
			if (!Campaign.Current.ConversationManager.IsConversationFlowActive)
			{
				MenuContext menuContext = this.MenuContext;
				if (menuContext != null)
				{
					menuContext.Refresh();
				}
			}
			this.RefreshHandler();
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000DF94B File Offset: 0x000DDB4B
		public void EnterMenuMode()
		{
			this.MenuContext = MBObjectManager.Instance.CreateObject<MenuContext>();
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnEnteringMenuMode(this.MenuContext);
			}
			this.MenuContext.Refresh();
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000DF97F File Offset: 0x000DDB7F
		public void ExitMenuMode()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnExitingMenuMode();
			}
			this.MenuContext.Destroy();
			MBObjectManager.Instance.UnregisterObject(this.MenuContext);
			this.MenuContext = null;
			this.GameMenuId = null;
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000DF9BB File Offset: 0x000DDBBB
		public void StartBattleSimulation()
		{
			this._battleSimulation = PlayerEncounter.Current.BattleSimulation;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleSimulationStarted(this._battleSimulation);
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x000DF9E3 File Offset: 0x000DDBE3
		public void EndBattleSimulation()
		{
			this._battleSimulation = null;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleSimulationEnded();
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000DF9FC File Offset: 0x000DDBFC
		public void OnPlayerSiegeActivated()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSiegeActivated();
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000DFA0E File Offset: 0x000DDC0E
		public void OnPlayerSiegeDeactivated()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSiegeDeactivated();
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000DFA20 File Offset: 0x000DDC20
		public void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSiegeEngineClick(siegeEngineFrame);
		}

		// Token: 0x04000F75 RID: 3957
		private Incident _nextIncident;

		// Token: 0x04000F76 RID: 3958
		private MenuContext _menuContext;

		// Token: 0x04000F77 RID: 3959
		private bool _mapConversationActive;

		// Token: 0x04000F78 RID: 3960
		private bool _closeScreenNextFrame;

		// Token: 0x04000F79 RID: 3961
		private IMapStateHandler _handler;

		// Token: 0x04000F7A RID: 3962
		private BattleSimulation _battleSimulation;
	}
}
