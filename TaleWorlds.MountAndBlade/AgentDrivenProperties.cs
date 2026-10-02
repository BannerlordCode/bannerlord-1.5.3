using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000104 RID: 260
	public class AgentDrivenProperties
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00017595 File Offset: 0x00015795
		internal float[] Values
		{
			get
			{
				return this._statValues;
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0001759D File Offset: 0x0001579D
		public AgentDrivenProperties()
		{
			this._statValues = new float[98];
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x000175B2 File Offset: 0x000157B2
		public float GetStat(DrivenProperty propertyEnum)
		{
			return this._statValues[(int)propertyEnum];
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000175BC File Offset: 0x000157BC
		public void SetStat(DrivenProperty propertyEnum, float value)
		{
			this._statValues[(int)propertyEnum] = value;
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x000175C7 File Offset: 0x000157C7
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x000175D1 File Offset: 0x000157D1
		public float SwingSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.SwingSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.SwingSpeedMultiplier, value);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000175DC File Offset: 0x000157DC
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x000175E6 File Offset: 0x000157E6
		public float ThrustOrRangedReadySpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.ThrustOrRangedReadySpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.ThrustOrRangedReadySpeedMultiplier, value);
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x000175F1 File Offset: 0x000157F1
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x000175FB File Offset: 0x000157FB
		public float HandlingMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.HandlingMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.HandlingMultiplier, value);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00017606 File Offset: 0x00015806
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x00017610 File Offset: 0x00015810
		public float ReloadSpeed
		{
			get
			{
				return this.GetStat(DrivenProperty.ReloadSpeed);
			}
			set
			{
				this.SetStat(DrivenProperty.ReloadSpeed, value);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0001761B File Offset: 0x0001581B
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x00017625 File Offset: 0x00015825
		public float MissileSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.MissileSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.MissileSpeedMultiplier, value);
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00017630 File Offset: 0x00015830
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x0001763A File Offset: 0x0001583A
		public float WeaponInaccuracy
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponInaccuracy);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponInaccuracy, value);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00017645 File Offset: 0x00015845
		// (set) Token: 0x06000C82 RID: 3202 RVA: 0x0001764F File Offset: 0x0001584F
		public float WeaponMaxMovementAccuracyPenalty
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponWorstMobileAccuracyPenalty);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponWorstMobileAccuracyPenalty, value);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0001765A File Offset: 0x0001585A
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x00017664 File Offset: 0x00015864
		public float WeaponMaxUnsteadyAccuracyPenalty
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponWorstUnsteadyAccuracyPenalty);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponWorstUnsteadyAccuracyPenalty, value);
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0001766F File Offset: 0x0001586F
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x00017679 File Offset: 0x00015879
		public float WeaponBestAccuracyWaitTime
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponBestAccuracyWaitTime);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponBestAccuracyWaitTime, value);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00017684 File Offset: 0x00015884
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x0001768E File Offset: 0x0001588E
		public float WeaponUnsteadyBeginTime
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponUnsteadyBeginTime);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponUnsteadyBeginTime, value);
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00017699 File Offset: 0x00015899
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x000176A3 File Offset: 0x000158A3
		public float WeaponUnsteadyEndTime
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponUnsteadyEndTime);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponUnsteadyEndTime, value);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x000176AE File Offset: 0x000158AE
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x000176B8 File Offset: 0x000158B8
		public float WeaponRotationalAccuracyPenaltyInRadians
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponRotationalAccuracyPenaltyInRadians);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponRotationalAccuracyPenaltyInRadians, value);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x000176C3 File Offset: 0x000158C3
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x000176CD File Offset: 0x000158CD
		public float WeaponExternalAccelerationAccuracyPenalty
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponExternalAccelerationAccuracyPenalty);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponExternalAccelerationAccuracyPenalty, value);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x000176D8 File Offset: 0x000158D8
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x000176E2 File Offset: 0x000158E2
		public float ArmorEncumbrance
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorEncumbrance);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorEncumbrance, value);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x000176ED File Offset: 0x000158ED
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x000176F7 File Offset: 0x000158F7
		public float DamageMultiplierBonus
		{
			get
			{
				return this.GetStat(DrivenProperty.DamageMultiplierBonus);
			}
			set
			{
				this.SetStat(DrivenProperty.DamageMultiplierBonus, value);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00017702 File Offset: 0x00015902
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0001770C File Offset: 0x0001590C
		public float ThrowingWeaponDamageMultiplierBonus
		{
			get
			{
				return this.GetStat(DrivenProperty.ThrowingWeaponDamageMultiplierBonus);
			}
			set
			{
				this.SetStat(DrivenProperty.ThrowingWeaponDamageMultiplierBonus, value);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00017717 File Offset: 0x00015917
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x00017721 File Offset: 0x00015921
		public float MeleeWeaponDamageMultiplierBonus
		{
			get
			{
				return this.GetStat(DrivenProperty.MeleeWeaponDamageMultiplierBonus);
			}
			set
			{
				this.SetStat(DrivenProperty.MeleeWeaponDamageMultiplierBonus, value);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0001772C File Offset: 0x0001592C
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x00017736 File Offset: 0x00015936
		public float ArmorPenetrationMultiplierCrossbow
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorPenetrationMultiplierCrossbow);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorPenetrationMultiplierCrossbow, value);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00017741 File Offset: 0x00015941
		// (set) Token: 0x06000C9A RID: 3226 RVA: 0x0001774B File Offset: 0x0001594B
		public float ArmorPenetrationMultiplierBow
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorPenetrationMultiplierBow);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorPenetrationMultiplierBow, value);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00017756 File Offset: 0x00015956
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x00017760 File Offset: 0x00015960
		public float WeaponsEncumbrance
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponsEncumbrance);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponsEncumbrance, value);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0001776B File Offset: 0x0001596B
		// (set) Token: 0x06000C9E RID: 3230 RVA: 0x00017775 File Offset: 0x00015975
		public float ArmorHead
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorHead);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorHead, value);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00017780 File Offset: 0x00015980
		// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x0001778A File Offset: 0x0001598A
		public float ArmorTorso
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorTorso);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorTorso, value);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00017795 File Offset: 0x00015995
		// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x0001779F File Offset: 0x0001599F
		public float ArmorLegs
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorLegs);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorLegs, value);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x000177AA File Offset: 0x000159AA
		// (set) Token: 0x06000CA4 RID: 3236 RVA: 0x000177B4 File Offset: 0x000159B4
		public float ArmorArms
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorArms);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorArms, value);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000177BF File Offset: 0x000159BF
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x000177C9 File Offset: 0x000159C9
		public float AttributeRiding
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeRiding);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeRiding, value);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x000177D4 File Offset: 0x000159D4
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x000177DE File Offset: 0x000159DE
		public float AttributeShield
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeShield);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeShield, value);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x000177E9 File Offset: 0x000159E9
		// (set) Token: 0x06000CAA RID: 3242 RVA: 0x000177F3 File Offset: 0x000159F3
		public float AttributeShieldMissileCollisionBodySizeAdder
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeShieldMissileCollisionBodySizeAdder);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeShieldMissileCollisionBodySizeAdder, value);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x000177FE File Offset: 0x000159FE
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00017808 File Offset: 0x00015A08
		public float ShieldBashStunDurationMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.ShieldBashStunDurationMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.ShieldBashStunDurationMultiplier, value);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00017813 File Offset: 0x00015A13
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0001781D File Offset: 0x00015A1D
		public float KickStunDurationMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.KickStunDurationMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.KickStunDurationMultiplier, value);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00017828 File Offset: 0x00015A28
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x00017832 File Offset: 0x00015A32
		public float ReloadMovementPenaltyFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.ReloadMovementPenaltyFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.ReloadMovementPenaltyFactor, value);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0001783D File Offset: 0x00015A3D
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x00017847 File Offset: 0x00015A47
		public float TopSpeedReachDuration
		{
			get
			{
				return this.GetStat(DrivenProperty.TopSpeedReachDuration);
			}
			set
			{
				this.SetStat(DrivenProperty.TopSpeedReachDuration, value);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00017852 File Offset: 0x00015A52
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0001785C File Offset: 0x00015A5C
		public float MaxSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.MaxSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.MaxSpeedMultiplier, value);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00017867 File Offset: 0x00015A67
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x00017871 File Offset: 0x00015A71
		public float CombatMaxSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.CombatMaxSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.CombatMaxSpeedMultiplier, value);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0001787C File Offset: 0x00015A7C
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x00017886 File Offset: 0x00015A86
		public float CrouchedSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.CrouchedSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.CrouchedSpeedMultiplier, value);
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00017891 File Offset: 0x00015A91
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0001789B File Offset: 0x00015A9B
		public float AttributeHorseArchery
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeHorseArchery);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeHorseArchery, value);
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x000178A6 File Offset: 0x00015AA6
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x000178B0 File Offset: 0x00015AB0
		public float AttributeCourage
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeCourage);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeCourage, value);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x000178BB File Offset: 0x00015ABB
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x000178C5 File Offset: 0x00015AC5
		public float MountManeuver
		{
			get
			{
				return this.GetStat(DrivenProperty.MountManeuver);
			}
			set
			{
				this.SetStat(DrivenProperty.MountManeuver, value);
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x000178D0 File Offset: 0x00015AD0
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x000178DA File Offset: 0x00015ADA
		public float MountSpeed
		{
			get
			{
				return this.GetStat(DrivenProperty.MountSpeed);
			}
			set
			{
				this.SetStat(DrivenProperty.MountSpeed, value);
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000178E5 File Offset: 0x00015AE5
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x000178EF File Offset: 0x00015AEF
		public float MountDashAccelerationMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.MountDashAccelerationMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.MountDashAccelerationMultiplier, value);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x000178FA File Offset: 0x00015AFA
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00017904 File Offset: 0x00015B04
		public float MountChargeDamage
		{
			get
			{
				return this.GetStat(DrivenProperty.MountChargeDamage);
			}
			set
			{
				this.SetStat(DrivenProperty.MountChargeDamage, value);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0001790F File Offset: 0x00015B0F
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x00017919 File Offset: 0x00015B19
		public float MountDifficulty
		{
			get
			{
				return this.GetStat(DrivenProperty.MountDifficulty);
			}
			set
			{
				this.SetStat(DrivenProperty.MountDifficulty, value);
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00017924 File Offset: 0x00015B24
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0001792E File Offset: 0x00015B2E
		public float BipedalRangedReadySpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.BipedalRangedReadySpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.BipedalRangedReadySpeedMultiplier, value);
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00017939 File Offset: 0x00015B39
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00017943 File Offset: 0x00015B43
		public float BipedalRangedReloadSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.BipedalRangedReloadSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.BipedalRangedReloadSpeedMultiplier, value);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0001794E File Offset: 0x00015B4E
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00017958 File Offset: 0x00015B58
		public float AiShooterErrorWoRangeUpdate
		{
			get
			{
				return this.GetStat(DrivenProperty.AiShooterErrorWoRangeUpdate);
			}
			set
			{
				this.SetStat(DrivenProperty.AiShooterErrorWoRangeUpdate, value);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00017963 File Offset: 0x00015B63
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x0001796C File Offset: 0x00015B6C
		public float AiRangedHorsebackMissileRange
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangedHorsebackMissileRange);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangedHorsebackMissileRange, value);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00017976 File Offset: 0x00015B76
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x0001797F File Offset: 0x00015B7F
		public float AiFacingMissileWatch
		{
			get
			{
				return this.GetStat(DrivenProperty.AiFacingMissileWatch);
			}
			set
			{
				this.SetStat(DrivenProperty.AiFacingMissileWatch, value);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00017989 File Offset: 0x00015B89
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00017992 File Offset: 0x00015B92
		public float AiFlyingMissileCheckRadius
		{
			get
			{
				return this.GetStat(DrivenProperty.AiFlyingMissileCheckRadius);
			}
			set
			{
				this.SetStat(DrivenProperty.AiFlyingMissileCheckRadius, value);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0001799C File Offset: 0x00015B9C
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x000179A5 File Offset: 0x00015BA5
		public float AiShootFreq
		{
			get
			{
				return this.GetStat(DrivenProperty.AiShootFreq);
			}
			set
			{
				this.SetStat(DrivenProperty.AiShootFreq, value);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000179AF File Offset: 0x00015BAF
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x000179B8 File Offset: 0x00015BB8
		public float AiWaitBeforeShootFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWaitBeforeShootFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWaitBeforeShootFactor, value);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000179C2 File Offset: 0x00015BC2
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x000179CB File Offset: 0x00015BCB
		public float AIBlockOnDecideAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIBlockOnDecideAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIBlockOnDecideAbility, value);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x000179D5 File Offset: 0x00015BD5
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x000179DE File Offset: 0x00015BDE
		public float AIParryOnDecideAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIParryOnDecideAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIParryOnDecideAbility, value);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000179E8 File Offset: 0x00015BE8
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x000179F1 File Offset: 0x00015BF1
		public float AiTryChamberAttackOnDecide
		{
			get
			{
				return this.GetStat(DrivenProperty.AiTryChamberAttackOnDecide);
			}
			set
			{
				this.SetStat(DrivenProperty.AiTryChamberAttackOnDecide, value);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x000179FB File Offset: 0x00015BFB
		// (set) Token: 0x06000CDE RID: 3294 RVA: 0x00017A04 File Offset: 0x00015C04
		public float AIAttackOnParryChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AIAttackOnParryChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AIAttackOnParryChance, value);
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00017A0E File Offset: 0x00015C0E
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x00017A18 File Offset: 0x00015C18
		public float AiAttackOnParryTiming
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackOnParryTiming);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackOnParryTiming, value);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00017A23 File Offset: 0x00015C23
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x00017A2D File Offset: 0x00015C2D
		public float AIDecideOnAttackChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AIDecideOnAttackChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AIDecideOnAttackChance, value);
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00017A38 File Offset: 0x00015C38
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x00017A42 File Offset: 0x00015C42
		public float AIParryOnAttackAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIParryOnAttackAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIParryOnAttackAbility, value);
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00017A4D File Offset: 0x00015C4D
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00017A57 File Offset: 0x00015C57
		public float AiKick
		{
			get
			{
				return this.GetStat(DrivenProperty.AiKick);
			}
			set
			{
				this.SetStat(DrivenProperty.AiKick, value);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00017A62 File Offset: 0x00015C62
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00017A6C File Offset: 0x00015C6C
		public float AiAttackCalculationMaxTimeFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackCalculationMaxTimeFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackCalculationMaxTimeFactor, value);
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00017A77 File Offset: 0x00015C77
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x00017A81 File Offset: 0x00015C81
		public float AiDecideOnAttackWhenReceiveHitTiming
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDecideOnAttackWhenReceiveHitTiming);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDecideOnAttackWhenReceiveHitTiming, value);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x00017A8C File Offset: 0x00015C8C
		// (set) Token: 0x06000CEC RID: 3308 RVA: 0x00017A96 File Offset: 0x00015C96
		public float AiDecideOnAttackContinueAction
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDecideOnAttackContinueAction);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDecideOnAttackContinueAction, value);
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00017AA1 File Offset: 0x00015CA1
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x00017AAB File Offset: 0x00015CAB
		public float AiDecideOnAttackingContinue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDecideOnAttackingContinue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDecideOnAttackingContinue, value);
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00017AB6 File Offset: 0x00015CB6
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x00017AC0 File Offset: 0x00015CC0
		public float AIParryOnAttackingContinueAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIParryOnAttackingContinueAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIParryOnAttackingContinueAbility, value);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00017ACB File Offset: 0x00015CCB
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x00017AD5 File Offset: 0x00015CD5
		public float AIDecideOnRealizeEnemyBlockingAttackAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIDecideOnRealizeEnemyBlockingAttackAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIDecideOnRealizeEnemyBlockingAttackAbility, value);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00017AE0 File Offset: 0x00015CE0
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x00017AEA File Offset: 0x00015CEA
		public float AIRealizeBlockingFromIncorrectSideAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIRealizeBlockingFromIncorrectSideAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIRealizeBlockingFromIncorrectSideAbility, value);
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00017AF5 File Offset: 0x00015CF5
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x00017AFF File Offset: 0x00015CFF
		public float AiAttackingShieldDefenseChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackingShieldDefenseChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackingShieldDefenseChance, value);
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00017B0A File Offset: 0x00015D0A
		// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x00017B14 File Offset: 0x00015D14
		public float AiAttackingShieldDefenseTimer
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackingShieldDefenseTimer);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackingShieldDefenseTimer, value);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00017B1F File Offset: 0x00015D1F
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x00017B29 File Offset: 0x00015D29
		public float AiCheckApplyMovementInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckApplyMovementInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckApplyMovementInterval, value);
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00017B34 File Offset: 0x00015D34
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00017B3E File Offset: 0x00015D3E
		public float AiCheckCalculateMovementInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckCalculateMovementInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckCalculateMovementInterval, value);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00017B49 File Offset: 0x00015D49
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00017B53 File Offset: 0x00015D53
		public float AiCheckDecideSimpleBehaviorInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckDecideSimpleBehaviorInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckDecideSimpleBehaviorInterval, value);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00017B5E File Offset: 0x00015D5E
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x00017B68 File Offset: 0x00015D68
		public float AiCheckDoSimpleBehaviorInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckDoSimpleBehaviorInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckDoSimpleBehaviorInterval, value);
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00017B73 File Offset: 0x00015D73
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x00017B7D File Offset: 0x00015D7D
		public float AiMovementDelayFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiMovementDelayFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiMovementDelayFactor, value);
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00017B88 File Offset: 0x00015D88
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x00017B92 File Offset: 0x00015D92
		public float AiParryDecisionChangeValue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiParryDecisionChangeValue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiParryDecisionChangeValue, value);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00017B9D File Offset: 0x00015D9D
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x00017BA7 File Offset: 0x00015DA7
		public float AiDefendWithShieldDecisionChanceValue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDefendWithShieldDecisionChanceValue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDefendWithShieldDecisionChanceValue, value);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00017BB2 File Offset: 0x00015DB2
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00017BBC File Offset: 0x00015DBC
		public float AiMoveEnemySideTimeValue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiMoveEnemySideTimeValue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiMoveEnemySideTimeValue, value);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00017BC7 File Offset: 0x00015DC7
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x00017BD1 File Offset: 0x00015DD1
		public float AiMinimumDistanceToContinueFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiMinimumDistanceToContinueFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiMinimumDistanceToContinueFactor, value);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00017BDC File Offset: 0x00015DDC
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x00017BE6 File Offset: 0x00015DE6
		public float AiChargeHorsebackTargetDistFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiChargeHorsebackTargetDistFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiChargeHorsebackTargetDistFactor, value);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00017BF1 File Offset: 0x00015DF1
		// (set) Token: 0x06000D0E RID: 3342 RVA: 0x00017BFB File Offset: 0x00015DFB
		public float AiRangerLeadErrorMin
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerLeadErrorMin);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerLeadErrorMin, value);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00017C06 File Offset: 0x00015E06
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x00017C10 File Offset: 0x00015E10
		public float AiRangerLeadErrorMax
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerLeadErrorMax);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerLeadErrorMax, value);
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00017C1B File Offset: 0x00015E1B
		// (set) Token: 0x06000D12 RID: 3346 RVA: 0x00017C25 File Offset: 0x00015E25
		public float AiRangerVerticalErrorMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerVerticalErrorMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerVerticalErrorMultiplier, value);
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x00017C30 File Offset: 0x00015E30
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x00017C3A File Offset: 0x00015E3A
		public float AiRangerHorizontalErrorMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerHorizontalErrorMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerHorizontalErrorMultiplier, value);
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00017C45 File Offset: 0x00015E45
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00017C4F File Offset: 0x00015E4F
		public float AIAttackOnDecideChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AIAttackOnDecideChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AIAttackOnDecideChance, value);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00017C5A File Offset: 0x00015E5A
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x00017C64 File Offset: 0x00015E64
		public float AiRaiseShieldDelayTimeBase
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRaiseShieldDelayTimeBase);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRaiseShieldDelayTimeBase, value);
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00017C6F File Offset: 0x00015E6F
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00017C79 File Offset: 0x00015E79
		public float AiUseShieldAgainstEnemyMissileProbability
		{
			get
			{
				return this.GetStat(DrivenProperty.AiUseShieldAgainstEnemyMissileProbability);
			}
			set
			{
				this.SetStat(DrivenProperty.AiUseShieldAgainstEnemyMissileProbability, value);
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00017C84 File Offset: 0x00015E84
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x00017C93 File Offset: 0x00015E93
		public int AiSpeciesIndex
		{
			get
			{
				return MathF.Round(this.GetStat(DrivenProperty.AiSpeciesIndex));
			}
			set
			{
				this.SetStat(DrivenProperty.AiSpeciesIndex, (float)value);
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00017C9F File Offset: 0x00015E9F
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x00017CA9 File Offset: 0x00015EA9
		public float AiRandomizedDefendDirectionChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRandomizedDefendDirectionChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRandomizedDefendDirectionChance, value);
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00017CB4 File Offset: 0x00015EB4
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x00017CBE File Offset: 0x00015EBE
		public float AiShooterError
		{
			get
			{
				return this.GetStat(DrivenProperty.AiShooterError);
			}
			set
			{
				this.SetStat(DrivenProperty.AiShooterError, value);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00017CC9 File Offset: 0x00015EC9
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x00017CD3 File Offset: 0x00015ED3
		public float AiWeaponFavorMultiplierMelee
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWeaponFavorMultiplierMelee);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWeaponFavorMultiplierMelee, value);
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00017CDE File Offset: 0x00015EDE
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x00017CE8 File Offset: 0x00015EE8
		public float AiWeaponFavorMultiplierRanged
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWeaponFavorMultiplierRanged);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWeaponFavorMultiplierRanged, value);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00017CF3 File Offset: 0x00015EF3
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x00017CFD File Offset: 0x00015EFD
		public float AiWeaponFavorMultiplierPolearm
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWeaponFavorMultiplierPolearm);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWeaponFavorMultiplierPolearm, value);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00017D08 File Offset: 0x00015F08
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x00017D12 File Offset: 0x00015F12
		public float AISetNoAttackTimerAfterBeingHitAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoAttackTimerAfterBeingHitAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoAttackTimerAfterBeingHitAbility, value);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00017D1D File Offset: 0x00015F1D
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00017D27 File Offset: 0x00015F27
		public float AISetNoAttackTimerAfterBeingParriedAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoAttackTimerAfterBeingParriedAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoAttackTimerAfterBeingParriedAbility, value);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00017D32 File Offset: 0x00015F32
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00017D3C File Offset: 0x00015F3C
		public float AISetNoDefendTimerAfterHittingAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoDefendTimerAfterHittingAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoDefendTimerAfterHittingAbility, value);
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00017D47 File Offset: 0x00015F47
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x00017D51 File Offset: 0x00015F51
		public float AISetNoDefendTimerAfterParryingAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoDefendTimerAfterParryingAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoDefendTimerAfterParryingAbility, value);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00017D5C File Offset: 0x00015F5C
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00017D66 File Offset: 0x00015F66
		public float AIEstimateStunDurationPrecision
		{
			get
			{
				return this.GetStat(DrivenProperty.AIEstimateStunDurationPrecision);
			}
			set
			{
				this.SetStat(DrivenProperty.AIEstimateStunDurationPrecision, value);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00017D71 File Offset: 0x00015F71
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00017D7B File Offset: 0x00015F7B
		public float AIHoldingReadyMaxDuration
		{
			get
			{
				return this.GetStat(DrivenProperty.AIHoldingReadyMaxDuration);
			}
			set
			{
				this.SetStat(DrivenProperty.AIHoldingReadyMaxDuration, value);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00017D86 File Offset: 0x00015F86
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x00017D90 File Offset: 0x00015F90
		public float AIHoldingReadyVariationPercentage
		{
			get
			{
				return this.GetStat(DrivenProperty.AIHoldingReadyVariationPercentage);
			}
			set
			{
				this.SetStat(DrivenProperty.AIHoldingReadyVariationPercentage, value);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00017D9B File Offset: 0x00015F9B
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x00017DA5 File Offset: 0x00015FA5
		public float OffhandWeaponDefendSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.OffhandWeaponDefendSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.OffhandWeaponDefendSpeedMultiplier, value);
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00017DB0 File Offset: 0x00015FB0
		internal float[] InitializeDrivenProperties(Agent agent, Equipment spawnEquipment, AgentBuildData agentBuildData)
		{
			MissionGameModels.Current.AgentStatCalculateModel.InitializeAgentStats(agent, spawnEquipment, this, agentBuildData);
			MissionGameModels.Current.AgentStatCalculateModel.UpdateAgentStats(agent, this);
			return this._statValues;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00017DDC File Offset: 0x00015FDC
		internal float[] UpdateDrivenProperties(Agent agent)
		{
			MissionGameModels.Current.AgentStatCalculateModel.UpdateAgentStats(agent, this);
			return this._statValues;
		}

		// Token: 0x040002D0 RID: 720
		private readonly float[] _statValues;
	}
}
