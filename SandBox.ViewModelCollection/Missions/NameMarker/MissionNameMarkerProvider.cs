using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000031 RID: 49
	public abstract class MissionNameMarkerProvider
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x00010A94 File Offset: 0x0000EC94
		public MissionNameMarkerProvider()
		{
		}

		// Token: 0x060003DA RID: 986
		public abstract void CreateMarkers(List<MissionNameMarkerTargetBaseVM> markers);

		// Token: 0x060003DB RID: 987 RVA: 0x00010A9C File Offset: 0x0000EC9C
		public void Initialize(Mission mission, Action onSetMarkersDirty)
		{
			this.OnInitialize(mission);
			this._initialized = true;
			this._onSetMarkersDirty = onSetMarkersDirty;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00010AB3 File Offset: 0x0000ECB3
		public void Destroy(Mission mission)
		{
			this.OnDestroy(mission);
			this._initialized = false;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00010AC3 File Offset: 0x0000ECC3
		public void Tick(float dt)
		{
			this.OnTick(dt);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00010ACC File Offset: 0x0000ECCC
		protected virtual void OnInitialize(Mission mission)
		{
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010ACE File Offset: 0x0000ECCE
		protected virtual void OnDestroy(Mission mission)
		{
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		protected virtual void OnTick(float dt)
		{
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00010AD2 File Offset: 0x0000ECD2
		protected void SetMarkersDirty()
		{
			this._onSetMarkersDirty();
		}

		// Token: 0x0400020B RID: 523
		private Action _onSetMarkersDirty;

		// Token: 0x0400020C RID: 524
		private bool _initialized;
	}
}
