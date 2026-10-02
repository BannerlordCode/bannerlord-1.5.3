using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000329 RID: 809
	public class PlayerConnectionInfo
	{
		// Token: 0x06002E3C RID: 11836 RVA: 0x000B320B File Offset: 0x000B140B
		public PlayerConnectionInfo(PlayerId playerID)
		{
			this.PlayerID = playerID;
			this._parameters = new Dictionary<string, object>();
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000B3225 File Offset: 0x000B1425
		public void AddParameter(string name, object parameter)
		{
			if (!this._parameters.ContainsKey(name))
			{
				this._parameters.Add(name, parameter);
			}
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000B3244 File Offset: 0x000B1444
		public T GetParameter<T>(string name) where T : class
		{
			if (this._parameters.ContainsKey(name))
			{
				return this._parameters[name] as T;
			}
			return default(T);
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x000B327F File Offset: 0x000B147F
		// (set) Token: 0x06002E40 RID: 11840 RVA: 0x000B3287 File Offset: 0x000B1487
		public int SessionKey { get; set; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002E41 RID: 11841 RVA: 0x000B3290 File Offset: 0x000B1490
		// (set) Token: 0x06002E42 RID: 11842 RVA: 0x000B3298 File Offset: 0x000B1498
		public string Name { get; set; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002E43 RID: 11843 RVA: 0x000B32A1 File Offset: 0x000B14A1
		// (set) Token: 0x06002E44 RID: 11844 RVA: 0x000B32A9 File Offset: 0x000B14A9
		public NetworkCommunicator NetworkPeer { get; set; }

		// Token: 0x04001239 RID: 4665
		private Dictionary<string, object> _parameters;

		// Token: 0x0400123D RID: 4669
		public readonly PlayerId PlayerID;
	}
}
