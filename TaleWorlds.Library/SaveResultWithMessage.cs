using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200008B RID: 139
	public struct SaveResultWithMessage
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00012492 File Offset: 0x00010692
		public static SaveResultWithMessage Default
		{
			get
			{
				return new SaveResultWithMessage(SaveResult.Success, string.Empty);
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001249F File Offset: 0x0001069F
		public SaveResultWithMessage(SaveResult result, string message)
		{
			this.SaveResult = result;
			this.Message = message;
		}

		// Token: 0x0400018F RID: 399
		public readonly SaveResult SaveResult;

		// Token: 0x04000190 RID: 400
		public readonly string Message;
	}
}
