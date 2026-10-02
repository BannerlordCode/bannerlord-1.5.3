using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000041 RID: 65
	public class BloodFeudClanMemberCapturedMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005F1 RID: 1521 RVA: 0x0001F71C File Offset: 0x0001D91C
		public BloodFeudClanMemberCapturedMapNotificationItemVM(BloodFeudClanMemberCapturedMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "blood_feud_clan_member_captured";
			this._onInspect = new Action(this.OnInspect);
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001F7AC File Offset: 0x0001D9AC
		private void OnInspect()
		{
			Settlement settlement = ((BloodFeudClanMemberCapturedMapNotification)base.Data).Settlement;
			if (settlement != null)
			{
				if (!Campaign.Current.VisualTrackerManager.CheckTracked(settlement))
				{
					Campaign.Current.VisualTrackerManager.RegisterObject(settlement);
				}
				base.GoToMapPosition(settlement.Position);
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001F7FC File Offset: 0x0001D9FC
		private void DailyTick()
		{
			int num = (int)((BloodFeudClanMemberCapturedMapNotification)base.Data).ExecutionDate.RemainingDaysFromNow + 1;
			base.Data.DescriptionText.SetTextVariable("DAYS", num);
			this.RefreshValues();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001F842 File Offset: 0x0001DA42
		private void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification = true)
		{
			if (prisoner == ((BloodFeudClanMemberCapturedMapNotification)base.Data).CaptiveHero)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001F85D File Offset: 0x0001DA5D
		private void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			if (hero == ((BloodFeudClanMemberCapturedMapNotification)base.Data).CaptiveHero && oldClan == Clan.PlayerClan)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001F880 File Offset: 0x0001DA80
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (victim == ((BloodFeudClanMemberCapturedMapNotification)base.Data).CaptiveHero)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001F89B File Offset: 0x0001DA9B
		public override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
