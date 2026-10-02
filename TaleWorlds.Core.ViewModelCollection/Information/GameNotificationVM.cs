using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000014 RID: 20
	public class GameNotificationVM : ViewModel
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000FB RID: 251 RVA: 0x00003E3C File Offset: 0x0000203C
		// (remove) Token: 0x060000FC RID: 252 RVA: 0x00003E74 File Offset: 0x00002074
		public event Action<GameNotificationItemVM> CurrentNotificationChanged;

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00003EAC File Offset: 0x000020AC
		private float CurrentNotificationOnScreenTime
		{
			get
			{
				float num = 0.6f;
				num += (float)this.CurrentNotification.ExtraTimeInMs / 1000f;
				int numberOfWords = this.GetNumberOfWords(this.CurrentNotification.GameNotificationText);
				if (numberOfWords > 4)
				{
					num += (float)(numberOfWords - 4) / 5f;
				}
				if (this.CurrentNotification.IsDialog)
				{
					num += 10000f;
				}
				return num + 1f / (float)(this._items.Count + 1);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003F24 File Offset: 0x00002124
		public void FadeOutCurrentNotification(bool useExtraDisplayTime = false)
		{
			if (this.GotNotification)
			{
				this.MustFadeOutCurrentNotification = true;
				if (useExtraDisplayTime)
				{
					this.CurrentNotificationFadeOutDelayInSeconds = (float)this.CurrentNotification.ExtraTimeInMs / 1000f;
				}
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003F50 File Offset: 0x00002150
		public void SkipCurrentNotification()
		{
			if (this._items.Count > 0)
			{
				this.CurrentNotification = this._items[0];
				this._items.RemoveAt(0);
				this.CurrentNotificationDurationInSeconds = this.CurrentNotificationOnScreenTime;
				return;
			}
			this.GotNotification = false;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003FA0 File Offset: 0x000021A0
		public GameNotificationVM()
		{
			this._items = new List<GameNotificationItemVM>();
			this.CurrentNotification = new GameNotificationItemVM("NULL", 0, null, null, "NULL", 0, false, null);
			this.GotNotification = false;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003FE0 File Offset: 0x000021E0
		public void ClearNotifications()
		{
			this._items.Clear();
			this.GotNotification = false;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003FF4 File Offset: 0x000021F4
		public MBInformationManager.DialogNotificationHandle AddDialogNotification(TextObject text, int extraTimeInMs, BasicCharacterObject announcerCharacter, Equipment equipment, MBInformationManager.NotificationPriority priority, string dialogSoundPath)
		{
			GameNotificationItemVM gameNotificationItemVM = new GameNotificationItemVM(text.ToString(), extraTimeInMs, announcerCharacter, equipment, null, (int)priority, true, dialogSoundPath);
			if (this.GotNotification && this.CurrentNotification.GameNotificationText == text.ToString())
			{
				return this.CurrentNotification.Handle;
			}
			if (this._items.Any<GameNotificationItemVM>((GameNotificationItemVM i) => i.GameNotificationText == text.ToString()))
			{
				return this._items.First<GameNotificationItemVM>((GameNotificationItemVM i) => i.GameNotificationText == text.ToString()).Handle;
			}
			if (this.GotNotification && this.CurrentNotification.Priority >= (int)priority)
			{
				int num = this._items.FindLastIndex((GameNotificationItemVM i) => i.Priority >= (int)priority) + 1;
				this._items.Insert(num, gameNotificationItemVM);
			}
			else
			{
				this.CurrentNotification = gameNotificationItemVM;
				this.CurrentNotificationDurationInSeconds = this.CurrentNotificationOnScreenTime;
				this.GotNotification = true;
			}
			return gameNotificationItemVM.Handle;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004100 File Offset: 0x00002300
		public MBInformationManager.NotificationStatus GetStatusOfDialogNotification(MBInformationManager.DialogNotificationHandle handle)
		{
			if (handle == null)
			{
				return MBInformationManager.NotificationStatus.Inactive;
			}
			if (this.GotNotification && this.CurrentNotification.Handle == handle)
			{
				return MBInformationManager.NotificationStatus.CurrentlyActive;
			}
			if (this._items.Any<GameNotificationItemVM>((GameNotificationItemVM i) => i.Handle == handle))
			{
				return MBInformationManager.NotificationStatus.InQueue;
			}
			return MBInformationManager.NotificationStatus.Inactive;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004160 File Offset: 0x00002360
		public void ClearDialogNotification(MBInformationManager.DialogNotificationHandle handle, bool fadeOut)
		{
			if (handle == null)
			{
				return;
			}
			this._items.RemoveAll((GameNotificationItemVM x) => x.Handle == handle);
			if (this.GotNotification && this.CurrentNotification.Handle == handle)
			{
				if (fadeOut)
				{
					this.FadeOutCurrentNotification(false);
					return;
				}
				this.SkipCurrentNotification();
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000041C8 File Offset: 0x000023C8
		public bool GetIsAnyDialogNotificationActiveOrQueued()
		{
			if (!this.GotNotification || !this.CurrentNotification.IsDialog)
			{
				return this._items.Any<GameNotificationItemVM>((GameNotificationItemVM x) => x.IsDialog);
			}
			return true;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004218 File Offset: 0x00002418
		public void ClearAllDialogNotifications(bool fadeOut)
		{
			this._items.RemoveAll((GameNotificationItemVM x) => x.IsDialog);
			if (this.GotNotification && this.CurrentNotification.IsDialog)
			{
				if (fadeOut)
				{
					this.FadeOutCurrentNotification(false);
					return;
				}
				this.SkipCurrentNotification();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004278 File Offset: 0x00002478
		public void AddGameNotification(string notificationText, int extraTimeInMs, BasicCharacterObject announcerCharacter, Equipment equipment, string soundId)
		{
			if (string.IsNullOrEmpty(notificationText))
			{
				Debug.FailedAssert("Quick information message is empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core.ViewModelCollection\\Information\\GameNotificationVM.cs", "AddGameNotification", 183);
			}
			GameNotificationItemVM gameNotificationItemVM = new GameNotificationItemVM(notificationText, extraTimeInMs, announcerCharacter, equipment, soundId, 0, false, null);
			if ((!this.GotNotification || this.CurrentNotification.GameNotificationText != notificationText) && !this._items.Any<GameNotificationItemVM>((GameNotificationItemVM i) => i.GameNotificationText == notificationText))
			{
				if (this.GotNotification)
				{
					this._items.Add(gameNotificationItemVM);
					return;
				}
				this.CurrentNotification = gameNotificationItemVM;
				this.CurrentNotificationDurationInSeconds = this.CurrentNotificationOnScreenTime;
				this.GotNotification = true;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004338 File Offset: 0x00002538
		private int GetNumberOfWords(string text)
		{
			string text2 = text.Trim();
			int num = 0;
			int i = 0;
			while (i < text2.Length)
			{
				while (i < text2.Length && !char.IsWhiteSpace(text2[i]))
				{
					i++;
				}
				num++;
				while (i < text2.Length && char.IsWhiteSpace(text2[i]))
				{
					i++;
				}
			}
			return num;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004398 File Offset: 0x00002598
		// (set) Token: 0x0600010A RID: 266 RVA: 0x000043A0 File Offset: 0x000025A0
		[DataSourceProperty]
		public GameNotificationItemVM CurrentNotification
		{
			get
			{
				return this._currentNotification;
			}
			set
			{
				if (this._currentNotification != value)
				{
					this._currentNotification = value;
					int notificationId = this.NotificationId;
					this.NotificationId = notificationId + 1;
					base.OnPropertyChangedWithValue<GameNotificationItemVM>(value, "CurrentNotification");
					Action<GameNotificationItemVM> currentNotificationChanged = this.CurrentNotificationChanged;
					if (currentNotificationChanged == null)
					{
						return;
					}
					currentNotificationChanged(value);
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000043EA File Offset: 0x000025EA
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000043F2 File Offset: 0x000025F2
		[DataSourceProperty]
		public bool GotNotification
		{
			get
			{
				return this._gotNotification;
			}
			set
			{
				if (value != this._gotNotification)
				{
					this._gotNotification = value;
					base.OnPropertyChangedWithValue(value, "GotNotification");
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004410 File Offset: 0x00002610
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004418 File Offset: 0x00002618
		[DataSourceProperty]
		public int NotificationId
		{
			get
			{
				return this._notificationId;
			}
			set
			{
				if (value != this._notificationId)
				{
					this._notificationId = value;
					base.OnPropertyChangedWithValue(value, "NotificationId");
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004436 File Offset: 0x00002636
		// (set) Token: 0x06000110 RID: 272 RVA: 0x0000443E File Offset: 0x0000263E
		[DataSourceProperty]
		public float CurrentNotificationDurationInSeconds
		{
			get
			{
				return this._currentNotificationDurationInSeconds;
			}
			set
			{
				if (value != this._currentNotificationDurationInSeconds)
				{
					this._currentNotificationDurationInSeconds = value;
					base.OnPropertyChangedWithValue(value, "CurrentNotificationDurationInSeconds");
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000445C File Offset: 0x0000265C
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00004464 File Offset: 0x00002664
		[DataSourceProperty]
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChangedWithValue(value, "IsPaused");
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004482 File Offset: 0x00002682
		// (set) Token: 0x06000114 RID: 276 RVA: 0x0000448A File Offset: 0x0000268A
		[DataSourceProperty]
		public bool MustFadeOutCurrentNotification
		{
			get
			{
				return this._mustFadeOutCurrentNotification;
			}
			set
			{
				if (value != this._mustFadeOutCurrentNotification)
				{
					this._mustFadeOutCurrentNotification = value;
					base.OnPropertyChangedWithValue(value, "MustFadeOutCurrentNotification");
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000044A8 File Offset: 0x000026A8
		// (set) Token: 0x06000116 RID: 278 RVA: 0x000044B0 File Offset: 0x000026B0
		[DataSourceProperty]
		public float CurrentNotificationFadeOutDelayInSeconds
		{
			get
			{
				return this._currentNotificationFadeOutDelayInSeconds;
			}
			set
			{
				if (value != this._currentNotificationFadeOutDelayInSeconds)
				{
					this._currentNotificationFadeOutDelayInSeconds = value;
					base.OnPropertyChangedWithValue(value, "CurrentNotificationFadeOutDelayInSeconds");
				}
			}
		}

		// Token: 0x0400006D RID: 109
		private readonly List<GameNotificationItemVM> _items;

		// Token: 0x0400006E RID: 110
		private const float MinimumDisplayTimeInSeconds = 0.6f;

		// Token: 0x0400006F RID: 111
		private const float ExtraDisplayTimeInSeconds = 1f;

		// Token: 0x04000070 RID: 112
		private GameNotificationItemVM _currentNotification;

		// Token: 0x04000071 RID: 113
		private bool _gotNotification;

		// Token: 0x04000072 RID: 114
		private int _notificationId;

		// Token: 0x04000073 RID: 115
		private float _currentNotificationDurationInSeconds;

		// Token: 0x04000074 RID: 116
		private bool _isPaused;

		// Token: 0x04000075 RID: 117
		private bool _mustFadeOutCurrentNotification;

		// Token: 0x04000076 RID: 118
		private float _currentNotificationFadeOutDelayInSeconds;
	}
}
