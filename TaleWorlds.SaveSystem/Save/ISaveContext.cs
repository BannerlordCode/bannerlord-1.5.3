using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000029 RID: 41
	internal interface ISaveContext
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000180 RID: 384
		DefinitionContext DefinitionContext { get; }

		// Token: 0x06000181 RID: 385
		int AddOrGetStringId(string text);

		// Token: 0x06000182 RID: 386
		int GetObjectId(object target);

		// Token: 0x06000183 RID: 387
		int GetContainerId(object target);

		// Token: 0x06000184 RID: 388
		int GetStringId(string target);

		// Token: 0x06000185 RID: 389
		bool Save(object target, MetaData metaData, out string errorMessage);

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000186 RID: 390
		GameData SaveData { get; }

		// Token: 0x06000187 RID: 391
		void ReportSaveIntegrityDrift(string message);
	}
}
