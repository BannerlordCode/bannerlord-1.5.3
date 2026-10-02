using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006A RID: 106
	public class MissionAgentLabelView : MissionView
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001E8A2 File Offset: 0x0001CAA2
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0001E8AA File Offset: 0x0001CAAA
		private bool IndicatorsActive
		{
			get
			{
				return this._indicatorsActive;
			}
			set
			{
				if (this._indicatorsActive != value)
				{
					this._indicatorsActive = value;
					this.UpdateAllAgentMeshVisibilities();
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001E8C2 File Offset: 0x0001CAC2
		private OrderController PlayerOrderController
		{
			get
			{
				Team playerTeam = base.Mission.PlayerTeam;
				if (playerTeam == null)
				{
					return null;
				}
				return playerTeam.PlayerOrderController;
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001E8DC File Offset: 0x0001CADC
		public MissionAgentLabelView()
		{
			this._agentMeshes = new Dictionary<Agent, MetaMesh>();
			this._labelMaterials = new Dictionary<Texture, Material>();
			this._closeAgentsWithMeshes = new List<Agent>();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001E930 File Offset: 0x0001CB30
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.Mission.Teams.OnPlayerTeamChanged += this.Mission_OnPlayerTeamChanged;
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			base.MissionScreen.OnSpectateAgentFocusIn += this.HandleSpectateAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut += this.HandleSpectateAgentFocusOut;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
		public override void AfterStart()
		{
			if (this.PlayerOrderController != null)
			{
				this.PlayerOrderController.OnSelectedFormationsChanged += this.OrderController_OnSelectedFormationsChanged;
				base.Mission.PlayerTeam.OnFormationsChanged += this.PlayerTeam_OnFormationsChanged;
			}
			BannerBearerLogic missionBehavior = base.Mission.GetMissionBehavior<BannerBearerLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.OnBannerBearerAgentUpdated += this.BannerBearerLogic_OnBannerBearerAgentUpdated;
			}
			this.UpdateAlwaysShowFriendlyTroopBanners();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001EA34 File Offset: 0x0001CC34
		public override void OnMissionTick(float dt)
		{
			bool isOrderFlagVisible = this._isOrderFlagVisible;
			this.UpdateIsOrderFlagVisible();
			if (!this._isOrderFlagVisible && isOrderFlagVisible)
			{
				this.UpdateAllAgentMeshVisibilities();
				this.SetHighlightForAgents(false, false);
				this.SetHighlightForAgents(false, false);
			}
			if (this._isOrderFlagVisible && !isOrderFlagVisible)
			{
				this.UpdateAllAgentMeshVisibilities();
				this.SetHighlightForAgents(true, false);
				this.SetHighlightForAgents(true, false);
			}
			this.UpdateProximityBannerTransparencies();
			this.IndicatorsActive = this._alwaysShowFriendlyTroopBanners || base.Input.IsGameKeyDown(5);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001EAB8 File Offset: 0x0001CCB8
		private void UpdateProximityBannerTransparencies()
		{
			for (int i = 0; i < this._closeAgentsWithMeshes.Count; i++)
			{
				Agent agent = this._closeAgentsWithMeshes[i];
				this.SetBannerHighlightVisibility(agent, this.IsAgentListeningToOrders(agent));
			}
			this._closeAgentsWithMeshes.Clear();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(base.Mission, base.MissionScreen.CombatCamera.Position.AsVec2, 8f, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				if (this._agentMeshes.ContainsKey(proximityMapSearchStruct.LastFoundAgent))
				{
					this._closeAgentsWithMeshes.Add(proximityMapSearchStruct.LastFoundAgent);
				}
				AgentProximityMap.FindNext(base.Mission, ref proximityMapSearchStruct);
			}
			for (int j = 0; j < this._closeAgentsWithMeshes.Count; j++)
			{
				Agent agent2 = this._closeAgentsWithMeshes[j];
				this.SetBannerHighlightVisibility(agent2, this.IsAgentListeningToOrders(agent2));
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001EBA2 File Offset: 0x0001CDA2
		public override void OnRemoveBehavior()
		{
			this.UnregisterEvents();
			base.OnRemoveBehavior();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001EBB0 File Offset: 0x0001CDB0
		public override void OnMissionScreenFinalize()
		{
			this.UnregisterEvents();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001EBC0 File Offset: 0x0001CDC0
		private void UnregisterEvents()
		{
			if (base.Mission != null)
			{
				base.Mission.Teams.OnPlayerTeamChanged -= this.Mission_OnPlayerTeamChanged;
				base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (base.MissionScreen != null)
			{
				base.MissionScreen.OnSpectateAgentFocusIn -= this.HandleSpectateAgentFocusIn;
				base.MissionScreen.OnSpectateAgentFocusOut -= this.HandleSpectateAgentFocusOut;
			}
			if (this.PlayerOrderController != null)
			{
				this.PlayerOrderController.OnSelectedFormationsChanged -= this.OrderController_OnSelectedFormationsChanged;
				if (base.Mission != null)
				{
					base.Mission.PlayerTeam.OnFormationsChanged -= this.PlayerTeam_OnFormationsChanged;
				}
			}
			BannerBearerLogic missionBehavior = base.Mission.GetMissionBehavior<BannerBearerLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.OnBannerBearerAgentUpdated -= this.BannerBearerLogic_OnBannerBearerAgentUpdated;
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001ECC2 File Offset: 0x0001CEC2
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			this.RemoveAgentLabel(affectedAgent);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001ECCB File Offset: 0x0001CECB
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			this.InitAgentLabel(agent, banner);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001ECD5 File Offset: 0x0001CED5
		public override void OnAssignPlayerAsSergeantOfFormation(Agent agent)
		{
			this.SetBannerHighlightVisibility(agent, true);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001ECDF File Offset: 0x0001CEDF
		public override void OnClearScene()
		{
			this._agentMeshes.Clear();
			this._labelMaterials.Clear();
			this._closeAgentsWithMeshes.Clear();
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001ED02 File Offset: 0x0001CF02
		private void PlayerTeam_OnFormationsChanged(Team team, Formation formation)
		{
			this.UpdateIsOrderFlagVisible();
			if (this._isOrderFlagVisible)
			{
				this.DehighlightAllAgents();
				this.SetHighlightForAgents(true, false);
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001ED20 File Offset: 0x0001CF20
		private void Mission_OnPlayerTeamChanged(Team previousTeam, Team currentTeam)
		{
			this.DehighlightAllAgents();
			this._isOrderFlagVisible = false;
			if (((previousTeam != null) ? previousTeam.PlayerOrderController : null) != null)
			{
				previousTeam.PlayerOrderController.OnSelectedFormationsChanged -= this.OrderController_OnSelectedFormationsChanged;
			}
			if (this.PlayerOrderController != null)
			{
				this.PlayerOrderController.OnSelectedFormationsChanged += this.OrderController_OnSelectedFormationsChanged;
			}
			this.SetHighlightForAgents(true, true);
			foreach (Agent agent in base.Mission.Agents)
			{
				this.UpdateVisibilityOfAgentMesh(agent);
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001EDD4 File Offset: 0x0001CFD4
		private void OrderController_OnSelectedFormationsChanged()
		{
			this.UpdateAllAgentMeshVisibilities();
			this.DehighlightAllAgents();
			this.UpdateIsOrderFlagVisible();
			if (this._isOrderFlagVisible)
			{
				this.SetHighlightForAgents(true, false);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
		private void BannerBearerLogic_OnBannerBearerAgentUpdated(Agent agent, bool isBannerBearer)
		{
			this.RemoveAgentLabel(agent);
			this.InitAgentLabel(agent, null);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001EE0C File Offset: 0x0001D00C
		private void RemoveAgentLabel(Agent agent)
		{
			if (agent.IsHuman && this._agentMeshes.ContainsKey(agent))
			{
				if (agent.AgentVisuals != null)
				{
					agent.AgentVisuals.ReplaceMeshWithMesh(this._agentMeshes[agent], null, BodyMeshTypes.Label);
				}
				this._agentMeshes.Remove(agent);
			}
			if (this._closeAgentsWithMeshes.Contains(agent))
			{
				this._closeAgentsWithMeshes.Remove(agent);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001EE80 File Offset: 0x0001D080
		private void InitAgentLabel(Agent agent, Banner peerBanner = null)
		{
			if (agent.IsHuman)
			{
				Banner banner = peerBanner ?? agent.Origin.Banner;
				if (banner != null)
				{
					MetaMesh copy = MetaMesh.GetCopy("troop_banner_selection", false, true);
					Material tableauMaterial = Material.GetFromResource("agent_label_with_tableau");
					Banner banner2 = banner;
					BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
					Texture texture = banner2.GetTableauTextureSmall(in bannerDebugInfo, null);
					if (copy != null && tableauMaterial != null)
					{
						Texture fromResource = Texture.GetFromResource("banner_top_of_head");
						Material material;
						if (this._labelMaterials.TryGetValue(texture ?? fromResource, out material))
						{
							tableauMaterial = material;
						}
						else
						{
							tableauMaterial = tableauMaterial.CreateCopy();
							Action<Texture> action = delegate(Texture tex)
							{
								tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap, tex);
							};
							Banner banner3 = banner;
							bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
							texture = banner3.GetTableauTextureSmall(in bannerDebugInfo, action);
							tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, fromResource);
							this._labelMaterials.Add(texture, tableauMaterial);
						}
						copy.SetMaterial(tableauMaterial);
						copy.SetVectorArgument(0.5f, 0.5f, 0.25f, 0.25f);
						agent.AgentVisuals.AddMultiMesh(copy, BodyMeshTypes.Label);
						this._agentMeshes.Add(agent, copy);
						this.UpdateVisibilityOfAgentMesh(agent);
						this.SetBannerHighlightVisibility(agent, false);
					}
				}
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001EFE8 File Offset: 0x0001D1E8
		private void UpdateVisibilityOfAgentMesh(Agent agent)
		{
			if (agent.IsActive() && this._agentMeshes.ContainsKey(agent))
			{
				bool flag = this.IsMeshVisibleForAgent(agent);
				this._agentMeshes[agent].SetVisibilityMask(flag ? VisibilityMaskFlags.Final : ((VisibilityMaskFlags)0U));
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001F02C File Offset: 0x0001D22C
		private bool IsMeshVisibleForAgent(Agent agent)
		{
			return (this._isResumingView || (!base.IsViewSuspended && !this._isSuspendingView)) && this.IsAllyInAllyTeam(agent) && base.MissionScreen.LastFollowedAgent != agent && BannerlordConfig.FriendlyTroopsBannerOpacity > 0f && !base.MissionScreen.IsPhotoModeEnabled && (this.IndicatorsActive || base.Mission.Mode == MissionMode.Deployment || this.IsAgentListeningToOrders(agent));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001F0A2 File Offset: 0x0001D2A2
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			base.OnMissionModeChange(oldMissionMode, atStart);
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001F0B2 File Offset: 0x0001D2B2
		private void OnUpdateOpacityValueOfAgentMesh(Agent agent)
		{
			if (agent.IsActive() && this._agentMeshes.ContainsKey(agent))
			{
				this.SetBannerHighlightVisibility(agent, this.IsAgentListeningToOrders(agent));
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001F0D8 File Offset: 0x0001D2D8
		private bool IsAllyInAllyTeam(Agent agent)
		{
			if (((agent != null) ? agent.Team : null) != null && base.Mission != null && agent != base.Mission.MainAgent)
			{
				Team team = null;
				Team team3;
				if (GameNetwork.IsSessionActive)
				{
					Team team2;
					if (!GameNetwork.IsMyPeerReady)
					{
						team2 = null;
					}
					else
					{
						NetworkCommunicator myPeer = GameNetwork.MyPeer;
						if (myPeer == null)
						{
							team2 = null;
						}
						else
						{
							MissionPeer component = myPeer.GetComponent<MissionPeer>();
							team2 = ((component != null) ? component.Team : null);
						}
					}
					team3 = team2;
				}
				else
				{
					team3 = base.Mission.PlayerTeam;
					team = base.Mission.PlayerAllyTeam;
				}
				return agent.Team == team3 || agent.Team == team;
			}
			return false;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001F16A File Offset: 0x0001D36A
		private void OnMainAgentChanged(Agent oldAgent)
		{
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001F172 File Offset: 0x0001D372
		private void HandleSpectateAgentFocusIn(Agent agent)
		{
			this.UpdateVisibilityOfAgentMesh(agent);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001F17B File Offset: 0x0001D37B
		private void HandleSpectateAgentFocusOut(Agent agent)
		{
			this.UpdateVisibilityOfAgentMesh(agent);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001F184 File Offset: 0x0001D384
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType == ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType)
			{
				this.UpdateAlwaysShowFriendlyTroopBanners();
				this.UpdateAllAgentMeshVisibilities();
			}
			if (optionType == ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity || optionType == ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType)
			{
				this.UpdateAllAgentMeshVisibilities();
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001F1A8 File Offset: 0x0001D3A8
		private void UpdateAlwaysShowFriendlyTroopBanners()
		{
			float config = ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType);
			this._alwaysShowFriendlyTroopBanners = config == 2f || (config == 1f && GameNetwork.IsMultiplayer);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
		private void UpdateAllAgentMeshVisibilities()
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman)
				{
					this.UpdateVisibilityOfAgentMesh(agent);
					if (this.IsMeshVisibleForAgent(agent))
					{
						this.OnUpdateOpacityValueOfAgentMesh(agent);
					}
				}
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001F250 File Offset: 0x0001D450
		private bool IsAgentListeningToOrders(Agent agent)
		{
			this.UpdateIsOrderFlagVisible();
			return this._isOrderFlagVisible && (this.PlayerOrderController != null && agent.Formation != null && this.PlayerOrderController.IsFormationListening(agent.Formation));
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001F288 File Offset: 0x0001D488
		private void SetBannerHighlightVisibility(Agent agent, bool highlightVisibility)
		{
			MetaMesh metaMesh;
			if (!this._agentMeshes.TryGetValue(agent, out metaMesh))
			{
				Debug.FailedAssert("Trying to update the banner of an agent that isn't present in _agentMeshes!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\MissionViews\\MissionAgentLabelView.cs", "SetBannerHighlightVisibility", 472);
				return;
			}
			float num = (highlightVisibility ? 1f : (-1f));
			float num2 = (agent.Position + this._meshOffset).Distance(base.MissionScreen.CombatCamera.Position);
			if (num2 < 1.5f)
			{
				num = 0f;
			}
			else if (num2 < 8f)
			{
				num *= (num2 - 1.5f) / 6.5f;
			}
			metaMesh.SetVectorArgument2(20f, 0.4f, 0.44f, num * BannerlordConfig.FriendlyTroopsBannerOpacity);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001F33D File Offset: 0x0001D53D
		private void UpdateIsOrderFlagVisible()
		{
			this._isOrderFlagVisible = this.PlayerOrderController != null && base.MissionScreen.OrderFlag != null && base.MissionScreen.OrderFlag.IsVisible;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001F370 File Offset: 0x0001D570
		private void SetHighlightForAgents(bool highlight, bool useAllTeamAgents)
		{
			if (this.PlayerOrderController == null)
			{
				bool flag = base.Mission.PlayerTeam == null;
				Debug.Print(string.Format("PlayerOrderController is null and playerTeamIsNull: {0}", flag), 0, Debug.DebugColor.White, 17179869184UL);
			}
			if (useAllTeamAgents)
			{
				if (this.PlayerOrderController.Owner != null)
				{
					Team team = this.PlayerOrderController.Owner.Team;
					if (team == null)
					{
						Debug.Print("PlayerOrderController.Owner.Team is null, overriding with Mission.Current.PlayerTeam", 0, Debug.DebugColor.White, 17179869184UL);
						team = Mission.Current.PlayerTeam;
					}
					using (List<Agent>.Enumerator enumerator = team.ActiveAgents.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Agent agent2 = enumerator.Current;
							this.SetBannerHighlightVisibility(agent2, highlight);
						}
						return;
					}
				}
				Debug.Print("PlayerOrderController.Owner is null", 0, Debug.DebugColor.White, 17179869184UL);
				return;
			}
			Action<Agent> <>9__0;
			foreach (Formation formation in this.PlayerOrderController.SelectedFormations)
			{
				Action<Agent> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(Agent agent)
					{
						this.SetBannerHighlightVisibility(agent, highlight);
					});
				}
				formation.ApplyActionOnEachUnit(action, null);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001F4E0 File Offset: 0x0001D6E0
		private void DehighlightAllAgents()
		{
			foreach (KeyValuePair<Agent, MetaMesh> keyValuePair in this._agentMeshes)
			{
				this.SetBannerHighlightVisibility(keyValuePair.Key, false);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001F53C File Offset: 0x0001D73C
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			this.UpdateVisibilityOfAgentMesh(agent);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001F545 File Offset: 0x0001D745
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001F553 File Offset: 0x0001D753
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001F561 File Offset: 0x0001D761
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
			this._isSuspendingView = true;
			this.UpdateAllAgentMeshVisibilities();
			this._isSuspendingView = false;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001F57D File Offset: 0x0001D77D
		protected override void OnResumeView()
		{
			base.OnResumeView();
			this._isResumingView = true;
			this.UpdateAllAgentMeshVisibilities();
			this._isResumingView = false;
		}

		// Token: 0x04000268 RID: 616
		private const float _highlightedLabelScaleFactor = 20f;

		// Token: 0x04000269 RID: 617
		private const float _labelBannerWidth = 0.4f;

		// Token: 0x0400026A RID: 618
		private const float _labelBlackBorderWidth = 0.44f;

		// Token: 0x0400026B RID: 619
		private readonly Vec3 _meshOffset = new Vec3(0f, 0f, 2f, -1f);

		// Token: 0x0400026C RID: 620
		private const float _nearDistance = 1.5f;

		// Token: 0x0400026D RID: 621
		private const float _farDistance = 8f;

		// Token: 0x0400026E RID: 622
		private readonly List<Agent> _closeAgentsWithMeshes;

		// Token: 0x0400026F RID: 623
		private readonly Dictionary<Agent, MetaMesh> _agentMeshes;

		// Token: 0x04000270 RID: 624
		private readonly Dictionary<Texture, Material> _labelMaterials;

		// Token: 0x04000271 RID: 625
		private bool _isSuspendingView;

		// Token: 0x04000272 RID: 626
		private bool _isResumingView;

		// Token: 0x04000273 RID: 627
		private bool _isOrderFlagVisible;

		// Token: 0x04000274 RID: 628
		private bool _alwaysShowFriendlyTroopBanners;

		// Token: 0x04000275 RID: 629
		private bool _indicatorsActive;
	}
}
