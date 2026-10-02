using System;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001A RID: 26
	public class HeroViewModel : CharacterViewModel
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000C310 File Offset: 0x0000A510
		public HeroViewModel(CharacterViewModel.StanceTypes stance = CharacterViewModel.StanceTypes.None)
			: base(stance)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000C31C File Offset: 0x0000A51C
		public override void SetEquipment(Equipment equipment)
		{
			this._equipment = ((equipment != null) ? equipment.Clone(false) : null);
			Equipment equipment2 = this._equipment;
			base.HasMount = ((equipment2 != null) ? equipment2[10].Item : null) != null;
			Equipment equipment3 = this._equipment;
			base.EquipmentCode = ((equipment3 != null) ? equipment3.CalculateEquipmentCode() : null);
			if (this._hero != null)
			{
				base.MountCreationKey = TaleWorlds.Core.MountCreationKey.GetRandomMountKeyString(equipment[10].Item, this._hero.CharacterObject.GetMountKeySeed());
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		public void FillFrom(Hero hero, int seed = -1, bool useCivilian = false, bool useCharacteristicIdleAction = false)
		{
			TextObject textObject;
			base.IsHidden = CampaignUIHelper.IsHeroInformationHidden(hero, out textObject);
			if (FaceGen.GetMaturityTypeWithAge(hero.Age) > BodyMeshMaturityType.Child && !base.IsHidden)
			{
				this._hero = hero;
				base.FillFrom(hero.CharacterObject, seed, null);
				base.MountCreationKey = TaleWorlds.Core.MountCreationKey.GetRandomMountKeyString(hero.CharacterObject.Equipment[10].Item, hero.CharacterObject.GetMountKeySeed());
				this.IsDead = hero.IsDead;
				if ((hero.IsNoncombatant && !hero.IsPartyLeader) || useCivilian)
				{
					Equipment civilianEquipment = hero.CivilianEquipment;
					this._equipment = ((civilianEquipment != null) ? civilianEquipment.Clone(false) : null);
				}
				else
				{
					Equipment battleEquipment = hero.BattleEquipment;
					this._equipment = ((battleEquipment != null) ? battleEquipment.Clone(false) : null);
				}
				if (useCharacteristicIdleAction)
				{
					ConversationAnimData conversationAnimData;
					if (Campaign.Current.ConversationManager.ConversationAnimationManager.ConversationAnims.TryGetValue(CharacterHelper.GetNonconversationPose(hero.CharacterObject), out conversationAnimData))
					{
						base.IdleAction = conversationAnimData.IdleAnimLoop;
					}
					base.IdleFaceAnim = CharacterHelper.GetNonconversationFacialIdle(hero.CharacterObject) ?? "";
				}
				Equipment equipment = this._equipment;
				base.EquipmentCode = ((equipment != null) ? equipment.CalculateEquipmentCode() : null);
				Equipment equipment2 = this._equipment;
				base.HasMount = ((equipment2 != null) ? equipment2[10].Item : null) != null;
				if (((hero != null) ? hero.ClanBanner : null) != null)
				{
					base.BannerCodeText = hero.ClanBanner.BannerCode;
				}
				IFaction mapFaction = hero.MapFaction;
				base.ArmorColor1 = ((mapFaction != null) ? mapFaction.Color : 0U);
				IFaction mapFaction2 = hero.MapFaction;
				base.ArmorColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 0U);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000C55E File Offset: 0x0000A75E
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._hero = null;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000C56D File Offset: 0x0000A76D
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000C575 File Offset: 0x0000A775
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (value != this._isDead)
				{
					this._isDead = value;
					base.OnPropertyChangedWithValue(value, "IsDead");
				}
			}
		}

		// Token: 0x040000C1 RID: 193
		private Hero _hero;

		// Token: 0x040000C2 RID: 194
		private bool _isDead;
	}
}
