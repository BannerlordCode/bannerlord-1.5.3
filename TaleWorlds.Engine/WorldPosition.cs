using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x020000A2 RID: 162
	[EngineStruct("rglWorld_position::Plain_world_position", false, null)]
	public struct WorldPosition
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00011CB0 File Offset: 0x0000FEB0
		public Vec2 AsVec2
		{
			get
			{
				return this._position.AsVec2;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x00011CBD File Offset: 0x0000FEBD
		public float X
		{
			get
			{
				return this._position.x;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00011CCA File Offset: 0x0000FECA
		public float Y
		{
			get
			{
				return this._position.y;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x00011CD8 File Offset: 0x0000FED8
		public bool IsValid
		{
			get
			{
				return this.AsVec2.IsValid && this._scene != UIntPtr.Zero;
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00011D07 File Offset: 0x0000FF07
		internal WorldPosition(UIntPtr scenePointer, Vec3 position)
		{
			this = new WorldPosition(scenePointer, UIntPtr.Zero, position, false);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00011D18 File Offset: 0x0000FF18
		internal WorldPosition(UIntPtr scenePointer, UIntPtr navMesh, Vec3 position, bool hasValidZ)
		{
			this._scene = scenePointer;
			this._navMesh = navMesh;
			this._nearestNavMesh = this._navMesh;
			this._position = position;
			this.Normal = Vec3.Zero;
			if (hasValidZ)
			{
				this._lastValidZPosition = this._position.AsVec2;
				this.State = ZValidityState.Valid;
				return;
			}
			this._lastValidZPosition = Vec2.Invalid;
			this.State = ZValidityState.Invalid;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00011D80 File Offset: 0x0000FF80
		public WorldPosition(Scene scene, Vec3 position)
		{
			this = new WorldPosition((scene != null) ? scene.Pointer : UIntPtr.Zero, UIntPtr.Zero, position, false);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00011DA5 File Offset: 0x0000FFA5
		public WorldPosition(Scene scene, UIntPtr navMesh, Vec3 position, bool hasValidZ)
		{
			this = new WorldPosition((scene != null) ? scene.Pointer : UIntPtr.Zero, navMesh, position, hasValidZ);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00011DC8 File Offset: 0x0000FFC8
		public void SetVec3(UIntPtr navMesh, Vec3 position, bool hasValidZ)
		{
			this._navMesh = navMesh;
			this._nearestNavMesh = this._navMesh;
			this._position = position;
			this.Normal = Vec3.Zero;
			if (hasValidZ)
			{
				this._lastValidZPosition = this._position.AsVec2;
				this.State = ZValidityState.Valid;
				return;
			}
			this._lastValidZPosition = Vec2.Invalid;
			this.State = ZValidityState.Invalid;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00011E28 File Offset: 0x00010028
		private void ValidateZ(ZValidityState minimumValidityState)
		{
			if (this.State < minimumValidityState)
			{
				EngineApplicationInterface.IScene.WorldPositionValidateZ(ref this, (int)minimumValidityState);
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00011E40 File Offset: 0x00010040
		private void ValidateZMT(ZValidityState minimumValidityState)
		{
			if (this.State < minimumValidityState)
			{
				using (new TWSharedMutexReadLock(Scene.PhysicsAndRayCastLock))
				{
					EngineApplicationInterface.IScene.WorldPositionValidateZ(ref this, (int)minimumValidityState);
				}
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00011E90 File Offset: 0x00010090
		public UIntPtr GetNavMesh()
		{
			this.ValidateZ(ZValidityState.ValidAccordingToNavMesh);
			return this._navMesh;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00011E9F File Offset: 0x0001009F
		public UIntPtr GetNavMeshMT()
		{
			this.ValidateZMT(ZValidityState.ValidAccordingToNavMesh);
			return this._navMesh;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00011EAE File Offset: 0x000100AE
		public UIntPtr GetNearestNavMesh()
		{
			EngineApplicationInterface.IScene.WorldPositionComputeNearestNavMesh(ref this);
			return this._nearestNavMesh;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00011EC1 File Offset: 0x000100C1
		public float GetNavMeshZ()
		{
			this.ValidateZ(ZValidityState.ValidAccordingToNavMesh);
			if (this.State >= ZValidityState.ValidAccordingToNavMesh)
			{
				return this._position.z;
			}
			return float.NaN;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00011EE4 File Offset: 0x000100E4
		public float GetNavMeshZMT()
		{
			this.ValidateZMT(ZValidityState.ValidAccordingToNavMesh);
			if (this.State >= ZValidityState.ValidAccordingToNavMesh)
			{
				return this._position.z;
			}
			return float.NaN;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00011F07 File Offset: 0x00010107
		public float GetGroundZ()
		{
			this.ValidateZ(ZValidityState.Valid);
			if (this.State >= ZValidityState.Valid)
			{
				return this._position.z;
			}
			return float.NaN;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00011F2A File Offset: 0x0001012A
		public float GetGroundZMT()
		{
			this.ValidateZMT(ZValidityState.Valid);
			if (this.State >= ZValidityState.Valid)
			{
				return this._position.z;
			}
			return float.NaN;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00011F4D File Offset: 0x0001014D
		public Vec3 GetNavMeshVec3()
		{
			return new Vec3(this._position.AsVec2, this.GetNavMeshZ(), -1f);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00011F6A File Offset: 0x0001016A
		public Vec3 GetNavMeshVec3MT()
		{
			return new Vec3(this._position.AsVec2, this.GetNavMeshZMT(), -1f);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00011F87 File Offset: 0x00010187
		public Vec3 GetGroundVec3()
		{
			return new Vec3(this._position.AsVec2, this.GetGroundZ(), -1f);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00011FA4 File Offset: 0x000101A4
		public Vec3 GetGroundVec3MT()
		{
			return new Vec3(this._position.AsVec2, this.GetGroundZMT(), -1f);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00011FC1 File Offset: 0x000101C1
		public Vec3 GetVec3WithoutValidity()
		{
			return this._position;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00011FCC File Offset: 0x000101CC
		public void SetVec2MT(Vec2 value)
		{
			if (this._position.AsVec2 != value)
			{
				if (this.State != ZValidityState.Invalid)
				{
					this.State = ZValidityState.Invalid;
				}
				else if (!this._lastValidZPosition.IsValid)
				{
					this.ValidateZMT(ZValidityState.ValidAccordingToNavMesh);
					this.State = ZValidityState.Invalid;
				}
				this._position.x = value.x;
				this._position.y = value.y;
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0001203C File Offset: 0x0001023C
		public void SetVec2(Vec2 value)
		{
			if (this._position.AsVec2 != value)
			{
				if (this.State != ZValidityState.Invalid)
				{
					this.State = ZValidityState.Invalid;
				}
				else if (!this._lastValidZPosition.IsValid)
				{
					this.ValidateZ(ZValidityState.ValidAccordingToNavMesh);
					this.State = ZValidityState.Invalid;
				}
				this._position.x = value.x;
				this._position.y = value.y;
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000120AC File Offset: 0x000102AC
		public float DistanceSquaredWithLimit(in Vec3 targetPoint, float limitSquared)
		{
			Vec2 asVec = this._position.AsVec2;
			Vec3 vec = targetPoint;
			float num = asVec.DistanceSquared(vec.AsVec2);
			if (num <= limitSquared)
			{
				return this.GetGroundVec3().DistanceSquared(targetPoint);
			}
			return num;
		}

		// Token: 0x04000212 RID: 530
		private readonly UIntPtr _scene;

		// Token: 0x04000213 RID: 531
		private UIntPtr _navMesh;

		// Token: 0x04000214 RID: 532
		private UIntPtr _nearestNavMesh;

		// Token: 0x04000215 RID: 533
		private Vec3 _position;

		// Token: 0x04000216 RID: 534
		[CustomEngineStructMemberData("normal_")]
		public Vec3 Normal;

		// Token: 0x04000217 RID: 535
		private Vec2 _lastValidZPosition;

		// Token: 0x04000218 RID: 536
		[CustomEngineStructMemberData("z_validity_state_")]
		public ZValidityState State;

		// Token: 0x04000219 RID: 537
		public static readonly WorldPosition Invalid = new WorldPosition(UIntPtr.Zero, UIntPtr.Zero, Vec3.Invalid, false);

		// Token: 0x020000E6 RID: 230
		public enum WorldPositionEnforcedCache
		{
			// Token: 0x04000504 RID: 1284
			None,
			// Token: 0x04000505 RID: 1285
			NavMeshVec3,
			// Token: 0x04000506 RID: 1286
			GroundVec3
		}
	}
}
