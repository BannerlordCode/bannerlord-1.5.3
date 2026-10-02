using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000091 RID: 145
	public class MultiplayerScoreboardAnimatedFillBarWidget : FillBarWidget
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007F9 RID: 2041 RVA: 0x0001735C File Offset: 0x0001555C
		// (remove) Token: 0x060007FA RID: 2042 RVA: 0x00017394 File Offset: 0x00015594
		public event MultiplayerScoreboardAnimatedFillBarWidget.FullFillFinishedHandler OnFullFillFinished;

		// Token: 0x060007FB RID: 2043 RVA: 0x000173C9 File Offset: 0x000155C9
		public MultiplayerScoreboardAnimatedFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000173E8 File Offset: 0x000155E8
		public void StartAnimation(float animationDelay = 0f)
		{
			if (base.FillWidget == null || base.ChangeWidget == null || MathF.Abs(this.AnimationFillSpeed) <= 1E-45f)
			{
				return;
			}
			this.AnimationDelay = animationDelay;
			this._ratioOfChangePerTick = this.AnimationFillSpeed;
			this._isXPIncreasing = this.TimesOfFullFill > 0 || (this.TimesOfFullFill == 0 && base.CurrentAmountAsFloat > base.InitialAmountAsFloat);
			if (!this._isStarted)
			{
				base.Context.TwoDimensionContext.CreateSoundEvent(this._xpBarSoundEventName);
				base.Context.TwoDimensionContext.PlaySoundEvent(this._xpBarSoundEventName);
			}
			this._isStarted = true;
			this.CalculateTargetValues();
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00017498 File Offset: 0x00015698
		public void Reset()
		{
			this._normalFillFromValue = 0f;
			this._normalFillToValue = 0f;
			this._highlightedFillFromValue = 0f;
			this._highlightedFillToValue = 0f;
			this._ratioOfChangePerTick = this.AnimationFillSpeed;
			this._animationDelayTimer = 0f;
			this._lerpRatioPerTick = 0f;
			this.AnimationDelay = 0f;
			this._isStarted = false;
			this._shouldStopLerping = false;
			this._isFirstStepCalculated = false;
			this._isProgressCompleted = false;
			base.Context.TwoDimensionContext.StopAndRemoveSoundEvent(this._xpBarSoundEventName);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00017530 File Offset: 0x00015730
		protected override void OnUpdate(float dt)
		{
			if (!this._isStarted || this._isProgressCompleted)
			{
				return;
			}
			this._animationDelayTimer += dt;
			if (this._animationDelayTimer >= this.AnimationDelay)
			{
				this._lerpRatioPerTick += dt * this._ratioOfChangePerTick;
				this._lerpRatioPerTick = Mathf.Clamp(this._lerpRatioPerTick, 0f, 1f);
				if (this._lerpRatioPerTick >= 1f && !this._shouldStopLerping)
				{
					this._lerpRatioPerTick = 0f;
					this.CalculateTargetValues();
					return;
				}
				if (this._lerpRatioPerTick >= 1f)
				{
					this._isProgressCompleted = true;
				}
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000175D8 File Offset: 0x000157D8
		protected override void OnLateUpdate(float dt)
		{
			if (base.FillWidget != null)
			{
				this.ChangeFillAmountOfWidget(base.FillWidget, this._normalFillFromValue, this._normalFillToValue, this._lerpRatioPerTick);
			}
			if (base.ChangeWidget != null)
			{
				this.ChangeFillAmountOfWidget(base.ChangeWidget, this._highlightedFillFromValue, this._highlightedFillToValue, this._lerpRatioPerTick);
			}
			if (base.DividerWidget != null)
			{
				if (this._isXPIncreasing && base.ChangeWidget != null)
				{
					base.DividerWidget.ScaledPositionXOffset = base.ChangeWidget.ScaledSuggestedWidth;
					base.DividerWidget.Color = Color.FromUint(uint.MaxValue);
				}
				else if (base.FillWidget != null)
				{
					base.DividerWidget.ScaledPositionXOffset = base.FillWidget.ScaledSuggestedWidth;
					base.DividerWidget.Color = Color.FromUint(4293185972U);
				}
				base.DividerWidget.ScaledPositionXOffset -= base.DividerWidget.Size.X;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000176CC File Offset: 0x000158CC
		private void CalculateTargetValues()
		{
			this.SetRegularFromValues(0f, false);
			bool flag = !this._isFirstStepCalculated;
			if (!this._isFirstStepCalculated)
			{
				float num = Mathf.Clamp(Mathf.Clamp(base.InitialAmountAsFloat, 0f, base.MaxAmountAsFloat) / base.MaxAmountAsFloat, 0f, 1f);
				this.SetRegularFromValues(num, true);
				this._isFirstStepCalculated = true;
			}
			this.DecideNextStep(flag);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001773A File Offset: 0x0001593A
		private void DecideNextStep(bool isFirstStep)
		{
			if (this.DoHaveFullFillStep())
			{
				this.FullFillStep();
			}
			else
			{
				this.LastFillStep();
			}
			if (!isFirstStep)
			{
				MultiplayerScoreboardAnimatedFillBarWidget.FullFillFinishedHandler onFullFillFinished = this.OnFullFillFinished;
				if (onFullFillFinished == null)
				{
					return;
				}
				onFullFillFinished(this._isXPIncreasing);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001776C File Offset: 0x0001596C
		private void LastFillStep()
		{
			float num = Mathf.Clamp(Mathf.Clamp(base.CurrentAmountAsFloat, 0f, base.MaxAmountAsFloat) / base.MaxAmountAsFloat, 0f, 1f);
			if (this._isXPIncreasing)
			{
				this._highlightedFillToValue = num;
			}
			else
			{
				this._normalFillToValue = num;
			}
			this._shouldStopLerping = true;
			base.Context.TwoDimensionContext.StopAndRemoveSoundEvent(this._xpBarSoundEventName);
			base.Context.TwoDimensionContext.PlaySound(this._xpBarStopSoundEventName);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000177F4 File Offset: 0x000159F4
		private void FullFillStep()
		{
			if (this.DoHaveFullFillStep())
			{
				if (this._isXPIncreasing)
				{
					this._highlightedFillToValue = 1f;
				}
				else
				{
					this._normalFillToValue = 0f;
				}
				this.TimesOfFullFill -= Math.Sign(this.TimesOfFullFill);
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00017844 File Offset: 0x00015A44
		private void SetRegularFromValues(float inputFromValue = 0f, bool useInputFromValue = false)
		{
			float num = (useInputFromValue ? inputFromValue : ((float)(this._isXPIncreasing ? 0 : 1)));
			this._normalFillFromValue = num;
			this._highlightedFillFromValue = num;
			this.StopMovementOfFillAmountAtFromValue(!this._isXPIncreasing);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00017882 File Offset: 0x00015A82
		private bool DoHaveFullFillStep()
		{
			return Math.Abs(this.TimesOfFullFill) > 0;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00017892 File Offset: 0x00015A92
		private void StopMovementOfFillAmountAtFromValue(bool stopHighlightedFill)
		{
			if (stopHighlightedFill)
			{
				this._highlightedFillToValue = this._highlightedFillFromValue;
				return;
			}
			this._normalFillToValue = this._normalFillFromValue;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000178B0 File Offset: 0x00015AB0
		private void ChangeFillAmountOfWidget(Widget fillWidget, float fromValue, float toValue, float stepSize)
		{
			float num = Mathf.Lerp(fromValue, toValue, stepSize);
			fillWidget.ScaledSuggestedWidth = num * fillWidget.ParentWidget.Size.X;
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x000178DF File Offset: 0x00015ADF
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x000178E7 File Offset: 0x00015AE7
		[Editor(false)]
		public bool IsStartRequested
		{
			get
			{
				return this._isStartRequested;
			}
			set
			{
				if (value != this._isStartRequested)
				{
					this._isStartRequested = value;
					if (this._isStartRequested)
					{
						this.Reset();
						this.StartAnimation(0f);
					}
					base.OnPropertyChanged(value, "IsStartRequested");
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0001791E File Offset: 0x00015B1E
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00017926 File Offset: 0x00015B26
		[Editor(false)]
		public float AnimationDelay
		{
			get
			{
				return this._animationDelay;
			}
			set
			{
				if (this._animationDelay != value)
				{
					this._animationDelay = value;
					base.OnPropertyChanged(value, "AnimationDelay");
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00017944 File Offset: 0x00015B44
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x0001794C File Offset: 0x00015B4C
		[Editor(false)]
		public float AnimationFillSpeed
		{
			get
			{
				return this._animationFillSpeed;
			}
			set
			{
				if (this._animationFillSpeed != value)
				{
					this._animationFillSpeed = value;
					base.OnPropertyChanged(value, "AnimationFillSpeed");
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0001796A File Offset: 0x00015B6A
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x00017972 File Offset: 0x00015B72
		[Editor(false)]
		public int TimesOfFullFill
		{
			get
			{
				return this._timesOfFullFill;
			}
			set
			{
				if (this._timesOfFullFill != value)
				{
					this._timesOfFullFill = value;
					base.OnPropertyChanged(value, "TimesOfFullFill");
				}
			}
		}

		// Token: 0x04000380 RID: 896
		private bool _isStarted;

		// Token: 0x04000381 RID: 897
		private bool _shouldStopLerping;

		// Token: 0x04000382 RID: 898
		private bool _isXPIncreasing;

		// Token: 0x04000383 RID: 899
		private bool _isFirstStepCalculated;

		// Token: 0x04000384 RID: 900
		private bool _isProgressCompleted;

		// Token: 0x04000385 RID: 901
		private float _ratioOfChangePerTick;

		// Token: 0x04000386 RID: 902
		private float _lerpRatioPerTick;

		// Token: 0x04000387 RID: 903
		private float _animationDelayTimer;

		// Token: 0x04000388 RID: 904
		private float _highlightedFillFromValue;

		// Token: 0x04000389 RID: 905
		private float _highlightedFillToValue;

		// Token: 0x0400038A RID: 906
		private float _normalFillFromValue;

		// Token: 0x0400038B RID: 907
		private float _normalFillToValue;

		// Token: 0x0400038C RID: 908
		private string _xpBarSoundEventName = "multiplayer/xpbar";

		// Token: 0x0400038D RID: 909
		private string _xpBarStopSoundEventName = "multiplayer/xpbar_stop";

		// Token: 0x0400038F RID: 911
		private bool _isStartRequested;

		// Token: 0x04000390 RID: 912
		private float _animationDelay;

		// Token: 0x04000391 RID: 913
		private float _animationFillSpeed;

		// Token: 0x04000392 RID: 914
		private int _timesOfFullFill;

		// Token: 0x020001B9 RID: 441
		// (Invoke) Token: 0x06001578 RID: 5496
		public delegate void FullFillFinishedHandler(bool isPositive);
	}
}
