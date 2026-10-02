using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DD RID: 733
	public struct MissionWeapon
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x000A1E6B File Offset: 0x000A006B
		// (set) Token: 0x06002A68 RID: 10856 RVA: 0x000A1E73 File Offset: 0x000A0073
		public ItemObject Item { get; private set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000A1E7C File Offset: 0x000A007C
		// (set) Token: 0x06002A6A RID: 10858 RVA: 0x000A1E84 File Offset: 0x000A0084
		public ItemModifier ItemModifier { get; private set; }

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000A1E8D File Offset: 0x000A008D
		public int WeaponsCount
		{
			get
			{
				return this._weapons.Count;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000A1E9A File Offset: 0x000A009A
		public WeaponComponentData CurrentUsageItem
		{
			get
			{
				if (this._weapons == null || this._weapons.Count == 0)
				{
					return null;
				}
				return this._weapons[this.CurrentUsageIndex];
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000A1EC4 File Offset: 0x000A00C4
		// (set) Token: 0x06002A6E RID: 10862 RVA: 0x000A1ECC File Offset: 0x000A00CC
		public short ReloadPhase { get; set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000A1ED8 File Offset: 0x000A00D8
		public short ReloadPhaseCount
		{
			get
			{
				short num = 1;
				if (this.CurrentUsageItem != null)
				{
					num = this.CurrentUsageItem.ReloadPhaseCount;
				}
				return num;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000A1EFC File Offset: 0x000A00FC
		public bool IsReloading
		{
			get
			{
				return this.ReloadPhase < this.ReloadPhaseCount;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000A1F0C File Offset: 0x000A010C
		// (set) Token: 0x06002A72 RID: 10866 RVA: 0x000A1F14 File Offset: 0x000A0114
		public Banner Banner { get; private set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000A1F1D File Offset: 0x000A011D
		// (set) Token: 0x06002A74 RID: 10868 RVA: 0x000A1F25 File Offset: 0x000A0125
		public float GlossMultiplier { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002A75 RID: 10869 RVA: 0x000A1F2E File Offset: 0x000A012E
		public short RawDataForNetwork
		{
			get
			{
				return this._dataValue;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x000A1F36 File Offset: 0x000A0136
		// (set) Token: 0x06002A77 RID: 10871 RVA: 0x000A1F3E File Offset: 0x000A013E
		public short HitPoints
		{
			get
			{
				return this._dataValue;
			}
			set
			{
				this._dataValue = value;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x000A1F47 File Offset: 0x000A0147
		// (set) Token: 0x06002A79 RID: 10873 RVA: 0x000A1F4F File Offset: 0x000A014F
		public short Amount
		{
			get
			{
				return this._dataValue;
			}
			set
			{
				this._dataValue = value;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000A1F58 File Offset: 0x000A0158
		public short Ammo
		{
			get
			{
				MissionWeapon.MissionSubWeapon ammoWeapon = this._ammoWeapon;
				if (ammoWeapon == null)
				{
					return 0;
				}
				return ammoWeapon.Value._dataValue;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002A7B RID: 10875 RVA: 0x000A1F70 File Offset: 0x000A0170
		public MissionWeapon AmmoWeapon
		{
			get
			{
				MissionWeapon.MissionSubWeapon ammoWeapon = this._ammoWeapon;
				if (ammoWeapon == null)
				{
					return MissionWeapon.Invalid;
				}
				return ammoWeapon.Value;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x000A1F87 File Offset: 0x000A0187
		public short MaxAmmo
		{
			get
			{
				return this._modifiedMaxDataValue;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002A7D RID: 10877 RVA: 0x000A1F8F File Offset: 0x000A018F
		public short ModifiedMaxAmount
		{
			get
			{
				return this._modifiedMaxDataValue;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x000A1F97 File Offset: 0x000A0197
		public short ModifiedMaxHitPoints
		{
			get
			{
				return this._modifiedMaxDataValue;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000A1F9F File Offset: 0x000A019F
		public bool IsEmpty
		{
			get
			{
				return this.CurrentUsageItem == null;
			}
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000A1FAC File Offset: 0x000A01AC
		public MissionWeapon(ItemObject item, ItemModifier itemModifier, Banner banner)
		{
			this.Item = item;
			this.ItemModifier = itemModifier;
			this.Banner = banner;
			this.CurrentUsageIndex = 0;
			this._weapons = new List<WeaponComponentData>(1);
			this._modifiedMaxDataValue = 0;
			this._hasAnyConsumableUsage = false;
			if (item != null && item.Weapons != null)
			{
				foreach (WeaponComponentData weaponComponentData in item.Weapons)
				{
					this._weapons.Add(weaponComponentData);
					bool isConsumable = weaponComponentData.IsConsumable;
					if (isConsumable || weaponComponentData.IsRangedWeapon || weaponComponentData.WeaponFlags.HasAnyFlag(WeaponFlags.HasHitPoints))
					{
						this._modifiedMaxDataValue = weaponComponentData.MaxDataValue;
						if (itemModifier != null)
						{
							if (weaponComponentData.WeaponFlags.HasAnyFlag(WeaponFlags.HasHitPoints))
							{
								this._modifiedMaxDataValue = weaponComponentData.GetModifiedMaximumHitPoints(itemModifier);
							}
							else if (isConsumable)
							{
								this._modifiedMaxDataValue = weaponComponentData.GetModifiedStackCount(itemModifier);
							}
						}
					}
					if (isConsumable)
					{
						this._hasAnyConsumableUsage = true;
					}
				}
			}
			this._dataValue = this._modifiedMaxDataValue;
			this.ReloadPhase = 0;
			this._ammoWeapon = null;
			this._attachedWeapons = null;
			this._attachedWeaponFrames = null;
			this.GlossMultiplier = 1f;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000A20F4 File Offset: 0x000A02F4
		public MissionWeapon(ItemObject primaryItem, ItemModifier itemModifier, Banner banner, short dataValue)
		{
			this = new MissionWeapon(primaryItem, itemModifier, banner);
			this._dataValue = dataValue;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000A2107 File Offset: 0x000A0307
		public MissionWeapon(ItemObject primaryItem, ItemModifier itemModifier, Banner banner, short dataValue, short reloadPhase, MissionWeapon? ammoWeapon)
		{
			this = new MissionWeapon(primaryItem, itemModifier, banner, dataValue);
			this.ReloadPhase = reloadPhase;
			this._ammoWeapon = ((ammoWeapon != null) ? new MissionWeapon.MissionSubWeapon(ammoWeapon.Value) : null);
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000A213A File Offset: 0x000A033A
		public TextObject GetModifiedItemName()
		{
			if (this.ItemModifier == null)
			{
				return this.Item.Name;
			}
			TextObject name = this.ItemModifier.Name;
			name.SetTextVariable("ITEMNAME", this.Item.Name);
			return name;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000A2172 File Offset: 0x000A0372
		public bool IsEqualTo(MissionWeapon other)
		{
			return this.Item == other.Item;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000A2183 File Offset: 0x000A0383
		public bool IsSameType(MissionWeapon other)
		{
			return this.Item.PrimaryWeapon.WeaponClass == other.Item.PrimaryWeapon.WeaponClass;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000A21A8 File Offset: 0x000A03A8
		public float GetWeight()
		{
			float num = (this.Item.PrimaryWeapon.IsConsumable ? (this.GetBaseWeight() * (float)this._dataValue) : this.GetBaseWeight());
			MissionWeapon.MissionSubWeapon ammoWeapon = this._ammoWeapon;
			return num + ((ammoWeapon != null) ? ammoWeapon.Value.GetWeight() : 0f);
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000A21FC File Offset: 0x000A03FC
		private float GetBaseWeight()
		{
			return this.Item.Weight;
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000A2209 File Offset: 0x000A0409
		public WeaponComponentData GetWeaponComponentDataForUsage(int usageIndex)
		{
			return this._weapons[usageIndex];
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000A2217 File Offset: 0x000A0417
		public int GetGetModifiedArmorForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedArmor(this.ItemModifier);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000A2235 File Offset: 0x000A0435
		public int GetModifiedThrustDamageForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedThrustDamage(this.ItemModifier);
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000A2253 File Offset: 0x000A0453
		public int GetModifiedSwingDamageForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedSwingDamage(this.ItemModifier);
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000A2271 File Offset: 0x000A0471
		public int GetModifiedMissileDamageForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedMissileDamage(this.ItemModifier);
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000A228F File Offset: 0x000A048F
		public int GetModifiedThrustSpeedForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedThrustSpeed(this.ItemModifier);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000A22AD File Offset: 0x000A04AD
		public int GetModifiedSwingSpeedForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedSwingSpeed(this.ItemModifier);
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000A22CB File Offset: 0x000A04CB
		public int GetModifiedMissileSpeedForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedMissileSpeed(this.ItemModifier);
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000A22E9 File Offset: 0x000A04E9
		public int GetModifiedMissileSpeedForUsage(int usageIndex)
		{
			return this._weapons[usageIndex].GetModifiedMissileSpeed(this.ItemModifier);
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000A2302 File Offset: 0x000A0502
		public int GetModifiedHandlingForCurrentUsage()
		{
			return this._weapons[this.CurrentUsageIndex].GetModifiedHandling(this.ItemModifier);
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000A2320 File Offset: 0x000A0520
		public WeaponData GetWeaponData(bool needBatchedVersionForMeshes)
		{
			if (!this.IsEmpty && this.Item.WeaponComponent != null)
			{
				WeaponComponent weaponComponent = this.Item.WeaponComponent;
				WeaponData weaponData = new WeaponData
				{
					WeaponKind = (int)this.Item.Id.InternalValue,
					ItemHolsterIndices = this.Item.GetItemHolsterIndices(),
					ReloadPhase = this.ReloadPhase,
					Difficulty = this.Item.Difficulty,
					BaseWeight = this.GetBaseWeight(),
					HasFlagAnimation = false,
					WeaponFrame = weaponComponent.PrimaryWeapon.Frame,
					ScaleFactor = this.Item.ScaleFactor,
					TotalInertia = weaponComponent.PrimaryWeapon.TotalInertia,
					CenterOfMass = weaponComponent.PrimaryWeapon.CenterOfMass,
					CenterOfMass3D = weaponComponent.PrimaryWeapon.CenterOfMass3D,
					HolsterPositionShift = this.Item.HolsterPositionShift,
					TrailParticleName = weaponComponent.PrimaryWeapon.TrailParticleName,
					SkeletonName = this.Item.SkeletonName,
					StaticAnimationName = this.Item.StaticAnimationName,
					AmmoOffset = weaponComponent.PrimaryWeapon.AmmoOffset
				};
				string physicsMaterial = weaponComponent.PrimaryWeapon.PhysicsMaterial;
				weaponData.PhysicsMaterialIndex = (string.IsNullOrEmpty(physicsMaterial) ? PhysicsMaterial.InvalidPhysicsMaterial.Index : PhysicsMaterial.GetFromName(physicsMaterial).Index);
				weaponData.FlyingSoundCode = SoundManager.GetEventGlobalIndex(weaponComponent.PrimaryWeapon.FlyingSoundCode);
				weaponData.PassbySoundCode = SoundManager.GetEventGlobalIndex(weaponComponent.PrimaryWeapon.PassbySoundCode);
				weaponData.StickingFrame = weaponComponent.PrimaryWeapon.StickingFrame;
				weaponData.CollisionShape = ((!needBatchedVersionForMeshes || string.IsNullOrEmpty(this.Item.CollisionBodyName)) ? null : PhysicsShape.GetFromResource(this.Item.CollisionBodyName, false));
				weaponData.Shape = ((!needBatchedVersionForMeshes || string.IsNullOrEmpty(this.Item.BodyName)) ? null : PhysicsShape.GetFromResource(this.Item.BodyName, false));
				weaponData.DataValue = this._dataValue;
				weaponData.CurrentUsageIndex = this.CurrentUsageIndex;
				int rangedUsageIndex = this.GetRangedUsageIndex();
				WeaponComponentData weaponComponentData;
				if (this.GetConsumableIfAny(out weaponComponentData))
				{
					weaponData.AirFrictionConstant = ItemObject.GetAirFrictionConstant(weaponComponentData.WeaponClass, weaponComponentData.WeaponFlags);
				}
				else if (rangedUsageIndex >= 0)
				{
					weaponData.AirFrictionConstant = ItemObject.GetAirFrictionConstant(this.GetWeaponComponentDataForUsage(rangedUsageIndex).WeaponClass, this.GetWeaponComponentDataForUsage(rangedUsageIndex).WeaponFlags);
				}
				weaponData.GlossMultiplier = this.GlossMultiplier;
				weaponData.HasLowerHolsterPriority = this.Item.HasLowerHolsterPriority;
				MissionWeapon.OnGetWeaponDataDelegate onGetWeaponDataHandler = MissionWeapon.OnGetWeaponDataHandler;
				if (onGetWeaponDataHandler != null)
				{
					onGetWeaponDataHandler(ref weaponData, this, false, this.Banner, needBatchedVersionForMeshes);
				}
				return weaponData;
			}
			return WeaponData.InvalidWeaponData;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000A25FC File Offset: 0x000A07FC
		public WeaponStatsData[] GetWeaponStatsData()
		{
			WeaponStatsData[] array = new WeaponStatsData[this._weapons.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.GetWeaponStatsDataForUsage(i);
			}
			return array;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000A2638 File Offset: 0x000A0838
		public WeaponStatsData GetWeaponStatsDataForUsage(int usageIndex)
		{
			WeaponStatsData weaponStatsData = default(WeaponStatsData);
			WeaponComponentData weaponComponentData = this._weapons[usageIndex];
			weaponStatsData.WeaponClass = (int)weaponComponentData.WeaponClass;
			weaponStatsData.AmmoClass = (int)weaponComponentData.AmmoClass;
			weaponStatsData.Properties = (uint)this.Item.ItemFlags;
			weaponStatsData.WeaponFlags = (ulong)weaponComponentData.WeaponFlags;
			weaponStatsData.ItemUsageIndex = (string.IsNullOrEmpty(weaponComponentData.ItemUsage) ? (-1) : weaponComponentData.GetItemUsageIndex());
			weaponStatsData.ThrustSpeed = weaponComponentData.GetModifiedThrustSpeed(this.ItemModifier);
			weaponStatsData.SwingSpeed = weaponComponentData.GetModifiedSwingSpeed(this.ItemModifier);
			weaponStatsData.MissileSpeed = weaponComponentData.GetModifiedMissileSpeed(this.ItemModifier);
			weaponStatsData.ShieldArmor = weaponComponentData.GetModifiedArmor(this.ItemModifier);
			weaponStatsData.Accuracy = weaponComponentData.Accuracy;
			weaponStatsData.WeaponLength = weaponComponentData.WeaponLength;
			weaponStatsData.WeaponBalance = weaponComponentData.WeaponBalance;
			weaponStatsData.ThrustDamage = weaponComponentData.GetModifiedThrustDamage(this.ItemModifier);
			weaponStatsData.ThrustDamageType = (int)weaponComponentData.ThrustDamageType;
			weaponStatsData.SwingDamage = weaponComponentData.GetModifiedSwingDamage(this.ItemModifier);
			weaponStatsData.SwingDamageType = (int)weaponComponentData.SwingDamageType;
			weaponStatsData.DefendSpeed = weaponComponentData.GetModifiedHandling(this.ItemModifier);
			weaponStatsData.SweetSpot = weaponComponentData.SweetSpotReach;
			weaponStatsData.MaxDataValue = this._modifiedMaxDataValue;
			weaponStatsData.WeaponFrame = weaponComponentData.Frame;
			weaponStatsData.RotationSpeed = weaponComponentData.RotationSpeed;
			weaponStatsData.ReloadPhaseCount = weaponComponentData.ReloadPhaseCount;
			return weaponStatsData;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000A27B8 File Offset: 0x000A09B8
		public WeaponData GetAmmoWeaponData(bool needBatchedVersion)
		{
			return this.AmmoWeapon.GetWeaponData(needBatchedVersion);
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000A27D4 File Offset: 0x000A09D4
		public WeaponStatsData[] GetAmmoWeaponStatsData()
		{
			return this.AmmoWeapon.GetWeaponStatsData();
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000A27EF File Offset: 0x000A09EF
		public int GetAttachedWeaponsCount()
		{
			List<MissionWeapon.MissionSubWeapon> attachedWeapons = this._attachedWeapons;
			if (attachedWeapons == null)
			{
				return 0;
			}
			return attachedWeapons.Count;
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000A2802 File Offset: 0x000A0A02
		public MissionWeapon GetAttachedWeapon(int attachmentIndex)
		{
			return this._attachedWeapons[attachmentIndex].Value;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000A2815 File Offset: 0x000A0A15
		public MatrixFrame GetAttachedWeaponFrame(int attachmentIndex)
		{
			return this._attachedWeaponFrames[attachmentIndex];
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000A2823 File Offset: 0x000A0A23
		public bool IsShield()
		{
			return this._weapons.Count == 1 && this._weapons[0].IsShield;
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000A2846 File Offset: 0x000A0A46
		public bool IsBanner()
		{
			return this._weapons.Count == 1 && this._weapons[0].WeaponClass == WeaponClass.Banner;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000A2870 File Offset: 0x000A0A70
		public bool IsAnyAmmo()
		{
			using (List<WeaponComponentData>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsAmmo)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000A28CC File Offset: 0x000A0ACC
		public bool HasAnyUsageWithWeaponClass(WeaponClass weaponClass)
		{
			using (List<WeaponComponentData>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.WeaponClass == weaponClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000A2928 File Offset: 0x000A0B28
		public bool HasAnyUsageWithAmmoClass(WeaponClass ammoClass)
		{
			using (List<WeaponComponentData>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AmmoClass == ammoClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000A2984 File Offset: 0x000A0B84
		public bool HasAllUsagesWithAnyWeaponFlag(WeaponFlags flags)
		{
			using (List<WeaponComponentData>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WeaponFlags.HasAnyFlag(flags))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000A29E4 File Offset: 0x000A0BE4
		public bool HasAnyUsageWithoutWeaponFlag(WeaponFlags flags)
		{
			using (List<WeaponComponentData>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WeaponFlags.HasAnyFlag(flags))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000A2A44 File Offset: 0x000A0C44
		public bool HasAnyUsageWithItemUsageSetFlags(ItemObject.ItemUsageSetFlags flags)
		{
			foreach (WeaponComponentData weaponComponentData in this._weapons)
			{
				if (weaponComponentData.ItemUsage != null && !weaponComponentData.ItemUsage.IsEmpty<char>() && MBItem.GetItemUsageSetFlags(weaponComponentData.ItemUsage).HasAllFlags(flags))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x000A2AC0 File Offset: 0x000A0CC0
		public void GatherInformationFromWeapon(out bool weaponHasMelee, out bool weaponHasShield, out bool weaponHasPolearm, out bool weaponHasNonConsumableRanged, out bool weaponHasThrown, out WeaponClass rangedAmmoClass)
		{
			weaponHasMelee = false;
			weaponHasShield = false;
			weaponHasPolearm = false;
			weaponHasNonConsumableRanged = false;
			weaponHasThrown = false;
			rangedAmmoClass = WeaponClass.Undefined;
			foreach (WeaponComponentData weaponComponentData in this._weapons)
			{
				weaponHasMelee = weaponHasMelee || weaponComponentData.IsMeleeWeapon;
				weaponHasShield = weaponHasShield || weaponComponentData.IsShield;
				weaponHasPolearm = weaponComponentData.IsPolearm;
				if (weaponComponentData.IsRangedWeapon)
				{
					weaponHasThrown = weaponComponentData.IsConsumable;
					weaponHasNonConsumableRanged = !weaponHasThrown;
					rangedAmmoClass = weaponComponentData.AmmoClass;
				}
			}
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000A2B6C File Offset: 0x000A0D6C
		public bool GetConsumableIfAny(out WeaponComponentData consumableWeapon)
		{
			consumableWeapon = null;
			if (this._hasAnyConsumableUsage)
			{
				foreach (WeaponComponentData weaponComponentData in this._weapons)
				{
					if (weaponComponentData.IsConsumable)
					{
						consumableWeapon = weaponComponentData;
						break;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x000A2BD4 File Offset: 0x000A0DD4
		public bool IsAnyConsumable()
		{
			return this._hasAnyConsumableUsage;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x000A2BDC File Offset: 0x000A0DDC
		public int GetRangedUsageIndex()
		{
			for (int i = 0; i < this._weapons.Count; i++)
			{
				if (this._weapons[i].IsRangedWeapon)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000A2C18 File Offset: 0x000A0E18
		public MissionWeapon Consume(short count)
		{
			this.Amount -= count;
			return new MissionWeapon(this.Item, this.ItemModifier, this.Banner, count, 0, null);
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x000A2C58 File Offset: 0x000A0E58
		public void ConsumeAmmo(short count)
		{
			if (count > 0)
			{
				MissionWeapon value = this._ammoWeapon.Value;
				value.Amount = count;
				this._ammoWeapon = new MissionWeapon.MissionSubWeapon(value);
				return;
			}
			this._ammoWeapon = null;
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000A2C91 File Offset: 0x000A0E91
		public void SetAmmo(MissionWeapon ammoWeapon)
		{
			this._ammoWeapon = new MissionWeapon.MissionSubWeapon(ammoWeapon);
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000A2CA0 File Offset: 0x000A0EA0
		public void ReloadAmmo(MissionWeapon ammoWeapon, short reloadPhase)
		{
			if (this._ammoWeapon != null && this._ammoWeapon.Value.Amount >= 0)
			{
				ammoWeapon.Amount += this._ammoWeapon.Value.Amount;
			}
			this._ammoWeapon = new MissionWeapon.MissionSubWeapon(ammoWeapon);
			this.ReloadPhase = reloadPhase;
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000A2D00 File Offset: 0x000A0F00
		public void AttachWeapon(MissionWeapon attachedWeapon, ref MatrixFrame attachFrame)
		{
			if (this._attachedWeapons == null)
			{
				this._attachedWeapons = new List<MissionWeapon.MissionSubWeapon>();
				this._attachedWeaponFrames = new List<MatrixFrame>();
			}
			this._attachedWeapons.Add(new MissionWeapon.MissionSubWeapon(attachedWeapon));
			this._attachedWeaponFrames.Add(attachFrame);
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000A2D4D File Offset: 0x000A0F4D
		public void RemoveAttachedWeapon(int attachmentIndex)
		{
			this._attachedWeapons.RemoveAt(attachmentIndex);
			this._attachedWeaponFrames.RemoveAt(attachmentIndex);
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000A2D67 File Offset: 0x000A0F67
		public bool HasEnoughSpaceForAmount(int amount)
		{
			return (int)(this.ModifiedMaxAmount - this.Amount) >= amount;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000A2D7C File Offset: 0x000A0F7C
		public void SetRandomGlossMultiplier(int seed)
		{
			Random random = new Random(seed);
			float num = 1f + (random.NextFloat() * 2f - 1f) * 0.3f;
			this.GlossMultiplier = num;
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000A2DB6 File Offset: 0x000A0FB6
		public void AddExtraModifiedMaxValue(short extraValue)
		{
			this._modifiedMaxDataValue += extraValue;
		}

		// Token: 0x04001048 RID: 4168
		public const short ReloadPhaseCountMax = 10;

		// Token: 0x04001049 RID: 4169
		public static MissionWeapon.OnGetWeaponDataDelegate OnGetWeaponDataHandler;

		// Token: 0x0400104A RID: 4170
		public static readonly MissionWeapon Invalid = new MissionWeapon(null, null, null);

		// Token: 0x0400104D RID: 4173
		private readonly List<WeaponComponentData> _weapons;

		// Token: 0x0400104E RID: 4174
		public int CurrentUsageIndex;

		// Token: 0x0400104F RID: 4175
		private bool _hasAnyConsumableUsage;

		// Token: 0x04001050 RID: 4176
		private short _dataValue;

		// Token: 0x04001051 RID: 4177
		private short _modifiedMaxDataValue;

		// Token: 0x04001055 RID: 4181
		private MissionWeapon.MissionSubWeapon _ammoWeapon;

		// Token: 0x04001056 RID: 4182
		private List<MissionWeapon.MissionSubWeapon> _attachedWeapons;

		// Token: 0x04001057 RID: 4183
		private List<MatrixFrame> _attachedWeaponFrames;

		// Token: 0x020005C5 RID: 1477
		public struct ImpactSoundModifier
		{
			// Token: 0x04001F8B RID: 8075
			public const string ModifierName = "impactModifier";

			// Token: 0x04001F8C RID: 8076
			public const float None = 0f;

			// Token: 0x04001F8D RID: 8077
			public const float ActiveBlock = 0.1f;

			// Token: 0x04001F8E RID: 8078
			public const float ChamberBlocked = 0.2f;

			// Token: 0x04001F8F RID: 8079
			public const float CrushThrough = 0.3f;
		}

		// Token: 0x020005C6 RID: 1478
		private class MissionSubWeapon
		{
			// Token: 0x17000AA6 RID: 2726
			// (get) Token: 0x06003F1C RID: 16156 RVA: 0x000F93C9 File Offset: 0x000F75C9
			// (set) Token: 0x06003F1D RID: 16157 RVA: 0x000F93D1 File Offset: 0x000F75D1
			public MissionWeapon Value { get; private set; }

			// Token: 0x06003F1E RID: 16158 RVA: 0x000F93DA File Offset: 0x000F75DA
			public MissionSubWeapon(MissionWeapon subWeapon)
			{
				this.Value = subWeapon;
			}
		}

		// Token: 0x020005C7 RID: 1479
		// (Invoke) Token: 0x06003F20 RID: 16160
		public delegate void OnGetWeaponDataDelegate(ref WeaponData weaponData, MissionWeapon weapon, bool isFemale, Banner banner, bool needBatchedVersion);
	}
}
