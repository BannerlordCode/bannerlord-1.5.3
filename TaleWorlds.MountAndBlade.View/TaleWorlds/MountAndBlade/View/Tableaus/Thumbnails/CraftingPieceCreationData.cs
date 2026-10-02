using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000046 RID: 70
	public class CraftingPieceCreationData : ThumbnailCreationData
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00010032 File Offset: 0x0000E232
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0001003A File Offset: 0x0000E23A
		public CraftingPiece CraftingPiece { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00010043 File Offset: 0x0000E243
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0001004B File Offset: 0x0000E24B
		public string Type { get; private set; }

		// Token: 0x06000260 RID: 608 RVA: 0x00010054 File Offset: 0x0000E254
		public CraftingPieceCreationData(CraftingPiece craftingPiece, string type, Action<Texture> setAction, Action cancelAction)
			: base(craftingPiece.StringId + "$" + type, setAction, cancelAction)
		{
			this.CraftingPiece = craftingPiece;
			this.Type = type;
		}
	}
}
