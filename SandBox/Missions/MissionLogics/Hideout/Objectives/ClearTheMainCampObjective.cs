using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Hideout.Objectives
{
	// Token: 0x02000097 RID: 151
	public class ClearTheMainCampObjective : MissionObjective
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0002B161 File Offset: 0x00029361
		public override string UniqueId
		{
			get
			{
				return "hideout_mission_clear_the_main_camp_objective";
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0002B168 File Offset: 0x00029368
		public override TextObject Name
		{
			get
			{
				return new TextObject("{=OLWkIYxa}Clear the Main Camp", null);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0002B175 File Offset: 0x00029375
		public override TextObject Description
		{
			get
			{
				return new TextObject("{=lGZLiIey}Clear the main camp with your troops.", null);
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0002B182 File Offset: 0x00029382
		public ClearTheMainCampObjective(Mission mission, List<Agent> agents)
			: base(mission)
		{
			this._agents = agents;
			this._requiredProgressAmount = agents.Count;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0002B1A0 File Offset: 0x000293A0
		public override MissionObjectiveProgressInfo GetCurrentProgress()
		{
			return new MissionObjectiveProgressInfo
			{
				CurrentProgressAmount = this._requiredProgressAmount - this._agents.Count,
				RequiredProgressAmount = this._requiredProgressAmount
			};
		}

		// Token: 0x04000367 RID: 871
		private readonly List<Agent> _agents;

		// Token: 0x04000368 RID: 872
		private readonly int _requiredProgressAmount;
	}
}
