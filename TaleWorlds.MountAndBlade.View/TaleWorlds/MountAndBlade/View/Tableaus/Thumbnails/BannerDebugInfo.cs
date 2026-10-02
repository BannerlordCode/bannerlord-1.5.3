using System;
using System.Text;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003A RID: 58
	public struct BannerDebugInfo
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0000EC8C File Offset: 0x0000CE8C
		public static BannerDebugInfo CreateManual(string sourceName)
		{
			return new BannerDebugInfo
			{
				SourceName = sourceName,
				SourceType = BannerDebugInfo.SourceTypes.Manual
			};
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000ECB4 File Offset: 0x0000CEB4
		public static BannerDebugInfo CreateWidget(string sourceName)
		{
			return new BannerDebugInfo
			{
				SourceType = BannerDebugInfo.SourceTypes.Widget,
				SourceName = sourceName
			};
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public string CreateName()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type:");
			stringBuilder.Append(BannerDebugInfo.GetSourceTypeName(this.SourceType));
			stringBuilder.Append("name:");
			stringBuilder.Append(this.SourceName);
			return stringBuilder.ToString();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000ED2A File Offset: 0x0000CF2A
		private static string GetSourceTypeName(BannerDebugInfo.SourceTypes type)
		{
			switch (type)
			{
			case BannerDebugInfo.SourceTypes.Widget:
				return "Wi";
			case BannerDebugInfo.SourceTypes.Manual:
				return "Mn";
			}
			return "Un";
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000ED54 File Offset: 0x0000CF54
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("type: {0}_", this.SourceType));
			stringBuilder.Append("name: " + this.SourceName + "_");
			return stringBuilder.ToString();
		}

		// Token: 0x04000134 RID: 308
		public BannerDebugInfo.SourceTypes SourceType;

		// Token: 0x04000135 RID: 309
		public string SourceName;

		// Token: 0x020000C2 RID: 194
		public enum SourceTypes
		{
			// Token: 0x04000389 RID: 905
			Undefined,
			// Token: 0x0400038A RID: 906
			Widget,
			// Token: 0x0400038B RID: 907
			Manual
		}
	}
}
