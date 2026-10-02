using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000004 RID: 4
	public class CustomBattleState : GameState
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000047CC File Offset: 0x000029CC
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000047D7 File Offset: 0x000029D7
		protected override void OnInitialize()
		{
			base.OnInitialize();
			CustomBattleHelper.AssertMissingTroopsForDebug();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000047E4 File Offset: 0x000029E4
		[CommandLineFunctionality.CommandLineArgumentFunction("enable_custom_record", "replay_mission")]
		public static string EnableRecordMission(List<string> strings)
		{
			if (!(GameStateManager.Current.ActiveState is CustomBattleState))
			{
				return "Mission recording for custom battle can only be enabled while in custom battle screen.";
			}
			MissionState.RecordMission = true;
			return "Mission recording activated.";
		}
	}
}
