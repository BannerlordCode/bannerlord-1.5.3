using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EC RID: 236
	public class EncyclopediaFamilyMemberVM : HeroVM
	{
		// Token: 0x060015CC RID: 5580 RVA: 0x000561AD File Offset: 0x000543AD
		public EncyclopediaFamilyMemberVM(Hero hero, Hero baseHero)
			: base(hero, false)
		{
			this._baseHero = baseHero;
			this.RefreshValues();
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x000561C4 File Offset: 0x000543C4
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._baseHero != null)
			{
				this.Role = ConversationHelper.GetHeroRelationToHeroTextShort(base.Hero, this._baseHero, true);
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x000561EC File Offset: 0x000543EC
		// (set) Token: 0x060015CF RID: 5583 RVA: 0x000561F4 File Offset: 0x000543F4
		[DataSourceProperty]
		public string Role
		{
			get
			{
				return this._role;
			}
			set
			{
				if (value != this._role)
				{
					this._role = value;
					base.OnPropertyChangedWithValue<string>(value, "Role");
				}
			}
		}

		// Token: 0x040009DE RID: 2526
		private readonly Hero _baseHero;

		// Token: 0x040009DF RID: 2527
		private string _role;
	}
}
