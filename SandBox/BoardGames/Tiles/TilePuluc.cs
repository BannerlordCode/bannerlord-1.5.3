using System;
using SandBox.BoardGames.Objects;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000FA RID: 250
	public class TilePuluc : Tile1D
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0005DEE1 File Offset: 0x0005C0E1
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x0005DEE9 File Offset: 0x0005C0E9
		public Vec3 PosLeft { get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x0005DEF2 File Offset: 0x0005C0F2
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x0005DEFA File Offset: 0x0005C0FA
		public Vec3 PosLeftMid { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0005DF03 File Offset: 0x0005C103
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x0005DF0B File Offset: 0x0005C10B
		public Vec3 PosRight { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0005DF14 File Offset: 0x0005C114
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0005DF1C File Offset: 0x0005C11C
		public Vec3 PosRightMid { get; private set; }

		// Token: 0x06000C96 RID: 3222 RVA: 0x0005DF25 File Offset: 0x0005C125
		public TilePuluc(GameEntity entity, BoardGameDecal decal, int x)
			: base(entity, decal, x)
		{
			this.UpdateTilePosition();
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0005DF38 File Offset: 0x0005C138
		public void UpdateTilePosition()
		{
			MatrixFrame globalFrame = base.Entity.GetGlobalFrame();
			MetaMesh tileMesh = base.Entity.GetFirstScriptOfType<Tile>().TileMesh;
			Vec3 vec = tileMesh.GetBoundingBox().max - tileMesh.GetBoundingBox().min;
			Mat3 mat = globalFrame.rotation.TransformToParent(in tileMesh.Frame.rotation);
			Vec3 vec2 = new Vec3(0f, vec.y / 6f, 0f, -1f);
			Vec3 vec3 = mat.TransformToParent(in vec2);
			vec2 = new Vec3(0f, vec.y / 3f, 0f, -1f);
			Vec3 vec4 = mat.TransformToParent(in vec2);
			Vec3 globalPosition = base.Entity.GlobalPosition;
			this.PosLeft = globalPosition + vec4;
			this.PosLeftMid = globalPosition + vec3;
			this.PosRight = globalPosition - vec4;
			this.PosRightMid = globalPosition - vec3;
		}
	}
}
