using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000348 RID: 840
	public class ScenePropPositiveLight : ScriptComponentBehavior
	{
		// Token: 0x06002F25 RID: 12069 RVA: 0x000B704E File Offset: 0x000B524E
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.SetMeshParams();
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000B705D File Offset: 0x000B525D
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetMeshParams();
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000B706C File Offset: 0x000B526C
		private void SetMeshParams()
		{
			MetaMesh metaMesh = base.GameEntity.GetMetaMesh(0);
			if (metaMesh != null)
			{
				uint num = this.CalculateFactor(new Vec3(this.DirectLightRed, this.DirectLightGreen, this.DirectLightBlue, -1f), this.DirectLightIntensity);
				metaMesh.SetFactor1Linear(num);
				uint num2 = this.CalculateFactor(new Vec3(this.AmbientLightRed, this.AmbientLightGreen, this.AmbientLightBlue, -1f), this.AmbientLightIntensity);
				metaMesh.SetFactor2Linear(num2);
				metaMesh.SetVectorArgument(this.Flatness_X, this.Flatness_Y, this.Flatness_Z, 1f);
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000B7110 File Offset: 0x000B5310
		private uint CalculateFactor(Vec3 color, float alpha)
		{
			float num = 10f;
			byte maxValue = byte.MaxValue;
			alpha = MathF.Min(MathF.Max(0f, alpha), num);
			return ((uint)(alpha / num * (float)maxValue) << 24) + ((uint)(color.x * (float)maxValue) << 16) + ((uint)(color.y * (float)maxValue) << 8) + (uint)(color.z * (float)maxValue);
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000B716C File Offset: 0x000B536C
		protected internal override bool IsOnlyVisual()
		{
			return true;
		}

		// Token: 0x040012E4 RID: 4836
		public float Flatness_X;

		// Token: 0x040012E5 RID: 4837
		public float Flatness_Y;

		// Token: 0x040012E6 RID: 4838
		public float Flatness_Z;

		// Token: 0x040012E7 RID: 4839
		public float DirectLightRed = 1f;

		// Token: 0x040012E8 RID: 4840
		public float DirectLightGreen = 1f;

		// Token: 0x040012E9 RID: 4841
		public float DirectLightBlue = 1f;

		// Token: 0x040012EA RID: 4842
		public float DirectLightIntensity = 1f;

		// Token: 0x040012EB RID: 4843
		public float AmbientLightRed;

		// Token: 0x040012EC RID: 4844
		public float AmbientLightGreen;

		// Token: 0x040012ED RID: 4845
		public float AmbientLightBlue = 1f;

		// Token: 0x040012EE RID: 4846
		public float AmbientLightIntensity = 1f;
	}
}
