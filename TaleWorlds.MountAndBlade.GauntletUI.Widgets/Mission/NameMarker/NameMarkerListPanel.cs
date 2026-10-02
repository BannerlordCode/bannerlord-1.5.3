using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F7 RID: 247
	public class NameMarkerListPanel : ListPanel
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x000230DD File Offset: 0x000212DD
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x000230E5 File Offset: 0x000212E5
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x000230EE File Offset: 0x000212EE
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x000230F6 File Offset: 0x000212F6
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x000230FF File Offset: 0x000212FF
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00023107 File Offset: 0x00021307
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00023110 File Offset: 0x00021310
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00023118 File Offset: 0x00021318
		public bool HasTypeMarker { get; private set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00023121 File Offset: 0x00021321
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x00023129 File Offset: 0x00021329
		public MarkerRect Rect { get; private set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00023132 File Offset: 0x00021332
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0002313A File Offset: 0x0002133A
		public bool IsInScreenBoundaries { get; private set; }

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00023144 File Offset: 0x00021344
		public NameMarkerListPanel(UIContext context)
			: base(context)
		{
			this._parentScreenWidget = base.EventManager.Root.GetChild(0).GetChild(0);
			this.Rect = new MarkerRect();
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000231B8 File Offset: 0x000213B8
		public void Update(float dt)
		{
			this._transitionDT = MathF.Clamp(dt * 12f, 0f, 1f);
			this._targetAlpha = ((this.IsMarkerEnabled || this.IsMarkerPersistent) ? this.GetDistanceRelatedAlphaTarget(this.Distance) : 0f);
			this.ApplyActionForThisAndAllChildren(new Action<Widget>(this.UpdateAlpha));
			TextWidget nameTextWidget = this.NameTextWidget;
			if ((nameTextWidget != null && nameTextWidget.IsVisible) || this.TypeVisualWidget.IsVisible)
			{
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			}
			this.UpdateRectangle();
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002328C File Offset: 0x0002148C
		private void UpdateAlpha(Widget item)
		{
			if ((item == this.NameTextWidget || item == this.DistanceTextWidget || item == this.DistanceIconWidget) && this.HasTypeMarker && !this.IsFocused)
			{
				return;
			}
			float num = this.LocalLerp(item.AlphaFactor, this._targetAlpha, this._transitionDT);
			item.SetAlpha(num);
			item.IsVisible = (double)item.AlphaFactor > 0.05;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00023300 File Offset: 0x00021500
		public void UpdateRectangle()
		{
			this.Rect.Reset();
			this.Rect.UpdatePoints(base.ScaledPositionXOffset, base.ScaledPositionXOffset + base.Size.X, base.ScaledPositionYOffset, base.ScaledPositionYOffset + base.Size.Y);
			this.IsInScreenBoundaries = this.Rect.Left > -50f && this.Rect.Right < base.EventManager.PageSize.X + 50f && this.Rect.Top > -50f && this.Rect.Bottom < base.EventManager.PageSize.Y + 50f;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000233C8 File Offset: 0x000215C8
		private float GetDistanceRelatedAlphaTarget(int distance)
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

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0002345C File Offset: 0x0002165C
		private float LocalLerp(float start, float end, float delta)
		{
			if (Math.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00023478 File Offset: 0x00021678
		private void OnStateChanged()
		{
			if (this.NameTextWidget != null)
			{
				this.NameTextWidget.SetState(this.NameType);
			}
			if (this.TypeVisualWidget != null)
			{
				this.TypeVisualWidget.SetState(this.IconType);
			}
			this.HasTypeMarker = this.IconType != string.Empty;
			if (this.HasTypeMarker && this.IsFocused)
			{
				TextWidget nameTextWidget = this.NameTextWidget;
				if (nameTextWidget != null)
				{
					nameTextWidget.SetAlpha(1f);
				}
				TextWidget distanceTextWidget = this.DistanceTextWidget;
				if (distanceTextWidget != null)
				{
					distanceTextWidget.SetAlpha(1f);
				}
				BrushWidget distanceIconWidget = this.DistanceIconWidget;
				if (distanceIconWidget != null)
				{
					distanceIconWidget.SetAlpha(1f);
				}
			}
			else if (this.HasTypeMarker && !this.IsFocused)
			{
				TextWidget nameTextWidget2 = this.NameTextWidget;
				if (nameTextWidget2 != null)
				{
					nameTextWidget2.SetAlpha(0f);
				}
				TextWidget distanceTextWidget2 = this.DistanceTextWidget;
				if (distanceTextWidget2 != null)
				{
					distanceTextWidget2.SetAlpha(0f);
				}
				BrushWidget distanceIconWidget2 = this.DistanceIconWidget;
				if (distanceIconWidget2 != null)
				{
					distanceIconWidget2.SetAlpha(0f);
				}
			}
			if (this.IsEnemy)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.EnemyColor;
			}
			else if (this.IsFriendly)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.FriendlyColor;
			}
			else if (this.HasMainQuest)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.MainQuestNotificationColor;
			}
			else if (this.HasIssue)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.IssueNotificationColor;
			}
			BrushWidget typeVisualWidget = this.TypeVisualWidget;
			Sprite sprite;
			if (typeVisualWidget == null)
			{
				sprite = null;
			}
			else
			{
				Style style = typeVisualWidget.Brush.GetStyle(this.IconType);
				if (style == null)
				{
					sprite = null;
				}
				else
				{
					StyleLayer layer = style.GetLayer(0);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
			}
			Sprite sprite2 = sprite;
			if (sprite2 != null)
			{
				base.SuggestedWidth = base.SuggestedHeight / (float)sprite2.Height * (float)sprite2.Width;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00023647 File Offset: 0x00021847
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x0002364F File Offset: 0x0002184F
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

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00023673 File Offset: 0x00021873
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x0002367B File Offset: 0x0002187B
		[DataSourceProperty]
		public BrushWidget TypeVisualWidget
		{
			get
			{
				return this._typeVisualWidget;
			}
			set
			{
				if (this._typeVisualWidget != value)
				{
					this._typeVisualWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "TypeVisualWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0002369F File Offset: 0x0002189F
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x000236A7 File Offset: 0x000218A7
		[DataSourceProperty]
		public BrushWidget DistanceIconWidget
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
					base.OnPropertyChanged<BrushWidget>(value, "DistanceIconWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x000236CB File Offset: 0x000218CB
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x000236D3 File Offset: 0x000218D3
		[DataSourceProperty]
		public TextWidget DistanceTextWidget
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
					base.OnPropertyChanged<TextWidget>(value, "DistanceTextWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000236F7 File Offset: 0x000218F7
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x000236FF File Offset: 0x000218FF
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

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00023722 File Offset: 0x00021922
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0002372A File Offset: 0x0002192A
		[Editor(false)]
		public Color IssueNotificationColor
		{
			get
			{
				return this._issueNotificationColor;
			}
			set
			{
				if (value != this._issueNotificationColor)
				{
					this._issueNotificationColor = value;
					base.OnPropertyChanged(value, "IssueNotificationColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00023753 File Offset: 0x00021953
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x0002375B File Offset: 0x0002195B
		[Editor(false)]
		public Color MainQuestNotificationColor
		{
			get
			{
				return this._mainQuestNotificationColor;
			}
			set
			{
				if (value != this._mainQuestNotificationColor)
				{
					this._mainQuestNotificationColor = value;
					base.OnPropertyChanged(value, "MainQuestNotificationColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00023784 File Offset: 0x00021984
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0002378C File Offset: 0x0002198C
		[Editor(false)]
		public Color EnemyColor
		{
			get
			{
				return this._enemyColor;
			}
			set
			{
				if (value != this._enemyColor)
				{
					this._enemyColor = value;
					base.OnPropertyChanged(value, "EnemyColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x000237B5 File Offset: 0x000219B5
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x000237BD File Offset: 0x000219BD
		[Editor(false)]
		public Color FriendlyColor
		{
			get
			{
				return this._friendlyColor;
			}
			set
			{
				if (value != this._friendlyColor)
				{
					this._friendlyColor = value;
					base.OnPropertyChanged(value, "FriendlyColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x000237E6 File Offset: 0x000219E6
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x000237EE File Offset: 0x000219EE
		[Editor(false)]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChanged<string>(value, "IconType");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00023817 File Offset: 0x00021A17
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0002381F File Offset: 0x00021A1F
		[Editor(false)]
		public string NameType
		{
			get
			{
				return this._nameType;
			}
			set
			{
				if (value != this._nameType)
				{
					this._nameType = value;
					base.OnPropertyChanged<string>(value, "NameType");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00023848 File Offset: 0x00021A48
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00023850 File Offset: 0x00021A50
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

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0002386E File Offset: 0x00021A6E
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00023876 File Offset: 0x00021A76
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

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00023894 File Offset: 0x00021A94
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x0002389C File Offset: 0x00021A9C
		[DataSourceProperty]
		public bool IsMarkerPersistent
		{
			get
			{
				return this._isMarkerPersistent;
			}
			set
			{
				if (this._isMarkerPersistent != value)
				{
					this._isMarkerPersistent = value;
					base.OnPropertyChanged(value, "IsMarkerPersistent");
				}
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x000238BA File Offset: 0x00021ABA
		// (set) Token: 0x06000CF7 RID: 3319 RVA: 0x000238C2 File Offset: 0x00021AC2
		[DataSourceProperty]
		public bool HasIssue
		{
			get
			{
				return this._hasIssue;
			}
			set
			{
				if (this._hasIssue != value)
				{
					this._hasIssue = value;
					base.OnPropertyChanged(value, "HasIssue");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000238E6 File Offset: 0x00021AE6
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x000238EE File Offset: 0x00021AEE
		[DataSourceProperty]
		public bool HasMainQuest
		{
			get
			{
				return this._hasMainQuest;
			}
			set
			{
				if (this._hasMainQuest != value)
				{
					this._hasMainQuest = value;
					base.OnPropertyChanged(value, "HasMainQuest");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00023912 File Offset: 0x00021B12
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x0002391A File Offset: 0x00021B1A
		[DataSourceProperty]
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
					base.OnPropertyChanged(value, "IsEnemy");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0002393E File Offset: 0x00021B3E
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x00023946 File Offset: 0x00021B46
		[DataSourceProperty]
		public bool IsFriendly
		{
			get
			{
				return this._isFriendly;
			}
			set
			{
				if (this._isFriendly != value)
				{
					this._isFriendly = value;
					base.OnPropertyChanged(value, "IsFriendly");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0002396A File Offset: 0x00021B6A
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00023974 File Offset: 0x00021B74
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
					if (!value && (this.IsMarkerEnabled || this.IsMarkerPersistent))
					{
						TextWidget nameTextWidget = this.NameTextWidget;
						if (nameTextWidget != null)
						{
							nameTextWidget.SetAlpha(0f);
						}
						TextWidget distanceTextWidget = this.DistanceTextWidget;
						if (distanceTextWidget != null)
						{
							distanceTextWidget.SetAlpha(0f);
						}
						BrushWidget distanceIconWidget = this.DistanceIconWidget;
						if (distanceIconWidget != null)
						{
							distanceIconWidget.SetAlpha(0f);
						}
					}
					else if (value && (this.IsMarkerEnabled || this.IsMarkerPersistent))
					{
						TextWidget nameTextWidget2 = this.NameTextWidget;
						if (nameTextWidget2 != null)
						{
							nameTextWidget2.SetAlpha(1f);
						}
						TextWidget distanceTextWidget2 = this.DistanceTextWidget;
						if (distanceTextWidget2 != null)
						{
							distanceTextWidget2.SetAlpha(1f);
						}
						BrushWidget distanceIconWidget2 = this.DistanceIconWidget;
						if (distanceIconWidget2 != null)
						{
							distanceIconWidget2.SetAlpha(1f);
						}
					}
					base.RenderLate = value;
				}
			}
		}

		// Token: 0x040005CF RID: 1487
		private Widget _parentScreenWidget;

		// Token: 0x040005D3 RID: 1491
		private const float BoundaryOffset = 50f;

		// Token: 0x040005D4 RID: 1492
		private float _transitionDT;

		// Token: 0x040005D5 RID: 1493
		private float _targetAlpha;

		// Token: 0x040005D6 RID: 1494
		private string _iconType = string.Empty;

		// Token: 0x040005D7 RID: 1495
		private string _nameType = string.Empty;

		// Token: 0x040005D8 RID: 1496
		private int _distance;

		// Token: 0x040005D9 RID: 1497
		private TextWidget _nameTextWidget;

		// Token: 0x040005DA RID: 1498
		private BrushWidget _typeVisualWidget;

		// Token: 0x040005DB RID: 1499
		private BrushWidget _distanceIconWidget;

		// Token: 0x040005DC RID: 1500
		private TextWidget _distanceTextWidget;

		// Token: 0x040005DD RID: 1501
		private Vec2 _position;

		// Token: 0x040005DE RID: 1502
		private Color _issueNotificationColor;

		// Token: 0x040005DF RID: 1503
		private Color _mainQuestNotificationColor;

		// Token: 0x040005E0 RID: 1504
		private Color _enemyColor;

		// Token: 0x040005E1 RID: 1505
		private Color _friendlyColor;

		// Token: 0x040005E2 RID: 1506
		private bool _isMarkerEnabled;

		// Token: 0x040005E3 RID: 1507
		private bool _isMarkerPersistent;

		// Token: 0x040005E4 RID: 1508
		private bool _hasIssue;

		// Token: 0x040005E5 RID: 1509
		private bool _hasMainQuest;

		// Token: 0x040005E6 RID: 1510
		private bool _isEnemy;

		// Token: 0x040005E7 RID: 1511
		private bool _isFriendly;

		// Token: 0x040005E8 RID: 1512
		private bool _isFocused;
	}
}
