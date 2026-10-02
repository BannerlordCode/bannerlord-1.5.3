using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D5 RID: 213
	public static class TeamSideEnumExtensions
	{
		// Token: 0x06000B39 RID: 2873 RVA: 0x00024AAC File Offset: 0x00022CAC
		public static bool IsValid(this TeamSideEnum teamSide)
		{
			return teamSide >= TeamSideEnum.PlayerTeam && teamSide < TeamSideEnum.NumSides;
		}
	}
}
