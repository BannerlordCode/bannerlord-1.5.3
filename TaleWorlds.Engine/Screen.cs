using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000086 RID: 134
	public static class Screen
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0000D85B File Offset: 0x0000BA5B
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x0000D862 File Offset: 0x0000BA62
		public static float RealScreenResolutionWidth { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0000D86A File Offset: 0x0000BA6A
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0000D871 File Offset: 0x0000BA71
		public static float RealScreenResolutionHeight { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0000D879 File Offset: 0x0000BA79
		public static Vec2 RealScreenResolution
		{
			get
			{
				return new Vec2(Screen.RealScreenResolutionWidth, Screen.RealScreenResolutionHeight);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0000D88A File Offset: 0x0000BA8A
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0000D891 File Offset: 0x0000BA91
		public static float AspectRatio { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0000D899 File Offset: 0x0000BA99
		// (set) Token: 0x06000C36 RID: 3126 RVA: 0x0000D8A0 File Offset: 0x0000BAA0
		public static Vec2 DesktopResolution { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0000D8A8 File Offset: 0x0000BAA8
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x0000D8AF File Offset: 0x0000BAAF
		public static Vec2 ScreenScale { get; private set; }

		// Token: 0x06000C39 RID: 3129 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		internal static void Update()
		{
			Screen.RealScreenResolutionWidth = EngineApplicationInterface.IScreen.GetRealScreenResolutionWidth();
			Screen.RealScreenResolutionHeight = EngineApplicationInterface.IScreen.GetRealScreenResolutionHeight();
			Screen.AspectRatio = EngineApplicationInterface.IScreen.GetAspectRatio();
			Screen.DesktopResolution = new Vec2(EngineApplicationInterface.IScreen.GetDesktopWidth(), EngineApplicationInterface.IScreen.GetDesktopHeight());
			Screen.ScreenScale = new Vec2(Screen.RealScreenResolutionWidth / Screen.DesktopResolution.x, Screen.RealScreenResolutionHeight / Screen.DesktopResolution.y);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0000D93A File Offset: 0x0000BB3A
		public static bool GetMouseVisible()
		{
			return EngineApplicationInterface.IScreen.GetMouseVisible();
		}
	}
}
