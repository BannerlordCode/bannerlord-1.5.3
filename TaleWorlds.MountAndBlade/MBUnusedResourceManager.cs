using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E7 RID: 487
	public class MBUnusedResourceManager
	{
		// Token: 0x06001CC8 RID: 7368 RVA: 0x00062253 File Offset: 0x00060453
		public static void SetMeshUsed(string meshName)
		{
			MBAPI.IMBWorld.SetMeshUsed(meshName);
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x00062260 File Offset: 0x00060460
		public static void SetMaterialUsed(string meshName)
		{
			MBAPI.IMBWorld.SetMaterialUsed(meshName);
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0006226D File Offset: 0x0006046D
		public static void SetBodyUsed(string bodyName)
		{
			MBAPI.IMBWorld.SetBodyUsed(bodyName);
		}
	}
}
