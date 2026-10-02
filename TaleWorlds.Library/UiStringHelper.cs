using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000030 RID: 48
	public static class UiStringHelper
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x00006DAC File Offset: 0x00004FAC
		public static bool IsStringNoneOrEmptyForUi(string str)
		{
			return string.IsNullOrEmpty(str) || str == "none";
		}
	}
}
