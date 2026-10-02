using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FE RID: 254
	public class PawnMuTorere : PawnBase
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0005E8FD File Offset: 0x0005CAFD
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0005E905 File Offset: 0x0005CB05
		public int X { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0005E90E File Offset: 0x0005CB0E
		public override bool IsPlaced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0005E911 File Offset: 0x0005CB11
		public PawnMuTorere(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0005E922 File Offset: 0x0005CB22
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
		}
	}
}
