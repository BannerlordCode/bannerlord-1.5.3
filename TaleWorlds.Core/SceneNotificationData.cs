using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x020000CB RID: 203
	public class SceneNotificationData
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x000239FD File Offset: 0x00021BFD
		public virtual string SceneID { get; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00023A05 File Offset: 0x00021C05
		public virtual string SoundEventPath { get; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00023A0D File Offset: 0x00021C0D
		public virtual TextObject TitleText { get; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00023A15 File Offset: 0x00021C15
		public virtual TextObject DescriptionText { get; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00023A1D File Offset: 0x00021C1D
		public virtual TextObject AffirmativeDescriptionText { get; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00023A25 File Offset: 0x00021C25
		public virtual TextObject NegativeDescriptionText { get; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00023A2D File Offset: 0x00021C2D
		public virtual TextObject AffirmativeHintText { get; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00023A35 File Offset: 0x00021C35
		public virtual TextObject AffirmativeHintTextExtended { get; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00023A3D File Offset: 0x00021C3D
		public virtual TextObject AffirmativeTitleText { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00023A45 File Offset: 0x00021C45
		public virtual TextObject NegativeTitleText { get; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x00023A4D File Offset: 0x00021C4D
		public virtual TextObject AffirmativeText { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00023A55 File Offset: 0x00021C55
		public virtual TextObject NegativeText { get; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00023A5D File Offset: 0x00021C5D
		public virtual bool IsAffirmativeOptionShown { get; }

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00023A65 File Offset: 0x00021C65
		public virtual bool IsNegativeOptionShown { get; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00023A6D File Offset: 0x00021C6D
		public virtual bool ShouldAutoConfirm { get; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00023A75 File Offset: 0x00021C75
		public virtual bool PauseActiveState { get; } = true;

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00023A7D File Offset: 0x00021C7D
		public virtual SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00023A85 File Offset: 0x00021C85
		public virtual SceneNotificationData.NotificationSceneProperties SceneProperties { get; } = new SceneNotificationData.NotificationSceneProperties
		{
			InitializePhysics = false,
			DisableStaticShadows = false,
			OverriddenWaterStrength = null
		};

		// Token: 0x06000B02 RID: 2818 RVA: 0x00023A8D File Offset: 0x00021C8D
		public virtual void OnAffirmativeAction()
		{
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00023A8F File Offset: 0x00021C8F
		public virtual void OnNegativeAction()
		{
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00023A91 File Offset: 0x00021C91
		public virtual void OnCloseAction()
		{
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00023A93 File Offset: 0x00021C93
		public virtual Banner[] GetBanners()
		{
			return Array.Empty<Banner>();
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00023A9A File Offset: 0x00021C9A
		public virtual SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationCharacter>();
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00023AA1 File Offset: 0x00021CA1
		public virtual SceneNotificationData.SceneNotificationShip[] GetShips()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationShip>();
		}

		// Token: 0x02000129 RID: 297
		public readonly struct SceneNotificationCharacter
		{
			// Token: 0x06000C2B RID: 3115 RVA: 0x00026BF8 File Offset: 0x00024DF8
			public SceneNotificationCharacter(BasicCharacterObject character, Equipment overriddenEquipment = null, BodyProperties overriddenBodyProperties = default(BodyProperties), bool useCivilianEquipment = false, uint customColor1 = 4294967295U, uint customColor2 = 4294967295U, bool useHorse = false)
			{
				this.Character = character;
				this.OverriddenEquipment = overriddenEquipment;
				this.OverriddenBodyProperties = overriddenBodyProperties;
				this.UseCivilianEquipment = useCivilianEquipment;
				this.CustomColor1 = customColor1;
				this.CustomColor2 = customColor2;
				this.UseHorse = useHorse;
			}

			// Token: 0x040007D4 RID: 2004
			public readonly BasicCharacterObject Character;

			// Token: 0x040007D5 RID: 2005
			public readonly Equipment OverriddenEquipment;

			// Token: 0x040007D6 RID: 2006
			public readonly BodyProperties OverriddenBodyProperties;

			// Token: 0x040007D7 RID: 2007
			public readonly bool UseCivilianEquipment;

			// Token: 0x040007D8 RID: 2008
			public readonly bool UseHorse;

			// Token: 0x040007D9 RID: 2009
			public readonly uint CustomColor1;

			// Token: 0x040007DA RID: 2010
			public readonly uint CustomColor2;
		}

		// Token: 0x0200012A RID: 298
		public readonly struct SceneNotificationShip
		{
			// Token: 0x06000C2C RID: 3116 RVA: 0x00026C2F File Offset: 0x00024E2F
			public SceneNotificationShip(string shipPrefabId, List<ShipVisualSlotInfo> shipUpgrades, float shipHitPointRatio, uint sailColor1, uint sailColor2, int shipSeed)
			{
				this.ShipPrefabId = shipPrefabId;
				this.ShipUpgrades = shipUpgrades;
				this.ShipHitPointRatio = shipHitPointRatio;
				this.SailColor1 = sailColor1;
				this.SailColor2 = sailColor2;
				this.ShipSeed = shipSeed;
			}

			// Token: 0x040007DB RID: 2011
			public readonly string ShipPrefabId;

			// Token: 0x040007DC RID: 2012
			public readonly List<ShipVisualSlotInfo> ShipUpgrades;

			// Token: 0x040007DD RID: 2013
			public readonly float ShipHitPointRatio;

			// Token: 0x040007DE RID: 2014
			public readonly uint SailColor1;

			// Token: 0x040007DF RID: 2015
			public readonly uint SailColor2;

			// Token: 0x040007E0 RID: 2016
			public readonly int ShipSeed;
		}

		// Token: 0x0200012B RID: 299
		public struct NotificationSceneProperties
		{
			// Token: 0x040007E1 RID: 2017
			public bool InitializePhysics;

			// Token: 0x040007E2 RID: 2018
			public bool DisableStaticShadows;

			// Token: 0x040007E3 RID: 2019
			public float? OverriddenWaterStrength;
		}

		// Token: 0x0200012C RID: 300
		public enum RelevantContextType
		{
			// Token: 0x040007E5 RID: 2021
			Any,
			// Token: 0x040007E6 RID: 2022
			MPLobby,
			// Token: 0x040007E7 RID: 2023
			CustomBattle,
			// Token: 0x040007E8 RID: 2024
			Mission,
			// Token: 0x040007E9 RID: 2025
			Map
		}
	}
}
