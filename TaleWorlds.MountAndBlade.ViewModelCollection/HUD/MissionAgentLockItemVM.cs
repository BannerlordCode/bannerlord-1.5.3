using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x0200004F RID: 79
	public class MissionAgentLockItemVM : ViewModel
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00017A85 File Offset: 0x00015C85
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x00017A8D File Offset: 0x00015C8D
		public Agent TrackedAgent { get; private set; }

		// Token: 0x06000686 RID: 1670 RVA: 0x00017A96 File Offset: 0x00015C96
		public MissionAgentLockItemVM(Agent agent, MissionAgentLockItemVM.LockStates initialLockState)
		{
			this.TrackedAgent = agent;
			this.LockState = (int)initialLockState;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00017AB3 File Offset: 0x00015CB3
		public void SetLockState(MissionAgentLockItemVM.LockStates lockState)
		{
			this.LockState = (int)lockState;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00017ABC File Offset: 0x00015CBC
		public void UpdatePosition(Vec2 position)
		{
			this.Position = position;
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00017AC5 File Offset: 0x00015CC5
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x00017ACD File Offset: 0x00015CCD
		[DataSourceProperty]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00017AF0 File Offset: 0x00015CF0
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00017AF8 File Offset: 0x00015CF8
		[DataSourceProperty]
		public int LockState
		{
			get
			{
				return this._lockState;
			}
			set
			{
				if (value != this._lockState)
				{
					this._lockState = value;
					base.OnPropertyChangedWithValue(value, "LockState");
				}
			}
		}

		// Token: 0x040002E8 RID: 744
		private Vec2 _position;

		// Token: 0x040002E9 RID: 745
		private int _lockState = -1;

		// Token: 0x020000E4 RID: 228
		public enum LockStates
		{
			// Token: 0x0400065F RID: 1631
			Possible,
			// Token: 0x04000660 RID: 1632
			Active
		}
	}
}
