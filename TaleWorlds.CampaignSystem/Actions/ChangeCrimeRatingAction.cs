using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C1 RID: 1217
	public static class ChangeCrimeRatingAction
	{
		// Token: 0x06004D16 RID: 19734 RVA: 0x00185E64 File Offset: 0x00184064
		private static void ApplyInternal(IFaction faction, float deltaCrimeRating, bool showNotification)
		{
			float resultNumber = Campaign.Current.Models.CrimeModel.GetEffectiveCrimeChange(faction, deltaCrimeRating).ResultNumber;
			deltaCrimeRating = resultNumber - faction.MainHeroCrimeRating;
			if (showNotification && !deltaCrimeRating.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				TextObject textObject = new TextObject("{=hwq0RMRN}Your criminal rating with {FACTION_NAME} has {?IS_INCREASED}increased{?}decreased{\\?} by {CHANGE} to {NEW_RATING}", null);
				textObject.SetTextVariable("CHANGE", MathF.Round(MathF.Abs(deltaCrimeRating)));
				textObject.SetTextVariable("IS_INCREASED", (deltaCrimeRating > 0f) ? 1 : 0);
				textObject.SetTextVariable("FACTION_NAME", faction.Name);
				textObject.SetTextVariable("NEW_RATING", MathF.Round(resultNumber));
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
			faction.MainHeroCrimeRating = resultNumber;
			if (resultNumber >= Campaign.Current.Models.CrimeModel.DeclareWarCrimeRatingThreshold && Hero.MainHero.MapFaction.Leader == Hero.MainHero && !faction.IsAtWarWith(Hero.MainHero.MapFaction) && Hero.MainHero.MapFaction != faction)
			{
				ChangeRelationAction.ApplyPlayerRelation(faction.Leader, -10, true, true);
				DeclareWarAction.ApplyByCrimeRatingChange(faction, Hero.MainHero.MapFaction);
			}
			CampaignEventDispatcher.Instance.OnCrimeRatingChanged(faction, deltaCrimeRating);
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x00185F98 File Offset: 0x00184198
		public static void Apply(IFaction faction, float deltaCrimeRating, bool showNotification = true)
		{
			ChangeCrimeRatingAction.ApplyInternal(faction, deltaCrimeRating, showNotification);
		}
	}
}
