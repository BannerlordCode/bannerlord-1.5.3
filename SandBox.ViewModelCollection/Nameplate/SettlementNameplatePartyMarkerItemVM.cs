using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001E RID: 30
	public class SettlementNameplatePartyMarkerItemVM : ViewModel
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000CC51 File Offset: 0x0000AE51
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000CC59 File Offset: 0x0000AE59
		public MobileParty Party { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000CC62 File Offset: 0x0000AE62
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000CC6A File Offset: 0x0000AE6A
		public int SortIndex { get; private set; }

		// Token: 0x060002DF RID: 735 RVA: 0x0000CC74 File Offset: 0x0000AE74
		public SettlementNameplatePartyMarkerItemVM(MobileParty mobileParty)
		{
			this.Party = mobileParty;
			this.IsBandit = mobileParty.IsBandit;
			Clan actualClan = mobileParty.ActualClan;
			this.HasBloodFeud = actualClan != null && actualClan.HasBloodFeudWithPlayer;
			if (mobileParty.IsCaravan)
			{
				this.IsCaravan = true;
				this.SortIndex = 1;
				return;
			}
			if (mobileParty.IsLordParty && mobileParty.LeaderHero != null)
			{
				this.IsLord = true;
				Clan actualClan2 = mobileParty.ActualClan;
				this.Visual = new BannerImageIdentifierVM((actualClan2 != null) ? actualClan2.Banner : null, true);
				this.SortIndex = 0;
				return;
			}
			this.IsDefault = true;
			this.SortIndex = 2;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000CD14 File Offset: 0x0000AF14
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000CD1C File Offset: 0x0000AF1C
		public BannerImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000CD3A File Offset: 0x0000AF3A
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000CD42 File Offset: 0x0000AF42
		public bool IsCaravan
		{
			get
			{
				return this._isCaravan;
			}
			set
			{
				if (value != this._isCaravan)
				{
					this._isCaravan = value;
					base.OnPropertyChangedWithValue(value, "IsCaravan");
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000CD60 File Offset: 0x0000AF60
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000CD68 File Offset: 0x0000AF68
		public bool IsLord
		{
			get
			{
				return this._isLord;
			}
			set
			{
				if (value != this._isLord)
				{
					this._isLord = value;
					base.OnPropertyChangedWithValue(value, "IsLord");
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000CD86 File Offset: 0x0000AF86
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000CD8E File Offset: 0x0000AF8E
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
			set
			{
				if (value != this._isDefault)
				{
					this._isDefault = value;
					base.OnPropertyChangedWithValue(value, "IsDefault");
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000CDAC File Offset: 0x0000AFAC
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000CDB4 File Offset: 0x0000AFB4
		public bool IsBandit
		{
			get
			{
				return this._isBandit;
			}
			set
			{
				if (value != this._isBandit)
				{
					this._isBandit = value;
					base.OnPropertyChangedWithValue(value, "IsBandit");
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000CDD2 File Offset: 0x0000AFD2
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000CDDA File Offset: 0x0000AFDA
		public bool HasBloodFeud
		{
			get
			{
				return this._hasBloodFeud;
			}
			set
			{
				if (value != this._hasBloodFeud)
				{
					this._hasBloodFeud = value;
					base.OnPropertyChangedWithValue(value, "HasBloodFeud");
				}
			}
		}

		// Token: 0x0400016B RID: 363
		private BannerImageIdentifierVM _visual;

		// Token: 0x0400016C RID: 364
		private bool _isCaravan;

		// Token: 0x0400016D RID: 365
		private bool _isLord;

		// Token: 0x0400016E RID: 366
		private bool _isDefault;

		// Token: 0x0400016F RID: 367
		private bool _isBandit;

		// Token: 0x04000170 RID: 368
		private bool _hasBloodFeud;
	}
}
