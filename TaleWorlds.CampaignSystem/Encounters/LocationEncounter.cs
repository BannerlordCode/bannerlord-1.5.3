using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000300 RID: 768
	public class LocationEncounter
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000B18EE File Offset: 0x000AFAEE
		public Settlement Settlement { get; }

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000B18F6 File Offset: 0x000AFAF6
		// (set) Token: 0x06002AD9 RID: 10969 RVA: 0x000B18FE File Offset: 0x000AFAFE
		public List<AccompanyingCharacter> CharactersAccompanyingPlayer { get; private set; }

		// Token: 0x06002ADA RID: 10970 RVA: 0x000B1907 File Offset: 0x000AFB07
		protected LocationEncounter(Settlement settlement)
		{
			this.Settlement = settlement;
			this.CharactersAccompanyingPlayer = new List<AccompanyingCharacter>();
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000B1924 File Offset: 0x000AFB24
		public void AddAccompanyingCharacter(LocationCharacter locationCharacter, bool isFollowing = false)
		{
			if (!this.CharactersAccompanyingPlayer.Any<AccompanyingCharacter>((AccompanyingCharacter x) => x.LocationCharacter.Character == locationCharacter.Character))
			{
				AccompanyingCharacter accompanyingCharacter = new AccompanyingCharacter(locationCharacter, isFollowing);
				this.CharactersAccompanyingPlayer.Add(accompanyingCharacter);
			}
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000B1970 File Offset: 0x000AFB70
		public AccompanyingCharacter GetAccompanyingCharacter(LocationCharacter locationCharacter)
		{
			return this.CharactersAccompanyingPlayer.Find((AccompanyingCharacter x) => x.LocationCharacter == locationCharacter);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000B19A4 File Offset: 0x000AFBA4
		public AccompanyingCharacter GetAccompanyingCharacter(CharacterObject character)
		{
			return this.CharactersAccompanyingPlayer.Find(delegate(AccompanyingCharacter x)
			{
				LocationCharacter locationCharacter = x.LocationCharacter;
				return ((locationCharacter != null) ? locationCharacter.Character : null) == character;
			});
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000B19D8 File Offset: 0x000AFBD8
		public void RemoveAccompanyingCharacter(LocationCharacter locationCharacter)
		{
			if (this.CharactersAccompanyingPlayer.Any<AccompanyingCharacter>((AccompanyingCharacter x) => x.LocationCharacter == locationCharacter))
			{
				AccompanyingCharacter accompanyingCharacter = this.CharactersAccompanyingPlayer.Find((AccompanyingCharacter x) => x.LocationCharacter == locationCharacter);
				this.CharactersAccompanyingPlayer.Remove(accompanyingCharacter);
			}
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000B1A30 File Offset: 0x000AFC30
		public void RemoveAccompanyingCharacter(Hero hero)
		{
			for (int i = this.CharactersAccompanyingPlayer.Count - 1; i >= 0; i--)
			{
				if (this.CharactersAccompanyingPlayer[i].LocationCharacter.Character.IsHero && this.CharactersAccompanyingPlayer[i].LocationCharacter.Character.HeroObject == hero)
				{
					this.CharactersAccompanyingPlayer.Remove(this.CharactersAccompanyingPlayer[i]);
					return;
				}
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000B1AA9 File Offset: 0x000AFCA9
		public void RemoveAllAccompanyingCharacters()
		{
			this.CharactersAccompanyingPlayer.Clear();
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000B1AB6 File Offset: 0x000AFCB6
		public void OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation)
		{
			if ((fromLocation == CampaignMission.Current.Location && toLocation == null) || (fromLocation == null && toLocation == CampaignMission.Current.Location))
			{
				CampaignMission.Current.OnCharacterLocationChanged(locationCharacter, fromLocation, toLocation);
			}
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000B1AE5 File Offset: 0x000AFCE5
		public virtual bool IsWorkshopLocation(Location location)
		{
			return false;
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000B1AE8 File Offset: 0x000AFCE8
		public virtual bool IsTavern(Location location)
		{
			return false;
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000B1AEB File Offset: 0x000AFCEB
		public virtual IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			return null;
		}

		// Token: 0x04000C4D RID: 3149
		public bool IsInsideOfASettlement;
	}
}
