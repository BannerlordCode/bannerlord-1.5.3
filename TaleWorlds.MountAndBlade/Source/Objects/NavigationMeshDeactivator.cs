using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Source.Objects
{
	// Token: 0x020003D5 RID: 981
	public class NavigationMeshDeactivator : ScriptComponentBehavior
	{
		// Token: 0x06003703 RID: 14083 RVA: 0x000E3CBB File Offset: 0x000E1EBB
		public void DisableAssignedFaces(Scene scene)
		{
			scene.SetAbilityOfFacesWithId(this.DisableFaceWithId, false);
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x000E3CCB File Offset: 0x000E1ECB
		public void EnableAssignedFaces(Scene scene)
		{
			scene.SetAbilityOfFacesWithId(this.DisableFaceWithId, true);
		}

		// Token: 0x040017AE RID: 6062
		public int DisableFaceWithId = -1;

		// Token: 0x040017AF RID: 6063
		public int DisableFaceWithIdForAnimals = -1;
	}
}
