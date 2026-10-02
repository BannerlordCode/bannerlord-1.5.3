using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x0200004E RID: 78
	public class MissionAgentLockVisualizerVM : ViewModel
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x000178E9 File Offset: 0x00015AE9
		public MissionAgentLockVisualizerVM()
		{
			this._allTrackedAgentsSet = new Dictionary<Agent, MissionAgentLockItemVM>();
			this.AllTrackedAgents = new MBBindingList<MissionAgentLockItemVM>();
			this.IsEnabled = true;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00017910 File Offset: 0x00015B10
		public void OnActiveLockAgentChange(Agent oldAgent, Agent newAgent)
		{
			if (oldAgent != null && this._allTrackedAgentsSet.ContainsKey(oldAgent))
			{
				this.AllTrackedAgents.Remove(this._allTrackedAgentsSet[oldAgent]);
				this._allTrackedAgentsSet.Remove(oldAgent);
			}
			if (newAgent != null)
			{
				if (this._allTrackedAgentsSet.ContainsKey(newAgent))
				{
					this._allTrackedAgentsSet[newAgent].SetLockState(MissionAgentLockItemVM.LockStates.Active);
					return;
				}
				MissionAgentLockItemVM missionAgentLockItemVM = new MissionAgentLockItemVM(newAgent, MissionAgentLockItemVM.LockStates.Active);
				this._allTrackedAgentsSet.Add(newAgent, missionAgentLockItemVM);
				this.AllTrackedAgents.Add(missionAgentLockItemVM);
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00017998 File Offset: 0x00015B98
		public void OnPossibleLockAgentChange(Agent oldPossibleAgent, Agent newPossibleAgent)
		{
			if (oldPossibleAgent != null && this._allTrackedAgentsSet.ContainsKey(oldPossibleAgent))
			{
				this.AllTrackedAgents.Remove(this._allTrackedAgentsSet[oldPossibleAgent]);
				this._allTrackedAgentsSet.Remove(oldPossibleAgent);
			}
			if (newPossibleAgent != null)
			{
				if (this._allTrackedAgentsSet.ContainsKey(newPossibleAgent))
				{
					this._allTrackedAgentsSet[newPossibleAgent].SetLockState(MissionAgentLockItemVM.LockStates.Possible);
					return;
				}
				MissionAgentLockItemVM missionAgentLockItemVM = new MissionAgentLockItemVM(newPossibleAgent, MissionAgentLockItemVM.LockStates.Possible);
				this._allTrackedAgentsSet.Add(newPossibleAgent, missionAgentLockItemVM);
				this.AllTrackedAgents.Add(missionAgentLockItemVM);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x00017A20 File Offset: 0x00015C20
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x00017A28 File Offset: 0x00015C28
		[DataSourceProperty]
		public MBBindingList<MissionAgentLockItemVM> AllTrackedAgents
		{
			get
			{
				return this._allTrackedAgents;
			}
			set
			{
				if (value != this._allTrackedAgents)
				{
					this._allTrackedAgents = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentLockItemVM>>(value, "AllTrackedAgents");
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x00017A46 File Offset: 0x00015C46
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x00017A4E File Offset: 0x00015C4E
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					if (!value)
					{
						this.AllTrackedAgents.Clear();
						this._allTrackedAgentsSet.Clear();
					}
				}
			}
		}

		// Token: 0x040002E4 RID: 740
		private readonly Dictionary<Agent, MissionAgentLockItemVM> _allTrackedAgentsSet;

		// Token: 0x040002E5 RID: 741
		private MBBindingList<MissionAgentLockItemVM> _allTrackedAgents;

		// Token: 0x040002E6 RID: 742
		private bool _isEnabled;
	}
}
