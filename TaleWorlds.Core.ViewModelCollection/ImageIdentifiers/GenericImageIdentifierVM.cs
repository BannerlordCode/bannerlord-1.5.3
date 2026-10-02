using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x02000020 RID: 32
	public class GenericImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x000059AF File Offset: 0x00003BAF
		public GenericImageIdentifierVM(ImageIdentifier imageIdentifier)
		{
			if (imageIdentifier == null)
			{
				base.ImageIdentifier = new EmptyImageIdentifier();
				return;
			}
			base.ImageIdentifier = imageIdentifier;
		}
	}
}
