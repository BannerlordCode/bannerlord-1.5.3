using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby
{
	// Token: 0x02000172 RID: 370
	public abstract class MultiplayerLocalDataContainer<T> where T : MultiplayerLocalData
	{
		// Token: 0x06000A58 RID: 2648 RVA: 0x000108BF File Offset: 0x0000EABF
		public MultiplayerLocalDataContainer()
		{
			this._operationQueue = new List<MultiplayerLocalDataContainer<T>.ContainerOperation>();
			this._dataList = new List<T>();
			this._saveDirectoryName = this.GetSaveDirectoryName();
			this._saveFileName = this.GetSaveFileName();
			this._isCacheDirty = true;
		}

		// Token: 0x06000A59 RID: 2649
		protected abstract string GetSaveDirectoryName();

		// Token: 0x06000A5A RID: 2650
		protected abstract string GetSaveFileName();

		// Token: 0x06000A5B RID: 2651 RVA: 0x000108FC File Offset: 0x0000EAFC
		public void AddEntry(T item)
		{
			List<MultiplayerLocalDataContainer<T>.ContainerOperation> operationQueue = this._operationQueue;
			lock (operationQueue)
			{
				MultiplayerLocalDataContainer<T>.ContainerOperation containerOperation = MultiplayerLocalDataContainer<T>.ContainerOperation.CreateAsAdd(item);
				this._operationQueue.Add(containerOperation);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0001094C File Offset: 0x0000EB4C
		public void InsertEntry(T item, int index)
		{
			List<MultiplayerLocalDataContainer<T>.ContainerOperation> operationQueue = this._operationQueue;
			lock (operationQueue)
			{
				MultiplayerLocalDataContainer<T>.ContainerOperation containerOperation = MultiplayerLocalDataContainer<T>.ContainerOperation.CreateAsInsert(item, index);
				this._operationQueue.Add(containerOperation);
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0001099C File Offset: 0x0000EB9C
		public void RemoveEntry(T item)
		{
			List<MultiplayerLocalDataContainer<T>.ContainerOperation> operationQueue = this._operationQueue;
			lock (operationQueue)
			{
				MultiplayerLocalDataContainer<T>.ContainerOperation containerOperation = MultiplayerLocalDataContainer<T>.ContainerOperation.CreateAsRemove(item);
				this._operationQueue.Add(containerOperation);
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000109EC File Offset: 0x0000EBEC
		public MBReadOnlyList<T> GetEntries()
		{
			return new MBReadOnlyList<T>(this._dataList);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000109FC File Offset: 0x0000EBFC
		internal async Task Tick(float dt)
		{
			if (this._isCacheDirty)
			{
				await this.LoadFileAux();
				this._isCacheDirty = false;
			}
			while (this._operationQueue.Count > 0)
			{
				this.HandleOperation(this._operationQueue[0]);
				this._operationQueue.RemoveAt(0);
			}
			if (this._isFileDirty)
			{
				await this.SaveFileAux();
				this._isFileDirty = false;
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00010A44 File Offset: 0x0000EC44
		private void HandleOperation(MultiplayerLocalDataContainer<T>.ContainerOperation operation)
		{
			switch (operation.OperationType)
			{
			case MultiplayerLocalDataContainer<T>.OperationType.Add:
				this.AddEntryAux(operation.Item);
				return;
			case MultiplayerLocalDataContainer<T>.OperationType.Insert:
				this.InsertEntryAux(operation.Item, operation.Index);
				return;
			case MultiplayerLocalDataContainer<T>.OperationType.Remove:
				this.RemoveEntryAux(operation.Item);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00010A98 File Offset: 0x0000EC98
		private void AddEntryAux(T item)
		{
			bool flag;
			this.OnBeforeAddEntry(item, out flag);
			if (!flag)
			{
				return;
			}
			bool flag2 = false;
			using (List<T>.Enumerator enumerator = this._dataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasSameContentWith(item))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				this._dataList.Add(item);
			}
			else
			{
				Debug.FailedAssert("Item is already in container: " + item, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "AddEntryAux", 234);
			}
			this._isFileDirty = true;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00010B44 File Offset: 0x0000ED44
		private void InsertEntryAux(T item, int index)
		{
			bool flag;
			this.OnBeforeAddEntry(item, out flag);
			if (!flag)
			{
				return;
			}
			bool flag2 = false;
			using (List<T>.Enumerator enumerator = this._dataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasSameContentWith(item))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				if (index >= 0 && index < this._dataList.Count)
				{
					this._dataList.Insert(index, item);
				}
				else
				{
					this._dataList.Add(item);
				}
			}
			else
			{
				Debug.FailedAssert("Item is already in container: " + item, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "InsertEntryAux", 272);
			}
			this._isFileDirty = true;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00010C14 File Offset: 0x0000EE14
		protected virtual void OnBeforeAddEntry(T item, out bool canAddEntry)
		{
			canAddEntry = true;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00010C1C File Offset: 0x0000EE1C
		private void RemoveEntryAux(T item)
		{
			bool flag;
			this.OnBeforeRemoveEntry(item, out flag);
			if (!flag)
			{
				return;
			}
			int count = this._dataList.Count;
			for (int i = this._dataList.Count - 1; i >= 0; i--)
			{
				if (this._dataList[i].HasSameContentWith(item))
				{
					this._dataList.Remove(item);
				}
			}
			if (count == this._dataList.Count)
			{
				Debug.FailedAssert("Item is not in container: " + item, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "RemoveEntryAux", 304);
			}
			this._isFileDirty = true;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00010CBE File Offset: 0x0000EEBE
		protected virtual void OnBeforeRemoveEntry(T item, out bool canRemoveEntry)
		{
			canRemoveEntry = true;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00010CC3 File Offset: 0x0000EEC3
		private PlatformFilePath GetDataFilePath()
		{
			return new PlatformFilePath(new PlatformDirectoryPath(PlatformFileType.User, this._saveDirectoryName), this._saveFileName);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00010CDC File Offset: 0x0000EEDC
		private async Task SaveFileAux()
		{
			try
			{
				await FileHelper.SaveFileAsync(this.GetDataFilePath(), Common.SerializeObjectAsJson(this._dataList));
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("An exception occured while trying to save " + base.GetType().Name + " data: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "SaveFileAux", 331);
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00010D24 File Offset: 0x0000EF24
		private async Task LoadFileAux()
		{
			PlatformFilePath oldFilePath = this.GetCompatibilityFilePath();
			if (FileHelper.FileExists(oldFilePath))
			{
				string text = await FileHelper.GetFileContentStringAsync(oldFilePath);
				FileHelper.DeleteFile(oldFilePath);
				if (!string.IsNullOrEmpty(text))
				{
					this._dataList.Clear();
					List<T> list = null;
					try
					{
						list = JsonConvert.DeserializeObject<List<T>>(text);
					}
					catch
					{
						try
						{
							list = this.DeserializeInCompatibilityMode(text);
						}
						catch
						{
							Debug.FailedAssert("Failed to load old data in compatibility mode", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "LoadFileAux", 362);
						}
					}
					if (list != null)
					{
						foreach (T t in list)
						{
							this._dataList.Add(t);
						}
					}
					this._isFileDirty = true;
					return;
				}
			}
			PlatformFilePath dataFilePath = this.GetDataFilePath();
			if (FileHelper.FileExists(dataFilePath))
			{
				string text2 = await FileHelper.GetFileContentStringAsync(dataFilePath);
				if (!string.IsNullOrEmpty(text2))
				{
					this._dataList.Clear();
					List<T> list2 = null;
					try
					{
						list2 = JsonConvert.DeserializeObject<List<T>>(text2);
					}
					catch
					{
						try
						{
							list2 = this.DeserializeInCompatibilityMode(text2);
							this._isFileDirty = true;
						}
						catch
						{
							Debug.FailedAssert("Failed to load file in compatibility mode", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "LoadFileAux", 403);
						}
					}
					if (list2 != null)
					{
						foreach (T t2 in list2)
						{
							this._dataList.Add(t2);
						}
					}
				}
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00010D69 File Offset: 0x0000EF69
		protected virtual PlatformFilePath GetCompatibilityFilePath()
		{
			return new PlatformFilePath(new PlatformDirectoryPath(PlatformFileType.User, "DataOld"), "TmpData");
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00010D80 File Offset: 0x0000EF80
		protected virtual List<T> DeserializeInCompatibilityMode(string serializedJson)
		{
			return null;
		}

		// Token: 0x04000532 RID: 1330
		private readonly string _saveDirectoryName;

		// Token: 0x04000533 RID: 1331
		private readonly string _saveFileName;

		// Token: 0x04000534 RID: 1332
		private readonly List<MultiplayerLocalDataContainer<T>.ContainerOperation> _operationQueue;

		// Token: 0x04000535 RID: 1333
		private readonly List<T> _dataList;

		// Token: 0x04000536 RID: 1334
		private bool _isFileDirty;

		// Token: 0x04000537 RID: 1335
		private bool _isCacheDirty;

		// Token: 0x020001E1 RID: 481
		private enum OperationType
		{
			// Token: 0x04000742 RID: 1858
			Add,
			// Token: 0x04000743 RID: 1859
			Insert,
			// Token: 0x04000744 RID: 1860
			Remove
		}

		// Token: 0x020001E2 RID: 482
		private struct ContainerOperation
		{
			// Token: 0x06000BA4 RID: 2980 RVA: 0x00017EAA File Offset: 0x000160AA
			private ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType type, T item, int index)
			{
				this.OperationType = type;
				this.Item = item;
				this.Index = index;
			}

			// Token: 0x06000BA5 RID: 2981 RVA: 0x00017EC1 File Offset: 0x000160C1
			public static MultiplayerLocalDataContainer<T>.ContainerOperation CreateAsAdd(T item)
			{
				return new MultiplayerLocalDataContainer<T>.ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType.Add, item, -1);
			}

			// Token: 0x06000BA6 RID: 2982 RVA: 0x00017ECB File Offset: 0x000160CB
			public static MultiplayerLocalDataContainer<T>.ContainerOperation CreateAsRemove(T item)
			{
				return new MultiplayerLocalDataContainer<T>.ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType.Remove, item, -1);
			}

			// Token: 0x06000BA7 RID: 2983 RVA: 0x00017ED5 File Offset: 0x000160D5
			public static MultiplayerLocalDataContainer<T>.ContainerOperation CreateAsInsert(T item, int index)
			{
				return new MultiplayerLocalDataContainer<T>.ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType.Insert, item, index);
			}

			// Token: 0x04000745 RID: 1861
			public readonly MultiplayerLocalDataContainer<T>.OperationType OperationType;

			// Token: 0x04000746 RID: 1862
			public readonly T Item;

			// Token: 0x04000747 RID: 1863
			public readonly int Index;
		}

		// Token: 0x020001E3 RID: 483
		private class ContainerOperationComparer : IComparer<MultiplayerLocalDataContainer<T>.ContainerOperation>
		{
			// Token: 0x06000BA8 RID: 2984 RVA: 0x00017EDF File Offset: 0x000160DF
			public int Compare(MultiplayerLocalDataContainer<T>.ContainerOperation x, MultiplayerLocalDataContainer<T>.ContainerOperation y)
			{
				return x.OperationType.CompareTo(y.OperationType);
			}
		}
	}
}
