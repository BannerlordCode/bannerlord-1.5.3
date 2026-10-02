using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032E RID: 814
	public class AnimatedFlag : ScriptComponentBehavior
	{
		// Token: 0x06002E68 RID: 11880 RVA: 0x000B33B0 File Offset: 0x000B15B0
		public AnimatedFlag()
		{
			this._prevFlagMeshFrame = new Vec3(0f, 0f, 0f, -1f);
			this._prevTheta = 0f;
			this._prevSkew = 0f;
			this._time = 0f;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000B3403 File Offset: 0x000B1603
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
			MBDebug.Print("AnimatedFlag : OnInit called.", 0, Debug.DebugColor.Yellow, 17592186044416UL);
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000B3430 File Offset: 0x000B1630
		private void SmoothTheta(ref float theta, float dt)
		{
			float num = theta - this._prevTheta;
			if (num > 3.1415927f)
			{
				num -= 6.2831855f;
				this._prevTheta += 6.2831855f;
			}
			else if (num < -3.1415927f)
			{
				num += 6.2831855f;
				this._prevTheta -= 6.2831855f;
			}
			num = MathF.Min(num, 150f * dt);
			theta = this._prevTheta + num * 0.05f;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000B34AB File Offset: 0x000B16AB
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000B34B8 File Offset: 0x000B16B8
		protected internal override void OnTick(float dt)
		{
			if (dt == 0f)
			{
				return;
			}
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			MetaMesh metaMesh = base.GameEntity.GetMetaMesh(0);
			if (metaMesh == null)
			{
				return;
			}
			Vec3 vec = globalFrame.origin - this._prevFlagMeshFrame;
			vec.x /= dt;
			vec.y /= dt;
			vec.z /= dt;
			vec = new Vec3(20f, 0f, -10f, -1f) * 0.1f - vec;
			if (vec.LengthSquared < 1E-08f)
			{
				return;
			}
			Vec3 vec2 = globalFrame.rotation.TransformToLocal(in vec);
			vec2.z = 0f;
			vec2.Normalize();
			float num = MathF.Atan2(vec2.y, vec2.x);
			this.SmoothTheta(ref num, dt);
			Vec3 scaleVector = metaMesh.Frame.rotation.GetScaleVector();
			MatrixFrame identity = MatrixFrame.Identity;
			identity.Scale(in scaleVector);
			identity.rotation.RotateAboutUp(num);
			this._prevTheta = num;
			float num2 = MathF.Acos(Vec3.DotProduct(vec, globalFrame.rotation.u) / vec.Length);
			float num3 = num2 - this._prevSkew;
			num3 = MathF.Min(num3, 150f * dt);
			num2 = this._prevSkew + num3 * 0.05f;
			this._prevSkew = num2;
			float num4 = MBMath.ClampFloat(vec.Length, 0.001f, 10000f);
			this._time += dt * num4 * 0.5f;
			metaMesh.Frame = identity;
			metaMesh.VectorUserData = new Vec3(MathF.Cos(num2), 1f - MathF.Sin(num2), 0f, this._time);
			this._prevFlagMeshFrame = globalFrame.origin;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000B36A4 File Offset: 0x000B18A4
		protected internal override bool IsOnlyVisual()
		{
			return true;
		}

		// Token: 0x04001249 RID: 4681
		private float _prevTheta;

		// Token: 0x0400124A RID: 4682
		private float _prevSkew;

		// Token: 0x0400124B RID: 4683
		private Vec3 _prevFlagMeshFrame;

		// Token: 0x0400124C RID: 4684
		private float _time;
	}
}
