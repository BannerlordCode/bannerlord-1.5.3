using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.MapBar
{
	// Token: 0x02000118 RID: 280
	public class MapTimeImageBrushWidget : BrushWidget
	{
		// Token: 0x06000EF6 RID: 3830 RVA: 0x00029493 File Offset: 0x00027693
		public MapTimeImageBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0002949C File Offset: 0x0002769C
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			StyleLayer layer = base.Brush.DefaultStyle.GetLayer("Default");
			StyleLayer layer2 = base.Brush.DefaultStyle.GetLayer("Part2");
			if (!this._initialized)
			{
				this._offset = layer2.XOffset;
				this._initialized = true;
			}
			float overridenWidth = layer.OverridenWidth;
			float num = -overridenWidth * ((float)this.DayTime / 24f) + this._offset;
			float num2;
			if (this.DayTime > 12.0)
			{
				num2 = num + overridenWidth;
			}
			else
			{
				num2 = num - overridenWidth;
			}
			layer.XOffset = num;
			layer2.XOffset = num2;
			base.OnRender(twoDimensionContext, drawContext);
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x00029544 File Offset: 0x00027744
		// (set) Token: 0x06000EF9 RID: 3833 RVA: 0x0002954C File Offset: 0x0002774C
		[Editor(false)]
		public double DayTime
		{
			get
			{
				return this._dayTime;
			}
			set
			{
				if (this._dayTime != value)
				{
					this._dayTime = value;
					base.OnPropertyChanged(value, "DayTime");
				}
			}
		}

		// Token: 0x040006D3 RID: 1747
		private float _offset;

		// Token: 0x040006D4 RID: 1748
		private bool _initialized;

		// Token: 0x040006D5 RID: 1749
		private double _dayTime;
	}
}
