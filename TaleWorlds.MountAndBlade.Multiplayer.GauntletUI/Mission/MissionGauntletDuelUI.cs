using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000E RID: 14
	[OverrideView(typeof(MissionMultiplayerDuelUI))]
	public class MissionGauntletDuelUI : MissionView
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x000051F0 File Offset: 0x000033F0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 15;
			this._client = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeDuelClient>();
			this._dataSource = new MultiplayerDuelVM(base.MissionScreen.CombatCamera, this._client);
			this._gauntletLayer = new GauntletLayer("MultiplayerDuel", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerDuel", this._dataSource);
			this._mpMissionCategory = UIResourceManager.LoadSpriteCategory("ui_mpmission");
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._equipmentController = base.Mission.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this._equipmentController.OnEquipmentRefreshed += this.OnEquipmentRefreshed;
			MissionPeer.OnEquipmentIndexRefreshed += this.OnPeerEquipmentIndexRefreshed;
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._lobbyComponent.OnPostMatchEnded += this.OnPostMatchEnded;
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Combine(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			this._dataSource.IsEnabled = true;
			this._isPeerEquipmentsDirty = true;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000531C File Offset: 0x0000351C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			SpriteCategory mpMissionCategory = this._mpMissionCategory;
			if (mpMissionCategory != null)
			{
				mpMissionCategory.Unload();
			}
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._gauntletLayer = null;
			this._equipmentController.OnEquipmentRefreshed -= this.OnEquipmentRefreshed;
			MissionPeer.OnEquipmentIndexRefreshed -= this.OnPeerEquipmentIndexRefreshed;
			this._lobbyComponent.OnPostMatchEnded -= this.OnPostMatchEnded;
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Remove(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000053CC File Offset: 0x000035CC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.Tick(dt);
			DuelMissionRepresentative myRepresentative = this._client.MyRepresentative;
			if (((myRepresentative != null) ? myRepresentative.ControlledAgent : null) != null && base.Input.IsGameKeyReleased(13))
			{
				this._client.MyRepresentative.OnInteraction();
			}
			if (this._isPeerEquipmentsDirty)
			{
				this._dataSource.Markers.RefreshPeerEquipments();
				this._isPeerEquipmentsDirty = false;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005443 File Offset: 0x00003643
		private void OnNativeOptionChanged(NativeOptions.NativeOptionsType optionType)
		{
			if (optionType == NativeOptions.NativeOptionsType.ScreenResolution)
			{
				this._dataSource.OnScreenResolutionChanged();
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005455 File Offset: 0x00003655
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			if (affectedAgent == Agent.Main)
			{
				this._dataSource.OnMainAgentRemoved();
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005475 File Offset: 0x00003675
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			base.OnAgentBuild(agent, banner);
			if (agent == Agent.Main)
			{
				this._dataSource.OnMainAgentBuild();
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005492 File Offset: 0x00003692
		public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(agent, focusableObject, isInteractable);
			if (!(focusableObject is DuelZoneLandmark) && !(focusableObject is Agent))
			{
				this._dataSource.Markers.OnFocusGained();
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000054BD File Offset: 0x000036BD
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			if (!(focusableObject is DuelZoneLandmark) && !(focusableObject is Agent))
			{
				this._dataSource.Markers.OnFocusLost();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000054E7 File Offset: 0x000036E7
		public void OnPeerEquipmentIndexRefreshed(MissionPeer peer, int equipmentSetIndex)
		{
			this._dataSource.Markers.OnPeerEquipmentRefreshed(peer);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000054FA File Offset: 0x000036FA
		private void OnEquipmentRefreshed(MissionPeer peer)
		{
			this._dataSource.Markers.OnPeerEquipmentRefreshed(peer);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000550D File Offset: 0x0000370D
		private void OnPostMatchEnded()
		{
			this._dataSource.IsEnabled = false;
		}

		// Token: 0x04000042 RID: 66
		private MultiplayerDuelVM _dataSource;

		// Token: 0x04000043 RID: 67
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000044 RID: 68
		private SpriteCategory _mpMissionCategory;

		// Token: 0x04000045 RID: 69
		private MissionMultiplayerGameModeDuelClient _client;

		// Token: 0x04000046 RID: 70
		private MissionLobbyEquipmentNetworkComponent _equipmentController;

		// Token: 0x04000047 RID: 71
		private MissionLobbyComponent _lobbyComponent;

		// Token: 0x04000048 RID: 72
		private bool _isPeerEquipmentsDirty;
	}
}
