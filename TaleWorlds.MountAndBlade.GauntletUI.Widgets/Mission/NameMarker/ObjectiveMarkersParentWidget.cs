using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000FA RID: 250
	public class ObjectiveMarkersParentWidget : Widget
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00023F8F File Offset: 0x0002218F
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00023F97 File Offset: 0x00022197
		public float MinDistanceToFocus { get; set; }

		// Token: 0x06000D1B RID: 3355 RVA: 0x00023FA0 File Offset: 0x000221A0
		public ObjectiveMarkersParentWidget(UIContext context)
			: base(context)
		{
			this._markers = new List<ObjectiveMarkerWidget>();
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00023FB4 File Offset: 0x000221B4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = (this.IsMarkersEnabled ? this.TargetAlphaValue : 0f);
			float num2 = MathF.Clamp(dt * 10f, 0f, 1f);
			base.AlphaFactor = Mathf.Lerp(base.AlphaFactor, num, num2);
			base.Children.Sort(new ObjectiveMarkersParentWidget.MarkerRenderOrderComparer());
			this._markers.Sort(new ObjectiveMarkersParentWidget.MarkerPositionComparer());
			this.UpdateMarkerCombinations();
			this.UpdateMarkerFocus();
			for (int i = 0; i < this._markers.Count; i++)
			{
				this._markers[i].Update(dt);
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0002405C File Offset: 0x0002225C
		private void UpdateMarkerCombinations()
		{
			for (int i = 0; i < this._markers.Count; i++)
			{
				this._markers[i].CombinedSiblingsCount = 0;
				this._markers[i].IsMainCombinationMarker = false;
			}
			for (int j = 0; j < this._markers.Count; j++)
			{
				ObjectiveMarkerWidget objectiveMarkerWidget = this._markers[j];
				if (!objectiveMarkerWidget.IsCombinedWithOtherMarkers && objectiveMarkerWidget.IsMarkerActive && objectiveMarkerWidget.IsMarkerEnabled)
				{
					Vec2 vec;
					List<ObjectiveMarkerWidget> list = this.FindClosestMarkersToCombine(objectiveMarkerWidget, out vec);
					objectiveMarkerWidget.CombinedSiblingsCount = list.Count;
					objectiveMarkerWidget.IsMainCombinationMarker = list.Count > 0;
					if (objectiveMarkerWidget.IsCombinedWithOtherMarkers)
					{
						objectiveMarkerWidget.CombinedAveragePosition = vec;
					}
					for (int k = 0; k < list.Count; k++)
					{
						list[k].CombinedSiblingsCount = list.Count;
						list[k].CombinedAveragePosition = vec;
						list[k].IsMainCombinationMarker = false;
					}
				}
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00024164 File Offset: 0x00022364
		private void UpdateMarkerFocus()
		{
			float num;
			ObjectiveMarkerWidget objectiveMarkerWidget = this.FindClosestMarkerToFocus(base.EventManager.PageSize * 0.5f, out num);
			if (objectiveMarkerWidget != this._lastFocusedWidget)
			{
				if (this._lastFocusedWidget != null)
				{
					this._lastFocusedWidget.IsFocused = false;
				}
				this._lastFocusedWidget = objectiveMarkerWidget;
				if (this._lastFocusedWidget != null)
				{
					this._lastFocusedWidget.IsFocused = true;
				}
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000241CC File Offset: 0x000223CC
		private ObjectiveMarkerWidget FindClosestMarkerToFocus(Vec2 screenPosition, out float distanceSquared)
		{
			ObjectiveMarkerWidget objectiveMarkerWidget = null;
			distanceSquared = this.MinDistanceToFocus * this.MinDistanceToFocus;
			for (int i = 0; i < this._markers.Count; i++)
			{
				ObjectiveMarkerWidget objectiveMarkerWidget2 = this._markers[i];
				if (objectiveMarkerWidget2.IsInScreenBoundaries)
				{
					float num = objectiveMarkerWidget2.Position.DistanceSquared(screenPosition);
					if (num < distanceSquared)
					{
						distanceSquared = num;
						objectiveMarkerWidget = objectiveMarkerWidget2;
					}
				}
			}
			return objectiveMarkerWidget;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00024234 File Offset: 0x00022434
		private List<ObjectiveMarkerWidget> FindClosestMarkersToCombine(ObjectiveMarkerWidget marker, out Vec2 averageScreenPosition)
		{
			List<ObjectiveMarkerWidget> list = new List<ObjectiveMarkerWidget>();
			averageScreenPosition = marker.Position;
			for (int i = 0; i < this._markers.Count; i++)
			{
				if (this._markers[i] != marker && this._markers[i].IsInScreenBoundaries && !this._markers[i].IsCombinedWithOtherMarkers && this._markers[i].IsMarkerActive && this._markers[i].IsMarkerEnabled && marker.Position.Distance(this._markers[i].Position) < this.MaxDistanceToCombineMarkers)
				{
					averageScreenPosition += this._markers[i].Position;
					list.Add(this._markers[i]);
				}
			}
			averageScreenPosition /= (float)(list.Count + 1);
			return list;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0002434C File Offset: 0x0002254C
		private void OnMarkersChanged(Widget widget, string eventName, object[] args)
		{
			ObjectiveMarkerWidget objectiveMarkerWidget;
			if (args.Length == 1 && (objectiveMarkerWidget = args[0] as ObjectiveMarkerWidget) != null)
			{
				if (eventName == "ItemAdd")
				{
					this._markers.Add(objectiveMarkerWidget);
					return;
				}
				if (eventName == "ItemRemove")
				{
					this._markers.Remove(objectiveMarkerWidget);
				}
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x0002439F File Offset: 0x0002259F
		// (set) Token: 0x06000D23 RID: 3363 RVA: 0x000243A7 File Offset: 0x000225A7
		public bool IsMarkersEnabled
		{
			get
			{
				return this._isMarkersEnabled;
			}
			set
			{
				if (this._isMarkersEnabled != value)
				{
					this._isMarkersEnabled = value;
					base.OnPropertyChanged(value, "IsMarkersEnabled");
				}
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x000243C5 File Offset: 0x000225C5
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x000243CD File Offset: 0x000225CD
		public float TargetAlphaValue
		{
			get
			{
				return this._targetAlphaValue;
			}
			set
			{
				if (this._targetAlphaValue != value)
				{
					this._targetAlphaValue = value;
					base.OnPropertyChanged(value, "TargetAlphaValue");
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x000243EB File Offset: 0x000225EB
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x000243F3 File Offset: 0x000225F3
		public float MaxDistanceToCombineMarkers
		{
			get
			{
				return this._maxDistanceToCombineMarkers;
			}
			set
			{
				if (this._maxDistanceToCombineMarkers != value)
				{
					this._maxDistanceToCombineMarkers = value;
					base.OnPropertyChanged(value, "MaxDistanceToCombineMarkers");
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00024411 File Offset: 0x00022611
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x0002441C File Offset: 0x0002261C
		[Editor(false)]
		public Widget MarkersContainer
		{
			get
			{
				return this._markersContainer;
			}
			set
			{
				if (value != this._markersContainer)
				{
					if (this._markersContainer != null)
					{
						this._markersContainer.EventFire += this.OnMarkersChanged;
					}
					this._markersContainer = value;
					if (this._markersContainer != null)
					{
						this._markersContainer.EventFire += this.OnMarkersChanged;
					}
					base.OnPropertyChanged<Widget>(value, "MarkersContainer");
				}
			}
		}

		// Token: 0x040005F4 RID: 1524
		private List<ObjectiveMarkerWidget> _markers;

		// Token: 0x040005F5 RID: 1525
		private ObjectiveMarkerWidget _lastFocusedWidget;

		// Token: 0x040005F6 RID: 1526
		private bool _isMarkersEnabled;

		// Token: 0x040005F7 RID: 1527
		private float _targetAlphaValue;

		// Token: 0x040005F8 RID: 1528
		private float _maxDistanceToCombineMarkers;

		// Token: 0x040005F9 RID: 1529
		private Widget _markersContainer;

		// Token: 0x020001C5 RID: 453
		private class MarkerPositionComparer : IComparer<ObjectiveMarkerWidget>
		{
			// Token: 0x0600159D RID: 5533 RVA: 0x0003AC70 File Offset: 0x00038E70
			public int Compare(ObjectiveMarkerWidget x, ObjectiveMarkerWidget y)
			{
				return x.Position.x.CompareTo(y.Position.x);
			}
		}

		// Token: 0x020001C6 RID: 454
		private class MarkerRenderOrderComparer : IComparer<Widget>
		{
			// Token: 0x0600159F RID: 5535 RVA: 0x0003ACA4 File Offset: 0x00038EA4
			public int Compare(Widget x, Widget y)
			{
				ObjectiveMarkerWidget objectiveMarkerWidget = x as ObjectiveMarkerWidget;
				ObjectiveMarkerWidget objectiveMarkerWidget2 = y as ObjectiveMarkerWidget;
				int num = (objectiveMarkerWidget == null).CompareTo(objectiveMarkerWidget2 == null);
				if (num != 0)
				{
					return num;
				}
				int num2 = objectiveMarkerWidget.IsMainCombinationMarker.CompareTo(objectiveMarkerWidget2.IsMainCombinationMarker);
				if (num2 != 0)
				{
					return num2;
				}
				int num3 = objectiveMarkerWidget.CombinedSiblingsCount.CompareTo(objectiveMarkerWidget2.CombinedSiblingsCount);
				if (num3 != 0)
				{
					return num3;
				}
				return objectiveMarkerWidget.Position.x.CompareTo(objectiveMarkerWidget2.Position.x);
			}
		}
	}
}
