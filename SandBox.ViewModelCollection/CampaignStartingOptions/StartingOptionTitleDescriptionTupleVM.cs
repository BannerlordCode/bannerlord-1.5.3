using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.CampaignStartingOptions
{
	// Token: 0x02000062 RID: 98
	public class StartingOptionTitleDescriptionTupleVM : ViewModel
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x000162D4 File Offset: 0x000144D4
		public StartingOptionTitleDescriptionTupleVM(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x000162EA File Offset: 0x000144EA
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x000162F2 File Offset: 0x000144F2
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00016315 File Offset: 0x00014515
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x0001631D File Offset: 0x0001451D
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

		// Token: 0x040002F8 RID: 760
		private string _name;

		// Token: 0x040002F9 RID: 761
		private string _description;
	}
}
