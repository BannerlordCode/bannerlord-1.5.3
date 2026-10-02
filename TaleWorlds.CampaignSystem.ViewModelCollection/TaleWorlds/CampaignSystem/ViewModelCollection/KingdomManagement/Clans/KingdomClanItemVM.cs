using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x0200008C RID: 140
	public class KingdomClanItemVM : KingdomItemVM
	{
		// Token: 0x06000B63 RID: 2915 RVA: 0x00030554 File Offset: 0x0002E754
		public KingdomClanItemVM(Clan clan, Action<KingdomClanItemVM> onSelect)
		{
			this.Clan = clan;
			this._onSelect = onSelect;
			this.Banner = new BannerImageIdentifierVM(clan.Banner, false);
			this.Banner_9 = new BannerImageIdentifierVM(clan.Banner, true);
			this.RefreshValues();
			this.Refresh();
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000305AC File Offset: 0x0002E7AC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Clan.Name.ToString();
			GameTexts.SetVariable("TIER", this.Clan.Tier);
			this.TierText = GameTexts.FindText("str_clan_tier", null).ToString();
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00030600 File Offset: 0x0002E800
		public void Refresh()
		{
			this.Members = new MBBindingList<HeroVM>();
			this.ClanType = 0;
			if (this.Clan.IsUnderMercenaryService)
			{
				this.ClanType = 2;
			}
			else if (this.Clan.Kingdom.RulingClan == this.Clan)
			{
				this.ClanType = 1;
			}
			foreach (Hero hero in this.Clan.Heroes.Where<Hero>((Hero h) => !h.IsDisabled && !h.IsNotSpawned && h.IsAlive && !h.IsChild))
			{
				this.Members.Add(new HeroVM(hero, false));
			}
			this.NumOfMembers = this.Members.Count;
			this.Fiefs = new MBBindingList<KingdomClanFiefItemVM>();
			foreach (Settlement settlement in this.Clan.Settlements.Where<Settlement>((Settlement s) => s.IsTown || s.IsCastle))
			{
				this.Fiefs.Add(new KingdomClanFiefItemVM(settlement));
			}
			this.NumOfFiefs = this.Fiefs.Count;
			this.Influence = (int)this.Clan.Influence;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00030778 File Offset: 0x0002E978
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0003078C File Offset: 0x0002E98C
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x00030794 File Offset: 0x0002E994
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

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x000307B7 File Offset: 0x0002E9B7
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x000307BF File Offset: 0x0002E9BF
		[DataSourceProperty]
		public int ClanType
		{
			get
			{
				return this._clanType;
			}
			set
			{
				if (value != this._clanType)
				{
					this._clanType = value;
					base.OnPropertyChangedWithValue(value, "ClanType");
				}
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x000307DD File Offset: 0x0002E9DD
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x000307E5 File Offset: 0x0002E9E5
		[DataSourceProperty]
		public int NumOfMembers
		{
			get
			{
				return this._numOfMembers;
			}
			set
			{
				if (value != this._numOfMembers)
				{
					this._numOfMembers = value;
					base.OnPropertyChangedWithValue(value, "NumOfMembers");
				}
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00030803 File Offset: 0x0002EA03
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0003080B File Offset: 0x0002EA0B
		[DataSourceProperty]
		public int NumOfFiefs
		{
			get
			{
				return this._numOfFiefs;
			}
			set
			{
				if (value != this._numOfFiefs)
				{
					this._numOfFiefs = value;
					base.OnPropertyChangedWithValue(value, "NumOfFiefs");
				}
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00030829 File Offset: 0x0002EA29
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x00030831 File Offset: 0x0002EA31
		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				if (value != this._tierText)
				{
					this._tierText = value;
					base.OnPropertyChangedWithValue<string>(value, "TierText");
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00030854 File Offset: 0x0002EA54
		// (set) Token: 0x06000B72 RID: 2930 RVA: 0x0003085C File Offset: 0x0002EA5C
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0003087A File Offset: 0x0002EA7A
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x00030882 File Offset: 0x0002EA82
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x000308A0 File Offset: 0x0002EAA0
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x000308A8 File Offset: 0x0002EAA8
		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Members");
				}
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x000308C6 File Offset: 0x0002EAC6
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x000308CE File Offset: 0x0002EACE
		[DataSourceProperty]
		public MBBindingList<KingdomClanFiefItemVM> Fiefs
		{
			get
			{
				return this._fiefs;
			}
			set
			{
				if (value != this._fiefs)
				{
					this._fiefs = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomClanFiefItemVM>>(value, "Fiefs");
				}
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x000308EC File Offset: 0x0002EAEC
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x000308F4 File Offset: 0x0002EAF4
		[DataSourceProperty]
		public int Influence
		{
			get
			{
				return this._influence;
			}
			set
			{
				if (value != this._influence)
				{
					this._influence = value;
					base.OnPropertyChangedWithValue(value, "Influence");
				}
			}
		}

		// Token: 0x04000506 RID: 1286
		private readonly Action<KingdomClanItemVM> _onSelect;

		// Token: 0x04000507 RID: 1287
		public readonly Clan Clan;

		// Token: 0x04000508 RID: 1288
		private string _name;

		// Token: 0x04000509 RID: 1289
		private BannerImageIdentifierVM _banner;

		// Token: 0x0400050A RID: 1290
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x0400050B RID: 1291
		private MBBindingList<HeroVM> _members;

		// Token: 0x0400050C RID: 1292
		private MBBindingList<KingdomClanFiefItemVM> _fiefs;

		// Token: 0x0400050D RID: 1293
		private int _influence;

		// Token: 0x0400050E RID: 1294
		private int _numOfMembers;

		// Token: 0x0400050F RID: 1295
		private int _numOfFiefs;

		// Token: 0x04000510 RID: 1296
		private string _tierText;

		// Token: 0x04000511 RID: 1297
		private int _clanType = -1;

		// Token: 0x020001EC RID: 492
		private enum ClanTypes
		{
			// Token: 0x0400119B RID: 4507
			Normal,
			// Token: 0x0400119C RID: 4508
			Leader,
			// Token: 0x0400119D RID: 4509
			Mercenary
		}
	}
}
