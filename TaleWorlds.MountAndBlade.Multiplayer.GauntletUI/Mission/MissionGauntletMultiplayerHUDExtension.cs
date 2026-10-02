using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000014 RID: 20
	[OverrideView(typeof(MissionMultiplayerHUDExtensionUIHandler))]
	public class MissionGauntletMultiplayerHUDExtension : MissionView
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x000060E3 File Offset: 0x000042E3
		public MissionGauntletMultiplayerHUDExtension()
		{
			this.ViewOrderPriority = 2;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000060F4 File Offset: 0x000042F4
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._mpMissionCategory = UIResourceManager.LoadSpriteCategory("ui_mpmission");
			this._dataSource = new MissionMultiplayerHUDExtensionVM(base.Mission);
			this._gauntletLayer = new GauntletLayer("HUDExtension", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("HUDExtension", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.MissionScreen.OnSpectateAgentFocusIn += this._dataSource.OnSpectatedAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut += this._dataSource.OnSpectatedAgentFocusOut;
			this._dataSource.OnPlayerFollowRequested += this.OnPlayerFollowRequested;
			this._dataSource.SpectatorControls.OnCycleTargetRequested += this.OnCycleTargetRequested;
			Game.Current.EventManager.RegisterEvent<MissionPlayerToggledOrderViewEvent>(new Action<MissionPlayerToggledOrderViewEvent>(this.OnMissionPlayerToggledOrderViewEvent));
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._lobbyComponent.OnPostMatchEnded += this.OnPostMatchEnded;
			GameKeyContext category = HotKeyManager.GetCategory("ScoreboardHotKeyCategory");
			if (!base.MissionScreen.SceneLayer.Input.IsCategoryRegistered(category))
			{
				base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006250 File Offset: 0x00004450
		public override void OnMissionScreenFinalize()
		{
			this._lobbyComponent.OnPostMatchEnded -= this.OnPostMatchEnded;
			base.MissionScreen.OnSpectateAgentFocusIn -= this._dataSource.OnSpectatedAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut -= this._dataSource.OnSpectatedAgentFocusOut;
			this._dataSource.OnPlayerFollowRequested -= this.OnPlayerFollowRequested;
			this._dataSource.SpectatorControls.OnCycleTargetRequested -= this.OnCycleTargetRequested;
			this.SetAllSpectatorLayersVisible(true);
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			SpriteCategory mpMissionCategory = this._mpMissionCategory;
			if (mpMissionCategory != null)
			{
				mpMissionCategory.Unload();
			}
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._gauntletLayer = null;
			Game.Current.EventManager.UnregisterEvent<MissionPlayerToggledOrderViewEvent>(new Action<MissionPlayerToggledOrderViewEvent>(this.OnMissionPlayerToggledOrderViewEvent));
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006344 File Offset: 0x00004544
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.Tick(dt);
			this.UpdateOverlayInputClaim();
			if (MultiplayerSpectatorHelper.IsLocalPeerSpectator() && base.MissionScreen.SceneLayer.Input.IsHotKeyPressed("ToggleHud"))
			{
				this._dataSource.ShowHud = !this._dataSource.ShowHud;
				this.SetAllSpectatorLayersVisible(this._dataSource.ShowHud);
				if (!this._dataSource.ShowHud)
				{
					TextObject textObject = new TextObject("{=RsT4BS5O}Spectator UI hidden. Press {KEY} to show it again.", null);
					textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("ScoreboardHotKeyCategory", "ToggleHud"), 1f));
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006404 File Offset: 0x00004604
		private void UpdateOverlayInputClaim()
		{
			if (this._gauntletLayer == null)
			{
				return;
			}
			bool flag = this._dataSource != null && MultiplayerSpectatorHelper.IsLocalPeerSpectator();
			bool flag2 = this._gauntletLayer.InputRestrictions.InputUsageMask == InputUsageMask.Mouse;
			if (flag != flag2)
			{
				if (flag)
				{
					this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Mouse);
				}
				else
				{
					this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				}
			}
			if (flag && ScreenManager.FocusedLayer == this._gauntletLayer && !this._gauntletLayer.IsFocusedOnInput())
			{
				ScreenManager.TryLoseFocus(this._gauntletLayer);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006494 File Offset: 0x00004694
		private void SetAllSpectatorLayersVisible(bool isVisible)
		{
			using (List<ScreenLayer>.Enumerator enumerator = base.MissionScreen.Layers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GauntletLayer gauntletLayer;
					if ((gauntletLayer = enumerator.Current as GauntletLayer) != null)
					{
						foreach (string text in MissionGauntletMultiplayerHUDExtension._spectatorToggleLayerNames)
						{
							if (gauntletLayer.Name == text)
							{
								ScreenManager.SetSuspendLayer(gauntletLayer, !isVisible);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006524 File Offset: 0x00004724
		private void OnCycleTargetRequested(int direction)
		{
			base.MissionScreen.RequestSpectatorCycle(direction);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006532 File Offset: 0x00004732
		private void OnPlayerFollowRequested(Agent agent)
		{
			if (agent == null || !agent.IsCameraAttachable())
			{
				return;
			}
			base.MissionScreen.SetAgentToFollow(agent);
			base.MissionScreen.SuppressSpectatorCyclingThisFrame();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006557 File Offset: 0x00004757
		private void OnMissionPlayerToggledOrderViewEvent(MissionPlayerToggledOrderViewEvent eventObj)
		{
			this._dataSource.IsOrderActive = eventObj.IsOrderEnabled;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000656A File Offset: 0x0000476A
		private void OnPostMatchEnded()
		{
			this._dataSource.ShowHud = false;
			this.SetAllSpectatorLayersVisible(true);
		}

		// Token: 0x04000062 RID: 98
		private static readonly string[] _spectatorToggleLayerNames = new string[] { "MultiplayerKillFeed", "MPMissionMarkers", "MultiplayerScoreboard", "HUDExtension" };

		// Token: 0x04000063 RID: 99
		private MissionMultiplayerHUDExtensionVM _dataSource;

		// Token: 0x04000064 RID: 100
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000065 RID: 101
		private SpriteCategory _mpMissionCategory;

		// Token: 0x04000066 RID: 102
		private MissionLobbyComponent _lobbyComponent;
	}
}
