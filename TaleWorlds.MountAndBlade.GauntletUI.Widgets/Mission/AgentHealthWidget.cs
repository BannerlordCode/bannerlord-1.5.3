using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D7 RID: 215
	public class AgentHealthWidget : Widget
	{
		// Token: 0x06000B06 RID: 2822 RVA: 0x0001F01B File Offset: 0x0001D21B
		public AgentHealthWidget(UIContext context)
			: base(context)
		{
			this._healthDrops = new List<AgentHealthWidget.HealthDropData>();
			this.CheckVisibility();
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0001F04C File Offset: 0x0001D24C
		private void CreateHealthDrop(Widget container, float previousHealthRatio, float currentHealthRatio)
		{
			float num = container.Size.X / base._scaleToUse;
			float num2 = Mathf.Ceil(num * (previousHealthRatio - currentHealthRatio));
			float num3 = Mathf.Floor(num * currentHealthRatio);
			BrushWidget brushWidget = new BrushWidget(base.Context);
			brushWidget.WidthSizePolicy = SizePolicy.Fixed;
			brushWidget.HeightSizePolicy = SizePolicy.Fixed;
			brushWidget.Brush = this.HealthDropBrush;
			brushWidget.SuggestedWidth = num2;
			brushWidget.SuggestedHeight = (float)brushWidget.ReadOnlyBrush.Sprite.Height;
			brushWidget.HorizontalAlignment = HorizontalAlignment.Left;
			brushWidget.VerticalAlignment = VerticalAlignment.Center;
			brushWidget.PositionXOffset = num3;
			brushWidget.ParentWidget = container;
			this._healthDrops.Add(new AgentHealthWidget.HealthDropData(brushWidget, this.AnimationDelay + this.AnimationDuration));
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0001F0FC File Offset: 0x0001D2FC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.HealthBar != null)
			{
				this.HealthBar.MaxAmount = this.MaxHealth;
				this.HealthBar.InitialAmount = this.Health;
			}
			if (this.HealthDropContainer != null)
			{
				this.HandleHealthDrops(dt);
			}
			this.CheckVisibility();
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0001F150 File Offset: 0x0001D350
		private void HandleHealthDrops(float dt)
		{
			for (int i = this._healthDrops.Count - 1; i >= 0; i--)
			{
				AgentHealthWidget.HealthDropData healthDropData = this._healthDrops[i];
				healthDropData.LifeTime -= dt;
				if (healthDropData.LifeTime <= 0f)
				{
					this.HealthDropContainer.RemoveChild(healthDropData.Widget);
					this._healthDrops.RemoveAt(i);
				}
				else
				{
					float num = Mathf.Min(1f, healthDropData.LifeTime / this.AnimationDuration);
					healthDropData.Widget.Brush.AlphaFactor = num;
				}
			}
			float num2 = ((this.MaxHealth != 0) ? ((float)this.Health / (float)this.MaxHealth) : 0f);
			num2 = MathF.Clamp(num2, 0f, 1f);
			if (num2 != this._previousHealthRatio)
			{
				if (num2 > this._previousHealthRatio)
				{
					for (int j = this._healthDrops.Count - 1; j >= 0; j--)
					{
						this.HealthDropContainer.RemoveChild(this._healthDrops[j].Widget);
						this._healthDrops.RemoveAt(j);
					}
				}
				else if (base.IsVisible)
				{
					this.CreateHealthDrop(this.HealthDropContainer, this._previousHealthRatio, num2);
				}
				this._previousHealthRatio = num2;
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0001F294 File Offset: 0x0001D494
		private void CheckVisibility()
		{
			bool flag = this.ShowHealthBar;
			if (flag)
			{
				flag = (float)this._health > 0f || this._healthDrops.Count > 0;
			}
			base.IsVisible = flag;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0001F2D2 File Offset: 0x0001D4D2
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x0001F2DA File Offset: 0x0001D4DA
		[Editor(false)]
		public int Health
		{
			get
			{
				return this._health;
			}
			set
			{
				if (this._health != value)
				{
					this._health = value;
					base.OnPropertyChanged(value, "Health");
				}
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0001F2F8 File Offset: 0x0001D4F8
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x0001F300 File Offset: 0x0001D500
		[Editor(false)]
		public int MaxHealth
		{
			get
			{
				return this._maxHealth;
			}
			set
			{
				if (this._maxHealth != value)
				{
					this._maxHealth = value;
					base.OnPropertyChanged(value, "MaxHealth");
				}
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0001F31E File Offset: 0x0001D51E
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x0001F326 File Offset: 0x0001D526
		[Editor(false)]
		public FillBarWidget HealthBar
		{
			get
			{
				return this._healthBar;
			}
			set
			{
				if (this._healthBar != value)
				{
					this._healthBar = value;
					base.OnPropertyChanged<FillBarWidget>(value, "HealthBar");
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0001F344 File Offset: 0x0001D544
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0001F34C File Offset: 0x0001D54C
		[Editor(false)]
		public Widget HealthDropContainer
		{
			get
			{
				return this._healthDropContainer;
			}
			set
			{
				if (this._healthDropContainer != value)
				{
					this._healthDropContainer = value;
					base.OnPropertyChanged<Widget>(value, "HealthDropContainer");
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0001F36A File Offset: 0x0001D56A
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x0001F372 File Offset: 0x0001D572
		[Editor(false)]
		public Brush HealthDropBrush
		{
			get
			{
				return this._healthDropBrush;
			}
			set
			{
				if (this._healthDropBrush != value)
				{
					this._healthDropBrush = value;
					base.OnPropertyChanged<Brush>(value, "HealthDropBrush");
				}
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0001F390 File Offset: 0x0001D590
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x0001F398 File Offset: 0x0001D598
		[Editor(false)]
		public bool ShowHealthBar
		{
			get
			{
				return this._showHealthBar;
			}
			set
			{
				if (this._showHealthBar != value)
				{
					this._showHealthBar = value;
					base.OnPropertyChanged(value, "ShowHealthBar");
				}
			}
		}

		// Token: 0x040004FD RID: 1277
		private float AnimationDelay = 0.2f;

		// Token: 0x040004FE RID: 1278
		private float AnimationDuration = 0.8f;

		// Token: 0x040004FF RID: 1279
		private float _previousHealthRatio;

		// Token: 0x04000500 RID: 1280
		private List<AgentHealthWidget.HealthDropData> _healthDrops;

		// Token: 0x04000501 RID: 1281
		private int _health;

		// Token: 0x04000502 RID: 1282
		private int _maxHealth;

		// Token: 0x04000503 RID: 1283
		private bool _showHealthBar;

		// Token: 0x04000504 RID: 1284
		private FillBarWidget _healthBar;

		// Token: 0x04000505 RID: 1285
		private Widget _healthDropContainer;

		// Token: 0x04000506 RID: 1286
		private Brush _healthDropBrush;

		// Token: 0x020001C1 RID: 449
		public class HealthDropData
		{
			// Token: 0x06001594 RID: 5524 RVA: 0x0003ABD6 File Offset: 0x00038DD6
			public HealthDropData(BrushWidget widget, float lifeTime)
			{
				this.Widget = widget;
				this.LifeTime = lifeTime;
			}

			// Token: 0x04000A39 RID: 2617
			public BrushWidget Widget;

			// Token: 0x04000A3A RID: 2618
			public float LifeTime;
		}
	}
}
