using System;

namespace TaleWorlds.PlayerServices
{
	// Token: 0x02000006 RID: 6
	public static class PlayerIdExtensions
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000029B7 File Offset: 0x00000BB7
		public static bool SupportsPlayerCard(this PlayerIdProvidedTypes type)
		{
			return type == PlayerIdProvidedTypes.GDK || type == PlayerIdProvidedTypes.PS;
		}
	}
}
