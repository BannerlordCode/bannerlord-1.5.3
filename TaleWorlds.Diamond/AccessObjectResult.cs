using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000018 RID: 24
	public class AccessObjectResult
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002A53 File Offset: 0x00000C53
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002A5B File Offset: 0x00000C5B
		public AccessObject AccessObject { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002A64 File Offset: 0x00000C64
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002A6C File Offset: 0x00000C6C
		public bool Success { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002A75 File Offset: 0x00000C75
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002A7D File Offset: 0x00000C7D
		public TextObject FailReason { get; set; }

		// Token: 0x06000089 RID: 137 RVA: 0x00002A8E File Offset: 0x00000C8E
		public static AccessObjectResult CreateSuccess(AccessObject accessObject)
		{
			return new AccessObjectResult
			{
				Success = true,
				AccessObject = accessObject
			};
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002AA3 File Offset: 0x00000CA3
		public static AccessObjectResult CreateFailed(TextObject failReason)
		{
			return new AccessObjectResult
			{
				Success = false,
				FailReason = failReason
			};
		}
	}
}
