using System;
using System.Text.RegularExpressions;

namespace TaleWorlds.Library
{
	// Token: 0x0200009F RID: 159
	public class UniqueSceneId
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00013AA0 File Offset: 0x00011CA0
		public string UniqueToken { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00013AA8 File Offset: 0x00011CA8
		public string Revision { get; }

		// Token: 0x0600059D RID: 1437 RVA: 0x00013AB0 File Offset: 0x00011CB0
		public UniqueSceneId(string uniqueToken, string revision)
		{
			if (uniqueToken == null)
			{
				throw new ArgumentNullException("uniqueToken");
			}
			this.UniqueToken = uniqueToken;
			if (revision == null)
			{
				throw new ArgumentNullException("revision");
			}
			this.Revision = revision;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00013AE4 File Offset: 0x00011CE4
		public string Serialize()
		{
			return string.Format(":ut[{0}]{1}:rev[{2}]{3}", new object[]
			{
				this.UniqueToken.Length,
				this.UniqueToken,
				this.Revision.Length,
				this.Revision
			});
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00013B3C File Offset: 0x00011D3C
		public static bool TryParse(string uniqueMapId, out UniqueSceneId identifiers)
		{
			identifiers = null;
			if (uniqueMapId == null)
			{
				return false;
			}
			Match match = UniqueSceneId.IdentifierPattern.Value.Match(uniqueMapId);
			if (match.Success)
			{
				identifiers = new UniqueSceneId(match.Groups[1].Value, match.Groups[2].Value);
				return true;
			}
			return false;
		}

		// Token: 0x040001B9 RID: 441
		private static readonly Lazy<Regex> IdentifierPattern = new Lazy<Regex>(() => new Regex("^:ut\\[\\d+\\](.*):rev\\[\\d+\\](.*)$", RegexOptions.Compiled));
	}
}
