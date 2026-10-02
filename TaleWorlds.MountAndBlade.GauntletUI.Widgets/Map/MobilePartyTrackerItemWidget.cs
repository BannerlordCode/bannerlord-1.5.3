using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x0200011D RID: 285
	public class MobilePartyTrackerItemWidget : Widget
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00029E36 File Offset: 0x00028036
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x00029E3E File Offset: 0x0002803E
		public Widget FrameVisualWidget { get; set; }

		// Token: 0x06000F28 RID: 3880 RVA: 0x00029E47 File Offset: 0x00028047
		public MobilePartyTrackerItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00029E50 File Offset: 0x00028050
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateScreenPosition();
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00029E60 File Offset: 0x00028060
		private void UpdateScreenPosition()
		{
			this._screenWidth = base.Context.EventManager.PageSize.X;
			this._screenHeight = base.Context.EventManager.PageSize.Y;
			if (!this.IsActive)
			{
				base.IsHidden = true;
				return;
			}
			Vec2 vec = new Vec2(this.Position);
			if (this.IsTracked)
			{
				if (!this.IsBehind && vec.X - base.Size.X / 2f > 0f && vec.x + base.Size.X / 2f < base.Context.EventManager.PageSize.X && vec.y > 0f && vec.y + base.Size.Y < base.Context.EventManager.PageSize.Y)
				{
					base.ScaledPositionXOffset = vec.x - base.Size.X / 2f;
					base.ScaledPositionYOffset = vec.y;
				}
				else
				{
					Vec2 vec2 = new Vec2(base.Context.EventManager.PageSize.X / 2f, base.Context.EventManager.PageSize.Y / 2f);
					vec -= vec2;
					if (this.IsBehind)
					{
						vec *= -1f;
					}
					float num = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
					float num2 = Mathf.Cos(num);
					float num3 = Mathf.Sin(num);
					float num4 = num2 / num3;
					Vec2 vec3 = vec2 * 1f;
					vec = ((num2 > 0f) ? new Vec2(-vec3.y / num4, vec2.y) : new Vec2(vec3.y / num4, -vec2.y));
					if (vec.x > vec3.x)
					{
						vec = new Vec2(vec3.x, -vec3.x * num4);
					}
					else if (vec.x < -vec3.x)
					{
						vec = new Vec2(-vec3.x, vec3.x * num4);
					}
					vec += vec2;
					base.ScaledPositionXOffset = Mathf.Clamp(vec.x - base.Size.X / 2f, 0f, this._screenWidth - base.Size.X);
					base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, this._screenHeight - (base.Size.Y + 55f));
				}
			}
			else
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y;
			}
			base.IsHidden = (!this.IsTracked && this.IsBehind) || base.ScaledPositionXOffset > base.Context.TwoDimensionContext.Width || base.ScaledPositionYOffset > base.Context.TwoDimensionContext.Height || base.ScaledPositionXOffset + base.Size.X < 0f || base.ScaledPositionYOffset + base.Size.Y < 0f;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0002A1C8 File Offset: 0x000283C8
		private void UpdateTrackerVisual()
		{
			if (this.FrameVisualWidget != null && this.TrackerImageBrush != null && !string.IsNullOrEmpty(this.TrackerType))
			{
				Widget frameVisualWidget = this.FrameVisualWidget;
				BrushLayer layer = this.TrackerImageBrush.GetLayer(this.TrackerType);
				frameVisualWidget.Sprite = ((layer != null) ? layer.Sprite : null);
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0002A21A File Offset: 0x0002841A
		// (set) Token: 0x06000F2D RID: 3885 RVA: 0x0002A222 File Offset: 0x00028422
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
				}
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0002A240 File Offset: 0x00028440
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x0002A248 File Offset: 0x00028448
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (this._isBehind != value)
				{
					this._isBehind = value;
					base.OnPropertyChanged(value, "IsBehind");
				}
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0002A266 File Offset: 0x00028466
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x0002A26E File Offset: 0x0002846E
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (this._isTracked != value)
				{
					this._isTracked = value;
					base.OnPropertyChanged(value, "IsTracked");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0002A28C File Offset: 0x0002848C
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0002A294 File Offset: 0x00028494
		public string TrackerType
		{
			get
			{
				return this._trackerType;
			}
			set
			{
				if (this._trackerType != value)
				{
					this._trackerType = value;
					base.OnPropertyChanged<string>(value, "TrackerType");
					this.UpdateTrackerVisual();
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0002A2BD File Offset: 0x000284BD
		// (set) Token: 0x06000F35 RID: 3893 RVA: 0x0002A2C5 File Offset: 0x000284C5
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

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0002A2E8 File Offset: 0x000284E8
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0002A2F0 File Offset: 0x000284F0
		public Brush TrackerImageBrush
		{
			get
			{
				return this._trackerImageBrush;
			}
			set
			{
				if (this._trackerImageBrush != value)
				{
					this._trackerImageBrush = value;
					base.OnPropertyChanged<Brush>(value, "TrackerImageBrush");
					this.UpdateTrackerVisual();
				}
			}
		}

		// Token: 0x040006EF RID: 1775
		private float _screenWidth;

		// Token: 0x040006F0 RID: 1776
		private float _screenHeight;

		// Token: 0x040006F1 RID: 1777
		private bool _isActive;

		// Token: 0x040006F2 RID: 1778
		private bool _isBehind;

		// Token: 0x040006F3 RID: 1779
		private bool _isTracked;

		// Token: 0x040006F4 RID: 1780
		private string _trackerType;

		// Token: 0x040006F5 RID: 1781
		private Vec2 _position;

		// Token: 0x040006F6 RID: 1782
		private Brush _trackerImageBrush;
	}
}
