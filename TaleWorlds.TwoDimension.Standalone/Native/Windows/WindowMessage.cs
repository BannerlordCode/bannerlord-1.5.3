using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000045 RID: 69
	public enum WindowMessage : uint
	{
		// Token: 0x040001A1 RID: 417
		Quit = 18U,
		// Token: 0x040001A2 RID: 418
		Close = 16U,
		// Token: 0x040001A3 RID: 419
		Size = 5U,
		// Token: 0x040001A4 RID: 420
		DisplayChange = 126U,
		// Token: 0x040001A5 RID: 421
		DpiChanged = 736U,
		// Token: 0x040001A6 RID: 422
		DeviceChange = 537U,
		// Token: 0x040001A7 RID: 423
		KeyDown = 256U,
		// Token: 0x040001A8 RID: 424
		KeyUp,
		// Token: 0x040001A9 RID: 425
		RightButtonUp = 517U,
		// Token: 0x040001AA RID: 426
		RightButtonDown = 516U,
		// Token: 0x040001AB RID: 427
		LeftButtonUp = 514U,
		// Token: 0x040001AC RID: 428
		LeftButtonDown = 513U,
		// Token: 0x040001AD RID: 429
		MouseMove = 512U,
		// Token: 0x040001AE RID: 430
		MouseWheel = 522U,
		// Token: 0x040001AF RID: 431
		KillFocus = 8U,
		// Token: 0x040001B0 RID: 432
		SetFocus = 7U
	}
}
