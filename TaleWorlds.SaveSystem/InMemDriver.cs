using System;
using System.IO;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x0200000D RID: 13
	public class InMemDriver : ISaveDriver
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002F3C File Offset: 0x0000113C
		public Task<SaveResultWithMessage> Save(string saveName, int version, MetaData metaData, GameData gameData)
		{
			byte[] data = gameData.GetData();
			MemoryStream memoryStream = new MemoryStream();
			metaData.Add("version", version.ToString());
			metaData.Serialize(memoryStream);
			memoryStream.Write(data, 0, data.Length);
			this._data = memoryStream.GetBuffer();
			return Task.FromResult<SaveResultWithMessage>(SaveResultWithMessage.Default);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002F94 File Offset: 0x00001194
		public MetaData LoadMetaData(string saveName)
		{
			MemoryStream memoryStream = new MemoryStream(this._data);
			MetaData metaData = MetaData.Deserialize(memoryStream);
			memoryStream.Close();
			return metaData;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002FBC File Offset: 0x000011BC
		public LoadData Load(string saveName)
		{
			MemoryStream memoryStream = new MemoryStream(this._data);
			MetaData metaData = MetaData.Deserialize(memoryStream);
			byte[] array = new byte[memoryStream.Length - memoryStream.Position];
			memoryStream.Read(array, 0, array.Length);
			GameData gameData = GameData.CreateFrom(array);
			return new LoadData(metaData, gameData);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003008 File Offset: 0x00001208
		public SaveGameFileInfo[] GetSaveGameFileInfos()
		{
			return new SaveGameFileInfo[0];
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003010 File Offset: 0x00001210
		public string[] GetSaveGameFileNames()
		{
			return new string[0];
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003018 File Offset: 0x00001218
		public bool Delete(string saveName)
		{
			this._data = new byte[0];
			return true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003027 File Offset: 0x00001227
		public bool IsSaveGameFileExists(string saveName)
		{
			return false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000302A File Offset: 0x0000122A
		public bool IsWorkingAsync()
		{
			return false;
		}

		// Token: 0x04000016 RID: 22
		private byte[] _data;
	}
}
