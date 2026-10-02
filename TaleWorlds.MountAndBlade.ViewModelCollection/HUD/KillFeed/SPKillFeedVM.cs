using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.General;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.Personal;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed
{
	// Token: 0x0200005C RID: 92
	public class SPKillFeedVM : ViewModel
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x0001A421 File Offset: 0x00018621
		public SPKillFeedVM()
		{
			this.GeneralCasualty = new SPGeneralKillNotificationVM();
			this.PersonalFeed = new SPPersonalKillNotificationVM();
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001A43F File Offset: 0x0001863F
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning)
		{
			this.GeneralCasualty.OnAgentRemoved(affectedAgent, affectorAgent, isHeadshot, isSuicide, isDrowning);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001A453 File Offset: 0x00018653
		public void OnPersonalKill(int damageAmount, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, bool isUnconscious)
		{
			this.PersonalFeed.OnPersonalKill(damageAmount, isMountDamage, isFriendlyFire, isHeadshot, killedAgentName, isUnconscious);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001A469 File Offset: 0x00018669
		public void OnPersonalDamage(int totalDamage, bool isVictimAgentMount, bool isFriendlyFire, string victimAgentName)
		{
			this.PersonalFeed.OnPersonalHit(totalDamage, isVictimAgentMount, isFriendlyFire, victimAgentName);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001A47B File Offset: 0x0001867B
		public void OnPersonalMessage(string message)
		{
			this.PersonalFeed.OnPersonalMessage(message);
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001A489 File Offset: 0x00018689
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0001A491 File Offset: 0x00018691
		[DataSourceProperty]
		public SPGeneralKillNotificationVM GeneralCasualty
		{
			get
			{
				return this._generalCasualty;
			}
			set
			{
				if (value != this._generalCasualty)
				{
					this._generalCasualty = value;
					base.OnPropertyChangedWithValue<SPGeneralKillNotificationVM>(value, "GeneralCasualty");
				}
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0001A4AF File Offset: 0x000186AF
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0001A4B7 File Offset: 0x000186B7
		[DataSourceProperty]
		public SPPersonalKillNotificationVM PersonalFeed
		{
			get
			{
				return this._personalFeed;
			}
			set
			{
				if (value != this._personalFeed)
				{
					this._personalFeed = value;
					base.OnPropertyChangedWithValue<SPPersonalKillNotificationVM>(value, "PersonalFeed");
				}
			}
		}

		// Token: 0x04000349 RID: 841
		private SPGeneralKillNotificationVM _generalCasualty;

		// Token: 0x0400034A RID: 842
		private SPPersonalKillNotificationVM _personalFeed;
	}
}
