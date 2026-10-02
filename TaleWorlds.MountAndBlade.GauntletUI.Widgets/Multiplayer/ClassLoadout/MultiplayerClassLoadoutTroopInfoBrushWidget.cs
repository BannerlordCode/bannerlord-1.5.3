using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000D0 RID: 208
	public class MultiplayerClassLoadoutTroopInfoBrushWidget : BrushWidget
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x0001E78C File Offset: 0x0001C98C
		public MultiplayerClassLoadoutTroopInfoBrushWidget(UIContext context)
			: base(context)
		{
			this.SetAlpha(this.DefaultAlpha);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0001E7AC File Offset: 0x0001C9AC
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.SetAlpha(1f);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0001E7BF File Offset: 0x0001C9BF
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.SetAlpha(this.DefaultAlpha);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0001E7D3 File Offset: 0x0001C9D3
		public override void OnBrushChanged()
		{
			base.OnBrushChanged();
			this.SetAlpha(this.DefaultAlpha);
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0001E7E7 File Offset: 0x0001C9E7
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x0001E7EF File Offset: 0x0001C9EF
		[Editor(false)]
		public float DefaultAlpha
		{
			get
			{
				return this._defaultAlpha;
			}
			set
			{
				if (value != this._defaultAlpha)
				{
					this._defaultAlpha = value;
					base.OnPropertyChanged(value, "DefaultAlpha");
				}
			}
		}

		// Token: 0x040004EF RID: 1263
		private float _defaultAlpha = 0.7f;
	}
}
