using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x020000B6 RID: 182
	public readonly struct CampaignSaveMetaDataArgs
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x0001EDAA File Offset: 0x0001CFAA
		public CampaignSaveMetaDataArgs(string[] moduleName, params KeyValuePair<string, string>[] otherArgs)
		{
			this.ModuleNames = moduleName;
			this.OtherData = otherArgs;
		}

		// Token: 0x04000536 RID: 1334
		public readonly string[] ModuleNames;

		// Token: 0x04000537 RID: 1335
		public readonly KeyValuePair<string, string>[] OtherData;
	}
}
