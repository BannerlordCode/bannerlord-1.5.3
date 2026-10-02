using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DC RID: 220
	public class CompassWidget : Widget
	{
		// Token: 0x06000B37 RID: 2871 RVA: 0x0001F815 File Offset: 0x0001DA15
		public CompassWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0001F81E File Offset: 0x0001DA1E
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.HandleHorizontalPositioning();
			this.HandleMarkerPositioning();
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0001F834 File Offset: 0x0001DA34
		private void HandleHorizontalPositioning()
		{
			if (this.ItemContainerPanel.ChildCount <= 0)
			{
				return;
			}
			List<List<Widget>> list = new List<List<Widget>>();
			float num = 0f;
			float num2 = this.ItemContainerPanel.ParentWidget.MeasuredSize.X * base._inverseScaleToUse - 50f;
			for (int i = 0; i < this.ItemContainerPanel.ChildCount; i++)
			{
				CompassElementWidget compassElementWidget = this.ItemContainerPanel.GetChild(i) as CompassElementWidget;
				if (!compassElementWidget.IsHidden)
				{
					float num3 = (compassElementWidget.Position + 1f) * 0.5f;
					compassElementWidget.MarginLeft = MBMath.Lerp(num, num2, num3, 1E-05f);
					bool flag = false;
					if (list.Count > 0)
					{
						List<Widget> list2 = list[list.Count - 1];
						int j = list2.Count - 1;
						while (j >= 0)
						{
							if (Math.Abs(list2[j].MarginLeft - compassElementWidget.MarginLeft) < 10f)
							{
								flag = true;
								compassElementWidget.MarginLeft = list[list.Count - 1][list[list.Count - 1].Count - 1].MarginLeft + 10f;
								list[list.Count - 1].Add(compassElementWidget);
								if (compassElementWidget.MarginLeft > num2)
								{
									float marginLeft = compassElementWidget.MarginLeft;
									compassElementWidget.MarginLeft = num2;
									float num4 = marginLeft - compassElementWidget.MarginLeft;
									for (int k = 1; k < list2.Count; k++)
									{
										int num5 = list2.Count - 1 - k;
										list2[num5].MarginLeft -= num4;
									}
									break;
								}
								break;
							}
							else
							{
								j--;
							}
						}
					}
					if (!flag)
					{
						list.Add(new List<Widget>());
						list[list.Count - 1].Add(compassElementWidget);
					}
				}
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		private void HandleMarkerPositioning()
		{
			if (this.MarkerContainerPanel.ChildCount <= 0)
			{
				return;
			}
			float num = 0f;
			float num2 = this.MarkerContainerPanel.ParentWidget.MeasuredSize.X * base._inverseScaleToUse;
			for (int i = 0; i < this.MarkerContainerPanel.ChildCount; i++)
			{
				CompassMarkerTextWidget compassMarkerTextWidget = this.MarkerContainerPanel.GetChild(i) as CompassMarkerTextWidget;
				float num3 = (compassMarkerTextWidget.Position + 1f) * 0.5f;
				compassMarkerTextWidget.MarginLeft = MBMath.Lerp(num, num2, num3, 1E-05f) - compassMarkerTextWidget.Size.X * 0.5f;
				compassMarkerTextWidget.IsHidden = MBMath.ApproximatelyEquals(num3, 0f, 1E-05f) || MBMath.ApproximatelyEquals(num3, 1f, 1E-05f);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x0001FAED File Offset: 0x0001DCED
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x0001FAF5 File Offset: 0x0001DCF5
		[DataSourceProperty]
		public Widget ItemContainerPanel
		{
			get
			{
				return this._itemContainerPanel;
			}
			set
			{
				if (this._itemContainerPanel != value)
				{
					this._itemContainerPanel = value;
					base.OnPropertyChanged<Widget>(value, "ItemContainerPanel");
				}
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0001FB13 File Offset: 0x0001DD13
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x0001FB1B File Offset: 0x0001DD1B
		[DataSourceProperty]
		public Widget MarkerContainerPanel
		{
			get
			{
				return this._markerContainerPanel;
			}
			set
			{
				if (this._markerContainerPanel != value)
				{
					this._markerContainerPanel = value;
					base.OnPropertyChanged<Widget>(value, "MarkerContainerPanel");
				}
			}
		}

		// Token: 0x04000514 RID: 1300
		private Widget _itemContainerPanel;

		// Token: 0x04000515 RID: 1301
		private Widget _markerContainerPanel;
	}
}
