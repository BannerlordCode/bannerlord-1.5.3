using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000244 RID: 580
	public struct ConversationCharacterData : ISerializableObject
	{
		// Token: 0x0600230C RID: 8972 RVA: 0x0009BA68 File Offset: 0x00099C68
		public ConversationCharacterData(CharacterObject character, PartyBase party = null, bool noHorse = false, bool noWeapon = false, bool spawnAfterFight = false, bool isCivilianEquipmentRequiredForLeader = false, bool isCivilianEquipmentRequiredForBodyGuardCharacters = false, bool noBodyguards = false)
		{
			this.Character = character;
			this.Party = party;
			this.NoHorse = noHorse;
			this.NoWeapon = noWeapon;
			this.NoBodyguards = noBodyguards;
			this.SpawnedAfterFight = spawnAfterFight;
			this.IsCivilianEquipmentRequiredForLeader = isCivilianEquipmentRequiredForLeader;
			this.IsCivilianEquipmentRequiredForBodyGuardCharacters = isCivilianEquipmentRequiredForBodyGuardCharacters;
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0009BAA8 File Offset: 0x00099CA8
		void ISerializableObject.DeserializeFrom(IReader reader)
		{
			MBGUID mbguid = new MBGUID(reader.ReadUInt());
			this.Character = (CharacterObject)MBObjectManager.Instance.GetObject(mbguid);
			int num = reader.ReadInt();
			this.Party = ConversationCharacterData.FindParty(num);
			this.NoHorse = reader.ReadBool();
			this.NoWeapon = reader.ReadBool();
			this.SpawnedAfterFight = reader.ReadBool();
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0009BB10 File Offset: 0x00099D10
		void ISerializableObject.SerializeTo(IWriter writer)
		{
			writer.WriteUInt(this.Character.Id.InternalValue);
			writer.WriteInt((this.Party == null) ? (-1) : this.Party.Index);
			writer.WriteBool(this.NoHorse);
			writer.WriteBool(this.NoWeapon);
			writer.WriteBool(this.SpawnedAfterFight);
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0009BB78 File Offset: 0x00099D78
		private static PartyBase FindParty(int index)
		{
			MobileParty mobileParty = Campaign.Current.CampaignObjectManager.FindFirst<MobileParty>((MobileParty x) => x.Party.Index == index);
			if (mobileParty != null)
			{
				return mobileParty.Party;
			}
			Settlement settlement = Settlement.All.FirstOrDefaultQ<Settlement>((Settlement x) => x.Party.Index == index);
			if (settlement != null)
			{
				return settlement.Party;
			}
			return null;
		}

		// Token: 0x04000A40 RID: 2624
		public CharacterObject Character;

		// Token: 0x04000A41 RID: 2625
		public PartyBase Party;

		// Token: 0x04000A42 RID: 2626
		public bool NoHorse;

		// Token: 0x04000A43 RID: 2627
		public bool NoWeapon;

		// Token: 0x04000A44 RID: 2628
		public bool NoBodyguards;

		// Token: 0x04000A45 RID: 2629
		public bool SpawnedAfterFight;

		// Token: 0x04000A46 RID: 2630
		public bool IsCivilianEquipmentRequiredForLeader;

		// Token: 0x04000A47 RID: 2631
		public bool IsCivilianEquipmentRequiredForBodyGuardCharacters;
	}
}
