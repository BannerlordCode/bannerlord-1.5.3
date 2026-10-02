using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005E RID: 94
	internal static class JsonTokenUtils
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x00016D36 File Offset: 0x00014F36
		internal static bool IsEndToken(JsonToken token)
		{
			return token - JsonToken.EndObject <= 2;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00016D42 File Offset: 0x00014F42
		internal static bool IsStartToken(JsonToken token)
		{
			return token - JsonToken.StartObject <= 2;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00016D4D File Offset: 0x00014F4D
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			return token - JsonToken.Integer <= 5 || token - JsonToken.Date <= 1;
		}
	}
}
