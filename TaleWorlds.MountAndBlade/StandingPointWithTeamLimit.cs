using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000364 RID: 868
	public class StandingPointWithTeamLimit : StandingPoint
	{
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x000CC045 File Offset: 0x000CA245
		// (set) Token: 0x06003206 RID: 12806 RVA: 0x000CC04D File Offset: 0x000CA24D
		public Team UsableTeam { get; set; }

		// Token: 0x06003207 RID: 12807 RVA: 0x000CC056 File Offset: 0x000CA256
		public override bool IsDisabledForAgent(Agent agent)
		{
			return agent.Team != this.UsableTeam || base.IsDisabledForAgent(agent);
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000CC06F File Offset: 0x000CA26F
		protected internal override bool IsUsableBySide(BattleSideEnum side)
		{
			return side == this.UsableTeam.Side && base.IsUsableBySide(side);
		}
	}
}
