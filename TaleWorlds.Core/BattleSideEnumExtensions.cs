using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000020 RID: 32
	public static class BattleSideEnumExtensions
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00006A70 File Offset: 0x00004C70
		public static bool IsValid(this BattleSideEnum battleSide)
		{
			return battleSide >= BattleSideEnum.Defender && battleSide < BattleSideEnum.NumSides;
		}
	}
}
