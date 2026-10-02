using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000083 RID: 131
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CombatLogNetworkMessage : GameNetworkMessage
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00008991 File Offset: 0x00006B91
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00008999 File Offset: 0x00006B99
		public int AttackerAgentIndex { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000089A2 File Offset: 0x00006BA2
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x000089AA File Offset: 0x00006BAA
		public int VictimAgentIndex { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000089B3 File Offset: 0x00006BB3
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x000089BB File Offset: 0x00006BBB
		public MissionObjectId MissionObjectHitId { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x000089C4 File Offset: 0x00006BC4
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x000089CC File Offset: 0x00006BCC
		public DamageTypes DamageType { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x000089D5 File Offset: 0x00006BD5
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x000089DD File Offset: 0x00006BDD
		public bool CrushedThrough { get; private set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000089E6 File Offset: 0x00006BE6
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x000089EE File Offset: 0x00006BEE
		public bool Chamber { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000089F7 File Offset: 0x00006BF7
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x000089FF File Offset: 0x00006BFF
		public bool IsRangedAttack { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00008A08 File Offset: 0x00006C08
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00008A10 File Offset: 0x00006C10
		public bool IsFriendlyFire { get; private set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00008A19 File Offset: 0x00006C19
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00008A21 File Offset: 0x00006C21
		public bool IsFatalDamage { get; private set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00008A2A File Offset: 0x00006C2A
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00008A32 File Offset: 0x00006C32
		public bool IsSpecialDamage { get; private set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00008A3B File Offset: 0x00006C3B
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00008A43 File Offset: 0x00006C43
		public BoneBodyPartType BodyPartHit { get; private set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00008A4C File Offset: 0x00006C4C
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00008A54 File Offset: 0x00006C54
		public float HitSpeed { get; private set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00008A5D File Offset: 0x00006C5D
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00008A65 File Offset: 0x00006C65
		public float Distance { get; private set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00008A6E File Offset: 0x00006C6E
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00008A76 File Offset: 0x00006C76
		public int InflictedDamage { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00008A7F File Offset: 0x00006C7F
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00008A87 File Offset: 0x00006C87
		public int AbsorbedDamage { get; private set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00008A90 File Offset: 0x00006C90
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00008A98 File Offset: 0x00006C98
		public int ModifiedDamage { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00008AA1 File Offset: 0x00006CA1
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00008AA9 File Offset: 0x00006CA9
		public int ReflectedDamage { get; private set; }

		// Token: 0x060004D9 RID: 1241 RVA: 0x00008AB2 File Offset: 0x00006CB2
		public CombatLogNetworkMessage()
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00008ABC File Offset: 0x00006CBC
		public CombatLogNetworkMessage(int attackerAgentIndex, int victimAgentIndex, MissionObjectId missionObjectHitId, CombatLogData combatLogData)
		{
			this.AttackerAgentIndex = attackerAgentIndex;
			this.VictimAgentIndex = victimAgentIndex;
			this.MissionObjectHitId = missionObjectHitId;
			this.DamageType = combatLogData.DamageType;
			this.CrushedThrough = combatLogData.CrushedThrough;
			this.Chamber = combatLogData.Chamber;
			this.IsRangedAttack = combatLogData.IsRangedAttack;
			this.IsFriendlyFire = combatLogData.IsFriendlyFire;
			this.IsFatalDamage = combatLogData.IsFatalDamage;
			this.IsSpecialDamage = combatLogData.IsSpecialDamage;
			this.BodyPartHit = combatLogData.BodyPartHit;
			this.HitSpeed = combatLogData.HitSpeed;
			this.Distance = combatLogData.Distance;
			this.InflictedDamage = combatLogData.InflictedDamage;
			this.AbsorbedDamage = combatLogData.AbsorbedDamage;
			this.ModifiedDamage = combatLogData.ModifiedDamage;
			this.ReflectedDamage = combatLogData.ReflectedDamage;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00008B9C File Offset: 0x00006D9C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AttackerAgentIndex);
			GameNetworkMessage.WriteAgentIndexToPacket(this.VictimAgentIndex);
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectHitId);
			GameNetworkMessage.WriteIntToPacket((int)this.DamageType, CompressionBasic.AgentHitDamageTypeCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.CrushedThrough);
			GameNetworkMessage.WriteBoolToPacket(this.Chamber);
			GameNetworkMessage.WriteBoolToPacket(this.IsRangedAttack);
			GameNetworkMessage.WriteBoolToPacket(this.IsFriendlyFire);
			GameNetworkMessage.WriteBoolToPacket(this.IsFatalDamage);
			GameNetworkMessage.WriteBoolToPacket(this.IsSpecialDamage);
			GameNetworkMessage.WriteIntToPacket((int)this.BodyPartHit, CompressionBasic.AgentHitBodyPartCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.HitSpeed, CompressionBasic.AgentHitRelativeSpeedCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Distance, CompressionBasic.AgentHitRelativeSpeedCompressionInfo);
			this.AbsorbedDamage = MBMath.ClampInt(this.AbsorbedDamage, 0, 2000);
			this.InflictedDamage = MBMath.ClampInt(this.InflictedDamage, 0, 2000);
			this.ModifiedDamage = MBMath.ClampInt(this.ModifiedDamage, -2000, 2000);
			this.ReflectedDamage = MBMath.ClampInt(this.ReflectedDamage, 0, 2000);
			GameNetworkMessage.WriteIntToPacket(this.AbsorbedDamage, CompressionBasic.AgentHitDamageCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.InflictedDamage, CompressionBasic.AgentHitDamageCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ModifiedDamage, CompressionBasic.AgentHitModifiedDamageCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ReflectedDamage, CompressionBasic.AgentHitDamageCompressionInfo);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00008CEC File Offset: 0x00006EEC
		protected override bool OnRead()
		{
			bool flag = true;
			this.AttackerAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.VictimAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.MissionObjectHitId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.DamageType = (DamageTypes)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageTypeCompressionInfo, ref flag);
			this.CrushedThrough = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.Chamber = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsRangedAttack = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsFriendlyFire = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsFatalDamage = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsSpecialDamage = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.BodyPartHit = (BoneBodyPartType)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitBodyPartCompressionInfo, ref flag);
			this.HitSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AgentHitRelativeSpeedCompressionInfo, ref flag);
			this.Distance = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AgentHitRelativeSpeedCompressionInfo, ref flag);
			this.AbsorbedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageCompressionInfo, ref flag);
			this.InflictedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageCompressionInfo, ref flag);
			this.ModifiedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitModifiedDamageCompressionInfo, ref flag);
			this.ReflectedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00008E02 File Offset: 0x00007002
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00008E0A File Offset: 0x0000700A
		protected override string OnGetLogFormat()
		{
			return "Agent got hit.";
		}
	}
}
