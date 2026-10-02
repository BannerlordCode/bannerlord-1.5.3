using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200047C RID: 1148
	public class CommentOnCharacterKilledBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AC9 RID: 19145 RVA: 0x00179A61 File Offset: 0x00177C61
		public override void RegisterEvents()
		{
			CampaignEvents.BeforeHeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnBeforeHeroKilled));
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x00179A7A File Offset: 0x00177C7A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x00179A7C File Offset: 0x00177C7C
		private void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.Clan != null && !Clan.BanditFactions.Contains(victim.Clan))
			{
				CharacterKilledLogEntry characterKilledLogEntry = new CharacterKilledLogEntry(victim, killer, detail);
				LogEntry.AddLogEntry(characterKilledLogEntry);
				if (this.IsRelatedToPlayer(victim) && (detail != KillCharacterAction.KillCharacterActionDetail.Executed || killer != Hero.MainHero) && (detail != KillCharacterAction.KillCharacterActionDetail.Executed || !killer.Clan.HasBloodFeudWithPlayer))
				{
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new DeathMapNotification(victim, killer, characterKilledLogEntry.GetEncyclopediaText(), detail, CampaignTime.Now));
				}
			}
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x00179AFC File Offset: 0x00177CFC
		private bool IsRelatedToPlayer(Hero victim)
		{
			bool flag = victim == Hero.MainHero.Mother || victim == Hero.MainHero.Father || victim == Hero.MainHero.Spouse || victim == Hero.MainHero;
			if (!flag)
			{
				foreach (Hero hero in Hero.MainHero.Children)
				{
					if (victim == hero)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				foreach (Hero hero2 in Hero.MainHero.Siblings)
				{
					if (victim == hero2)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}
	}
}
