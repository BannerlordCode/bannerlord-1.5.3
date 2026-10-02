using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002B RID: 43
	public class ImageFit
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000EC69 File Offset: 0x0000CE69
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000EC71 File Offset: 0x0000CE71
		public ImageFit.ImageFitTypes Type { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000EC7A File Offset: 0x0000CE7A
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000EC82 File Offset: 0x0000CE82
		public ImageFit.ImageHorizontalAlignments HorizontalAlignment { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000EC8B File Offset: 0x0000CE8B
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000EC93 File Offset: 0x0000CE93
		public ImageFit.ImageVerticalAlignments VerticalAlignment { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		public float OffsetX { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000ECB5 File Offset: 0x0000CEB5
		public float OffsetY { get; set; }

		// Token: 0x06000349 RID: 841 RVA: 0x0000ECBE File Offset: 0x0000CEBE
		public ImageFit()
		{
			this.Type = ImageFit.ImageFitTypes.StretchToFit;
			this.HorizontalAlignment = ImageFit.ImageHorizontalAlignments.Center;
			this.VerticalAlignment = ImageFit.ImageVerticalAlignments.Center;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public ImageFitResult GetFittedRectangle(in Vector2 containerSize, in Vector2 imageSize)
		{
			switch (this.Type)
			{
			case ImageFit.ImageFitTypes.StretchToFit:
				return new ImageFitResult(0f, 0f, containerSize.X, containerSize.Y);
			case ImageFit.ImageFitTypes.Cover:
				return this.GetRectangleForCover(in containerSize, in imageSize);
			case ImageFit.ImageFitTypes.Contain:
				return this.GetRectangleForContain(in containerSize, in imageSize);
			default:
				Debug.FailedAssert(string.Format("Image fit type not handled: {0}", this.Type), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\ImageFit.cs", "GetFittedRectangle", 55);
				return new ImageFitResult(0f, 0f, containerSize.X, containerSize.Y);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000ED74 File Offset: 0x0000CF74
		private ImageFitResult GetRectangleForCover(in Vector2 containerSize, in Vector2 imageSize)
		{
			float num = containerSize.X / imageSize.X;
			float num2 = containerSize.Y / imageSize.Y;
			float num3 = MathF.Max(num, num2);
			float num4 = imageSize.X * num3;
			float num5 = imageSize.Y * num3;
			Vector2 vector = new Vector2(num4, num5);
			float num6;
			float num7;
			this.GetImageAlignment(in containerSize, in vector, out num6, out num7);
			return new ImageFitResult(num6, num7, num4, num5);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
		private ImageFitResult GetRectangleForContain(in Vector2 containerSize, in Vector2 imageSize)
		{
			float num = containerSize.X / imageSize.X;
			float num2 = containerSize.Y / imageSize.Y;
			float num3 = MathF.Min(num, num2);
			float num4 = imageSize.X * num3;
			float num5 = imageSize.Y * num3;
			Vector2 vector = new Vector2(num4, num5);
			float num6;
			float num7;
			this.GetImageAlignment(in containerSize, in vector, out num6, out num7);
			return new ImageFitResult(num6, num7, num4, num5);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000EE3C File Offset: 0x0000D03C
		private void GetImageAlignment(in Vector2 containerSize, in Vector2 imageSize, out float x, out float y)
		{
			x = 0f;
			y = 0f;
			switch (this.HorizontalAlignment)
			{
			case ImageFit.ImageHorizontalAlignments.Left:
				x = 0f;
				break;
			case ImageFit.ImageHorizontalAlignments.Center:
				x = (containerSize.X - imageSize.X) * 0.5f;
				break;
			case ImageFit.ImageHorizontalAlignments.Right:
				x = containerSize.X - imageSize.X;
				break;
			}
			switch (this.VerticalAlignment)
			{
			case ImageFit.ImageVerticalAlignments.Top:
				y = 0f;
				break;
			case ImageFit.ImageVerticalAlignments.Center:
				y = (containerSize.Y - imageSize.Y) * 0.5f;
				break;
			case ImageFit.ImageVerticalAlignments.Bottom:
				y = containerSize.Y - imageSize.Y;
				break;
			}
			x += this.OffsetX;
			y += this.OffsetY;
		}

		// Token: 0x0200007C RID: 124
		public enum ImageFitTypes : byte
		{
			// Token: 0x04000439 RID: 1081
			StretchToFit,
			// Token: 0x0400043A RID: 1082
			Cover,
			// Token: 0x0400043B RID: 1083
			Contain
		}

		// Token: 0x0200007D RID: 125
		public enum ImageHorizontalAlignments : byte
		{
			// Token: 0x0400043D RID: 1085
			Left,
			// Token: 0x0400043E RID: 1086
			Center,
			// Token: 0x0400043F RID: 1087
			Right
		}

		// Token: 0x0200007E RID: 126
		public enum ImageVerticalAlignments : byte
		{
			// Token: 0x04000441 RID: 1089
			Top,
			// Token: 0x04000442 RID: 1090
			Center,
			// Token: 0x04000443 RID: 1091
			Bottom
		}
	}
}
