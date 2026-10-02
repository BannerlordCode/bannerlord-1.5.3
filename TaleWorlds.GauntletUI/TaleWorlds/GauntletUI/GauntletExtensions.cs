using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000024 RID: 36
	public static class GauntletExtensions
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0000EABC File Offset: 0x0000CCBC
		public static void SetGlobalAlphaRecursively(this Widget widget, float alphaFactor)
		{
			widget.SetAlpha(alphaFactor);
			List<Widget> children = widget.Children;
			for (int i = 0; i < children.Count; i++)
			{
				children[i].SetGlobalAlphaRecursively(alphaFactor);
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000EAF8 File Offset: 0x0000CCF8
		public static void SetAlpha(this Widget widget, float alphaFactor)
		{
			BrushWidget brushWidget;
			if ((brushWidget = widget as BrushWidget) != null)
			{
				brushWidget.Brush.GlobalAlphaFactor = alphaFactor;
			}
			TextureWidget textureWidget;
			if ((textureWidget = widget as TextureWidget) != null)
			{
				textureWidget.Brush.GlobalAlphaFactor = alphaFactor;
			}
			widget.AlphaFactor = alphaFactor;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000EB38 File Offset: 0x0000CD38
		public static void RegisterBrushStatesOfWidget(this Widget widget)
		{
			BrushWidget brushWidget;
			if ((brushWidget = widget as BrushWidget) != null)
			{
				foreach (Style style in brushWidget.ReadOnlyBrush.Styles)
				{
					if (!widget.ContainsState(style.Name))
					{
						widget.AddState(style.Name);
					}
				}
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000EBB0 File Offset: 0x0000CDB0
		public static string GetFullIDPath(this Widget widget)
		{
			StringBuilder stringBuilder = new StringBuilder(string.IsNullOrEmpty(widget.Id) ? widget.GetType().Name : widget.Id);
			for (Widget widget2 = widget.ParentWidget; widget2 != null; widget2 = widget2.ParentWidget)
			{
				stringBuilder.Insert(0, (string.IsNullOrEmpty(widget2.Id) ? widget2.GetType().Name : widget2.Id) + "\\");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000EC30 File Offset: 0x0000CE30
		public static void ApplyActionForThisAndAllChildren(this Widget widget, Action<Widget> action)
		{
			action(widget);
			List<Widget> children = widget.Children;
			for (int i = 0; i < children.Count; i++)
			{
				children[i].ApplyActionForThisAndAllChildren(action);
			}
		}
	}
}
