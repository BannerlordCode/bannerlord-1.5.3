using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004B RID: 75
	public struct TextureCreationInfo
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000274 RID: 628 RVA: 0x000113C4 File Offset: 0x0000F5C4
		public bool IsSuccess
		{
			get
			{
				return this.IsValid && (this.CreatedNewTexture || this.UsingExistingTexture);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000113E0 File Offset: 0x0000F5E0
		public bool IsFail
		{
			get
			{
				return this.IsValid && !this.CreatedNewTexture && !this.UsingExistingTexture;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00011400 File Offset: 0x0000F600
		public static TextureCreationInfo WithNewTexture(Texture texture = null)
		{
			return new TextureCreationInfo
			{
				IsValid = true,
				CreatedNewTexture = true,
				Texture = texture
			};
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00011430 File Offset: 0x0000F630
		public static TextureCreationInfo WithExistingTexture(Texture texture)
		{
			return new TextureCreationInfo
			{
				IsValid = true,
				UsingExistingTexture = true,
				Texture = texture
			};
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00011460 File Offset: 0x0000F660
		public static TextureCreationInfo Fail()
		{
			return new TextureCreationInfo
			{
				IsValid = true
			};
		}

		// Token: 0x0400014E RID: 334
		public bool IsValid;

		// Token: 0x0400014F RID: 335
		public bool CreatedNewTexture;

		// Token: 0x04000150 RID: 336
		public bool UsingExistingTexture;

		// Token: 0x04000151 RID: 337
		public Texture Texture;
	}
}
