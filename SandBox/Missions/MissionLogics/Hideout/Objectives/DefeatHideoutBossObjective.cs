using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Hideout.Objectives
{
	// Token: 0x02000098 RID: 152
	internal class DefeatHideoutBossObjective : MissionObjective
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0002B1DC File Offset: 0x000293DC
		public override string UniqueId
		{
			get
			{
				return "hideout_mission_defeat_hideout_boss_objective";
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0002B1E3 File Offset: 0x000293E3
		public override TextObject Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0002B1EB File Offset: 0x000293EB
		public override TextObject Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002B1F4 File Offset: 0x000293F4
		public DefeatHideoutBossObjective(Mission mission, bool isDuel)
			: base(mission)
		{
			this._name = (isDuel ? new TextObject("{=QEynMlwL}Win the Duel", null) : new TextObject("{=0sPTRh6L}Win the Fight", null));
			this._description = (isDuel ? new TextObject("{=t13oVKkw}Win the duel against the bandit boss.", null) : new TextObject("{=7vqW1CsE}Eliminate the bandit boss and his troops.", null));
		}

		// Token: 0x04000369 RID: 873
		private readonly TextObject _name;

		// Token: 0x0400036A RID: 874
		private readonly TextObject _description;
	}
}
