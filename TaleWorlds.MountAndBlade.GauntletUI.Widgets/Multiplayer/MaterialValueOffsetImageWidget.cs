using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000085 RID: 133
	public class MaterialValueOffsetImageWidget : ImageWidget
	{
		// Token: 0x0600079B RID: 1947 RVA: 0x00016698 File Offset: 0x00014898
		public MaterialValueOffsetImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000166A4 File Offset: 0x000148A4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._visualDirty)
			{
				foreach (Style style in base.Brush.Styles)
				{
					foreach (StyleLayer styleLayer in style.GetLayers())
					{
						styleLayer.ValueFactor += this.ValueOffset;
						styleLayer.SaturationFactor += this.SaturationOffset;
						styleLayer.HueFactor += this.HueOffset;
					}
				}
				this._visualDirty = false;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001675C File Offset: 0x0001495C
		// (set) Token: 0x0600079E RID: 1950 RVA: 0x00016764 File Offset: 0x00014964
		public float ValueOffset
		{
			get
			{
				return this._valueOffset;
			}
			set
			{
				this._valueOffset = value;
				this._visualDirty = true;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00016774 File Offset: 0x00014974
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0001677C File Offset: 0x0001497C
		public float SaturationOffset
		{
			get
			{
				return this._saturationOffset;
			}
			set
			{
				this._saturationOffset = value;
				this._visualDirty = true;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0001678C File Offset: 0x0001498C
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00016794 File Offset: 0x00014994
		public float HueOffset
		{
			get
			{
				return this._hueOffset;
			}
			set
			{
				this._hueOffset = value;
				this._visualDirty = true;
			}
		}

		// Token: 0x04000351 RID: 849
		private bool _visualDirty;

		// Token: 0x04000352 RID: 850
		private float _valueOffset;

		// Token: 0x04000353 RID: 851
		private float _saturationOffset;

		// Token: 0x04000354 RID: 852
		private float _hueOffset;
	}
}
