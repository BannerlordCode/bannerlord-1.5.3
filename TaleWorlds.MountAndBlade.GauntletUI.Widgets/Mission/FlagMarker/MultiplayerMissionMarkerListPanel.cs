using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.FlagMarker
{
	// Token: 0x02000101 RID: 257
	public class MultiplayerMissionMarkerListPanel : ListPanel
	{
		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00025D7C File Offset: 0x00023F7C
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00025D84 File Offset: 0x00023F84
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00025D8D File Offset: 0x00023F8D
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00025D95 File Offset: 0x00023F95
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00025D9E File Offset: 0x00023F9E
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x00025DA6 File Offset: 0x00023FA6
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00025DAF File Offset: 0x00023FAF
		public MultiplayerMissionMarkerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00025DDC File Offset: 0x00023FDC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = MathF.Clamp(dt * 12f, 0f, 1f);
			if (!this._initialized)
			{
				this.SetInitialAlphaValuesOnCreation();
				this._initialized = true;
			}
			if (this.IsMarkerEnabled)
			{
				List<Widget> allChildrenAndThisRecursive = base.GetAllChildrenAndThisRecursive();
				for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
				{
					Widget widget = allChildrenAndThisRecursive[i];
					bool flag;
					if (widget != this && widget != this._activeWidget)
					{
						Widget activeWidget = this._activeWidget;
						flag = activeWidget != null && activeWidget.CheckIsMyChildRecursive(widget);
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						float distanceRelatedAlphaTarget = this.GetDistanceRelatedAlphaTarget(this.Distance);
						if (widget == this.SpawnFlagIconWidget)
						{
							widget.SetAlpha(this.IsSpawnFlag ? this.LocalLerp(widget.AlphaFactor, distanceRelatedAlphaTarget, num) : 0f);
						}
						else
						{
							widget.SetAlpha(this.LocalLerp(widget.AlphaFactor, distanceRelatedAlphaTarget, num));
						}
						if (widget != this.RemovalTimeVisiblityWidget)
						{
							widget.IsVisible = (double)widget.AlphaFactor > 0.05;
						}
					}
					else if (widget != this.RemovalTimeVisiblityWidget)
					{
						widget.IsVisible = false;
					}
				}
			}
			else
			{
				List<Widget> allChildrenAndThisRecursive2 = base.GetAllChildrenAndThisRecursive();
				for (int j = 0; j < allChildrenAndThisRecursive2.Count; j++)
				{
					Widget widget2 = allChildrenAndThisRecursive2[j];
					bool flag2;
					if (widget2 != this && widget2 != this._activeWidget)
					{
						Widget activeWidget2 = this._activeWidget;
						flag2 = activeWidget2 != null && activeWidget2.CheckIsMyChildRecursive(widget2);
					}
					else
					{
						flag2 = true;
					}
					if (flag2)
					{
						if (widget2 == this.SpawnFlagIconWidget)
						{
							widget2.SetAlpha(this.IsSpawnFlag ? this.LocalLerp(widget2.AlphaFactor, 0f, num) : 0f);
						}
						else
						{
							widget2.SetAlpha(this.LocalLerp(widget2.AlphaFactor, 0f, num));
						}
						if (widget2 != this.RemovalTimeVisiblityWidget)
						{
							widget2.IsVisible = (double)widget2.AlphaFactor > 0.05;
						}
					}
					else if (widget2 != this.RemovalTimeVisiblityWidget)
					{
						widget2.IsVisible = false;
					}
				}
			}
			Widget activeWidget3 = this._activeWidget;
			if (activeWidget3 != null && activeWidget3.IsVisible)
			{
				if (this._activeMarkerType == MultiplayerMissionMarkerListPanel.MissionMarkerType.Flag)
				{
					float x = base.Context.EventManager.PageSize.X;
					float y = base.Context.EventManager.PageSize.Y;
					base.ScaledPositionXOffset = MathF.Clamp(this.Position.x - base.Size.X / 2f, 10f, x - base.Size.X - 10f);
					base.ScaledPositionYOffset = MathF.Clamp(this.Position.y - base.Size.Y / 2f, 10f, y - base.Size.Y - 10f);
					return;
				}
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000260F4 File Offset: 0x000242F4
		private float GetDistanceRelatedAlphaTarget(int distance)
		{
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

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002617C File Offset: 0x0002437C
		private void SetInitialAlphaValuesOnCreation()
		{
			if (this.IsMarkerEnabled)
			{
				List<Widget> allChildrenAndThisRecursive = base.GetAllChildrenAndThisRecursive();
				for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
				{
					Widget widget = allChildrenAndThisRecursive[i];
					bool flag;
					if (widget != this && widget != this._activeWidget)
					{
						Widget activeWidget = this._activeWidget;
						flag = activeWidget != null && activeWidget.CheckIsMyChildRecursive(widget);
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						if (widget == this.SpawnFlagIconWidget)
						{
							widget.SetAlpha((float)(this.IsSpawnFlag ? 1 : 0));
						}
						else
						{
							widget.SetAlpha(1f);
						}
						if (widget != this.RemovalTimeVisiblityWidget)
						{
							widget.IsVisible = (double)widget.AlphaFactor > 0.05;
						}
					}
					else if (widget != this.RemovalTimeVisiblityWidget)
					{
						widget.IsVisible = false;
					}
				}
				return;
			}
			List<Widget> allChildrenAndThisRecursive2 = base.GetAllChildrenAndThisRecursive();
			for (int j = 0; j < allChildrenAndThisRecursive2.Count; j++)
			{
				Widget widget2 = allChildrenAndThisRecursive2[j];
				bool flag2;
				if (widget2 != this && widget2 != this._activeWidget)
				{
					Widget activeWidget2 = this._activeWidget;
					flag2 = activeWidget2 != null && activeWidget2.CheckIsMyChildRecursive(widget2);
				}
				else
				{
					flag2 = true;
				}
				if (flag2)
				{
					widget2.SetAlpha(0f);
					if (widget2 != this.RemovalTimeVisiblityWidget)
					{
						widget2.IsVisible = false;
					}
				}
				else if (widget2 != this.RemovalTimeVisiblityWidget)
				{
					widget2.IsVisible = false;
				}
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x000262BC File Offset: 0x000244BC
		private float LocalLerp(float start, float end, float delta)
		{
			if (Math.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x000262D8 File Offset: 0x000244D8
		private void MarkerTypeUpdated()
		{
			this._activeMarkerType = (MultiplayerMissionMarkerListPanel.MissionMarkerType)this.MarkerType;
			switch (this._activeMarkerType)
			{
			case MultiplayerMissionMarkerListPanel.MissionMarkerType.Flag:
				this._activeWidget = this.FlagWidget;
				return;
			case MultiplayerMissionMarkerListPanel.MissionMarkerType.Peer:
				this._activeWidget = this.PeerWidget;
				return;
			case MultiplayerMissionMarkerListPanel.MissionMarkerType.SiegeEngine:
				this._activeWidget = this.SiegeEngineWidget;
				return;
			default:
				return;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00026331 File Offset: 0x00024531
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x00026339 File Offset: 0x00024539
		public Widget FlagWidget
		{
			get
			{
				return this._flagWidget;
			}
			set
			{
				if (this._flagWidget != value)
				{
					this._flagWidget = value;
					base.OnPropertyChanged<Widget>(value, "FlagWidget");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x0002635D File Offset: 0x0002455D
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x00026365 File Offset: 0x00024565
		public Widget RemovalTimeVisiblityWidget
		{
			get
			{
				return this._removalTimeVisiblityWidget;
			}
			set
			{
				if (this._removalTimeVisiblityWidget != value)
				{
					this._removalTimeVisiblityWidget = value;
					base.OnPropertyChanged<Widget>(value, "RemovalTimeVisiblityWidget");
				}
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00026383 File Offset: 0x00024583
		// (set) Token: 0x06000DE1 RID: 3553 RVA: 0x0002638B File Offset: 0x0002458B
		public Widget SpawnFlagIconWidget
		{
			get
			{
				return this._spawnFlagIconWidget;
			}
			set
			{
				if (this._spawnFlagIconWidget != value)
				{
					this._spawnFlagIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "SpawnFlagIconWidget");
				}
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x000263A9 File Offset: 0x000245A9
		// (set) Token: 0x06000DE3 RID: 3555 RVA: 0x000263B1 File Offset: 0x000245B1
		public Widget PeerWidget
		{
			get
			{
				return this._peerWidget;
			}
			set
			{
				if (this._peerWidget != value)
				{
					this._peerWidget = value;
					base.OnPropertyChanged<Widget>(value, "PeerWidget");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x000263D5 File Offset: 0x000245D5
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x000263DD File Offset: 0x000245DD
		public Widget SiegeEngineWidget
		{
			get
			{
				return this._siegeEngineWidget;
			}
			set
			{
				if (value != this._siegeEngineWidget)
				{
					this._siegeEngineWidget = value;
					base.OnPropertyChanged<Widget>(value, "SiegeEngineWidget");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x00026401 File Offset: 0x00024601
		// (set) Token: 0x06000DE7 RID: 3559 RVA: 0x00026409 File Offset: 0x00024609
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

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0002642C File Offset: 0x0002462C
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x00026434 File Offset: 0x00024634
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

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00026452 File Offset: 0x00024652
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0002645A File Offset: 0x0002465A
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

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00026478 File Offset: 0x00024678
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x00026480 File Offset: 0x00024680
		public bool IsSpawnFlag
		{
			get
			{
				return this._isSpawnFlag;
			}
			set
			{
				if (this._isSpawnFlag != value)
				{
					this._isSpawnFlag = value;
					base.OnPropertyChanged(value, "IsSpawnFlag");
				}
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0002649E File Offset: 0x0002469E
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x000264A6 File Offset: 0x000246A6
		public int MarkerType
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
					base.OnPropertyChanged(value, "MarkerType");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x04000643 RID: 1603
		private const int FlagMarkerEdgeMargin = 10;

		// Token: 0x04000647 RID: 1607
		private MultiplayerMissionMarkerListPanel.MissionMarkerType _activeMarkerType;

		// Token: 0x04000648 RID: 1608
		private Widget _activeWidget;

		// Token: 0x04000649 RID: 1609
		private bool _initialized;

		// Token: 0x0400064A RID: 1610
		private int _distance;

		// Token: 0x0400064B RID: 1611
		private Widget _flagWidget;

		// Token: 0x0400064C RID: 1612
		private Widget _peerWidget;

		// Token: 0x0400064D RID: 1613
		private Widget _siegeEngineWidget;

		// Token: 0x0400064E RID: 1614
		private Widget _spawnFlagIconWidget;

		// Token: 0x0400064F RID: 1615
		private Vec2 _position;

		// Token: 0x04000650 RID: 1616
		private bool _isMarkerEnabled;

		// Token: 0x04000651 RID: 1617
		private bool _isSpawnFlag;

		// Token: 0x04000652 RID: 1618
		private int _markerType;

		// Token: 0x04000653 RID: 1619
		private Widget _removalTimeVisiblityWidget;

		// Token: 0x020001C8 RID: 456
		public enum MissionMarkerType
		{
			// Token: 0x04000A43 RID: 2627
			Flag,
			// Token: 0x04000A44 RID: 2628
			Peer,
			// Token: 0x04000A45 RID: 2629
			SiegeEngine
		}
	}
}
