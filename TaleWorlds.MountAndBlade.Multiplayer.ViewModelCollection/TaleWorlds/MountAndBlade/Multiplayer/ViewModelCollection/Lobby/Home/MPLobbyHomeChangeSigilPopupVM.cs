using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x0200004F RID: 79
	public class MPLobbyHomeChangeSigilPopupVM : ViewModel
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00015F18 File Offset: 0x00014118
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00015F20 File Offset: 0x00014120
		public MPLobbyCosmeticSigilItemVM SelectedSigil { get; private set; }

		// Token: 0x060006CF RID: 1743 RVA: 0x00015F29 File Offset: 0x00014129
		public MPLobbyHomeChangeSigilPopupVM(Action<MPLobbyCosmeticSigilItemVM> onItemObtainRequested)
		{
			this._onItemObtainRequested = onItemObtainRequested;
			this.RefreshValues();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00015F40 File Offset: 0x00014140
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=7R0i82Nw}Change Sigil", null).ToString();
			this.ChangeText = new TextObject("{=Ba50zU7Z}Change", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00015F98 File Offset: 0x00014198
		private void RefreshSigilList()
		{
			this.SigilList = new MBBindingList<MPLobbyCosmeticSigilItemVM>();
			this.SelectedSigil = null;
			MBReadOnlyList<CosmeticElement> cosmeticElementsList = CosmeticsManager.CosmeticElementsList;
			IReadOnlyList<string> ownedCosmetics = NetworkMain.GameClient.OwnedCosmetics;
			for (int i = 0; i < cosmeticElementsList.Count; i++)
			{
				if (cosmeticElementsList[i].Type == CosmeticsManager.CosmeticType.Sigil)
				{
					SigilCosmeticElement sigilCosmeticElement = cosmeticElementsList[i] as SigilCosmeticElement;
					MPLobbyCosmeticSigilItemVM mplobbyCosmeticSigilItemVM = new MPLobbyCosmeticSigilItemVM(new Banner(sigilCosmeticElement.BannerCode).GetIconMeshId(), (int)sigilCosmeticElement.Rarity, sigilCosmeticElement.Cost, sigilCosmeticElement.Id);
					mplobbyCosmeticSigilItemVM.IsUnlocked = ownedCosmetics.Contains(sigilCosmeticElement.Id) || sigilCosmeticElement.IsFree;
					this.SigilList.Add(mplobbyCosmeticSigilItemVM);
				}
			}
			this.IsUsingClanSigil = NetworkMain.GameClient.PlayerData.IsUsingClanSigil;
			this.SelectPlayerSigil(NetworkMain.GameClient.PlayerData);
			this.Loot = NetworkMain.GameClient.PlayerData.Gold;
			this.SigilList.Sort(new MPLobbyHomeChangeSigilPopupVM.SigilItemUnlockStatusComparer());
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00016094 File Offset: 0x00014294
		private void SelectPlayerSigil(PlayerData playerData)
		{
			int playerBannerID = new Banner(playerData.Sigil).GetIconMeshId();
			this.OnSigilSelected(this.SigilList.First<MPLobbyCosmeticSigilItemVM>((MPLobbyCosmeticSigilItemVM s) => s.IconID == playerBannerID));
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000160DA File Offset: 0x000142DA
		public void Open()
		{
			this.IsInClan = NetworkMain.GameClient.IsInClan;
			this.IsEnabled = true;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000160F3 File Offset: 0x000142F3
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000160FC File Offset: 0x000142FC
		public async void ExecuteChangeSigil()
		{
			TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.ChangeSigil(this.SelectedSigil.CosmeticID).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				NetworkMain.GameClient.PlayerData.IsUsingClanSigil = this.IsUsingClanSigil;
				this.IsEnabled = false;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00016135 File Offset: 0x00014335
		private void OnSigilObtainRequested(MPLobbyCosmeticSigilItemVM sigilItem)
		{
			this._onItemObtainRequested(sigilItem);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00016143 File Offset: 0x00014343
		private void OnSigilSelected(MPLobbyCosmeticSigilItemVM sigilItem)
		{
			if (sigilItem != this.SelectedSigil)
			{
				if (this.SelectedSigil != null)
				{
					this.SelectedSigil.IsUsed = false;
				}
				this.SelectedSigil = sigilItem;
				if (this.SelectedSigil != null)
				{
					this.SelectedSigil.IsUsed = true;
				}
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001617D File Offset: 0x0001437D
		public void OnLootUpdated(int finalLoot)
		{
			this.Loot = finalLoot;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00016186 File Offset: 0x00014386
		private void OnIsEnabledChanged()
		{
			if (this.IsEnabled)
			{
				this.RefreshSigilList();
				MPLobbyCosmeticSigilItemVM.SetOnObtainRequestedCallback(new Action<MPLobbyCosmeticSigilItemVM>(this.OnSigilObtainRequested));
				MPLobbyCosmeticSigilItemVM.SetOnSelectionCallback(new Action<MPLobbyCosmeticSigilItemVM>(this.OnSigilSelected));
				return;
			}
			MPLobbyCosmeticSigilItemVM.ResetOnObtainRequestedCallback();
			MPLobbyCosmeticSigilItemVM.ResetOnSelectionCallback();
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000161C3 File Offset: 0x000143C3
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000161EC File Offset: 0x000143EC
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x000161FB File Offset: 0x000143FB
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001620A File Offset: 0x0001440A
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00016212 File Offset: 0x00014412
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0001622F File Offset: 0x0001442F
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00016237 File Offset: 0x00014437
		[DataSourceProperty]
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
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00016254 File Offset: 0x00014454
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0001625C File Offset: 0x0001445C
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
					this.OnIsEnabledChanged();
				}
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00016280 File Offset: 0x00014480
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x00016288 File Offset: 0x00014488
		[DataSourceProperty]
		public bool IsLoading
		{
			get
			{
				return this._isLoading;
			}
			set
			{
				if (value != this._isLoading)
				{
					this._isLoading = value;
					base.OnPropertyChangedWithValue(value, "IsLoading");
				}
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000162A6 File Offset: 0x000144A6
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x000162AE File Offset: 0x000144AE
		[DataSourceProperty]
		public bool IsInClan
		{
			get
			{
				return this._isInClan;
			}
			set
			{
				if (value != this._isInClan)
				{
					this._isInClan = value;
					base.OnPropertyChangedWithValue(value, "IsInClan");
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000162CC File Offset: 0x000144CC
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x000162D4 File Offset: 0x000144D4
		[DataSourceProperty]
		public bool IsUsingClanSigil
		{
			get
			{
				return this._isUsingClanSigil;
			}
			set
			{
				if (value != this._isUsingClanSigil)
				{
					this._isUsingClanSigil = value;
					base.OnPropertyChangedWithValue(value, "IsUsingClanSigil");
				}
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x000162F2 File Offset: 0x000144F2
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x000162FA File Offset: 0x000144FA
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

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001631D File Offset: 0x0001451D
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x00016325 File Offset: 0x00014525
		[DataSourceProperty]
		public string ChangeText
		{
			get
			{
				return this._changeText;
			}
			set
			{
				if (value != this._changeText)
				{
					this._changeText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChangeText");
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00016348 File Offset: 0x00014548
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x00016350 File Offset: 0x00014550
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00016373 File Offset: 0x00014573
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0001637B File Offset: 0x0001457B
		[DataSourceProperty]
		public int Loot
		{
			get
			{
				return this._loot;
			}
			set
			{
				if (value != this._loot)
				{
					this._loot = value;
					base.OnPropertyChangedWithValue(value, "Loot");
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00016399 File Offset: 0x00014599
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x000163A1 File Offset: 0x000145A1
		[DataSourceProperty]
		public MBBindingList<MPLobbyCosmeticSigilItemVM> SigilList
		{
			get
			{
				return this._sigilList;
			}
			set
			{
				if (value != this._sigilList)
				{
					this._sigilList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyCosmeticSigilItemVM>>(value, "SigilList");
				}
			}
		}

		// Token: 0x04000333 RID: 819
		private readonly Action<MPLobbyCosmeticSigilItemVM> _onItemObtainRequested;

		// Token: 0x04000335 RID: 821
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000336 RID: 822
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000337 RID: 823
		private bool _isEnabled;

		// Token: 0x04000338 RID: 824
		private bool _isLoading;

		// Token: 0x04000339 RID: 825
		private bool _isInClan;

		// Token: 0x0400033A RID: 826
		private bool _isUsingClanSigil;

		// Token: 0x0400033B RID: 827
		private string _titleText;

		// Token: 0x0400033C RID: 828
		private string _changeText;

		// Token: 0x0400033D RID: 829
		private string _cancelText;

		// Token: 0x0400033E RID: 830
		private int _loot;

		// Token: 0x0400033F RID: 831
		private MBBindingList<MPLobbyCosmeticSigilItemVM> _sigilList;

		// Token: 0x02000102 RID: 258
		private class SigilItemUnlockStatusComparer : IComparer<MPLobbyCosmeticSigilItemVM>
		{
			// Token: 0x06001226 RID: 4646 RVA: 0x000394CC File Offset: 0x000376CC
			public int Compare(MPLobbyCosmeticSigilItemVM x, MPLobbyCosmeticSigilItemVM y)
			{
				return y.IsUnlocked.CompareTo(x.IsUnlocked);
			}
		}
	}
}
