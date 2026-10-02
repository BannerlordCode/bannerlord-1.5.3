using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringReferenceExtensions
	{
		// Token: 0x060005C8 RID: 1480 RVA: 0x00018988 File Offset: 0x00016B88
		public static int IndexOf(this StringReference s, char c, int startIndex, int length)
		{
			int num = Array.IndexOf<char>(s.Chars, c, s.StartIndex + startIndex, length);
			if (num == -1)
			{
				return -1;
			}
			return num - s.StartIndex;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000189BC File Offset: 0x00016BBC
		public static bool StartsWith(this StringReference s, string text)
		{
			if (text.Length > s.Length)
			{
				return false;
			}
			char[] chars = s.Chars;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != chars[i + s.StartIndex])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00018A0C File Offset: 0x00016C0C
		public static bool EndsWith(this StringReference s, string text)
		{
			if (text.Length > s.Length)
			{
				return false;
			}
			char[] chars = s.Chars;
			int num = s.StartIndex + s.Length - text.Length;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != chars[i + num])
				{
					return false;
				}
			}
			return true;
		}
	}
}
