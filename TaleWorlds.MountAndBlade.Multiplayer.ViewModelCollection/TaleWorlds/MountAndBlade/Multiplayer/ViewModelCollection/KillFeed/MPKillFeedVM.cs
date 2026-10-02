using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.General;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.Personal;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed
{
	// Token: 0x0200008A RID: 138
	public class MPKillFeedVM : ViewModel
	{
		// Token: 0x06000D8F RID: 3471 RVA: 0x00029ACC File Offset: 0x00027CCC
		public MPKillFeedVM()
		{
			this.GeneralCasualty = new MPGeneralKillNotificationVM();
			this.PersonalCasualty = new MPPersonalKillNotificationVM();
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00029AEC File Offset: 0x00027CEC
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, bool isPersonalFeedEnabled, WeaponClass killWeaponClass = WeaponClass.Undefined)
		{
			Agent assistedAgent = this.GetAssistedAgent(affectedAgent, affectorAgent);
			if (assistedAgent != null && assistedAgent.IsMainAgent && isPersonalFeedEnabled)
			{
				string text = affectedAgent.Name;
				if (affectedAgent.MissionPeer != null)
				{
					text = affectedAgent.MissionPeer.DisplayedName;
				}
				this.OnPersonalAssist(text);
			}
			this.GeneralCasualty.OnAgentRemoved(affectedAgent, affectorAgent, assistedAgent, killWeaponClass);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00029B44 File Offset: 0x00027D44
		private void OnPersonalAssist(string victimAgentName)
		{
			this.PersonalCasualty.OnPersonalAssist(victimAgentName);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00029B52 File Offset: 0x00027D52
		public void OnPersonalDamage(int damageAmount, bool isFatal, bool isMountDamage, bool isFriendlyDamage, bool isHeadshot, string killedAgentName)
		{
			this.PersonalCasualty.OnPersonalHit(damageAmount, isFatal, isMountDamage, isFriendlyDamage, isHeadshot, killedAgentName);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00029B68 File Offset: 0x00027D68
		private Agent GetAssistedAgent(Agent affectedAgent, Agent affectorAgent)
		{
			if (affectedAgent == null)
			{
				return null;
			}
			Agent.Hitter assistingHitter = affectedAgent.GetAssistingHitter((affectorAgent != null) ? affectorAgent.MissionPeer : null);
			if (assistingHitter == null)
			{
				return null;
			}
			MissionPeer hitterPeer = assistingHitter.HitterPeer;
			if (hitterPeer == null)
			{
				return null;
			}
			return hitterPeer.ControlledAgent;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00029B97 File Offset: 0x00027D97
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x00029B9F File Offset: 0x00027D9F
		[DataSourceProperty]
		public MPGeneralKillNotificationVM GeneralCasualty
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
					base.OnPropertyChangedWithValue<MPGeneralKillNotificationVM>(value, "GeneralCasualty");
				}
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00029BBD File Offset: 0x00027DBD
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x00029BC5 File Offset: 0x00027DC5
		[DataSourceProperty]
		public MPPersonalKillNotificationVM PersonalCasualty
		{
			get
			{
				return this._personalCasualty;
			}
			set
			{
				if (value != this._personalCasualty)
				{
					this._personalCasualty = value;
					base.OnPropertyChangedWithValue<MPPersonalKillNotificationVM>(value, "PersonalCasualty");
				}
			}
		}

		// Token: 0x04000630 RID: 1584
		private MPGeneralKillNotificationVM _generalCasualty;

		// Token: 0x04000631 RID: 1585
		private MPPersonalKillNotificationVM _personalCasualty;
	}
}
