using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x02000078 RID: 120
	public class MPArmoryClassStatsVM : ViewModel
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x00023C61 File Offset: 0x00021E61
		public MPArmoryClassStatsVM()
		{
			this._dummyPerkList = new List<IReadOnlyPerkObject>();
			this.FactionDescription = new TextObject("{=5Pea977J}Faction: ", null).ToString();
			this.HeroInformation = new HeroInformationVM();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00023C95 File Offset: 0x00021E95
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CostHint = new HintViewModel(GameTexts.FindText("str_armory_troop_cost", null), null);
			this.HeroInformation.RefreshValues();
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00023CC0 File Offset: 0x00021EC0
		public void RefreshWith(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			this.FactionName = heroClass.Culture.Name.ToString();
			this.FlavorText = GameTexts.FindText("str_troop_description", heroClass.StringId).ToString();
			this.HeroInformation.RefreshWith(heroClass, this._dummyPerkList);
			this.Cost = heroClass.TroopCost;
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00023D1C File Offset: 0x00021F1C
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x00023D24 File Offset: 0x00021F24
		[DataSourceProperty]
		public string FactionDescription
		{
			get
			{
				return this._factionDescription;
			}
			set
			{
				if (value != this._factionDescription)
				{
					this._factionDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionDescription");
				}
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00023D47 File Offset: 0x00021F47
		// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x00023D4F File Offset: 0x00021F4F
		[DataSourceProperty]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (value != this._factionName)
				{
					this._factionName = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionName");
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00023D72 File Offset: 0x00021F72
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x00023D7A File Offset: 0x00021F7A
		[DataSourceProperty]
		public string FlavorText
		{
			get
			{
				return this._flavorText;
			}
			set
			{
				if (value != this._flavorText)
				{
					this._flavorText = value;
					base.OnPropertyChangedWithValue<string>(value, "FlavorText");
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00023D9D File Offset: 0x00021F9D
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x00023DA5 File Offset: 0x00021FA5
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00023DC3 File Offset: 0x00021FC3
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00023DCB File Offset: 0x00021FCB
		[DataSourceProperty]
		public HintViewModel CostHint
		{
			get
			{
				return this._costHint;
			}
			set
			{
				if (value != this._costHint)
				{
					this._costHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CostHint");
				}
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00023DE9 File Offset: 0x00021FE9
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x00023DF1 File Offset: 0x00021FF1
		[DataSourceProperty]
		public HeroInformationVM HeroInformation
		{
			get
			{
				return this._heroInformation;
			}
			set
			{
				if (value != this._heroInformation)
				{
					this._heroInformation = value;
					base.OnPropertyChangedWithValue<HeroInformationVM>(value, "HeroInformation");
				}
			}
		}

		// Token: 0x04000567 RID: 1383
		private readonly List<IReadOnlyPerkObject> _dummyPerkList;

		// Token: 0x04000568 RID: 1384
		private string _factionDescription;

		// Token: 0x04000569 RID: 1385
		private string _factionName;

		// Token: 0x0400056A RID: 1386
		private string _flavorText;

		// Token: 0x0400056B RID: 1387
		private int _cost;

		// Token: 0x0400056C RID: 1388
		private HintViewModel _costHint;

		// Token: 0x0400056D RID: 1389
		private HeroInformationVM _heroInformation;
	}
}
