using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030C RID: 780
	public static class ModuleNetworkData
	{
		// Token: 0x06002CE1 RID: 11489 RVA: 0x000AC918 File Offset: 0x000AAB18
		public static EquipmentElement ReadItemReferenceFromPacket(MBObjectManager objectManager, ref bool bufferReadValid)
		{
			MBObjectBase mbobjectBase = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			MBObjectBase mbobjectBase2 = null;
			if (flag)
			{
				mbobjectBase2 = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			}
			ItemObject itemObject = mbobjectBase as ItemObject;
			return new EquipmentElement(itemObject, null, mbobjectBase2 as ItemObject, false);
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000AC95C File Offset: 0x000AAB5C
		public static void WriteItemReferenceToPacket(EquipmentElement equipElement)
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(equipElement.Item, CompressionBasic.GUIDCompressionInfo);
			if (equipElement.CosmeticItem != null)
			{
				GameNetworkMessage.WriteBoolToPacket(true);
				GameNetworkMessage.WriteObjectReferenceToPacket(equipElement.CosmeticItem, CompressionBasic.GUIDCompressionInfo);
				return;
			}
			GameNetworkMessage.WriteBoolToPacket(false);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000AC994 File Offset: 0x000AAB94
		public static MissionWeapon ReadWeaponReferenceFromPacket(MBObjectManager objectManager, ref bool bufferReadValid)
		{
			if (GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid))
			{
				return MissionWeapon.Invalid;
			}
			MBObjectBase mbobjectBase = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ItemDataValueCompressionInfo, ref bufferReadValid);
			int num2 = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponReloadPhaseCompressionInfo, ref bufferReadValid);
			short num3 = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref bufferReadValid);
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			Banner banner = null;
			if (flag)
			{
				string text = GameNetworkMessage.ReadBannerCodeFromPacket(ref bufferReadValid);
				if (bufferReadValid)
				{
					banner = new Banner(text);
				}
			}
			ItemObject itemObject = mbobjectBase as ItemObject;
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			MissionWeapon? missionWeapon = null;
			if (bufferReadValid && flag2)
			{
				MBObjectBase mbobjectBase2 = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
				int num4 = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ItemDataValueCompressionInfo, ref bufferReadValid);
				ItemObject itemObject2 = mbobjectBase2 as ItemObject;
				missionWeapon = new MissionWeapon?(new MissionWeapon(itemObject2, null, banner, (short)num4));
			}
			return new MissionWeapon(itemObject, null, banner, (short)num, (short)num2, missionWeapon)
			{
				CurrentUsageIndex = (int)num3
			};
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000ACA6C File Offset: 0x000AAC6C
		public static void WriteWeaponReferenceToPacket(MissionWeapon weapon)
		{
			GameNetworkMessage.WriteBoolToPacket(weapon.IsEmpty);
			if (!weapon.IsEmpty)
			{
				GameNetworkMessage.WriteObjectReferenceToPacket(weapon.Item, CompressionBasic.GUIDCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)weapon.RawDataForNetwork, CompressionBasic.ItemDataValueCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)weapon.ReloadPhase, CompressionMission.WeaponReloadPhaseCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(weapon.CurrentUsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
				bool flag = weapon.Banner != null;
				GameNetworkMessage.WriteBoolToPacket(flag);
				if (flag)
				{
					GameNetworkMessage.WriteBannerCodeToPacket(weapon.Banner.BannerCode);
				}
				MissionWeapon ammoWeapon = weapon.AmmoWeapon;
				bool flag2 = !ammoWeapon.IsEmpty;
				GameNetworkMessage.WriteBoolToPacket(flag2);
				if (flag2)
				{
					GameNetworkMessage.WriteObjectReferenceToPacket(ammoWeapon.Item, CompressionBasic.GUIDCompressionInfo);
					GameNetworkMessage.WriteIntToPacket((int)ammoWeapon.RawDataForNetwork, CompressionBasic.ItemDataValueCompressionInfo);
				}
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000ACB34 File Offset: 0x000AAD34
		public static MissionWeapon ReadMissileWeaponReferenceFromPacket(MBObjectManager objectManager, ref bool bufferReadValid)
		{
			MBObjectBase mbobjectBase = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			short num = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref bufferReadValid);
			ItemObject itemObject = mbobjectBase as ItemObject;
			return new MissionWeapon(itemObject, null, null, 1)
			{
				CurrentUsageIndex = (int)num
			};
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000ACB74 File Offset: 0x000AAD74
		public static void WriteMissileWeaponReferenceToPacket(MissionWeapon weapon)
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(weapon.Item, CompressionBasic.GUIDCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(weapon.CurrentUsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
		}
	}
}
