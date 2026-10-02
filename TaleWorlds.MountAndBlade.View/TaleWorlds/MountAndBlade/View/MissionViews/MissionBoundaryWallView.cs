using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006E RID: 110
	public class MissionBoundaryWallView : MissionView
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x0001F668 File Offset: 0x0001D868
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			foreach (ICollection<Vec2> collection in base.Mission.Boundaries.Values)
			{
				this.CreateBoundaryEntity(collection);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001F6C8 File Offset: 0x0001D8C8
		private void CreateBoundaryEntity(ICollection<Vec2> boundaryPoints)
		{
			Mesh mesh = BoundaryWallView.CreateBoundaryMesh(base.Mission.Scene, boundaryPoints, 536918784U);
			if (mesh != null)
			{
				GameEntity gameEntity = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
				gameEntity.AddMesh(mesh, true);
				MatrixFrame identity = MatrixFrame.Identity;
				gameEntity.SetGlobalFrame(in identity, true);
				gameEntity.Name = "boundary_wall";
				gameEntity.SetMobility(GameEntity.Mobility.Stationary);
				gameEntity.EntityFlags |= EntityFlags.DoNotRenderToEnvmap;
			}
		}
	}
}
