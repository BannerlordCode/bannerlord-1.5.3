using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000044 RID: 68
	public class CharacterThumbnailCreationData : ThumbnailCreationData
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000FA5E File Offset: 0x0000DC5E
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000FA66 File Offset: 0x0000DC66
		public CharacterCode CharacterCode { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000FA6F File Offset: 0x0000DC6F
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000FA77 File Offset: 0x0000DC77
		public bool IsBig { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000FA80 File Offset: 0x0000DC80
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public int CustomSizeX { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000FA91 File Offset: 0x0000DC91
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000FA99 File Offset: 0x0000DC99
		public int CustomSizeY { get; private set; }

		// Token: 0x06000255 RID: 597 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		public CharacterThumbnailCreationData(CharacterCode characterCode, Action<Texture> setAction, Action cancelAction, bool isBig, int customSizeX = -1, int customSizeY = -1)
			: base("", setAction, cancelAction)
		{
			characterCode.BodyProperties = new BodyProperties(new DynamicBodyProperties((float)((int)characterCode.BodyProperties.Age), (float)((int)characterCode.BodyProperties.Weight), (float)((int)characterCode.BodyProperties.Build)), characterCode.BodyProperties.StaticProperties);
			base.RenderId = characterCode.CreateNewCodeString();
			base.RenderId += (isBig ? "1" : "0");
			if (customSizeX > 0)
			{
				base.RenderId += string.Format("_x:{0}", customSizeX);
			}
			if (customSizeY > 0)
			{
				base.RenderId += string.Format("_y:{0}", customSizeY);
			}
			this.CharacterCode = characterCode;
			this.IsBig = isBig;
			this.CustomSizeX = customSizeX;
			this.CustomSizeY = customSizeY;
		}
	}
}
