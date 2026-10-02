using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Missions.Multiplayer
{
	// Token: 0x020003F3 RID: 1011
	public readonly struct MultiplayerBattleColors
	{
		// Token: 0x060037E0 RID: 14304 RVA: 0x000E7BF6 File Offset: 0x000E5DF6
		public MultiplayerBattleColors(MultiplayerBattleColors.MultiplayerCultureColorInfo attackerColors, MultiplayerBattleColors.MultiplayerCultureColorInfo defenderColors)
		{
			this.AttackerColors = attackerColors;
			this.DefenderColors = defenderColors;
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000E7C06 File Offset: 0x000E5E06
		public static MultiplayerBattleColors CreateWith(BasicCultureObject attackerCulture, BasicCultureObject defenderCulture)
		{
			return MultiplayerBattleColors.GetCultureColors(attackerCulture, defenderCulture);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000E7C10 File Offset: 0x000E5E10
		public MultiplayerBattleColors.MultiplayerCultureColorInfo GetPeerColors(MissionPeer peer)
		{
			if (peer == null)
			{
				return this.AttackerColors;
			}
			if (this.AttackerColors.Culture == this.DefenderColors.Culture)
			{
				if (peer.Team == null)
				{
					return this.AttackerColors;
				}
				if (peer.Team.Side != BattleSideEnum.Attacker)
				{
					return this.DefenderColors;
				}
				return this.AttackerColors;
			}
			else
			{
				if (peer.Culture != this.AttackerColors.Culture)
				{
					return this.DefenderColors;
				}
				return this.AttackerColors;
			}
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000E7C8C File Offset: 0x000E5E8C
		private static MultiplayerBattleColors GetCultureColors(BasicCultureObject attackerCulture, BasicCultureObject defenderCulture)
		{
			if (attackerCulture == null)
			{
				attackerCulture = MultiplayerBattleColors.GetFallbackCulture();
			}
			if (defenderCulture == null)
			{
				defenderCulture = MultiplayerBattleColors.GetFallbackCulture();
			}
			bool flag = !string.IsNullOrEmpty(attackerCulture.StringId) && !string.IsNullOrEmpty(defenderCulture.StringId) && attackerCulture.StringId == defenderCulture.StringId;
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = new MultiplayerBattleColors.MultiplayerCultureColorInfo(attackerCulture, false);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo2 = new MultiplayerBattleColors.MultiplayerCultureColorInfo(defenderCulture, flag);
			return new MultiplayerBattleColors(multiplayerCultureColorInfo, multiplayerCultureColorInfo2);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000E7CF4 File Offset: 0x000E5EF4
		private static BasicCultureObject GetFallbackCulture()
		{
			MBReadOnlyList<BasicCultureObject> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>();
			if (objectTypeList != null && objectTypeList.Count > 0)
			{
				return objectTypeList.FirstOrDefault<BasicCultureObject>();
			}
			Debug.FailedAssert("No culture objects in the object manager", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MultiplayerBattleColors.cs", "GetFallbackCulture", 114);
			return null;
		}

		// Token: 0x04001827 RID: 6183
		public readonly MultiplayerBattleColors.MultiplayerCultureColorInfo AttackerColors;

		// Token: 0x04001828 RID: 6184
		public readonly MultiplayerBattleColors.MultiplayerCultureColorInfo DefenderColors;

		// Token: 0x020006A7 RID: 1703
		public readonly struct MultiplayerCultureColorInfo
		{
			// Token: 0x06004298 RID: 17048 RVA: 0x00100984 File Offset: 0x000FEB84
			public MultiplayerCultureColorInfo(BasicCultureObject culture, bool swapColors)
			{
				this.Culture = culture;
				this.Color1 = Color.FromUint(this.Color1Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color2) : null) : ((culture != null) ? new uint?(culture.Color) : null)) ?? 0U);
				this.Color2 = Color.FromUint(this.Color2Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color) : null) : ((culture != null) ? new uint?(culture.Color2) : null)) ?? 0U);
				this.ClothingColor1 = Color.FromUint(this.ClothingColor1Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color2) : null) : ((culture != null) ? new uint?(culture.Color) : null)) ?? 0U);
				this.ClothingColor2 = Color.FromUint(this.ClothingColor2Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color) : null) : ((culture != null) ? new uint?(culture.Color2) : null)) ?? 0U);
				this.BannerBackgroundColor = Color.FromUint(this.BannerBackgroundColorUint = (swapColors ? ((culture != null) ? new uint?(culture.BackgroundColor2) : null) : ((culture != null) ? new uint?(culture.BackgroundColor1) : null)) ?? 0U);
				this.BannerForegroundColor = Color.FromUint(this.BannerForegroundColorUint = (swapColors ? ((culture != null) ? new uint?(culture.ForegroundColor2) : null) : ((culture != null) ? new uint?(culture.ForegroundColor1) : null)) ?? 0U);
			}

			// Token: 0x04002358 RID: 9048
			public readonly BasicCultureObject Culture;

			// Token: 0x04002359 RID: 9049
			public readonly Color Color1;

			// Token: 0x0400235A RID: 9050
			public readonly uint Color1Uint;

			// Token: 0x0400235B RID: 9051
			public readonly Color Color2;

			// Token: 0x0400235C RID: 9052
			public readonly uint Color2Uint;

			// Token: 0x0400235D RID: 9053
			public readonly Color ClothingColor1;

			// Token: 0x0400235E RID: 9054
			public readonly uint ClothingColor1Uint;

			// Token: 0x0400235F RID: 9055
			public readonly Color ClothingColor2;

			// Token: 0x04002360 RID: 9056
			public readonly uint ClothingColor2Uint;

			// Token: 0x04002361 RID: 9057
			public readonly Color BannerBackgroundColor;

			// Token: 0x04002362 RID: 9058
			public readonly uint BannerBackgroundColorUint;

			// Token: 0x04002363 RID: 9059
			public readonly Color BannerForegroundColor;

			// Token: 0x04002364 RID: 9060
			public readonly uint BannerForegroundColorUint;
		}
	}
}
