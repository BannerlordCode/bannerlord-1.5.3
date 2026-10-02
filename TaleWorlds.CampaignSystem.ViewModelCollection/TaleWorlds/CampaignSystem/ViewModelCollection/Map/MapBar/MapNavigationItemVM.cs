using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x02000063 RID: 99
	public class MapNavigationItemVM : ViewModel
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x00022410 File Offset: 0x00020610
		public MapNavigationItemVM(INavigationElement navigationElement)
		{
			this.NavigationElement = navigationElement;
			this.Tooltip = new BasicTooltipViewModel(() => this.GetTooltip());
			this.AlertTooltip = new BasicTooltipViewModel(() => this.GetAlertTooltip());
			this.ItemId = this.NavigationElement.StringId;
			this.RefreshStates(true);
			this.RefreshValues();
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00022478 File Offset: 0x00020678
		private string GetTooltip()
		{
			NavigationPermissionItem permission = this.NavigationElement.Permission;
			if (permission.IsAuthorized || this.NavigationElement.IsActive)
			{
				TextObject tooltip = this.NavigationElement.Tooltip;
				if (tooltip == null)
				{
					return null;
				}
				return tooltip.ToString();
			}
			else
			{
				TextObject reasonString = permission.ReasonString;
				if (reasonString == null)
				{
					return null;
				}
				return reasonString.ToString();
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000224D0 File Offset: 0x000206D0
		private string GetAlertTooltip()
		{
			TextObject alertTooltip = this.NavigationElement.AlertTooltip;
			if (alertTooltip == null)
			{
				return null;
			}
			return alertTooltip.ToString();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000224E8 File Offset: 0x000206E8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AlertText = GameTexts.FindText("str_map_bar_alert", null).ToString();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00022508 File Offset: 0x00020708
		public void RefreshStates(bool forceRefresh = false)
		{
			this.IsActive = this.NavigationElement.IsActive;
			this.HasAlert = this.NavigationElement.HasAlert;
			this.IsEnabled = this.NavigationElement.Permission.IsAuthorized;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00022550 File Offset: 0x00020750
		public void ExecuteOpen()
		{
			this.NavigationElement.OpenView();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002255D File Offset: 0x0002075D
		public void ExecuteGoToLink()
		{
			this.NavigationElement.GoToLink();
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002256A File Offset: 0x0002076A
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00022572 File Offset: 0x00020772
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00022590 File Offset: 0x00020790
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00022598 File Offset: 0x00020798
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x000225B6 File Offset: 0x000207B6
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x000225BE File Offset: 0x000207BE
		[DataSourceProperty]
		public bool HasAlert
		{
			get
			{
				return this._hasAlert;
			}
			set
			{
				if (value != this._hasAlert)
				{
					this._hasAlert = value;
					base.OnPropertyChangedWithValue(value, "HasAlert");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x000225DC File Offset: 0x000207DC
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x000225E4 File Offset: 0x000207E4
		[DataSourceProperty]
		public string ItemId
		{
			get
			{
				return this._itemId;
			}
			set
			{
				if (value != this._itemId)
				{
					this._itemId = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemId");
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00022607 File Offset: 0x00020807
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0002260F File Offset: 0x0002080F
		[DataSourceProperty]
		public string AlertText
		{
			get
			{
				return this._alertText;
			}
			set
			{
				if (value != this._alertText)
				{
					this._alertText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlertText");
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00022632 File Offset: 0x00020832
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0002263A File Offset: 0x0002083A
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00022658 File Offset: 0x00020858
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00022660 File Offset: 0x00020860
		[DataSourceProperty]
		public BasicTooltipViewModel AlertTooltip
		{
			get
			{
				return this._alertTooltip;
			}
			set
			{
				if (value != this._alertTooltip)
				{
					this._alertTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AlertTooltip");
				}
			}
		}

		// Token: 0x040002EC RID: 748
		public readonly INavigationElement NavigationElement;

		// Token: 0x040002ED RID: 749
		private bool _isEnabled;

		// Token: 0x040002EE RID: 750
		private bool _isActive;

		// Token: 0x040002EF RID: 751
		private bool _hasAlert;

		// Token: 0x040002F0 RID: 752
		private string _itemId;

		// Token: 0x040002F1 RID: 753
		private string _alertText;

		// Token: 0x040002F2 RID: 754
		private BasicTooltipViewModel _tooltip;

		// Token: 0x040002F3 RID: 755
		private BasicTooltipViewModel _alertTooltip;
	}
}
