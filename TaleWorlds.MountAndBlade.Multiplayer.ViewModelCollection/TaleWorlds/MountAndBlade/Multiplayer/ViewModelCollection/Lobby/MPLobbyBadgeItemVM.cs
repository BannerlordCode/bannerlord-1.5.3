using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000028 RID: 40
	public class MPLobbyBadgeItemVM : ViewModel
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000B927 File Offset: 0x00009B27
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000B92F File Offset: 0x00009B2F
		public Badge Badge { get; private set; }

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B938 File Offset: 0x00009B38
		public MPLobbyBadgeItemVM(Badge badge, Action onSelectedBadgeChange, Func<Badge, bool> hasPlayerEarnedBadge, Action<MPLobbyBadgeItemVM> onInspected)
		{
			this._hasPlayerEarnedBadge = hasPlayerEarnedBadge;
			this._onSelectedBadgeChange = onSelectedBadgeChange;
			this._onInspected = onInspected;
			this.Badge = badge;
			this.Conditions = new MBBindingList<StringPairItemVM>();
			this.UpdateWith(this.Badge);
			this.RefreshValues();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B985 File Offset: 0x00009B85
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.BadgeConditionsText = GameTexts.FindText("str_multiplayer_badge_conditions", null).ToString();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000B9A3 File Offset: 0x00009BA3
		public void RefreshKeyBindings(HotKey inspectProgressKey)
		{
			this.InspectProgressKey = InputKeyItemVM.CreateFromHotKey(inspectProgressKey, false);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000B9B2 File Offset: 0x00009BB2
		public override void OnFinalize()
		{
			InputKeyItemVM inspectProgressKey = this.InspectProgressKey;
			if (inspectProgressKey == null)
			{
				return;
			}
			inspectProgressKey.OnFinalize();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000B9C4 File Offset: 0x00009BC4
		public void UpdateWith(Badge badge)
		{
			this.Badge = badge;
			this.BadgeId = ((this.Badge == null) ? "none" : this.Badge.StringId);
			this.UpdateIsSelected();
			this.IsEarned = this._hasPlayerEarnedBadge(badge);
			this.RefreshProperties();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BA18 File Offset: 0x00009C18
		private void RefreshProperties()
		{
			this.Conditions.Clear();
			if (this.Badge != null)
			{
				this.Name = this.Badge.Name.ToString();
				this.Description = this.Badge.Description.ToString();
				ConditionalBadge conditionalBadge;
				if ((conditionalBadge = this.Badge as ConditionalBadge) == null || conditionalBadge.BadgeConditions.Count <= 0 || this.Badge.IsTimed)
				{
					return;
				}
				using (IEnumerator<BadgeCondition> enumerator = conditionalBadge.BadgeConditions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BadgeCondition badgeCondition = enumerator.Current;
						if (badgeCondition.Type == ConditionType.PlayerDataNumeric)
						{
							int num = NetworkMain.GameClient.PlayerData.GetBadgeConditionNumericValue(badgeCondition);
							if (badgeCondition.StringId.Equals("Playtime"))
							{
								num /= 3600;
							}
							this.Conditions.Add(new StringPairItemVM(badgeCondition.Description.ToString(), num.ToString(), null));
						}
					}
					return;
				}
			}
			this.Name = new TextObject("{=koX9okuG}None", null).ToString();
			this.Description = new TextObject("{=gcl2duJH}Reset your badge", null).ToString();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000BB58 File Offset: 0x00009D58
		private async void ExecuteSetAsActive()
		{
			this.IsBeingChanged = true;
			if (this.Badge != null)
			{
				if (this.IsEarned)
				{
					await NetworkMain.GameClient.UpdateShownBadgeId(this.Badge.StringId);
				}
				else
				{
					InformationManager.ShowInquiry(new InquiryData(string.Empty, new TextObject("{=B1KQ4i9q}Badge is not earned yet. Please check conditions.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), false, false);
				}
			}
			else
			{
				await NetworkMain.GameClient.UpdateShownBadgeId("");
			}
			this.IsBeingChanged = false;
			this._onSelectedBadgeChange();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000BB91 File Offset: 0x00009D91
		private void ExecuteShowProgression()
		{
			if (this.Badge is ConditionalBadge)
			{
				Action<MPLobbyAchievementBadgeGroupVM> onBadgeProgressInfoRequested = this._onBadgeProgressInfoRequested;
				if (onBadgeProgressInfoRequested == null)
				{
					return;
				}
				onBadgeProgressInfoRequested(this._group);
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public void UpdateIsSelected()
		{
			if (this.Badge == null)
			{
				PlayerData playerData = NetworkMain.GameClient.PlayerData;
				this.IsSelected = string.IsNullOrEmpty((playerData != null) ? playerData.ShownBadgeId : null);
				return;
			}
			string stringId = this.Badge.StringId;
			PlayerData playerData2 = NetworkMain.GameClient.PlayerData;
			this.IsSelected = stringId == ((playerData2 != null) ? playerData2.ShownBadgeId : null);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000BC1B File Offset: 0x00009E1B
		public void SetGroup(MPLobbyAchievementBadgeGroupVM group, Action<MPLobbyAchievementBadgeGroupVM> onBadgeProgressInfoRequested)
		{
			this._group = group;
			this._onBadgeProgressInfoRequested = onBadgeProgressInfoRequested;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000BC2B File Offset: 0x00009E2B
		private void ExecuteGainFocus()
		{
			this.IsFocused = true;
			if (this.HasNotification)
			{
				this.HasNotification = false;
			}
			Action<MPLobbyBadgeItemVM> onInspected = this._onInspected;
			if (onInspected == null)
			{
				return;
			}
			onInspected(this);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000BC54 File Offset: 0x00009E54
		private void ExecuteLoseFocus()
		{
			this.IsFocused = false;
			Action<MPLobbyBadgeItemVM> onInspected = this._onInspected;
			if (onInspected == null)
			{
				return;
			}
			onInspected(null);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000BC6E File Offset: 0x00009E6E
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000BC76 File Offset: 0x00009E76
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000BC99 File Offset: 0x00009E99
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000BCA1 File Offset: 0x00009EA1
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000BCC4 File Offset: 0x00009EC4
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000BCCC File Offset: 0x00009ECC
		[DataSourceProperty]
		public string BadgeConditionsText
		{
			get
			{
				return this._badgeConditionsText;
			}
			set
			{
				if (value != this._badgeConditionsText)
				{
					this._badgeConditionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeConditionsText");
				}
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000BCEF File Offset: 0x00009EEF
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000BCF7 File Offset: 0x00009EF7
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> Conditions
		{
			get
			{
				return this._conditions;
			}
			set
			{
				if (value != this._conditions)
				{
					this._conditions = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "Conditions");
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000BD15 File Offset: 0x00009F15
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000BD1D File Offset: 0x00009F1D
		[DataSourceProperty]
		public string BadgeId
		{
			get
			{
				return this._badgeId;
			}
			set
			{
				if (value != this._badgeId)
				{
					this._badgeId = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeId");
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000BD40 File Offset: 0x00009F40
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000BD48 File Offset: 0x00009F48
		[DataSourceProperty]
		public bool IsEarned
		{
			get
			{
				return this._isEarned;
			}
			set
			{
				if (value != this._isEarned)
				{
					this._isEarned = value;
					base.OnPropertyChangedWithValue(value, "IsEarned");
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000BD66 File Offset: 0x00009F66
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000BD6E File Offset: 0x00009F6E
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000BD8C File Offset: 0x00009F8C
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000BD94 File Offset: 0x00009F94
		[DataSourceProperty]
		public bool HasNotification
		{
			get
			{
				return this._hasNotification;
			}
			set
			{
				if (value != this._hasNotification)
				{
					this._hasNotification = value;
					base.OnPropertyChangedWithValue(value, "HasNotification");
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000BDB2 File Offset: 0x00009FB2
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000BDBA File Offset: 0x00009FBA
		[DataSourceProperty]
		public bool IsBeingChanged
		{
			get
			{
				return this._isBeingChanged;
			}
			set
			{
				if (value != this._isBeingChanged)
				{
					this._isBeingChanged = value;
					base.OnPropertyChangedWithValue(value, "IsBeingChanged");
				}
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000BDD8 File Offset: 0x00009FD8
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000BDFE File Offset: 0x00009FFE
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0000BE06 File Offset: 0x0000A006
		[DataSourceProperty]
		public InputKeyItemVM InspectProgressKey
		{
			get
			{
				return this._inspectProgressKey;
			}
			set
			{
				if (value != this._inspectProgressKey)
				{
					this._inspectProgressKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "InspectProgressKey");
				}
			}
		}

		// Token: 0x0400017E RID: 382
		private readonly Func<Badge, bool> _hasPlayerEarnedBadge;

		// Token: 0x0400017F RID: 383
		private readonly Action _onSelectedBadgeChange;

		// Token: 0x04000180 RID: 384
		private readonly Action<MPLobbyBadgeItemVM> _onInspected;

		// Token: 0x04000181 RID: 385
		private MPLobbyAchievementBadgeGroupVM _group;

		// Token: 0x04000182 RID: 386
		private Action<MPLobbyAchievementBadgeGroupVM> _onBadgeProgressInfoRequested;

		// Token: 0x04000183 RID: 387
		private const string PlaytimeConditionID = "Playtime";

		// Token: 0x04000184 RID: 388
		private string _name;

		// Token: 0x04000185 RID: 389
		private string _description;

		// Token: 0x04000186 RID: 390
		private string _badgeConditionsText;

		// Token: 0x04000187 RID: 391
		private string _badgeId;

		// Token: 0x04000188 RID: 392
		private bool _isEarned;

		// Token: 0x04000189 RID: 393
		private bool _isSelected;

		// Token: 0x0400018A RID: 394
		private bool _hasNotification;

		// Token: 0x0400018B RID: 395
		private bool _isBeingChanged;

		// Token: 0x0400018C RID: 396
		private bool _isFocused;

		// Token: 0x0400018D RID: 397
		private MBBindingList<StringPairItemVM> _conditions;

		// Token: 0x0400018E RID: 398
		private InputKeyItemVM _inspectProgressKey;
	}
}
