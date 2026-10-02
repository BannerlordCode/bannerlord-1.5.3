using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000086 RID: 134
	public class MaterialValueOffsetTextWidget : TextWidget
	{
		// Token: 0x060007A3 RID: 1955 RVA: 0x000167A4 File Offset: 0x000149A4
		public MaterialValueOffsetTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000167B0 File Offset: 0x000149B0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._visualDirty)
			{
				base.Brush.TextValueFactor += this.ValueOffset;
				base.Brush.TextSaturationFactor += this.SaturationOffset;
				base.Brush.TextHueFactor += this.HueOffset;
				foreach (Style style in base.Brush.Styles)
				{
					style.TextValueFactor += this.ValueOffset;
					style.TextSaturationFactor += this.SaturationOffset;
					style.TextHueFactor += this.HueOffset;
				}
				this._visualDirty = false;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0001689C File Offset: 0x00014A9C
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x000168A4 File Offset: 0x00014AA4
		public float ValueOffset
		{
			get
			{
				return this._valueOffset;
			}
			set
			{
				if (this._valueOffset != value)
				{
					this._valueOffset = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000168BD File Offset: 0x00014ABD
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x000168C5 File Offset: 0x00014AC5
		public float SaturationOffset
		{
			get
			{
				return this._saturationOffset;
			}
			set
			{
				if (this._saturationOffset != value)
				{
					this._saturationOffset = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x000168DE File Offset: 0x00014ADE
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x000168E6 File Offset: 0x00014AE6
		public float HueOffset
		{
			get
			{
				return this._hueOffset;
			}
			set
			{
				if (this._hueOffset != value)
				{
					this._hueOffset = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x04000355 RID: 853
		private bool _visualDirty;

		// Token: 0x04000356 RID: 854
		private float _valueOffset;

		// Token: 0x04000357 RID: 855
		private float _saturationOffset;

		// Token: 0x04000358 RID: 856
		private float _hueOffset;
	}
}
