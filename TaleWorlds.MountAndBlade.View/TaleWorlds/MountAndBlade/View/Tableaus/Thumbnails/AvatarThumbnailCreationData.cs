using System;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003C RID: 60
	public class AvatarThumbnailCreationData : ThumbnailCreationData
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000EEBD File Offset: 0x0000D0BD
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000EEC5 File Offset: 0x0000D0C5
		public string AvatarID { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000EECE File Offset: 0x0000D0CE
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000EED6 File Offset: 0x0000D0D6
		public byte[] AvatarBytes { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000EEDF File Offset: 0x0000D0DF
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000EEE7 File Offset: 0x0000D0E7
		public uint Width { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
		public uint Height { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000EF01 File Offset: 0x0000D101
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000EF09 File Offset: 0x0000D109
		public AvatarData.ImageType ImageType { get; private set; }

		// Token: 0x0600022E RID: 558 RVA: 0x0000EF12 File Offset: 0x0000D112
		public AvatarThumbnailCreationData(string avatarID, byte[] avatarBytes, uint width, uint height, AvatarData.ImageType imageType)
			: base(avatarID, null, null)
		{
			this.AvatarID = avatarID;
			this.AvatarBytes = avatarBytes;
			this.Width = width;
			this.Height = height;
			this.ImageType = imageType;
		}
	}
}
