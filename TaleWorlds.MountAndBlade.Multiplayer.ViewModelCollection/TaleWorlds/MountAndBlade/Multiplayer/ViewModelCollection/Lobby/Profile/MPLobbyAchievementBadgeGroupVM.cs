using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000034 RID: 52
	public class MPLobbyAchievementBadgeGroupVM : ViewModel
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x0001111B File Offset: 0x0000F31B
		public MPLobbyAchievementBadgeGroupVM(string groupID, Action<MPLobbyAchievementBadgeGroupVM> onBadgeProgressInfoRequested)
		{
			this._onBadgeProgressInfoRequested = onBadgeProgressInfoRequested;
			this.GroupID = groupID;
			this.Badges = new MBBindingList<MPLobbyBadgeItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00011142 File Offset: 0x0000F342
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ProgressCompletedText = new TextObject("{=vlACTion}You've unlocked all badges!", null).ToString();
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00011160 File Offset: 0x0000F360
		public void RefreshKeyBindings(HotKey inspectProgressKey)
		{
			this._inspectProgressKey = inspectProgressKey;
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in this.Badges)
			{
				mplobbyBadgeItemVM.RefreshKeyBindings(inspectProgressKey);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000111B4 File Offset: 0x0000F3B4
		public void OnGroupBadgeAdded(MPLobbyBadgeItemVM badgeItem)
		{
			if (this.ShownBadgeItem == null)
			{
				this.ShownBadgeItem = badgeItem;
			}
			else if (badgeItem.IsEarned && badgeItem.Badge.Index > this.ShownBadgeItem.Badge.Index)
			{
				this.ShownBadgeItem = badgeItem;
			}
			badgeItem.SetGroup(this, this._onBadgeProgressInfoRequested);
			this._totalBadgeCount++;
			if (badgeItem.IsEarned)
			{
				this._unlockedBadgeCount++;
			}
			this.IsProgressComplete = this._totalBadgeCount == this._unlockedBadgeCount;
			ConditionalBadge conditionalBadge;
			if ((conditionalBadge = badgeItem.Badge as ConditionalBadge) != null && conditionalBadge.BadgeConditions.Count > 0 && !conditionalBadge.IsTimed)
			{
				BadgeCondition badgeCondition = conditionalBadge.BadgeConditions[0];
				string text;
				int num;
				if (badgeCondition.Parameters.TryGetValue("min_value", out text) && int.TryParse(text, out num))
				{
					int num2 = NetworkMain.GameClient.PlayerData.GetBadgeConditionNumericValue(badgeCondition);
					if (badgeCondition.StringId.Equals("Playtime"))
					{
						num /= 3600;
						num2 /= 3600;
					}
					this.TotalProgress = Math.Max(this.TotalProgress, num);
					this.CurrentProgress = num2;
				}
				else
				{
					this.SetProgressAsCompleted();
				}
			}
			else
			{
				this.SetProgressAsCompleted();
			}
			badgeItem.RefreshKeyBindings(this._inspectProgressKey);
			this.Badges.Add(badgeItem);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00011314 File Offset: 0x0000F514
		private void SetProgressAsCompleted()
		{
			this.TotalProgress = 1;
			this.CurrentProgress = 1;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00011324 File Offset: 0x0000F524
		public void UpdateBadgeSelection()
		{
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in this.Badges)
			{
				mplobbyBadgeItemVM.UpdateIsSelected();
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00011370 File Offset: 0x0000F570
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00011378 File Offset: 0x0000F578
		[DataSourceProperty]
		public bool IsProgressComplete
		{
			get
			{
				return this._isProgressComplete;
			}
			set
			{
				if (value != this._isProgressComplete)
				{
					this._isProgressComplete = value;
					base.OnPropertyChangedWithValue(value, "IsProgressComplete");
					if (value)
					{
						this.SetProgressAsCompleted();
					}
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001139F File Offset: 0x0000F59F
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x000113A7 File Offset: 0x0000F5A7
		[DataSourceProperty]
		public string ProgressCompletedText
		{
			get
			{
				return this._progressCompletedText;
			}
			set
			{
				if (value != this._progressCompletedText)
				{
					this._progressCompletedText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressCompletedText");
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x000113CA File Offset: 0x0000F5CA
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x000113D2 File Offset: 0x0000F5D2
		[DataSourceProperty]
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (value != this._currentProgress)
				{
					this._currentProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProgress");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x000113F0 File Offset: 0x0000F5F0
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x000113F8 File Offset: 0x0000F5F8
		[DataSourceProperty]
		public int TotalProgress
		{
			get
			{
				return this._totalProgress;
			}
			set
			{
				if (value != this._totalProgress)
				{
					this._totalProgress = value;
					base.OnPropertyChangedWithValue(value, "TotalProgress");
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00011416 File Offset: 0x0000F616
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0001141E File Offset: 0x0000F61E
		[DataSourceProperty]
		public MPLobbyBadgeItemVM ShownBadgeItem
		{
			get
			{
				return this._shownBadgeItem;
			}
			set
			{
				if (value != this._shownBadgeItem)
				{
					this._shownBadgeItem = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeItemVM>(value, "ShownBadgeItem");
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0001143C File Offset: 0x0000F63C
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00011444 File Offset: 0x0000F644
		[DataSourceProperty]
		public MBBindingList<MPLobbyBadgeItemVM> Badges
		{
			get
			{
				return this._badges;
			}
			set
			{
				if (value != this._badges)
				{
					this._badges = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyBadgeItemVM>>(value, "Badges");
				}
			}
		}

		// Token: 0x0400024C RID: 588
		public readonly string GroupID;

		// Token: 0x0400024D RID: 589
		private readonly Action<MPLobbyAchievementBadgeGroupVM> _onBadgeProgressInfoRequested;

		// Token: 0x0400024E RID: 590
		private int _unlockedBadgeCount;

		// Token: 0x0400024F RID: 591
		private int _totalBadgeCount;

		// Token: 0x04000250 RID: 592
		private const string PlaytimeConditionID = "Playtime";

		// Token: 0x04000251 RID: 593
		private HotKey _inspectProgressKey;

		// Token: 0x04000252 RID: 594
		private bool _isProgressComplete;

		// Token: 0x04000253 RID: 595
		private string _progressCompletedText;

		// Token: 0x04000254 RID: 596
		private int _currentProgress;

		// Token: 0x04000255 RID: 597
		private int _totalProgress;

		// Token: 0x04000256 RID: 598
		private MPLobbyBadgeItemVM _shownBadgeItem;

		// Token: 0x04000257 RID: 599
		private MBBindingList<MPLobbyBadgeItemVM> _badges;
	}
}
