using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Towns.Objectives
{
	// Token: 0x02000092 RID: 146
	public class FindThePrisonerObjective : MissionObjective
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00025F32 File Offset: 0x00024132
		public override string UniqueId
		{
			get
			{
				return "prison_break_find_the_prisoner_objective";
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00025F39 File Offset: 0x00024139
		public override TextObject Name
		{
			get
			{
				return new TextObject("{=nxkYh5Ut}Find the Prisoner", null);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00025F46 File Offset: 0x00024146
		public override TextObject Description
		{
			get
			{
				return new TextObject("{=R7z9qNqS}Find and talk to the prisoner without alerting the guards.", null);
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00025F53 File Offset: 0x00024153
		public FindThePrisonerObjective(Mission mission)
			: base(mission)
		{
		}
	}
}
