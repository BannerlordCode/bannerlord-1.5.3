using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200003C RID: 60
	public struct NativeMessage
	{
		// Token: 0x04000159 RID: 345
		public IntPtr handle;

		// Token: 0x0400015A RID: 346
		public WindowMessage msg;

		// Token: 0x0400015B RID: 347
		public IntPtr wParam;

		// Token: 0x0400015C RID: 348
		public IntPtr lParam;

		// Token: 0x0400015D RID: 349
		public uint time;

		// Token: 0x0400015E RID: 350
		public Point p;
	}
}
