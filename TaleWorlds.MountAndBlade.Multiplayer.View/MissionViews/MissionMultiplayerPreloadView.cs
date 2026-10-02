using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x0200000E RID: 14
	public class MissionMultiplayerPreloadView : MissionView
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000021BC File Offset: 0x000003BC
		public override void OnPreMissionTick(float dt)
		{
			if (!this._preloadDone)
			{
				MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
				IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses(MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)));
				IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses2 = MultiplayerClassDivisions.GetMPHeroClasses(MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)));
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				foreach (MultiplayerClassDivisions.MPHeroClass mpheroClass in mpheroClasses)
				{
					list.Add(mpheroClass.HeroCharacter);
					if (missionBehavior.GameType == MultiplayerGameType.Captain)
					{
						list.Add(mpheroClass.TroopCharacter);
					}
				}
				foreach (MultiplayerClassDivisions.MPHeroClass mpheroClass2 in mpheroClasses2)
				{
					list.Add(mpheroClass2.HeroCharacter);
					if (missionBehavior.GameType == MultiplayerGameType.Captain)
					{
						list.Add(mpheroClass2.TroopCharacter);
					}
				}
				this._helperInstance.PreloadCharacters(list);
				MissionMultiplayerSiegeClient missionBehavior2 = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
				if (missionBehavior2 != null)
				{
					this._helperInstance.PreloadItems(missionBehavior2.GetSiegeMissiles());
				}
				this._preloadDone = true;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022FC File Offset: 0x000004FC
		public override void OnSceneRenderingStarted()
		{
			this._helperInstance.WaitForMeshesToBeLoaded();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002309 File Offset: 0x00000509
		public override void OnMissionStateDeactivated()
		{
			base.OnMissionStateDeactivated();
			this._helperInstance.Clear();
		}

		// Token: 0x04000001 RID: 1
		private PreloadHelper _helperInstance = new PreloadHelper();

		// Token: 0x04000002 RID: 2
		private bool _preloadDone;
	}
}
