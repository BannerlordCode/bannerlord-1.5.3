using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x0200040B RID: 1035
	public abstract class AutoBlockModel : MBGameModel<AutoBlockModel>
	{
		// Token: 0x0600389A RID: 14490
		public abstract Agent.UsageDirection GetBlockDirection(Mission mission);
	}
}
