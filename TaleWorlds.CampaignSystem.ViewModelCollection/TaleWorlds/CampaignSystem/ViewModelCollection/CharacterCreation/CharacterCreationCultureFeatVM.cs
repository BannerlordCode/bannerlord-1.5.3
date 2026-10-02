using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000152 RID: 338
	public class CharacterCreationCultureFeatVM : ViewModel
	{
		// Token: 0x060020D1 RID: 8401 RVA: 0x00075C87 File Offset: 0x00073E87
		public CharacterCreationCultureFeatVM(bool isPositive, string description)
		{
			this.IsPositive = isPositive;
			this.Description = description;
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00075C9D File Offset: 0x00073E9D
		// (set) Token: 0x060020D3 RID: 8403 RVA: 0x00075CA5 File Offset: 0x00073EA5
		[DataSourceProperty]
		public bool IsPositive
		{
			get
			{
				return this._isPositive;
			}
			set
			{
				if (value != this._isPositive)
				{
					this._isPositive = value;
					base.OnPropertyChangedWithValue(value, "IsPositive");
				}
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x00075CC3 File Offset: 0x00073EC3
		// (set) Token: 0x060020D5 RID: 8405 RVA: 0x00075CCB File Offset: 0x00073ECB
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x04000F06 RID: 3846
		private bool _isPositive;

		// Token: 0x04000F07 RID: 3847
		private string _description;
	}
}
