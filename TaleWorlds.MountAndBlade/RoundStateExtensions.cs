using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031F RID: 799
	public static class RoundStateExtensions
	{
		// Token: 0x06002DDB RID: 11739 RVA: 0x000B2764 File Offset: 0x000B0964
		public static bool StateHasVisualTimer(this MultiplayerRoundState roundState)
		{
			return roundState - MultiplayerRoundState.Preparation <= 1;
		}
	}
}
