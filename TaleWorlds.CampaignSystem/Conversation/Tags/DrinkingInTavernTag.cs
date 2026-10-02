using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000250 RID: 592
	public class DrinkingInTavernTag : ConversationTag
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x0009E15A File Offset: 0x0009C35A
		public override string StringId
		{
			get
			{
				return "DrinkingInTavernTag";
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0009E164 File Offset: 0x0009C364
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (LocationComplex.Current != null && character.IsHero)
			{
				Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(character.HeroObject);
				Location locationWithId = LocationComplex.Current.GetLocationWithId("tavern");
				if (character.HeroObject.IsWanderer && Settlement.CurrentSettlement != null && locationWithId == locationOfCharacter)
				{
					return true;
				}
			}
			else if (character.HeroObject == null && LocationComplex.Current != null && Settlement.CurrentSettlement != null && LocationComplex.Current.GetLocationWithId("tavern") == CampaignMission.Current.Location)
			{
				return true;
			}
			return false;
		}

		// Token: 0x04000A9E RID: 2718
		public const string Id = "DrinkingInTavernTag";
	}
}
