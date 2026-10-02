using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x02000101 RID: 257
	public class CraftingPerkVM : ViewModel
	{
		// Token: 0x060016E3 RID: 5859 RVA: 0x00058FEB File Offset: 0x000571EB
		public CraftingPerkVM(PerkObject perk)
		{
			this.Perk = perk;
			this.Name = this.Perk.Name.ToString();
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x00059010 File Offset: 0x00057210
		// (set) Token: 0x060016E5 RID: 5861 RVA: 0x00059018 File Offset: 0x00057218
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

		// Token: 0x04000A6A RID: 2666
		public readonly PerkObject Perk;

		// Token: 0x04000A6B RID: 2667
		private string _name;
	}
}
