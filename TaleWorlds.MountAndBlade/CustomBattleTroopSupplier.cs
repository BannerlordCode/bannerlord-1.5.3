using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000212 RID: 530
	public class CustomBattleTroopSupplier : IMissionTroopSupplier
	{
		// Token: 0x06001EBC RID: 7868 RVA: 0x000694AC File Offset: 0x000676AC
		public CustomBattleTroopSupplier(CustomBattleCombatant customBattleCombatant, bool isPlayerSide, bool isPlayerGeneral, bool isSallyOut, Func<BasicCharacterObject, bool> customAllocationConditions = null)
		{
			this._customBattleCombatant = customBattleCombatant;
			this._customAllocationConditions = customAllocationConditions;
			this._isPlayerSide = isPlayerSide;
			this._isPlayerGeneral = isPlayerSide && isPlayerGeneral;
			this._isSallyOut = isSallyOut;
			this.ArrangePriorities();
			this._nextTroopRank = 0;
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000694FC File Offset: 0x000676FC
		private void ArrangePriorities()
		{
			this._characters = new PriorityQueue<float, BasicCharacterObject>(new GenericComparer<float>());
			int[] array = new int[8];
			int[] array2 = new int[8];
			int i;
			int j;
			for (i = 0; i < 8; i = j + 1)
			{
				array[i] = this._customBattleCombatant.Characters.Count<BasicCharacterObject>((BasicCharacterObject character) => character.DefaultFormationClass == (FormationClass)i);
				j = i;
			}
			UnitSpawnPrioritizations unitSpawnPrioritizations = (this._isPlayerSide ? Game.Current.UnitSpawnPrioritization : UnitSpawnPrioritizations.HighLevel);
			int num = array.Sum();
			float num2 = 1000f;
			foreach (BasicCharacterObject basicCharacterObject in this._customBattleCombatant.Characters)
			{
				FormationClass formationClass = basicCharacterObject.GetFormationClass();
				float num3;
				if (this._isSallyOut)
				{
					num3 = this.GetSallyOutAmbushProbabilityOfTroop(basicCharacterObject, num, ref num2);
				}
				else
				{
					num3 = this.GetDefaultProbabilityOfTroop(basicCharacterObject, num, unitSpawnPrioritizations, ref num2, ref array, ref array2);
				}
				array[(int)formationClass]--;
				array2[(int)formationClass]++;
				this._characters.Enqueue(num3, basicCharacterObject);
			}
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x00069648 File Offset: 0x00067848
		private float GetSallyOutAmbushProbabilityOfTroop(BasicCharacterObject character, int troopCountTotal, ref float heroProbability)
		{
			float num = 0f;
			if (character.IsHero)
			{
				float num2 = heroProbability;
				heroProbability = num2 - 1f;
				num = num2;
			}
			else
			{
				num += (float)character.Level;
				if (character.HasMount())
				{
					num += 100f;
				}
			}
			return num;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x00069690 File Offset: 0x00067890
		private float GetDefaultProbabilityOfTroop(BasicCharacterObject character, int troopCountTotal, UnitSpawnPrioritizations unitSpawnPrioritization, ref float heroProbability, ref int[] troopCountByFormationType, ref int[] enqueuedTroopCountByFormationType)
		{
			FormationClass formationClass = character.GetFormationClass();
			float num = (float)troopCountByFormationType[(int)formationClass] / (float)((unitSpawnPrioritization == UnitSpawnPrioritizations.Homogeneous) ? (enqueuedTroopCountByFormationType[(int)formationClass] + 1) : troopCountTotal);
			float num2;
			if (!character.IsHero)
			{
				num2 = num;
			}
			else
			{
				float num3 = heroProbability;
				heroProbability = num3 - 1f;
				num2 = num3;
			}
			float num4 = num2;
			if (!character.IsHero && (unitSpawnPrioritization == UnitSpawnPrioritizations.HighLevel || unitSpawnPrioritization == UnitSpawnPrioritizations.LowLevel))
			{
				num4 += (float)character.Level;
				if (unitSpawnPrioritization == UnitSpawnPrioritizations.LowLevel)
				{
					num4 *= -1f;
				}
			}
			return num4;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x00069700 File Offset: 0x00067900
		public IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate)
		{
			List<BasicCharacterObject> list = this.AllocateTroops(numberToAllocate);
			CustomBattleAgentOrigin[] array = new CustomBattleAgentOrigin[list.Count];
			this._numAllocated += list.Count;
			for (int i = 0; i < array.Length; i++)
			{
				UniqueTroopDescriptor uniqueTroopDescriptor = new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed);
				CustomBattleAgentOrigin[] array2 = array;
				int num = i;
				CustomBattleCombatant customBattleCombatant = this._customBattleCombatant;
				BasicCharacterObject basicCharacterObject = list[i];
				bool isPlayerSide = this._isPlayerSide;
				int nextTroopRank = this._nextTroopRank;
				this._nextTroopRank = nextTroopRank + 1;
				array2[num] = new CustomBattleAgentOrigin(customBattleCombatant, basicCharacterObject, this, isPlayerSide, nextTroopRank, uniqueTroopDescriptor);
			}
			if (array.Length < numberToAllocate)
			{
				this._anyTroopRemainsToBeSupplied = false;
			}
			return array;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00069794 File Offset: 0x00067994
		public IAgentOriginBase SupplyOneTroop()
		{
			BasicCharacterObject basicCharacterObject = this.AllocateTroop();
			if (basicCharacterObject != null)
			{
				UniqueTroopDescriptor uniqueTroopDescriptor = new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed);
				CustomBattleCombatant customBattleCombatant = this._customBattleCombatant;
				BasicCharacterObject basicCharacterObject2 = basicCharacterObject;
				bool isPlayerSide = this._isPlayerSide;
				int nextTroopRank = this._nextTroopRank;
				this._nextTroopRank = nextTroopRank + 1;
				return new CustomBattleAgentOrigin(customBattleCombatant, basicCharacterObject2, this, isPlayerSide, nextTroopRank, uniqueTroopDescriptor);
			}
			this._anyTroopRemainsToBeSupplied = false;
			return null;
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000697EC File Offset: 0x000679EC
		public IEnumerable<IAgentOriginBase> GetAllTroops()
		{
			CustomBattleAgentOrigin[] array = new CustomBattleAgentOrigin[this._customBattleCombatant.Characters.Count<BasicCharacterObject>()];
			int num = 0;
			foreach (BasicCharacterObject basicCharacterObject in this._customBattleCombatant.Characters)
			{
				array[num] = new CustomBattleAgentOrigin(this._customBattleCombatant, basicCharacterObject, this, this._isPlayerSide, -1, default(UniqueTroopDescriptor));
				num++;
			}
			return array;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x00069878 File Offset: 0x00067A78
		public BasicCharacterObject GetGeneralCharacter()
		{
			return this._customBattleCombatant.General;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x00069888 File Offset: 0x00067A88
		private List<BasicCharacterObject> AllocateTroops(int numberToAllocate)
		{
			if (numberToAllocate > this._characters.Count)
			{
				numberToAllocate = this._characters.Count;
			}
			List<BasicCharacterObject> list = new List<BasicCharacterObject>();
			while (numberToAllocate > 0 && this._characters.Count > 0)
			{
				BasicCharacterObject basicCharacterObject = this._characters.DequeueValue();
				if (this._customAllocationConditions == null || this._customAllocationConditions(basicCharacterObject))
				{
					list.Add(basicCharacterObject);
					numberToAllocate--;
				}
			}
			return list;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000698FC File Offset: 0x00067AFC
		private BasicCharacterObject AllocateTroop()
		{
			BasicCharacterObject basicCharacterObject = null;
			while (this._characters.Count > 0)
			{
				BasicCharacterObject basicCharacterObject2 = this._characters.DequeueValue();
				if (this._customAllocationConditions == null || this._customAllocationConditions(basicCharacterObject2))
				{
					basicCharacterObject = basicCharacterObject2;
					break;
				}
			}
			return basicCharacterObject;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x00069942 File Offset: 0x00067B42
		public void OnTroopWounded()
		{
			this._numWounded++;
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00069952 File Offset: 0x00067B52
		public void OnTroopKilled()
		{
			this._numKilled++;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00069962 File Offset: 0x00067B62
		public void OnTroopRouted()
		{
			this._numRouted++;
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x00069972 File Offset: 0x00067B72
		public int NumRemovedTroops
		{
			get
			{
				return this._numWounded + this._numKilled + this._numRouted;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x00069988 File Offset: 0x00067B88
		public int NumTroopsNotSupplied
		{
			get
			{
				return this._characters.Count - this._numAllocated;
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0006999C File Offset: 0x00067B9C
		public int GetNumberOfPlayerControllableTroops()
		{
			return this._customBattleCombatant.CountOfCharacters;
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x000699A9 File Offset: 0x00067BA9
		public bool AnyTroopRemainsToBeSupplied
		{
			get
			{
				return this._anyTroopRemainsToBeSupplied;
			}
		}

		// Token: 0x04000A78 RID: 2680
		private readonly CustomBattleCombatant _customBattleCombatant;

		// Token: 0x04000A79 RID: 2681
		private PriorityQueue<float, BasicCharacterObject> _characters;

		// Token: 0x04000A7A RID: 2682
		private int _numAllocated;

		// Token: 0x04000A7B RID: 2683
		private int _numWounded;

		// Token: 0x04000A7C RID: 2684
		private int _numKilled;

		// Token: 0x04000A7D RID: 2685
		private int _numRouted;

		// Token: 0x04000A7E RID: 2686
		private Func<BasicCharacterObject, bool> _customAllocationConditions;

		// Token: 0x04000A7F RID: 2687
		private bool _anyTroopRemainsToBeSupplied = true;

		// Token: 0x04000A80 RID: 2688
		private readonly bool _isPlayerSide;

		// Token: 0x04000A81 RID: 2689
		private readonly bool _isPlayerGeneral;

		// Token: 0x04000A82 RID: 2690
		private readonly bool _isSallyOut;

		// Token: 0x04000A83 RID: 2691
		private int _nextTroopRank;
	}
}
