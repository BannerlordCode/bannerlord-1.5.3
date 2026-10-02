using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.Layout
{
	// Token: 0x02000047 RID: 71
	public class StackLayout : ILayout
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0001101D File Offset: 0x0000F21D
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00011025 File Offset: 0x0000F225
		public ContainerItemDescription DefaultItemDescription { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0001102E File Offset: 0x0000F22E
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00011036 File Offset: 0x0000F236
		public LayoutMethod LayoutMethod { get; set; }

		// Token: 0x06000444 RID: 1092 RVA: 0x0001103F File Offset: 0x0000F23F
		public StackLayout()
		{
			this.DefaultItemDescription = new ContainerItemDescription();
			this._layoutBoxes = new Dictionary<int, LayoutBox>(64);
			this._parallelMeasureBasicChildDelegate = new TWParallel.ParallelForAuxPredicate(this.ParallelMeasureBasicChild);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00011074 File Offset: 0x0000F274
		public ContainerItemDescription GetItemDescription(Widget owner, Widget child, int childIndex)
		{
			Container container;
			if ((container = owner as Container) != null)
			{
				return container.GetItemDescription(child.Id, childIndex);
			}
			return this.DefaultItemDescription;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000110A0 File Offset: 0x0000F2A0
		public Vector2 MeasureChildren(Widget widget, Vector2 measureSpec, SpriteData spriteData, float renderScale)
		{
			Container container = widget as Container;
			Vector2 vector = default(Vector2);
			if (widget.ChildCount > 0)
			{
				if (this.LayoutMethod == LayoutMethod.HorizontalLeftToRight || this.LayoutMethod == LayoutMethod.HorizontalRightToLeft || this.LayoutMethod == LayoutMethod.HorizontalCentered || this.LayoutMethod == LayoutMethod.HorizontalSpaced)
				{
					vector = this.MeasureLinear(widget, measureSpec, AlignmentAxis.Horizontal);
					if (container != null && container.IsDragHovering)
					{
						vector.X += 20f;
					}
				}
				else if (this.LayoutMethod == LayoutMethod.VerticalTopToBottom || this.LayoutMethod == LayoutMethod.VerticalBottomToTop || this.LayoutMethod == LayoutMethod.VerticalCentered || this.LayoutMethod == LayoutMethod.VerticalSpaced)
				{
					vector = this.MeasureLinear(widget, measureSpec, AlignmentAxis.Vertical);
					if (container != null && container.IsDragHovering)
					{
						vector.Y += 20f;
					}
				}
			}
			return vector;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001115C File Offset: 0x0000F35C
		public void OnLayout(Widget widget, float left, float bottom, float right, float top)
		{
			if (this.LayoutMethod == LayoutMethod.HorizontalLeftToRight || this.LayoutMethod == LayoutMethod.HorizontalRightToLeft || this.LayoutMethod == LayoutMethod.HorizontalCentered || this.LayoutMethod == LayoutMethod.HorizontalSpaced)
			{
				this.LayoutLinearHorizontal(widget, left, bottom, right, top);
				return;
			}
			if (this.LayoutMethod == LayoutMethod.VerticalTopToBottom || this.LayoutMethod == LayoutMethod.VerticalBottomToTop || this.LayoutMethod == LayoutMethod.VerticalCentered || this.LayoutMethod == LayoutMethod.VerticalSpaced)
			{
				this.LayoutLinearVertical(widget, left, bottom, right, top);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000111CB File Offset: 0x0000F3CB
		private static float GetData(Vector2 vector2, int row)
		{
			if (row == 0)
			{
				return vector2.X;
			}
			return vector2.Y;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000111DD File Offset: 0x0000F3DD
		private static void SetData(ref Vector2 vector2, int row, float data)
		{
			if (row == 0)
			{
				vector2.X = data;
			}
			vector2.Y = data;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000111F0 File Offset: 0x0000F3F0
		public int GetIndexForDrop(Container widget, Vector2 draggedWidgetPosition)
		{
			int num = 0;
			if (this.LayoutMethod == LayoutMethod.VerticalTopToBottom || this.LayoutMethod == LayoutMethod.VerticalBottomToTop || this.LayoutMethod == LayoutMethod.VerticalCentered || this.LayoutMethod == LayoutMethod.VerticalSpaced)
			{
				num = 1;
			}
			bool flag = this.LayoutMethod == LayoutMethod.HorizontalRightToLeft || this.LayoutMethod == LayoutMethod.VerticalBottomToTop;
			float data = StackLayout.GetData(draggedWidgetPosition, num);
			int num2 = 0;
			bool flag2 = false;
			int num3 = 0;
			while (num3 != widget.ChildCount && !flag2)
			{
				Widget child = widget.GetChild(num3);
				if (child != null)
				{
					float data2 = StackLayout.GetData(child.GlobalPosition * child.Context.CustomScale, num);
					float num4 = data2 + StackLayout.GetData(child.Size, num);
					float num5 = (data2 + num4) / 2f;
					if (!flag)
					{
						if (data < num5)
						{
							num2 = num3;
							flag2 = true;
						}
					}
					else if (data > num5)
					{
						num2 = num3;
						flag2 = true;
					}
				}
				num3++;
			}
			if (!flag2)
			{
				num2 = widget.ChildCount;
			}
			return num2;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000112D0 File Offset: 0x0000F4D0
		private void ParallelMeasureBasicChild(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				Widget child = this._parallelMeasureBasicChildWidget.GetChild(i);
				if (child == null)
				{
					Debug.FailedAssert("Trying to measure a null child for parent" + this._parallelMeasureBasicChildWidget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "ParallelMeasureBasicChild", 185);
				}
				else if (child.IsVisible)
				{
					AlignmentAxis parallelMeasureBasicChildAlignmentAxis = this._parallelMeasureBasicChildAlignmentAxis;
					if (parallelMeasureBasicChildAlignmentAxis != AlignmentAxis.Horizontal)
					{
						if (parallelMeasureBasicChildAlignmentAxis == AlignmentAxis.Vertical)
						{
							if (child.HeightSizePolicy != SizePolicy.StretchToParent)
							{
								child.Measure(this._parallelMeasureBasicChildMeasureSpec);
							}
						}
					}
					else if (child.WidthSizePolicy != SizePolicy.StretchToParent)
					{
						child.Measure(this._parallelMeasureBasicChildMeasureSpec);
					}
				}
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00011370 File Offset: 0x0000F570
		private Vector2 MeasureLinear(Widget widget, Vector2 measureSpec, AlignmentAxis alignmentAxis)
		{
			this._parallelMeasureBasicChildWidget = widget;
			this._parallelMeasureBasicChildMeasureSpec = measureSpec;
			this._parallelMeasureBasicChildAlignmentAxis = alignmentAxis;
			TWParallel.ForWithoutRenderThread(0, widget.ChildCount, this._parallelMeasureBasicChildDelegate, 64);
			this._parallelMeasureBasicChildWidget = null;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int num4 = 0;
			for (int i = 0; i < widget.ChildCount; i++)
			{
				Widget child = widget.GetChild(i);
				if (child == null)
				{
					Debug.FailedAssert("Trying to measure a null child for parent" + widget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "MeasureLinear", 235);
				}
				else if (child.IsVisible)
				{
					ContainerItemDescription itemDescription = this.GetItemDescription(widget, child, i);
					if (alignmentAxis == AlignmentAxis.Horizontal)
					{
						if (child.WidthSizePolicy == SizePolicy.StretchToParent)
						{
							num4++;
							num3 += itemDescription.WidthStretchRatio;
						}
						else
						{
							num2 += child.MeasuredSize.X + child.ScaledMarginLeft + child.ScaledMarginRight;
						}
						num = MathF.Max(num, child.MeasuredSize.Y + child.ScaledMarginTop + child.ScaledMarginBottom);
					}
					else if (alignmentAxis == AlignmentAxis.Vertical)
					{
						if (child.HeightSizePolicy == SizePolicy.StretchToParent)
						{
							num4++;
							num3 += itemDescription.HeightStretchRatio;
						}
						else
						{
							num += child.MeasuredSize.Y + child.ScaledMarginTop + child.ScaledMarginBottom;
						}
						num2 = MathF.Max(num2, child.MeasuredSize.X + child.ScaledMarginLeft + child.ScaledMarginRight);
					}
				}
			}
			if (num4 > 0)
			{
				float num5 = 0f;
				if (alignmentAxis == AlignmentAxis.Horizontal)
				{
					num5 = measureSpec.X - num2;
				}
				else if (alignmentAxis == AlignmentAxis.Vertical)
				{
					num5 = measureSpec.Y - num;
				}
				float num6 = num5;
				int num7 = num4;
				for (int j = 0; j < widget.ChildCount; j++)
				{
					Widget child2 = widget.GetChild(j);
					if (child2 == null)
					{
						Debug.FailedAssert("Trying to measure a null child for parent" + widget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "MeasureLinear", 297);
					}
					else if (child2.IsVisible && ((alignmentAxis == AlignmentAxis.Horizontal && child2.WidthSizePolicy == SizePolicy.StretchToParent) || (alignmentAxis == AlignmentAxis.Vertical && child2.HeightSizePolicy == SizePolicy.StretchToParent)))
					{
						ContainerItemDescription itemDescription2 = this.GetItemDescription(widget, child2, j);
						Vector2 vector = new Vector2(0f, 0f);
						if (num6 <= 0f)
						{
							if (alignmentAxis == AlignmentAxis.Horizontal)
							{
								vector = new Vector2(0f, measureSpec.Y);
							}
							else if (alignmentAxis == AlignmentAxis.Vertical)
							{
								vector = new Vector2(measureSpec.X, 0f);
							}
						}
						else if (alignmentAxis == AlignmentAxis.Horizontal)
						{
							float num8 = num5 * itemDescription2.WidthStretchRatio / num3;
							if (num7 == 1)
							{
								num8 = num6;
							}
							vector = new Vector2(num8, measureSpec.Y);
						}
						else if (alignmentAxis == AlignmentAxis.Vertical)
						{
							float num9 = num5 * itemDescription2.HeightStretchRatio / num3;
							if (num7 == 1)
							{
								num9 = num6;
							}
							vector = new Vector2(measureSpec.X, num9);
						}
						child2.Measure(vector);
						num7--;
						Vector2 measuredSize = child2.MeasuredSize;
						measuredSize.X += child2.ScaledMarginLeft + child2.ScaledMarginRight;
						measuredSize.Y += child2.ScaledMarginTop + child2.ScaledMarginBottom;
						if (alignmentAxis == AlignmentAxis.Horizontal)
						{
							num6 -= measuredSize.X;
							num2 += measuredSize.X;
							num = MathF.Max(num, measuredSize.Y);
						}
						else if (alignmentAxis == AlignmentAxis.Vertical)
						{
							num6 -= measuredSize.Y;
							num += measuredSize.Y;
							num2 = MathF.Max(num2, measuredSize.X);
						}
					}
				}
			}
			float num10 = num2;
			float num11 = num;
			return new Vector2(num10, num11);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000116F4 File Offset: 0x0000F8F4
		private void ParallelUpdateLayouts(Widget widget)
		{
			StackLayout.<>c__DisplayClass23_0 CS$<>8__locals1 = new StackLayout.<>c__DisplayClass23_0();
			CS$<>8__locals1.widget = widget;
			CS$<>8__locals1.<>4__this = this;
			TWParallel.ForWithoutRenderThread(0, CS$<>8__locals1.widget.ChildCount, new TWParallel.ParallelForAuxPredicate(CS$<>8__locals1.<ParallelUpdateLayouts>g__UpdateChildLayoutMT|0), 16);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00011734 File Offset: 0x0000F934
		private void LayoutLinearHorizontal(Widget widget, float left, float bottom, float right, float top)
		{
			Container container = widget as Container;
			float num = 0f;
			float num2 = 0f;
			float num3 = right - left;
			float num4 = bottom - top;
			this._layoutBoxes.Clear();
			int num5 = 0;
			float num6 = 0f;
			for (int i = 0; i < widget.ChildCount; i++)
			{
				Widget child = widget.GetChild(i);
				if (child == null)
				{
					Debug.FailedAssert("Trying to measure a null child for parent" + widget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "LayoutLinearHorizontal", 418);
				}
				else if (child.IsVisible)
				{
					num5++;
					num6 += child.MeasuredSize.X + child.ScaledMarginLeft + child.ScaledMarginRight;
				}
			}
			if (container != null && container.IsDragHovering)
			{
				num6 += 20f;
			}
			if (this.LayoutMethod == LayoutMethod.HorizontalCentered || (this.LayoutMethod == LayoutMethod.HorizontalSpaced && num5 == 1))
			{
				num = (right - left) / 2f - num6 / 2f;
			}
			if (num5 > 0)
			{
				float num7 = right - left - num6;
				float num8 = ((num5 > 1) ? (num7 / (float)(num5 - 1)) : 0f);
				for (int j = 0; j < widget.ChildCount; j++)
				{
					Widget child2 = widget.GetChild(j);
					if (child2 == null)
					{
						Debug.FailedAssert("Trying to measure a null child for parent" + widget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "LayoutLinearHorizontal", 451);
					}
					else
					{
						if (container != null && container.IsDragHovering && j == container.DragHoverInsertionIndex)
						{
							if (this.LayoutMethod == LayoutMethod.HorizontalRightToLeft)
							{
								num3 -= 20f;
							}
							else
							{
								num += 20f;
							}
						}
						if (child2.IsVisible)
						{
							float num9 = child2.MeasuredSize.X + child2.ScaledMarginLeft + child2.ScaledMarginRight;
							if (this.LayoutMethod == LayoutMethod.HorizontalRightToLeft)
							{
								num = num3 - num9;
							}
							else
							{
								num3 = num + num9;
							}
							if (widget.ChildCount < 64)
							{
								child2.Layout(num, num4, num3, num2);
							}
							else
							{
								LayoutBox layoutBox = new LayoutBox
								{
									Left = num,
									Right = num3,
									Bottom = num4,
									Top = num2
								};
								this._layoutBoxes.Add(j, layoutBox);
							}
							if (this.LayoutMethod == LayoutMethod.HorizontalRightToLeft)
							{
								num3 = num;
							}
							else if (this.LayoutMethod == LayoutMethod.HorizontalSpaced)
							{
								num = num3 + num8;
							}
							else
							{
								num = num3;
							}
						}
						else
						{
							this._layoutBoxes.Add(j, default(LayoutBox));
						}
					}
				}
			}
			if (widget.ChildCount >= 64)
			{
				this.ParallelUpdateLayouts(widget);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000119B4 File Offset: 0x0000FBB4
		private void LayoutLinearVertical(Widget widget, float left, float bottom, float right, float top)
		{
			Container container = widget as Container;
			float num = 0f;
			float num2 = 0f;
			float num3 = bottom - top;
			float num4 = right - left;
			this._layoutBoxes.Clear();
			int num5 = 0;
			float num6 = 0f;
			for (int i = 0; i < widget.ChildCount; i++)
			{
				Widget child = widget.GetChild(i);
				if (child == null)
				{
					Debug.FailedAssert("Trying to measure a null child for parent" + widget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "LayoutLinearVertical", 543);
				}
				else if (child.IsVisible)
				{
					num5++;
					num6 += child.MeasuredSize.Y + child.ScaledMarginTop + child.ScaledMarginBottom;
				}
			}
			if (container != null && container.IsDragHovering)
			{
				num6 += 20f;
			}
			if (this.LayoutMethod == LayoutMethod.VerticalCentered || (this.LayoutMethod == LayoutMethod.VerticalSpaced && num5 == 1))
			{
				num2 = (bottom - top) / 2f - num6 / 2f;
			}
			if (num5 > 0)
			{
				float num7 = bottom - top - num6;
				float num8 = ((num5 > 1) ? (num7 / (float)(num5 - 1)) : 0f);
				for (int j = 0; j < widget.ChildCount; j++)
				{
					Widget child2 = widget.GetChild(j);
					if (child2 == null)
					{
						Debug.FailedAssert("Trying to measure a null child for parent" + widget.GetFullIDPath(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Layout\\StackLayout.cs", "LayoutLinearVertical", 576);
					}
					else
					{
						if (container != null && container.IsDragHovering && j == container.DragHoverInsertionIndex)
						{
							if (this.LayoutMethod == LayoutMethod.VerticalBottomToTop)
							{
								num3 -= 20f;
							}
							else
							{
								num2 += 20f;
							}
						}
						if (child2.IsVisible)
						{
							float num9 = child2.MeasuredSize.Y + child2.ScaledMarginTop + child2.ScaledMarginBottom;
							if (this.LayoutMethod == LayoutMethod.VerticalBottomToTop)
							{
								num2 = num3 - num9;
							}
							else
							{
								num3 = num2 + num9;
							}
							if (widget.ChildCount < 64)
							{
								child2.Layout(num, num3, num4, num2);
							}
							else
							{
								LayoutBox layoutBox = new LayoutBox
								{
									Left = num,
									Right = num4,
									Bottom = num3,
									Top = num2
								};
								this._layoutBoxes.Add(j, layoutBox);
							}
							if (this.LayoutMethod == LayoutMethod.VerticalBottomToTop)
							{
								num3 = num2;
							}
							else if (this.LayoutMethod == LayoutMethod.VerticalSpaced)
							{
								num2 = num3 + num8;
							}
							else
							{
								num2 = num3;
							}
						}
						else
						{
							this._layoutBoxes.Add(j, default(LayoutBox));
						}
					}
				}
			}
			if (widget.ChildCount >= 64)
			{
				this.ParallelUpdateLayouts(widget);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00011C34 File Offset: 0x0000FE34
		public Vector2 GetDropGizmoPosition(Container widget, Vector2 draggedWidgetPosition)
		{
			int num = 0;
			if (this.LayoutMethod == LayoutMethod.VerticalTopToBottom || this.LayoutMethod == LayoutMethod.VerticalBottomToTop || this.LayoutMethod == LayoutMethod.VerticalCentered || this.LayoutMethod == LayoutMethod.VerticalSpaced)
			{
				num = 1;
			}
			bool flag = this.LayoutMethod == LayoutMethod.HorizontalRightToLeft || this.LayoutMethod == LayoutMethod.VerticalBottomToTop;
			int indexForDrop = this.GetIndexForDrop(widget, draggedWidgetPosition);
			int num2 = indexForDrop - 1;
			Vector2 globalPosition = widget.GlobalPosition;
			Vector2 globalPosition2 = widget.GlobalPosition;
			if (!flag)
			{
				if (num2 >= 0 && num2 < widget.ChildCount)
				{
					Widget child = widget.GetChild(num2);
					StackLayout.SetData(ref globalPosition, num, StackLayout.GetData(child.GlobalPosition, num) + StackLayout.GetData(child.Size, num));
				}
				if (indexForDrop >= 0 && indexForDrop < widget.ChildCount)
				{
					StackLayout.SetData(ref globalPosition2, num, StackLayout.GetData(widget.GetChild(indexForDrop).GlobalPosition, num));
				}
				else if (indexForDrop >= widget.ChildCount && widget.ChildCount > 0)
				{
					StackLayout.SetData(ref globalPosition2, num, StackLayout.GetData(globalPosition, num) + 20f);
				}
			}
			else
			{
				StackLayout.SetData(ref globalPosition, num, StackLayout.GetData(globalPosition, num) + StackLayout.GetData(widget.Size, num));
				StackLayout.SetData(ref globalPosition2, num, StackLayout.GetData(globalPosition2, num) + StackLayout.GetData(widget.Size, num));
				if (num2 >= 0 && num2 < widget.ChildCount)
				{
					Widget child2 = widget.GetChild(num2);
					StackLayout.SetData(ref globalPosition, num, StackLayout.GetData(child2.GlobalPosition, num));
				}
				if (indexForDrop >= 0 && indexForDrop < widget.ChildCount)
				{
					Widget child3 = widget.GetChild(indexForDrop);
					StackLayout.SetData(ref globalPosition2, num, StackLayout.GetData(child3.GlobalPosition, num) + StackLayout.GetData(child3.Size, num));
				}
				else if (indexForDrop >= widget.ChildCount && widget.ChildCount > 0)
				{
					StackLayout.SetData(ref globalPosition2, num, StackLayout.GetData(globalPosition, num) - 20f);
				}
			}
			return new Vector2((globalPosition.X + globalPosition2.X) / 2f, (globalPosition.Y + globalPosition2.Y) / 2f);
		}

		// Token: 0x04000223 RID: 547
		private const int DragHoverAperture = 20;

		// Token: 0x04000224 RID: 548
		private readonly Dictionary<int, LayoutBox> _layoutBoxes;

		// Token: 0x04000225 RID: 549
		private Widget _parallelMeasureBasicChildWidget;

		// Token: 0x04000226 RID: 550
		private Vector2 _parallelMeasureBasicChildMeasureSpec;

		// Token: 0x04000227 RID: 551
		private AlignmentAxis _parallelMeasureBasicChildAlignmentAxis;

		// Token: 0x04000228 RID: 552
		private TWParallel.ParallelForAuxPredicate _parallelMeasureBasicChildDelegate;
	}
}
