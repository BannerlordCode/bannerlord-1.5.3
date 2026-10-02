using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F4 RID: 244
	public class OrderOfBattleScreenWidget : Widget
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00022709 File Offset: 0x00020909
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x00022711 File Offset: 0x00020911
		public float AlphaChangeDuration { get; set; } = 0.15f;

		// Token: 0x06000C93 RID: 3219 RVA: 0x0002271A File Offset: 0x0002091A
		public OrderOfBattleScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00022750 File Offset: 0x00020950
		protected override void OnLateUpdate(float dt)
		{
			this.CanToggleHeroSelection = base.EventManager.DraggedWidget == null;
			if (this._isTransitioning)
			{
				if (this._alphaChangeTimeElapsed < this.AlphaChangeDuration)
				{
					this._currentAlpha = MathF.Lerp(this._initialAlpha, this._targetAlpha, this._alphaChangeTimeElapsed / this.AlphaChangeDuration, 1E-05f);
					ListPanel leftSideFormations = this.LeftSideFormations;
					if (leftSideFormations != null)
					{
						leftSideFormations.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					ListPanel rightSideFormations = this.RightSideFormations;
					if (rightSideFormations != null)
					{
						rightSideFormations.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					ListPanel captainPool = this.CaptainPool;
					if (captainPool != null)
					{
						captainPool.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					Widget markers = this.Markers;
					if (markers != null)
					{
						markers.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					this._alphaChangeTimeElapsed += dt;
					return;
				}
				this._currentAlpha = this._targetAlpha;
				ListPanel leftSideFormations2 = this.LeftSideFormations;
				if (leftSideFormations2 != null)
				{
					leftSideFormations2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				ListPanel rightSideFormations2 = this.RightSideFormations;
				if (rightSideFormations2 != null)
				{
					rightSideFormations2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				ListPanel captainPool2 = this.CaptainPool;
				if (captainPool2 != null)
				{
					captainPool2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				Widget markers2 = this.Markers;
				if (markers2 != null)
				{
					markers2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				this._isTransitioning = false;
			}
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00022890 File Offset: 0x00020A90
		protected void OnCameraControlsEnabledChanged()
		{
			this._alphaChangeTimeElapsed = 0f;
			this._targetAlpha = (this.AreCameraControlsEnabled ? this.CameraEnabledAlpha : 1f);
			this._initialAlpha = this._currentAlpha;
			this._isTransitioning = true;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x000228CB File Offset: 0x00020ACB
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x000228D3 File Offset: 0x00020AD3
		[Editor(false)]
		public bool AreCameraControlsEnabled
		{
			get
			{
				return this._areCameraControlsEnabled;
			}
			set
			{
				if (value != this._areCameraControlsEnabled)
				{
					this._areCameraControlsEnabled = value;
					base.OnPropertyChanged(value, "AreCameraControlsEnabled");
					this.OnCameraControlsEnabledChanged();
				}
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000228F7 File Offset: 0x00020AF7
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000228FF File Offset: 0x00020AFF
		[Editor(false)]
		public float CameraEnabledAlpha
		{
			get
			{
				return this._cameraEnabledAlpha;
			}
			set
			{
				if (value != this._cameraEnabledAlpha)
				{
					this._cameraEnabledAlpha = value;
					base.OnPropertyChanged(value, "CameraEnabledAlpha");
				}
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0002291D File Offset: 0x00020B1D
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x00022925 File Offset: 0x00020B25
		[Editor(false)]
		public ListPanel LeftSideFormations
		{
			get
			{
				return this._leftSideFormations;
			}
			set
			{
				if (value != this._leftSideFormations)
				{
					this._leftSideFormations = value;
					base.OnPropertyChanged<ListPanel>(value, "LeftSideFormations");
				}
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00022943 File Offset: 0x00020B43
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x0002294B File Offset: 0x00020B4B
		[Editor(false)]
		public ListPanel RightSideFormations
		{
			get
			{
				return this._rightSideFormations;
			}
			set
			{
				if (value != this._rightSideFormations)
				{
					this._rightSideFormations = value;
					base.OnPropertyChanged<ListPanel>(value, "RightSideFormations");
				}
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00022969 File Offset: 0x00020B69
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x00022971 File Offset: 0x00020B71
		[Editor(false)]
		public ListPanel CaptainPool
		{
			get
			{
				return this._captainPool;
			}
			set
			{
				if (value != this._captainPool)
				{
					this._captainPool = value;
					base.OnPropertyChanged<ListPanel>(value, "CaptainPool");
				}
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0002298F File Offset: 0x00020B8F
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00022997 File Offset: 0x00020B97
		[Editor(false)]
		public Widget Markers
		{
			get
			{
				return this._markers;
			}
			set
			{
				if (value != this._markers)
				{
					this._markers = value;
					base.OnPropertyChanged<Widget>(value, "Markers");
				}
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x000229B5 File Offset: 0x00020BB5
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x000229BD File Offset: 0x00020BBD
		[Editor(false)]
		public bool CanToggleHeroSelection
		{
			get
			{
				return this._canToggleHeroSelection;
			}
			set
			{
				if (value != this._canToggleHeroSelection)
				{
					this._canToggleHeroSelection = value;
					base.OnPropertyChanged(value, "CanToggleHeroSelection");
				}
			}
		}

		// Token: 0x040005AE RID: 1454
		private float _alphaChangeTimeElapsed;

		// Token: 0x040005AF RID: 1455
		private float _initialAlpha = 1f;

		// Token: 0x040005B0 RID: 1456
		private float _targetAlpha;

		// Token: 0x040005B1 RID: 1457
		private float _currentAlpha = 1f;

		// Token: 0x040005B2 RID: 1458
		private bool _isTransitioning;

		// Token: 0x040005B3 RID: 1459
		private bool _areCameraControlsEnabled;

		// Token: 0x040005B4 RID: 1460
		private float _cameraEnabledAlpha = 0.2f;

		// Token: 0x040005B5 RID: 1461
		private ListPanel _leftSideFormations;

		// Token: 0x040005B6 RID: 1462
		private ListPanel _rightSideFormations;

		// Token: 0x040005B7 RID: 1463
		private ListPanel _captainPool;

		// Token: 0x040005B8 RID: 1464
		private Widget _markers;

		// Token: 0x040005B9 RID: 1465
		private bool _canToggleHeroSelection;
	}
}
