using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x0200008B RID: 139
	public class KingdomClanFiefItemVM : ViewModel
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x00030444 File Offset: 0x0002E644
		public KingdomClanFiefItemVM(Settlement settlement)
		{
			this.Settlement = settlement;
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.VisualPath = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.RefreshValues();
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0003048B File Offset: 0x0002E68B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FiefName = this.Settlement.Name.ToString();
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x000304A9 File Offset: 0x0002E6A9
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this.Settlement, true });
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000304D2 File Offset: 0x0002E6D2
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000304D9 File Offset: 0x0002E6D9
		public void ExecuteLink()
		{
			if (this.Settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x000304FD File Offset: 0x0002E6FD
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x00030505 File Offset: 0x0002E705
		[DataSourceProperty]
		public string VisualPath
		{
			get
			{
				return this._visualPath;
			}
			set
			{
				if (value != this._visualPath)
				{
					this._visualPath = value;
					base.OnPropertyChanged("FileName");
				}
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00030527 File Offset: 0x0002E727
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0003052F File Offset: 0x0002E72F
		[DataSourceProperty]
		public string FiefName
		{
			get
			{
				return this._fiefName;
			}
			set
			{
				if (value != this._fiefName)
				{
					this._fiefName = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefName");
				}
			}
		}

		// Token: 0x04000503 RID: 1283
		private readonly Settlement Settlement;

		// Token: 0x04000504 RID: 1284
		private string _visualPath;

		// Token: 0x04000505 RID: 1285
		private string _fiefName;
	}
}
