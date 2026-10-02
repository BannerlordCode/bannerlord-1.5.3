using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F4 RID: 244
	public class DefaultAgeModel : AgeModel
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x0006708D File Offset: 0x0006528D
		public override int BecomeInfantAge
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x00067090 File Offset: 0x00065290
		public override int BecomeChildAge
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00067093 File Offset: 0x00065293
		public override int BecomeTeenagerAge
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x00067097 File Offset: 0x00065297
		public override int HeroComesOfAge
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x0006709B File Offset: 0x0006529B
		public override int MiddleAdultHoodAge
		{
			get
			{
				return 35;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x0006709F File Offset: 0x0006529F
		public override int BecomeOldAge
		{
			get
			{
				return 55;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x000670A3 File Offset: 0x000652A3
		public override int MaxAge
		{
			get
			{
				return 128;
			}
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000670AC File Offset: 0x000652AC
		public override void GetAgeLimitForLocation(CharacterObject character, out int minimumAge, out int maximumAge, string additionalTags = "")
		{
			if (character.Occupation == Occupation.TavernWench)
			{
				minimumAge = 20;
				maximumAge = 28;
				return;
			}
			if (character.Occupation == Occupation.Townsfolk)
			{
				if (additionalTags == "TavernVisitor")
				{
					minimumAge = 20;
					maximumAge = 60;
					return;
				}
				if (additionalTags == "TavernDrinker")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "SlowTownsman")
				{
					minimumAge = 50;
					maximumAge = 70;
					return;
				}
				if (additionalTags == "TownsfolkCarryingStuff")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "BroomsWoman")
				{
					minimumAge = 30;
					maximumAge = 45;
					return;
				}
				if (additionalTags == "Dancer")
				{
					minimumAge = 20;
					maximumAge = 28;
					return;
				}
				if (additionalTags == "Beggar")
				{
					minimumAge = 60;
					maximumAge = 90;
					return;
				}
				if (additionalTags == "Child")
				{
					minimumAge = this.BecomeChildAge;
					maximumAge = this.BecomeTeenagerAge;
					return;
				}
				if (additionalTags == "Teenager")
				{
					minimumAge = this.BecomeTeenagerAge;
					maximumAge = this.HeroComesOfAge;
					return;
				}
				if (additionalTags == "Infant")
				{
					minimumAge = this.BecomeInfantAge;
					maximumAge = this.BecomeChildAge;
					return;
				}
				if (additionalTags == "Notary" || additionalTags == "Barber")
				{
					minimumAge = 30;
					maximumAge = 80;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = 70;
				return;
			}
			else if (character.Occupation == Occupation.Villager)
			{
				if (additionalTags == "TownsfolkCarryingStuff")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "Child")
				{
					minimumAge = this.BecomeChildAge;
					maximumAge = this.BecomeTeenagerAge;
					return;
				}
				if (additionalTags == "Teenager")
				{
					minimumAge = this.BecomeTeenagerAge;
					maximumAge = this.HeroComesOfAge;
					return;
				}
				if (additionalTags == "Infant")
				{
					minimumAge = this.BecomeInfantAge;
					maximumAge = this.BecomeChildAge;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = 70;
				return;
			}
			else
			{
				if (character.Occupation == Occupation.TavernGameHost)
				{
					minimumAge = 30;
					maximumAge = 40;
					return;
				}
				if (character.Occupation == Occupation.Musician)
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (character.Occupation == Occupation.ArenaMaster)
				{
					minimumAge = 30;
					maximumAge = 60;
					return;
				}
				if (character.Occupation == Occupation.ShopWorker)
				{
					minimumAge = 18;
					maximumAge = 50;
					return;
				}
				if (character.Occupation == Occupation.Tavernkeeper)
				{
					minimumAge = 40;
					maximumAge = 80;
					return;
				}
				if (character.Occupation == Occupation.RansomBroker)
				{
					minimumAge = 30;
					maximumAge = 60;
					return;
				}
				if (character.Occupation == Occupation.Blacksmith || character.Occupation == Occupation.GoodsTrader || character.Occupation == Occupation.HorseTrader || character.Occupation == Occupation.Armorer || character.Occupation == Occupation.Weaponsmith)
				{
					minimumAge = 30;
					maximumAge = 80;
					return;
				}
				if (additionalTags == "AlleyGangMember")
				{
					minimumAge = 30;
					maximumAge = 40;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = this.MaxAge;
				return;
			}
		}

		// Token: 0x04000764 RID: 1892
		public const string TavernVisitorTag = "TavernVisitor";

		// Token: 0x04000765 RID: 1893
		public const string TavernDrinkerTag = "TavernDrinker";

		// Token: 0x04000766 RID: 1894
		public const string SlowTownsmanTag = "SlowTownsman";

		// Token: 0x04000767 RID: 1895
		public const string TownsfolkCarryingStuffTag = "TownsfolkCarryingStuff";

		// Token: 0x04000768 RID: 1896
		public const string BroomsWomanTag = "BroomsWoman";

		// Token: 0x04000769 RID: 1897
		public const string DancerTag = "Dancer";

		// Token: 0x0400076A RID: 1898
		public const string BeggarTag = "Beggar";

		// Token: 0x0400076B RID: 1899
		public const string ChildTag = "Child";

		// Token: 0x0400076C RID: 1900
		public const string TeenagerTag = "Teenager";

		// Token: 0x0400076D RID: 1901
		public const string InfantTag = "Infant";

		// Token: 0x0400076E RID: 1902
		public const string NotaryTag = "Notary";

		// Token: 0x0400076F RID: 1903
		public const string BarberTag = "Barber";

		// Token: 0x04000770 RID: 1904
		public const string AlleyGangMemberTag = "AlleyGangMember";
	}
}
