using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E7 RID: 231
	public class EmptyImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B96 RID: 2966 RVA: 0x00025790 File Offset: 0x00023990
		public EmptyImageIdentifier()
		{
			base.Id = string.Empty;
			base.AdditionalArgs = string.Empty;
			base.TextureProviderName = string.Empty;
		}
	}
}
