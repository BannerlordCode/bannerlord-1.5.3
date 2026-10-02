using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A5 RID: 677
	public class MissionRecorder
	{
		// Token: 0x06002593 RID: 9619 RVA: 0x0008901C File Offset: 0x0008721C
		public MissionRecorder(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x0008902B File Offset: 0x0008722B
		public void RestartRecord()
		{
			MBAPI.IMBMission.RestartRecord(this._mission.Pointer);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00089042 File Offset: 0x00087242
		public void ProcessRecordUntilTime(float time)
		{
			MBAPI.IMBMission.ProcessRecordUntilTime(this._mission.Pointer, time);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x0008905A File Offset: 0x0008725A
		public bool IsEndOfRecord()
		{
			return MBAPI.IMBMission.EndOfRecord(this._mission.Pointer);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00089071 File Offset: 0x00087271
		public void StartRecording()
		{
			MBAPI.IMBMission.StartRecording();
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x0008907D File Offset: 0x0008727D
		public void RecordCurrentState()
		{
			MBAPI.IMBMission.RecordCurrentState(this._mission.Pointer);
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x00089094 File Offset: 0x00087294
		public void BackupRecordToFile(string fileName, string gameType, string sceneLevels)
		{
			MBAPI.IMBMission.BackupRecordToFile(this._mission.Pointer, fileName, gameType, sceneLevels);
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000890AE File Offset: 0x000872AE
		public void RestoreRecordFromFile(string fileName)
		{
			MBAPI.IMBMission.RestoreRecordFromFile(this._mission.Pointer, fileName);
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000890C6 File Offset: 0x000872C6
		public void ClearRecordBuffers()
		{
			MBAPI.IMBMission.ClearRecordBuffers(this._mission.Pointer);
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000890DD File Offset: 0x000872DD
		public static string GetSceneNameForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetSceneNameForReplay(fileName);
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000890EA File Offset: 0x000872EA
		public static string GetGameTypeForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetGameTypeForReplay(fileName);
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000890F7 File Offset: 0x000872F7
		public static string GetSceneLevelsForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetSceneLevelsForReplay(fileName);
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00089104 File Offset: 0x00087304
		public static string GetAtmosphereNameForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetAtmosphereNameForReplay(fileName);
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00089111 File Offset: 0x00087311
		public static int GetAtmosphereSeasonForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetAtmosphereSeasonForReplay(fileName);
		}

		// Token: 0x04000E8B RID: 3723
		private readonly Mission _mission;
	}
}
