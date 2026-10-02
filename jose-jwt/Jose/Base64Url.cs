using System;

namespace Jose
{
	// Token: 0x0200002B RID: 43
	public static class Base64Url
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x000058EA File Offset: 0x00003AEA
		public static string Encode(byte[] input)
		{
			return Convert.ToBase64String(input).Split(new char[] { '=' })[0].Replace('+', '-').Replace('/', '_');
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005918 File Offset: 0x00003B18
		public static byte[] Decode(string input)
		{
			string text = input.Replace('-', '+');
			text = text.Replace('_', '/');
			switch (text.Length % 4)
			{
			case 0:
				goto IL_0065;
			case 2:
				text += "==";
				goto IL_0065;
			case 3:
				text += "=";
				goto IL_0065;
			}
			throw new ArgumentOutOfRangeException("input", "Illegal base64url string!");
			IL_0065:
			return Convert.FromBase64String(text);
		}
	}
}
