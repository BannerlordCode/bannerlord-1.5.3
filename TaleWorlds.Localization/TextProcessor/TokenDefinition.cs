using System;
using System.Text.RegularExpressions;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000029 RID: 41
	internal class TokenDefinition
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000060C9 File Offset: 0x000042C9
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000060D1 File Offset: 0x000042D1
		public TokenType TokenType { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000060DA File Offset: 0x000042DA
		// (set) Token: 0x0600011F RID: 287 RVA: 0x000060E2 File Offset: 0x000042E2
		public int Precedence { get; private set; }

		// Token: 0x06000120 RID: 288 RVA: 0x000060EB File Offset: 0x000042EB
		public TokenDefinition(TokenType tokenType, string regexPattern, int precedence)
		{
			this._regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			this.TokenType = tokenType;
			this.Precedence = precedence;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006110 File Offset: 0x00004310
		internal Match CheckMatch(string str, int beginIndex)
		{
			beginIndex = this.SkipWhiteSpace(str, beginIndex);
			Match match = this._regex.Match(str, beginIndex);
			if (match.Success && match.Index == beginIndex)
			{
				return match;
			}
			return null;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000614C File Offset: 0x0000434C
		private int SkipWhiteSpace(string str, int beginIndex)
		{
			int num = beginIndex;
			int length = str.Length;
			while (num < length && char.IsWhiteSpace(str[num]))
			{
				num++;
			}
			return num;
		}

		// Token: 0x04000062 RID: 98
		private readonly Regex _regex;
	}
}
