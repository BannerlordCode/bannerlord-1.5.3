using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x0200008F RID: 143
	public class KingdomArmyItemVM : KingdomItemVM
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x000318C9 File Offset: 0x0002FAC9
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x000318D1 File Offset: 0x0002FAD1
		public float DistanceToMainParty { get; set; }

		// Token: 0x06000BCF RID: 3023 RVA: 0x000318DC File Offset: 0x0002FADC
		public KingdomArmyItemVM(Army army, Action<KingdomArmyItemVM> onSelect)
		{
			this.Army = army;
			this._onSelect = onSelect;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			CampaignUIHelper.GetCharacterCode(army.ArmyOwner.CharacterObject, false);
			this.Leader = new HeroVM(this.Army.LeaderParty.LeaderHero, false);
			this.LordCount = army.Parties.Count;
			this.Strength = army.Parties.Sum<MobileParty>((MobileParty p) => p.Party.NumberOfAllMembers);
			this.Location = CampaignUIHelper.GetPartyLocationText(army.LeaderParty);
			this.Behavior = army.GetLongTermBehaviorText(true).ToString();
			this.UpdateIsNew();
			this.Cohesion = (int)this.Army.Cohesion;
			this.Parties = new MBBindingList<KingdomArmyPartyItemVM>();
			foreach (MobileParty mobileParty in this.Army.Parties)
			{
				this.Parties.Add(new KingdomArmyPartyItemVM(mobileParty));
			}
			this.DistanceToMainParty = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(army.LeaderParty, MobileParty.MainParty, army.LeaderParty.NavigationCapability);
			this.IsMainArmy = army.LeaderParty == MobileParty.MainParty;
			this.RefreshValues();
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00031A54 File Offset: 0x0002FC54
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmyName = this.Army.Name.ToString();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_cohesion", null));
			GameTexts.SetVariable("STR2", this.Cohesion.ToString());
			this.CohesionLabel = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_men_count", null));
			GameTexts.SetVariable("RIGHT", this.Strength.ToString());
			this.StrengthLabel = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
			this.ShipCount = this.Army.Parties.Sum<MobileParty>(delegate(MobileParty p)
			{
				MBReadOnlyList<Ship> ships = p.Ships;
				if (ships == null)
				{
					return 0;
				}
				return ships.Count;
			});
			this.ShipCountLabel = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).SetTextVariable("LEFT", new TextObject("{=7Q8ufo5X}Ships", null)).SetTextVariable("RIGHT", this.ShipCount)
				.ToString();
			this.Parties.ApplyActionOnAllItems(delegate(KingdomArmyPartyItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00031B9D File Offset: 0x0002FD9D
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
			this.ExecuteResetNew();
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00031BB7 File Offset: 0x0002FDB7
		private void ExecuteResetNew()
		{
			if (base.IsNew)
			{
				this._viewDataTracker.OnArmyExamined(this.Army);
				this.UpdateIsNew();
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00031BD8 File Offset: 0x0002FDD8
		private void UpdateIsNew()
		{
			base.IsNew = this._viewDataTracker.UnExaminedArmies.Any<Army>((Army a) => a == this.Army);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00031BFC File Offset: 0x0002FDFC
		protected void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x00031C0E File Offset: 0x0002FE0E
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00031C16 File Offset: 0x0002FE16
		[DataSourceProperty]
		public MBBindingList<KingdomArmyPartyItemVM> Parties
		{
			get
			{
				return this._parties;
			}
			set
			{
				if (value != this._parties)
				{
					this._parties = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomArmyPartyItemVM>>(value, "Parties");
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00031C34 File Offset: 0x0002FE34
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x00031C3C File Offset: 0x0002FE3C
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChanged("Visual");
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x00031C59 File Offset: 0x0002FE59
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x00031C61 File Offset: 0x0002FE61
		[DataSourceProperty]
		public string ArmyName
		{
			get
			{
				return this._armyName;
			}
			set
			{
				if (value != this._armyName)
				{
					this._armyName = value;
					base.OnPropertyChangedWithValue<string>(value, "ArmyName");
				}
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00031C84 File Offset: 0x0002FE84
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x00031C8C File Offset: 0x0002FE8C
		[DataSourceProperty]
		public int Cohesion
		{
			get
			{
				return this._cohesion;
			}
			set
			{
				if (value != this._cohesion)
				{
					this._cohesion = value;
					base.OnPropertyChangedWithValue(value, "Cohesion");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00031CAA File Offset: 0x0002FEAA
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x00031CB2 File Offset: 0x0002FEB2
		[DataSourceProperty]
		public string CohesionLabel
		{
			get
			{
				return this._cohesionLabel;
			}
			set
			{
				if (value != this._cohesionLabel)
				{
					this._cohesionLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "CohesionLabel");
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00031CD5 File Offset: 0x0002FED5
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00031CDD File Offset: 0x0002FEDD
		[DataSourceProperty]
		public int LordCount
		{
			get
			{
				return this._lordCount;
			}
			set
			{
				if (value != this._lordCount)
				{
					this._lordCount = value;
					base.OnPropertyChangedWithValue(value, "LordCount");
				}
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00031CFB File Offset: 0x0002FEFB
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x00031D03 File Offset: 0x0002FF03
		[DataSourceProperty]
		public int Strength
		{
			get
			{
				return this._strength;
			}
			set
			{
				if (value != this._strength)
				{
					this._strength = value;
					base.OnPropertyChangedWithValue(value, "Strength");
				}
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00031D21 File Offset: 0x0002FF21
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x00031D29 File Offset: 0x0002FF29
		[DataSourceProperty]
		public string StrengthLabel
		{
			get
			{
				return this._strengthLabel;
			}
			set
			{
				if (value != this._strengthLabel)
				{
					this._strengthLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthLabel");
				}
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00031D4C File Offset: 0x0002FF4C
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00031D54 File Offset: 0x0002FF54
		[DataSourceProperty]
		public int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
				}
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00031D72 File Offset: 0x0002FF72
		// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x00031D7A File Offset: 0x0002FF7A
		[DataSourceProperty]
		public string ShipCountLabel
		{
			get
			{
				return this._shipCountLabel;
			}
			set
			{
				if (value != this._shipCountLabel)
				{
					this._shipCountLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipCountLabel");
				}
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00031D9D File Offset: 0x0002FF9D
		// (set) Token: 0x06000BEA RID: 3050 RVA: 0x00031DA5 File Offset: 0x0002FFA5
		[DataSourceProperty]
		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				if (value != this._location)
				{
					this._location = value;
					base.OnPropertyChangedWithValue<string>(value, "Location");
				}
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00031DC8 File Offset: 0x0002FFC8
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x00031DD0 File Offset: 0x0002FFD0
		[DataSourceProperty]
		public string Behavior
		{
			get
			{
				return this._behavior;
			}
			set
			{
				if (value != this._behavior)
				{
					this._behavior = value;
					base.OnPropertyChanged("Objective");
				}
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00031DF2 File Offset: 0x0002FFF2
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x00031DFA File Offset: 0x0002FFFA
		[DataSourceProperty]
		public bool IsMainArmy
		{
			get
			{
				return this._isMainArmy;
			}
			set
			{
				if (value != this._isMainArmy)
				{
					this._isMainArmy = value;
					base.OnPropertyChangedWithValue(value, "IsMainArmy");
				}
			}
		}

		// Token: 0x04000537 RID: 1335
		public readonly Army Army;

		// Token: 0x04000539 RID: 1337
		private readonly Action<KingdomArmyItemVM> _onSelect;

		// Token: 0x0400053A RID: 1338
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x0400053B RID: 1339
		private HeroVM _leader;

		// Token: 0x0400053C RID: 1340
		private MBBindingList<KingdomArmyPartyItemVM> _parties;

		// Token: 0x0400053D RID: 1341
		private string _armyName;

		// Token: 0x0400053E RID: 1342
		private int _strength;

		// Token: 0x0400053F RID: 1343
		private int _cohesion;

		// Token: 0x04000540 RID: 1344
		private string _strengthLabel;

		// Token: 0x04000541 RID: 1345
		private string _shipCountLabel;

		// Token: 0x04000542 RID: 1346
		private int _shipCount;

		// Token: 0x04000543 RID: 1347
		private int _lordCount;

		// Token: 0x04000544 RID: 1348
		private string _location;

		// Token: 0x04000545 RID: 1349
		private string _behavior;

		// Token: 0x04000546 RID: 1350
		private string _cohesionLabel;

		// Token: 0x04000547 RID: 1351
		private bool _isMainArmy;
	}
}
