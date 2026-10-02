using System;
using System.Linq.Expressions;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000345 RID: 837
	public class RoadStart : ScriptComponentBehavior
	{
		// Token: 0x06002F12 RID: 12050 RVA: 0x000B6C30 File Offset: 0x000B4E30
		protected internal override void OnInit()
		{
			this.pathEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(base.Scene, false, true, true);
			this.pathEntity.Name = "Road_Entity";
			this.UpdatePathMesh();
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000B6C5C File Offset: 0x000B4E5C
		protected internal override void OnEditorInit()
		{
			this.OnInit();
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000B6C64 File Offset: 0x000B4E64
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			if (this.pathEntity != null)
			{
				this.pathEntity.Remove(removeReason);
			}
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000B6C88 File Offset: 0x000B4E88
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (base.Scene.IsEntityFrameChanged(base.GameEntity.Name))
			{
				this.UpdatePathMesh();
			}
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000B6CC0 File Offset: 0x000B4EC0
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == MBGlobals.GetMemberName<string>(Expression.Lambda<Func<string>>(Expression.Field(Expression.Constant(this, typeof(RoadStart)), fieldof(RoadStart.materialName)), Array.Empty<ParameterExpression>())))
			{
				this.UpdatePathMesh();
			}
			if (this.pathMesh != null)
			{
				this.pathMesh.SetVectorArgument2(this.textureSweepX, this.textureSweepY, 0f, 0f);
			}
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x000B6D40 File Offset: 0x000B4F40
		private void UpdatePathMesh()
		{
			this.pathEntity.ClearComponents();
			this.pathMesh = MetaMesh.CreateMetaMesh(null);
			Material fromResource = Material.GetFromResource(this.materialName);
			if (fromResource != null)
			{
				this.pathMesh.SetMaterial(fromResource);
			}
			else
			{
				this.pathMesh.SetMaterial(Material.GetDefaultMaterial());
			}
			this.pathEntity.AddMultiMesh(this.pathMesh, true);
			this.pathMesh.SetVectorArgument2(this.textureSweepX, this.textureSweepY, 0f, 0f);
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x000B6DCA File Offset: 0x000B4FCA
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x040012CE RID: 4814
		public float textureSweepX;

		// Token: 0x040012CF RID: 4815
		public float textureSweepY;

		// Token: 0x040012D0 RID: 4816
		public string materialName = "blood_decal_terrain_material";

		// Token: 0x040012D1 RID: 4817
		private GameEntity pathEntity;

		// Token: 0x040012D2 RID: 4818
		private MetaMesh pathMesh;
	}
}
