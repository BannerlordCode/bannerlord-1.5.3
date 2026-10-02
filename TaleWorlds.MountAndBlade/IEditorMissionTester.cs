using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DF RID: 735
	public interface IEditorMissionTester
	{
		// Token: 0x06002B23 RID: 11043
		void StartMissionForEditor(string missionName, string sceneName, string levels);

		// Token: 0x06002B24 RID: 11044
		void StartMissionForReplayEditor(string missionName, string sceneName, string levels, string fileName, bool record, float startTime, float endTime);
	}
}
