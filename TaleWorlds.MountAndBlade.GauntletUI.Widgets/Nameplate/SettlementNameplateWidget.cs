using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000083 RID: 131
	public class SettlementNameplateWidget : Widget, IComparable<SettlementNameplateWidget>
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x00015946 File Offset: 0x00013B46
		public SettlementNameplateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001596C File Offset: 0x00013B6C
		private float _screenEdgeAlphaTarget
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00015973 File Offset: 0x00013B73
		private float _normalNeutralAlphaTarget
		{
			get
			{
				return 0.35f;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x0001597A File Offset: 0x00013B7A
		private float _normalAllyAlphaTarget
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00015981 File Offset: 0x00013B81
		private float _normalEnemyAlphaTarget
		{
			get
			{
				return 0.35f;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00015988 File Offset: 0x00013B88
		private float _trackedAlphaTarget
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001598F File Offset: 0x00013B8F
		private float _trackedColorFactorTarget
		{
			get
			{
				return 1.3f;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00015996 File Offset: 0x00013B96
		private float _normalColorFactorTarget
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000159A0 File Offset: 0x00013BA0
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			SettlementNameplateItemWidget nameplateItem = this.NameplateItem;
			if (nameplateItem != null)
			{
				nameplateItem.ParallelUpdate(dt);
			}
			if (nameplateItem != null && this._cachedItemSize != nameplateItem.Size)
			{
				this._cachedItemSize = nameplateItem.Size;
				ListPanel eventsListPanel = this._eventsListPanel;
				ListPanel notificationListPanel = this._notificationListPanel;
				if (eventsListPanel != null)
				{
					eventsListPanel.ScaledPositionXOffset = this._cachedItemSize.X;
				}
				if (notificationListPanel != null)
				{
					notificationListPanel.ScaledPositionYOffset = -this._cachedItemSize.Y;
				}
				base.SuggestedWidth = this._cachedItemSize.X * base._inverseScaleToUse;
				base.SuggestedHeight = this._cachedItemSize.Y * base._inverseScaleToUse;
				base.ScaledSuggestedWidth = this._cachedItemSize.X;
				base.ScaledSuggestedHeight = this._cachedItemSize.Y;
			}
			base.IsEnabled = this.IsVisibleOnMap;
			this.UpdateNameplateTransparencyAndBrightness(dt);
			this.UpdatePosition(dt);
			this.UpdateTutorialState();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00015A98 File Offset: 0x00013C98
		private void UpdatePosition(float dt)
		{
			SettlementNameplateItemWidget nameplateItem = this.NameplateItem;
			MapEventVisualBrushWidget mapEventVisualBrushWidget = ((nameplateItem != null) ? nameplateItem.MapEventVisualWidget : null);
			if (nameplateItem == null || mapEventVisualBrushWidget == null)
			{
				Debug.FailedAssert("Related widget null on UpdatePosition!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Nameplate\\SettlementNameplateWidget.cs", "UpdatePosition", 104);
				return;
			}
			bool flag = false;
			this._positionTimer += dt;
			if (this.IsVisibleOnMap || this._positionTimer < 2f)
			{
				float num = this.Position.X - base.Size.X / 2f - base.ScaledMarginLeft;
				float num2 = this.Position.X + base.Size.X / 2f + base.ScaledMarginRight;
				float num3 = this.Position.Y - base.Size.Y - base.ScaledMarginTop;
				float num4 = this.Position.Y + base.ScaledMarginBottom;
				bool flag2 = this.WSign > 0 && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
				if (this.IsTracked && !flag2)
				{
					Vec2 vec = new Vec2(num, num3);
					Vector2 vector = base.Context.EventManager.PageSize - base.Size;
					vector.X -= base.ScaledMarginLeft + base.ScaledMarginRight;
					vector.Y -= base.ScaledMarginTop + base.ScaledMarginBottom;
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
				}
				else
				{
					base.ScaledPositionXOffset = num;
					base.ScaledPositionYOffset = num3;
				}
				flag = base.ScaledPositionYOffset - mapEventVisualBrushWidget.Size.Y < 0f;
			}
			if (flag)
			{
				mapEventVisualBrushWidget.VerticalAlignment = VerticalAlignment.Bottom;
				mapEventVisualBrushWidget.ScaledPositionYOffset = mapEventVisualBrushWidget.Size.Y;
				return;
			}
			mapEventVisualBrushWidget.VerticalAlignment = VerticalAlignment.Top;
			mapEventVisualBrushWidget.ScaledPositionYOffset = -mapEventVisualBrushWidget.Size.Y;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00015DED File Offset: 0x00013FED
		private void OnNotificationListUpdated(Widget widget)
		{
			this._updatePositionNextFrame = true;
			this.AddLateUpdateAction();
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00015DFC File Offset: 0x00013FFC
		private void OnNotificationListUpdated(Widget parentWidget, Widget addedWidget)
		{
			this._updatePositionNextFrame = true;
			this.AddLateUpdateAction();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00015E0B File Offset: 0x0001400B
		private void AddLateUpdateAction()
		{
			if (!this._lateUpdateActionAdded)
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.CustomLateUpdate), 1);
				this._lateUpdateActionAdded = true;
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00015E35 File Offset: 0x00014035
		private void CustomLateUpdate(float dt)
		{
			if (this._updatePositionNextFrame)
			{
				this.UpdatePosition(dt);
				this._updatePositionNextFrame = false;
			}
			this._lateUpdateActionAdded = false;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00015E54 File Offset: 0x00014054
		private void UpdateTutorialState()
		{
			if (this._tutorialAnimState == SettlementNameplateWidget.TutorialAnimState.Start)
			{
				this._tutorialAnimState = SettlementNameplateWidget.TutorialAnimState.FirstFrame;
			}
			else
			{
				SettlementNameplateWidget.TutorialAnimState tutorialAnimState = this._tutorialAnimState;
			}
			if (this.IsTargetedByTutorial)
			{
				this.SetState("Default");
				return;
			}
			this.SetState("Disabled");
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00015E90 File Offset: 0x00014090
		private void SetNameplateRelationType(int type)
		{
			if (this.NameplateItem != null)
			{
				switch (type)
				{
				case 0:
					this.NameplateItem.Color = Color.Black;
					return;
				case 1:
					this.NameplateItem.Color = Color.ConvertStringToColor("#245E05FF");
					return;
				case 2:
					this.NameplateItem.Color = Color.ConvertStringToColor("#870707FF");
					return;
				case 3:
					this.NameplateItem.Color = Color.ConvertStringToColor("#2986CCFF");
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00015F10 File Offset: 0x00014110
		private void UpdateNameplateTransparencyAndBrightness(float dt)
		{
			SettlementNameplateItemWidget nameplateItem = this.NameplateItem;
			TextWidget textWidget = ((nameplateItem != null) ? nameplateItem.SettlementNameTextWidget : null);
			MaskedTextureWidget maskedTextureWidget = ((nameplateItem != null) ? nameplateItem.SettlementBannerWidget : null);
			GridWidget gridWidget = ((nameplateItem != null) ? nameplateItem.SettlementPartiesGridWidget : null);
			Widget widget = ((nameplateItem != null) ? nameplateItem.InspectedIconWidget : null);
			Widget widget2 = ((nameplateItem != null) ? nameplateItem.PortIconWidget : null);
			Widget widget3 = ((nameplateItem != null) ? nameplateItem.ParleyIconWidget : null);
			ListPanel eventsListPanel = this._eventsListPanel;
			if (nameplateItem == null || textWidget == null || maskedTextureWidget == null || gridWidget == null || widget == null || widget2 == null || widget3 == null || eventsListPanel == null)
			{
				Debug.FailedAssert("Related widget null on UpdateNameplateTransparencyAndBrightness!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Nameplate\\SettlementNameplateWidget.cs", "UpdateNameplateTransparencyAndBrightness", 302);
				return;
			}
			widget2.IsVisible = this.HasPort;
			float num = dt * this._lerpModifier;
			if (this.IsVisibleOnMap)
			{
				base.IsVisible = true;
				float num2 = this.DetermineTargetAlphaValue();
				float num3 = this.DetermineTargetColorFactor();
				float num4 = MathF.Lerp(nameplateItem.AlphaFactor, num2, num, 1E-05f);
				float num5 = MathF.Lerp(nameplateItem.ColorFactor, num3, num, 1E-05f);
				float num6 = MathF.Lerp(textWidget.ReadOnlyBrush.GlobalAlphaFactor, 1f, num, 1E-05f);
				nameplateItem.AlphaFactor = num4;
				nameplateItem.ColorFactor = num5;
				textWidget.Brush.GlobalAlphaFactor = num6;
				maskedTextureWidget.Brush.GlobalAlphaFactor = num6;
				gridWidget.SetGlobalAlphaRecursively(num6);
				widget3.AlphaFactor = MathF.Lerp(widget3.AlphaFactor, (float)(this.CanParley ? 1 : 0), num, 1E-05f);
				eventsListPanel.SetGlobalAlphaRecursively(num6);
			}
			else if (nameplateItem.AlphaFactor > this._lerpThreshold)
			{
				float num7 = MathF.Lerp(nameplateItem.AlphaFactor, 0f, num, 1E-05f);
				nameplateItem.AlphaFactor = num7;
				textWidget.Brush.GlobalAlphaFactor = num7;
				maskedTextureWidget.Brush.GlobalAlphaFactor = num7;
				gridWidget.SetGlobalAlphaRecursively(num7);
				widget3.AlphaFactor = num7;
				eventsListPanel.SetGlobalAlphaRecursively(num7);
			}
			else
			{
				base.IsVisible = false;
			}
			if (this.IsInRange && this.IsVisibleOnMap)
			{
				if (Math.Abs(widget.AlphaFactor - 1f) > this._lerpThreshold)
				{
					widget.AlphaFactor = MathF.Lerp(widget.AlphaFactor, 1f, num, 1E-05f);
					return;
				}
			}
			else if (nameplateItem.AlphaFactor - 0f > this._lerpThreshold)
			{
				widget.AlphaFactor = MathF.Lerp(widget.AlphaFactor, 0f, num, 1E-05f);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00016184 File Offset: 0x00014384
		private float DetermineTargetAlphaValue()
		{
			if (this.IsInsideWindow)
			{
				if (this.IsTracked)
				{
					return this._trackedAlphaTarget;
				}
				if (this.RelationType == 0)
				{
					return this._normalNeutralAlphaTarget;
				}
				if (this.RelationType == 1)
				{
					return this._normalAllyAlphaTarget;
				}
				return this._normalEnemyAlphaTarget;
			}
			else
			{
				if (this.IsTracked)
				{
					return this._screenEdgeAlphaTarget;
				}
				return 0f;
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000161E2 File Offset: 0x000143E2
		private float DetermineTargetColorFactor()
		{
			if (this.IsTracked)
			{
				return this._trackedColorFactorTarget;
			}
			return this._normalColorFactorTarget;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000161FC File Offset: 0x000143FC
		public int CompareTo(SettlementNameplateWidget other)
		{
			return other.DistanceToCamera.CompareTo(this.DistanceToCamera);
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x0001621D File Offset: 0x0001441D
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x00016225 File Offset: 0x00014425
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

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00016248 File Offset: 0x00014448
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x00016250 File Offset: 0x00014450
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (this._isVisibleOnMap != value)
				{
					if (this._isVisibleOnMap && !value)
					{
						this._positionTimer = 0f;
					}
					this._isVisibleOnMap = value;
					base.OnPropertyChanged(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x00016284 File Offset: 0x00014484
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x0001628C File Offset: 0x0001448C
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (this._isTracked != value)
				{
					this._isTracked = value;
					base.OnPropertyChanged(value, "IsTracked");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x000162AA File Offset: 0x000144AA
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x000162B2 File Offset: 0x000144B2
		public bool IsTargetedByTutorial
		{
			get
			{
				return this._isTargetedByTutorial;
			}
			set
			{
				if (this._isTargetedByTutorial != value)
				{
					this._isTargetedByTutorial = value;
					base.OnPropertyChanged(value, "IsTargetedByTutorial");
					if (value)
					{
						this._tutorialAnimState = SettlementNameplateWidget.TutorialAnimState.Start;
					}
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x000162DA File Offset: 0x000144DA
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x000162E2 File Offset: 0x000144E2
		public bool IsInsideWindow
		{
			get
			{
				return this._isInsideWindow;
			}
			set
			{
				if (this._isInsideWindow != value)
				{
					this._isInsideWindow = value;
					base.OnPropertyChanged(value, "IsInsideWindow");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00016300 File Offset: 0x00014500
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00016308 File Offset: 0x00014508
		public bool IsInRange
		{
			get
			{
				return this._isInRange;
			}
			set
			{
				if (this._isInRange != value)
				{
					this._isInRange = value;
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x0001631A File Offset: 0x0001451A
		// (set) Token: 0x0600077F RID: 1919 RVA: 0x00016322 File Offset: 0x00014522
		public bool CanParley
		{
			get
			{
				return this._canParley;
			}
			set
			{
				if (this._canParley != value)
				{
					this._canParley = value;
					base.OnPropertyChanged(value, "CanParley");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00016340 File Offset: 0x00014540
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x00016348 File Offset: 0x00014548
		public bool HasPort
		{
			get
			{
				return this._hasPort;
			}
			set
			{
				if (value != this._hasPort)
				{
					this._hasPort = value;
					base.OnPropertyChanged(value, "HasPort");
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00016366 File Offset: 0x00014566
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x0001636E File Offset: 0x0001456E
		public int RelationType
		{
			get
			{
				return this._relationType;
			}
			set
			{
				if (this._relationType != value)
				{
					this._relationType = value;
					base.OnPropertyChanged(value, "RelationType");
					this.SetNameplateRelationType(value);
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00016393 File Offset: 0x00014593
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0001639B File Offset: 0x0001459B
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

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x000163B9 File Offset: 0x000145B9
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x000163C1 File Offset: 0x000145C1
		public float WPos
		{
			get
			{
				return this._wPos;
			}
			set
			{
				if (this._wPos != value)
				{
					this._wPos = value;
					base.OnPropertyChanged(value, "WPos");
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000163DF File Offset: 0x000145DF
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x000163E7 File Offset: 0x000145E7
		public float DistanceToCamera
		{
			get
			{
				return this._distanceToCamera;
			}
			set
			{
				if (this._distanceToCamera != value)
				{
					this._distanceToCamera = value;
					base.OnPropertyChanged(value, "DistanceToCamera");
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00016405 File Offset: 0x00014605
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x0001640D File Offset: 0x0001460D
		public SettlementNameplateItemWidget NameplateItem
		{
			get
			{
				return this._nameplateItem;
			}
			set
			{
				if (this._nameplateItem != value)
				{
					this._nameplateItem = value;
					base.OnPropertyChanged<SettlementNameplateItemWidget>(value, "NameplateItem");
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0001642B File Offset: 0x0001462B
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x00016434 File Offset: 0x00014634
		public ListPanel NotificationListPanel
		{
			get
			{
				return this._notificationListPanel;
			}
			set
			{
				if (this._notificationListPanel != value)
				{
					this._notificationListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "NotificationListPanel");
					this._notificationListPanel.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnNotificationListUpdated));
					this._notificationListPanel.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.OnNotificationListUpdated));
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x00016495 File Offset: 0x00014695
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x0001649D File Offset: 0x0001469D
		public ListPanel EventsListPanel
		{
			get
			{
				return this._eventsListPanel;
			}
			set
			{
				if (value != this._eventsListPanel)
				{
					this._eventsListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "EventsListPanel");
				}
			}
		}

		// Token: 0x04000332 RID: 818
		private float _positionTimer;

		// Token: 0x04000333 RID: 819
		private bool _updatePositionNextFrame;

		// Token: 0x04000334 RID: 820
		private SettlementNameplateWidget.TutorialAnimState _tutorialAnimState;

		// Token: 0x04000335 RID: 821
		private float _lerpThreshold = 5E-05f;

		// Token: 0x04000336 RID: 822
		private float _lerpModifier = 10f;

		// Token: 0x04000337 RID: 823
		private Vector2 _cachedItemSize;

		// Token: 0x04000338 RID: 824
		private bool _lateUpdateActionAdded;

		// Token: 0x04000339 RID: 825
		private Vec2 _position;

		// Token: 0x0400033A RID: 826
		private bool _isVisibleOnMap;

		// Token: 0x0400033B RID: 827
		private bool _isTracked;

		// Token: 0x0400033C RID: 828
		private bool _isInsideWindow;

		// Token: 0x0400033D RID: 829
		private bool _isTargetedByTutorial;

		// Token: 0x0400033E RID: 830
		private int _relationType = -1;

		// Token: 0x0400033F RID: 831
		private int _wSign;

		// Token: 0x04000340 RID: 832
		private float _wPos;

		// Token: 0x04000341 RID: 833
		private float _distanceToCamera;

		// Token: 0x04000342 RID: 834
		private bool _isInRange;

		// Token: 0x04000343 RID: 835
		private bool _canParley;

		// Token: 0x04000344 RID: 836
		private bool _hasPort;

		// Token: 0x04000345 RID: 837
		private SettlementNameplateItemWidget _nameplateItem;

		// Token: 0x04000346 RID: 838
		private ListPanel _notificationListPanel;

		// Token: 0x04000347 RID: 839
		private ListPanel _eventsListPanel;

		// Token: 0x020001B8 RID: 440
		public enum TutorialAnimState
		{
			// Token: 0x04000A1A RID: 2586
			Idle,
			// Token: 0x04000A1B RID: 2587
			Start,
			// Token: 0x04000A1C RID: 2588
			FirstFrame,
			// Token: 0x04000A1D RID: 2589
			Playing
		}
	}
}
