using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E9 RID: 233
	public class ItemImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B9F RID: 2975 RVA: 0x00025832 File Offset: 0x00023A32
		public ItemImageIdentifier(ItemObject item, string bannerCode = "")
		{
			base.Id = ((item != null) ? item.StringId : null) ?? "";
			base.AdditionalArgs = bannerCode;
			base.TextureProviderName = "ItemImageTextureProvider";
		}
	}
}
