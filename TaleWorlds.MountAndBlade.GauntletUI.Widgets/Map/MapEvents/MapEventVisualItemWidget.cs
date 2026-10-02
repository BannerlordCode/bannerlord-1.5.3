using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapEvents
{
	// Token: 0x02000129 RID: 297
	public class MapEventVisualItemWidget : Widget
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x0002BCF2 File Offset: 0x00029EF2
		public MapEventVisualItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0002BCFB File Offset: 0x00029EFB
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			this.UpdatePosition();
			this.UpdateVisibility();
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002BD10 File Offset: 0x00029F10
		private void UpdateVisibility()
		{
			base.IsVisible = this.IsVisibleOnMap;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0002BD20 File Offset: 0x00029F20
		private void UpdatePosition()
		{
			if (this.IsVisibleOnMap)
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y;
				return;
			}
			base.ScaledPositionXOffset = -10000f;
			base.ScaledPositionYOffset = -10000f;
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0002BD8C File Offset: 0x00029F8C
		// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x0002BD94 File Offset: 0x00029F94
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0002BDB7 File Offset: 0x00029FB7
		// (set) Token: 0x06000FCA RID: 4042 RVA: 0x0002BDBF File Offset: 0x00029FBF
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (this._isVisibleOnMap != value)
				{
					this._isVisibleOnMap = value;
					base.OnPropertyChanged(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x04000736 RID: 1846
		private Vec2 _position;

		// Token: 0x04000737 RID: 1847
		private bool _isVisibleOnMap;
	}
}
