using System;
using System.IO;

namespace TaleWorlds.Library
{
	// Token: 0x02000089 RID: 137
	public class ResourceDepotLocation
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x000123AD File Offset: 0x000105AD
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x000123B5 File Offset: 0x000105B5
		public string BasePath { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000123BE File Offset: 0x000105BE
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x000123C6 File Offset: 0x000105C6
		public string Path { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000123CF File Offset: 0x000105CF
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x000123D7 File Offset: 0x000105D7
		public string FullPath { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x000123E0 File Offset: 0x000105E0
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x000123E8 File Offset: 0x000105E8
		public FileSystemWatcher Watcher { get; private set; }

		// Token: 0x06000506 RID: 1286 RVA: 0x000123F1 File Offset: 0x000105F1
		public ResourceDepotLocation(string basePath, string path, string fullPath)
		{
			this.BasePath = basePath;
			this.Path = path;
			this.FullPath = fullPath;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00012410 File Offset: 0x00010610
		public void StartWatchingChanges(FileSystemEventHandler onChangeEvent, RenamedEventHandler onRenameEvent)
		{
			this.Watcher = new FileSystemWatcher
			{
				Path = this.FullPath,
				NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime),
				Filter = "*.*",
				IncludeSubdirectories = true,
				EnableRaisingEvents = true
			};
			this.Watcher.Changed += onChangeEvent;
			this.Watcher.Created += onChangeEvent;
			this.Watcher.Deleted += onChangeEvent;
			this.Watcher.Renamed += onRenameEvent;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00012485 File Offset: 0x00010685
		public void StopWatchingChanges()
		{
			this.Watcher.Dispose();
		}
	}
}
