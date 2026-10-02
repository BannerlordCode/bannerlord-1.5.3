using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C8 RID: 1224
	public static class ChangeRelationAction
	{
		// Token: 0x06004D3B RID: 19771 RVA: 0x00186DD8 File Offset: 0x00184FD8
		private static void ApplyInternal(Hero originalHero, Hero originalGainedRelationWith, int relationChange, bool showQuickNotification, ChangeRelationAction.ChangeRelationDetail detail)
		{
			relationChange = Campaign.Current.Models.DiplomacyModel.GetEffectiveRelationChange(originalHero, originalGainedRelationWith, relationChange);
			if (relationChange != 0)
			{
				Hero hero;
				Hero hero2;
				Campaign.Current.Models.DiplomacyModel.GetHeroesForEffectiveRelation(originalHero, originalGainedRelationWith, out hero, out hero2);
				int num = CharacterRelationManager.GetHeroRelation(hero, hero2) + relationChange;
				num = MBMath.ClampInt(num, -100, 100);
				hero.SetPersonalRelation(hero2, num);
				CampaignEventDispatcher.Instance.OnHeroRelationChanged(hero, hero2, relationChange, showQuickNotification, detail, originalHero, originalGainedRelationWith);
			}
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x00186E4C File Offset: 0x0018504C
		private static void ApplyInternalBySet(Hero originalHero, Hero originalGainedRelationWith, int relationAmount, bool showQuickNotification, ChangeRelationAction.ChangeRelationDetail detail)
		{
			Hero hero;
			Hero hero2;
			Campaign.Current.Models.DiplomacyModel.GetHeroesForEffectiveRelation(originalHero, originalGainedRelationWith, out hero, out hero2);
			int heroRelation = CharacterRelationManager.GetHeroRelation(hero, hero2);
			int num = relationAmount - heroRelation;
			if (num != 0)
			{
				hero.SetPersonalRelation(hero2, relationAmount);
				CampaignEventDispatcher.Instance.OnHeroRelationChanged(hero, hero2, num, showQuickNotification, detail, originalHero, originalGainedRelationWith);
			}
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x00186E9C File Offset: 0x0018509C
		public static void ApplyPlayerRelation(Hero gainedRelationWith, int relation, bool affectRelatives = true, bool showQuickNotification = true)
		{
			ChangeRelationAction.ApplyInternal(Hero.MainHero, gainedRelationWith, relation, showQuickNotification, ChangeRelationAction.ChangeRelationDetail.Default);
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x00186EAC File Offset: 0x001850AC
		public static void ApplyRelationChangeBetweenHeroes(Hero hero, Hero gainedRelationWith, int relationChange, bool showQuickNotification = true)
		{
			ChangeRelationAction.ApplyInternal(hero, gainedRelationWith, relationChange, showQuickNotification, ChangeRelationAction.ChangeRelationDetail.Default);
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x00186EB8 File Offset: 0x001850B8
		public static void ApplyEmissaryRelation(Hero emissary, Hero gainedRelationWith, int relationChange, bool showQuickNotification = true)
		{
			ChangeRelationAction.ApplyInternal(emissary, gainedRelationWith, relationChange, showQuickNotification, ChangeRelationAction.ChangeRelationDetail.Emissary);
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x00186EC4 File Offset: 0x001850C4
		public static void SetRelationBetweenHeroes(Hero hero, Hero gainedRelationWith, int newRelation, bool showQuickNotification = true)
		{
			ChangeRelationAction.ApplyInternalBySet(hero, gainedRelationWith, newRelation, showQuickNotification, ChangeRelationAction.ChangeRelationDetail.Default);
		}

		// Token: 0x020008DD RID: 2269
		public enum ChangeRelationDetail
		{
			// Token: 0x04002660 RID: 9824
			Default,
			// Token: 0x04002661 RID: 9825
			Emissary
		}
	}
}
