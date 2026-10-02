using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000347 RID: 839
	public class ScenePropNegativeLight : ScriptComponentBehavior
	{
		// Token: 0x06002F21 RID: 12065 RVA: 0x000B6FB2 File Offset: 0x000B51B2
		protected internal override void OnEditorTick(float dt)
		{
			this.SetMeshParameters();
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000B6FBC File Offset: 0x000B51BC
		private void SetMeshParameters()
		{
			MetaMesh metaMesh = base.GameEntity.GetMetaMesh(0);
			if (metaMesh != null)
			{
				metaMesh.SetVectorArgument(this.Flatness_X, this.Flatness_Y, this.Flatness_Z, this.Alpha);
				if (this.Is_Dark_Light)
				{
					metaMesh.SetVectorArgument2(1f, 0f, 0f, 0f);
					return;
				}
				metaMesh.SetVectorArgument2(0f, 0f, 0f, 0f);
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000B703D File Offset: 0x000B523D
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetMeshParameters();
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000B704B File Offset: 0x000B524B
		protected internal override bool IsOnlyVisual()
		{
			return true;
		}

		// Token: 0x040012DF RID: 4831
		public float Flatness_X;

		// Token: 0x040012E0 RID: 4832
		public float Flatness_Y;

		// Token: 0x040012E1 RID: 4833
		public float Flatness_Z;

		// Token: 0x040012E2 RID: 4834
		public float Alpha = 1f;

		// Token: 0x040012E3 RID: 4835
		public bool Is_Dark_Light = true;
	}
}
