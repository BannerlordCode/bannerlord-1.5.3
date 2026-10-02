using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x02000005 RID: 5
	public class FullScreenNoticeVM : ViewModel
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000022CE File Offset: 0x000004CE
		public FullScreenNoticeVM()
		{
			this.IsNoticeActive = true;
			this.RefreshValues();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022E4 File Offset: 0x000004E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NoticeTitleText = new TextObject("{=LW8QCm22}Notice", null).ToString();
			this.ConfirmText = new TextObject("{=5Unqsx3N}Confirm", null).ToString();
			TextObject textObject = new TextObject("{=Uma907CV}We have released the War Sails Expansion Pack for the game which includes ships, naval battles, and much more. You can use the link on the main menu to go to the store page for more information and obtaining the expansion pack. The addition of the product page link also requires us to update our ESRB rating information as follows:", null);
			string text = new TextObject("{=VIdtkghu}The ESRB rating information has been updated to include the interactive element:{newline}In-Game Purchases", null).SetTextVariable("newline", "\n").ToString();
			this.NoticeContentText = new TextObject("{=!}{LEFT}{newline}{newline}{RIGHT}", null).SetTextVariable("newline", "\n").SetTextVariable("LEFT", textObject).SetTextVariable("RIGHT", text)
				.ToString();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000238A File Offset: 0x0000058A
		public void ExecuteCloseNotice()
		{
			this.IsNoticeActive = false;
			BannerlordConfig.IAPNoticeConfirmed = true;
			BannerlordConfig.Save();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000239F File Offset: 0x0000059F
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000023B7 File Offset: 0x000005B7
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000023BF File Offset: 0x000005BF
		[DataSourceProperty]
		public bool IsNoticeActive
		{
			get
			{
				return this._isNoticeActive;
			}
			set
			{
				if (value != this._isNoticeActive)
				{
					this._isNoticeActive = value;
					base.OnPropertyChangedWithValue(value, "IsNoticeActive");
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000023DD File Offset: 0x000005DD
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000023E5 File Offset: 0x000005E5
		[DataSourceProperty]
		public string NoticeTitleText
		{
			get
			{
				return this._noticeTitleText;
			}
			set
			{
				if (value != this._noticeTitleText)
				{
					this._noticeTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoticeTitleText");
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002408 File Offset: 0x00000608
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002410 File Offset: 0x00000610
		[DataSourceProperty]
		public string NoticeContentText
		{
			get
			{
				return this._noticeContentText;
			}
			set
			{
				if (value != this._noticeContentText)
				{
					this._noticeContentText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoticeContentText");
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002433 File Offset: 0x00000633
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000243B File Offset: 0x0000063B
		[DataSourceProperty]
		public string ConfirmText
		{
			get
			{
				return this._confirmText;
			}
			set
			{
				if (value != this._confirmText)
				{
					this._confirmText = value;
					base.OnPropertyChangedWithValue<string>(value, "ConfirmText");
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000245E File Offset: 0x0000065E
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000246D File Offset: 0x0000066D
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002475 File Offset: 0x00000675
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x0400000A RID: 10
		private bool _isNoticeActive;

		// Token: 0x0400000B RID: 11
		private string _noticeTitleText;

		// Token: 0x0400000C RID: 12
		private string _noticeContentText;

		// Token: 0x0400000D RID: 13
		private string _confirmText;

		// Token: 0x0400000E RID: 14
		private InputKeyItemVM _doneInputKey;
	}
}
