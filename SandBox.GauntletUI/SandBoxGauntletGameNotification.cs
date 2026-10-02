using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI
{
	// Token: 0x02000012 RID: 18
	public class SandBoxGauntletGameNotification : GauntletGameNotification
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00007BFC File Offset: 0x00005DFC
		public new static void Initialize()
		{
			GauntletGameNotification gauntletGameNotification = GauntletGameNotification.Current;
			if (gauntletGameNotification != null)
			{
				gauntletGameNotification.OnFinalize();
			}
			GauntletGameNotification.Current = new SandBoxGauntletGameNotification();
			ScreenManager.AddGlobalLayer(GauntletGameNotification.Current, false);
			GauntletGameNotification.Current.RegisterEvents();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007C30 File Offset: 0x00005E30
		protected override void OnReceiveNewNotification(GameNotificationItemVM notification)
		{
			base.OnReceiveNewNotification(notification);
			SoundEvent currentNotificationSoundEvent = this._currentNotificationSoundEvent;
			if (currentNotificationSoundEvent != null)
			{
				currentNotificationSoundEvent.Release();
			}
			this._currentNotificationSoundEvent = null;
			if (notification != null && notification.IsDialog)
			{
				this._currentNotificationSoundEvent = SoundEvent.CreateEventFromExternalFile("event:/Extra/voiceover", notification.DialogSoundPath, null, false, false);
				SoundEvent currentNotificationSoundEvent2 = this._currentNotificationSoundEvent;
				if (currentNotificationSoundEvent2 == null)
				{
					return;
				}
				currentNotificationSoundEvent2.Play();
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007C91 File Offset: 0x00005E91
		public override void OnFinalize()
		{
			base.OnFinalize();
			SoundEvent currentNotificationSoundEvent = this._currentNotificationSoundEvent;
			if (currentNotificationSoundEvent != null)
			{
				currentNotificationSoundEvent.Release();
			}
			this._currentNotificationSoundEvent = null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007CB4 File Offset: 0x00005EB4
		public override void RegisterEvents()
		{
			base.RegisterEvents();
			CampaignInformationManager.OnDisplayDialog += this._dataSource.AddDialogNotification;
			CampaignInformationManager.OnGetStatusOfDialogNotification += this._dataSource.GetStatusOfDialogNotification;
			CampaignInformationManager.OnClearDialogNotification += this._dataSource.ClearDialogNotification;
			CampaignInformationManager.IsAnyDialogNotificationActiveOrQueued += this._dataSource.GetIsAnyDialogNotificationActiveOrQueued;
			CampaignInformationManager.OnClearAllDialogNotifications += this._dataSource.ClearAllDialogNotifications;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00007D38 File Offset: 0x00005F38
		public override void UnregisterEvents()
		{
			base.UnregisterEvents();
			CampaignInformationManager.OnDisplayDialog -= this._dataSource.AddDialogNotification;
			CampaignInformationManager.OnGetStatusOfDialogNotification -= this._dataSource.GetStatusOfDialogNotification;
			CampaignInformationManager.OnClearDialogNotification -= this._dataSource.ClearDialogNotification;
			CampaignInformationManager.IsAnyDialogNotificationActiveOrQueued -= this._dataSource.GetIsAnyDialogNotificationActiveOrQueued;
			CampaignInformationManager.OnClearAllDialogNotifications -= this._dataSource.ClearAllDialogNotifications;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00007DB9 File Offset: 0x00005FB9
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.TickSoundEvent();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007DC8 File Offset: 0x00005FC8
		private void TickSoundEvent()
		{
			if (this._currentNotificationSoundEvent != null)
			{
				if (this._dataSource.GotNotification && this._dataSource.CurrentNotification.IsDialog)
				{
					if (!this._currentNotificationSoundEvent.IsValid || this._currentNotificationSoundEvent.IsStopped())
					{
						this._currentNotificationSoundEvent.Release();
						this._currentNotificationSoundEvent = null;
						this._dataSource.FadeOutCurrentNotification(true);
						return;
					}
					if (this._dataSource.IsPaused && this._currentNotificationSoundEvent.IsPlaying())
					{
						this._currentNotificationSoundEvent.Pause();
						return;
					}
					if (!this._dataSource.IsPaused && this._currentNotificationSoundEvent.IsPaused())
					{
						this._currentNotificationSoundEvent.Resume();
						return;
					}
				}
				else
				{
					this._currentNotificationSoundEvent.Release();
					this._currentNotificationSoundEvent = null;
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007E9C File Offset: 0x0000609C
		protected override bool GetShouldBeSuspended()
		{
			bool flag = base.GetShouldBeSuspended();
			if (this._dataSource.GotNotification && this._dataSource.CurrentNotification.IsDialog)
			{
				bool flag2;
				if (!flag && !MBCommon.IsPaused)
				{
					GameStateManager gameStateManager = GameStateManager.Current;
					flag2 = gameStateManager != null && gameStateManager.ActiveStateDisabledByUser;
				}
				else
				{
					flag2 = true;
				}
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x0400005A RID: 90
		private SoundEvent _currentNotificationSoundEvent;
	}
}
