using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B6 RID: 438
	[ScriptingInterfaceBase]
	internal interface IMBGameEntityExtensions
	{
		// Token: 0x06001915 RID: 6421
		[EngineMethod("create_from_weapon", false, null, false)]
		GameEntity CreateFromWeapon(UIntPtr scenePointer, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, in WeaponData ammoWeaponData, WeaponStatsData[] ammoWeaponStatsData, int ammoWeaponStatsDataLength, bool showHolsterWithWeapon);

		// Token: 0x06001916 RID: 6422
		[EngineMethod("fade_out", false, null, false)]
		void FadeOut(UIntPtr entityPointer, float interval, bool isRemovingFromScene);

		// Token: 0x06001917 RID: 6423
		[EngineMethod("fade_in", false, null, false)]
		void FadeIn(UIntPtr entityPointer, bool resetAlpha);

		// Token: 0x06001918 RID: 6424
		[EngineMethod("hide_if_not_fading_out", false, null, false)]
		void HideIfNotFadingOut(UIntPtr entityPointer);
	}
}
