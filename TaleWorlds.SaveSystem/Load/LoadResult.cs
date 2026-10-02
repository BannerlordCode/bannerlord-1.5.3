using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003C RID: 60
	public class LoadResult
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000C475 File Offset: 0x0000A675
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000C47D File Offset: 0x0000A67D
		public object Root { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000C486 File Offset: 0x0000A686
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000C48E File Offset: 0x0000A68E
		public bool Successful { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000C497 File Offset: 0x0000A697
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000C49F File Offset: 0x0000A69F
		public LoadError[] Errors { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		public MetaData MetaData { get; private set; }

		// Token: 0x06000263 RID: 611 RVA: 0x0000C4B9 File Offset: 0x0000A6B9
		private LoadResult()
		{
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000C4C1 File Offset: 0x0000A6C1
		internal static LoadResult CreateSuccessful(object root, MetaData metaData, LoadCallbackInitializator loadCallbackInitializator)
		{
			return new LoadResult
			{
				Root = root,
				Successful = true,
				MetaData = metaData,
				_loadCallbackInitializator = loadCallbackInitializator
			};
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
		internal static LoadResult CreateFailed(IEnumerable<LoadError> errors)
		{
			return new LoadResult
			{
				Successful = false,
				Errors = errors.ToArray<LoadError>()
			};
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000C4FE File Offset: 0x0000A6FE
		public void InitializeObjects()
		{
			this._loadCallbackInitializator.InitializeObjects();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000C50B File Offset: 0x0000A70B
		public void AfterInitializeObjects()
		{
			this._loadCallbackInitializator.AfterInitializeObjects();
		}

		// Token: 0x040000BC RID: 188
		private LoadCallbackInitializator _loadCallbackInitializator;
	}
}
