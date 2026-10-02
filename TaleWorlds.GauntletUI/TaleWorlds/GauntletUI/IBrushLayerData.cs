using System;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000028 RID: 40
	public interface IBrushLayerData
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002F9 RID: 761
		// (set) Token: 0x060002FA RID: 762
		string Name { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002FB RID: 763
		// (set) Token: 0x060002FC RID: 764
		Sprite Sprite { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002FD RID: 765
		// (set) Token: 0x060002FE RID: 766
		Color Color { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002FF RID: 767
		// (set) Token: 0x06000300 RID: 768
		float ColorFactor { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000301 RID: 769
		// (set) Token: 0x06000302 RID: 770
		float AlphaFactor { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000303 RID: 771
		// (set) Token: 0x06000304 RID: 772
		float HueFactor { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000305 RID: 773
		// (set) Token: 0x06000306 RID: 774
		float SaturationFactor { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000307 RID: 775
		// (set) Token: 0x06000308 RID: 776
		float ValueFactor { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000309 RID: 777
		// (set) Token: 0x0600030A RID: 778
		bool IsHidden { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600030B RID: 779
		// (set) Token: 0x0600030C RID: 780
		float XOffset { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600030D RID: 781
		// (set) Token: 0x0600030E RID: 782
		float YOffset { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600030F RID: 783
		// (set) Token: 0x06000310 RID: 784
		float Rotation { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000311 RID: 785
		// (set) Token: 0x06000312 RID: 786
		float ExtendLeft { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000313 RID: 787
		// (set) Token: 0x06000314 RID: 788
		float ExtendRight { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000315 RID: 789
		// (set) Token: 0x06000316 RID: 790
		float ExtendTop { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000317 RID: 791
		// (set) Token: 0x06000318 RID: 792
		float ExtendBottom { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000319 RID: 793
		// (set) Token: 0x0600031A RID: 794
		float OverridenWidth { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600031B RID: 795
		// (set) Token: 0x0600031C RID: 796
		float OverridenHeight { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600031D RID: 797
		// (set) Token: 0x0600031E RID: 798
		BrushLayerSizePolicy WidthPolicy { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600031F RID: 799
		// (set) Token: 0x06000320 RID: 800
		BrushLayerSizePolicy HeightPolicy { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000321 RID: 801
		// (set) Token: 0x06000322 RID: 802
		bool HorizontalFlip { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000323 RID: 803
		// (set) Token: 0x06000324 RID: 804
		bool VerticalFlip { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000325 RID: 805
		// (set) Token: 0x06000326 RID: 806
		bool UseOverlayAlphaAsMask { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000327 RID: 807
		// (set) Token: 0x06000328 RID: 808
		BrushOverlayMethod OverlayMethod { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000329 RID: 809
		// (set) Token: 0x0600032A RID: 810
		Sprite OverlaySprite { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600032B RID: 811
		// (set) Token: 0x0600032C RID: 812
		float OverlayXOffset { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600032D RID: 813
		// (set) Token: 0x0600032E RID: 814
		float OverlayYOffset { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600032F RID: 815
		// (set) Token: 0x06000330 RID: 816
		bool UseRandomBaseOverlayXOffset { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000331 RID: 817
		// (set) Token: 0x06000332 RID: 818
		bool UseRandomBaseOverlayYOffset { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000333 RID: 819
		// (set) Token: 0x06000334 RID: 820
		ImageFit.ImageFitTypes ImageFitType { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000335 RID: 821
		// (set) Token: 0x06000336 RID: 822
		ImageFit.ImageHorizontalAlignments ImageFitHorizontalAlignment { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000337 RID: 823
		// (set) Token: 0x06000338 RID: 824
		ImageFit.ImageVerticalAlignments ImageFitVerticalAlignment { get; set; }

		// Token: 0x06000339 RID: 825
		float GetValueAsFloat(BrushAnimationProperty.BrushAnimationPropertyType propertyType);

		// Token: 0x0600033A RID: 826
		Color GetValueAsColor(BrushAnimationProperty.BrushAnimationPropertyType propertyType);

		// Token: 0x0600033B RID: 827
		Sprite GetValueAsSprite(BrushAnimationProperty.BrushAnimationPropertyType propertyType);
	}
}
