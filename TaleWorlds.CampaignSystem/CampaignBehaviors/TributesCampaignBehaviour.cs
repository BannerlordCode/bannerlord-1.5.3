using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200046D RID: 1133
	public class TributesCampaignBehaviour : CampaignBehaviorBase
	{
		// Token: 0x06004943 RID: 18755 RVA: 0x0016F0B6 File Offset: 0x0016D2B6
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanEarnedGoldFromTributeEvent.AddNonSerializedListener(this, new Action<Clan, IFaction>(TributesCampaignBehaviour.OnClanEarnedGoldFromTribute));
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x0016F0D0 File Offset: 0x0016D2D0
		private static void OnClanEarnedGoldFromTribute(Clan clan, IFaction payerFaction)
		{
			StanceLink stanceWith = clan.MapFaction.GetStanceWith(payerFaction);
			if ((clan == Clan.PlayerClan || payerFaction == Clan.PlayerClan.MapFaction) && stanceWith.GetRemainingTributePaymentCount() == 0)
			{
				bool flag = payerFaction == Clan.PlayerClan.MapFaction;
				TextObject textObject = (flag ? new TextObject("{=LJFXfmpn}The tribute your kingdom owed to {ENEMY_FACTION} is now complete.", null) : new TextObject("{=aod7KVc8}The tribute {ENEMY_FACTION} owed to your kingdom is now complete.", null));
				IFaction faction = (flag ? clan.MapFaction : payerFaction);
				textObject.SetTextVariable("ENEMY_FACTION", faction.Name);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new TributeFinishedMapNotification(textObject, faction));
			}
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x0016F169 File Offset: 0x0016D369
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
