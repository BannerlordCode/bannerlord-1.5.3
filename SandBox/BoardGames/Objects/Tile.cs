using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Objects
{
	// Token: 0x02000103 RID: 259
	public class Tile : ScriptComponentBehavior
	{
		// Token: 0x06000CF0 RID: 3312 RVA: 0x0005EEC0 File Offset: 0x0005D0C0
		protected override void OnInit()
		{
			base.OnInit();
			base.GameEntity.RemoveMultiMesh(base.GameEntity.GetMetaMesh(0));
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0005EEF4 File Offset: 0x0005D0F4
		public void SetVisibility(bool visible)
		{
			base.GameEntity.SetVisibilityExcludeParents(visible);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0005EF10 File Offset: 0x0005D110
		protected override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x04000597 RID: 1431
		public MetaMesh TileMesh;
	}
}
