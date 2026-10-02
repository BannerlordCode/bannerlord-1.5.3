using System;
using System.Numerics;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000052 RID: 82
	public class BrushWidget : Widget
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000172A4 File Offset: 0x000154A4
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x0001731E File Offset: 0x0001551E
		[Editor(false)]
		public Brush Brush
		{
			get
			{
				if (this._originalBrush == null)
				{
					this._originalBrush = base.Context.DefaultBrush;
					this._clonedBrush = this._originalBrush.Clone();
					this.BrushRenderer.Brush = this.ReadOnlyBrush;
				}
				else if (this._clonedBrush == null)
				{
					this._clonedBrush = this._originalBrush.Clone();
					this.BrushRenderer.Brush = this.ReadOnlyBrush;
				}
				return this._clonedBrush;
			}
			set
			{
				if (this._originalBrush != value)
				{
					this._originalBrush = value;
					this._clonedBrush = null;
					this.OnBrushChanged();
					base.OnPropertyChanged<Brush>(value, "Brush");
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00017349 File Offset: 0x00015549
		public Brush ReadOnlyBrush
		{
			get
			{
				if (this._clonedBrush != null)
				{
					return this._clonedBrush;
				}
				if (this._originalBrush == null)
				{
					this._originalBrush = base.Context.DefaultBrush;
				}
				return this._originalBrush;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00017379 File Offset: 0x00015579
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00017395 File Offset: 0x00015595
		[Editor(false)]
		public new Sprite Sprite
		{
			get
			{
				return this.ReadOnlyBrush.DefaultStyle.GetLayer("Default").Sprite;
			}
			set
			{
				this.Brush.DefaultStyle.GetLayer("Default").Sprite = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000173B2 File Offset: 0x000155B2
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x000173BA File Offset: 0x000155BA
		public BrushRenderer BrushRenderer { get; private set; }

		// Token: 0x06000589 RID: 1417 RVA: 0x000173C3 File Offset: 0x000155C3
		public BrushWidget(UIContext context)
			: base(context)
		{
			this.BrushRenderer = new BrushRenderer();
			base.EventFire += this.BrushWidget_EventFire;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000173EC File Offset: 0x000155EC
		private void BrushWidget_EventFire(Widget arg1, string eventName, object[] arg3)
		{
			if (this.ReadOnlyBrush != null)
			{
				AudioProperty eventAudioProperty = this.ReadOnlyBrush.SoundProperties.GetEventAudioProperty(eventName);
				if (eventAudioProperty != null && eventAudioProperty.AudioName != null && !eventAudioProperty.AudioName.Equals(""))
				{
					base.EventManager.Context.TwoDimensionContext.PlaySound(eventAudioProperty.AudioName);
				}
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001744B File Offset: 0x0001564B
		public override void UpdateBrushes(float dt)
		{
			this.UpdateBrushRendererInternal(dt);
			if (!this.IsBrushUpdateNeeded())
			{
				this.UnRegisterUpdateBrushes();
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00017462 File Offset: 0x00015662
		protected bool IsBrushUpdateNeeded()
		{
			Brush brush = this.Brush;
			return base.IsVisible && this.BrushRenderer.IsUpdateNeeded() && this.AreaRect.IsCollide(in base.EventManager.AreaRectangle);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00017498 File Offset: 0x00015698
		protected void UpdateBrushRendererInternal(float dt)
		{
			UIContext context = base.Context;
			bool flag;
			if (context == null)
			{
				flag = null != null;
			}
			else
			{
				TwoDimensionContext twoDimensionContext = context.TwoDimensionContext;
				flag = ((twoDimensionContext != null) ? twoDimensionContext.Platform : null) != null;
			}
			if (!flag)
			{
				Debug.FailedAssert("Trying to update brush renderer after context or platform is finalized", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\BaseTypes\\BrushWidget.cs", "UpdateBrushRendererInternal", 129);
				return;
			}
			this.BrushRenderer.ForcePixelPerfectPlacement = base.ForcePixelPerfectRenderPlacement;
			this.BrushRenderer.UseLocalTimer = !base.UseGlobalTimeForAnimation;
			this.BrushRenderer.Brush = this.ReadOnlyBrush;
			this.BrushRenderer.CurrentState = base.CurrentState;
			this.BrushRenderer.Update(base.EventManager.LocalFrameNumber, base.Context.TwoDimensionContext.Platform.ApplicationTime, dt);
			if (base.RestartAnimationFirstFrame && !this._animRestarted)
			{
				base.EventManager.AddLateUpdateAction(this, delegate(float _dt)
				{
					if (base.RestartAnimationFirstFrame)
					{
						this.BrushRenderer.RestartAnimation();
					}
				}, 5);
				this._animRestarted = true;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00017584 File Offset: 0x00015784
		public override void SetState(string stateName)
		{
			if (base.CurrentState != stateName)
			{
				if (base.EventManager != null && this.ReadOnlyBrush != null)
				{
					AudioProperty stateAudioProperty = this.ReadOnlyBrush.SoundProperties.GetStateAudioProperty(stateName);
					if (stateAudioProperty != null)
					{
						if (stateAudioProperty.AudioName != null && !stateAudioProperty.AudioName.Equals(""))
						{
							base.EventManager.Context.TwoDimensionContext.PlaySound(stateAudioProperty.AudioName);
						}
						else
						{
							Debug.FailedAssert(string.Concat(new string[] { "Widget with id \"", base.Id, "\" has a sound having no audioName for event \"", stateName, "\"!" }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\BaseTypes\\BrushWidget.cs", "SetState", 169);
						}
					}
				}
				this.RegisterUpdateBrushes();
			}
			base.SetState(stateName);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00017655 File Offset: 0x00015855
		protected override void RefreshState()
		{
			base.RefreshState();
			this.RegisterUpdateBrushes();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00017664 File Offset: 0x00015864
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (this.IsBrushUpdateNeeded() && base.EventManager.LocalFrameNumber != this.BrushRenderer.LastUpdatedFrameNumber)
			{
				this.RegisterUpdateBrushes();
				this.UpdateBrushRendererInternal(base.EventManager.CachedDt);
			}
			this.BrushRenderer.Render(drawContext, in this.AreaRect, base._scaleToUse, base.Context.ContextAlpha, default(Vector2), default(Vector2));
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000176DD File Offset: 0x000158DD
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this.BrushRenderer.SetSeed(this._seed);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000176F8 File Offset: 0x000158F8
		public override void UpdateAnimationPropertiesSubTask(float alphaFactor)
		{
			this.Brush.GlobalAlphaFactor = alphaFactor;
			foreach (Widget widget in base.Children)
			{
				widget.UpdateAnimationPropertiesSubTask(alphaFactor);
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00017758 File Offset: 0x00015958
		public virtual void OnBrushChanged()
		{
			this.RegisterUpdateBrushes();
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00017760 File Offset: 0x00015960
		protected void RegisterUpdateBrushes()
		{
			base.EventManager.RegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, this);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001776F File Offset: 0x0001596F
		protected void UnRegisterUpdateBrushes()
		{
			base.EventManager.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, this);
		}

		// Token: 0x040002A4 RID: 676
		private Brush _originalBrush;

		// Token: 0x040002A5 RID: 677
		private Brush _clonedBrush;

		// Token: 0x040002A7 RID: 679
		private bool _animRestarted;
	}
}
