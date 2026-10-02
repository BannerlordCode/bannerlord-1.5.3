using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034E RID: 846
	[Obsolete]
	public class DestructableMissionObject : MissionObject
	{
		// Token: 0x0600300D RID: 12301 RVA: 0x000BCEED File Offset: 0x000BB0ED
		protected internal override void OnEditorInit()
		{
			Debug.FailedAssert("This scene is using old prefabs with the old destruction system, please update!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\DestructableMissionObject.cs", "OnEditorInit", 18);
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000BCF05 File Offset: 0x000BB105
		protected internal override void OnInit()
		{
			Debug.FailedAssert("This scene is using old prefabs with the old destruction system, please update! The game will now close!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\DestructableMissionObject.cs", "OnInit", 23);
			Environment.Exit(0);
		}
	}
}
