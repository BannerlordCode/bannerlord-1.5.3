using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters
{
	// Token: 0x02000134 RID: 308
	public class ClanSupporterItemVM : ViewModel
	{
		// Token: 0x06001CCD RID: 7373 RVA: 0x0006905B File Offset: 0x0006725B
		public ClanSupporterItemVM(Hero hero)
		{
			this.Hero = new HeroVM(hero, false);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00069070 File Offset: 0x00067270
		public void ExecuteOpenTooltip()
		{
			InformationManager.ShowTooltip(typeof(Hero), new object[]
			{
				this.Hero.Hero,
				false
			});
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0006909E File Offset: 0x0006729E
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x000690A5 File Offset: 0x000672A5
		// (set) Token: 0x06001CD1 RID: 7377 RVA: 0x000690AD File Offset: 0x000672AD
		[DataSourceProperty]
		public HeroVM Hero
		{
			get
			{
				return this._hero;
			}
			set
			{
				if (value != this._hero)
				{
					this._hero = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Hero");
				}
			}
		}

		// Token: 0x04000D2A RID: 3370
		private HeroVM _hero;
	}
}
