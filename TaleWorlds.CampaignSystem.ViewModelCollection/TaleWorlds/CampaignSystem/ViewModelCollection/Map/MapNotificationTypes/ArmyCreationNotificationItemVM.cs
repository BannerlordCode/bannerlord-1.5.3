using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200003F RID: 63
	public class ArmyCreationNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001F56F File Offset: 0x0001D76F
		public Army Army { get; }

		// Token: 0x060005EA RID: 1514 RVA: 0x0001F578 File Offset: 0x0001D778
		public ArmyCreationNotificationItemVM(ArmyCreationMapNotification data)
			: base(data)
		{
			this.Army = data.CreatedArmy;
			base.NotificationIdentifier = "armycreation";
			this._onInspect = delegate
			{
				Army army = this.Army;
				CampaignVec2? campaignVec;
				if (army == null)
				{
					campaignVec = null;
				}
				else
				{
					MobileParty leaderParty = army.LeaderParty;
					campaignVec = ((leaderParty != null) ? new CampaignVec2?(leaderParty.Position) : null);
				}
				base.GoToMapPosition(campaignVec ?? MobileParty.MainParty.Position);
			};
			CampaignEvents.OnPartyJoinedArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyJoinedArmy));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001F5FA File Offset: 0x0001D7FA
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == MobileParty.MainParty.ActualClan && oldKingdom != newKingdom)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001F613 File Offset: 0x0001D813
		private void OnArmyDispersed(Army arg1, Army.ArmyDispersionReason arg2, bool isPlayersArmy)
		{
			if (arg1 == this.Army)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001F624 File Offset: 0x0001D824
		private void OnPartyJoinedArmy(MobileParty party)
		{
			if (party == MobileParty.MainParty && party.Army == this.Army)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001F642 File Offset: 0x0001D842
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnPartyJoinedArmyEvent.ClearListeners(this);
			CampaignEvents.ArmyDispersed.ClearListeners(this);
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}
	}
}
