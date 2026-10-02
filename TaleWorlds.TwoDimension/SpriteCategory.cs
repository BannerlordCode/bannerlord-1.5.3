using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000030 RID: 48
	public class SpriteCategory
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00008577 File Offset: 0x00006777
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000857F File Offset: 0x0000677F
		public string Name { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00008588 File Offset: 0x00006788
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00008590 File Offset: 0x00006790
		public List<SpritePart> SpriteParts { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00008599 File Offset: 0x00006799
		// (set) Token: 0x06000218 RID: 536 RVA: 0x000085A1 File Offset: 0x000067A1
		public List<SpritePart> SortedSpritePartList { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000085AA File Offset: 0x000067AA
		// (set) Token: 0x0600021A RID: 538 RVA: 0x000085B2 File Offset: 0x000067B2
		public List<Texture> SpriteSheets { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600021B RID: 539 RVA: 0x000085BB File Offset: 0x000067BB
		// (set) Token: 0x0600021C RID: 540 RVA: 0x000085C3 File Offset: 0x000067C3
		public int SpriteSheetCount { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000085CC File Offset: 0x000067CC
		// (set) Token: 0x0600021E RID: 542 RVA: 0x000085D4 File Offset: 0x000067D4
		public bool IsLoaded { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000085DD File Offset: 0x000067DD
		// (set) Token: 0x06000220 RID: 544 RVA: 0x000085E5 File Offset: 0x000067E5
		public bool IsPartiallyLoaded { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000085EE File Offset: 0x000067EE
		// (set) Token: 0x06000222 RID: 546 RVA: 0x000085F6 File Offset: 0x000067F6
		public Vec2i[] SheetSizes { get; set; }

		// Token: 0x06000223 RID: 547 RVA: 0x00008600 File Offset: 0x00006800
		public SpriteCategory(string name, int spriteSheetCount, bool alwaysLoad = false)
		{
			this.Name = name;
			this.SpriteSheetCount = spriteSheetCount;
			this.AlwaysLoad = alwaysLoad;
			this.SpriteSheets = new List<Texture>();
			this.SpriteParts = new List<SpritePart>();
			this.SortedSpritePartList = new List<SpritePart>();
			this.SheetSizes = new Vec2i[spriteSheetCount];
			this._spritePartComparer = new SpriteCategory.SpriteSizeComparer();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008660 File Offset: 0x00006860
		public void Load(ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot)
		{
			if (!this.IsLoaded)
			{
				this.IsLoaded = true;
				this.IsPartiallyLoaded = false;
				for (int i = 1; i <= this.SpriteSheetCount; i++)
				{
					Texture texture = resourceContext.LoadTexture(resourceDepot, string.Concat(new object[] { "SpriteSheets\\", this.Name, "\\", this.Name, "_", i }));
					this.SpriteSheets.Add(texture);
				}
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000086E8 File Offset: 0x000068E8
		public void Unload()
		{
			if (this.IsLoaded)
			{
				this.SpriteSheets.ForEach(delegate(Texture s)
				{
					s.PlatformTexture.Release();
				});
				this.SpriteSheets.Clear();
				this.IsLoaded = false;
				this.IsPartiallyLoaded = false;
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008740 File Offset: 0x00006940
		public void Reload(ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot, SpriteCategory newCategoryInfo)
		{
			if (this.IsLoaded)
			{
				this.SpriteParts = newCategoryInfo.SpriteParts;
				this.SheetSizes = newCategoryInfo.SheetSizes;
				this.SortList();
				if (this.IsPartiallyLoaded)
				{
					List<int> list = new List<int>();
					for (int i = 0; i < this.SpriteSheetCount; i++)
					{
						if (this.SpriteSheets[i] != null)
						{
							list.Add(i + 1);
							this.PartialUnloadAtIndex(i + 1);
						}
					}
					for (int j = 0; j < list.Count; j++)
					{
						this.PartialLoadAtIndex(resourceContext, resourceDepot, list[j]);
					}
					return;
				}
				this.Unload();
				this.Load(resourceContext, resourceDepot);
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000087E4 File Offset: 0x000069E4
		public void InitializePartialLoad()
		{
			if (!this.IsLoaded)
			{
				this.IsLoaded = true;
				this.IsPartiallyLoaded = true;
				for (int i = 1; i <= this.SpriteSheetCount; i++)
				{
					this.SpriteSheets.Add(null);
				}
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008824 File Offset: 0x00006A24
		public void ReleasePartialLoad()
		{
			if (this.IsLoaded)
			{
				for (int i = 1; i <= this.SpriteSheetCount; i++)
				{
					this.PartialUnloadAtIndex(i);
				}
				this.SpriteSheets.Clear();
				this.IsLoaded = false;
				this.IsPartiallyLoaded = false;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000886C File Offset: 0x00006A6C
		public void PartialLoadAtIndex(ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot, int sheetIndex)
		{
			if (sheetIndex >= 1 && sheetIndex <= this.SpriteSheetCount && this.IsLoaded && this.SpriteSheets[sheetIndex - 1] == null)
			{
				Texture texture = resourceContext.LoadTexture(resourceDepot, string.Concat(new object[] { "SpriteSheets\\", this.Name, "\\", this.Name, "_", sheetIndex }));
				this.SpriteSheets[sheetIndex - 1] = texture;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000088F4 File Offset: 0x00006AF4
		public void PartialUnloadAtIndex(int sheetIndex)
		{
			if (sheetIndex >= 1 && sheetIndex <= this.SpriteSheetCount && this.IsLoaded && this.SpriteSheets[sheetIndex - 1] != null)
			{
				this.SpriteSheets[sheetIndex - 1].PlatformTexture.Release();
				this.SpriteSheets[sheetIndex - 1] = null;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000894D File Offset: 0x00006B4D
		public void SortList()
		{
			this.SortedSpritePartList.Clear();
			this.SortedSpritePartList.AddRange(this.SpriteParts);
			this.SortedSpritePartList.Sort(this._spritePartComparer);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000897C File Offset: 0x00006B7C
		public bool IsCategoryFullyLoaded()
		{
			for (int i = 0; i < this.SpriteSheets.Count; i++)
			{
				Texture texture = this.SpriteSheets[i];
				if (texture == null || !texture.IsLoaded())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400010F RID: 271
		public const int SpriteSheetSize = 4096;

		// Token: 0x04000117 RID: 279
		public readonly bool AlwaysLoad;

		// Token: 0x04000119 RID: 281
		private SpriteCategory.SpriteSizeComparer _spritePartComparer;

		// Token: 0x02000044 RID: 68
		protected class SpriteSizeComparer : IComparer<SpritePart>
		{
			// Token: 0x060002C7 RID: 711 RVA: 0x0000A95F File Offset: 0x00008B5F
			public int Compare(SpritePart x, SpritePart y)
			{
				return y.Width * y.Height - x.Width * x.Height;
			}
		}
	}
}
