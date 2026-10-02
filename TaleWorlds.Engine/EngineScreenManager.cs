using System;
using TaleWorlds.DotNet;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x02000046 RID: 70
	internal class EngineScreenManager
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x000041CA File Offset: 0x000023CA
		[EngineCallback(null, false)]
		internal static void PreTick(float dt)
		{
			ScreenManager.EarlyUpdate(EngineApplicationInterface.IScreen.GetUsableAreaPercentages());
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000041DB File Offset: 0x000023DB
		[EngineCallback(null, false)]
		public static void Tick(float dt)
		{
			ScreenManager.Tick(dt);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000041E3 File Offset: 0x000023E3
		[EngineCallback(null, false)]
		internal static void LateTick(float dt)
		{
			ScreenManager.LateTick(dt);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000041EB File Offset: 0x000023EB
		[EngineCallback(null, false)]
		internal static void OnOnscreenKeyboardDone(string inputText)
		{
			ScreenManager.OnOnscreenKeyboardDone(inputText);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000041F3 File Offset: 0x000023F3
		[EngineCallback(null, false)]
		internal static void OnOnscreenKeyboardCanceled()
		{
			ScreenManager.OnOnscreenKeyboardCanceled();
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000041FA File Offset: 0x000023FA
		[EngineCallback(null, false)]
		internal static void OnGameWindowFocusChange(bool focusGained)
		{
			ScreenManager.OnGameWindowFocusChange(focusGained);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00004202 File Offset: 0x00002402
		[EngineCallback(null, false)]
		internal static void Update()
		{
			ScreenManager.Update(EngineScreenManager._lastPressedKeys);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0000420E File Offset: 0x0000240E
		[EngineCallback(null, false)]
		internal static void InitializeLastPressedKeys(NativeArray lastKeysPressed)
		{
			EngineScreenManager._lastPressedKeys = new NativeArrayEnumerator<int>(lastKeysPressed);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0000421B File Offset: 0x0000241B
		internal static void Initialize()
		{
			ScreenManager.Initialize(new ScreenManagerEngineConnection());
		}

		// Token: 0x0400005B RID: 91
		private static NativeArrayEnumerator<int> _lastPressedKeys;
	}
}
