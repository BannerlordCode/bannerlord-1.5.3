using System;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x0200000E RID: 14
	public interface ISaveDriver
	{
		// Token: 0x06000045 RID: 69
		Task<SaveResultWithMessage> Save(string saveName, int version, MetaData metaData, GameData gameData);

		// Token: 0x06000046 RID: 70
		SaveGameFileInfo[] GetSaveGameFileInfos();

		// Token: 0x06000047 RID: 71
		string[] GetSaveGameFileNames();

		// Token: 0x06000048 RID: 72
		MetaData LoadMetaData(string saveName);

		// Token: 0x06000049 RID: 73
		LoadData Load(string saveName);

		// Token: 0x0600004A RID: 74
		bool Delete(string saveName);

		// Token: 0x0600004B RID: 75
		bool IsSaveGameFileExists(string saveName);

		// Token: 0x0600004C RID: 76
		bool IsWorkingAsync();
	}
}
