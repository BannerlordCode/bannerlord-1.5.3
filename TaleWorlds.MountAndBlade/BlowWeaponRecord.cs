using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EF RID: 495
	[EngineStruct("Blow_weapon_record", false, null)]
	public struct BlowWeaponRecord
	{
		// Token: 0x06001CEC RID: 7404 RVA: 0x000625C4 File Offset: 0x000607C4
		public void FillAsMeleeBlow(ItemObject item, WeaponComponentData weaponComponentData, int affectorWeaponSlot, sbyte weaponAttachBoneIndex)
		{
			this._isMissile = false;
			if (weaponComponentData != null)
			{
				this.ItemFlags = item.ItemFlags;
				this.WeaponFlags = weaponComponentData.WeaponFlags;
				this.WeaponClass = weaponComponentData.WeaponClass;
				this.BoneNoToAttach = weaponAttachBoneIndex;
				this.AffectorWeaponSlotOrMissileIndex = affectorWeaponSlot;
				this.Weight = item.Weight;
				this._isMaterialMetal = weaponComponentData.PhysicsMaterial.Contains("metal");
				return;
			}
			this._isMaterialMetal = false;
			this.AffectorWeaponSlotOrMissileIndex = -1;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00062640 File Offset: 0x00060840
		public void FillAsMissileBlow(ItemObject item, WeaponComponentData weaponComponentData, int missileIndex, sbyte weaponAttachBoneIndex, Vec3 startingPosition, Vec3 currentPosition, Vec3 velocity)
		{
			this._isMissile = true;
			this.StartingPosition = startingPosition;
			this.CurrentPosition = currentPosition;
			this.Velocity = velocity;
			this.ItemFlags = item.ItemFlags;
			this.WeaponFlags = weaponComponentData.WeaponFlags;
			this.WeaponClass = weaponComponentData.WeaponClass;
			this.BoneNoToAttach = weaponAttachBoneIndex;
			this.AffectorWeaponSlotOrMissileIndex = missileIndex;
			this.Weight = item.Weight;
			this._isMaterialMetal = weaponComponentData.PhysicsMaterial.Contains("metal");
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000626C1 File Offset: 0x000608C1
		public bool HasWeapon()
		{
			return this.AffectorWeaponSlotOrMissileIndex >= 0;
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000626CF File Offset: 0x000608CF
		public bool IsMissile
		{
			get
			{
				return this._isMissile;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x000626D7 File Offset: 0x000608D7
		public bool IsShield
		{
			get
			{
				return !this.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask) && this.WeaponFlags.HasAllFlags(WeaponFlags.HasHitPoints | WeaponFlags.CanBlockRanged);
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x000626FB File Offset: 0x000608FB
		public bool IsRanged
		{
			get
			{
				return this.WeaponFlags.HasAnyFlag(WeaponFlags.RangedWeapon);
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0006270A File Offset: 0x0006090A
		public bool IsAmmo
		{
			get
			{
				return !this.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask) && this.WeaponFlags.HasAnyFlag(WeaponFlags.Consumable);
			}
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00062730 File Offset: 0x00060930
		public int GetHitSound(bool isOwnerHumanoid, bool isCriticalBlow, bool isLowBlow, bool isNonTipThrust, AgentAttackType attackType, DamageTypes damageType)
		{
			int num;
			if (this.HasWeapon())
			{
				if (this.IsRanged || this.IsAmmo)
				{
					switch (this.WeaponClass)
					{
					case WeaponClass.Sling:
					case WeaponClass.Stone:
					case WeaponClass.BallistaStone:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingStoneHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingStoneLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatThrowingStoneMed;
					case WeaponClass.Boulder:
					case WeaponClass.BallistaBoulder:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatBoulderHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatBoulderLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatBoulderMed;
					case WeaponClass.ThrowingAxe:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingAxeHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingAxeLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatThrowingAxeMed;
					case WeaponClass.ThrowingKnife:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerMed;
					case WeaponClass.Javelin:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatMissileHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatMissileLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatMissileMed;
					}
					if (isCriticalBlow)
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMissileHigh;
					}
					else if (isLowBlow)
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMissileLow;
					}
					else
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMissileMed;
					}
				}
				else if (this.IsShield)
				{
					if (this._isMaterialMetal)
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMetalShieldBash;
					}
					else
					{
						num = CombatSoundContainer.SoundCodeMissionCombatWoodShieldBash;
					}
				}
				else if (attackType == AgentAttackType.Bash)
				{
					num = CombatSoundContainer.SoundCodeMissionCombatBluntLow;
				}
				else
				{
					if (isNonTipThrust)
					{
						damageType = DamageTypes.Blunt;
					}
					switch (damageType)
					{
					case DamageTypes.Cut:
						if (isCriticalBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatCutHigh;
						}
						else if (isLowBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatCutLow;
						}
						else
						{
							num = CombatSoundContainer.SoundCodeMissionCombatCutMed;
						}
						break;
					case DamageTypes.Pierce:
						if (isCriticalBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatPierceHigh;
						}
						else if (isLowBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatPierceLow;
						}
						else
						{
							num = CombatSoundContainer.SoundCodeMissionCombatPierceMed;
						}
						break;
					case DamageTypes.Blunt:
						if (isCriticalBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatBluntHigh;
						}
						else if (isLowBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatBluntLow;
						}
						else
						{
							num = CombatSoundContainer.SoundCodeMissionCombatBluntMed;
						}
						break;
					default:
						num = CombatSoundContainer.SoundCodeMissionCombatBluntMed;
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BlowWeaponRecord.cs", "GetHitSound", 250);
						break;
					}
				}
			}
			else if (!isOwnerHumanoid)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatChargeDamage;
			}
			else if (attackType == AgentAttackType.Kick)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatKick;
			}
			else if (isCriticalBlow)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatPunchHigh;
			}
			else if (isLowBlow)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatPunchLow;
			}
			else
			{
				num = CombatSoundContainer.SoundCodeMissionCombatPunchMed;
			}
			return num;
		}

		// Token: 0x040009CD RID: 2509
		public Vec3 StartingPosition;

		// Token: 0x040009CE RID: 2510
		public Vec3 CurrentPosition;

		// Token: 0x040009CF RID: 2511
		public Vec3 Velocity;

		// Token: 0x040009D0 RID: 2512
		public ItemFlags ItemFlags;

		// Token: 0x040009D1 RID: 2513
		public WeaponFlags WeaponFlags;

		// Token: 0x040009D2 RID: 2514
		public WeaponClass WeaponClass;

		// Token: 0x040009D3 RID: 2515
		public sbyte BoneNoToAttach;

		// Token: 0x040009D4 RID: 2516
		public int AffectorWeaponSlotOrMissileIndex;

		// Token: 0x040009D5 RID: 2517
		public float Weight;

		// Token: 0x040009D6 RID: 2518
		[CustomEngineStructMemberData(true)]
		[MarshalAs(UnmanagedType.U1)]
		private bool _isMissile;

		// Token: 0x040009D7 RID: 2519
		[MarshalAs(UnmanagedType.U1)]
		private bool _isMaterialMetal;
	}
}
