using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000022 RID: 34
	public class GamepadCursorParentWidget : Widget
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x00006C96 File Offset: 0x00004E96
		public GamepadCursorParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006CA0 File Offset: 0x00004EA0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.CenterWidget.SetGlobalAlphaRecursively(MathF.Lerp(this.CenterWidget.AlphaFactor, this.HasTarget ? 0.67f : 1f, 0.16f, 1E-05f));
			GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
			Widget widget = ((instance != null) ? instance.LastTargetedWidget : null);
			if (widget != null)
			{
				this.CenterWidget.PivotX = 0.5f;
				this.CenterWidget.PivotY = 0.5f;
				this.CenterWidget.Rotation = widget.GlobalRotation;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00006D33 File Offset: 0x00004F33
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00006D3B File Offset: 0x00004F3B
		public float XOffset
		{
			get
			{
				return this._xOffset;
			}
			set
			{
				if (value != this._xOffset)
				{
					this._xOffset = value;
					base.OnPropertyChanged(value, "XOffset");
					this.CenterWidget.ScaledPositionXOffset = value;
				}
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00006D65 File Offset: 0x00004F65
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00006D6D File Offset: 0x00004F6D
		public float YOffset
		{
			get
			{
				return this._yOffset;
			}
			set
			{
				if (value != this._yOffset)
				{
					this._yOffset = value;
					base.OnPropertyChanged(value, "YOffset");
					this.CenterWidget.ScaledPositionYOffset = value;
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00006D97 File Offset: 0x00004F97
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00006D9F File Offset: 0x00004F9F
		public bool HasTarget
		{
			get
			{
				return this._hasTarget;
			}
			set
			{
				if (value != this._hasTarget)
				{
					this._hasTarget = value;
					base.OnPropertyChanged(value, "HasTarget");
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00006DBD File Offset: 0x00004FBD
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00006DC5 File Offset: 0x00004FC5
		public BrushWidget CenterWidget
		{
			get
			{
				return this._centerWidget;
			}
			set
			{
				if (value != this._centerWidget)
				{
					this._centerWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "CenterWidget");
				}
			}
		}

		// Token: 0x040000C9 RID: 201
		private float _xOffset;

		// Token: 0x040000CA RID: 202
		private float _yOffset;

		// Token: 0x040000CB RID: 203
		private bool _hasTarget;

		// Token: 0x040000CC RID: 204
		private BrushWidget _centerWidget;
	}
}
