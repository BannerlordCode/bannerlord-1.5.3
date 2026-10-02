using System;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000044 RID: 68
	[OverrideView(typeof(MarriageOfferPopupView))]
	public class GauntletMarriageOfferPopupView : MapView
	{
		// Token: 0x06000326 RID: 806 RVA: 0x00012928 File Offset: 0x00010B28
		public GauntletMarriageOfferPopupView(Hero suitor, Hero maiden)
		{
			this._suitor = suitor;
			this._maiden = maiden;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00012940 File Offset: 0x00010B40
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new MarriageOfferPopupVM(this._suitor, this._maiden, new Action(this.OnPopupClosed));
			this.InitializeKeyVisuals();
			base.Layer = new GauntletLayer("MapMarriageOffer", 203, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			this._movie = this._layerAsGauntletLayer.LoadMovie("MarriageOfferPopup", this._dataSource);
			base.MapScreen.AddLayer(base.Layer);
			base.MapScreen.SetIsMarriageOfferPopupActive(true);
			this._previousTimeControlMode = Campaign.Current.TimeControlMode;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.SetTimeControlModeLock(true);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00012A5F File Offset: 0x00010C5F
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.HandleInput();
			MarriageOfferPopupVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00012A7E File Offset: 0x00010C7E
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			this.HandleInput();
			MarriageOfferPopupVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00012A9D File Offset: 0x00010C9D
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			this.HandleInput();
			MarriageOfferPopupVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00012ABC File Offset: 0x00010CBC
		protected override void OnFinalize()
		{
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			base.MapScreen.RemoveLayer(base.Layer);
			this._movie = null;
			this._dataSource = null;
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			base.MapScreen.SetIsMarriageOfferPopupActive(false);
			Campaign.Current.SetTimeControlModeLock(false);
			Campaign.Current.TimeControlMode = this._previousTimeControlMode;
			base.OnFinalize();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00012B34 File Offset: 0x00010D34
		protected override bool IsEscaped()
		{
			MarriageOfferPopupVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.ExecuteDeclineOffer();
			}
			return true;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00012B48 File Offset: 0x00010D48
		protected override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return false;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00012B4B File Offset: 0x00010D4B
		private void OnPopupClosed()
		{
			base.MapScreen.CloseMarriageOfferPopup();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00012B58 File Offset: 0x00010D58
		private void HandleInput()
		{
			if (this._dataSource != null)
			{
				if (base.Layer.Input.IsGameKeyPressed(39))
				{
					base.MapScreen.OpenEncyclopedia();
					return;
				}
				if (base.Layer.Input.IsHotKeyReleased("Confirm"))
				{
					UISoundsHelper.PlayUISound("event:/ui/panels/next");
					this._dataSource.ExecuteAcceptOffer();
					return;
				}
				if (base.Layer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/panels/next");
					this._dataSource.ExecuteDeclineOffer();
				}
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00012BE6 File Offset: 0x00010DE6
		private void InitializeKeyVisuals()
		{
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
		}

		// Token: 0x0400012D RID: 301
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x0400012E RID: 302
		private MarriageOfferPopupVM _dataSource;

		// Token: 0x0400012F RID: 303
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000130 RID: 304
		private CampaignTimeControlMode _previousTimeControlMode;

		// Token: 0x04000131 RID: 305
		private Hero _suitor;

		// Token: 0x04000132 RID: 306
		private Hero _maiden;
	}
}
