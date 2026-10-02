using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000035 RID: 53
	public class MPLobbyBadgeProgressInformationVM : ViewModel
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x00011464 File Offset: 0x0000F664
		public MPLobbyBadgeProgressInformationVM(Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this.AvailableBadgeIDs = new MBBindingList<StringPairItemVM>();
			this.ShownBadgeCount = 5;
			for (int i = 0; i < this.ShownBadgeCount; i++)
			{
				this.AvailableBadgeIDs.Add(new StringPairItemVM(string.Empty, string.Empty, null));
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000114BC File Offset: 0x0000F6BC
		public void OpenWith(MPLobbyAchievementBadgeGroupVM badgeGroup)
		{
			this.BadgeGroup = badgeGroup;
			this.TitleText = (this.BadgeGroup.ShownBadgeItem.Badge as ConditionalBadge).BadgeConditions[0].Description.ToString();
			this._shownBadgeIndexOffset = 0;
			this.RefreshShownBadges();
			Func<string> getExitText = this._getExitText;
			this.ClickToCloseText = ((getExitText != null) ? getExitText() : null);
			this.IsEnabled = true;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001152C File Offset: 0x0000F72C
		private void RefreshShownBadges()
		{
			int num = this.BadgeGroup.Badges.IndexOf(this.BadgeGroup.ShownBadgeItem) + this._shownBadgeIndexOffset;
			int num2 = 0;
			int num3 = this.ShownBadgeCount / 2;
			for (int i = num - num3; i <= num + num3; i++)
			{
				if (i >= 0 && i < this.BadgeGroup.Badges.Count)
				{
					MPLobbyBadgeItemVM mplobbyBadgeItemVM = this.BadgeGroup.Badges[i];
					this.AvailableBadgeIDs[num2].Value = mplobbyBadgeItemVM.BadgeId;
					this.AvailableBadgeIDs[num2].Definition = mplobbyBadgeItemVM.Name;
				}
				else
				{
					this.AvailableBadgeIDs[num2].Value = string.Empty;
					this.AvailableBadgeIDs[num2].Definition = string.Empty;
				}
				num2++;
			}
			this.CanIncreaseBadgeIndices = this.BadgeGroup.Badges.IndexOf(this.BadgeGroup.Badges[num]) < this.BadgeGroup.Badges.Count - 1;
			this.CanDecreaseBadgeIndices = num > 0;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001164E File Offset: 0x0000F84E
		public void ExecuteClosePopup()
		{
			this.BadgeGroup = null;
			this.IsEnabled = false;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001165E File Offset: 0x0000F85E
		public void ExecuteIncreaseActiveBadgeIndices()
		{
			if (this.CanIncreaseBadgeIndices)
			{
				this._shownBadgeIndexOffset++;
				this.RefreshShownBadges();
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001167C File Offset: 0x0000F87C
		public void ExecuteDecreaseActiveBadgeIndices()
		{
			if (this.CanDecreaseBadgeIndices)
			{
				this._shownBadgeIndexOffset--;
				this.RefreshShownBadges();
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001169A File Offset: 0x0000F89A
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM previousTabInputKey = this.PreviousTabInputKey;
			if (previousTabInputKey != null)
			{
				previousTabInputKey.OnFinalize();
			}
			InputKeyItemVM nextTabInputKey = this.NextTabInputKey;
			if (nextTabInputKey == null)
			{
				return;
			}
			nextTabInputKey.OnFinalize();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000116C3 File Offset: 0x0000F8C3
		public void SetPreviousTabInputKey(HotKey hotKey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000116D2 File Offset: 0x0000F8D2
		public void SetNextTabInputKey(HotKey hotKey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x000116E1 File Offset: 0x0000F8E1
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x000116E9 File Offset: 0x0000F8E9
		[DataSourceProperty]
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChanged("PreviousTabInputKey");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00011706 File Offset: 0x0000F906
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x0001170E File Offset: 0x0000F90E
		[DataSourceProperty]
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChanged("NextTabInputKey");
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001172B File Offset: 0x0000F92B
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00011733 File Offset: 0x0000F933
		[DataSourceProperty]
		public int ShownBadgeCount
		{
			get
			{
				return this._shownBadgeCount;
			}
			set
			{
				if (value != this._shownBadgeCount)
				{
					this._shownBadgeCount = value;
					base.OnPropertyChangedWithValue(value, "ShownBadgeCount");
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00011751 File Offset: 0x0000F951
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00011759 File Offset: 0x0000F959
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00011777 File Offset: 0x0000F977
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x0001177F File Offset: 0x0000F97F
		[DataSourceProperty]
		public bool CanIncreaseBadgeIndices
		{
			get
			{
				return this._canIncreaseBadgeIndices;
			}
			set
			{
				if (value != this._canIncreaseBadgeIndices)
				{
					this._canIncreaseBadgeIndices = value;
					base.OnPropertyChangedWithValue(value, "CanIncreaseBadgeIndices");
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0001179D File Offset: 0x0000F99D
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x000117A5 File Offset: 0x0000F9A5
		[DataSourceProperty]
		public bool CanDecreaseBadgeIndices
		{
			get
			{
				return this._canDecreaseBadgeIndices;
			}
			set
			{
				if (value != this._canDecreaseBadgeIndices)
				{
					this._canDecreaseBadgeIndices = value;
					base.OnPropertyChangedWithValue(value, "CanDecreaseBadgeIndices");
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000117C3 File Offset: 0x0000F9C3
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x000117CB File Offset: 0x0000F9CB
		[DataSourceProperty]
		public string ClickToCloseText
		{
			get
			{
				return this._clickToCloseText;
			}
			set
			{
				if (value != this._clickToCloseText)
				{
					this._clickToCloseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToCloseText");
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000117EE File Offset: 0x0000F9EE
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x000117F6 File Offset: 0x0000F9F6
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00011819 File Offset: 0x0000FA19
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00011821 File Offset: 0x0000FA21
		[DataSourceProperty]
		public MPLobbyAchievementBadgeGroupVM BadgeGroup
		{
			get
			{
				return this._badgeGroup;
			}
			set
			{
				if (value != this._badgeGroup)
				{
					this._badgeGroup = value;
					base.OnPropertyChangedWithValue<MPLobbyAchievementBadgeGroupVM>(value, "BadgeGroup");
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0001183F File Offset: 0x0000FA3F
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00011847 File Offset: 0x0000FA47
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> AvailableBadgeIDs
		{
			get
			{
				return this._availableBadgeIDs;
			}
			set
			{
				if (value != this._availableBadgeIDs)
				{
					this._availableBadgeIDs = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "AvailableBadgeIDs");
				}
			}
		}

		// Token: 0x04000258 RID: 600
		private int _shownBadgeIndexOffset;

		// Token: 0x04000259 RID: 601
		private const int MaxShownBadgeCount = 5;

		// Token: 0x0400025A RID: 602
		private readonly Func<string> _getExitText;

		// Token: 0x0400025B RID: 603
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x0400025C RID: 604
		private InputKeyItemVM _nextTabInputKey;

		// Token: 0x0400025D RID: 605
		private int _shownBadgeCount;

		// Token: 0x0400025E RID: 606
		private bool _isEnabled;

		// Token: 0x0400025F RID: 607
		private bool _canIncreaseBadgeIndices;

		// Token: 0x04000260 RID: 608
		private bool _canDecreaseBadgeIndices;

		// Token: 0x04000261 RID: 609
		private string _clickToCloseText;

		// Token: 0x04000262 RID: 610
		private string _titleText;

		// Token: 0x04000263 RID: 611
		private MPLobbyAchievementBadgeGroupVM _badgeGroup;

		// Token: 0x04000264 RID: 612
		private MBBindingList<StringPairItemVM> _availableBadgeIDs;
	}
}
