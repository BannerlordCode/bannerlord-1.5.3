using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Map
{
	// Token: 0x02000046 RID: 70
	public class MapEventVisualItemVM : ViewModel
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00012567 File Offset: 0x00010767
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x0001256F File Offset: 0x0001076F
		public MapEvent MapEvent { get; private set; }

		// Token: 0x06000487 RID: 1159 RVA: 0x00012578 File Offset: 0x00010778
		public MapEventVisualItemVM(Camera mapCamera, MapEvent mapEvent)
		{
			this._mapCamera = mapCamera;
			this.MapEvent = mapEvent;
			this._mapEventPositionCache = mapEvent.Position;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001259A File Offset: 0x0001079A
		public void UpdateProperties()
		{
			this.EventType = (int)SandBoxUIHelper.GetMapEventVisualTypeFromMapEvent(this.MapEvent);
			this._isAVisibleEvent = this.MapEvent.IsVisible;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000125C0 File Offset: 0x000107C0
		public void ParallelUpdatePosition()
		{
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			if (this._mapEventPositionCache != this.MapEvent.Position)
			{
				this._mapEventPositionCache = this.MapEvent.Position;
			}
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, this._mapEventPositionCache.AsVec3() + new Vec3(0f, 0f, 1.5f, -1f), ref this._latestX, ref this._latestY, ref this._latestW);
			this._bindPosition = new Vec2(this._latestX, this._latestY);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00012675 File Offset: 0x00010875
		public void DetermineIsVisibleOnMap()
		{
			this._bindIsVisibleOnMap = this._latestW > 0f && this._mapCamera.Position.z < 200f && this._isAVisibleEvent;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000126AA File Offset: 0x000108AA
		public void UpdateBindingProperties()
		{
			this.Position = this._bindPosition;
			this.IsVisibleOnMap = this._bindIsVisibleOnMap;
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x000126C4 File Offset: 0x000108C4
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x000126CC File Offset: 0x000108CC
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
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000126EF File Offset: 0x000108EF
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x000126F7 File Offset: 0x000108F7
		public int EventType
		{
			get
			{
				return this._eventType;
			}
			set
			{
				if (this._eventType != value)
				{
					this._eventType = value;
					base.OnPropertyChangedWithValue(value, "EventType");
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00012715 File Offset: 0x00010915
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x0001271D File Offset: 0x0001091D
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
					this._isVisibleOnMap = value;
					base.OnPropertyChangedWithValue(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x0400024F RID: 591
		private Camera _mapCamera;

		// Token: 0x04000250 RID: 592
		private bool _isAVisibleEvent;

		// Token: 0x04000251 RID: 593
		private CampaignVec2 _mapEventPositionCache;

		// Token: 0x04000252 RID: 594
		private const float CameraDistanceCutoff = 200f;

		// Token: 0x04000253 RID: 595
		private Vec2 _bindPosition;

		// Token: 0x04000254 RID: 596
		private bool _bindIsVisibleOnMap;

		// Token: 0x04000255 RID: 597
		private float _latestX;

		// Token: 0x04000256 RID: 598
		private float _latestY;

		// Token: 0x04000257 RID: 599
		private float _latestW;

		// Token: 0x04000258 RID: 600
		private Vec2 _position;

		// Token: 0x04000259 RID: 601
		private int _eventType;

		// Token: 0x0400025A RID: 602
		private bool _isVisibleOnMap;
	}
}
