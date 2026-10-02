using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x0200001E RID: 30
	public class CharacterImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00005986 File Offset: 0x00003B86
		public CharacterImageIdentifierVM(CharacterCode characterCode)
		{
			base.ImageIdentifier = new CharacterImageIdentifier(characterCode);
		}
	}
}
