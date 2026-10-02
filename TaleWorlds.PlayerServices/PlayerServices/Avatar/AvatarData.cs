using System;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x0200000A RID: 10
	public class AvatarData
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002E62 File Offset: 0x00001062
		public AvatarData()
		{
			this.Status = AvatarData.DataStatus.NotReady;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E71 File Offset: 0x00001071
		public AvatarData(byte[] image, uint width, uint height)
		{
			this.SetImageData(image, width, height);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002E82 File Offset: 0x00001082
		public AvatarData(byte[] image)
		{
			this.SetImageData(image);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002E91 File Offset: 0x00001091
		public void SetImageData(byte[] image, uint width, uint height)
		{
			this.Image = image;
			this.Width = width;
			this.Height = height;
			this.Type = AvatarData.ImageType.Raw;
			this.Status = AvatarData.DataStatus.Ready;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002EB6 File Offset: 0x000010B6
		public void SetImageData(byte[] image)
		{
			this.Image = image;
			this.Type = AvatarData.ImageType.Image;
			this.Status = AvatarData.DataStatus.Ready;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002ECD File Offset: 0x000010CD
		public void SetFailed()
		{
			this.Status = AvatarData.DataStatus.Failed;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002ED6 File Offset: 0x000010D6
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002EDE File Offset: 0x000010DE
		public byte[] Image { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002EE7 File Offset: 0x000010E7
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002EEF File Offset: 0x000010EF
		public uint Width { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002EF8 File Offset: 0x000010F8
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002F00 File Offset: 0x00001100
		public uint Height { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002F09 File Offset: 0x00001109
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002F11 File Offset: 0x00001111
		public AvatarData.ImageType Type { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002F1A File Offset: 0x0000111A
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002F22 File Offset: 0x00001122
		public AvatarData.DataStatus Status { get; private set; }

		// Token: 0x02000013 RID: 19
		public enum ImageType
		{
			// Token: 0x0400003F RID: 63
			Image,
			// Token: 0x04000040 RID: 64
			Raw
		}

		// Token: 0x02000014 RID: 20
		public enum DataStatus
		{
			// Token: 0x04000042 RID: 66
			NotReady,
			// Token: 0x04000043 RID: 67
			Ready,
			// Token: 0x04000044 RID: 68
			Failed
		}
	}
}
