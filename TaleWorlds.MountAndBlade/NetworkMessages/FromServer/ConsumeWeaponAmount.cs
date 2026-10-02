using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000084 RID: 132
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ConsumeWeaponAmount : GameNetworkMessage
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00008E11 File Offset: 0x00007011
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00008E19 File Offset: 0x00007019
		public MissionObjectId SpawnedItemEntityId { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00008E22 File Offset: 0x00007022
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00008E2A File Offset: 0x0000702A
		public short ConsumedAmount { get; private set; }

		// Token: 0x060004E3 RID: 1251 RVA: 0x00008E33 File Offset: 0x00007033
		public ConsumeWeaponAmount(MissionObjectId spawnedItemEntityId, short consumedAmount)
		{
			this.SpawnedItemEntityId = spawnedItemEntityId;
			this.ConsumedAmount = consumedAmount;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00008E49 File Offset: 0x00007049
		public ConsumeWeaponAmount()
		{
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00008E51 File Offset: 0x00007051
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SpawnedItemEntityId);
			GameNetworkMessage.WriteIntToPacket((int)this.ConsumedAmount, CompressionBasic.ItemDataValueCompressionInfo);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00008E70 File Offset: 0x00007070
		protected override bool OnRead()
		{
			bool flag = true;
			this.SpawnedItemEntityId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.ConsumedAmount = (short)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ItemDataValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00008EA0 File Offset: 0x000070A0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00008EA5 File Offset: 0x000070A5
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Consumed ", this.ConsumedAmount, " from ", this.SpawnedItemEntityId });
		}
	}
}
