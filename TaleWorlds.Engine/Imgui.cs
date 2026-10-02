using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000053 RID: 83
	public class Imgui
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x00006AFF File Offset: 0x00004CFF
		public static void BeginMainThreadScope()
		{
			EngineApplicationInterface.IImgui.BeginMainThreadScope();
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00006B0B File Offset: 0x00004D0B
		public static void EndMainThreadScope()
		{
			EngineApplicationInterface.IImgui.EndMainThreadScope();
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00006B17 File Offset: 0x00004D17
		public static void PushStyleColor(Imgui.ColorStyle style, ref Vec3 color)
		{
			EngineApplicationInterface.IImgui.PushStyleColor((int)style, ref color);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00006B25 File Offset: 0x00004D25
		public static void PopStyleColor()
		{
			EngineApplicationInterface.IImgui.PopStyleColor();
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00006B31 File Offset: 0x00004D31
		public static void NewFrame()
		{
			EngineApplicationInterface.IImgui.NewFrame();
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00006B3D File Offset: 0x00004D3D
		public static void Render()
		{
			EngineApplicationInterface.IImgui.Render();
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00006B49 File Offset: 0x00004D49
		public static void Begin(string text)
		{
			EngineApplicationInterface.IImgui.Begin(text);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00006B56 File Offset: 0x00004D56
		public static void Begin(string text, ref bool is_open)
		{
			EngineApplicationInterface.IImgui.BeginWithCloseButton(text, ref is_open);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00006B64 File Offset: 0x00004D64
		public static void End()
		{
			EngineApplicationInterface.IImgui.End();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00006B70 File Offset: 0x00004D70
		public static void Text(string text)
		{
			EngineApplicationInterface.IImgui.Text(text);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00006B7D File Offset: 0x00004D7D
		public static bool Checkbox(string text, ref bool is_checked)
		{
			return EngineApplicationInterface.IImgui.Checkbox(text, ref is_checked);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00006B8B File Offset: 0x00004D8B
		public static bool TreeNode(string name)
		{
			return EngineApplicationInterface.IImgui.TreeNode(name);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00006B98 File Offset: 0x00004D98
		public static void TreePop()
		{
			EngineApplicationInterface.IImgui.TreePop();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00006BA4 File Offset: 0x00004DA4
		public static void Separator()
		{
			EngineApplicationInterface.IImgui.Separator();
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00006BB0 File Offset: 0x00004DB0
		public static bool Button(string text)
		{
			return EngineApplicationInterface.IImgui.Button(text);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00006BC0 File Offset: 0x00004DC0
		public static void PlotLines(string name, float[] values, int valuesCount, int valuesOffset, string overlayText, float minScale, float maxScale, float graphWidth, float graphHeight, int stride)
		{
			EngineApplicationInterface.IImgui.PlotLines(name, values, valuesCount, valuesOffset, overlayText, minScale, maxScale, graphWidth, graphHeight, stride);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00006BE7 File Offset: 0x00004DE7
		public static void ProgressBar(float progress)
		{
			EngineApplicationInterface.IImgui.ProgressBar(progress);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00006BF4 File Offset: 0x00004DF4
		public static void NewLine()
		{
			EngineApplicationInterface.IImgui.NewLine();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00006C00 File Offset: 0x00004E00
		public static void SameLine(float posX = 0f, float spacingWidth = 0f)
		{
			EngineApplicationInterface.IImgui.SameLine(posX, spacingWidth);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00006C0E File Offset: 0x00004E0E
		public static bool Combo(string label, ref int selectedIndex, string items)
		{
			return EngineApplicationInterface.IImgui.Combo(label, ref selectedIndex, items);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00006C1D File Offset: 0x00004E1D
		public static bool ComboCustomSeperator(string label, ref int selectedIndex, string items, char seperator)
		{
			return EngineApplicationInterface.IImgui.ComboCustomSeperator(label, ref selectedIndex, items, seperator.ToString());
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00006C33 File Offset: 0x00004E33
		public static bool InputInt(string label, ref int value)
		{
			return EngineApplicationInterface.IImgui.InputInt(label, ref value);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00006C41 File Offset: 0x00004E41
		public static bool SliderFloat(string label, ref float value, float min, float max)
		{
			return EngineApplicationInterface.IImgui.SliderFloat(label, ref value, min, max);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00006C51 File Offset: 0x00004E51
		public static void Columns(int count = 1, string id = "", bool border = true)
		{
			EngineApplicationInterface.IImgui.Columns(count, id, border);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00006C60 File Offset: 0x00004E60
		public static void NextColumn()
		{
			EngineApplicationInterface.IImgui.NextColumn();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00006C6C File Offset: 0x00004E6C
		public static bool RadioButton(string label, bool active)
		{
			return EngineApplicationInterface.IImgui.RadioButton(label, active);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00006C7A File Offset: 0x00004E7A
		public static bool CollapsingHeader(string label)
		{
			return EngineApplicationInterface.IImgui.CollapsingHeader(label);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00006C87 File Offset: 0x00004E87
		public static bool IsItemHovered()
		{
			return EngineApplicationInterface.IImgui.IsItemHovered();
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00006C93 File Offset: 0x00004E93
		public static void SetTooltip(string label)
		{
			EngineApplicationInterface.IImgui.SetTooltip(label);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00006CA0 File Offset: 0x00004EA0
		public static bool SmallButton(string label)
		{
			return EngineApplicationInterface.IImgui.SmallButton(label);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00006CAD File Offset: 0x00004EAD
		public static bool InputFloat(string label, ref float val, float step, float stepFast, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat(label, ref val, step, stepFast, decimalPrecision);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00006CC0 File Offset: 0x00004EC0
		public static bool InputText(string label, ref string text)
		{
			bool flag = false;
			text = EngineApplicationInterface.IImgui.InputText(label, text, ref flag);
			return flag;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00006CE4 File Offset: 0x00004EE4
		public static bool InputTextMultilineCopyPaste(string label, int textBoxHeight, ref string text)
		{
			bool flag = false;
			text = EngineApplicationInterface.IImgui.InputTextMultilineCopyPaste(label, text, textBoxHeight, ref flag);
			return flag;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00006D06 File Offset: 0x00004F06
		public static bool InputFloat2(string label, ref float val0, ref float val1, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat2(label, ref val0, ref val1, decimalPrecision);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00006D16 File Offset: 0x00004F16
		public static bool InputFloat3(string label, ref float val0, ref float val1, ref float val2, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat3(label, ref val0, ref val1, ref val2, decimalPrecision);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00006D28 File Offset: 0x00004F28
		public static bool InputFloat4(string label, ref float val0, ref float val1, ref float val2, ref float val3, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat4(label, ref val0, ref val1, ref val2, ref val3, decimalPrecision);
		}

		// Token: 0x020000C3 RID: 195
		public enum ColorStyle
		{
			// Token: 0x040003BE RID: 958
			Text,
			// Token: 0x040003BF RID: 959
			TextDisabled,
			// Token: 0x040003C0 RID: 960
			WindowBg,
			// Token: 0x040003C1 RID: 961
			ChildWindowBg,
			// Token: 0x040003C2 RID: 962
			PopupBg,
			// Token: 0x040003C3 RID: 963
			Border,
			// Token: 0x040003C4 RID: 964
			BorderShadow,
			// Token: 0x040003C5 RID: 965
			FrameBg,
			// Token: 0x040003C6 RID: 966
			FrameBgHovered,
			// Token: 0x040003C7 RID: 967
			FrameBgActive,
			// Token: 0x040003C8 RID: 968
			TitleBg,
			// Token: 0x040003C9 RID: 969
			TitleBgCollapsed,
			// Token: 0x040003CA RID: 970
			TitleBgActive,
			// Token: 0x040003CB RID: 971
			MenuBarBg,
			// Token: 0x040003CC RID: 972
			ScrollbarBg,
			// Token: 0x040003CD RID: 973
			ScrollbarGrab,
			// Token: 0x040003CE RID: 974
			ScrollbarGrabHovered,
			// Token: 0x040003CF RID: 975
			ScrollbarGrabActive,
			// Token: 0x040003D0 RID: 976
			ComboBg,
			// Token: 0x040003D1 RID: 977
			CheckMark,
			// Token: 0x040003D2 RID: 978
			SliderGrab,
			// Token: 0x040003D3 RID: 979
			SliderGrabActive,
			// Token: 0x040003D4 RID: 980
			Button,
			// Token: 0x040003D5 RID: 981
			ButtonHovered,
			// Token: 0x040003D6 RID: 982
			ButtonActive,
			// Token: 0x040003D7 RID: 983
			Header,
			// Token: 0x040003D8 RID: 984
			HeaderHovered,
			// Token: 0x040003D9 RID: 985
			HeaderActive,
			// Token: 0x040003DA RID: 986
			Column,
			// Token: 0x040003DB RID: 987
			ColumnHovered,
			// Token: 0x040003DC RID: 988
			ColumnActive,
			// Token: 0x040003DD RID: 989
			ResizeGrip,
			// Token: 0x040003DE RID: 990
			ResizeGripHovered,
			// Token: 0x040003DF RID: 991
			ResizeGripActive,
			// Token: 0x040003E0 RID: 992
			CloseButton,
			// Token: 0x040003E1 RID: 993
			CloseButtonHovered,
			// Token: 0x040003E2 RID: 994
			CloseButtonActive,
			// Token: 0x040003E3 RID: 995
			PlotLines,
			// Token: 0x040003E4 RID: 996
			PlotLinesHovered,
			// Token: 0x040003E5 RID: 997
			PlotHistogram,
			// Token: 0x040003E6 RID: 998
			PlotHistogramHovered,
			// Token: 0x040003E7 RID: 999
			TextSelectedBg,
			// Token: 0x040003E8 RID: 1000
			ModalWindowDarkening
		}
	}
}
