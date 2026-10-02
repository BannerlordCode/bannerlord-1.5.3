using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TroopSuppliers
{
	// Token: 0x020000B6 RID: 182
	public class PartyGroupTroopSupplier : IMissionTroopSupplier
	{
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x0005E20D File Offset: 0x0005C40D
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x0005E215 File Offset: 0x0005C415
		internal MapEventSide PartyGroup { get; private set; }

		// Token: 0x0600140B RID: 5131 RVA: 0x0005E220 File Offset: 0x0005C420
		public PartyGroupTroopSupplier(MapEvent mapEvent, BattleSideEnum side, FlattenedTroopRoster priorTroops = null, Func<UniqueTroopDescriptor, MapEventParty, bool> customAllocationConditions = null)
		{
			this._customAllocationConditions = customAllocationConditions;
			this.PartyGroup = mapEvent.GetMapEventSide(side);
			this._isPlayerSide = mapEvent.PlayerSide == side;
			this._initialTroopCount = this.PartyGroup.TroopCount;
			this.PartyGroup.MakeReadyForMission(priorTroops);
			this._nextTroopRank = 0;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0005E284 File Offset: 0x0005C484
		public IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate)
		{
			List<UniqueTroopDescriptor> list = null;
			this.PartyGroup.AllocateTroops(ref list, numberToAllocate, this._customAllocationConditions);
			PartyGroupAgentOrigin[] array = new PartyGroupAgentOrigin[list.Count];
			this._numAllocated += list.Count;
			for (int i = 0; i < array.Length; i++)
			{
				PartyGroupAgentOrigin[] array2 = array;
				int num = i;
				UniqueTroopDescriptor uniqueTroopDescriptor = list[i];
				int nextTroopRank = this._nextTroopRank;
				this._nextTroopRank = nextTroopRank + 1;
				array2[num] = new PartyGroupAgentOrigin(this, uniqueTroopDescriptor, nextTroopRank);
			}
			if (array.Length < numberToAllocate)
			{
				this._anyTroopRemainsToBeSupplied = false;
			}
			return array;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0005E304 File Offset: 0x0005C504
		public IAgentOriginBase SupplyOneTroop()
		{
			UniqueTroopDescriptor uniqueTroopDescriptor;
			if (this.PartyGroup.AllocateTroop(this._customAllocationConditions, out uniqueTroopDescriptor))
			{
				UniqueTroopDescriptor uniqueTroopDescriptor2 = uniqueTroopDescriptor;
				int nextTroopRank = this._nextTroopRank;
				this._nextTroopRank = nextTroopRank + 1;
				IAgentOriginBase agentOriginBase = new PartyGroupAgentOrigin(this, uniqueTroopDescriptor2, nextTroopRank);
				this._anyTroopRemainsToBeSupplied = this._anyTroopRemainsToBeSupplied && this.PartyGroup.HasReadyTroops;
				return agentOriginBase;
			}
			this._anyTroopRemainsToBeSupplied = false;
			return null;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0005E364 File Offset: 0x0005C564
		public IEnumerable<IAgentOriginBase> GetAllTroops()
		{
			List<UniqueTroopDescriptor> list = null;
			this.PartyGroup.GetAllTroops(ref list);
			PartyGroupAgentOrigin[] array = new PartyGroupAgentOrigin[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PartyGroupAgentOrigin(this, list[i], i);
			}
			return array;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0005E3AC File Offset: 0x0005C5AC
		public BasicCharacterObject GetGeneralCharacter()
		{
			return this.PartyGroup.LeaderParty.General;
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0005E3BE File Offset: 0x0005C5BE
		public int NumRemovedTroops
		{
			get
			{
				return this._numWounded + this._numKilled + this._numRouted;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0005E3D4 File Offset: 0x0005C5D4
		public int NumTroopsNotSupplied
		{
			get
			{
				return this._initialTroopCount - this._numAllocated;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0005E3E3 File Offset: 0x0005C5E3
		public bool AnyTroopRemainsToBeSupplied
		{
			get
			{
				return this._anyTroopRemainsToBeSupplied;
			}
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0005E3EC File Offset: 0x0005C5EC
		public int GetNumberOfPlayerControllableTroops()
		{
			int num = 0;
			foreach (MapEventParty mapEventParty in this.PartyGroup.Parties)
			{
				PartyBase party = mapEventParty.Party;
				if (PartyBase.IsPartyUnderPlayerCommand(party) || (party.Side == PartyBase.MainParty.Side && this.PartyGroup.MapEvent.IsPlayerSergeant()))
				{
					num += party.NumberOfHealthyMembers;
				}
			}
			return num;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0005E47C File Offset: 0x0005C67C
		public void OnTroopWounded(UniqueTroopDescriptor troopDescriptor)
		{
			this._numWounded++;
			this.PartyGroup.OnTroopWounded(troopDescriptor);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0005E498 File Offset: 0x0005C698
		public void OnTroopKilled(UniqueTroopDescriptor troopDescriptor)
		{
			this._numKilled++;
			this.PartyGroup.OnTroopKilled(troopDescriptor);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0005E4B4 File Offset: 0x0005C6B4
		public void OnTroopRouted(UniqueTroopDescriptor troopDescriptor, bool isOrderRetreat)
		{
			this._numRouted++;
			this.PartyGroup.OnTroopRouted(troopDescriptor, isOrderRetreat);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0005E4D1 File Offset: 0x0005C6D1
		internal CharacterObject GetTroop(UniqueTroopDescriptor troopDescriptor)
		{
			return this.PartyGroup.GetAllocatedTroop(troopDescriptor) ?? this.PartyGroup.GetReadyTroop(troopDescriptor);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0005E4F0 File Offset: 0x0005C6F0
		public PartyBase GetParty(UniqueTroopDescriptor troopDescriptor)
		{
			PartyBase partyBase = this.PartyGroup.GetAllocatedTroopParty(troopDescriptor);
			if (partyBase == null)
			{
				partyBase = this.PartyGroup.GetReadyTroopParty(troopDescriptor);
			}
			return partyBase;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0005E51B File Offset: 0x0005C71B
		public void OnTroopScoreHit(UniqueTroopDescriptor descriptor, BasicCharacterObject attackedCharacter, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			this.PartyGroup.OnTroopScoreHit(descriptor, (CharacterObject)attackedCharacter, damage, isFatal, isTeamKill, attackerWeapon, false);
		}

		// Token: 0x04000684 RID: 1668
		private readonly int _initialTroopCount;

		// Token: 0x04000685 RID: 1669
		private int _numAllocated;

		// Token: 0x04000686 RID: 1670
		private int _numWounded;

		// Token: 0x04000687 RID: 1671
		private int _numKilled;

		// Token: 0x04000688 RID: 1672
		private int _numRouted;

		// Token: 0x04000689 RID: 1673
		private bool _isPlayerSide;

		// Token: 0x0400068A RID: 1674
		private Func<UniqueTroopDescriptor, MapEventParty, bool> _customAllocationConditions;

		// Token: 0x0400068B RID: 1675
		private bool _anyTroopRemainsToBeSupplied = true;

		// Token: 0x0400068C RID: 1676
		private int _nextTroopRank;
	}
}
