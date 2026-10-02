using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E4 RID: 1252
	public static class MakeHeroFugitiveAction
	{
		// Token: 0x06004DC5 RID: 19909 RVA: 0x00189400 File Offset: 0x00187600
		private static void ApplyInternal(Hero fugitive, bool showNotification)
		{
			if (fugitive.IsAlive)
			{
				if (fugitive.PartyBelongedTo != null)
				{
					if (fugitive.PartyBelongedTo.LeaderHero == fugitive)
					{
						DestroyPartyAction.Apply(null, fugitive.PartyBelongedTo);
					}
					else
					{
						fugitive.PartyBelongedTo.MemberRoster.RemoveTroop(fugitive.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
				}
				if (fugitive.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(fugitive);
				}
				fugitive.ChangeState(Hero.CharacterStates.Fugitive);
				CampaignEventDispatcher.Instance.OnCharacterBecameFugitive(fugitive, showNotification);
			}
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x0018947B File Offset: 0x0018767B
		public static void Apply(Hero fugitive, bool showNotification = false)
		{
			MakeHeroFugitiveAction.ApplyInternal(fugitive, showNotification);
		}
	}
}
