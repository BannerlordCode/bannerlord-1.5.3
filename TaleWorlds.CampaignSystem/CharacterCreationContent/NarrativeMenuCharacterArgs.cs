using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x0200021E RID: 542
	public readonly struct NarrativeMenuCharacterArgs
	{
		// Token: 0x060020F0 RID: 8432 RVA: 0x0009410C File Offset: 0x0009230C
		public NarrativeMenuCharacterArgs(string characterId, int age, string equipmentId, string animationId, string spawnPointEntityId, string leftHandItemId = "", string rightHandItemId = "", MountCreationKey mountCreationKey = null, bool isHuman = true, bool isFemale = false)
		{
			this.CharacterId = characterId;
			this.Age = age;
			this.EquipmentId = equipmentId;
			this.AnimationId = animationId;
			this.SpawnPointEntityId = spawnPointEntityId;
			this.LeftHandItemId = leftHandItemId;
			this.RightHandItemId = rightHandItemId;
			this.MountCreationKey = mountCreationKey;
			this.IsHuman = isHuman;
			this.IsFemale = isFemale;
		}

		// Token: 0x0400099C RID: 2460
		public readonly string CharacterId;

		// Token: 0x0400099D RID: 2461
		public readonly int Age;

		// Token: 0x0400099E RID: 2462
		public readonly string EquipmentId;

		// Token: 0x0400099F RID: 2463
		public readonly string AnimationId;

		// Token: 0x040009A0 RID: 2464
		public readonly string SpawnPointEntityId;

		// Token: 0x040009A1 RID: 2465
		public readonly string LeftHandItemId;

		// Token: 0x040009A2 RID: 2466
		public readonly string RightHandItemId;

		// Token: 0x040009A3 RID: 2467
		public readonly MountCreationKey MountCreationKey;

		// Token: 0x040009A4 RID: 2468
		public readonly bool IsHuman;

		// Token: 0x040009A5 RID: 2469
		public readonly bool IsFemale;
	}
}
