using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B7 RID: 183
	public class RecruitVolunteerOwnerVM : HeroVM
	{
		// Token: 0x0600119A RID: 4506 RVA: 0x00046933 File Offset: 0x00044B33
		public RecruitVolunteerOwnerVM(Hero hero, int relation)
			: base(hero, hero != null && hero.IsNotable)
		{
			this._hero = hero;
			this.RelationToPlayer = relation;
			this.RefreshValues();
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004695C File Offset: 0x00044B5C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._hero != null)
			{
				if (this._hero.IsPreacher)
				{
					this.TitleText = GameTexts.FindText("str_preacher", null).ToString();
					return;
				}
				if (this._hero.IsGangLeader)
				{
					this.TitleText = GameTexts.FindText("str_gang_leader", null).ToString();
					return;
				}
				if (this._hero.IsMerchant)
				{
					this.TitleText = GameTexts.FindText("str_merchant", null).ToString();
					return;
				}
				if (this._hero.IsRuralNotable)
				{
					this.TitleText = GameTexts.FindText("str_rural_notable", null).ToString();
				}
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00046A09 File Offset: 0x00044C09
		public void ExecuteOpenEncyclopedia()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._hero.EncyclopediaLink);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00046A25 File Offset: 0x00044C25
		public void ExecuteFocus()
		{
			Action<RecruitVolunteerOwnerVM> onFocused = RecruitVolunteerOwnerVM.OnFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(this);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00046A37 File Offset: 0x00044C37
		public void ExecuteUnfocus()
		{
			Action<RecruitVolunteerOwnerVM> onFocused = RecruitVolunteerOwnerVM.OnFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(null);
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00046A49 File Offset: 0x00044C49
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x00046A51 File Offset: 0x00044C51
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x00046A74 File Offset: 0x00044C74
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x00046A7C File Offset: 0x00044C7C
		[DataSourceProperty]
		public int RelationToPlayer
		{
			get
			{
				return this._relationToPlayer;
			}
			set
			{
				if (value != this._relationToPlayer)
				{
					this._relationToPlayer = value;
					base.OnPropertyChangedWithValue(value, "RelationToPlayer");
				}
			}
		}

		// Token: 0x040007F9 RID: 2041
		public static Action<RecruitVolunteerOwnerVM> OnFocused;

		// Token: 0x040007FA RID: 2042
		private Hero _hero;

		// Token: 0x040007FB RID: 2043
		private string _titleText;

		// Token: 0x040007FC RID: 2044
		private int _relationToPlayer;
	}
}
