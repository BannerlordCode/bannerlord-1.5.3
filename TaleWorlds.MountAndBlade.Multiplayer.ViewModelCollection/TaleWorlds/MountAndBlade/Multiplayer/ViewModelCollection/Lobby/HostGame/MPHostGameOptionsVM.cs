using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame
{
	// Token: 0x02000046 RID: 70
	public class MPHostGameOptionsVM : ViewModel
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x00014F11 File Offset: 0x00013111
		public MPHostGameOptionsVM(bool isInMission, MPCustomGameVM.CustomGameMode customGameMode = MPCustomGameVM.CustomGameMode.CustomServer)
		{
			this.IsInMission = isInMission;
			this.GeneralOptions = new MBBindingList<GenericHostGameOptionDataVM>();
			this._optionComparer = new MPHostGameOptionsVM.OptionPreferredIndexComparer();
			this._customGameMode = customGameMode;
			this.InitializeDefaultOptionList();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00014F4E File Offset: 0x0001314E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.GeneralOptions.ApplyActionOnAllItems(delegate(GenericHostGameOptionDataVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00014F80 File Offset: 0x00013180
		private void InitializeDefaultOptionList()
		{
			this.IsRefreshed = false;
			string text;
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				text = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			else
			{
				text = "Skirmish";
				MultiplayerOptions.Instance.SetValueForOptionWithMultipleSelectionFromText(MultiplayerOptions.OptionType.PremadeMatchGameMode, text);
			}
			this.OnGameModeChanged(text);
			foreach (GenericHostGameOptionDataVM genericHostGameOptionDataVM in this.GeneralOptions.ToList<GenericHostGameOptionDataVM>())
			{
				if ((genericHostGameOptionDataVM.OptionType == MultiplayerOptions.OptionType.GameType || genericHostGameOptionDataVM.OptionType == MultiplayerOptions.OptionType.PremadeMatchGameMode) && genericHostGameOptionDataVM is MultipleSelectionHostGameOptionDataVM)
				{
					(genericHostGameOptionDataVM as MultipleSelectionHostGameOptionDataVM).OnChangedSelection = new Action<MultipleSelectionHostGameOptionDataVM>(this.OnChangeSelected);
				}
			}
			this.IsRefreshed = true;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00015048 File Offset: 0x00013248
		private void OnChangeSelected(MultipleSelectionHostGameOptionDataVM option)
		{
			this.IsRefreshed = false;
			if (option.OptionType == MultiplayerOptions.OptionType.GameType || option.OptionType == MultiplayerOptions.OptionType.PremadeMatchGameMode)
			{
				this.OnGameModeChanged(MultiplayerOptions.Instance.GetMultiplayerOptionsList(MultiplayerOptions.OptionType.GameType)[option.Selector.SelectedIndex]);
			}
			this.IsRefreshed = true;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001509C File Offset: 0x0001329C
		private void OnGameModeChanged(string gameModeName)
		{
			this._hostGameItemsForNextTick.Clear();
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this.FillOptionsForCustomServer(gameModeName);
			}
			else if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.FillOptionsForPremadeGame();
			}
			MultipleSelectionHostGameOptionDataVM multipleSelectionHostGameOptionDataVM = this.GeneralOptions.First<GenericHostGameOptionDataVM>((GenericHostGameOptionDataVM o) => o.OptionType == MultiplayerOptions.OptionType.Map) as MultipleSelectionHostGameOptionDataVM;
			if (multipleSelectionHostGameOptionDataVM != null)
			{
				multipleSelectionHostGameOptionDataVM.RefreshList();
			}
			this.GeneralOptions.Sort(this._optionComparer);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00015120 File Offset: 0x00013320
		private void FillOptionsForCustomServer(string gameModeName)
		{
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionType != MultiplayerOptions.OptionType.PremadeMatchGameMode && optionType != MultiplayerOptions.OptionType.PremadeGameType && optionProperty != null)
				{
					int preferredIndex = (int)optionType;
					bool flag = optionProperty.ValidGameModes == null;
					if (optionProperty.ValidGameModes != null && optionProperty.ValidGameModes.Contains(gameModeName))
					{
						flag = true;
					}
					GenericHostGameOptionDataVM genericHostGameOptionDataVM = this.GeneralOptions.FirstOrDefault<GenericHostGameOptionDataVM>((GenericHostGameOptionDataVM o) => o.PreferredIndex == preferredIndex);
					if (flag)
					{
						if (genericHostGameOptionDataVM == null)
						{
							GenericHostGameOptionDataVM genericHostGameOptionDataVM2 = this.CreateOption(optionType, preferredIndex);
							this.GeneralOptions.Add(genericHostGameOptionDataVM2);
						}
						else
						{
							genericHostGameOptionDataVM.RefreshData();
						}
					}
					else if (genericHostGameOptionDataVM != null)
					{
						this.GeneralOptions.Remove(genericHostGameOptionDataVM);
					}
				}
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000151E8 File Offset: 0x000133E8
		private void FillOptionsForPremadeGame()
		{
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				bool flag = false;
				if (optionType == MultiplayerOptions.OptionType.ServerName || optionType == MultiplayerOptions.OptionType.GamePassword || optionType == MultiplayerOptions.OptionType.CultureTeam1 || optionType == MultiplayerOptions.OptionType.CultureTeam2 || optionType == MultiplayerOptions.OptionType.Map || optionType == MultiplayerOptions.OptionType.PremadeMatchGameMode || optionType == MultiplayerOptions.OptionType.PremadeGameType)
				{
					flag = true;
				}
				if (flag && optionProperty != null)
				{
					int preferredIndex = (int)optionType;
					GenericHostGameOptionDataVM genericHostGameOptionDataVM = this.GeneralOptions.FirstOrDefault<GenericHostGameOptionDataVM>((GenericHostGameOptionDataVM o) => o.PreferredIndex == preferredIndex);
					if (genericHostGameOptionDataVM == null)
					{
						GenericHostGameOptionDataVM genericHostGameOptionDataVM2 = this.CreateOption(optionType, preferredIndex);
						this.GeneralOptions.Add(genericHostGameOptionDataVM2);
					}
					else
					{
						genericHostGameOptionDataVM.RefreshData();
					}
				}
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00015288 File Offset: 0x00013488
		private GenericHostGameOptionDataVM CreateOption(MultiplayerOptions.OptionType type, int preferredIndex)
		{
			GenericHostGameOptionDataVM genericHostGameOptionDataVM = null;
			switch (this.GetSpecificHostGameOptionTypeOf(type))
			{
			case OptionsVM.OptionsDataType.BooleanOption:
				genericHostGameOptionDataVM = new BooleanHostGameOptionDataVM(type, preferredIndex);
				break;
			case OptionsVM.OptionsDataType.NumericOption:
				genericHostGameOptionDataVM = new NumericHostGameOptionDataVM(type, preferredIndex);
				break;
			case OptionsVM.OptionsDataType.MultipleSelectionOption:
				genericHostGameOptionDataVM = new MultipleSelectionHostGameOptionDataVM(type, preferredIndex);
				break;
			case OptionsVM.OptionsDataType.InputOption:
				genericHostGameOptionDataVM = new InputHostGameOptionDataVM(type, preferredIndex);
				break;
			}
			if (genericHostGameOptionDataVM == null)
			{
				Debug.FailedAssert("Item was not added to host game options because it has an invalid type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\HostGame\\MPHostGameOptionsVM.cs", "CreateOption", 218);
				return null;
			}
			return genericHostGameOptionDataVM;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00015300 File Offset: 0x00013500
		private OptionsVM.OptionsDataType GetSpecificHostGameOptionTypeOf(MultiplayerOptions.OptionType type)
		{
			MultiplayerOptionsProperty optionProperty = type.GetOptionProperty();
			switch (optionProperty.OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
				return OptionsVM.OptionsDataType.BooleanOption;
			case MultiplayerOptions.OptionValueType.Integer:
				return OptionsVM.OptionsDataType.NumericOption;
			case MultiplayerOptions.OptionValueType.Enum:
				return OptionsVM.OptionsDataType.MultipleSelectionOption;
			case MultiplayerOptions.OptionValueType.String:
				if (!optionProperty.HasMultipleSelections)
				{
					return OptionsVM.OptionsDataType.InputOption;
				}
				return OptionsVM.OptionsDataType.MultipleSelectionOption;
			default:
				return OptionsVM.OptionsDataType.None;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00015346 File Offset: 0x00013546
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x0001534E File Offset: 0x0001354E
		[DataSourceProperty]
		public MBBindingList<GenericHostGameOptionDataVM> GeneralOptions
		{
			get
			{
				return this._generalOptions;
			}
			set
			{
				if (value != this._generalOptions)
				{
					this._generalOptions = value;
					base.OnPropertyChangedWithValue<MBBindingList<GenericHostGameOptionDataVM>>(value, "GeneralOptions");
				}
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001536C File Offset: 0x0001356C
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x00015374 File Offset: 0x00013574
		[DataSourceProperty]
		public bool IsRefreshed
		{
			get
			{
				return this._isRefreshed;
			}
			set
			{
				if (value != this._isRefreshed)
				{
					this._isRefreshed = value;
					base.OnPropertyChangedWithValue(value, "IsRefreshed");
				}
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x00015392 File Offset: 0x00013592
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0001539A File Offset: 0x0001359A
		[DataSourceProperty]
		public bool IsInMission
		{
			get
			{
				return this._isInMission;
			}
			set
			{
				if (value != this._isInMission)
				{
					this._isInMission = value;
					base.OnPropertyChangedWithValue(value, "IsInMission");
				}
			}
		}

		// Token: 0x0400030D RID: 781
		private List<GenericHostGameOptionDataVM> _hostGameItemsForNextTick = new List<GenericHostGameOptionDataVM>();

		// Token: 0x0400030E RID: 782
		private MPHostGameOptionsVM.OptionPreferredIndexComparer _optionComparer;

		// Token: 0x0400030F RID: 783
		private MPCustomGameVM.CustomGameMode _customGameMode;

		// Token: 0x04000310 RID: 784
		private bool _isRefreshed;

		// Token: 0x04000311 RID: 785
		private bool _isInMission;

		// Token: 0x04000312 RID: 786
		private MBBindingList<GenericHostGameOptionDataVM> _generalOptions;

		// Token: 0x020000FD RID: 253
		private class OptionPreferredIndexComparer : IComparer<GenericHostGameOptionDataVM>
		{
			// Token: 0x0600121A RID: 4634 RVA: 0x00039314 File Offset: 0x00037514
			public int Compare(GenericHostGameOptionDataVM x, GenericHostGameOptionDataVM y)
			{
				return x.PreferredIndex.CompareTo(y.PreferredIndex);
			}
		}
	}
}
