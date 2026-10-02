using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.Layout;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information.RundownTooltip
{
	// Token: 0x0200014F RID: 335
	public class RundownTooltipWidget : TooltipWidget
	{
		// Token: 0x060011F0 RID: 4592 RVA: 0x00032004 File Offset: 0x00030204
		public RundownTooltipWidget(UIContext context)
			: base(context)
		{
			this.RefreshOnNextLateUpdate();
			this._animationDelayInFrames = 2;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00032090 File Offset: 0x00030290
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.LineContainerWidget != null)
			{
				GridLayout gridLayout = this.LineContainerWidget.GridLayout;
				bool flag = this._lastCheckedColumnWidths.Count != gridLayout.ColumnWidths.Count;
				bool flag2 = false;
				for (int i = 0; i < this._lastCheckedColumnWidths.Count; i++)
				{
					float num = this._lastCheckedColumnWidths[i];
					float num2 = ((i < gridLayout.ColumnWidths.Count) ? gridLayout.ColumnWidths[i] : (-1f));
					if (MathF.Abs(num - num2) > 1E-05f)
					{
						flag2 = true;
						break;
					}
				}
				if (flag || flag2)
				{
					this._lastCheckedColumnWidths = gridLayout.ColumnWidths;
					RundownColumnDividerCollectionWidget dividerCollectionWidget = this.DividerCollectionWidget;
					if (dividerCollectionWidget == null)
					{
						return;
					}
					dividerCollectionWidget.Refresh(gridLayout.ColumnWidths);
				}
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00032158 File Offset: 0x00030358
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			GridLayout gridLayout = this.LineContainerWidget.GridLayout;
			for (int i = 0; i < this.LineContainerWidget.ChildCount; i++)
			{
				RundownLineWidget rundownLineWidget = this.LineContainerWidget.GetChild(i) as RundownLineWidget;
				int num = i / this.LineContainerWidget.RowCount;
				rundownLineWidget.RefreshValueOffset((num < gridLayout.ColumnWidths.Count) ? gridLayout.ColumnWidths[num] : (-1f));
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x000321D4 File Offset: 0x000303D4
		private void Refresh()
		{
			RundownTooltipWidget.ValueCategorization valueCategorizationAsInt = (RundownTooltipWidget.ValueCategorization)this.ValueCategorizationAsInt;
			if (this.LineContainerWidget != null)
			{
				List<RundownLineWidget> list = new List<RundownLineWidget>();
				float num = 0f;
				float num2 = 0f;
				using (List<Widget>.Enumerator enumerator = this.LineContainerWidget.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RundownLineWidget rundownLineWidget;
						if ((rundownLineWidget = enumerator.Current as RundownLineWidget) != null)
						{
							list.Add(rundownLineWidget);
							float value = rundownLineWidget.Value;
							if (value < num)
							{
								num = value;
							}
							if (value > num2)
							{
								num2 = value;
							}
						}
					}
				}
				foreach (RundownLineWidget rundownLineWidget2 in list)
				{
					float value2 = rundownLineWidget2.Value;
					Brush brush = rundownLineWidget2.ValueTextWidget.Brush;
					Color color = this._defaultValueColor;
					if (valueCategorizationAsInt != RundownTooltipWidget.ValueCategorization.None)
					{
						float num3 = ((value2 < 0f) ? num : num2);
						float num4 = MathF.Abs(value2 / num3);
						float num5 = (float)((valueCategorizationAsInt == RundownTooltipWidget.ValueCategorization.LargeIsBetter) ? 1 : (-1)) * value2;
						color = Color.Lerp(this._defaultValueColor, (num5 < 0f) ? this._negativeValueColor : this._positiveValueColor, num4);
					}
					brush.FontColor = color;
				}
			}
			this._willRefreshThisFrame = false;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00032328 File Offset: 0x00030528
		private void RefreshOnNextLateUpdate()
		{
			if (!this._willRefreshThisFrame)
			{
				this._willRefreshThisFrame = true;
				base.EventManager.AddLateUpdateAction(this, delegate(float _)
				{
					this.Refresh();
				}, 1);
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00032352 File Offset: 0x00030552
		private void OnLineContainerEventFire(Widget widget, string eventName, object[] args)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove")
			{
				this.RefreshOnNextLateUpdate();
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00032374 File Offset: 0x00030574
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x0003237C File Offset: 0x0003057C
		[Editor(false)]
		public GridWidget LineContainerWidget
		{
			get
			{
				return this._lineContainerWidget;
			}
			set
			{
				if (value != this._lineContainerWidget)
				{
					if (this._lineContainerWidget != null)
					{
						this._lineContainerWidget.EventFire -= this.OnLineContainerEventFire;
					}
					this._lineContainerWidget = value;
					base.OnPropertyChanged<GridWidget>(value, "LineContainerWidget");
					this.RefreshOnNextLateUpdate();
					if (this._lineContainerWidget != null)
					{
						this._lineContainerWidget.EventFire += this.OnLineContainerEventFire;
					}
				}
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x000323E9 File Offset: 0x000305E9
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x000323F1 File Offset: 0x000305F1
		[Editor(false)]
		public RundownColumnDividerCollectionWidget DividerCollectionWidget
		{
			get
			{
				return this._dividerCollectionWidget;
			}
			set
			{
				if (value != this._dividerCollectionWidget)
				{
					this._dividerCollectionWidget = value;
					base.OnPropertyChanged<RundownColumnDividerCollectionWidget>(value, "DividerCollectionWidget");
					this.RefreshOnNextLateUpdate();
				}
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00032415 File Offset: 0x00030615
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x0003241D File Offset: 0x0003061D
		[Editor(false)]
		public int ValueCategorizationAsInt
		{
			get
			{
				return this._valueCategorizationAsInt;
			}
			set
			{
				if (value != this._valueCategorizationAsInt)
				{
					this._valueCategorizationAsInt = value;
					base.OnPropertyChanged(value, "ValueCategorizationAsInt");
					this.RefreshOnNextLateUpdate();
				}
			}
		}

		// Token: 0x04000839 RID: 2105
		private readonly Color _defaultValueColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400083A RID: 2106
		private readonly Color _negativeValueColor = new Color(0.8352941f, 0.12941177f, 0.12941177f, 1f);

		// Token: 0x0400083B RID: 2107
		private readonly Color _positiveValueColor = new Color(0.38039216f, 0.7490196f, 0.33333334f, 1f);

		// Token: 0x0400083C RID: 2108
		private bool _willRefreshThisFrame;

		// Token: 0x0400083D RID: 2109
		private IReadOnlyList<float> _lastCheckedColumnWidths = new List<float>();

		// Token: 0x0400083E RID: 2110
		private GridWidget _lineContainerWidget;

		// Token: 0x0400083F RID: 2111
		private RundownColumnDividerCollectionWidget _dividerCollectionWidget;

		// Token: 0x04000840 RID: 2112
		private int _valueCategorizationAsInt;

		// Token: 0x020001D6 RID: 470
		private enum ValueCategorization
		{
			// Token: 0x04000A7E RID: 2686
			None,
			// Token: 0x04000A7F RID: 2687
			LargeIsBetter,
			// Token: 0x04000A80 RID: 2688
			SmallIsBetter
		}
	}
}
