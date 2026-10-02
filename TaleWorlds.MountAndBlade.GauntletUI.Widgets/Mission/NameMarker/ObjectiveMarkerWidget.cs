using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000FB RID: 251
	public class ObjectiveMarkerWidget : Widget
	{
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x00024483 File Offset: 0x00022683
		public bool IsCombinedWithOtherMarkers
		{
			get
			{
				return this.CombinedSiblingsCount > 0;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0002448E File Offset: 0x0002268E
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00024496 File Offset: 0x00022696
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x0002449F File Offset: 0x0002269F
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x000244A7 File Offset: 0x000226A7
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x000244B0 File Offset: 0x000226B0
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x000244B8 File Offset: 0x000226B8
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x000244C1 File Offset: 0x000226C1
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x000244C9 File Offset: 0x000226C9
		public MarkerRect Rect { get; private set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x000244D2 File Offset: 0x000226D2
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x000244DA File Offset: 0x000226DA
		public bool IsInScreenBoundaries { get; private set; }

		// Token: 0x06000D35 RID: 3381 RVA: 0x000244E3 File Offset: 0x000226E3
		public ObjectiveMarkerWidget(UIContext context)
			: base(context)
		{
			this.Rect = new MarkerRect();
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00024518 File Offset: 0x00022718
		public void Update(float dt)
		{
			float transitionRatio = MathF.Clamp(dt * 12f, 0f, 1f);
			float num = ((this.IsMarkerEnabled && this.IsMarkerActive) ? this.GetDistanceRelatedAlphaTarget(this.Distance) : 0f);
			float distanceAlpha = ((this.IsFocused && (!this.IsCombinedWithOtherMarkers || this.IsMainCombinationMarker)) ? num : 0f);
			float num2 = ((this.IsFocused && !this.IsCombinedWithOtherMarkers) ? num : 0f);
			this.DistanceContainerWidget.ApplyActionForThisAndAllChildren(delegate(Widget w)
			{
				ObjectiveMarkerWidget.UpdateAlpha(w, distanceAlpha, transitionRatio);
			});
			ObjectiveMarkerWidget.UpdateAlpha(this.NameTextWidget, num2, transitionRatio);
			ObjectiveMarkerWidget.UpdateAlpha(this.QuestIconWidget, num, transitionRatio);
			ObjectiveMarkerWidget.UpdateAlpha(this.CombinationCountWidget, num, transitionRatio);
			base.ScaledPositionYOffset = this.Position.Y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.X - base.Size.X / 2f;
			this.CombinationCountWidget.Text = (this.IsMainCombinationMarker ? (this.CombinedSiblingsCount + 1).ToString() : string.Empty);
			Vec2 vec = (this.IsCombinedWithOtherMarkers ? (this.CombinedAveragePosition - this.Position) : Vec2.Zero);
			this.MainContainer.ScaledPositionXOffset = MathF.Lerp(this.MainContainer.ScaledPositionXOffset, vec.x, transitionRatio, 1E-05f);
			this.MainContainer.ScaledPositionYOffset = MathF.Lerp(this.MainContainer.ScaledPositionYOffset, vec.y, transitionRatio, 1E-05f);
			this.UpdateRectangle();
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000246F0 File Offset: 0x000228F0
		private static void UpdateAlpha(Widget item, float targetAlpha, float transitionRatio)
		{
			if (item == null)
			{
				return;
			}
			float num = ObjectiveMarkerWidget.LocalLerp(item.AlphaFactor, targetAlpha, transitionRatio);
			item.SetAlpha(num);
			item.IsVisible = num > 1E-05f;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00024724 File Offset: 0x00022924
		public void UpdateRectangle()
		{
			this.Rect.Reset();
			this.Rect.UpdatePoints(base.ScaledPositionXOffset, base.ScaledPositionXOffset + base.Size.X, base.ScaledPositionYOffset, base.ScaledPositionYOffset + base.Size.Y);
			this.IsInScreenBoundaries = this.Rect.Left > -50f && this.Rect.Right < base.EventManager.PageSize.X + 50f && this.Rect.Top > -50f && this.Rect.Bottom < base.EventManager.PageSize.Y + 50f;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x000247EC File Offset: 0x000229EC
		private float GetDistanceRelatedAlphaTarget(int distance)
		{
			if (this.IsCombinedWithOtherMarkers)
			{
				if (this.IsMainCombinationMarker)
				{
					return 1f;
				}
				if ((this.CombinedAveragePosition - this.Position).Distance(new Vec2(this.MainContainer.ScaledPositionXOffset, this.MainContainer.ScaledPositionYOffset)) <= 5f)
				{
					return 0f;
				}
				return 1f;
			}
			else
			{
				if (this.IsFocused)
				{
					return 1f;
				}
				if ((float)distance > this.FarDistanceCutoff)
				{
					return this.FarAlphaTarget;
				}
				if ((float)distance <= this.FarDistanceCutoff && (float)distance >= this.CloseDistanceCutoff)
				{
					float num = (float)Math.Pow((double)(((float)distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
					return MathF.Clamp(MathF.Lerp(1f, this.FarAlphaTarget, num, 1E-05f), this.FarAlphaTarget, 1f);
				}
				return 1f;
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000248DF File Offset: 0x00022ADF
		private static float LocalLerp(float start, float end, float delta)
		{
			if (Math.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000248F9 File Offset: 0x00022AF9
		private void OnStateChanged()
		{
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x000248FB File Offset: 0x00022AFB
		// (set) Token: 0x06000D3D RID: 3389 RVA: 0x00024903 File Offset: 0x00022B03
		[DataSourceProperty]
		public TextWidget NameTextWidget
		{
			get
			{
				return this._nameTextWidget;
			}
			set
			{
				if (this._nameTextWidget != value)
				{
					this._nameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameTextWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00024927 File Offset: 0x00022B27
		// (set) Token: 0x06000D3F RID: 3391 RVA: 0x0002492F File Offset: 0x00022B2F
		[DataSourceProperty]
		public TextWidget CombinationCountWidget
		{
			get
			{
				return this._combinationCountWidget;
			}
			set
			{
				if (this._combinationCountWidget != value)
				{
					this._combinationCountWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "CombinationCountWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00024953 File Offset: 0x00022B53
		// (set) Token: 0x06000D41 RID: 3393 RVA: 0x0002495B File Offset: 0x00022B5B
		[DataSourceProperty]
		public Widget QuestIconWidget
		{
			get
			{
				return this._questIconWidget;
			}
			set
			{
				if (this._questIconWidget != value)
				{
					this._questIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "QuestIconWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0002497F File Offset: 0x00022B7F
		// (set) Token: 0x06000D43 RID: 3395 RVA: 0x00024987 File Offset: 0x00022B87
		[DataSourceProperty]
		public Widget MainContainer
		{
			get
			{
				return this._mainContainer;
			}
			set
			{
				if (this._mainContainer != value)
				{
					this._mainContainer = value;
					base.OnPropertyChanged<Widget>(value, "MainContainer");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x000249AB File Offset: 0x00022BAB
		// (set) Token: 0x06000D45 RID: 3397 RVA: 0x000249B3 File Offset: 0x00022BB3
		[DataSourceProperty]
		public Widget DistanceContainerWidget
		{
			get
			{
				return this._distanceContainerWidget;
			}
			set
			{
				if (this._distanceContainerWidget != value)
				{
					this._distanceContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "DistanceContainerWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x000249D7 File Offset: 0x00022BD7
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x000249DF File Offset: 0x00022BDF
		[DataSourceProperty]
		public Widget DistanceIconWidget
		{
			get
			{
				return this._distanceIconWidget;
			}
			set
			{
				if (this._distanceIconWidget != value)
				{
					this._distanceIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "DistanceIconWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00024A03 File Offset: 0x00022C03
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x00024A0B File Offset: 0x00022C0B
		[DataSourceProperty]
		public Widget DistanceTextWidget
		{
			get
			{
				return this._distanceTextWidget;
			}
			set
			{
				if (this._distanceTextWidget != value)
				{
					this._distanceTextWidget = value;
					base.OnPropertyChanged<Widget>(value, "DistanceTextWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00024A2F File Offset: 0x00022C2F
		// (set) Token: 0x06000D4B RID: 3403 RVA: 0x00024A37 File Offset: 0x00022C37
		[DataSourceProperty]
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

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00024A5A File Offset: 0x00022C5A
		// (set) Token: 0x06000D4D RID: 3405 RVA: 0x00024A62 File Offset: 0x00022C62
		[DataSourceProperty]
		public Vec2 CombinedAveragePosition
		{
			get
			{
				return this._combinedAveragePosition;
			}
			set
			{
				if (this._combinedAveragePosition != value)
				{
					this._combinedAveragePosition = value;
					base.OnPropertyChanged(value, "CombinedAveragePosition");
				}
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00024A85 File Offset: 0x00022C85
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x00024A8D File Offset: 0x00022C8D
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value)
				{
					this._distance = value;
					base.OnPropertyChanged(value, "Distance");
				}
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00024AAB File Offset: 0x00022CAB
		// (set) Token: 0x06000D51 RID: 3409 RVA: 0x00024AB3 File Offset: 0x00022CB3
		[DataSourceProperty]
		public int CombinedSiblingsCount
		{
			get
			{
				return this._combinedSiblingsCount;
			}
			set
			{
				if (this._combinedSiblingsCount != value)
				{
					this._combinedSiblingsCount = value;
					base.OnPropertyChanged(value, "CombinedSiblingsCount");
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00024AD1 File Offset: 0x00022CD1
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x00024AD9 File Offset: 0x00022CD9
		[DataSourceProperty]
		public bool IsMainCombinationMarker
		{
			get
			{
				return this._isMainCombinationMarker;
			}
			set
			{
				if (this._isMainCombinationMarker != value)
				{
					this._isMainCombinationMarker = value;
					base.OnPropertyChanged(value, "IsMainCombinationMarker");
				}
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00024AF7 File Offset: 0x00022CF7
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x00024AFF File Offset: 0x00022CFF
		[DataSourceProperty]
		public bool IsDistanceRelevant
		{
			get
			{
				return this._isDistanceRelevant;
			}
			set
			{
				if (this._isDistanceRelevant != value)
				{
					this._isDistanceRelevant = value;
					base.OnPropertyChanged(value, "IsDistanceRelevant");
				}
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00024B1D File Offset: 0x00022D1D
		// (set) Token: 0x06000D57 RID: 3415 RVA: 0x00024B25 File Offset: 0x00022D25
		[DataSourceProperty]
		public bool IsMarkerEnabled
		{
			get
			{
				return this._isMarkerEnabled;
			}
			set
			{
				if (this._isMarkerEnabled != value)
				{
					this._isMarkerEnabled = value;
					base.OnPropertyChanged(value, "IsMarkerEnabled");
				}
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00024B43 File Offset: 0x00022D43
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x00024B4B File Offset: 0x00022D4B
		[DataSourceProperty]
		public bool IsMarkerActive
		{
			get
			{
				return this._isMarkerActive;
			}
			set
			{
				if (this._isMarkerActive != value)
				{
					this._isMarkerActive = value;
					base.OnPropertyChanged(value, "IsMarkerActive");
				}
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00024B69 File Offset: 0x00022D69
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00024B71 File Offset: 0x00022D71
		[Editor(false)]
		public new bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChanged(value, "IsFocused");
					base.RenderLate = value;
				}
			}
		}

		// Token: 0x040005FF RID: 1535
		private const float BoundaryOffset = 50f;

		// Token: 0x04000600 RID: 1536
		private int _distance;

		// Token: 0x04000601 RID: 1537
		private int _combinedSiblingsCount;

		// Token: 0x04000602 RID: 1538
		private TextWidget _nameTextWidget;

		// Token: 0x04000603 RID: 1539
		private TextWidget _combinationCountWidget;

		// Token: 0x04000604 RID: 1540
		private Widget _mainContainer;

		// Token: 0x04000605 RID: 1541
		private Widget _questIconWidget;

		// Token: 0x04000606 RID: 1542
		private Widget _distanceContainerWidget;

		// Token: 0x04000607 RID: 1543
		private Widget _distanceIconWidget;

		// Token: 0x04000608 RID: 1544
		private Widget _distanceTextWidget;

		// Token: 0x04000609 RID: 1545
		private Vec2 _position;

		// Token: 0x0400060A RID: 1546
		private Vec2 _combinedAveragePosition;

		// Token: 0x0400060B RID: 1547
		private bool _isMainCombinationMarker;

		// Token: 0x0400060C RID: 1548
		private bool _isDistanceRelevant;

		// Token: 0x0400060D RID: 1549
		private bool _isMarkerEnabled;

		// Token: 0x0400060E RID: 1550
		private bool _isMarkerActive;

		// Token: 0x0400060F RID: 1551
		private bool _isFocused;
	}
}
