using System;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x02000022 RID: 34
	internal class SandBoxEditorMissionTester : IEditorMissionTester
	{
		// Token: 0x0600010C RID: 268 RVA: 0x000073AE File Offset: 0x000055AE
		void IEditorMissionTester.StartMissionForEditor(string missionName, string sceneName, string levels)
		{
			MBGameManager.StartNewGame(new EditorSceneMissionManager(missionName, sceneName, levels, false, "", false, 0f, 0f));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000073CE File Offset: 0x000055CE
		void IEditorMissionTester.StartMissionForReplayEditor(string missionName, string sceneName, string levels, string fileName, bool record, float startTime, float endTime)
		{
			MBGameManager.StartNewGame(new EditorSceneMissionManager(missionName, sceneName, levels, true, fileName, record, startTime, endTime));
		}
	}
}
