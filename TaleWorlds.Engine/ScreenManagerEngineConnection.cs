using System;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x02000047 RID: 71
	public class ScreenManagerEngineConnection : IScreenManagerEngineConnection
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0000422F File Offset: 0x0000242F
		float IScreenManagerEngineConnection.RealScreenResolutionWidth
		{
			get
			{
				return Screen.RealScreenResolutionWidth;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00004236 File Offset: 0x00002436
		float IScreenManagerEngineConnection.RealScreenResolutionHeight
		{
			get
			{
				return Screen.RealScreenResolutionHeight;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0000423D File Offset: 0x0000243D
		float IScreenManagerEngineConnection.AspectRatio
		{
			get
			{
				return Screen.AspectRatio;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00004244 File Offset: 0x00002444
		Vec2 IScreenManagerEngineConnection.DesktopResolution
		{
			get
			{
				return Screen.DesktopResolution;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000424B File Offset: 0x0000244B
		void IScreenManagerEngineConnection.ActivateMouseCursor(CursorType mouseId)
		{
			MouseManager.ActivateMouseCursor(mouseId);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00004253 File Offset: 0x00002453
		void IScreenManagerEngineConnection.SetMouseVisible(bool value)
		{
			EngineApplicationInterface.IScreen.SetMouseVisible(value);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00004260 File Offset: 0x00002460
		bool IScreenManagerEngineConnection.GetMouseVisible()
		{
			return EngineApplicationInterface.IScreen.GetMouseVisible();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0000426C File Offset: 0x0000246C
		bool IScreenManagerEngineConnection.GetIsEnterButtonRDown()
		{
			return EngineApplicationInterface.IScreen.IsEnterButtonCross();
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00004278 File Offset: 0x00002478
		void IScreenManagerEngineConnection.BeginDebugPanel(string panelTitle)
		{
			Imgui.BeginMainThreadScope();
			Imgui.Begin(panelTitle);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00004285 File Offset: 0x00002485
		void IScreenManagerEngineConnection.EndDebugPanel()
		{
			Imgui.End();
			Imgui.EndMainThreadScope();
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00004291 File Offset: 0x00002491
		void IScreenManagerEngineConnection.DrawDebugText(string text)
		{
			Imgui.Text(text);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00004299 File Offset: 0x00002499
		bool IScreenManagerEngineConnection.DrawDebugTreeNode(string text)
		{
			return Imgui.TreeNode(text);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000042A1 File Offset: 0x000024A1
		void IScreenManagerEngineConnection.PopDebugTreeNode()
		{
			Imgui.TreePop();
		}
	}
}
