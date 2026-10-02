using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x0200021D RID: 541
	public class NarrativeMenuCharacter
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060020CE RID: 8398 RVA: 0x00093F34 File Offset: 0x00092134
		// (set) Token: 0x060020CF RID: 8399 RVA: 0x00093F3C File Offset: 0x0009213C
		public BodyProperties BodyProperties { get; private set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060020D0 RID: 8400 RVA: 0x00093F45 File Offset: 0x00092145
		// (set) Token: 0x060020D1 RID: 8401 RVA: 0x00093F4D File Offset: 0x0009214D
		public int Race { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00093F56 File Offset: 0x00092156
		// (set) Token: 0x060020D3 RID: 8403 RVA: 0x00093F5E File Offset: 0x0009215E
		public bool IsFemale { get; set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x00093F67 File Offset: 0x00092167
		// (set) Token: 0x060020D5 RID: 8405 RVA: 0x00093F6F File Offset: 0x0009216F
		public MBEquipmentRoster Equipment { get; private set; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060020D6 RID: 8406 RVA: 0x00093F78 File Offset: 0x00092178
		// (set) Token: 0x060020D7 RID: 8407 RVA: 0x00093F80 File Offset: 0x00092180
		public string AnimationId { get; private set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x00093F89 File Offset: 0x00092189
		// (set) Token: 0x060020D9 RID: 8409 RVA: 0x00093F91 File Offset: 0x00092191
		public MountCreationKey MountCreationKey { get; private set; }

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x00093F9A File Offset: 0x0009219A
		// (set) Token: 0x060020DB RID: 8411 RVA: 0x00093FA2 File Offset: 0x000921A2
		public string Item1Id { get; private set; }

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x00093FAB File Offset: 0x000921AB
		// (set) Token: 0x060020DD RID: 8413 RVA: 0x00093FB3 File Offset: 0x000921B3
		public string Item2Id { get; private set; }

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x00093FBC File Offset: 0x000921BC
		// (set) Token: 0x060020DF RID: 8415 RVA: 0x00093FC4 File Offset: 0x000921C4
		public EquipmentIndex RightHandEquipmentIndex { get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x00093FCD File Offset: 0x000921CD
		// (set) Token: 0x060020E1 RID: 8417 RVA: 0x00093FD5 File Offset: 0x000921D5
		public EquipmentIndex LeftHandEquipmentIndex { get; private set; }

		// Token: 0x060020E2 RID: 8418 RVA: 0x00093FE0 File Offset: 0x000921E0
		public NarrativeMenuCharacter(string stringId, BodyProperties bodyProperties, int race, bool isFemale)
		{
			this.StringId = stringId;
			this.BodyProperties = bodyProperties;
			this.Race = race;
			this.IsFemale = isFemale;
			this.IsHuman = true;
			this.SpawnPointEntityId = "spawnpoint_player_1";
			this.AnimationId = "act_inventory_idle_start";
			this.Equipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00094047 File Offset: 0x00092247
		public NarrativeMenuCharacter(string stringId)
		{
			this.StringId = stringId;
			this.IsHuman = false;
			this.SpawnPointEntityId = "spawnpoint_mount_1";
			this.AnimationId = "act_inventory_idle_start";
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00094073 File Offset: 0x00092273
		public void UpdateBodyProperties(BodyProperties bodyProperties, int race, bool isFemale)
		{
			this.BodyProperties = bodyProperties;
			this.Race = race;
			this.IsFemale = isFemale;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x0009408A File Offset: 0x0009228A
		public void SetEquipment(MBEquipmentRoster equipment)
		{
			this.Equipment = equipment;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00094093 File Offset: 0x00092293
		public void SetAnimationId(string animationId)
		{
			this.AnimationId = animationId;
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0009409C File Offset: 0x0009229C
		public void SetRightHandItem(string itemId)
		{
			this.Item1Id = itemId;
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000940A5 File Offset: 0x000922A5
		public void SetLeftHandItem(string itemId)
		{
			this.Item2Id = itemId;
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000940AE File Offset: 0x000922AE
		public void EquipRightHandItemWithEquipmentIndex(EquipmentIndex item)
		{
			this.RightHandEquipmentIndex = item;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000940B7 File Offset: 0x000922B7
		public void EquipLeftHandItemWithEquipmentIndex(EquipmentIndex item)
		{
			this.LeftHandEquipmentIndex = item;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000940C0 File Offset: 0x000922C0
		public void SetSpawnPointEntityId(string spawnPointEntityId)
		{
			this.SpawnPointEntityId = spawnPointEntityId;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000940CC File Offset: 0x000922CC
		public void ChangeAge(float age)
		{
			BodyProperties bodyProperties = this.BodyProperties;
			this.BodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, age);
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000940EE File Offset: 0x000922EE
		public void SetMountCreationKey(MountCreationKey mountCreationKey)
		{
			this.MountCreationKey = mountCreationKey;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000940F7 File Offset: 0x000922F7
		public void SetHorseItemId(string itemId)
		{
			this.Item1Id = itemId;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00094100 File Offset: 0x00092300
		public void SetHarnessItemId(string itemId)
		{
			this.Item2Id = itemId;
		}

		// Token: 0x0400098F RID: 2447
		public readonly string StringId;

		// Token: 0x04000990 RID: 2448
		public readonly bool IsHuman;

		// Token: 0x04000991 RID: 2449
		public string SpawnPointEntityId;
	}
}
