using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033E RID: 830
	public class MapAtmosphereProbe : ScriptComponentBehavior
	{
		// Token: 0x06002EDB RID: 11995 RVA: 0x000B56EC File Offset: 0x000B38EC
		public float GetInfluenceAmount(Vec3 worldPosition)
		{
			return MBMath.SmoothStep(this.minRadius, this.maxRadius, worldPosition.Distance(base.GameEntity.GetGlobalFrame().origin));
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000B5724 File Offset: 0x000B3924
		public MapAtmosphereProbe()
		{
			this.hideAllProbes = MapAtmosphereProbe.hideAllProbesStatic;
			if (MBEditor.IsEditModeOn)
			{
				this.innerSphereMesh = MetaMesh.GetCopy("physics_sphere_detailed", true, false);
				this.outerSphereMesh = MetaMesh.GetCopy("physics_sphere_detailed", true, false);
				this.innerSphereMesh.SetMaterial(Material.GetFromResource("light_radius_visualizer"));
				this.outerSphereMesh.SetMaterial(Material.GetFromResource("light_radius_visualizer"));
			}
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000B57BC File Offset: 0x000B39BC
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (this.visualizeRadius && !MapAtmosphereProbe.hideAllProbesStatic)
			{
				uint num = 16711680U;
				uint num2 = 720640U;
				if (MBEditor.IsEntitySelected(base.GameEntity))
				{
					num |= 2147483648U;
					num2 |= 2147483648U;
				}
				else
				{
					num |= 1073741824U;
					num2 |= 1073741824U;
				}
				this.innerSphereMesh.SetFactor1(num);
				this.outerSphereMesh.SetFactor1(num2);
				MatrixFrame matrixFrame;
				matrixFrame.origin = base.GameEntity.GetGlobalFrame().origin;
				matrixFrame.rotation = Mat3.Identity;
				matrixFrame.rotation.ApplyScaleLocal(this.minRadius);
				MatrixFrame matrixFrame2;
				matrixFrame2.origin = base.GameEntity.GetGlobalFrame().origin;
				matrixFrame2.rotation = Mat3.Identity;
				matrixFrame2.rotation.ApplyScaleLocal(this.maxRadius);
				this.innerSphereMesh.SetVectorArgument(this.minRadius, this.maxRadius, 0f, 0f);
				this.outerSphereMesh.SetVectorArgument(this.minRadius, this.maxRadius, 0f, 0f);
				MBEditor.RenderEditorMesh(this.innerSphereMesh, matrixFrame);
				MBEditor.RenderEditorMesh(this.outerSphereMesh, matrixFrame2);
			}
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000B5904 File Offset: 0x000B3B04
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "minRadius")
			{
				this.minRadius = MBMath.ClampFloat(this.minRadius, 0.1f, this.maxRadius);
			}
			if (variableName == "maxRadius")
			{
				this.maxRadius = MBMath.ClampFloat(this.maxRadius, this.minRadius, float.MaxValue);
			}
			if (variableName == "hideAllProbes")
			{
				MapAtmosphereProbe.hideAllProbesStatic = this.hideAllProbes;
			}
		}

		// Token: 0x04001299 RID: 4761
		public bool visualizeRadius = true;

		// Token: 0x0400129A RID: 4762
		public bool hideAllProbes = true;

		// Token: 0x0400129B RID: 4763
		public static bool hideAllProbesStatic = true;

		// Token: 0x0400129C RID: 4764
		public float minRadius = 1f;

		// Token: 0x0400129D RID: 4765
		public float maxRadius = 2f;

		// Token: 0x0400129E RID: 4766
		public float rainDensity;

		// Token: 0x0400129F RID: 4767
		public float temperature;

		// Token: 0x040012A0 RID: 4768
		public string atmosphereType;

		// Token: 0x040012A1 RID: 4769
		public string colorGrade;

		// Token: 0x040012A2 RID: 4770
		private MetaMesh innerSphereMesh;

		// Token: 0x040012A3 RID: 4771
		private MetaMesh outerSphereMesh;
	}
}
