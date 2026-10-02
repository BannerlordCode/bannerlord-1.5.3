using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000048 RID: 72
	public class ItemThumbnailCreationData : ThumbnailCreationData
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0001133C File Offset: 0x0000F53C
		// (set) Token: 0x0600026C RID: 620 RVA: 0x00011344 File Offset: 0x0000F544
		public ItemObject ItemObject { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0001134D File Offset: 0x0000F54D
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00011355 File Offset: 0x0000F555
		public string AdditionalArgs { get; private set; }

		// Token: 0x0600026F RID: 623 RVA: 0x0001135E File Offset: 0x0000F55E
		public ItemThumbnailCreationData(ItemObject itemObject, string additionalArgs, Action<Texture> setAction, Action cancelAction)
			: base(itemObject.StringId, setAction, cancelAction)
		{
			this.ItemObject = itemObject;
			this.AdditionalArgs = additionalArgs;
		}
	}
}
