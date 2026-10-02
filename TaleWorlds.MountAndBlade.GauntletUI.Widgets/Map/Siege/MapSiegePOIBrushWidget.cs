using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x02000120 RID: 288
	public class MapSiegePOIBrushWidget : BrushWidget
	{
		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002A52B File Offset: 0x0002872B
		private Color _fullColor
		{
			get
			{
				return new Color(0.2784314f, 0.9882353f, 0.44313726f, 1f);
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0002A546 File Offset: 0x00028746
		private Color _emptyColor
		{
			get
			{
				return new Color(0.9882353f, 0.2784314f, 0.2784314f, 1f);
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002A561 File Offset: 0x00028761
		// (set) Token: 0x06000F47 RID: 3911 RVA: 0x0002A569 File Offset: 0x00028769
		public SliderWidget Slider { get; set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0002A572 File Offset: 0x00028772
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x0002A57A File Offset: 0x0002877A
		public Brush ConstructionBrush { get; set; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0002A583 File Offset: 0x00028783
		// (set) Token: 0x06000F4B RID: 3915 RVA: 0x0002A58B File Offset: 0x0002878B
		public Brush NormalBrush { get; set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0002A594 File Offset: 0x00028794
		// (set) Token: 0x06000F4D RID: 3917 RVA: 0x0002A59C File Offset: 0x0002879C
		public Vec2 ScreenPosition { get; set; }

		// Token: 0x06000F4E RID: 3918 RVA: 0x0002A5A5 File Offset: 0x000287A5
		public MapSiegePOIBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002A5BC File Offset: 0x000287BC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			base.ScaledPositionXOffset = this.ScreenPosition.x - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.ScreenPosition.y;
			float num = (float)(this.IsInVisibleRange ? 1 : 0);
			float num2 = MathF.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, num, dt * 10f, 1E-05f);
			this.SetGlobalAlphaRecursively(num2);
			base.IsEnabled = false;
			if (this._animState == MapSiegePOIBrushWidget.AnimState.Start)
			{
				this._tickCount++;
				if (this._tickCount > 5)
				{
					this._animState = MapSiegePOIBrushWidget.AnimState.Starting;
				}
			}
			else if (this._animState == MapSiegePOIBrushWidget.AnimState.Starting)
			{
				(this.Slider.Filler as BrushWidget).BrushRenderer.RestartAnimation();
				if (this.QueueIndex == 0)
				{
					this.HammerAnimWidget.BrushRenderer.RestartAnimation();
				}
				this._animState = MapSiegePOIBrushWidget.AnimState.Playing;
			}
			if (!this._isBrushChanged)
			{
				(this.Slider.Filler as BrushWidget).Brush = (this.IsConstructing ? this.ConstructionBrush : this.NormalBrush);
				this._animState = MapSiegePOIBrushWidget.AnimState.Start;
				this._isBrushChanged = true;
			}
			if (!this.IsConstructing)
			{
				this.UpdateColorOfSlider();
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0002A6F8 File Offset: 0x000288F8
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			this.IsPOISelected = true;
			base.EventFired("OnSelection", Array.Empty<object>());
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0002A717 File Offset: 0x00028917
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0002A71F File Offset: 0x0002891F
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0002A728 File Offset: 0x00028928
		private void SetMachineTypeIcon(int machineType)
		{
			string text = "SPGeneral\\MapSiege\\";
			switch (machineType)
			{
			case 0:
				text += "wall";
				break;
			case 1:
				text += "broken_wall";
				break;
			case 2:
				text += "ballista";
				break;
			case 3:
				text += "trebuchet";
				break;
			case 4:
				text += "ladder";
				break;
			case 5:
				text += "ram";
				break;
			case 6:
				text += "tower";
				break;
			case 7:
				text += "mangonel";
				break;
			default:
				text += "fallback";
				break;
			}
			this.MachineTypeIconWidget.Sprite = base.Context.SpriteData.GetSprite(text);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0002A7FC File Offset: 0x000289FC
		private void UpdateColorOfSlider()
		{
			(this.Slider.Filler as BrushWidget).Brush.Color = Color.Lerp(this._emptyColor, this._fullColor, this.Slider.ValueFloat / this.Slider.MaxValueFloat);
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0002A84B File Offset: 0x00028A4B
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x0002A853 File Offset: 0x00028A53
		public MapSiegeConstructionControllerWidget ConstructionControllerWidget
		{
			get
			{
				return this._constructionControllerWidget;
			}
			set
			{
				if (this._constructionControllerWidget != value)
				{
					this._constructionControllerWidget = value;
				}
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0002A865 File Offset: 0x00028A65
		// (set) Token: 0x06000F58 RID: 3928 RVA: 0x0002A86D File Offset: 0x00028A6D
		public bool IsPlayerSidePOI
		{
			get
			{
				return this._isPlayerSidePOI;
			}
			set
			{
				if (this._isPlayerSidePOI != value)
				{
					this._isPlayerSidePOI = value;
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0002A87F File Offset: 0x00028A7F
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0002A887 File Offset: 0x00028A87
		public bool IsInVisibleRange
		{
			get
			{
				return this._isInVisibleRange;
			}
			set
			{
				if (this._isInVisibleRange != value)
				{
					this._isInVisibleRange = value;
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0002A899 File Offset: 0x00028A99
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0002A8A1 File Offset: 0x00028AA1
		public bool IsPOISelected
		{
			get
			{
				return this._isPOISelected;
			}
			set
			{
				if (this._isPOISelected != value)
				{
					this._isPOISelected = value;
					this.ConstructionControllerWidget.SetCurrentPOIWidget(value ? this : null);
				}
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0002A8C5 File Offset: 0x00028AC5
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0002A8CD File Offset: 0x00028ACD
		public bool IsConstructing
		{
			get
			{
				return this._isConstructing;
			}
			set
			{
				if (this._isConstructing != value)
				{
					this._isConstructing = value;
					this._isBrushChanged = false;
					this._animState = MapSiegePOIBrushWidget.AnimState.Idle;
				}
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002A8ED File Offset: 0x00028AED
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0002A8F5 File Offset: 0x00028AF5
		public int MachineType
		{
			get
			{
				return this._machineType;
			}
			set
			{
				if (this._machineType != value)
				{
					this._machineType = value;
					this.SetMachineTypeIcon(value);
				}
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0002A90E File Offset: 0x00028B0E
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0002A916 File Offset: 0x00028B16
		public int QueueIndex
		{
			get
			{
				return this._queueIndex;
			}
			set
			{
				if (this._queueIndex != value)
				{
					this._queueIndex = value;
					this._animState = MapSiegePOIBrushWidget.AnimState.Start;
					this._tickCount = 0;
				}
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0002A936 File Offset: 0x00028B36
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0002A93E File Offset: 0x00028B3E
		public Widget MachineTypeIconWidget
		{
			get
			{
				return this._machineTypeIconWidget;
			}
			set
			{
				if (this._machineTypeIconWidget != value)
				{
					this._machineTypeIconWidget = value;
				}
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0002A950 File Offset: 0x00028B50
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0002A958 File Offset: 0x00028B58
		public BrushWidget HammerAnimWidget
		{
			get
			{
				return this._hammerAnimWidget;
			}
			set
			{
				if (this._hammerAnimWidget != value)
				{
					this._hammerAnimWidget = value;
				}
			}
		}

		// Token: 0x040006FD RID: 1789
		private MapSiegePOIBrushWidget.AnimState _animState;

		// Token: 0x04000702 RID: 1794
		private bool _isBrushChanged;

		// Token: 0x04000703 RID: 1795
		private int _tickCount;

		// Token: 0x04000704 RID: 1796
		private bool _isConstructing;

		// Token: 0x04000705 RID: 1797
		private bool _isPlayerSidePOI;

		// Token: 0x04000706 RID: 1798
		private bool _isInVisibleRange;

		// Token: 0x04000707 RID: 1799
		private bool _isPOISelected;

		// Token: 0x04000708 RID: 1800
		private BrushWidget _hammerAnimWidget;

		// Token: 0x04000709 RID: 1801
		private Widget _machineTypeIconWidget;

		// Token: 0x0400070A RID: 1802
		private int _machineType = -1;

		// Token: 0x0400070B RID: 1803
		private int _queueIndex = -1;

		// Token: 0x0400070C RID: 1804
		private MapSiegeConstructionControllerWidget _constructionControllerWidget;

		// Token: 0x020001CC RID: 460
		public enum AnimState
		{
			// Token: 0x04000A56 RID: 2646
			Idle,
			// Token: 0x04000A57 RID: 2647
			Start,
			// Token: 0x04000A58 RID: 2648
			Starting,
			// Token: 0x04000A59 RID: 2649
			Playing
		}
	}
}
