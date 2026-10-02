using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattleObjects
{
	// Token: 0x02000029 RID: 41
	public class CustomBattleBannerEffects
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000A922 File Offset: 0x00008B22
		private static CustomBattleBannerEffects Instance
		{
			get
			{
				return CustomGame.Current.CustomBattleBannerEffects;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000A92E File Offset: 0x00008B2E
		public static BannerEffect IncreasedMeleeDamage
		{
			get
			{
				return CustomBattleBannerEffects.Instance._increasedMeleeDamage;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A93A File Offset: 0x00008B3A
		public static BannerEffect IncreasedMeleeDamageAgainstMountedTroops
		{
			get
			{
				return CustomBattleBannerEffects.Instance._increasedMeleeDamageAgainstMountedTroops;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000A946 File Offset: 0x00008B46
		public static BannerEffect IncreasedRangedDamage
		{
			get
			{
				return CustomBattleBannerEffects.Instance._increasedRangedDamage;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000A952 File Offset: 0x00008B52
		public static BannerEffect IncreasedChargeDamage
		{
			get
			{
				return CustomBattleBannerEffects.Instance._increasedChargeDamage;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000A95E File Offset: 0x00008B5E
		public static BannerEffect DecreasedRangedWeaponAccuracy
		{
			get
			{
				return CustomBattleBannerEffects.Instance._decreasedRangedAccuracyPenalty;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000A96A File Offset: 0x00008B6A
		public static BannerEffect DecreasedMoraleShock
		{
			get
			{
				return CustomBattleBannerEffects.Instance._decreasedMoraleShock;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A976 File Offset: 0x00008B76
		public static BannerEffect DecreasedMeleeAttackDamage
		{
			get
			{
				return CustomBattleBannerEffects.Instance._decreasedMeleeAttackDamage;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000A982 File Offset: 0x00008B82
		public static BannerEffect DecreasedRangedAttackDamage
		{
			get
			{
				return CustomBattleBannerEffects.Instance._decreasedRangedAttackDamage;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000A98E File Offset: 0x00008B8E
		public static BannerEffect DecreasedShieldDamage
		{
			get
			{
				return CustomBattleBannerEffects.Instance._decreasedShieldDamage;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000A99A File Offset: 0x00008B9A
		public static BannerEffect IncreasedTroopMovementSpeed
		{
			get
			{
				return CustomBattleBannerEffects.Instance._increasedTroopMovementSpeed;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000A9A6 File Offset: 0x00008BA6
		public static BannerEffect IncreasedMountMovementSpeed
		{
			get
			{
				return CustomBattleBannerEffects.Instance._increasedMountMovementSpeed;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A9B2 File Offset: 0x00008BB2
		public CustomBattleBannerEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A9C0 File Offset: 0x00008BC0
		private void RegisterAll()
		{
			this._increasedMeleeDamage = this.Create("IncreasedMeleeDamage");
			this._increasedMeleeDamageAgainstMountedTroops = this.Create("IncreasedMeleeDamageAgainstMountedTroops");
			this._increasedRangedDamage = this.Create("IncreasedRangedDamage");
			this._increasedChargeDamage = this.Create("IncreasedChargeDamage");
			this._decreasedRangedAccuracyPenalty = this.Create("DecreasedRangedAccuracyPenalty");
			this._decreasedMoraleShock = this.Create("DecreasedMoraleShock");
			this._decreasedMeleeAttackDamage = this.Create("DecreasedMeleeAttackDamage");
			this._decreasedRangedAttackDamage = this.Create("DecreasedRangedAttackDamage");
			this._decreasedShieldDamage = this.Create("DecreasedShieldDamage");
			this._increasedTroopMovementSpeed = this.Create("IncreasedTroopMovementSpeed");
			this._increasedMountMovementSpeed = this.Create("IncreasedMountMovementSpeed");
			this.InitializeAll();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000AA8E File Offset: 0x00008C8E
		private BannerEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BannerEffect>(new BannerEffect(stringId));
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		private void InitializeAll()
		{
			this._increasedMeleeDamage.Initialize("{=unaWKloT}Increased Melee Damage", "{=8ZNOgT8Z}{BONUS_AMOUNT}% melee damage to troops in your formation.", 0.05f, 0.1f, 0.15f, EffectIncrementType.AddFactor);
			this._increasedMeleeDamageAgainstMountedTroops.Initialize("{=2bHoiaoe}Increased Damage Against Mounted Troops", "{=9RZLSV3E}{BONUS_AMOUNT}% damage by melee troops in your formation against cavalry.", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
			this._increasedRangedDamage.Initialize("{=Ch5NpCd0}Increased Ranged Damage", "{=labbKop6}{BONUS_AMOUNT}% ranged damage to troops in your formation.", 0.04f, 0.06f, 0.08f, EffectIncrementType.AddFactor);
			this._decreasedRangedAccuracyPenalty.Initialize("{=MkBPRCuF}Decreased Ranged Accuracy Penalty", "{=Gu0Wxxul}{BONUS_AMOUNT}% accuracy penalty for ranged troops in your formation.", -0.04f, -0.06f, -0.08f, EffectIncrementType.AddFactor);
			this._increasedChargeDamage.Initialize("{=O2oBC9sH}Increased Charge Damage", "{=Z2xgnrDa}{BONUS_AMOUNT}% charge damage to mounted troops in your formation.", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
			this._decreasedMoraleShock.Initialize("{=nOMT0Cw6}Decreased Morale Shock", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.1f, -0.2f, -0.3f, EffectIncrementType.AddFactor);
			this._decreasedMeleeAttackDamage.Initialize("{=a3Vc59WV}Decreased Taken Melee Attack Damage", "{=ORFrCYSn}{BONUS_AMOUNT}% damage by melee attacks to troops in your formation.", -0.05f, -0.1f, -0.15f, EffectIncrementType.AddFactor);
			this._decreasedRangedAttackDamage.Initialize("{=p0JFbL7G}Decreased Taken Ranged Attack Damage", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.05f, -0.1f, -0.15f, EffectIncrementType.AddFactor);
			this._decreasedShieldDamage.Initialize("{=T79exjaP}Decreased Taken Shield Damage", "{=klGEDUmw}{BONUS_AMOUNT}% damage to shields of troops in your formation.", -0.15f, -0.25f, -0.3f, EffectIncrementType.AddFactor);
			this._increasedTroopMovementSpeed.Initialize("{=PbJAOKKZ}Increased Troop Movement Speed", "{=nqWulUTP}{BONUS_AMOUNT}% movement speed to infantry in your formation.", 0.15f, 0.25f, 0.3f, EffectIncrementType.AddFactor);
			this._increasedMountMovementSpeed.Initialize("{=nMfxbc0Y}Increased Mount Movement Speed", "{=g0l7W5xQ}{BONUS_AMOUNT}% movement speed to mounts in your formation.", 0.05f, 0.08f, 0.1f, EffectIncrementType.AddFactor);
		}

		// Token: 0x0400011B RID: 283
		private BannerEffect _increasedMeleeDamage;

		// Token: 0x0400011C RID: 284
		private BannerEffect _increasedMeleeDamageAgainstMountedTroops;

		// Token: 0x0400011D RID: 285
		private BannerEffect _increasedRangedDamage;

		// Token: 0x0400011E RID: 286
		private BannerEffect _increasedChargeDamage;

		// Token: 0x0400011F RID: 287
		private BannerEffect _decreasedRangedAccuracyPenalty;

		// Token: 0x04000120 RID: 288
		private BannerEffect _decreasedMoraleShock;

		// Token: 0x04000121 RID: 289
		private BannerEffect _decreasedMeleeAttackDamage;

		// Token: 0x04000122 RID: 290
		private BannerEffect _decreasedRangedAttackDamage;

		// Token: 0x04000123 RID: 291
		private BannerEffect _decreasedShieldDamage;

		// Token: 0x04000124 RID: 292
		private BannerEffect _increasedTroopMovementSpeed;

		// Token: 0x04000125 RID: 293
		private BannerEffect _increasedMountMovementSpeed;
	}
}
