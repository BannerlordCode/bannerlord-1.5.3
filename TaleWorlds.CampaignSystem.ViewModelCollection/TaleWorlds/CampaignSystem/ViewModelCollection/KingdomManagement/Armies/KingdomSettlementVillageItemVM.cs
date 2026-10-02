using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x02000093 RID: 147
	public class KingdomSettlementVillageItemVM : ViewModel
	{
		// Token: 0x06000C55 RID: 3157 RVA: 0x00032EA8 File Offset: 0x000310A8
		public KingdomSettlementVillageItemVM(Village village)
		{
			this._village = village;
			this.VisualPath = village.BackgroundMeshName + "_t";
			this.RefreshValues();
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00032ED3 File Offset: 0x000310D3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._village.Name.ToString();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00032EF1 File Offset: 0x000310F1
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[]
			{
				this._village.Settlement,
				true
			});
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00032F1F File Offset: 0x0003111F
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00032F26 File Offset: 0x00031126
		public void ExecuteLink()
		{
			if (this._village != null && this._village.Settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._village.Settlement.EncyclopediaLink);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00032F5C File Offset: 0x0003115C
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x00032F64 File Offset: 0x00031164
		[DataSourceProperty]
		public ImageIdentifierVM Visual
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
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00032F82 File Offset: 0x00031182
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x00032F8A File Offset: 0x0003118A
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

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00032FAD File Offset: 0x000311AD
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x00032FB5 File Offset: 0x000311B5
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
					base.OnPropertyChangedWithValue<string>(value, "VisualPath");
				}
			}
		}

		// Token: 0x0400057A RID: 1402
		private Village _village;

		// Token: 0x0400057B RID: 1403
		private ImageIdentifierVM _visual;

		// Token: 0x0400057C RID: 1404
		private string _name;

		// Token: 0x0400057D RID: 1405
		private string _visualPath;
	}
}
