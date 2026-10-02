using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003C5 RID: 965
	public class SpawnerBase : ScriptComponentBehavior
	{
		// Token: 0x06003641 RID: 13889 RVA: 0x000E0031 File Offset: 0x000DE231
		protected internal override bool OnCheckForProblems()
		{
			return !this._spawnerEditorHelper.IsValid;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000E0041 File Offset: 0x000DE241
		public virtual void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			Debug.FailedAssert("Please override 'AssignParameters' function in the derived class.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SpawnerBase.cs", "AssignParameters", 40);
		}

		// Token: 0x04001756 RID: 5974
		[EditorVisibleScriptComponentVariable(true)]
		public string ToBeSpawnedOverrideName = "";

		// Token: 0x04001757 RID: 5975
		[EditorVisibleScriptComponentVariable(true)]
		public string ToBeSpawnedOverrideNameForFireVersion = "";

		// Token: 0x04001758 RID: 5976
		protected SpawnerEntityEditorHelper _spawnerEditorHelper;

		// Token: 0x04001759 RID: 5977
		protected SpawnerEntityMissionHelper _spawnerMissionHelper;

		// Token: 0x0400175A RID: 5978
		protected SpawnerEntityMissionHelper _spawnerMissionHelperFire;

		// Token: 0x0200068E RID: 1678
		public class SpawnerPermissionField : EditorVisibleScriptComponentVariable
		{
			// Token: 0x0600422D RID: 16941 RVA: 0x000FFCE5 File Offset: 0x000FDEE5
			public SpawnerPermissionField()
				: base(false)
			{
			}
		}
	}
}
