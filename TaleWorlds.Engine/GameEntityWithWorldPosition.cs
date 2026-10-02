using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000050 RID: 80
	public class GameEntityWithWorldPosition
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x00006858 File Offset: 0x00004A58
		public GameEntityWithWorldPosition(WeakGameEntity gameEntity)
		{
			this._customLocalFrame = MatrixFrame.Identity;
			this._gameEntity = gameEntity;
			Scene scene = gameEntity.Scene;
			float groundHeightAtPosition = scene.GetGroundHeightAtPosition(gameEntity.GlobalPosition, BodyFlags.CommonCollisionExcludeFlags);
			this._worldPosition = new WorldPosition(scene, UIntPtr.Zero, new Vec3(gameEntity.GlobalPosition.AsVec2, groundHeightAtPosition, -1f), false);
			this._worldPosition.GetGroundVec3();
			this._orthonormalRotation = gameEntity.GetGlobalFrame().rotation;
			this._orthonormalRotation.Orthonormalize();
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x000068ED File Offset: 0x00004AED
		public WeakGameEntity GameEntity
		{
			get
			{
				return this._gameEntity;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x000068F5 File Offset: 0x00004AF5
		public WorldPosition WorldPosition
		{
			get
			{
				this.ValidateWorldPosition();
				return this._worldPosition;
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00006904 File Offset: 0x00004B04
		private void ValidateWorldPosition()
		{
			Vec3 vec = (this._customLocalFrame.IsIdentity ? this.GameEntity.GetGlobalFrame().origin : this.GameEntity.GetGlobalFrame().TransformToParent(in this._customLocalFrame).origin);
			if (!this._worldPosition.AsVec2.NearlyEquals(vec.AsVec2, 1E-05f))
			{
				this._worldPosition.SetVec3(UIntPtr.Zero, vec, false);
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00006988 File Offset: 0x00004B88
		public void InvalidateWorldPosition()
		{
			this._worldPosition.State = ZValidityState.Invalid;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00006998 File Offset: 0x00004B98
		public WorldFrame WorldFrame
		{
			get
			{
				Mat3 mat = (this._customLocalFrame.rotation.IsIdentity() ? this.GameEntity.GetGlobalFrame().rotation : this.GameEntity.GetGlobalFrame().rotation.TransformToParent(in this._customLocalFrame.rotation));
				if (!mat.NearlyEquals(in this._orthonormalRotation, 1E-05f))
				{
					this._orthonormalRotation = mat;
					this._orthonormalRotation.Orthonormalize();
				}
				return new WorldFrame(this._orthonormalRotation, this.WorldPosition);
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00006A2A File Offset: 0x00004C2A
		public void SetCustomLocalFrame(in MatrixFrame customLocalFrame)
		{
			this._customLocalFrame = customLocalFrame;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00006A38 File Offset: 0x00004C38
		public Vec2 AsVec2
		{
			get
			{
				this.ValidateWorldPosition();
				return this._worldPosition.AsVec2;
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00006A4B File Offset: 0x00004C4B
		public UIntPtr GetNavMesh()
		{
			this.ValidateWorldPosition();
			return this._worldPosition.GetNavMesh();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00006A5E File Offset: 0x00004C5E
		public Vec3 GetNavMeshVec3()
		{
			this.ValidateWorldPosition();
			return this._worldPosition.GetNavMeshVec3();
		}

		// Token: 0x040000B3 RID: 179
		private MatrixFrame _customLocalFrame;

		// Token: 0x040000B4 RID: 180
		private readonly WeakGameEntity _gameEntity;

		// Token: 0x040000B5 RID: 181
		private WorldPosition _worldPosition;

		// Token: 0x040000B6 RID: 182
		private Mat3 _orthonormalRotation;
	}
}
