using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Towns.Objectives
{
	// Token: 0x02000091 RID: 145
	public class EscapeThePrisonObjective : MissionObjective
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00025F08 File Offset: 0x00024108
		public override string UniqueId
		{
			get
			{
				return "prison_break_escape_the_prison_objective";
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00025F0F File Offset: 0x0002410F
		public override TextObject Name
		{
			get
			{
				return new TextObject("{=LLZCYIzm}Escape the Prison", null);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00025F1C File Offset: 0x0002411C
		public override TextObject Description
		{
			get
			{
				return new TextObject("{=ibdGpCkR}Reach the exit and escape with the prisoner.", null);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00025F29 File Offset: 0x00024129
		public EscapeThePrisonObjective(Mission mission)
			: base(mission)
		{
		}
	}
}
