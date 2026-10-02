using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E5 RID: 229
	public class CharacterImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B94 RID: 2964 RVA: 0x00025718 File Offset: 0x00023918
		public CharacterImageIdentifier(CharacterCode characterCode)
		{
			base.Id = ((characterCode != null) ? characterCode.Code : null) ?? "";
			base.AdditionalArgs = "";
			base.TextureProviderName = "CharacterImageTextureProvider";
		}
	}
}
