using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000053 RID: 83
	public class CheerBarkNodeItemVM : ViewModel
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060006B3 RID: 1715 RVA: 0x0001858C File Offset: 0x0001678C
		// (remove) Token: 0x060006B4 RID: 1716 RVA: 0x000185C0 File Offset: 0x000167C0
		internal static event Action<CheerBarkNodeItemVM> OnSelection;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006B5 RID: 1717 RVA: 0x000185F4 File Offset: 0x000167F4
		// (remove) Token: 0x060006B6 RID: 1718 RVA: 0x00018628 File Offset: 0x00016828
		internal static event Action<CheerBarkNodeItemVM> OnNodeFocused;

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001865C File Offset: 0x0001685C
		public CheerBarkNodeItemVM(string tauntVisualName, TextObject nodeName, string nodeId, HotKey key, bool consoleOnlyShortcut = false, TauntUsageManager.TauntUsage.TauntUsageFlag disabledReason = TauntUsageManager.TauntUsage.TauntUsageFlag.None)
		{
			this._nodeName = nodeName;
			this.TauntVisualName = tauntVisualName;
			this.TypeAsString = nodeId;
			this.TauntUsageDisabledReason = disabledReason;
			this.IsDisabled = disabledReason != TauntUsageManager.TauntUsage.TauntUsageFlag.None && disabledReason != TauntUsageManager.TauntUsage.TauntUsageFlag.IsLeftStance;
			this.SubNodes = new MBBindingList<CheerBarkNodeItemVM>();
			if (key != null)
			{
				this.ShortcutKey = InputKeyItemVM.CreateFromHotKey(key, consoleOnlyShortcut);
			}
			this.RefreshValues();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000186C8 File Offset: 0x000168C8
		public CheerBarkNodeItemVM(TextObject nodeName, string nodeId, HotKey key, bool consoleOnlyShortcut = false, TauntUsageManager.TauntUsage.TauntUsageFlag disabledReason = TauntUsageManager.TauntUsage.TauntUsageFlag.None)
		{
			this._nodeName = nodeName;
			this.TauntVisualName = string.Empty;
			this.TypeAsString = nodeId;
			this.TauntUsageDisabledReason = disabledReason;
			this.IsDisabled = disabledReason > TauntUsageManager.TauntUsage.TauntUsageFlag.None;
			this.SubNodes = new MBBindingList<CheerBarkNodeItemVM>();
			if (key != null)
			{
				this.ShortcutKey = InputKeyItemVM.CreateFromHotKey(key, consoleOnlyShortcut);
			}
			this.RefreshValues();
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001872C File Offset: 0x0001692C
		public void ClearSelectionRecursive()
		{
			this.IsSelected = false;
			for (int i = 0; i < this.SubNodes.Count; i++)
			{
				this.SubNodes[i].ClearSelectionRecursive();
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00018767 File Offset: 0x00016967
		public void ExecuteFocused()
		{
			Action<CheerBarkNodeItemVM> onNodeFocused = CheerBarkNodeItemVM.OnNodeFocused;
			if (onNodeFocused == null)
			{
				return;
			}
			onNodeFocused(this);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00018779 File Offset: 0x00016979
		public override void RefreshValues()
		{
			TextObject nodeName = this._nodeName;
			this.CheerNameText = ((nodeName != null) ? nodeName.ToString() : null);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00018793 File Offset: 0x00016993
		public void AddSubNode(CheerBarkNodeItemVM subNode)
		{
			this.SubNodes.Add(subNode);
			this.HasSubNodes = true;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000187A8 File Offset: 0x000169A8
		public override void OnFinalize()
		{
			base.OnFinalize();
			MBBindingList<CheerBarkNodeItemVM> subNodes = this.SubNodes;
			if (subNodes != null)
			{
				subNodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
				{
					n.OnFinalize();
				});
			}
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey != null)
			{
				shortcutKey.OnFinalize();
			}
			this.ShortcutKey = null;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00018803 File Offset: 0x00016A03
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0001880B File Offset: 0x00016A0B
		[DataSourceProperty]
		public InputKeyItemVM ShortcutKey
		{
			get
			{
				return this._shortcutKey;
			}
			set
			{
				if (value != this._shortcutKey)
				{
					this._shortcutKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShortcutKey");
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00018829 File Offset: 0x00016A29
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x00018831 File Offset: 0x00016A31
		[DataSourceProperty]
		public MBBindingList<CheerBarkNodeItemVM> SubNodes
		{
			get
			{
				return this._subNodes;
			}
			set
			{
				if (value != this._subNodes)
				{
					this._subNodes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CheerBarkNodeItemVM>>(value, "SubNodes");
				}
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0001884F File Offset: 0x00016A4F
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x00018857 File Offset: 0x00016A57
		[DataSourceProperty]
		public string CheerNameText
		{
			get
			{
				return this._cheerNameText;
			}
			set
			{
				if (value != this._cheerNameText)
				{
					this._cheerNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "CheerNameText");
				}
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0001887A File Offset: 0x00016A7A
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00018882 File Offset: 0x00016A82
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x000188A0 File Offset: 0x00016AA0
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x000188A8 File Offset: 0x00016AA8
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
					if (this._isSelected)
					{
						Action<CheerBarkNodeItemVM> onSelection = CheerBarkNodeItemVM.OnSelection;
						if (onSelection == null)
						{
							return;
						}
						onSelection(this);
					}
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x000188DE File Offset: 0x00016ADE
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x000188E6 File Offset: 0x00016AE6
		[DataSourceProperty]
		public bool HasSubNodes
		{
			get
			{
				return this._hasSubNodes;
			}
			set
			{
				if (value != this._hasSubNodes)
				{
					this._hasSubNodes = value;
					base.OnPropertyChanged("HasSubNodes");
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00018903 File Offset: 0x00016B03
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x0001890B File Offset: 0x00016B0B
		[DataSourceProperty]
		public string TypeAsString
		{
			get
			{
				return this._typeAsString;
			}
			set
			{
				if (value != this._typeAsString)
				{
					this._typeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeAsString");
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0001892E File Offset: 0x00016B2E
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x00018936 File Offset: 0x00016B36
		[DataSourceProperty]
		public string TauntVisualName
		{
			get
			{
				return this._tauntVisualName;
			}
			set
			{
				if (value != this._tauntVisualName)
				{
					this._tauntVisualName = value;
					base.OnPropertyChangedWithValue<string>(value, "TauntVisualName");
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00018959 File Offset: 0x00016B59
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x00018961 File Offset: 0x00016B61
		[DataSourceProperty]
		public string SelectedNodeText
		{
			get
			{
				return this._selectedNodeText;
			}
			set
			{
				if (value != this._selectedNodeText)
				{
					this._selectedNodeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedNodeText");
				}
			}
		}

		// Token: 0x04000302 RID: 770
		public readonly TauntUsageManager.TauntUsage.TauntUsageFlag TauntUsageDisabledReason;

		// Token: 0x04000303 RID: 771
		private readonly TextObject _nodeName;

		// Token: 0x04000304 RID: 772
		private InputKeyItemVM _shortcutKey;

		// Token: 0x04000305 RID: 773
		private MBBindingList<CheerBarkNodeItemVM> _subNodes;

		// Token: 0x04000306 RID: 774
		private string _cheerNameText;

		// Token: 0x04000307 RID: 775
		private string _typeAsString;

		// Token: 0x04000308 RID: 776
		private string _tauntVisualName;

		// Token: 0x04000309 RID: 777
		private string _selectedNodeText;

		// Token: 0x0400030A RID: 778
		private bool _isDisabled;

		// Token: 0x0400030B RID: 779
		private bool _isSelected;

		// Token: 0x0400030C RID: 780
		private bool _hasSubNodes;
	}
}
