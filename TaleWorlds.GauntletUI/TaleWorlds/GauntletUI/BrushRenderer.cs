using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000017 RID: 23
	public class BrushRenderer
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007DF7 File Offset: 0x00005FF7
		private float _brushTimer
		{
			get
			{
				if (!this.UseLocalTimer)
				{
					return this._globalTime;
				}
				return this._brushLocalTimer;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00007E0E File Offset: 0x0000600E
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007E16 File Offset: 0x00006016
		public ulong LastUpdatedFrameNumber { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007E1F File Offset: 0x0000601F
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00007E27 File Offset: 0x00006027
		public bool ForcePixelPerfectPlacement { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007E30 File Offset: 0x00006030
		public Style CurrentStyle
		{
			get
			{
				return this._styleOfCurrentState;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00007E38 File Offset: 0x00006038
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00007E40 File Offset: 0x00006040
		public Brush Brush
		{
			get
			{
				return this._brush;
			}
			set
			{
				if (this._brush != value)
				{
					this._brush = value;
					this._brushLocalTimer = 0f;
					int num = ((this._brush != null) ? this._brush.Layers.Count : 0);
					if (this._startBrushLayerState == null)
					{
						this._startBrushLayerState = new Dictionary<string, BrushLayerState>(num);
						this._currentBrushLayerState = new Dictionary<string, BrushLayerState>(num);
					}
					else
					{
						this._startBrushLayerState.Clear();
						this._currentBrushLayerState.Clear();
					}
					if (this._brush != null)
					{
						this._styleOfCurrentState = this._brush.DefaultStyle;
						if (!string.IsNullOrEmpty(this.CurrentState))
						{
							this._styleOfCurrentState = this._brush.GetStyleOrDefault(this.CurrentState);
						}
						BrushState brushState = default(BrushState);
						brushState.FillFrom(this._styleOfCurrentState);
						this._startBrushState = brushState;
						this._currentBrushState = brushState;
						foreach (StyleLayer styleLayer in this._styleOfCurrentState.GetLayers())
						{
							BrushLayerState brushLayerState = default(BrushLayerState);
							brushLayerState.FillFrom(styleLayer);
							this._startBrushLayerState[styleLayer.Name] = brushLayerState;
							this._currentBrushLayerState[styleLayer.Name] = brushLayerState;
						}
					}
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00007F7A File Offset: 0x0000617A
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00007F84 File Offset: 0x00006184
		public string CurrentState
		{
			get
			{
				return this._currentState;
			}
			set
			{
				if (this._currentState != value)
				{
					string currentState = this._currentState;
					this._brushLocalTimer = 0f;
					this._currentState = value;
					this._startBrushState = this._currentBrushState;
					foreach (KeyValuePair<string, BrushLayerState> keyValuePair in this._currentBrushLayerState)
					{
						this._startBrushLayerState[keyValuePair.Key] = keyValuePair.Value;
					}
					if (this.Brush != null)
					{
						Style styleOrDefault = this.Brush.GetStyleOrDefault(this.CurrentState);
						this._styleOfCurrentState = styleOrDefault;
						this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.None;
						if (styleOrDefault.AnimationMode == StyleAnimationMode.BasicTransition)
						{
							if (!string.IsNullOrEmpty(currentState))
							{
								this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.PlayingBasicTranisition;
								return;
							}
						}
						else if (styleOrDefault.AnimationMode == StyleAnimationMode.Animation && (!string.IsNullOrEmpty(currentState) || !string.IsNullOrEmpty(styleOrDefault.AnimationToPlayOnBegin)))
						{
							this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.PlayingAnimation;
						}
					}
				}
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008088 File Offset: 0x00006288
		public BrushRenderer()
		{
			this._startBrushState = default(BrushState);
			this._currentBrushState = default(BrushState);
			this._startBrushLayerState = new Dictionary<string, BrushLayerState>();
			this._currentBrushLayerState = new Dictionary<string, BrushLayerState>();
			this._brushLocalTimer = 0f;
			this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.None;
			this._cachedImageFit = new ImageFit();
			this._randomXOffset = -1f;
			this._randomYOffset = -1f;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000080FC File Offset: 0x000062FC
		private float GetRandomXOffset()
		{
			if (this._randomXOffset < 0f)
			{
				Random random = new Random(this._offsetSeed);
				this._randomXOffset = (float)random.Next(0, 2048);
				this._randomYOffset = (float)random.Next(0, 2048);
			}
			return this._randomXOffset;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008150 File Offset: 0x00006350
		private float GetRandomYOffset()
		{
			if (this._randomYOffset < 0f)
			{
				Random random = new Random(this._offsetSeed);
				this._randomXOffset = (float)random.Next(0, 2048);
				this._randomYOffset = (float)random.Next(0, 2048);
			}
			return this._randomYOffset;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000081A4 File Offset: 0x000063A4
		public void Update(ulong frameNumber, float globalAnimTime, float dt)
		{
			this._globalTime = globalAnimTime;
			this.LastUpdatedFrameNumber = frameNumber;
			this._brushLocalTimer += dt;
			if (this.Brush != null)
			{
				Style styleOfCurrentState = this._styleOfCurrentState;
				if ((this._brushRendererAnimationState == BrushRenderer.BrushRendererAnimationState.None || this._brushRendererAnimationState == BrushRenderer.BrushRendererAnimationState.Ended) && (!string.IsNullOrEmpty(styleOfCurrentState.AnimationToPlayOnBegin) || this._styleOfCurrentState.Version != this._latestStyleVersion))
				{
					this._latestStyleVersion = styleOfCurrentState.Version;
					BrushState brushState = default(BrushState);
					brushState.FillFrom(styleOfCurrentState);
					this._startBrushState = brushState;
					this._currentBrushState = brushState;
					foreach (StyleLayer styleLayer in styleOfCurrentState.GetLayers())
					{
						BrushLayerState brushLayerState = default(BrushLayerState);
						brushLayerState.FillFrom(styleLayer);
						this._currentBrushLayerState[styleLayer.Name] = brushLayerState;
						this._startBrushLayerState[styleLayer.Name] = brushLayerState;
					}
					return;
				}
				if (this._brushRendererAnimationState == BrushRenderer.BrushRendererAnimationState.PlayingBasicTranisition)
				{
					float num = (this.UseLocalTimer ? this._brushLocalTimer : globalAnimTime);
					if (num >= this.Brush.TransitionDuration)
					{
						this.EndAnimation();
						return;
					}
					float num2 = num / this.Brush.TransitionDuration;
					if (num2 > 1f)
					{
						num2 = 1f;
					}
					BrushState startBrushState = this._startBrushState;
					BrushState brushState2 = default(BrushState);
					brushState2.LerpFrom(startBrushState, styleOfCurrentState, num2);
					this._currentBrushState = brushState2;
					foreach (StyleLayer styleLayer2 in styleOfCurrentState.GetLayers())
					{
						BrushLayerState brushLayerState2 = this._startBrushLayerState[styleLayer2.Name];
						BrushLayerState brushLayerState3 = default(BrushLayerState);
						brushLayerState3.LerpFrom(brushLayerState2, styleLayer2, num2);
						this._currentBrushLayerState[styleLayer2.Name] = brushLayerState3;
					}
					return;
				}
				else if (this._brushRendererAnimationState == BrushRenderer.BrushRendererAnimationState.PlayingAnimation)
				{
					string animationToPlayOnBegin = styleOfCurrentState.AnimationToPlayOnBegin;
					BrushAnimation animation = this.Brush.GetAnimation(animationToPlayOnBegin);
					if (animation == null || (!animation.Loop && this._brushTimer >= animation.Duration))
					{
						this.EndAnimation();
						return;
					}
					float num3 = this._brushTimer % animation.Duration;
					bool flag = this._brushTimer < animation.Duration;
					BrushState startBrushState2 = this._startBrushState;
					BrushLayerAnimation styleAnimation = animation.StyleAnimation;
					BrushState brushState3 = this.AnimateBrushState(animation, styleAnimation, num3, flag, startBrushState2, styleOfCurrentState);
					this._currentBrushState = brushState3;
					foreach (StyleLayer styleLayer3 in styleOfCurrentState.GetLayers())
					{
						BrushLayerState brushLayerState4 = this._startBrushLayerState[styleLayer3.Name];
						BrushLayerAnimation layerAnimation = animation.GetLayerAnimation(styleLayer3.Name);
						BrushLayerState brushLayerState5 = this.AnimateBrushLayerState(animation, layerAnimation, num3, flag, brushLayerState4, styleLayer3);
						this._currentBrushLayerState[styleLayer3.Name] = brushLayerState5;
					}
				}
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000845C File Offset: 0x0000665C
		private BrushLayerState AnimateBrushLayerState(BrushAnimation animation, BrushLayerAnimation layerAnimation, float brushStateTimer, bool isFirstCycle, BrushLayerState startState, IBrushLayerData source)
		{
			BrushLayerState brushLayerState = default(BrushLayerState);
			brushLayerState.FillFrom(source);
			if (layerAnimation != null)
			{
				foreach (BrushAnimationProperty brushAnimationProperty in layerAnimation.Collections)
				{
					BrushAnimationProperty.BrushAnimationPropertyType propertyType = brushAnimationProperty.PropertyType;
					BrushAnimationKeyFrame brushAnimationKeyFrame = null;
					BrushAnimationKeyFrame brushAnimationKeyFrame2;
					if (animation.Loop)
					{
						BrushAnimationKeyFrame frameAt = brushAnimationProperty.GetFrameAt(0);
						if (isFirstCycle && this._brushTimer < frameAt.Time)
						{
							brushAnimationKeyFrame2 = frameAt;
						}
						else
						{
							brushAnimationKeyFrame2 = brushAnimationProperty.GetFrameAfter(brushStateTimer);
							if (brushAnimationKeyFrame2 == null)
							{
								brushAnimationKeyFrame2 = frameAt;
								brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationProperty.Count - 1);
							}
							else if (brushAnimationKeyFrame2 == frameAt)
							{
								brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationProperty.Count - 1);
							}
							else
							{
								brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationKeyFrame2.Index - 1);
							}
						}
					}
					else
					{
						brushAnimationKeyFrame2 = brushAnimationProperty.GetFrameAfter(brushStateTimer);
						if (brushAnimationKeyFrame2 != null)
						{
							brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationKeyFrame2.Index - 1);
						}
						else
						{
							brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationProperty.Count - 1);
						}
					}
					BrushAnimationKeyFrame brushAnimationKeyFrame3 = null;
					BrushLayerState brushLayerState2 = default(BrushLayerState);
					IBrushLayerData brushLayerData = null;
					BrushAnimationKeyFrame brushAnimationKeyFrame4 = null;
					float num3;
					if (brushAnimationKeyFrame2 != null)
					{
						if (brushAnimationKeyFrame != null)
						{
							float num;
							float num2;
							if (animation.Loop)
							{
								if (brushAnimationKeyFrame2.Index == 0)
								{
									num = brushAnimationKeyFrame2.Time + (animation.Duration - brushAnimationKeyFrame.Time);
									if (brushStateTimer >= brushAnimationKeyFrame.Time)
									{
										num2 = brushStateTimer - brushAnimationKeyFrame.Time;
									}
									else
									{
										num2 = animation.Duration - brushAnimationKeyFrame.Time + brushStateTimer;
									}
								}
								else
								{
									num = brushAnimationKeyFrame2.Time - brushAnimationKeyFrame.Time;
									num2 = brushStateTimer - brushAnimationKeyFrame.Time;
								}
							}
							else
							{
								num = brushAnimationKeyFrame2.Time - brushAnimationKeyFrame.Time;
								num2 = brushStateTimer - brushAnimationKeyFrame.Time;
							}
							num3 = num2 * (1f / num);
							brushAnimationKeyFrame3 = brushAnimationKeyFrame;
							brushAnimationKeyFrame4 = brushAnimationKeyFrame2;
						}
						else
						{
							num3 = brushStateTimer * (1f / brushAnimationKeyFrame2.Time);
							brushLayerState2 = startState;
							brushAnimationKeyFrame4 = brushAnimationKeyFrame2;
						}
					}
					else
					{
						num3 = (brushStateTimer - brushAnimationKeyFrame.Time) * (1f / (animation.Duration - brushAnimationKeyFrame.Time));
						brushAnimationKeyFrame3 = brushAnimationKeyFrame;
						brushLayerData = source;
					}
					num3 = AnimationInterpolation.Ease(animation.InterpolationType, animation.InterpolationFunction, num3);
					switch (propertyType)
					{
					case BrushAnimationProperty.BrushAnimationPropertyType.ColorFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.AlphaFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.HueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.SaturationFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.ValueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverlayXOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverlayYOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextOutlineAmount:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextGlowRadius:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextBlur:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextShadowOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextShadowAngle:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextColorFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextAlphaFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextHueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextSaturationFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextValueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.XOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.YOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.Rotation:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverridenWidth:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverridenHeight:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendLeft:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendRight:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendTop:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendBottom:
					{
						float num4 = ((brushAnimationKeyFrame3 != null) ? brushAnimationKeyFrame3.GetValueAsFloat() : brushLayerState2.GetValueAsFloat(propertyType));
						float num5 = ((brushLayerData != null) ? brushLayerData.GetValueAsFloat(propertyType) : brushAnimationKeyFrame4.GetValueAsFloat());
						brushLayerState.SetValueAsFloat(propertyType, MathF.Lerp(num4, num5, num3, 1E-05f));
						break;
					}
					case BrushAnimationProperty.BrushAnimationPropertyType.Color:
					case BrushAnimationProperty.BrushAnimationPropertyType.FontColor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextGlowColor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextOutlineColor:
					{
						Color color = ((brushAnimationKeyFrame3 != null) ? brushAnimationKeyFrame3.GetValueAsColor() : brushLayerState2.GetValueAsColor(propertyType));
						Color color2 = ((brushLayerData != null) ? brushLayerData.GetValueAsColor(propertyType) : brushAnimationKeyFrame4.GetValueAsColor());
						BrushAnimationProperty.BrushAnimationPropertyType brushAnimationPropertyType = propertyType;
						Color color3 = Color.Lerp(color, color2, num3);
						brushLayerState.SetValueAsColor(brushAnimationPropertyType, in color3);
						break;
					}
					case BrushAnimationProperty.BrushAnimationPropertyType.Sprite:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverlaySprite:
					{
						Sprite sprite = ((brushAnimationKeyFrame3 != null) ? brushAnimationKeyFrame3.GetValueAsSprite() : null) ?? brushLayerState2.GetValueAsSprite(propertyType);
						Sprite sprite2 = ((brushLayerData != null) ? brushLayerData.GetValueAsSprite(propertyType) : null) ?? brushAnimationKeyFrame4.GetValueAsSprite();
						brushLayerState.SetValueAsSprite(propertyType, ((double)num3 <= 0.9) ? sprite : sprite2);
						break;
					}
					}
				}
			}
			return brushLayerState;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000884C File Offset: 0x00006A4C
		public bool IsUpdateNeeded()
		{
			return this._brushRendererAnimationState == BrushRenderer.BrushRendererAnimationState.PlayingBasicTranisition || this._brushRendererAnimationState == BrushRenderer.BrushRendererAnimationState.PlayingAnimation || (this._styleOfCurrentState != null && this._styleOfCurrentState.Version != this._latestStyleVersion);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008884 File Offset: 0x00006A84
		private BrushState AnimateBrushState(BrushAnimation animation, BrushLayerAnimation layerAnimation, float brushStateTimer, bool isFirstCycle, BrushState startState, Style source)
		{
			BrushState brushState = default(BrushState);
			brushState.FillFrom(source);
			if (layerAnimation != null)
			{
				foreach (BrushAnimationProperty brushAnimationProperty in layerAnimation.Collections)
				{
					BrushAnimationProperty.BrushAnimationPropertyType propertyType = brushAnimationProperty.PropertyType;
					BrushAnimationKeyFrame brushAnimationKeyFrame = null;
					BrushAnimationKeyFrame brushAnimationKeyFrame2;
					if (animation.Loop)
					{
						BrushAnimationKeyFrame frameAt = brushAnimationProperty.GetFrameAt(0);
						if (isFirstCycle && this._brushTimer < frameAt.Time)
						{
							brushAnimationKeyFrame2 = frameAt;
						}
						else
						{
							brushAnimationKeyFrame2 = brushAnimationProperty.GetFrameAfter(brushStateTimer);
							if (brushAnimationKeyFrame2 == null)
							{
								brushAnimationKeyFrame2 = frameAt;
								brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationProperty.Count - 1);
							}
							else if (brushAnimationKeyFrame2 == frameAt)
							{
								brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationProperty.Count - 1);
							}
							else
							{
								brushAnimationKeyFrame = brushAnimationProperty.GetFrameAt(brushAnimationKeyFrame2.Index - 1);
							}
						}
					}
					else
					{
						brushAnimationKeyFrame2 = brushAnimationProperty.GetFrameAfter(brushStateTimer);
						brushAnimationKeyFrame = ((brushAnimationKeyFrame2 != null) ? brushAnimationProperty.GetFrameAt(brushAnimationKeyFrame2.Index - 1) : brushAnimationProperty.GetFrameAt(brushAnimationProperty.Count - 1));
					}
					BrushAnimationKeyFrame brushAnimationKeyFrame3 = null;
					BrushState brushState2 = default(BrushState);
					Style style = null;
					BrushAnimationKeyFrame brushAnimationKeyFrame4 = null;
					float num3;
					if (brushAnimationKeyFrame2 != null)
					{
						if (brushAnimationKeyFrame != null)
						{
							float num;
							float num2;
							if (animation.Loop)
							{
								if (brushAnimationKeyFrame2.Index == 0)
								{
									num = brushAnimationKeyFrame2.Time + (animation.Duration - brushAnimationKeyFrame.Time);
									if (brushStateTimer >= brushAnimationKeyFrame.Time)
									{
										num2 = brushStateTimer - brushAnimationKeyFrame.Time;
									}
									else
									{
										num2 = animation.Duration - brushAnimationKeyFrame.Time + brushStateTimer;
									}
								}
								else
								{
									num = brushAnimationKeyFrame2.Time - brushAnimationKeyFrame.Time;
									num2 = brushStateTimer - brushAnimationKeyFrame.Time;
								}
							}
							else
							{
								num = brushAnimationKeyFrame2.Time - brushAnimationKeyFrame.Time;
								num2 = brushStateTimer - brushAnimationKeyFrame.Time;
							}
							num3 = num2 * (1f / num);
							brushAnimationKeyFrame3 = brushAnimationKeyFrame;
							brushAnimationKeyFrame4 = brushAnimationKeyFrame2;
						}
						else
						{
							num3 = brushStateTimer * (1f / brushAnimationKeyFrame2.Time);
							brushState2 = startState;
							brushAnimationKeyFrame4 = brushAnimationKeyFrame2;
						}
					}
					else
					{
						num3 = (brushStateTimer - brushAnimationKeyFrame.Time) * (1f / (animation.Duration - brushAnimationKeyFrame.Time));
						brushAnimationKeyFrame3 = brushAnimationKeyFrame;
						style = source;
					}
					num3 = MathF.Clamp(num3, 0f, 1f);
					num3 = AnimationInterpolation.Ease(animation.InterpolationType, animation.InterpolationFunction, num3);
					switch (propertyType)
					{
					case BrushAnimationProperty.BrushAnimationPropertyType.ColorFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.AlphaFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.HueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.SaturationFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.ValueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverlayXOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverlayYOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextOutlineAmount:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextGlowRadius:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextBlur:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextShadowOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextShadowAngle:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextColorFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextAlphaFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextHueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextSaturationFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextValueFactor:
					case BrushAnimationProperty.BrushAnimationPropertyType.XOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.YOffset:
					case BrushAnimationProperty.BrushAnimationPropertyType.Rotation:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverridenWidth:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverridenHeight:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendLeft:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendRight:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendTop:
					case BrushAnimationProperty.BrushAnimationPropertyType.ExtendBottom:
					{
						float num4 = ((brushAnimationKeyFrame3 != null) ? brushAnimationKeyFrame3.GetValueAsFloat() : brushState2.GetValueAsFloat(propertyType));
						float num5 = ((style != null) ? style.GetValueAsFloat(propertyType) : brushAnimationKeyFrame4.GetValueAsFloat());
						brushState.SetValueAsFloat(propertyType, MathF.Lerp(num4, num5, num3, 1E-05f));
						break;
					}
					case BrushAnimationProperty.BrushAnimationPropertyType.Color:
					case BrushAnimationProperty.BrushAnimationPropertyType.FontColor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextGlowColor:
					case BrushAnimationProperty.BrushAnimationPropertyType.TextOutlineColor:
					{
						Color color = ((brushAnimationKeyFrame3 != null) ? brushAnimationKeyFrame3.GetValueAsColor() : brushState2.GetValueAsColor(propertyType));
						Color color2 = ((style != null) ? style.GetValueAsColor(propertyType) : brushAnimationKeyFrame4.GetValueAsColor());
						BrushAnimationProperty.BrushAnimationPropertyType brushAnimationPropertyType = propertyType;
						Color color3 = Color.Lerp(color, color2, num3);
						brushState.SetValueAsColor(brushAnimationPropertyType, in color3);
						break;
					}
					case BrushAnimationProperty.BrushAnimationPropertyType.Sprite:
					case BrushAnimationProperty.BrushAnimationPropertyType.OverlaySprite:
					{
						Sprite sprite = ((brushAnimationKeyFrame3 != null) ? brushAnimationKeyFrame3.GetValueAsSprite() : null) ?? brushState2.GetValueAsSprite(propertyType);
						Sprite sprite2 = ((style != null) ? style.GetValueAsSprite(propertyType) : null) ?? brushAnimationKeyFrame4.GetValueAsSprite();
						brushState.SetValueAsSprite(propertyType, ((double)num3 <= 0.9) ? sprite : sprite2);
						break;
					}
					}
				}
			}
			return brushState;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008C7C File Offset: 0x00006E7C
		private void EndAnimation()
		{
			if (this.Brush != null)
			{
				Style styleOfCurrentState = this._styleOfCurrentState;
				BrushState brushState = default(BrushState);
				brushState.FillFrom(styleOfCurrentState);
				this._startBrushState = brushState;
				this._currentBrushState = brushState;
				if (this.Brush.TransitionDuration == 0f)
				{
					this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.None;
				}
				foreach (StyleLayer styleLayer in styleOfCurrentState.GetLayers())
				{
					BrushLayerState brushLayerState = default(BrushLayerState);
					brushLayerState.FillFrom(styleLayer);
					this._startBrushLayerState[styleLayer.Name] = brushLayerState;
					this._currentBrushLayerState[styleLayer.Name] = brushLayerState;
				}
				this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.Ended;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008D2C File Offset: 0x00006F2C
		public void Render(TwoDimensionDrawContext drawContext, in Rectangle2D rect, float scale, float contextAlpha, Vector2 overlayOffset = default(Vector2), Vector2 overlaySize = default(Vector2))
		{
			if (this.Brush != null)
			{
				Vector2 vector = new Vector2(rect.LocalPosition.X, rect.LocalPosition.Y);
				Vector2 vector2 = new Vector2(rect.LocalScale.X, rect.LocalScale.Y);
				Vector2 vector3 = vector2 / scale;
				if (this.ForcePixelPerfectPlacement)
				{
					vector.X = (float)MathF.Round(vector.X);
					vector.Y = (float)MathF.Round(vector.Y);
				}
				Style styleOfCurrentState = this._styleOfCurrentState;
				for (int i = 0; i < styleOfCurrentState.LayerCount; i++)
				{
					Rectangle2D rectangle2D = rect;
					StyleLayer layer = styleOfCurrentState.GetLayer(i);
					if (!layer.IsHidden)
					{
						BrushLayerState brushLayerState;
						if (this._currentBrushLayerState.Count == 1)
						{
							Dictionary<string, BrushLayerState>.ValueCollection.Enumerator enumerator = this._currentBrushLayerState.Values.GetEnumerator();
							enumerator.MoveNext();
							brushLayerState = enumerator.Current;
						}
						else
						{
							brushLayerState = this._currentBrushLayerState[layer.Name];
						}
						Sprite sprite = brushLayerState.Sprite;
						Texture texture = ((sprite != null) ? sprite.Texture : null);
						if (texture != null)
						{
							float num = vector.X + brushLayerState.XOffset * scale;
							float num2 = vector.Y + brushLayerState.YOffset * scale;
							SimpleMaterial simpleMaterial = drawContext.CreateSimpleMaterial();
							simpleMaterial.OverlayEnabled = false;
							simpleMaterial.CircularMaskingEnabled = false;
							Vector2 vector4;
							if (layer.OverlayMethod == BrushOverlayMethod.CoverWithTexture && layer.OverlaySprite != null)
							{
								Sprite overlaySprite = layer.OverlaySprite;
								Texture texture2 = overlaySprite.Texture;
								if (texture2 != null)
								{
									simpleMaterial.OverlayEnabled = true;
									simpleMaterial.StartCoordinate = new Vector2(num, num2);
									simpleMaterial.Size = vector2;
									simpleMaterial.OverlayTexture = texture2;
									simpleMaterial.UseOverlayAlphaAsMask = layer.UseOverlayAlphaAsMask;
									vector4 = default(Vector2);
									float num3;
									float num4;
									if (overlayOffset != vector4)
									{
										num3 = overlayOffset.X;
										num4 = overlayOffset.Y;
									}
									else if (layer.UseOverlayAlphaAsMask)
									{
										num3 = brushLayerState.XOffset + brushLayerState.OverlayXOffset;
										num4 = brushLayerState.YOffset + brushLayerState.OverlayYOffset;
									}
									else
									{
										num3 = brushLayerState.OverlayXOffset;
										num4 = brushLayerState.OverlayYOffset;
									}
									if (layer.UseRandomBaseOverlayXOffset)
									{
										num3 += this.GetRandomXOffset();
									}
									if (layer.UseRandomBaseOverlayYOffset)
									{
										num4 += this.GetRandomYOffset();
									}
									vector4 = default(Vector2);
									float num5;
									float num6;
									if (overlaySize != vector4)
									{
										num5 = overlaySize.X;
										num6 = overlaySize.Y;
									}
									else if (layer.UseOverlayAlphaAsMask)
									{
										num5 = vector3.X;
										num6 = vector3.Y;
									}
									else
									{
										num5 = (float)overlaySprite.Width;
										num6 = (float)overlaySprite.Height;
									}
									simpleMaterial.OverlayXOffset = num3 * scale;
									simpleMaterial.OverlayYOffset = num4 * scale;
									simpleMaterial.OverlayTextureWidth = num5 * scale;
									simpleMaterial.OverlayTextureHeight = num6 * scale;
									simpleMaterial.Scale = scale;
								}
							}
							simpleMaterial.Texture = texture;
							simpleMaterial.NinePatchParameters = sprite.NinePatchParameters;
							simpleMaterial.Color = brushLayerState.Color * this.Brush.GlobalColor;
							simpleMaterial.ColorFactor = brushLayerState.ColorFactor * this.Brush.GlobalColorFactor;
							simpleMaterial.AlphaFactor = brushLayerState.AlphaFactor * this.Brush.GlobalAlphaFactor * contextAlpha;
							simpleMaterial.HueFactor = brushLayerState.HueFactor;
							simpleMaterial.SaturationFactor = brushLayerState.SaturationFactor;
							simpleMaterial.ValueFactor = brushLayerState.ValueFactor;
							float num7 = 0f;
							float num8 = 0f;
							this._cachedImageFit.Type = layer.ImageFitType;
							this._cachedImageFit.HorizontalAlignment = layer.ImageFitHorizontalAlignment;
							this._cachedImageFit.VerticalAlignment = layer.ImageFitVerticalAlignment;
							this._cachedImageFit.OffsetX = 0f;
							this._cachedImageFit.OffsetY = 0f;
							ImageFit cachedImageFit = this._cachedImageFit;
							vector4 = new Vector2((float)sprite.Width, (float)sprite.Height);
							ImageFitResult fittedRectangle = cachedImageFit.GetFittedRectangle(in vector2, in vector4);
							if (layer.WidthPolicy == BrushLayerSizePolicy.StretchToTarget)
							{
								float num9 = brushLayerState.ExtendLeft;
								if (layer.HorizontalFlip)
								{
									num9 = brushLayerState.ExtendRight;
								}
								num7 = fittedRectangle.Width;
								num7 += (brushLayerState.ExtendRight + brushLayerState.ExtendLeft) * scale;
								num -= num9 * scale;
							}
							else if (layer.WidthPolicy == BrushLayerSizePolicy.Original)
							{
								num7 = (float)sprite.Width * scale;
							}
							else if (layer.WidthPolicy == BrushLayerSizePolicy.Overriden)
							{
								num7 = layer.OverridenWidth * scale;
							}
							if (layer.HeightPolicy == BrushLayerSizePolicy.StretchToTarget)
							{
								float num10 = brushLayerState.ExtendTop;
								if (layer.HorizontalFlip)
								{
									num10 = brushLayerState.ExtendBottom;
								}
								num8 = fittedRectangle.Height;
								num8 += (brushLayerState.ExtendTop + brushLayerState.ExtendBottom) * scale;
								num2 -= num10 * scale;
							}
							else if (layer.HeightPolicy == BrushLayerSizePolicy.Original)
							{
								num8 = (float)sprite.Height * scale;
							}
							else if (layer.HeightPolicy == BrushLayerSizePolicy.Overriden)
							{
								num8 = layer.OverridenHeight * scale;
							}
							if (layer.HorizontalFlip)
							{
								num7 *= -1f;
							}
							if (layer.VerticalFlip)
							{
								num8 *= -1f;
							}
							float num11 = ((vector2.X == 0f) ? 1f : (num7 / vector2.X));
							float num12 = ((vector2.Y == 0f) ? 1f : (num8 / vector2.Y));
							Vector2 vector5 = new Vector2(num - vector.X + fittedRectangle.OffsetX, num2 - vector.Y + fittedRectangle.OffsetY);
							Vector2 vector6 = new Vector2(num11, num12);
							rectangle2D.AddVisualOffset(vector5.X, vector5.Y);
							rectangle2D.AddVisualScale(vector6.X - 1f, vector6.Y - 1f);
							rectangle2D.AddVisualRotationOffset(brushLayerState.Rotation);
							rectangle2D.ValidateVisuals();
							drawContext.DrawSprite(sprite, simpleMaterial, in rectangle2D, scale);
						}
					}
				}
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00009320 File Offset: 0x00007520
		public TextMaterial CreateTextMaterial(TwoDimensionDrawContext drawContext)
		{
			TextMaterial textMaterial = this._currentBrushState.CreateTextMaterial(drawContext);
			if (this.Brush != null)
			{
				textMaterial.ColorFactor *= this.Brush.GlobalColorFactor;
				textMaterial.AlphaFactor *= this.Brush.GlobalAlphaFactor;
				textMaterial.Color *= this.Brush.GlobalColor;
			}
			return textMaterial;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009390 File Offset: 0x00007590
		public void RestartAnimation()
		{
			if (this.Brush != null)
			{
				this._brushLocalTimer = 0f;
				Style styleOfCurrentState = this._styleOfCurrentState;
				this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.None;
				if (styleOfCurrentState != null)
				{
					if (styleOfCurrentState.AnimationMode == StyleAnimationMode.BasicTransition)
					{
						this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.PlayingBasicTranisition;
						return;
					}
					if (styleOfCurrentState.AnimationMode == StyleAnimationMode.Animation)
					{
						this._brushRendererAnimationState = BrushRenderer.BrushRendererAnimationState.PlayingAnimation;
					}
				}
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000093E2 File Offset: 0x000075E2
		public void SetSeed(int seed)
		{
			this._offsetSeed = seed;
		}

		// Token: 0x04000087 RID: 135
		private BrushState _startBrushState;

		// Token: 0x04000088 RID: 136
		private BrushState _currentBrushState;

		// Token: 0x04000089 RID: 137
		private Dictionary<string, BrushLayerState> _startBrushLayerState;

		// Token: 0x0400008A RID: 138
		private Dictionary<string, BrushLayerState> _currentBrushLayerState;

		// Token: 0x0400008B RID: 139
		public bool UseLocalTimer;

		// Token: 0x0400008C RID: 140
		private float _brushLocalTimer;

		// Token: 0x0400008D RID: 141
		private float _globalTime;

		// Token: 0x0400008E RID: 142
		private int _offsetSeed;

		// Token: 0x0400008F RID: 143
		private float _randomXOffset;

		// Token: 0x04000090 RID: 144
		private float _randomYOffset;

		// Token: 0x04000091 RID: 145
		private BrushRenderer.BrushRendererAnimationState _brushRendererAnimationState;

		// Token: 0x04000094 RID: 148
		private Brush _brush;

		// Token: 0x04000095 RID: 149
		private long _latestStyleVersion;

		// Token: 0x04000096 RID: 150
		private string _currentState;

		// Token: 0x04000097 RID: 151
		private Style _styleOfCurrentState;

		// Token: 0x04000098 RID: 152
		private ImageFit _cachedImageFit;

		// Token: 0x0200007A RID: 122
		public enum BrushRendererAnimationState
		{
			// Token: 0x04000433 RID: 1075
			None,
			// Token: 0x04000434 RID: 1076
			PlayingAnimation,
			// Token: 0x04000435 RID: 1077
			PlayingBasicTranisition,
			// Token: 0x04000436 RID: 1078
			Ended
		}
	}
}
