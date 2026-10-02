using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000013 RID: 19
	public class GameNotificationItemVM : ViewModel
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public GameNotificationItemVM(string notificationText, int extraTimeInMs, BasicCharacterObject announcerCharacter, Equipment characterEquipment, string soundId, int priority, bool isDialog, string dialogSoundPath)
		{
			this.GameNotificationText = notificationText;
			this.NotificationSoundId = soundId;
			this.Announcer = ((announcerCharacter != null) ? new CharacterImageIdentifierVM(CharacterCode.CreateFrom(announcerCharacter, characterEquipment)) : new CharacterImageIdentifierVM(null));
			this.CharacterNameText = ((announcerCharacter != null) ? announcerCharacter.Name.ToString() : "");
			this.ExtraTimeInMs = extraTimeInMs;
			this.Priority = priority;
			this.IsDialog = isDialog;
			this.DialogSoundPath = dialogSoundPath;
			if (this.IsDialog)
			{
				this.Handle = new MBInformationManager.DialogNotificationHandle();
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00003D41 File Offset: 0x00001F41
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00003D49 File Offset: 0x00001F49
		[DataSourceProperty]
		public int ExtraTimeInMs
		{
			get
			{
				return this._extraTimeInMs;
			}
			set
			{
				if (value != this._extraTimeInMs)
				{
					this._extraTimeInMs = value;
					base.OnPropertyChangedWithValue(value, "ExtraTimeInMs");
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003D67 File Offset: 0x00001F67
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00003D6F File Offset: 0x00001F6F
		[DataSourceProperty]
		public string GameNotificationText
		{
			get
			{
				return this._gameNotificationText;
			}
			set
			{
				if (value != this._gameNotificationText)
				{
					this._gameNotificationText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameNotificationText");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003D92 File Offset: 0x00001F92
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00003D9A File Offset: 0x00001F9A
		[DataSourceProperty]
		public string CharacterNameText
		{
			get
			{
				return this._characterNameText;
			}
			set
			{
				if (value != this._characterNameText)
				{
					this._characterNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "CharacterNameText");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003DBD File Offset: 0x00001FBD
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00003DC5 File Offset: 0x00001FC5
		[DataSourceProperty]
		public string NotificationSoundId
		{
			get
			{
				return this._notificationSoundId;
			}
			set
			{
				if (value != this._notificationSoundId)
				{
					this._notificationSoundId = value;
					base.OnPropertyChangedWithValue<string>(value, "NotificationSoundId");
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00003DE8 File Offset: 0x00001FE8
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00003DF0 File Offset: 0x00001FF0
		[DataSourceProperty]
		public string DialogSoundPath
		{
			get
			{
				return this._dialogSoundPath;
			}
			set
			{
				if (value != this._dialogSoundPath)
				{
					this._dialogSoundPath = value;
					base.OnPropertyChangedWithValue<string>(value, "DialogSoundPath");
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003E13 File Offset: 0x00002013
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00003E1B File Offset: 0x0000201B
		[DataSourceProperty]
		public CharacterImageIdentifierVM Announcer
		{
			get
			{
				return this._announcer;
			}
			set
			{
				if (value != this._announcer)
				{
					this._announcer = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Announcer");
				}
			}
		}

		// Token: 0x04000063 RID: 99
		public readonly int Priority;

		// Token: 0x04000064 RID: 100
		public readonly bool IsDialog;

		// Token: 0x04000065 RID: 101
		public readonly MBInformationManager.DialogNotificationHandle Handle;

		// Token: 0x04000066 RID: 102
		private string _gameNotificationText;

		// Token: 0x04000067 RID: 103
		private string _characterNameText;

		// Token: 0x04000068 RID: 104
		private string _notificationSoundId;

		// Token: 0x04000069 RID: 105
		private string _dialogSoundPath;

		// Token: 0x0400006A RID: 106
		private int _extraTimeInMs;

		// Token: 0x0400006B RID: 107
		private CharacterImageIdentifierVM _announcer;
	}
}
