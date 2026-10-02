using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003D RID: 61
	public class ScreenBackgroundBrushWidget : BrushWidget
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000B800 File Offset: 0x00009A00
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000B808 File Offset: 0x00009A08
		public bool IsParticleVisible { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000B811 File Offset: 0x00009A11
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000B819 File Offset: 0x00009A19
		public bool IsSmokeVisible { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000B822 File Offset: 0x00009A22
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000B82A File Offset: 0x00009A2A
		public bool IsFullscreenImageEnabled { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000B833 File Offset: 0x00009A33
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000B83B File Offset: 0x00009A3B
		public bool AnimEnabled { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000B844 File Offset: 0x00009A44
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0000B84C File Offset: 0x00009A4C
		public Widget ParticleWidget1 { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000B855 File Offset: 0x00009A55
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000B85D File Offset: 0x00009A5D
		public Widget ParticleWidget2 { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000B866 File Offset: 0x00009A66
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000B86E File Offset: 0x00009A6E
		public Widget SmokeWidget1 { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000B877 File Offset: 0x00009A77
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000B87F File Offset: 0x00009A7F
		public Widget SmokeWidget2 { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000B888 File Offset: 0x00009A88
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000B890 File Offset: 0x00009A90
		public float SmokeSpeedModifier { get; set; } = 1f;

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000B899 File Offset: 0x00009A99
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0000B8A1 File Offset: 0x00009AA1
		public float ParticleSpeedModifier { get; set; } = 1f;

		// Token: 0x060003A8 RID: 936 RVA: 0x0000B8AA File Offset: 0x00009AAA
		public ScreenBackgroundBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._firstFrame)
			{
				this.UpdateBackgroundImage();
				this._firstFrame = false;
			}
			this.ParticleWidget1.IsVisible = this.IsParticleVisible;
			this.ParticleWidget2.IsVisible = this.IsParticleVisible;
			this.SmokeWidget1.IsVisible = this.IsSmokeVisible;
			this.SmokeWidget2.IsVisible = this.IsSmokeVisible;
			if (this.AnimEnabled)
			{
				if (this.IsParticleVisible)
				{
					this.ParticleWidget1.PositionXOffset = this._totalParticleXOffset;
					this.ParticleWidget2.PositionXOffset = this.ParticleWidget1.PositionXOffset + this.ParticleWidget1.SuggestedWidth;
					this._totalParticleXOffset -= dt * 10f * this.ParticleSpeedModifier;
					if (Math.Abs(this._totalParticleXOffset) >= this.ParticleWidget1.SuggestedWidth)
					{
						this._totalParticleXOffset = 0f;
					}
				}
				if (this.IsSmokeVisible)
				{
					this.SmokeWidget1.PositionXOffset = this._totalSmokeXOffset;
					this.SmokeWidget2.PositionXOffset = this.SmokeWidget1.PositionXOffset - this.SmokeWidget1.SuggestedWidth;
					if (Math.Abs(this._totalSmokeXOffset) >= this.SmokeWidget1.SuggestedWidth)
					{
						this._totalSmokeXOffset = 0f;
					}
					this._totalSmokeXOffset += dt * 10f * this.SmokeSpeedModifier;
				}
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000BA3C File Offset: 0x00009C3C
		private void UpdateBackgroundImage()
		{
			if (this.IsFullscreenImageEnabled)
			{
				int num = base.Context.UIRandom.Next(base.Brush.Styles.Count);
				StyleLayer[] layers = base.ReadOnlyBrush.Styles.ElementAt<Style>(num).GetLayers();
				if (layers.Length != 0)
				{
					base.Brush.Sprite = layers[0].Sprite;
					return;
				}
			}
			else
			{
				base.Brush.Sprite = null;
			}
		}

		// Token: 0x04000181 RID: 385
		private bool _firstFrame = true;

		// Token: 0x04000182 RID: 386
		private float _totalSmokeXOffset;

		// Token: 0x04000183 RID: 387
		private float _totalParticleXOffset;
	}
}
