using System;

namespace TaleWorlds.MountAndBlade.DedicatedCustomServer.ClientHelper
{
	// Token: 0x02000007 RID: 7
	public class ProgressUpdate
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000057 RID: 87 RVA: 0x0000302D File Offset: 0x0000122D
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003035 File Offset: 0x00001235
		public long BytesRead { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000303E File Offset: 0x0000123E
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003046 File Offset: 0x00001246
		public long TotalBytes { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000304F File Offset: 0x0000124F
		public float MegaBytesRead
		{
			get
			{
				return (float)this.BytesRead / 1048576f;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005C RID: 92 RVA: 0x0000305E File Offset: 0x0000125E
		public float TotalMegaBytes
		{
			get
			{
				return (float)this.TotalBytes / 1048576f;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000306D File Offset: 0x0000126D
		public ProgressUpdate(long bytesRead, long totalBytes)
		{
			this.BytesRead = bytesRead;
			this.TotalBytes = totalBytes;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003083 File Offset: 0x00001283
		public float ProgressRatio
		{
			get
			{
				return (float)this.BytesRead / (float)this.TotalBytes;
			}
		}

		// Token: 0x04000026 RID: 38
		private const float bytesPerMegaByte = 1048576f;
	}
}
