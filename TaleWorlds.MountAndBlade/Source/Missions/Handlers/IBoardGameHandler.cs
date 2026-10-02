using System;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers
{
	// Token: 0x020003E3 RID: 995
	public interface IBoardGameHandler
	{
		// Token: 0x0600375D RID: 14173
		void SwitchTurns();

		// Token: 0x0600375E RID: 14174
		void DiceRoll(int roll);

		// Token: 0x0600375F RID: 14175
		void Install();

		// Token: 0x06003760 RID: 14176
		void Uninstall();

		// Token: 0x06003761 RID: 14177
		void Activate();
	}
}
