using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E4 RID: 740
	public class MonsterMissionDataCreator : IMonsterMissionDataCreator
	{
		// Token: 0x06002B36 RID: 11062 RVA: 0x000A6E52 File Offset: 0x000A5052
		IMonsterMissionData IMonsterMissionDataCreator.CreateMonsterMissionData(Monster monster)
		{
			return new MonsterMissionData(monster);
		}
	}
}
