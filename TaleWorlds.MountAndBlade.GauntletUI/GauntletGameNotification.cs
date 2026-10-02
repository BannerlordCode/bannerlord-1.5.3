using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000F RID: 15
	public class GauntletGameNotification : GlobalLayer
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00004C0E File Offset: 0x00002E0E
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00004C15 File Offset: 0x00002E15
		protected static GauntletGameNotification Current { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004C1D File Offset: 0x00002E1D
		protected virtual string MovieName
		{
			get
			{
				return "GameNotificationUI";
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004C24 File Offset: 0x00002E24
		protected GauntletGameNotification()
		{
			this._dataSource = new GameNotificationVM();
			this._dataSource.CurrentNotificationChanged += this.OnReceiveNewNotification;
			this._layer = new GauntletLayer("GameNotification", 19200, false);
			this._layer.LoadMovie(this.MovieName, this._dataSource);
			base.Layer = this._layer;
			this._layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Mouse);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004CA6 File Offset: 0x00002EA6
		protected virtual void OnReceiveNewNotification(GameNotificationItemVM notification)
		{
			if (!string.IsNullOrEmpty((notification != null) ? notification.NotificationSoundId : null))
			{
				SoundEvent.PlaySound2D(notification.NotificationSoundId);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004CC7 File Offset: 0x00002EC7
		public static void Initialize()
		{
			GauntletGameNotification gauntletGameNotification = GauntletGameNotification.Current;
			if (gauntletGameNotification != null)
			{
				gauntletGameNotification.OnFinalize();
			}
			GauntletGameNotification.Current = new GauntletGameNotification();
			ScreenManager.AddGlobalLayer(GauntletGameNotification.Current, false);
			GauntletGameNotification.Current.RegisterEvents();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public virtual void OnFinalize()
		{
			GameNotificationVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.ClearNotifications();
			}
			this.UnregisterEvents();
			ScreenManager.RemoveGlobalLayer(this, true);
			this._dataSource = null;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004D1F File Offset: 0x00002F1F
		public virtual void RegisterEvents()
		{
			MBInformationManager.FiringQuickInformation += this._dataSource.AddGameNotification;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004D37 File Offset: 0x00002F37
		public virtual void UnregisterEvents()
		{
			MBInformationManager.FiringQuickInformation -= this._dataSource.AddGameNotification;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004D50 File Offset: 0x00002F50
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			bool shouldBeSuspended = this.GetShouldBeSuspended();
			if (shouldBeSuspended != this._isSuspended)
			{
				ScreenManager.SetSuspendLayer(GauntletGameNotification.Current._layer, shouldBeSuspended);
				this._isSuspended = shouldBeSuspended;
			}
			this._dataSource.IsPaused = this._isSuspended;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004D9C File Offset: 0x00002F9C
		protected virtual bool GetShouldBeSuspended()
		{
			return GauntletSceneNotification.Current.IsActive || LoadingWindow.IsLoadingWindowActive;
		}

		// Token: 0x04000054 RID: 84
		protected GameNotificationVM _dataSource;

		// Token: 0x04000055 RID: 85
		private readonly GauntletLayer _layer;

		// Token: 0x04000057 RID: 87
		private bool _isSuspended;
	}
}
