using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x0200001D RID: 29
	public class BannerImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00005971 File Offset: 0x00003B71
		public BannerImageIdentifierVM(Banner banner, bool nineGrid = false)
		{
			base.ImageIdentifier = new BannerImageIdentifier(banner, nineGrid);
		}
	}
}
