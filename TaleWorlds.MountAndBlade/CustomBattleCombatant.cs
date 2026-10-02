using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000211 RID: 529
	public class CustomBattleCombatant : IBattleCombatant
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x00069314 File Offset: 0x00067514
		// (set) Token: 0x06001EA6 RID: 7846 RVA: 0x0006931C File Offset: 0x0006751C
		public TextObject Name { get; private set; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x00069325 File Offset: 0x00067525
		// (set) Token: 0x06001EA8 RID: 7848 RVA: 0x0006932D File Offset: 0x0006752D
		public BattleSideEnum Side { get; set; }

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x00069336 File Offset: 0x00067536
		public BasicCharacterObject General
		{
			get
			{
				return this._general;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x0006933E File Offset: 0x0006753E
		// (set) Token: 0x06001EAB RID: 7851 RVA: 0x00069346 File Offset: 0x00067546
		public BasicCultureObject BasicCulture { get; private set; }

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001EAC RID: 7852 RVA: 0x0006934F File Offset: 0x0006754F
		public Tuple<uint, uint> PrimaryColorPair
		{
			get
			{
				return new Tuple<uint, uint>(this.Banner.GetPrimaryColor(), this.Banner.GetFirstIconColor());
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001EAD RID: 7853 RVA: 0x0006936C File Offset: 0x0006756C
		public Tuple<uint, uint> AlternativeColorPair
		{
			get
			{
				return new Tuple<uint, uint>(this.Banner.GetFirstIconColor(), this.Banner.GetPrimaryColor());
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001EAE RID: 7854 RVA: 0x00069389 File Offset: 0x00067589
		// (set) Token: 0x06001EAF RID: 7855 RVA: 0x00069391 File Offset: 0x00067591
		public Banner Banner { get; private set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x0006939A File Offset: 0x0006759A
		public IEnumerable<BasicCharacterObject> Characters
		{
			get
			{
				return this._characters.AsReadOnly();
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x000693A7 File Offset: 0x000675A7
		public int CountOfCharacters
		{
			get
			{
				return this._characters.Count<BasicCharacterObject>();
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x000693B4 File Offset: 0x000675B4
		// (set) Token: 0x06001EB3 RID: 7859 RVA: 0x000693BC File Offset: 0x000675BC
		public int NumberOfAllMembers { get; private set; }

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x000693C5 File Offset: 0x000675C5
		public int NumberOfHealthyMembers
		{
			get
			{
				return this._characters.Count;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000693D2 File Offset: 0x000675D2
		public BattleEnvironment CurrentBattleEnvironment { get; }

		// Token: 0x06001EB6 RID: 7862 RVA: 0x000693DA File Offset: 0x000675DA
		public CustomBattleCombatant(TextObject name, BasicCultureObject culture, Banner banner, BattleEnvironment battleEnvironment = BattleEnvironment.Land)
		{
			this.Name = name;
			this.BasicCulture = culture;
			this.Banner = banner;
			this.CurrentBattleEnvironment = battleEnvironment;
			this._characters = new List<BasicCharacterObject>();
			this._general = null;
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x00069414 File Offset: 0x00067614
		public void AddCharacter(BasicCharacterObject characterObject, int number)
		{
			for (int i = 0; i < number; i++)
			{
				this._characters.Add(characterObject);
			}
			this.NumberOfAllMembers += number;
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x00069447 File Offset: 0x00067647
		public void SetGeneral(BasicCharacterObject generalCharacter)
		{
			this._general = generalCharacter;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x00069450 File Offset: 0x00067650
		public int GetTacticsSkillAmount()
		{
			if (this._characters.Count > 0)
			{
				return this._characters.Max<BasicCharacterObject>((BasicCharacterObject h) => h.GetSkillValue(DefaultSkills.Tactics));
			}
			return 0;
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0006948C File Offset: 0x0006768C
		public int GetNumberOfMissionReadyTroops()
		{
			return this.NumberOfHealthyMembers;
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x00069494 File Offset: 0x00067694
		public bool IsUnderPlayersCommand(BattleSideEnum playerSide)
		{
			return this.Side == playerSide && this.General.IsPlayerCharacter;
		}

		// Token: 0x04000A74 RID: 2676
		private List<BasicCharacterObject> _characters;

		// Token: 0x04000A75 RID: 2677
		private BasicCharacterObject _general;
	}
}
