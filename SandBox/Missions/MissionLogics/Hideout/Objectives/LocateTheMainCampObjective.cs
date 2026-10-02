using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Hideout.Objectives
{
	// Token: 0x02000099 RID: 153
	public class LocateTheMainCampObjective : MissionObjective
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0002B24A File Offset: 0x0002944A
		public override string UniqueId
		{
			get
			{
				return "hideout_mission_locate_the_main_camp_objective";
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0002B251 File Offset: 0x00029451
		public override TextObject Name
		{
			get
			{
				return new TextObject("{=2g03vuC7}Locate the Main Camp", null);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0002B25E File Offset: 0x0002945E
		public override TextObject Description
		{
			get
			{
				return new TextObject("{=wmvJ0bcH}Sneak your way through the sentries.", null);
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0002B26B File Offset: 0x0002946B
		public LocateTheMainCampObjective(Mission mission)
			: base(mission)
		{
		}
	}
}
