using System;
using TaleWorlds.Core;

namespace StoryMode.StoryModeObjects
{
	// Token: 0x02000017 RID: 23
	public class StoryModeBannerEffects
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005487 File Offset: 0x00003687
		public static BannerEffect DragonBannerEffect
		{
			get
			{
				return StoryModeManager.Current.StoryModeBannerEffects._dragonBannerEffect;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005498 File Offset: 0x00003698
		public StoryModeBannerEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000054A6 File Offset: 0x000036A6
		private void RegisterAll()
		{
			this._dragonBannerEffect = this.Create("dragon_banner_effect");
			this.InitializeAll();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000054BF File Offset: 0x000036BF
		private BannerEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BannerEffect>(new BannerEffect(stringId));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000054D6 File Offset: 0x000036D6
		private void InitializeAll()
		{
			this._dragonBannerEffect.Initialize("{=!}Not Implemented.", "{=!}Not Implemented.", 0f, 0f, 0f, EffectIncrementType.Invalid);
		}

		// Token: 0x04000040 RID: 64
		private const string NotImplementedText = "{=!}Not Implemented.";

		// Token: 0x04000041 RID: 65
		private BannerEffect _dragonBannerEffect;
	}
}
