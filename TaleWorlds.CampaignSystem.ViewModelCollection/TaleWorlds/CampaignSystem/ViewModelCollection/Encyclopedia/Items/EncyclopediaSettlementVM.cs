using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EE RID: 238
	public class EncyclopediaSettlementVM : ViewModel
	{
		// Token: 0x060015D7 RID: 5591 RVA: 0x000562DC File Offset: 0x000544DC
		public EncyclopediaSettlementVM(Settlement settlement)
		{
			if (!settlement.IsHideout)
			{
				this._settlement = settlement;
			}
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.FileName = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.RefreshValues();
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0005632B File Offset: 0x0005452B
		public override void RefreshValues()
		{
			base.RefreshValues();
			Settlement settlement = this._settlement;
			this.NameText = ((settlement != null) ? settlement.Name.ToString() : null) ?? "";
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00056359 File Offset: 0x00054559
		public void ExecuteLink()
		{
			if (this._settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._settlement.EncyclopediaLink);
			}
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x0005637D File Offset: 0x0005457D
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00056384 File Offset: 0x00054584
		public void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this._settlement });
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x000563A4 File Offset: 0x000545A4
		// (set) Token: 0x060015DD RID: 5597 RVA: 0x000563AC File Offset: 0x000545AC
		[DataSourceProperty]
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (value != this._fileName)
				{
					this._fileName = value;
					base.OnPropertyChangedWithValue<string>(value, "FileName");
				}
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x000563CF File Offset: 0x000545CF
		// (set) Token: 0x060015DF RID: 5599 RVA: 0x000563D7 File Offset: 0x000545D7
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x040009E3 RID: 2531
		private Settlement _settlement;

		// Token: 0x040009E4 RID: 2532
		private string _fileName;

		// Token: 0x040009E5 RID: 2533
		private string _nameText;
	}
}
