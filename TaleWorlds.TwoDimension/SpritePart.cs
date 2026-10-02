using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000033 RID: 51
	public class SpritePart
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000093C6 File Offset: 0x000075C6
		// (set) Token: 0x06000243 RID: 579 RVA: 0x000093CE File Offset: 0x000075CE
		public string Name { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000093D7 File Offset: 0x000075D7
		// (set) Token: 0x06000245 RID: 581 RVA: 0x000093DF File Offset: 0x000075DF
		public int Width { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000246 RID: 582 RVA: 0x000093E8 File Offset: 0x000075E8
		// (set) Token: 0x06000247 RID: 583 RVA: 0x000093F0 File Offset: 0x000075F0
		public int Height { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000093F9 File Offset: 0x000075F9
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00009401 File Offset: 0x00007601
		public int SheetID { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000940A File Offset: 0x0000760A
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00009412 File Offset: 0x00007612
		public int SheetX { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000941B File Offset: 0x0000761B
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00009423 File Offset: 0x00007623
		public int SheetY { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000942C File Offset: 0x0000762C
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00009434 File Offset: 0x00007634
		public float MinU { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000943D File Offset: 0x0000763D
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00009445 File Offset: 0x00007645
		public float MinV { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000944E File Offset: 0x0000764E
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00009456 File Offset: 0x00007656
		public float MaxU { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000945F File Offset: 0x0000765F
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00009467 File Offset: 0x00007667
		public float MaxV { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00009470 File Offset: 0x00007670
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00009478 File Offset: 0x00007678
		public int SheetWidth { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009481 File Offset: 0x00007681
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00009489 File Offset: 0x00007689
		public int SheetHeight { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009494 File Offset: 0x00007694
		public Texture Texture
		{
			get
			{
				SpriteCategory category = this._category;
				if (category != null && category.IsLoaded)
				{
					List<Texture> spriteSheets = this._category.SpriteSheets;
					int? num = ((spriteSheets != null) ? new int?(spriteSheets.Count) : null);
					int sheetID = this.SheetID;
					if ((num.GetValueOrDefault() >= sheetID) & (num != null))
					{
						return this._category.SpriteSheets[this.SheetID - 1];
					}
				}
				return null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00009512 File Offset: 0x00007712
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000951A File Offset: 0x0000771A
		public SpriteCategory Category
		{
			get
			{
				return this._category;
			}
			internal set
			{
				this._category = value;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009523 File Offset: 0x00007723
		public SpritePart(string name, SpriteCategory category, int width, int height)
		{
			this.Name = name;
			this.Width = width;
			this.Height = height;
			this._category = category;
			this._category.SpriteParts.Add(this);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000955C File Offset: 0x0000775C
		public void UpdateInitValues()
		{
			Vec2i vec2i = this._category.SheetSizes[this.SheetID - 1];
			this.SheetWidth = vec2i.X;
			this.SheetHeight = vec2i.Y;
			double num = 1.0 / (double)this.SheetWidth;
			double num2 = 1.0 / (double)this.SheetHeight;
			double num3 = (double)this.SheetX * num;
			double num4 = (double)(this.SheetX + this.Width) * num;
			double num5 = (double)this.SheetY * num2;
			double num6 = (double)(this.SheetY + this.Height) * num2;
			this.MinU = (float)num3;
			this.MaxU = (float)num4;
			this.MinV = (float)num5;
			this.MaxV = (float)num6;
		}

		// Token: 0x0400012B RID: 299
		private SpriteCategory _category;
	}
}
