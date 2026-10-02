using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders.ImageIdentifiers
{
	// Token: 0x02000026 RID: 38
	public class CraftingPieceImageTextureProvider : ImageIdentifierTextureProvider
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00009328 File Offset: 0x00007528
		protected override void OnCreateImageWithId(string id, string additionalArgs)
		{
			if (string.IsNullOrEmpty(id))
			{
				base.OnTextureCreated(null);
				return;
			}
			CraftingPiece @object = MBObjectManager.Instance.GetObject<CraftingPiece>(id.Split(new char[] { '$' })[0]);
			if (@object == null)
			{
				Debug.FailedAssert("WRONG CraftingPiece IMAGE IDENTIFIER ID", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\TextureProviders\\ImageIdentifiers\\CraftingPieceImageTextureProvider.cs", "OnCreateImageWithId", 22);
				base.OnTextureCreated(null);
				return;
			}
			base.ThumbnailCreationData = new CraftingPieceCreationData(@object, id.Split(new char[] { '$' })[1], new Action<Texture>(base.OnTextureCreated), new Action(base.OnTextureCreationCancelled));
			ThumbnailCacheManager.Current.CreateTexture(base.ThumbnailCreationData);
		}
	}
}
