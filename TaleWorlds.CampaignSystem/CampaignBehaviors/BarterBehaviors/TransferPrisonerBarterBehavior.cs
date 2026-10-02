using System;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors
{
	// Token: 0x0200048E RID: 1166
	public class TransferPrisonerBarterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B19 RID: 19225 RVA: 0x0017AF88 File Offset: 0x00179188
		public override void RegisterEvents()
		{
			CampaignEvents.BarterablesRequested.AddNonSerializedListener(this, new Action<BarterData>(this.CheckForBarters));
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x0017AFA1 File Offset: 0x001791A1
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x0017AFA4 File Offset: 0x001791A4
		public void CheckForBarters(BarterData args)
		{
			PartyBase offererParty = args.OffererParty;
			PartyBase otherParty = args.OtherParty;
			if (offererParty != null && otherParty != null)
			{
				foreach (CharacterObject characterObject in offererParty.PrisonerHeroes)
				{
					if (characterObject.IsHero && FactionManager.IsAtWarAgainstFaction(characterObject.HeroObject.MapFaction, otherParty.MapFaction))
					{
						Barterable barterable = new TransferPrisonerBarterable(characterObject.HeroObject, args.OffererHero, args.OffererParty, args.OtherHero, otherParty);
						args.AddBarterable<PrisonerBarterGroup>(barterable, false);
					}
				}
				foreach (CharacterObject characterObject2 in otherParty.PrisonerHeroes)
				{
					if (characterObject2.IsHero && FactionManager.IsAtWarAgainstFaction(characterObject2.HeroObject.MapFaction, offererParty.MapFaction))
					{
						Barterable barterable2 = new TransferPrisonerBarterable(characterObject2.HeroObject, args.OtherHero, args.OtherParty, args.OffererHero, offererParty);
						args.AddBarterable<PrisonerBarterGroup>(barterable2, false);
					}
				}
			}
		}
	}
}
