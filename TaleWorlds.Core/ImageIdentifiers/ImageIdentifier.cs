using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E8 RID: 232
	public abstract class ImageIdentifier
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x000257B9 File Offset: 0x000239B9
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x000257C1 File Offset: 0x000239C1
		public string Id { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x000257CA File Offset: 0x000239CA
		// (set) Token: 0x06000B9A RID: 2970 RVA: 0x000257D2 File Offset: 0x000239D2
		public string TextureProviderName { get; protected set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x000257DB File Offset: 0x000239DB
		// (set) Token: 0x06000B9C RID: 2972 RVA: 0x000257E3 File Offset: 0x000239E3
		public string AdditionalArgs { get; protected set; }

		// Token: 0x06000B9D RID: 2973 RVA: 0x000257EC File Offset: 0x000239EC
		public bool Equals(ImageIdentifier other)
		{
			return other != null && this.Id.Equals(other.Id) && this.AdditionalArgs.Equals(other.AdditionalArgs) && this.TextureProviderName.Equals(other.TextureProviderName);
		}
	}
}
