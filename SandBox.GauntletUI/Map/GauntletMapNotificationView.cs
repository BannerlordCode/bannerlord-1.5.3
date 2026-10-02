using System;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003B RID: 59
	[OverrideView(typeof(MapNotificationView))]
	public class GauntletMapNotificationView : MapNotificationView
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x00010BBC File Offset: 0x0000EDBC
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._mapNavigationHandler = base.MapScreen.NavigationHandler;
			this._dataSource = new MapNotificationVM(this._mapNavigationHandler, new Action<CampaignVec2>(base.MapScreen.FastMoveCameraToPosition));
			this._dataSource.ReceiveNewNotification += this.OnReceiveNewNotification;
			this._dataSource.SetRemoveInputKey(HotKeyManager.GetCategory("MapNotificationHotKeyCategory").GetHotKey("RemoveNotification"));
			base.Layer = new GauntletLayer("MapNotification", 100, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MapNotificationHotKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapNotificationUI", this._dataSource);
			base.MapScreen.AddLayer(base.Layer);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00010CB5 File Offset: 0x0000EEB5
		private void OnReceiveNewNotification(MapNotificationItemBaseVM newNotification)
		{
			if (!string.IsNullOrEmpty(newNotification.SoundId))
			{
				SoundEvent.PlaySound2D(newNotification.SoundId);
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00010CD0 File Offset: 0x0000EED0
		public override void RegisterMapNotificationType(Type data, Type item)
		{
			this._dataSource.RegisterMapNotificationType(data, item);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00010CDF File Offset: 0x0000EEDF
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._dataSource.OnFinalize();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00010CF2 File Offset: 0x0000EEF2
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._dataSource.OnFrameTick(dt);
			this.HandleInput();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00010D0D File Offset: 0x0000EF0D
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			this._dataSource.OnMenuModeTick(dt);
			this.HandleInput();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00010D28 File Offset: 0x0000EF28
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00010D44 File Offset: 0x0000EF44
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00010D60 File Offset: 0x0000EF60
		private void HandleInput()
		{
			if (!this._isHoveringOnNotification && this._dataSource.FocusedNotificationItem != null)
			{
				this._isHoveringOnNotification = true;
				base.Layer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(base.Layer);
			}
			else if (this._isHoveringOnNotification && this._dataSource.FocusedNotificationItem == null)
			{
				this._isHoveringOnNotification = false;
				base.Layer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(base.Layer);
			}
			if (this._isHoveringOnNotification && this._dataSource.FocusedNotificationItem != null && base.Layer.Input.IsHotKeyReleased("RemoveNotification") && !this._dataSource.FocusedNotificationItem.ForceInspection)
			{
				SoundEvent.PlaySound2D("event:/ui/default");
				this._dataSource.FocusedNotificationItem.ExecuteRemove();
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00010E2E File Offset: 0x0000F02E
		public override void ResetNotifications()
		{
			base.ResetNotifications();
			MapNotificationVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.RemoveAllNotifications();
		}

		// Token: 0x04000103 RID: 259
		private MapNotificationVM _dataSource;

		// Token: 0x04000104 RID: 260
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000105 RID: 261
		private INavigationHandler _mapNavigationHandler;

		// Token: 0x04000106 RID: 262
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000107 RID: 263
		private bool _isHoveringOnNotification;

		// Token: 0x04000108 RID: 264
		private const string _defaultSound = "event:/ui/default";
	}
}
