using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200004F RID: 79
	public class DefaultBannerEffects
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00015D2C File Offset: 0x00013F2C
		private static DefaultBannerEffects Instance
		{
			get
			{
				return Game.Current.DefaultBannerEffects;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00015D38 File Offset: 0x00013F38
		public static BannerEffect IncreasedMeleeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMeleeDamage;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00015D44 File Offset: 0x00013F44
		public static BannerEffect IncreasedMeleeDamageAgainstMountedTroops
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMeleeDamageAgainstMountedTroops;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00015D50 File Offset: 0x00013F50
		public static BannerEffect IncreasedRangedDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedRangedDamage;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00015D5C File Offset: 0x00013F5C
		public static BannerEffect IncreasedChargeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedChargeDamage;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00015D68 File Offset: 0x00013F68
		public static BannerEffect DecreasedChargeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedChargeDamage;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00015D74 File Offset: 0x00013F74
		public static BannerEffect DecreasedRangedAccuracyPenalty
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedRangedAccuracyPenalty;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00015D80 File Offset: 0x00013F80
		public static BannerEffect DecreasedMoraleShock
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedMoraleShock;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00015D8C File Offset: 0x00013F8C
		public static BannerEffect DecreasedMeleeAttackDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedMeleeAttackDamage;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00015D98 File Offset: 0x00013F98
		public static BannerEffect DecreasedRangedAttackDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedRangedAttackDamage;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00015DA4 File Offset: 0x00013FA4
		public static BannerEffect DecreasedShieldDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedShieldDamage;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00015DB0 File Offset: 0x00013FB0
		public static BannerEffect IncreasedTroopMovementSpeed
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedTroopMovementSpeed;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00015DBC File Offset: 0x00013FBC
		public static BannerEffect IncreasedMountMovementSpeed
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMountMovementSpeed;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00015DC8 File Offset: 0x00013FC8
		public static BannerEffect IncreasedMoraleShockByMeleeTroops
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMoraleShockByMeleeTroops;
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00015DD4 File Offset: 0x00013FD4
		public DefaultBannerEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00015DE4 File Offset: 0x00013FE4
		private void RegisterAll()
		{
			this._increasedMeleeDamage = this.Create("IncreasedMeleeDamage");
			this._increasedMeleeDamageAgainstMountedTroops = this.Create("IncreasedMeleeDamageAgainstMountedTroops");
			this._increasedRangedDamage = this.Create("IncreasedRangedDamage");
			this._increasedChargeDamage = this.Create("IncreasedChargeDamage");
			this._decreasedChargeDamage = this.Create("DecreasedChargeDamage");
			this._decreasedRangedAccuracyPenalty = this.Create("DecreasedRangedAccuracyPenalty");
			this._decreasedMoraleShock = this.Create("DecreasedMoraleShock");
			this._decreasedMeleeAttackDamage = this.Create("DecreasedMeleeAttackDamage");
			this._decreasedRangedAttackDamage = this.Create("DecreasedRangedAttackDamage");
			this._decreasedShieldDamage = this.Create("DecreasedShieldDamage");
			this._increasedTroopMovementSpeed = this.Create("IncreasedTroopMovementSpeed");
			this._increasedMountMovementSpeed = this.Create("IncreasedMountMovementSpeed");
			this._increasedMoraleShockByMeleeTroops = this.Create("IncreasedMoraleShockByMeleeTroops");
			this.InitializeAll();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00015ED4 File Offset: 0x000140D4
		private BannerEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BannerEffect>(new BannerEffect(stringId));
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00015EEC File Offset: 0x000140EC
		private void InitializeAll()
		{
			this._increasedMeleeDamage.Initialize("{=unaWKloT}Increased Melee Damage", "{=8ZNOgT8Z}{BONUS_AMOUNT}% melee damage to troops in your formation.", 0.05f, 0.1f, 0.15f, EffectIncrementType.AddFactor);
			this._increasedMeleeDamageAgainstMountedTroops.Initialize("{=t0Qzb7CY}Increased Melee Damage Against Mounted Troops", "{=sxGmF0tC}{BONUS_AMOUNT}% melee damage by troops in your formation against cavalry.", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
			this._increasedRangedDamage.Initialize("{=Ch5NpCd0}Increased Ranged Damage", "{=labbKop6}{BONUS_AMOUNT}% ranged damage to troops in your formation.", 0.04f, 0.06f, 0.08f, EffectIncrementType.AddFactor);
			this._increasedChargeDamage.Initialize("{=O2oBC9sH}Increased Charge Damage", "{=Z2xgnrDa}{BONUS_AMOUNT}% charge damage to mounted troops in your formation.", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
			this._decreasedChargeDamage.Initialize("{=PkFT0D9a}Decreased Charge Damage", "{=Z2xgnrDa}{BONUS_AMOUNT}% charge damage to mounted troops in your formation.", -0.1f, -0.2f, -0.3f, EffectIncrementType.AddFactor);
			this._decreasedRangedAccuracyPenalty.Initialize("{=MkBPRCuF}Decreased Ranged Accuracy Penalty", "{=Gu0Wxxul}{BONUS_AMOUNT}% accuracy penalty for ranged troops in your formation.", -0.04f, -0.06f, -0.08f, EffectIncrementType.AddFactor);
			this._decreasedMoraleShock.Initialize("{=nOMT0Cw6}Decreased Morale Shock", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.1f, -0.2f, -0.3f, EffectIncrementType.AddFactor);
			this._decreasedMeleeAttackDamage.Initialize("{=a3Vc59WV}Decreased Taken Melee Attack Damage", "{=ORFrCYSn}{BONUS_AMOUNT}% damage by melee attacks to troops in your formation.", -0.05f, -0.1f, -0.15f, EffectIncrementType.AddFactor);
			this._decreasedRangedAttackDamage.Initialize("{=p0JFbL7G}Decreased Taken Ranged Attack Damage", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.05f, -0.1f, -0.15f, EffectIncrementType.AddFactor);
			this._decreasedShieldDamage.Initialize("{=T79exjaP}Decreased Taken Shield Damage", "{=klGEDUmw}{BONUS_AMOUNT}% damage to shields of troops in your formation.", -0.15f, -0.25f, -0.3f, EffectIncrementType.AddFactor);
			this._increasedTroopMovementSpeed.Initialize("{=PbJAOKKZ}Increased Troop Movement Speed", "{=nqWulUTP}{BONUS_AMOUNT}% movement speed to infantry in your formation.", 0.15f, 0.25f, 0.3f, EffectIncrementType.AddFactor);
			this._increasedMountMovementSpeed.Initialize("{=nMfxbc0Y}Increased Mount Movement Speed", "{=g0l7W5xQ}{BONUS_AMOUNT}% movement speed to mounts in your formation.", 0.05f, 0.08f, 0.1f, EffectIncrementType.AddFactor);
			this._increasedMoraleShockByMeleeTroops.Initialize("{=nOMT0Cw6}Increased Morale Shock", "{=!}INCREASED MORALE SHOCK BY MELEE TROOPS DESCRIPTION", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
		}

		// Token: 0x04000304 RID: 772
		private BannerEffect _increasedMeleeDamage;

		// Token: 0x04000305 RID: 773
		private BannerEffect _increasedMeleeDamageAgainstMountedTroops;

		// Token: 0x04000306 RID: 774
		private BannerEffect _increasedRangedDamage;

		// Token: 0x04000307 RID: 775
		private BannerEffect _increasedChargeDamage;

		// Token: 0x04000308 RID: 776
		private BannerEffect _decreasedChargeDamage;

		// Token: 0x04000309 RID: 777
		private BannerEffect _decreasedRangedAccuracyPenalty;

		// Token: 0x0400030A RID: 778
		private BannerEffect _decreasedMoraleShock;

		// Token: 0x0400030B RID: 779
		private BannerEffect _decreasedMeleeAttackDamage;

		// Token: 0x0400030C RID: 780
		private BannerEffect _decreasedRangedAttackDamage;

		// Token: 0x0400030D RID: 781
		private BannerEffect _decreasedShieldDamage;

		// Token: 0x0400030E RID: 782
		private BannerEffect _increasedTroopMovementSpeed;

		// Token: 0x0400030F RID: 783
		private BannerEffect _increasedMountMovementSpeed;

		// Token: 0x04000310 RID: 784
		private BannerEffect _increasedMoraleShockByMeleeTroops;
	}
}
