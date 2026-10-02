using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EA RID: 234
	public class EncyclopediaDwellingVM : ViewModel
	{
		// Token: 0x060015B8 RID: 5560 RVA: 0x00055F18 File Offset: 0x00054118
		public EncyclopediaDwellingVM(WorkshopType workshop)
		{
			this._workshop = workshop;
			this.FileName = workshop.StringId;
			this.RefreshValues();
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00055F39 File Offset: 0x00054139
		public EncyclopediaDwellingVM(VillageType villageType)
		{
			this._villageType = villageType;
			this.FileName = villageType.StringId;
			this.RefreshValues();
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00055F5C File Offset: 0x0005415C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._workshop != null)
			{
				this.NameText = this._workshop.Name.ToString();
				return;
			}
			if (this._villageType != null)
			{
				this.NameText = this._villageType.ShortName.ToString();
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x00055FAC File Offset: 0x000541AC
		// (set) Token: 0x060015BC RID: 5564 RVA: 0x00055FB4 File Offset: 0x000541B4
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

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x00055FD7 File Offset: 0x000541D7
		// (set) Token: 0x060015BE RID: 5566 RVA: 0x00055FDF File Offset: 0x000541DF
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

		// Token: 0x040009D6 RID: 2518
		private readonly WorkshopType _workshop;

		// Token: 0x040009D7 RID: 2519
		private readonly VillageType _villageType;

		// Token: 0x040009D8 RID: 2520
		private string _fileName;

		// Token: 0x040009D9 RID: 2521
		private string _nameText;
	}
}
