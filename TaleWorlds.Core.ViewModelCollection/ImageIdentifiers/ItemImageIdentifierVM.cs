using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x02000022 RID: 34
	public class ItemImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00005B1C File Offset: 0x00003D1C
		public ItemImageIdentifierVM(ItemObject itemObject, string bannerCode = "")
		{
			this._itemObject = itemObject;
			this._bannerCode = bannerCode;
			base.ImageIdentifier = new ItemImageIdentifier(this._itemObject, this._bannerCode);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005B49 File Offset: 0x00003D49
		public ItemImageIdentifierVM Clone()
		{
			return new ItemImageIdentifierVM(this._itemObject, this._bannerCode);
		}

		// Token: 0x040000AB RID: 171
		private readonly ItemObject _itemObject;

		// Token: 0x040000AC RID: 172
		private readonly string _bannerCode;
	}
}
