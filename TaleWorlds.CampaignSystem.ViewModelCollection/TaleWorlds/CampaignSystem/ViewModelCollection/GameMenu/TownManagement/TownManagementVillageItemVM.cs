using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000B1 RID: 177
	public class TownManagementVillageItemVM : ViewModel
	{
		// Token: 0x06001093 RID: 4243 RVA: 0x000439C0 File Offset: 0x00041BC0
		public TownManagementVillageItemVM(Village village)
		{
			this._village = village;
			this.Background = village.Settlement.SettlementComponent.BackgroundMeshName + "_t";
			this.VillageType = (int)this.DetermineVillageType(village.VillageType);
			this.RefreshValues();
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00043A12 File Offset: 0x00041C12
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._village.Name.ToString();
			this.ProductionName = this._village.VillageType.PrimaryProduction.Name.ToString();
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00043A50 File Offset: 0x00041C50
		public void ExecuteShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this._village.Settlement });
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00043A75 File Offset: 0x00041C75
		public void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00043A7C File Offset: 0x00041C7C
		private TownManagementVillageItemVM.VillageTypes DetermineVillageType(VillageType village)
		{
			if (village == DefaultVillageTypes.EuropeHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.EuropeHorseRanch;
			}
			if (village == DefaultVillageTypes.BattanianHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.BattanianHorseRanch;
			}
			if (village == DefaultVillageTypes.SteppeHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.SteppeHorseRanch;
			}
			if (village == DefaultVillageTypes.DesertHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.DesertHorseRanch;
			}
			if (village == DefaultVillageTypes.WheatFarm)
			{
				return TownManagementVillageItemVM.VillageTypes.WheatFarm;
			}
			if (village == DefaultVillageTypes.Lumberjack)
			{
				return TownManagementVillageItemVM.VillageTypes.Lumberjack;
			}
			if (village == DefaultVillageTypes.ClayMine)
			{
				return TownManagementVillageItemVM.VillageTypes.ClayMine;
			}
			if (village == DefaultVillageTypes.SaltMine)
			{
				return TownManagementVillageItemVM.VillageTypes.SaltMine;
			}
			if (village == DefaultVillageTypes.IronMine)
			{
				return TownManagementVillageItemVM.VillageTypes.IronMine;
			}
			if (village == DefaultVillageTypes.Fisherman)
			{
				return TownManagementVillageItemVM.VillageTypes.Fisherman;
			}
			if (village == DefaultVillageTypes.CattleRange)
			{
				return TownManagementVillageItemVM.VillageTypes.CattleRange;
			}
			if (village == DefaultVillageTypes.SheepFarm)
			{
				return TownManagementVillageItemVM.VillageTypes.SheepFarm;
			}
			if (village == DefaultVillageTypes.VineYard)
			{
				return TownManagementVillageItemVM.VillageTypes.VineYard;
			}
			if (village == DefaultVillageTypes.FlaxPlant)
			{
				return TownManagementVillageItemVM.VillageTypes.FlaxPlant;
			}
			if (village == DefaultVillageTypes.DateFarm)
			{
				return TownManagementVillageItemVM.VillageTypes.DateFarm;
			}
			if (village == DefaultVillageTypes.OliveTrees)
			{
				return TownManagementVillageItemVM.VillageTypes.OliveTrees;
			}
			if (village == DefaultVillageTypes.SilkPlant)
			{
				return TownManagementVillageItemVM.VillageTypes.SilkPlant;
			}
			if (village == DefaultVillageTypes.SilverMine)
			{
				return TownManagementVillageItemVM.VillageTypes.SilverMine;
			}
			return TownManagementVillageItemVM.VillageTypes.None;
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00043B48 File Offset: 0x00041D48
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x00043B50 File Offset: 0x00041D50
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

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x00043B73 File Offset: 0x00041D73
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x00043B7B File Offset: 0x00041D7B
		[DataSourceProperty]
		public string ProductionName
		{
			get
			{
				return this._productionName;
			}
			set
			{
				if (value != this._productionName)
				{
					this._productionName = value;
					base.OnPropertyChangedWithValue<string>(value, "ProductionName");
				}
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x00043B9E File Offset: 0x00041D9E
		// (set) Token: 0x0600109D RID: 4253 RVA: 0x00043BA6 File Offset: 0x00041DA6
		[DataSourceProperty]
		public string Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChangedWithValue<string>(value, "Background");
				}
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x00043BC9 File Offset: 0x00041DC9
		// (set) Token: 0x0600109F RID: 4255 RVA: 0x00043BD1 File Offset: 0x00041DD1
		[DataSourceProperty]
		public int VillageType
		{
			get
			{
				return this._villageType;
			}
			set
			{
				if (value != this._villageType)
				{
					this._villageType = value;
					base.OnPropertyChangedWithValue(value, "VillageType");
				}
			}
		}

		// Token: 0x04000789 RID: 1929
		private readonly Village _village;

		// Token: 0x0400078A RID: 1930
		private string _name;

		// Token: 0x0400078B RID: 1931
		private string _background;

		// Token: 0x0400078C RID: 1932
		private string _productionName;

		// Token: 0x0400078D RID: 1933
		private int _villageType;

		// Token: 0x02000221 RID: 545
		private enum VillageTypes
		{
			// Token: 0x0400121B RID: 4635
			None,
			// Token: 0x0400121C RID: 4636
			EuropeHorseRanch,
			// Token: 0x0400121D RID: 4637
			BattanianHorseRanch,
			// Token: 0x0400121E RID: 4638
			SteppeHorseRanch,
			// Token: 0x0400121F RID: 4639
			DesertHorseRanch,
			// Token: 0x04001220 RID: 4640
			WheatFarm,
			// Token: 0x04001221 RID: 4641
			Lumberjack,
			// Token: 0x04001222 RID: 4642
			ClayMine,
			// Token: 0x04001223 RID: 4643
			SaltMine,
			// Token: 0x04001224 RID: 4644
			IronMine,
			// Token: 0x04001225 RID: 4645
			Fisherman,
			// Token: 0x04001226 RID: 4646
			CattleRange,
			// Token: 0x04001227 RID: 4647
			SheepFarm,
			// Token: 0x04001228 RID: 4648
			VineYard,
			// Token: 0x04001229 RID: 4649
			FlaxPlant,
			// Token: 0x0400122A RID: 4650
			DateFarm,
			// Token: 0x0400122B RID: 4651
			OliveTrees,
			// Token: 0x0400122C RID: 4652
			SilkPlant,
			// Token: 0x0400122D RID: 4653
			SilverMine
		}
	}
}
