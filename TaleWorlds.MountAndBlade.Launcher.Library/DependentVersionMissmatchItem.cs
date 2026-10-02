using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x0200000C RID: 12
	public struct DependentVersionMissmatchItem
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003551 File Offset: 0x00001751
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003559 File Offset: 0x00001759
		public string MissmatchedModuleId { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003562 File Offset: 0x00001762
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000356A File Offset: 0x0000176A
		public List<Tuple<DependedModule, ApplicationVersion>> MissmatchedDependencies { get; private set; }

		// Token: 0x0600006C RID: 108 RVA: 0x00003573 File Offset: 0x00001773
		public DependentVersionMissmatchItem(string missmatchedModuleId, List<Tuple<DependedModule, ApplicationVersion>> missmatchedDependencies)
		{
			this.MissmatchedModuleId = missmatchedModuleId;
			this.MissmatchedDependencies = missmatchedDependencies;
		}
	}
}
