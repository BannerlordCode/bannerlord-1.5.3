using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Barter;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001B RID: 27
	[OverrideView(typeof(BarterView))]
	public class MissionGauntletBarterView : MissionView
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		public override void AfterStart()
		{
			base.AfterStart();
			this._barter = Campaign.Current.BarterManager;
			BarterManager barter = this._barter;
			barter.BarterBegin = (BarterManager.BarterBeginEventDelegate)Delegate.Combine(barter.BarterBegin, new BarterManager.BarterBeginEventDelegate(this.OnBarterBegin));
			BarterManager barter2 = this._barter;
			barter2.Closed = (BarterManager.BarterCloseEventDelegate)Delegate.Combine(barter2.Closed, new BarterManager.BarterCloseEventDelegate(this.OnBarterClosed));
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A92C File Offset: 0x00008B2C
		private void OnBarterBegin(BarterData args)
		{
			this._barterCategory = UIResourceManager.LoadSpriteCategory("ui_barter");
			this._dataSource = new BarterVM(args);
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._gauntletLayer = new GauntletLayer("MissionBarter", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("BarterScreen", this._dataSource);
			GameKeyContext category = HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory");
			this._gauntletLayer.Input.RegisterHotKeyCategory(category);
			GameKeyContext category2 = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
			this._gauntletLayer.Input.RegisterHotKeyCategory(category2);
			this._gauntletLayer.IsFocusLayer = true;
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.MissionScreen.SetLayerCategoriesStateAndDeactivateOthers(new string[] { "SceneLayer", "MissionBarter" }, true);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000AA74 File Offset: 0x00008C74
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			BarterItemVM.IsFiveStackModifierActive = this.IsDownInGauntletLayer("FiveStackModifier");
			BarterItemVM.IsEntireStackModifierActive = this.IsDownInGauntletLayer("EntireStackModifier");
			if (this.IsReleasedInGauntletLayer("Confirm"))
			{
				if (!this._dataSource.IsOfferDisabled)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteOffer();
					return;
				}
			}
			else
			{
				if (this.IsReleasedInGauntletLayer("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteCancel();
					return;
				}
				if (this.IsReleasedInGauntletLayer("Reset"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteReset();
				}
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000AB1D File Offset: 0x00008D1D
		private bool IsDownInGauntletLayer(string hotKeyID)
		{
			GauntletLayer gauntletLayer = this._gauntletLayer;
			return gauntletLayer != null && gauntletLayer.Input.IsHotKeyDown(hotKeyID);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000AB36 File Offset: 0x00008D36
		private bool IsReleasedInGauntletLayer(string hotKeyID)
		{
			GauntletLayer gauntletLayer = this._gauntletLayer;
			return gauntletLayer != null && gauntletLayer.Input.IsHotKeyReleased(hotKeyID);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000AB50 File Offset: 0x00008D50
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			BarterManager barter = this._barter;
			barter.BarterBegin = (BarterManager.BarterBeginEventDelegate)Delegate.Remove(barter.BarterBegin, new BarterManager.BarterBeginEventDelegate(this.OnBarterBegin));
			BarterManager barter2 = this._barter;
			barter2.Closed = (BarterManager.BarterCloseEventDelegate)Delegate.Remove(barter2.Closed, new BarterManager.BarterCloseEventDelegate(this.OnBarterClosed));
			this._gauntletLayer = null;
			BarterVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		private void OnBarterClosed()
		{
			base.MissionScreen.SetLayerCategoriesState(new string[] { "MissionBarter" }, false);
			base.MissionScreen.SetLayerCategoriesState(new string[] { "MissionConversation" }, true);
			base.MissionScreen.SetLayerCategoriesState(new string[] { "SceneLayer" }, true);
			BarterItemVM.IsFiveStackModifierActive = false;
			BarterItemVM.IsEntireStackModifierActive = false;
			this._barterCategory.Unload();
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource = null;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000AC88 File Offset: 0x00008E88
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000ACAD File Offset: 0x00008EAD
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000ACD2 File Offset: 0x00008ED2
		public override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return true;
		}

		// Token: 0x04000076 RID: 118
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000077 RID: 119
		private BarterVM _dataSource;

		// Token: 0x04000078 RID: 120
		private BarterManager _barter;

		// Token: 0x04000079 RID: 121
		private SpriteCategory _barterCategory;
	}
}
