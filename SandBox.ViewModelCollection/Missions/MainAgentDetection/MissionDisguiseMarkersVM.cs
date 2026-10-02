using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000043 RID: 67
	public class MissionDisguiseMarkersVM : ViewModel
	{
		// Token: 0x0600046E RID: 1134 RVA: 0x00012254 File Offset: 0x00010454
		public MissionDisguiseMarkersVM()
		{
			this.HostileAgents = new MBBindingList<MissionDisguiseMarkerItemVM>();
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00012267 File Offset: 0x00010467
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x0001226F File Offset: 0x0001046F
		[DataSourceProperty]
		public MissionDisguiseMarkerItemVM TargetAgent
		{
			get
			{
				return this._targetAgent;
			}
			set
			{
				if (value != this._targetAgent)
				{
					this._targetAgent = value;
					base.OnPropertyChangedWithValue<MissionDisguiseMarkerItemVM>(value, "TargetAgent");
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x0001228D File Offset: 0x0001048D
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00012295 File Offset: 0x00010495
		[DataSourceProperty]
		public MBBindingList<MissionDisguiseMarkerItemVM> HostileAgents
		{
			get
			{
				return this._hostileAgents;
			}
			set
			{
				if (value != this._hostileAgents)
				{
					this._hostileAgents = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionDisguiseMarkerItemVM>>(value, "HostileAgents");
				}
			}
		}

		// Token: 0x04000245 RID: 581
		private MissionDisguiseMarkerItemVM _targetAgent;

		// Token: 0x04000246 RID: 582
		private MBBindingList<MissionDisguiseMarkerItemVM> _hostileAgents;
	}
}
