using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200002F RID: 47
	public static class EnumHelper
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00006CF4 File Offset: 0x00004EF4
		public static ulong GetCombinedULongEnumFlagsValue(Type type)
		{
			ulong num = 0UL;
			foreach (object obj in Enum.GetValues(type))
			{
				num |= (ulong)obj;
			}
			return num;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006D50 File Offset: 0x00004F50
		public static uint GetCombinedUIntEnumFlagsValue(Type type)
		{
			uint num = 0U;
			foreach (object obj in Enum.GetValues(type))
			{
				num |= (uint)obj;
			}
			return num;
		}
	}
}
