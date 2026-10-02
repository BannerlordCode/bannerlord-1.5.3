using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002B RID: 43
	public class PartyTradeVM : ViewModel
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000352 RID: 850 RVA: 0x00016BC0 File Offset: 0x00014DC0
		// (remove) Token: 0x06000353 RID: 851 RVA: 0x00016BF4 File Offset: 0x00014DF4
		public static event Action RemoveZeroCounts;

		// Token: 0x06000354 RID: 852 RVA: 0x00016C28 File Offset: 0x00014E28
		public PartyTradeVM(PartyScreenLogic partyScreenLogic, TroopRosterElement troopRoster, PartyScreenLogic.PartyRosterSide side, bool isTransfarable, bool isPrisoner, Action<int, bool> onApplyTransaction)
		{
			this._partyScreenLogic = partyScreenLogic;
			this._referenceTroopRoster = troopRoster;
			this._side = side;
			this._onApplyTransaction = onApplyTransaction;
			this._otherSide = ((side == PartyScreenLogic.PartyRosterSide.Right) ? PartyScreenLogic.PartyRosterSide.Left : PartyScreenLogic.PartyRosterSide.Right);
			this.IsTransfarable = isTransfarable;
			this._isPrisoner = isPrisoner;
			this.TakeHint = new HintViewModel(GameTexts.FindText("str_take", null), null);
			this.GiveHint = new HintViewModel(GameTexts.FindText("str_give", null), null);
			this.UpdateTroopData(troopRoster, side, true);
			this.RefreshValues();
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00016CBA File Offset: 0x00014EBA
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ThisStockLbl = GameTexts.FindText("str_party_your_party", null).ToString();
			this.TotalStockLbl = GameTexts.FindText("str_party_total_units", null).ToString();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00016CF0 File Offset: 0x00014EF0
		public void UpdateTroopData(TroopRosterElement troopRoster, PartyScreenLogic.PartyRosterSide side, bool forceUpdate = true)
		{
			if (side != PartyScreenLogic.PartyRosterSide.Left && side != PartyScreenLogic.PartyRosterSide.Right)
			{
				Debug.FailedAssert("Troop has to be either from left or right side", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Party\\PartyTradeVM.cs", "UpdateTroopData", 54);
				return;
			}
			TroopRosterElement? troopRosterElement = null;
			TroopRosterElement? troopRosterElement2 = null;
			troopRosterElement = new TroopRosterElement?(troopRoster);
			troopRosterElement2 = this.FindTroopFromSide(troopRoster.Character, this._otherSide, this._isPrisoner);
			this.InitialThisStock = ((troopRosterElement != null) ? troopRosterElement.GetValueOrDefault().Number : 0);
			this.InitialOtherStock = ((troopRosterElement2 != null) ? troopRosterElement2.GetValueOrDefault().Number : 0);
			this.TotalStock = this.InitialThisStock + this.InitialOtherStock;
			this.ThisStock = this.InitialThisStock;
			this.OtherStock = this.InitialOtherStock;
			if (forceUpdate)
			{
				this.ThisStockUpdated();
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00016DC4 File Offset: 0x00014FC4
		public TroopRosterElement? FindTroopFromSide(CharacterObject character, PartyScreenLogic.PartyRosterSide side, bool isPrisoner)
		{
			TroopRosterElement? troopRosterElement = null;
			TroopRoster[] array = (isPrisoner ? this._partyScreenLogic.PrisonerRosters : this._partyScreenLogic.MemberRosters);
			int num = array[(int)side].FindIndexOfTroop(character);
			if (num >= 0)
			{
				troopRosterElement = new TroopRosterElement?(array[(int)side].GetElementCopyAtIndex(num));
			}
			return troopRosterElement;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00016E14 File Offset: 0x00015014
		private void ThisStockUpdated()
		{
			this.ExecuteApplyTransaction();
			this.OtherStock = this.TotalStock - this.ThisStock;
			this.IsThisStockIncreasable = this.OtherStock > 0;
			this.IsOtherStockIncreasable = this.OtherStock < this.TotalStock && this.IsTransfarable;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00016E66 File Offset: 0x00015066
		public void ExecuteIncreasePlayerStock()
		{
			if (this.OtherStock > 0)
			{
				this.ThisStock++;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00016E7F File Offset: 0x0001507F
		public void ExecuteIncreaseOtherStock()
		{
			if (this.OtherStock < this.TotalStock)
			{
				this.ThisStock--;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00016E9D File Offset: 0x0001509D
		public void ExecuteReset()
		{
			this.OtherStock = this.InitialOtherStock;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00016EAC File Offset: 0x000150AC
		public void ExecuteApplyTransaction()
		{
			int num = this.ThisStock - this.InitialThisStock;
			bool flag = (num >= 0 && this._side == PartyScreenLogic.PartyRosterSide.Right) || (num <= 0 && this._side == PartyScreenLogic.PartyRosterSide.Left);
			if (num == 0 || this._onApplyTransaction == null)
			{
				return;
			}
			if (num < 0)
			{
				PartyScreenLogic.PartyRosterSide otherSide = this._otherSide;
			}
			else
			{
				PartyScreenLogic.PartyRosterSide side = this._side;
			}
			int num2 = MathF.Abs(num);
			this._onApplyTransaction(num2, flag);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00016F1C File Offset: 0x0001511C
		public void ExecuteRemoveZeroCounts()
		{
			Action removeZeroCounts = PartyTradeVM.RemoveZeroCounts;
			if (removeZeroCounts == null)
			{
				return;
			}
			removeZeroCounts();
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00016F2D File Offset: 0x0001512D
		// (set) Token: 0x0600035F RID: 863 RVA: 0x00016F35 File Offset: 0x00015135
		[DataSourceProperty]
		public bool IsTransfarable
		{
			get
			{
				return this._isTransfarable;
			}
			set
			{
				if (value != this._isTransfarable)
				{
					this._isTransfarable = value;
					base.OnPropertyChangedWithValue(value, "IsTransfarable");
				}
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00016F53 File Offset: 0x00015153
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00016F5B File Offset: 0x0001515B
		[DataSourceProperty]
		public string ThisStockLbl
		{
			get
			{
				return this._thisStockLbl;
			}
			set
			{
				if (value != this._thisStockLbl)
				{
					this._thisStockLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ThisStockLbl");
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00016F7E File Offset: 0x0001517E
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00016F86 File Offset: 0x00015186
		[DataSourceProperty]
		public string TotalStockLbl
		{
			get
			{
				return this._totalStockLbl;
			}
			set
			{
				if (value != this._totalStockLbl)
				{
					this._totalStockLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalStockLbl");
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00016FA9 File Offset: 0x000151A9
		// (set) Token: 0x06000365 RID: 869 RVA: 0x00016FB1 File Offset: 0x000151B1
		[DataSourceProperty]
		public int ThisStock
		{
			get
			{
				return this._thisStock;
			}
			set
			{
				if (value != this._thisStock)
				{
					this._thisStock = value;
					base.OnPropertyChangedWithValue(value, "ThisStock");
					this.ThisStockUpdated();
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00016FD5 File Offset: 0x000151D5
		// (set) Token: 0x06000367 RID: 871 RVA: 0x00016FDD File Offset: 0x000151DD
		[DataSourceProperty]
		public int InitialThisStock
		{
			get
			{
				return this._initialThisStock;
			}
			set
			{
				if (value != this._initialThisStock)
				{
					this._initialThisStock = value;
					base.OnPropertyChangedWithValue(value, "InitialThisStock");
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00016FFB File Offset: 0x000151FB
		// (set) Token: 0x06000369 RID: 873 RVA: 0x00017003 File Offset: 0x00015203
		[DataSourceProperty]
		public int OtherStock
		{
			get
			{
				return this._otherStock;
			}
			set
			{
				if (value != this._otherStock)
				{
					this._otherStock = value;
					base.OnPropertyChangedWithValue(value, "OtherStock");
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00017021 File Offset: 0x00015221
		// (set) Token: 0x0600036B RID: 875 RVA: 0x00017029 File Offset: 0x00015229
		[DataSourceProperty]
		public int InitialOtherStock
		{
			get
			{
				return this._initialOtherStock;
			}
			set
			{
				if (value != this._initialOtherStock)
				{
					this._initialOtherStock = value;
					base.OnPropertyChangedWithValue(value, "InitialOtherStock");
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00017047 File Offset: 0x00015247
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0001704F File Offset: 0x0001524F
		[DataSourceProperty]
		public int TotalStock
		{
			get
			{
				return this._totalStock;
			}
			set
			{
				if (value != this._totalStock)
				{
					this._totalStock = value;
					base.OnPropertyChangedWithValue(value, "TotalStock");
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0001706D File Offset: 0x0001526D
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00017075 File Offset: 0x00015275
		[DataSourceProperty]
		public bool IsThisStockIncreasable
		{
			get
			{
				return this._isThisStockIncreasable;
			}
			set
			{
				if (value != this._isThisStockIncreasable)
				{
					this._isThisStockIncreasable = value;
					base.OnPropertyChangedWithValue(value, "IsThisStockIncreasable");
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00017093 File Offset: 0x00015293
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0001709B File Offset: 0x0001529B
		[DataSourceProperty]
		public bool IsOtherStockIncreasable
		{
			get
			{
				return this._isOtherStockIncreasable;
			}
			set
			{
				if (value != this._isOtherStockIncreasable)
				{
					this._isOtherStockIncreasable = value;
					base.OnPropertyChangedWithValue(value, "IsOtherStockIncreasable");
				}
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000372 RID: 882 RVA: 0x000170B9 File Offset: 0x000152B9
		// (set) Token: 0x06000373 RID: 883 RVA: 0x000170C1 File Offset: 0x000152C1
		[DataSourceProperty]
		public HintViewModel TakeHint
		{
			get
			{
				return this._takeHint;
			}
			set
			{
				if (value != this._takeHint)
				{
					this._takeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TakeHint");
				}
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000170DF File Offset: 0x000152DF
		// (set) Token: 0x06000375 RID: 885 RVA: 0x000170E7 File Offset: 0x000152E7
		[DataSourceProperty]
		public HintViewModel GiveHint
		{
			get
			{
				return this._giveHint;
			}
			set
			{
				if (value != this._giveHint)
				{
					this._giveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GiveHint");
				}
			}
		}

		// Token: 0x0400017C RID: 380
		private readonly PartyScreenLogic _partyScreenLogic;

		// Token: 0x0400017D RID: 381
		private readonly Action<int, bool> _onApplyTransaction;

		// Token: 0x0400017E RID: 382
		private readonly bool _isPrisoner;

		// Token: 0x0400017F RID: 383
		private TroopRosterElement _referenceTroopRoster;

		// Token: 0x04000180 RID: 384
		private readonly PartyScreenLogic.PartyRosterSide _side;

		// Token: 0x04000181 RID: 385
		private PartyScreenLogic.PartyRosterSide _otherSide;

		// Token: 0x04000182 RID: 386
		private bool _isTransfarable;

		// Token: 0x04000183 RID: 387
		private string _thisStockLbl;

		// Token: 0x04000184 RID: 388
		private string _totalStockLbl;

		// Token: 0x04000185 RID: 389
		private int _thisStock = -1;

		// Token: 0x04000186 RID: 390
		private int _initialThisStock;

		// Token: 0x04000187 RID: 391
		private int _otherStock;

		// Token: 0x04000188 RID: 392
		private int _initialOtherStock;

		// Token: 0x04000189 RID: 393
		private int _totalStock;

		// Token: 0x0400018A RID: 394
		private bool _isThisStockIncreasable;

		// Token: 0x0400018B RID: 395
		private bool _isOtherStockIncreasable;

		// Token: 0x0400018C RID: 396
		private HintViewModel _takeHint;

		// Token: 0x0400018D RID: 397
		private HintViewModel _giveHint;
	}
}
