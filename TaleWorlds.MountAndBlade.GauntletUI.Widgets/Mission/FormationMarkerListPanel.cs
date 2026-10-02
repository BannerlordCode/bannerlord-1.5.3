using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E1 RID: 225
	public class FormationMarkerListPanel : ListPanel
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0002042D File Offset: 0x0001E62D
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00020435 File Offset: 0x0001E635
		public float FarScaleTarget { get; set; } = 0.5f;

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0002043E File Offset: 0x0001E63E
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00020446 File Offset: 0x0001E646
		public float CloseScaleTarget { get; set; } = 1.4f;

		// Token: 0x06000B96 RID: 2966 RVA: 0x00020450 File Offset: 0x0001E650
		public FormationMarkerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000204E8 File Offset: 0x0001E6E8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = MathF.Clamp(dt * 12f, 0f, 1f);
			if (this._isMarkersDirty)
			{
				Sprite sprite = null;
				if (!string.IsNullOrEmpty(this.MarkerType) && this.IconBrush != null)
				{
					BrushLayer layer = this.IconBrush.GetLayer(this.MarkerType);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
				if (sprite != null && this.FormationTypeMarker != null)
				{
					this.FormationTypeMarker.Sprite = sprite;
				}
				else
				{
					Debug.FailedAssert("Couldn't find formation marker type image", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\FormationMarkerListPanel.cs", "OnLateUpdate", 55);
				}
				if (this.TeamTypeMarker != null)
				{
					this.TeamTypeMarker.RegisterBrushStatesOfWidget();
					if (this.TeamType == 0)
					{
						this.TeamTypeMarker.SetState("Player");
					}
					else if (this.TeamType == 1)
					{
						this.TeamTypeMarker.SetState("Ally");
					}
					else
					{
						this.TeamTypeMarker.SetState("Enemy");
					}
				}
				this._isMarkersDirty = false;
			}
			float num2;
			if (this.IsMarkerEnabled)
			{
				num2 = this.GetTargetAlpha(this.Distance);
				if (!this.IsActive)
				{
					num2 *= 0.5f;
				}
				if (!this._markerDefaultSize.IsValid)
				{
					this._markerDefaultSize = new Vec2(this.TeamTypeMarker.SuggestedWidth, this.TeamTypeMarker.SuggestedHeight);
				}
				float distanceRelatedScale = this.GetDistanceRelatedScale(this.Distance);
				this.TeamTypeMarker.SuggestedWidth = this._markerDefaultSize.X * distanceRelatedScale;
				this.TeamTypeMarker.SuggestedHeight = this._markerDefaultSize.Y * distanceRelatedScale;
			}
			else
			{
				num2 = 0f;
			}
			if (this._smoothedAlpha == -1f)
			{
				this._smoothedAlpha = num2;
			}
			else
			{
				this._smoothedAlpha = MathF.Lerp(this._smoothedAlpha, num2, num, 1E-05f);
			}
			this.SetGlobalAlphaRecursively(this._smoothedAlpha);
			this.UpdateScreenPosition();
			if (this._smoothedAlpha <= 0.05f)
			{
				base.IsVisible = false;
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x000206D0 File Offset: 0x0001E8D0
		private void UpdateScreenPosition()
		{
			float num = this.Position.X - base.Size.X / 2f;
			float num2 = this.Position.X + base.Size.X / 2f;
			float num3 = this.Position.Y - base.Size.Y / 2f;
			float num4 = this.Position.Y + base.Size.Y / 2f;
			bool flag = this.WSign > 0 && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
			bool flag2 = this.WSign > 0 && (num2 > 0f || num < base.Context.EventManager.PageSize.X) && (num4 > 0f || num3 < base.Context.EventManager.PageSize.Y);
			if (!flag && this.IsTargetingAFormation)
			{
				base.IsVisible = true;
				Vec2 vec = new Vec2(num, num3);
				Vector2 vector = base.Context.EventManager.PageSize - base.Size;
				Vec2 vec2 = vector / 2f;
				vec -= vec2;
				if (this.WSign < 0)
				{
					vec *= -1f;
				}
				float num5 = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
				float num6 = Mathf.Cos(num5);
				float num7 = Mathf.Sin(num5);
				float num8 = num6 / num7;
				Vec2 vec3 = vec2 * 1f;
				vec = ((num6 > 0f) ? new Vec2(-vec3.y / num8, vec2.y) : new Vec2(vec3.y / num8, -vec2.y));
				if (vec.x > vec3.x)
				{
					vec = new Vec2(vec3.x, -vec3.x * num8);
				}
				else if (vec.x < -vec3.x)
				{
					vec = new Vec2(-vec3.x, vec3.x * num8);
				}
				vec += vec2;
				base.ScaledPositionXOffset = Mathf.Clamp(vec.x, 0f, vector.X);
				base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, vector.Y);
				return;
			}
			if (flag || flag2)
			{
				base.IsVisible = true;
				base.ScaledPositionXOffset = num;
				base.ScaledPositionYOffset = num3;
				return;
			}
			base.IsVisible = false;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x000209B4 File Offset: 0x0001EBB4
		private float GetDistanceRelatedScale(float distance)
		{
			if (this.ShowDistanceTexts)
			{
				return 1f;
			}
			if (distance > this.FarDistanceCutoff)
			{
				return this.FarScaleTarget;
			}
			if (distance > this.FarDistanceCutoff || distance < this.CloseDistanceCutoff)
			{
				return this.CloseScaleTarget;
			}
			if (!this.HasUsableFadeRange())
			{
				return this.FarScaleTarget;
			}
			float num = (float)Math.Pow((double)((distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
			return MathF.Clamp(MathF.Lerp(this.CloseScaleTarget, this.FarScaleTarget, num, 1E-05f), this.FarScaleTarget, this.CloseScaleTarget);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00020A56 File Offset: 0x0001EC56
		private bool HasUsableFadeRange()
		{
			return this.FarDistanceCutoff - this.CloseDistanceCutoff > 0f;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00020A6C File Offset: 0x0001EC6C
		private float GetTargetAlpha(float distance)
		{
			if (this.VisibilityState == 0)
			{
				return 0f;
			}
			if (this.VisibilityState == 2)
			{
				return 1f;
			}
			if (this.VisibilityState == -1)
			{
				return this.GetLegacyDistanceAlpha(distance) * this._visibilityRatioTarget;
			}
			return this.GetDistanceRelatedAlphaTarget(distance);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00020AAC File Offset: 0x0001ECAC
		private float GetDistanceRelatedAlphaTarget(float distance)
		{
			if (distance <= this.AlwaysOnDistance)
			{
				return 1f;
			}
			if (distance >= this.FarDistanceCutoff)
			{
				return this.FarAlphaTarget;
			}
			float num = this.FarDistanceCutoff - this.AlwaysOnDistance;
			if (num <= 0f)
			{
				return this.FarAlphaTarget;
			}
			float num2 = (float)Math.Pow((double)((distance - this.AlwaysOnDistance) / num), 0.3333333333333333);
			return MathF.Clamp(MathF.Lerp(1f, this.FarAlphaTarget, num2, 1E-05f), this.FarAlphaTarget, 1f);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00020B38 File Offset: 0x0001ED38
		private float GetLegacyDistanceAlpha(float distance)
		{
			if (distance > this.FarDistanceCutoff)
			{
				return this.FarAlphaTarget;
			}
			if (distance >= this.CloseDistanceCutoff)
			{
				if (!this.HasUsableFadeRange())
				{
					return this.FarAlphaTarget;
				}
				float num = (float)Math.Pow((double)((distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
				return MathF.Clamp(MathF.Lerp(1f, this.FarAlphaTarget, num, 1E-05f), this.FarAlphaTarget, 1f);
			}
			else
			{
				if (distance > this.CloseDistanceCutoff - this.ClosestFadeoutRange)
				{
					float num2 = (distance - (this.CloseDistanceCutoff - this.ClosestFadeoutRange)) / this.ClosestFadeoutRange;
					return MathF.Lerp(0f, 1f, num2, 1E-05f);
				}
				return 0f;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00020BFD File Offset: 0x0001EDFD
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x00020C05 File Offset: 0x0001EE05
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

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00020C23 File Offset: 0x0001EE23
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x00020C2B File Offset: 0x0001EE2B
		[DataSourceProperty]
		public bool IsTargetingAFormation
		{
			get
			{
				return this._isTargetingAFormation;
			}
			set
			{
				if (this._isTargetingAFormation != value)
				{
					this._isTargetingAFormation = value;
					base.OnPropertyChanged(value, "IsTargetingAFormation");
				}
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00020C49 File Offset: 0x0001EE49
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x00020C51 File Offset: 0x0001EE51
		[DataSourceProperty]
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
				}
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00020C6F File Offset: 0x0001EE6F
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x00020C77 File Offset: 0x0001EE77
		[DataSourceProperty]
		public bool ShowDistanceTexts
		{
			get
			{
				return this._showDistanceTexts;
			}
			set
			{
				if (this._showDistanceTexts != value)
				{
					this._showDistanceTexts = value;
					base.OnPropertyChanged(value, "ShowDistanceTexts");
				}
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00020C95 File Offset: 0x0001EE95
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00020C9D File Offset: 0x0001EE9D
		[DataSourceProperty]
		public int TeamType
		{
			get
			{
				return this._teamType;
			}
			set
			{
				if (this._teamType != value)
				{
					this._teamType = value;
					base.OnPropertyChanged(value, "TeamType");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00020CC2 File Offset: 0x0001EEC2
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00020CCA File Offset: 0x0001EECA
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (this._wSign != value)
				{
					this._wSign = value;
					base.OnPropertyChanged(value, "WSign");
				}
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00020CE8 File Offset: 0x0001EEE8
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x00020CF0 File Offset: 0x0001EEF0
		[DataSourceProperty]
		public float Distance
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

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00020D0E File Offset: 0x0001EF0E
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x00020D16 File Offset: 0x0001EF16
		[DataSourceProperty]
		public float FarAlphaTarget
		{
			get
			{
				return this._farAlphaTarget;
			}
			set
			{
				if (this._farAlphaTarget != value)
				{
					this._farAlphaTarget = value;
					base.OnPropertyChanged(value, "FarAlphaTarget");
				}
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00020D34 File Offset: 0x0001EF34
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x00020D3C File Offset: 0x0001EF3C
		[DataSourceProperty]
		public float FarDistanceCutoff
		{
			get
			{
				return this._farDistanceCutoff;
			}
			set
			{
				if (this._farDistanceCutoff != value)
				{
					this._farDistanceCutoff = value;
					base.OnPropertyChanged(value, "FarDistanceCutoff");
				}
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00020D5A File Offset: 0x0001EF5A
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x00020D62 File Offset: 0x0001EF62
		[DataSourceProperty]
		public float CloseDistanceCutoff
		{
			get
			{
				return this._closeDistanceCutoff;
			}
			set
			{
				if (this._closeDistanceCutoff != value)
				{
					this._closeDistanceCutoff = value;
					base.OnPropertyChanged(value, "CloseDistanceCutoff");
				}
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00020D80 File Offset: 0x0001EF80
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x00020D88 File Offset: 0x0001EF88
		[DataSourceProperty]
		public float ClosestFadeoutRange
		{
			get
			{
				return this._closestFadeoutRange;
			}
			set
			{
				if (this._closestFadeoutRange != value)
				{
					this._closestFadeoutRange = value;
					base.OnPropertyChanged(value, "ClosestFadeoutRange");
				}
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00020DA6 File Offset: 0x0001EFA6
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x00020DB0 File Offset: 0x0001EFB0
		[DataSourceProperty]
		public float VisibilityRatio
		{
			get
			{
				return this._visibilityRatioTarget;
			}
			set
			{
				float num = MathF.Clamp(value, 0f, 1f);
				if (!this._visibilityRatioTarget.ApproximatelyEqualsTo(num, 1E-05f))
				{
					this._visibilityRatioTarget = num;
					base.OnPropertyChanged(value, "VisibilityRatio");
				}
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x00020DF4 File Offset: 0x0001EFF4
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x00020DFC File Offset: 0x0001EFFC
		[DataSourceProperty]
		public float AlwaysOnDistance
		{
			get
			{
				return this._alwaysOnDistance;
			}
			set
			{
				if (!this._alwaysOnDistance.ApproximatelyEqualsTo(value, 1E-05f))
				{
					this._alwaysOnDistance = value;
					base.OnPropertyChanged(value, "AlwaysOnDistance");
				}
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x00020E24 File Offset: 0x0001F024
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x00020E2C File Offset: 0x0001F02C
		[DataSourceProperty]
		public int VisibilityState
		{
			get
			{
				return this._visibilityState;
			}
			set
			{
				if (value != this._visibilityState)
				{
					this._visibilityState = value;
					base.OnPropertyChanged(value, "VisibilityState");
				}
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00020E4A File Offset: 0x0001F04A
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x00020E52 File Offset: 0x0001F052
		[DataSourceProperty]
		public string MarkerType
		{
			get
			{
				return this._markerType;
			}
			set
			{
				if (this._markerType != value)
				{
					this._markerType = value;
					base.OnPropertyChanged<string>(value, "MarkerType");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00020E7C File Offset: 0x0001F07C
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x00020E84 File Offset: 0x0001F084
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

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00020EA7 File Offset: 0x0001F0A7
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x00020EAF File Offset: 0x0001F0AF
		[DataSourceProperty]
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (this._iconBrush != value)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
				}
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00020ECD File Offset: 0x0001F0CD
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x00020ED5 File Offset: 0x0001F0D5
		[DataSourceProperty]
		public Widget FormationTypeMarker
		{
			get
			{
				return this._formationTypeMarker;
			}
			set
			{
				if (this._formationTypeMarker != value)
				{
					this._formationTypeMarker = value;
					base.OnPropertyChanged<Widget>(value, "FormationTypeMarker");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00020EFA File Offset: 0x0001F0FA
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x00020F02 File Offset: 0x0001F102
		[DataSourceProperty]
		public Widget TeamTypeMarker
		{
			get
			{
				return this._teamTypeMarker;
			}
			set
			{
				if (this._teamTypeMarker != value)
				{
					this._teamTypeMarker = value;
					base.OnPropertyChanged<Widget>(value, "TeamTypeMarker");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00020F27 File Offset: 0x0001F127
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00020F2F File Offset: 0x0001F12F
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
				}
			}
		}

		// Token: 0x04000536 RID: 1334
		public const int VisibilityStateNeutral = -1;

		// Token: 0x04000537 RID: 1335
		public const int VisibilityStateHidden = 0;

		// Token: 0x04000538 RID: 1336
		public const int VisibilityStateDistanceScaled = 1;

		// Token: 0x04000539 RID: 1337
		public const int VisibilityStateAlwaysVisible = 2;

		// Token: 0x0400053C RID: 1340
		private bool _isMarkersDirty = true;

		// Token: 0x0400053D RID: 1341
		private Vec2 _markerDefaultSize = Vec2.Invalid;

		// Token: 0x0400053E RID: 1342
		private const float MinimumVisibleAlpha = 0.05f;

		// Token: 0x0400053F RID: 1343
		private const float UnevaluatedAlpha = -1f;

		// Token: 0x04000540 RID: 1344
		private bool _isMarkerEnabled;

		// Token: 0x04000541 RID: 1345
		private bool _isTargetingAFormation;

		// Token: 0x04000542 RID: 1346
		public bool _showDistanceTexts;

		// Token: 0x04000543 RID: 1347
		private bool _isActive = true;

		// Token: 0x04000544 RID: 1348
		private int _teamType;

		// Token: 0x04000545 RID: 1349
		private int _wSign;

		// Token: 0x04000546 RID: 1350
		private float _distance;

		// Token: 0x04000547 RID: 1351
		private float _farAlphaTarget = 0.2f;

		// Token: 0x04000548 RID: 1352
		private float _farDistanceCutoff = 50f;

		// Token: 0x04000549 RID: 1353
		private float _closeDistanceCutoff = 25f;

		// Token: 0x0400054A RID: 1354
		private float _closestFadeoutRange = 3f;

		// Token: 0x0400054B RID: 1355
		private float _visibilityRatioTarget = 1f;

		// Token: 0x0400054C RID: 1356
		private float _alwaysOnDistance = 25f;

		// Token: 0x0400054D RID: 1357
		private int _visibilityState = -1;

		// Token: 0x0400054E RID: 1358
		private string _markerType;

		// Token: 0x0400054F RID: 1359
		private Vec2 _position;

		// Token: 0x04000550 RID: 1360
		private Brush _iconBrush;

		// Token: 0x04000551 RID: 1361
		private Widget _formationTypeMarker;

		// Token: 0x04000552 RID: 1362
		private Widget _teamTypeMarker;

		// Token: 0x04000553 RID: 1363
		private TextWidget _nameTextWidget;

		// Token: 0x04000554 RID: 1364
		private float _smoothedAlpha = -1f;
	}
}
