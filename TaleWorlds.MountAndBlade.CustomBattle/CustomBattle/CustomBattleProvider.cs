using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.CustomBattle;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x02000019 RID: 25
	public class CustomBattleProvider : ICustomBattleProvider
	{
		// Token: 0x06000125 RID: 293 RVA: 0x0000905D File Offset: 0x0000725D
		public void StartCustomBattle()
		{
			MBGameManager.StartNewGame(new CustomGameManager());
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009069 File Offset: 0x00007269
		public TextObject GetName()
		{
			return new TextObject("{=RZyk1LZy}Land Custom Battle", null);
		}
	}
}
