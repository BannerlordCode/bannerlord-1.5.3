using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x0200009C RID: 156
	public class MissionAlwaysVisibleMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00030986 File Offset: 0x0002EB86
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0003098E File Offset: 0x0002EB8E
		public MissionPeer TargetPeer { get; private set; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00030997 File Offset: 0x0002EB97
		public override Vec3 WorldPosition
		{
			get
			{
				return this._position;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0003099F File Offset: 0x0002EB9F
		protected override float HeightOffset
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x000309A6 File Offset: 0x0002EBA6
		public MissionAlwaysVisibleMarkerTargetVM(MissionPeer peer, Vec3 position, Action<MissionAlwaysVisibleMarkerTargetVM> onRemove)
			: base(MissionMarkerType.Peer)
		{
			this.TargetPeer = peer;
			this._position = position;
			this._onRemove = onRemove;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x000309C4 File Offset: 0x0002EBC4
		public void ExecuteRemove()
		{
			Action<MissionAlwaysVisibleMarkerTargetVM> onRemove = this._onRemove;
			if (onRemove == null)
			{
				return;
			}
			onRemove(this);
		}

		// Token: 0x0400073C RID: 1852
		private Vec3 _position;

		// Token: 0x0400073D RID: 1853
		private Action<MissionAlwaysVisibleMarkerTargetVM> _onRemove;
	}
}
