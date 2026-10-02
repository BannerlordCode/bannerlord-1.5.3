using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DA RID: 218
	public class CompassElementWidget : Widget
	{
		// Token: 0x06000B22 RID: 2850 RVA: 0x0001F560 File Offset: 0x0001D760
		public CompassElementWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0001F574 File Offset: 0x0001D774
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.HandleDistanceFading(dt);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0001F584 File Offset: 0x0001D784
		private void HandleDistanceFading(float dt)
		{
			if (this.Distance < 10)
			{
				this._alpha -= 2f * dt;
			}
			else
			{
				this._alpha += 2f * dt;
			}
			this._alpha = MBMath.ClampFloat(this._alpha, 0f, 1f);
			if (this.BannerWidget != null)
			{
				int childCount = this.BannerWidget.ChildCount;
				for (int i = 0; i < childCount; i++)
				{
					Widget child = this.FlagWidget.GetChild(i);
					Color color = child.Color;
					color.Alpha = this._alpha;
					child.Color = color;
				}
			}
			if (this.FlagWidget != null)
			{
				int childCount2 = this.FlagWidget.ChildCount;
				for (int j = 0; j < childCount2; j++)
				{
					Widget child2 = this.FlagWidget.GetChild(j);
					Color color2 = child2.Color;
					color2.Alpha = this._alpha;
					child2.Color = color2;
				}
			}
			base.IsVisible = this._alpha > 1E-05f;
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0001F686 File Offset: 0x0001D886
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x0001F68E File Offset: 0x0001D88E
		[DataSourceProperty]
		public float Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (Math.Abs(this._position - value) > 1E-45f)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x0001F6B7 File Offset: 0x0001D8B7
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x0001F6BF File Offset: 0x0001D8BF
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value)
				{
					this._distance = value;
					base.OnPropertyChanged(value, "Distance");
				}
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x0001F6DD File Offset: 0x0001D8DD
		// (set) Token: 0x06000B2A RID: 2858 RVA: 0x0001F6E5 File Offset: 0x0001D8E5
		[DataSourceProperty]
		public Widget BannerWidget
		{
			get
			{
				return this._bannerWidget;
			}
			set
			{
				if (this._bannerWidget != value)
				{
					this._bannerWidget = value;
					base.OnPropertyChanged<Widget>(value, "BannerWidget");
				}
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0001F703 File Offset: 0x0001D903
		// (set) Token: 0x06000B2C RID: 2860 RVA: 0x0001F70B File Offset: 0x0001D90B
		[DataSourceProperty]
		public Widget FlagWidget
		{
			get
			{
				return this._flagWidget;
			}
			set
			{
				if (this._flagWidget != value)
				{
					this._flagWidget = value;
					base.OnPropertyChanged<Widget>(value, "FlagWidget");
				}
			}
		}

		// Token: 0x0400050B RID: 1291
		private float _alpha = 1f;

		// Token: 0x0400050C RID: 1292
		private float _position;

		// Token: 0x0400050D RID: 1293
		private int _distance;

		// Token: 0x0400050E RID: 1294
		private Widget _bannerWidget;

		// Token: 0x0400050F RID: 1295
		private Widget _flagWidget;
	}
}
