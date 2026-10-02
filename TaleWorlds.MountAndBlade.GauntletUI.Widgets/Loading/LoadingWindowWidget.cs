using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Loading
{
	// Token: 0x02000132 RID: 306
	public class LoadingWindowWidget : Widget
	{
		// Token: 0x0600100F RID: 4111 RVA: 0x0002C7DF File Offset: 0x0002A9DF
		public LoadingWindowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002C7E8 File Offset: 0x0002A9E8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.AnimWidget != null && base.IsVisible && this.AnimWidget.IsVisible)
			{
				this.AnimWidget.PositionXOffset = MathF.PingPong(-200f, 200f, this._totalDt);
				this._totalDt += dt * 500f;
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002C84D File Offset: 0x0002AA4D
		private void UpdateStates()
		{
			base.IsVisible = this.IsActive;
			base.IsEnabled = this.IsActive;
			base.ParentWidget.IsVisible = this.IsActive;
			base.ParentWidget.IsEnabled = this.IsActive;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002C88C File Offset: 0x0002AA8C
		private void UpdateImage(string imageName)
		{
			Sprite sprite = base.Context.SpriteData.GetSprite(imageName);
			if (sprite == null)
			{
				base.Sprite = base.Context.SpriteData.GetSprite("background_1");
				return;
			}
			base.Sprite = sprite;
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0002C8D1 File Offset: 0x0002AAD1
		// (set) Token: 0x06001014 RID: 4116 RVA: 0x0002C8D9 File Offset: 0x0002AAD9
		[Editor(false)]
		public Widget AnimWidget
		{
			get
			{
				return this._animWidget;
			}
			set
			{
				if (this._animWidget != value)
				{
					this._animWidget = value;
					base.OnPropertyChanged<Widget>(value, "AnimWidget");
				}
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0002C8F7 File Offset: 0x0002AAF7
		// (set) Token: 0x06001016 RID: 4118 RVA: 0x0002C8FF File Offset: 0x0002AAFF
		[Editor(false)]
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
					this.UpdateStates();
				}
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x0002C923 File Offset: 0x0002AB23
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x0002C92B File Offset: 0x0002AB2B
		[Editor(false)]
		public string ImageName
		{
			get
			{
				return this._imageName;
			}
			set
			{
				if (this._imageName != value)
				{
					this._imageName = value;
					base.OnPropertyChanged<string>(value, "ImageName");
					this.UpdateImage(value);
				}
			}
		}

		// Token: 0x04000752 RID: 1874
		private const string _defaultBackgroundSpriteData = "background_1";

		// Token: 0x04000753 RID: 1875
		private const float _animWidgetMaxOffset = 200f;

		// Token: 0x04000754 RID: 1876
		private float _totalDt;

		// Token: 0x04000755 RID: 1877
		private Widget _animWidget;

		// Token: 0x04000756 RID: 1878
		private bool _isActive;

		// Token: 0x04000757 RID: 1879
		private string _imageName;
	}
}
