using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000081 RID: 129
	public class MissionViewsContainer
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x00025030 File Offset: 0x00023230
		public MissionViewsContainer()
		{
			this._missionViews = new List<MissionView>();
			this._missionViewsCopy = this._missionViews.ToList<MissionView>();
			this._missionViewsCopiedFrame = Utilities.EngineFrameNo;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00025066 File Offset: 0x00023266
		public void Add(MissionView missionView)
		{
			this._missionViews.Add(missionView);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00025074 File Offset: 0x00023274
		public void Remove(MissionView missionView)
		{
			this._missionViews.Remove(missionView);
			missionView.IsFinalized = true;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002508A File Offset: 0x0002328A
		public bool Contains(MissionView missionView)
		{
			return this._missionViews.Contains(missionView);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00025098 File Offset: 0x00023298
		public bool Any(Func<MissionView, bool> predicate)
		{
			return this._missionViews.Any<MissionView>(predicate);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000250A8 File Offset: 0x000232A8
		public void ForEach(Action<MissionView> action)
		{
			foreach (MissionView missionView in this.GetMissionViewsCopy())
			{
				if (!missionView.IsFinalized)
				{
					action(missionView);
				}
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00025104 File Offset: 0x00023304
		private List<MissionView> GetMissionViewsCopy()
		{
			int engineFrameNo = Utilities.EngineFrameNo;
			if (this._missionViewsCopiedFrame != engineFrameNo)
			{
				this._missionViewsCopy = this._missionViews.ToList<MissionView>();
				this._missionViewsCopiedFrame = engineFrameNo;
			}
			return this._missionViewsCopy;
		}

		// Token: 0x040002CE RID: 718
		private List<MissionView> _missionViews;

		// Token: 0x040002CF RID: 719
		private List<MissionView> _missionViewsCopy;

		// Token: 0x040002D0 RID: 720
		private int _missionViewsCopiedFrame = -1;
	}
}
