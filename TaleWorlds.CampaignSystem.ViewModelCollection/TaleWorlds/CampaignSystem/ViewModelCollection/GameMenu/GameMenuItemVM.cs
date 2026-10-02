using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x020000A2 RID: 162
	public class GameMenuItemVM : ViewModel
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0003FA2E File Offset: 0x0003DC2E
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x0003FA36 File Offset: 0x0003DC36
		public string OptionID { get; private set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0003FA3F File Offset: 0x0003DC3F
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x0003FA47 File Offset: 0x0003DC47
		public GameMenuOption GameMenuOption { get; private set; }

		// Token: 0x06000F49 RID: 3913 RVA: 0x0003FA50 File Offset: 0x0003DC50
		public GameMenuItemVM()
		{
			this.ItemHint = new HintViewModel();
			this.Quests = new MBBindingList<QuestMarkerVM>();
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0003FA7C File Offset: 0x0003DC7C
		public void InitializeWith(in GameMenuItemVM.GameMenuItemCreationData data)
		{
			this.GameMenuOption = data.GameMenuOption;
			this.Index = data.Index;
			this._menuContext = data.MenuContext;
			this._itemType = (int)data.Type;
			this._tooltip = data.Tooltip;
			this._nonWaitText = data.Text;
			this._waitText = data.Text2;
			this.Item = this._nonWaitText.ToString();
			this.ItemHint.HintText = this._tooltip;
			this.OptionLeaveType = data.GameMenuOption.OptionLeaveType.ToString();
			this.OptionID = data.GameMenuOption.IdString;
			if (data.OptionQuestData != this._questFlags)
			{
				this.Quests.Clear();
				for (int i = 0; i < GameMenuOption.IssueQuestFlagsValues.Length; i++)
				{
					GameMenuOption.IssueQuestFlags issueQuestFlags = GameMenuOption.IssueQuestFlagsValues[i];
					if (issueQuestFlags != GameMenuOption.IssueQuestFlags.None && (data.OptionQuestData & issueQuestFlags) != GameMenuOption.IssueQuestFlags.None)
					{
						CampaignUIHelper.IssueQuestFlags issueQuestFlags2 = (CampaignUIHelper.IssueQuestFlags)issueQuestFlags;
						this.Quests.Add(new QuestMarkerVM(issueQuestFlags2, null, null));
					}
				}
				this._questFlags = data.OptionQuestData;
			}
			this.ShortcutKey = ((data.ShortcutKey != null) ? InputKeyItemVM.CreateFromGameKey(data.ShortcutKey, true) : null);
			this.RefreshValues();
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0003FBB3 File Offset: 0x0003DDB3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Refresh();
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0003FBC1 File Offset: 0x0003DDC1
		public void ExecuteAction()
		{
			MenuContext menuContext = this._menuContext;
			if (menuContext == null)
			{
				return;
			}
			menuContext.InvokeConsequence(this.Index);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0003FBD9 File Offset: 0x0003DDD9
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this.ShortcutKey != null)
			{
				this.ShortcutKey.OnFinalize();
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
		public void Refresh()
		{
			int itemType = this._itemType;
			if (itemType != 0)
			{
				int num = itemType - 1;
			}
			this.IsWaitActive = Campaign.Current.GameMenuManager.GetVirtualMenuIsWaitActive(this._menuContext);
			this.IsEnabled = Campaign.Current.GameMenuManager.GetVirtualMenuOptionIsEnabled(this._menuContext, this.Index);
			this.ItemHint.HintText = Campaign.Current.GameMenuManager.GetVirtualMenuOptionTooltip(this._menuContext, this.Index);
			this.GameMenuStringId = this._menuContext.GameMenu.StringId;
			if (PlayerEncounter.Battle != null)
			{
				this.BattleSize = PlayerEncounter.Battle.AttackerSide.TroopCount + PlayerEncounter.Battle.DefenderSide.TroopCount;
			}
			else
			{
				this.BattleSize = -1;
			}
			MapEvent battle = PlayerEncounter.Battle;
			this.IsNavalBattle = battle != null && battle.IsNavalMapEvent;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0003FCD8 File Offset: 0x0003DED8
		public void UpdateWith(GameMenuItemVM newItem)
		{
			this.Item = newItem.Item;
			this.OptionLeaveType = newItem.OptionLeaveType;
			this.ItemHint = newItem.ItemHint;
			this.Quests = newItem.Quests;
			this.Index = newItem.Index;
			this.GameMenuOption = newItem.GameMenuOption;
			this.Refresh();
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0003FD33 File Offset: 0x0003DF33
		// (set) Token: 0x06000F51 RID: 3921 RVA: 0x0003FD3B File Offset: 0x0003DF3B
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x0003FD59 File Offset: 0x0003DF59
		// (set) Token: 0x06000F53 RID: 3923 RVA: 0x0003FD61 File Offset: 0x0003DF61
		[DataSourceProperty]
		public string OptionLeaveType
		{
			get
			{
				return this._optionLeaveType;
			}
			set
			{
				if (value != this._optionLeaveType)
				{
					this._optionLeaveType = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionLeaveType");
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0003FD84 File Offset: 0x0003DF84
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0003FD8C File Offset: 0x0003DF8C
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0003FDAA File Offset: 0x0003DFAA
		// (set) Token: 0x06000F57 RID: 3927 RVA: 0x0003FDB2 File Offset: 0x0003DFB2
		[DataSourceProperty]
		public bool IsWaitActive
		{
			get
			{
				return this._isWaitActive;
			}
			set
			{
				if (value != this._isWaitActive)
				{
					this._isWaitActive = value;
					base.OnPropertyChangedWithValue(value, "IsWaitActive");
					this.Item = (value ? this._waitText.ToString() : this._nonWaitText.ToString());
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0003FDF1 File Offset: 0x0003DFF1
		// (set) Token: 0x06000F59 RID: 3929 RVA: 0x0003FDF9 File Offset: 0x0003DFF9
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

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0003FE17 File Offset: 0x0003E017
		// (set) Token: 0x06000F5B RID: 3931 RVA: 0x0003FE1F File Offset: 0x0003E01F
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0003FE3D File Offset: 0x0003E03D
		// (set) Token: 0x06000F5D RID: 3933 RVA: 0x0003FE45 File Offset: 0x0003E045
		[DataSourceProperty]
		public HintViewModel ItemHint
		{
			get
			{
				return this._itemHint;
			}
			set
			{
				if (value != this._itemHint)
				{
					this._itemHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ItemHint");
				}
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0003FE63 File Offset: 0x0003E063
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x0003FE6B File Offset: 0x0003E06B
		[DataSourceProperty]
		public HintViewModel QuestHint
		{
			get
			{
				return this._questHint;
			}
			set
			{
				if (value != this._questHint)
				{
					this._questHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "QuestHint");
				}
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0003FE89 File Offset: 0x0003E089
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x0003FE91 File Offset: 0x0003E091
		[DataSourceProperty]
		public HintViewModel IssueHint
		{
			get
			{
				return this._issueHint;
			}
			set
			{
				if (value != this._issueHint)
				{
					this._issueHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IssueHint");
				}
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0003FEAF File Offset: 0x0003E0AF
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x0003FEB7 File Offset: 0x0003E0B7
		[DataSourceProperty]
		public string GameMenuStringId
		{
			get
			{
				return this._gameMenuStringId;
			}
			set
			{
				if (value != this._gameMenuStringId)
				{
					this._gameMenuStringId = value;
					base.OnPropertyChangedWithValue<string>(value, "GameMenuStringId");
				}
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0003FEDA File Offset: 0x0003E0DA
		// (set) Token: 0x06000F65 RID: 3941 RVA: 0x0003FEE2 File Offset: 0x0003E0E2
		[DataSourceProperty]
		public string Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue<string>(value, "Item");
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0003FF05 File Offset: 0x0003E105
		// (set) Token: 0x06000F67 RID: 3943 RVA: 0x0003FF0D File Offset: 0x0003E10D
		[DataSourceProperty]
		public int BattleSize
		{
			get
			{
				return this._battleSize;
			}
			set
			{
				if (value != this._battleSize)
				{
					this._battleSize = value;
					base.OnPropertyChangedWithValue(value, "BattleSize");
				}
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0003FF2B File Offset: 0x0003E12B
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x0003FF33 File Offset: 0x0003E133
		[DataSourceProperty]
		public bool IsNavalBattle
		{
			get
			{
				return this._isNavalBattle;
			}
			set
			{
				if (value != this._isNavalBattle)
				{
					this._isNavalBattle = value;
					base.OnPropertyChangedWithValue(value, "IsNavalBattle");
				}
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0003FF51 File Offset: 0x0003E151
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x0003FF59 File Offset: 0x0003E159
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

		// Token: 0x040006DE RID: 1758
		private MenuContext _menuContext;

		// Token: 0x040006DF RID: 1759
		public int Index;

		// Token: 0x040006E0 RID: 1760
		private TextObject _nonWaitText;

		// Token: 0x040006E1 RID: 1761
		private TextObject _waitText;

		// Token: 0x040006E2 RID: 1762
		private TextObject _tooltip;

		// Token: 0x040006E4 RID: 1764
		private GameMenuOption.IssueQuestFlags _questFlags;

		// Token: 0x040006E5 RID: 1765
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x040006E6 RID: 1766
		private int _itemType = -1;

		// Token: 0x040006E7 RID: 1767
		private bool _isWaitActive;

		// Token: 0x040006E8 RID: 1768
		private bool _isEnabled;

		// Token: 0x040006E9 RID: 1769
		private HintViewModel _itemHint;

		// Token: 0x040006EA RID: 1770
		private HintViewModel _questHint;

		// Token: 0x040006EB RID: 1771
		private HintViewModel _issueHint;

		// Token: 0x040006EC RID: 1772
		private bool _isHighlightEnabled;

		// Token: 0x040006ED RID: 1773
		private string _optionLeaveType;

		// Token: 0x040006EE RID: 1774
		private string _gameMenuStringId;

		// Token: 0x040006EF RID: 1775
		private string _item;

		// Token: 0x040006F0 RID: 1776
		private int _battleSize = -1;

		// Token: 0x040006F1 RID: 1777
		private bool _isNavalBattle;

		// Token: 0x040006F2 RID: 1778
		private InputKeyItemVM _shortcutKey;

		// Token: 0x02000216 RID: 534
		public readonly struct GameMenuItemCreationData
		{
			// Token: 0x17000C2A RID: 3114
			// (get) Token: 0x06002584 RID: 9604 RVA: 0x00082000 File Offset: 0x00080200
			public string OptionID
			{
				get
				{
					return this.GameMenuOption.IdString;
				}
			}

			// Token: 0x06002585 RID: 9605 RVA: 0x00082010 File Offset: 0x00080210
			public GameMenuItemCreationData(MenuContext menuContext, int index, TextObject text, TextObject text2, TextObject tooltip, GameMenu.MenuAndOptionType type, GameMenuOption.IssueQuestFlags questFlags, GameMenuOption gameMenuOption, GameKey shortcutKey)
			{
				this.OptionQuestData = questFlags;
				this.MenuContext = menuContext;
				this.Index = index;
				this.Text = text;
				this.Text2 = text2;
				this.Tooltip = tooltip;
				this.Type = type;
				this.GameMenuOption = gameMenuOption;
				this.ShortcutKey = shortcutKey;
			}

			// Token: 0x040011FA RID: 4602
			public readonly MenuContext MenuContext;

			// Token: 0x040011FB RID: 4603
			public readonly int Index;

			// Token: 0x040011FC RID: 4604
			public readonly TextObject Text;

			// Token: 0x040011FD RID: 4605
			public readonly TextObject Text2;

			// Token: 0x040011FE RID: 4606
			public readonly TextObject Tooltip;

			// Token: 0x040011FF RID: 4607
			public readonly GameMenu.MenuAndOptionType Type;

			// Token: 0x04001200 RID: 4608
			public readonly GameMenuOption.IssueQuestFlags OptionQuestData;

			// Token: 0x04001201 RID: 4609
			public readonly GameMenuOption GameMenuOption;

			// Token: 0x04001202 RID: 4610
			public readonly GameKey ShortcutKey;
		}
	}
}
