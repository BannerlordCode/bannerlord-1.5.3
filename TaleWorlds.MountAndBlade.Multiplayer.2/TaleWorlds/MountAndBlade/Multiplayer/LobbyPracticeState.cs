using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000055 RID: 85
	public class LobbyPracticeState : GameState
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000BDFA File Offset: 0x00009FFA
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._practiceOpened)
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000BE16 File Offset: 0x0000A016
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!this._practiceOpened)
			{
				this.OpenPracticeMission();
				this._practiceOpened = true;
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000BE34 File Offset: 0x0000A034
		private void OpenPracticeMission()
		{
			BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("mp_heavy_cavalry_empire_hero");
			BasicCharacterObject object2 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("mp_skirmisher_battania_troop");
			BasicCharacterObject object3 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("mp_light_ranged_khuzait_troop");
			Game.Current.PlayerTroop = @object;
			BasicCultureObject object4;
			BasicCultureObject basicCultureObject = (object4 = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire"));
			Banner banner = basicCultureObject.Banner;
			Banner banner2 = object4.Banner;
			CustomBattleCombatant customBattleCombatant = new CustomBattleCombatant(new TextObject("{=sSJSTe5p}Player Party", null), basicCultureObject, banner, BattleEnvironment.Land);
			CustomBattleCombatant customBattleCombatant2 = new CustomBattleCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), object4, banner2, BattleEnvironment.Land);
			customBattleCombatant.AddCharacter(@object, 1);
			customBattleCombatant2.AddCharacter(@object, 1);
			customBattleCombatant.AddCharacter(object2, 3);
			customBattleCombatant2.AddCharacter(object2, 3);
			customBattleCombatant.AddCharacter(object3, 8);
			customBattleCombatant2.AddCharacter(object3, 8);
			customBattleCombatant.SetGeneral(@object);
			customBattleCombatant2.SetGeneral(@object);
			customBattleCombatant.Side = BattleSideEnum.Attacker;
			customBattleCombatant2.Side = BattleSideEnum.Defender;
			MultiplayerPracticeMissions.OpenMultiplayerPracticeMission("mp_practice_battle", @object, customBattleCombatant, customBattleCombatant2, true, null, "", "summer", 6f);
		}

		// Token: 0x040000E5 RID: 229
		private bool _practiceOpened;
	}
}
