using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x02000049 RID: 73
	public class MapConversationTableauData
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000268 RID: 616 RVA: 0x000172A0 File Offset: 0x000154A0
		// (set) Token: 0x06000269 RID: 617 RVA: 0x000172A8 File Offset: 0x000154A8
		public ConversationCharacterData PlayerCharacterData { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000172B1 File Offset: 0x000154B1
		// (set) Token: 0x0600026B RID: 619 RVA: 0x000172B9 File Offset: 0x000154B9
		public ConversationCharacterData ConversationPartnerData { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600026C RID: 620 RVA: 0x000172C2 File Offset: 0x000154C2
		// (set) Token: 0x0600026D RID: 621 RVA: 0x000172CA File Offset: 0x000154CA
		public TerrainType ConversationTerrainType { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600026E RID: 622 RVA: 0x000172D3 File Offset: 0x000154D3
		// (set) Token: 0x0600026F RID: 623 RVA: 0x000172DB File Offset: 0x000154DB
		public float TimeOfDay { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000270 RID: 624 RVA: 0x000172E4 File Offset: 0x000154E4
		// (set) Token: 0x06000271 RID: 625 RVA: 0x000172EC File Offset: 0x000154EC
		public bool IsCurrentTerrainUnderSnow { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000272 RID: 626 RVA: 0x000172F5 File Offset: 0x000154F5
		// (set) Token: 0x06000273 RID: 627 RVA: 0x000172FD File Offset: 0x000154FD
		public Settlement Settlement { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00017306 File Offset: 0x00015506
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0001730E File Offset: 0x0001550E
		public string LocationId { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00017317 File Offset: 0x00015517
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0001731F File Offset: 0x0001551F
		public bool IsSnowing { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00017328 File Offset: 0x00015528
		// (set) Token: 0x06000279 RID: 633 RVA: 0x00017330 File Offset: 0x00015530
		public bool IsRaining { get; private set; }

		// Token: 0x0600027A RID: 634 RVA: 0x00017339 File Offset: 0x00015539
		private MapConversationTableauData()
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00017344 File Offset: 0x00015544
		public static MapConversationTableauData CreateFrom(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, TerrainType terrainType, float timeOfDay, bool isCurrentTerrainUnderSnow, Settlement settlement, string locationId, bool isRaining, bool isSnowing)
		{
			return new MapConversationTableauData
			{
				PlayerCharacterData = playerCharacterData,
				ConversationPartnerData = conversationPartnerData,
				ConversationTerrainType = terrainType,
				TimeOfDay = timeOfDay,
				IsCurrentTerrainUnderSnow = isCurrentTerrainUnderSnow,
				Settlement = settlement,
				LocationId = locationId,
				IsRaining = isRaining,
				IsSnowing = isSnowing
			};
		}
	}
}
