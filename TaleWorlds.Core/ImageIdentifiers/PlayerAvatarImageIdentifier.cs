using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000EA RID: 234
	public class PlayerAvatarImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000BA0 RID: 2976 RVA: 0x00025867 File Offset: 0x00023A67
		public PlayerAvatarImageIdentifier(PlayerId playerId, int forcedAvatarIndex)
		{
			base.Id = playerId.ToString();
			base.AdditionalArgs = string.Format("{0}", forcedAvatarIndex);
			base.TextureProviderName = "PlayerAvatarImageTextureProvider";
		}
	}
}
