using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E4 RID: 228
	public class MissionSiegeEngineMarkerWidget : Widget
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x00021095 File Offset: 0x0001F295
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x0002109D File Offset: 0x0001F29D
		public SliderWidget Slider { get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x000210A6 File Offset: 0x0001F2A6
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x000210AE File Offset: 0x0001F2AE
		public BrushWidget MachineIconParent { get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x000210B7 File Offset: 0x0001F2B7
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x000210BF File Offset: 0x0001F2BF
		public Brush EnemyBrush { get; set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x000210C8 File Offset: 0x0001F2C8
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x000210D0 File Offset: 0x0001F2D0
		public Brush AllyBrush { get; set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x000210D9 File Offset: 0x0001F2D9
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x000210E1 File Offset: 0x0001F2E1
		public Vec2 ScreenPosition { get; set; }

		// Token: 0x06000BDB RID: 3035 RVA: 0x000210EC File Offset: 0x0001F2EC
		public MissionSiegeEngineMarkerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00021140 File Offset: 0x0001F340
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			base.ScaledPositionXOffset = this.ScreenPosition.x - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.ScreenPosition.y;
			float num = (this.IsActive ? 0.65f : 0f);
			float num2 = MathF.Lerp(base.AlphaFactor, num, dt * 10f, 1E-05f);
			this.SetGlobalAlphaRecursively(num2);
			if (!this._isBrushChanged)
			{
				this.MachineIconParent.Brush = (this.IsEnemy ? this.EnemyBrush : this.AllyBrush);
				this._isBrushChanged = true;
			}
			this.UpdateColorOfSlider();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x000211F4 File Offset: 0x0001F3F4
		private void SetMachineTypeIcon(string machineType)
		{
			string text = "SPGeneral\\MapSiege\\" + machineType;
			this.MachineTypeIconWidget.Sprite = base.Context.SpriteData.GetSprite(text);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0002122C File Offset: 0x0001F42C
		private void UpdateColorOfSlider()
		{
			(this.Slider.Filler as BrushWidget).Brush.Color = Color.Lerp(this._emptyColor, this._fullColor, this.Slider.ValueFloat / this.Slider.MaxValueFloat);
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0002127B File Offset: 0x0001F47B
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00021283 File Offset: 0x0001F483
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (this._isEnemy != value)
				{
					this._isEnemy = value;
				}
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00021295 File Offset: 0x0001F495
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x0002129D File Offset: 0x0001F49D
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
				}
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x000212AF File Offset: 0x0001F4AF
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x000212B7 File Offset: 0x0001F4B7
		public string EngineType
		{
			get
			{
				return this._engineType;
			}
			set
			{
				if (this._engineType != value)
				{
					this._engineType = value;
					this.SetMachineTypeIcon(value);
				}
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x000212D5 File Offset: 0x0001F4D5
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x000212DD File Offset: 0x0001F4DD
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

		// Token: 0x04000559 RID: 1369
		private Color _fullColor = new Color(0.2784314f, 0.9882353f, 0.44313726f, 1f);

		// Token: 0x0400055A RID: 1370
		private Color _emptyColor = new Color(0.9882353f, 0.2784314f, 0.2784314f, 1f);

		// Token: 0x04000560 RID: 1376
		private bool _isBrushChanged;

		// Token: 0x04000561 RID: 1377
		private bool _isEnemy;

		// Token: 0x04000562 RID: 1378
		private bool _isActive;

		// Token: 0x04000563 RID: 1379
		private Widget _machineTypeIconWidget;

		// Token: 0x04000564 RID: 1380
		private string _engineType;
	}
}
