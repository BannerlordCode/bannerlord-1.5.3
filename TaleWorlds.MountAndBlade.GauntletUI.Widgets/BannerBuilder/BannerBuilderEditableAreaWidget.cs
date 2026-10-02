using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.BannerBuilder
{
	// Token: 0x02000195 RID: 405
	public class BannerBuilderEditableAreaWidget : Widget
	{
		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x000395B1 File Offset: 0x000377B1
		// (set) Token: 0x06001500 RID: 5376 RVA: 0x000395B9 File Offset: 0x000377B9
		public ButtonWidget DragWidgetTopRight { get; set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x000395C2 File Offset: 0x000377C2
		// (set) Token: 0x06001502 RID: 5378 RVA: 0x000395CA File Offset: 0x000377CA
		public ButtonWidget DragWidgetRight { get; set; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x000395D3 File Offset: 0x000377D3
		// (set) Token: 0x06001504 RID: 5380 RVA: 0x000395DB File Offset: 0x000377DB
		public ButtonWidget DragWidgetTop { get; set; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x000395E4 File Offset: 0x000377E4
		// (set) Token: 0x06001506 RID: 5382 RVA: 0x000395EC File Offset: 0x000377EC
		public ButtonWidget RotateWidget { get; set; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x000395F5 File Offset: 0x000377F5
		// (set) Token: 0x06001508 RID: 5384 RVA: 0x000395FD File Offset: 0x000377FD
		public BannerTableauWidget BannerTableauWidget { get; set; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x00039606 File Offset: 0x00037806
		// (set) Token: 0x0600150A RID: 5386 RVA: 0x0003960E File Offset: 0x0003780E
		public Widget EditableAreaVisualWidget { get; set; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x00039617 File Offset: 0x00037817
		// (set) Token: 0x0600150C RID: 5388 RVA: 0x0003961F File Offset: 0x0003781F
		public int LayerIndex { get; set; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x00039628 File Offset: 0x00037828
		// (set) Token: 0x0600150E RID: 5390 RVA: 0x00039630 File Offset: 0x00037830
		public bool IsMirrorActive { get; set; }

		// Token: 0x0600150F RID: 5391 RVA: 0x00039639 File Offset: 0x00037839
		public BannerBuilderEditableAreaWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00039642 File Offset: 0x00037842
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.BannerTableauWidget.MeshIndexToUpdate = this.LayerIndex;
			if (!this._initialized)
			{
				this.Initialize();
			}
			this.UpdateRequiredValues();
			this.UpdateEditableAreaVisual();
			this.HandleCursor();
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0003967C File Offset: 0x0003787C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				BannerBuilderEditableAreaWidget.BuilderMode currentMode = this._currentMode;
				if (currentMode != BannerBuilderEditableAreaWidget.BuilderMode.None && currentMode - BannerBuilderEditableAreaWidget.BuilderMode.Rotating <= 4)
				{
					base.EventFired("RefreshBanner", Array.Empty<object>());
				}
			}
			this.HandleRotation();
			this.HandlePositioning();
			this.HandleForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType.Right);
			this.HandleForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType.Top);
			this.HandleForCorner();
			this._latestMousePosition = base.EventManager.MousePosition;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x000396ED File Offset: 0x000378ED
		private void Initialize()
		{
			this._centerOfSigil = new Vec2(0f, 0f);
			this._sizeOfSigil = new Vec2(0f, 0f);
			this._initialized = true;
			this.OnIsLayerPatternChanged(false);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00039728 File Offset: 0x00037928
		private void UpdateRequiredValues()
		{
			float num = this._positionValue.X / (float)this.TotalAreaSize * base.Size.X;
			float num2 = this._positionValue.Y / (float)this.TotalAreaSize * base.Size.Y;
			this._centerOfSigil.x = num;
			this._centerOfSigil.y = num2;
			float num3 = this._sizeValue.X / (float)this.TotalAreaSize * base.Size.X;
			float num4 = this._sizeValue.Y / (float)this.TotalAreaSize * base.Size.Y;
			this._sizeOfSigil.x = num3;
			this._sizeOfSigil.y = num4;
			this._positionLimitMin = 0f;
			this._positionLimitMax = this._positionLimitMin + (float)this.TotalAreaSize;
			this._sizeLimitMax = this.TotalAreaSize;
			this._areaScale = (float)this.TotalAreaSize / base.Size.X;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0003982C File Offset: 0x00037A2C
		private void HandlePositioning()
		{
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Positioning)
				{
					Vector2 vector = base.EventManager.MousePosition - this._latestMousePosition;
					vector *= (float)this.TotalAreaSize / base.Size.X;
					Vector2 vector2 = new Vector2(this.PositionValue.X, this.PositionValue.Y);
					vector2 += vector;
					vector2 = new Vector2(MathF.Clamp(vector2.X, this._positionLimitMin, this._positionLimitMax), MathF.Clamp(vector2.Y, this._positionLimitMin, this._positionLimitMax));
					this.PositionValue = vector2;
					this.BannerTableauWidget.UpdatePositionValueManual = this.PositionValue;
				}
				if (this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.Positioning && base.EventManager.HoveredWidget == this)
				{
					this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.Positioning;
					return;
				}
			}
			else
			{
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00039928 File Offset: 0x00037B28
		private void HandleRotation()
		{
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Rotating)
				{
					Vec2 vec = base.GlobalPosition + this._centerOfSigil;
					Vector2 vector = base.EventManager.MousePosition - new Vector2(vec.X, vec.y);
					vector.Y *= -1f;
					float num = BannerBuilderEditableAreaWidget.AngleFromDir(vector);
					this.RotationValue = (float)Math.Round((double)num, 3);
					this.BannerTableauWidget.UpdateRotationValueManualWithMirror = new ValueTuple<float, bool>(this.RotationValue, this.IsMirrorActive);
				}
				if (this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.Rotating && base.EventManager.HoveredWidget == this.RotateWidget)
				{
					this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.Rotating;
				}
			}
			else
			{
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
			this.UpdatePositionOfWidget(this.RotateWidget, BannerBuilderEditableAreaWidget.WidgetPlacementType.Vertical, this.RotationValue, 55f, 30f);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00039A1C File Offset: 0x00037C1C
		private void HandleForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType resizeType)
		{
			ButtonWidget widgetFor = this.GetWidgetFor(resizeType);
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing || this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing)
				{
					Vector2 vector = base.EventManager.MousePosition - this._resizeStartMousePosition;
					vector.Y *= -1f;
					Vec2 vec = BannerBuilderEditableAreaWidget.DirFromAngle(this.RotationValue);
					vec.y *= -1f;
					vector = BannerBuilderEditableAreaWidget.TransformToParent(vector, vec);
					vector.X *= -1f;
					vector.Y *= -1f;
					BannerBuilderEditableAreaWidget.BuilderMode currentMode = this._currentMode;
					if (currentMode != BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing)
					{
						if (currentMode == BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing)
						{
							vector.X = 0f;
						}
					}
					else
					{
						vector.Y = 0f;
					}
					vector *= (float)this.TotalAreaSize / base.Size.X * 2f;
					Vec2 vec2 = new Vec2(this._resizeStartSize.X, this._resizeStartSize.Y);
					vec2 += vector;
					vec2 = new Vector2((float)((int)MathF.Clamp((float)((int)vec2.X), 2f, (float)this._sizeLimitMax)), (float)((int)MathF.Clamp((float)((int)vec2.Y), 2f, (float)this._sizeLimitMax)));
					Vec2 vec3 = this._resizeStartSize - vec2;
					if (vec3.x != 0f || vec3.y != 0f)
					{
						this.BannerTableauWidget.UpdateSizeValueManual = vec2;
						this.SizeValue = vec2;
					}
				}
				if ((this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing || this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing) && base.EventManager.HoveredWidget == widgetFor)
				{
					this._resizeStartMousePosition = base.EventManager.MousePosition;
					this._resizeStartWidget = base.EventManager.HoveredWidget;
					this._resizeStartSize = this.SizeValue;
					this._currentMode = ((base.EventManager.HoveredWidget == this.GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType.Right)) ? BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing : BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing);
				}
			}
			else
			{
				this._resizeStartWidget = null;
				this._resizeStartSize = Vec2.Zero;
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
			this.UpdatePositionOfWidget(widgetFor, (resizeType == BannerBuilderEditableAreaWidget.EdgeResizeType.Right) ? BannerBuilderEditableAreaWidget.WidgetPlacementType.Horizontal : BannerBuilderEditableAreaWidget.WidgetPlacementType.Vertical, this.RotationValue + BannerBuilderEditableAreaWidget.AngleOffsetForEdge(resizeType), 15f, 0f);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00039C64 File Offset: 0x00037E64
		private void HandleForCorner()
		{
			ButtonWidget dragWidgetTopRight = this.DragWidgetTopRight;
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				bool flag = Input.IsKeyDown(InputKey.LeftShift) || Input.IsKeyDown(InputKey.RightShift);
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing)
				{
					Vector2 vector = base.EventManager.MousePosition - this._resizeStartMousePosition;
					vector.Y *= -1f;
					vector *= (float)this.TotalAreaSize / base.Size.X * 2f;
					Vec2 vec = BannerBuilderEditableAreaWidget.DirFromAngle(this.RotationValue);
					vec.y *= -1f;
					vector = BannerBuilderEditableAreaWidget.TransformToParent(vector, vec);
					vector.X *= -1f;
					vector.Y *= -1f;
					Vec2 vec2 = new Vec2(this._resizeStartSize.X, this._resizeStartSize.Y);
					if (flag)
					{
						Vector2 vector2 = new Vector2(this._centerOfSigil.X, this._centerOfSigil.Y) + base.GlobalPosition;
						float num = (vector2 - this._resizeStartMousePosition).Length();
						bool flag2 = (vector2 - base.EventManager.MousePosition).Length() < num;
						float num2 = vector.Length() * this._areaScale * (float)(flag2 ? (-1) : 1);
						float length = this._resizeStartSize.Length;
						float num3 = num2 / length;
						vec2 += num3 * vec2 * this._areaScale / 4f;
					}
					else
					{
						vec2 += vector;
					}
					vec2 = new Vector2((float)((int)MathF.Clamp((float)((int)vec2.X), 2f, (float)this._sizeLimitMax)), (float)((int)MathF.Clamp((float)((int)vec2.Y), 2f, (float)this._sizeLimitMax)));
					Vec2 vec3 = this._resizeStartSize - vec2;
					if (vec3.x != 0f || vec3.y != 0f)
					{
						this.BannerTableauWidget.UpdateSizeValueManual = vec2;
						this.SizeValue = vec2;
					}
				}
				if (this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing && base.EventManager.HoveredWidget == dragWidgetTopRight)
				{
					if (!flag || this._resizeStartWidget == null)
					{
						this._resizeStartMousePosition = base.EventManager.MousePosition;
						this._resizeStartWidget = base.EventManager.HoveredWidget;
						this._resizeStartSize = this.SizeValue;
					}
					this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing;
				}
			}
			else
			{
				this._resizeStartWidget = null;
				this._resizeStartSize = Vec2.Zero;
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
			this.UpdatePositionOfWidget(dragWidgetTopRight, BannerBuilderEditableAreaWidget.WidgetPlacementType.Max, this.RotationValue + BannerBuilderEditableAreaWidget.AngleOffsetForCorner(), 20f, 0f);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00039F28 File Offset: 0x00038128
		private void HandleCursor()
		{
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Rotating || base.EventManager.HoveredWidget == this.RotateWidget)
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.Rotate;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Positioning || base.EventManager.HoveredWidget == this)
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.Move;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing || base.EventManager.HoveredWidget == this.GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType.Right))
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.HorizontalResize;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing || base.EventManager.HoveredWidget == this.GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType.Top))
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.VerticalResize;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing || base.EventManager.HoveredWidget == this.DragWidgetTopRight)
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.DiagonalRightResize;
				return;
			}
			base.Context.ActiveCursorOfContext = UIContext.MouseCursors.Default;
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0003A00C File Offset: 0x0003820C
		private void UpdateEditableAreaVisual()
		{
			this.EditableAreaVisualWidget.HorizontalAlignment = HorizontalAlignment.Center;
			this.EditableAreaVisualWidget.VerticalAlignment = VerticalAlignment.Center;
			this.EditableAreaVisualWidget.WidthSizePolicy = SizePolicy.Fixed;
			this.EditableAreaVisualWidget.HeightSizePolicy = SizePolicy.Fixed;
			float num = (float)this.EditableAreaSize / (float)this.TotalAreaSize;
			this.EditableAreaVisualWidget.ScaledSuggestedWidth = base.Size.X * num;
			this.EditableAreaVisualWidget.ScaledSuggestedHeight = base.Size.Y * num;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0003A089 File Offset: 0x00038289
		private ButtonWidget GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType edgeResizeType)
		{
			if (edgeResizeType != BannerBuilderEditableAreaWidget.EdgeResizeType.Top)
			{
				return this.DragWidgetRight;
			}
			return this.DragWidgetTop;
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0003A09C File Offset: 0x0003829C
		private void UpdatePositionOfWidget(Widget widget, BannerBuilderEditableAreaWidget.WidgetPlacementType placementType, float directionFromCenter, float distanceFromCenterModifier, float distanceFromEdgesModifier = 0f)
		{
			Vec2 vec = BannerBuilderEditableAreaWidget.DirFromAngle(directionFromCenter);
			vec.y *= -1f;
			float num = 0f;
			switch (placementType)
			{
			case BannerBuilderEditableAreaWidget.WidgetPlacementType.Horizontal:
				num = this._sizeOfSigil.X;
				break;
			case BannerBuilderEditableAreaWidget.WidgetPlacementType.Vertical:
				num = this._sizeOfSigil.Y;
				break;
			case BannerBuilderEditableAreaWidget.WidgetPlacementType.Max:
				num = this._sizeOfSigil.Length;
				break;
			}
			float num2 = (num * base._inverseScaleToUse + distanceFromCenterModifier) * 0.5f * base._scaleToUse;
			Vec2 vec2 = this._centerOfSigil + vec * num2;
			vec2.x -= widget.Size.X / 2f;
			vec2.y -= widget.Size.Y / 2f;
			this.ApplyPositionOffsetToWidget(widget, vec2, distanceFromEdgesModifier * base._scaleToUse);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0003A178 File Offset: 0x00038378
		private void ApplyPositionOffsetToWidget(Widget widget, Vec2 pos, float additionalModifier = 0f)
		{
			widget.ScaledPositionXOffset = MathF.Clamp(pos.x, 12f + additionalModifier, base.Size.X - (12f + additionalModifier));
			widget.ScaledPositionYOffset = MathF.Clamp(pos.y, 12f + additionalModifier, base.Size.Y - (12f + additionalModifier));
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0003A1DB File Offset: 0x000383DB
		private void OnIsLayerPatternChanged(bool isLayerPattern)
		{
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0003A1DD File Offset: 0x000383DD
		private void OnPositionChanged(Vec2 newPosition)
		{
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0003A1DF File Offset: 0x000383DF
		private void OnSizeChanged(Vec2 newSize)
		{
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0003A1E1 File Offset: 0x000383E1
		private void OnRotationChanged(float newRotation)
		{
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0003A1E3 File Offset: 0x000383E3
		// (set) Token: 0x06001522 RID: 5410 RVA: 0x0003A1EB File Offset: 0x000383EB
		[Editor(false)]
		public bool IsLayerPattern
		{
			get
			{
				return this._isLayerPattern;
			}
			set
			{
				if (this._isLayerPattern != value)
				{
					this._isLayerPattern = value;
					base.OnPropertyChanged(value, "IsLayerPattern");
					this.OnIsLayerPatternChanged(value);
				}
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x0003A210 File Offset: 0x00038410
		// (set) Token: 0x06001524 RID: 5412 RVA: 0x0003A218 File Offset: 0x00038418
		[Editor(false)]
		public Vec2 PositionValue
		{
			get
			{
				return this._positionValue;
			}
			set
			{
				if (this._positionValue != value)
				{
					this._positionValue = value;
					base.OnPropertyChanged(value, "PositionValue");
					this.OnPositionChanged(value);
				}
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0003A242 File Offset: 0x00038442
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x0003A24A File Offset: 0x0003844A
		[Editor(false)]
		public Vec2 SizeValue
		{
			get
			{
				return this._sizeValue;
			}
			set
			{
				if (this._sizeValue != value)
				{
					this._sizeValue = value;
					base.OnPropertyChanged(value, "SizeValue");
					this.OnSizeChanged(value);
				}
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0003A274 File Offset: 0x00038474
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x0003A27C File Offset: 0x0003847C
		[Editor(false)]
		public float RotationValue
		{
			get
			{
				return this._rotationValue;
			}
			set
			{
				if (this._rotationValue != value)
				{
					this._rotationValue = value;
					base.OnPropertyChanged(value, "RotationValue");
					this.OnRotationChanged(value);
				}
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0003A2A1 File Offset: 0x000384A1
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x0003A2A9 File Offset: 0x000384A9
		[Editor(false)]
		public int EditableAreaSize
		{
			get
			{
				return this._editableAreaSize;
			}
			set
			{
				if (this._editableAreaSize != value)
				{
					this._editableAreaSize = value;
					base.OnPropertyChanged(value, "EditableAreaSize");
				}
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0003A2C7 File Offset: 0x000384C7
		// (set) Token: 0x0600152C RID: 5420 RVA: 0x0003A2CF File Offset: 0x000384CF
		[Editor(false)]
		public int TotalAreaSize
		{
			get
			{
				return this._totalAreaSize;
			}
			set
			{
				if (this._totalAreaSize != value)
				{
					this._totalAreaSize = value;
					base.OnPropertyChanged(value, "TotalAreaSize");
				}
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0003A2F0 File Offset: 0x000384F0
		private static Vec2 DirFromAngle(float angle)
		{
			float num = angle * 6.2831855f;
			return new Vec2(-MathF.Sin(num), MathF.Cos(num));
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0003A31C File Offset: 0x0003851C
		private static float AngleFromDir(Vec2 directionVector)
		{
			float num;
			if (directionVector.X < 0f)
			{
				num = (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f * -1f;
			}
			else
			{
				num = 360f - (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f;
			}
			return num / 360f;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0003A386 File Offset: 0x00038586
		private static float AngleOffsetForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType edge)
		{
			return 1f - (float)edge * 0.25f;
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0003A396 File Offset: 0x00038596
		private static float AngleOffsetForCorner()
		{
			return 0.875f;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0003A3A0 File Offset: 0x000385A0
		private static Vec2 TransformToParent(Vec2 a, Vec2 b)
		{
			return new Vec2(b.Y * a.X + b.X * a.Y, -b.X * a.X + b.Y * a.Y);
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0003A3F4 File Offset: 0x000385F4
		private static Vector2 TransformToParent(Vector2 a, Vec2 b)
		{
			return new Vector2(b.Y * a.X + b.X * a.Y, -b.X * a.X + b.Y * a.Y);
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0003A444 File Offset: 0x00038644
		private static Vector2 TransformToParent(Vec2 a, Vector2 b)
		{
			return new Vector2(b.Y * a.X + b.X * a.Y, -b.X * a.X + b.Y * a.Y);
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0003A491 File Offset: 0x00038691
		private static Vector2 TransformToParent(Vector2 a, Vector2 b)
		{
			return new Vector2(b.Y * a.X + b.X * a.Y, -b.X * a.Y + b.Y * a.Y);
		}

		// Token: 0x04000998 RID: 2456
		private bool _initialized;

		// Token: 0x04000999 RID: 2457
		private Vec2 _centerOfSigil;

		// Token: 0x0400099A RID: 2458
		private Vec2 _sizeOfSigil;

		// Token: 0x0400099B RID: 2459
		private float _positionLimitMin;

		// Token: 0x0400099C RID: 2460
		private float _positionLimitMax;

		// Token: 0x0400099D RID: 2461
		private float _areaScale;

		// Token: 0x0400099E RID: 2462
		private const int _sizeLimitMin = 2;

		// Token: 0x0400099F RID: 2463
		private int _sizeLimitMax;

		// Token: 0x040009A0 RID: 2464
		private BannerBuilderEditableAreaWidget.BuilderMode _currentMode;

		// Token: 0x040009A1 RID: 2465
		private Vector2 _latestMousePosition;

		// Token: 0x040009A2 RID: 2466
		private Vector2 _resizeStartMousePosition;

		// Token: 0x040009A3 RID: 2467
		private Widget _resizeStartWidget;

		// Token: 0x040009A4 RID: 2468
		private Vec2 _resizeStartSize;

		// Token: 0x040009A5 RID: 2469
		private bool _isLayerPattern;

		// Token: 0x040009A6 RID: 2470
		private Vec2 _positionValue;

		// Token: 0x040009A7 RID: 2471
		private Vec2 _sizeValue;

		// Token: 0x040009A8 RID: 2472
		private float _rotationValue;

		// Token: 0x040009A9 RID: 2473
		private int _editableAreaSize;

		// Token: 0x040009AA RID: 2474
		private int _totalAreaSize;

		// Token: 0x020001DA RID: 474
		private enum BuilderMode
		{
			// Token: 0x04000A8B RID: 2699
			None,
			// Token: 0x04000A8C RID: 2700
			Rotating,
			// Token: 0x04000A8D RID: 2701
			Positioning,
			// Token: 0x04000A8E RID: 2702
			HorizontalResizing,
			// Token: 0x04000A8F RID: 2703
			VerticalResizing,
			// Token: 0x04000A90 RID: 2704
			RightCornerResizing
		}

		// Token: 0x020001DB RID: 475
		private enum WidgetPlacementType
		{
			// Token: 0x04000A92 RID: 2706
			Horizontal,
			// Token: 0x04000A93 RID: 2707
			Vertical,
			// Token: 0x04000A94 RID: 2708
			Max
		}

		// Token: 0x020001DC RID: 476
		private enum EdgeResizeType
		{
			// Token: 0x04000A96 RID: 2710
			Top,
			// Token: 0x04000A97 RID: 2711
			Right
		}
	}
}
