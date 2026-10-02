using System;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000002 RID: 2
	public class AsyncFileSaveDriver : ISaveDriver
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public AsyncFileSaveDriver()
		{
			this._saveDriver = new FileDriver();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
		private void WaitPreviousTask()
		{
			Task currentNonSaveTask = this._currentNonSaveTask;
			Task<SaveResultWithMessage> currentSaveTask = this._currentSaveTask;
			if (currentNonSaveTask != null && !currentNonSaveTask.IsCompleted)
			{
				using (new PerformanceTestBlock("AsyncFileSaveDriver::Save - waiting previous save"))
				{
					currentNonSaveTask.Wait();
					return;
				}
			}
			if (currentSaveTask != null && !currentSaveTask.IsCompleted)
			{
				using (new PerformanceTestBlock("MBAsyncSaveDriver::Save - waiting previous save"))
				{
					currentSaveTask.Wait();
				}
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E4 File Offset: 0x000002E4
		Task<SaveResultWithMessage> ISaveDriver.Save(string saveName, int version, MetaData metaData, GameData gameData)
		{
			this.WaitPreviousTask();
			this._currentSaveTask = Task.Run<SaveResultWithMessage>(delegate
			{
				Task<SaveResultWithMessage> task = this._saveDriver.Save(saveName, version, metaData, gameData);
				this._currentNonSaveTask = null;
				return task;
			});
			return this._currentSaveTask;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000213E File Offset: 0x0000033E
		SaveGameFileInfo[] ISaveDriver.GetSaveGameFileInfos()
		{
			this.WaitPreviousTask();
			return this._saveDriver.GetSaveGameFileInfos();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002151 File Offset: 0x00000351
		string[] ISaveDriver.GetSaveGameFileNames()
		{
			this.WaitPreviousTask();
			return this._saveDriver.GetSaveGameFileNames();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002164 File Offset: 0x00000364
		MetaData ISaveDriver.LoadMetaData(string saveName)
		{
			this.WaitPreviousTask();
			return this._saveDriver.LoadMetaData(saveName);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002178 File Offset: 0x00000378
		LoadData ISaveDriver.Load(string saveName)
		{
			this.WaitPreviousTask();
			return this._saveDriver.Load(saveName);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000218C File Offset: 0x0000038C
		bool ISaveDriver.Delete(string saveName)
		{
			this.WaitPreviousTask();
			return this._saveDriver.Delete(saveName);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021A0 File Offset: 0x000003A0
		bool ISaveDriver.IsSaveGameFileExists(string saveName)
		{
			this.WaitPreviousTask();
			return this._saveDriver.IsSaveGameFileExists(saveName);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021B4 File Offset: 0x000003B4
		bool ISaveDriver.IsWorkingAsync()
		{
			return true;
		}

		// Token: 0x04000001 RID: 1
		private FileDriver _saveDriver;

		// Token: 0x04000002 RID: 2
		private Task _currentNonSaveTask;

		// Token: 0x04000003 RID: 3
		private Task<SaveResultWithMessage> _currentSaveTask;
	}
}
