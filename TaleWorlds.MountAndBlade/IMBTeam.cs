using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BA RID: 442
	[ScriptingInterfaceBase]
	internal interface IMBTeam
	{
		// Token: 0x06001925 RID: 6437
		[EngineMethod("is_enemy", false, null, true)]
		bool IsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex);

		// Token: 0x06001926 RID: 6438
		[EngineMethod("set_is_enemy", false, null, false)]
		void SetIsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex, bool isEnemy);
	}
}
