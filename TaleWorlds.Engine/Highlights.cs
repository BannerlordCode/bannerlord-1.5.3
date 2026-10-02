using System;
using System.Collections.Generic;

namespace TaleWorlds.Engine
{
	// Token: 0x02000051 RID: 81
	public class Highlights
	{
		// Token: 0x0600087D RID: 2173 RVA: 0x00006A71 File Offset: 0x00004C71
		public static void Initialize()
		{
			EngineApplicationInterface.IHighlights.Initialize();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00006A7D File Offset: 0x00004C7D
		public static void OpenGroup(string id)
		{
			EngineApplicationInterface.IHighlights.OpenGroup(id);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00006A8A File Offset: 0x00004C8A
		public static void CloseGroup(string id, bool destroy = false)
		{
			EngineApplicationInterface.IHighlights.CloseGroup(id, destroy);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00006A98 File Offset: 0x00004C98
		public static void SaveScreenshot(string highlightId, string groupId)
		{
			EngineApplicationInterface.IHighlights.SaveScreenshot(highlightId, groupId);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00006AA6 File Offset: 0x00004CA6
		public static void SaveVideo(string highlightId, string groupId, int startDelta, int endDelta)
		{
			EngineApplicationInterface.IHighlights.SaveVideo(highlightId, groupId, startDelta, endDelta);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00006AB8 File Offset: 0x00004CB8
		public static void OpenSummary(List<string> groups)
		{
			string text = string.Join("::", groups);
			EngineApplicationInterface.IHighlights.OpenSummary(text);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00006ADC File Offset: 0x00004CDC
		public static void AddHighlight(string id, string name)
		{
			EngineApplicationInterface.IHighlights.AddHighlight(id, name);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00006AEA File Offset: 0x00004CEA
		public static void RemoveHighlight(string id)
		{
			EngineApplicationInterface.IHighlights.RemoveHighlight(id);
		}

		// Token: 0x020000C1 RID: 193
		public enum Significance
		{
			// Token: 0x040003AC RID: 940
			None,
			// Token: 0x040003AD RID: 941
			ExtremelyBad,
			// Token: 0x040003AE RID: 942
			VeryBad,
			// Token: 0x040003AF RID: 943
			Bad = 4,
			// Token: 0x040003B0 RID: 944
			Neutral = 16,
			// Token: 0x040003B1 RID: 945
			Good = 256,
			// Token: 0x040003B2 RID: 946
			VeryGood = 512,
			// Token: 0x040003B3 RID: 947
			ExtremelyGoods = 1024,
			// Token: 0x040003B4 RID: 948
			Max = 2048
		}

		// Token: 0x020000C2 RID: 194
		public enum Type
		{
			// Token: 0x040003B6 RID: 950
			None,
			// Token: 0x040003B7 RID: 951
			Milestone,
			// Token: 0x040003B8 RID: 952
			Achievement,
			// Token: 0x040003B9 RID: 953
			Incident = 4,
			// Token: 0x040003BA RID: 954
			StateChange = 8,
			// Token: 0x040003BB RID: 955
			Unannounced = 16,
			// Token: 0x040003BC RID: 956
			Max = 32
		}
	}
}
