using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics.Arena;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions.Tournaments
{
	// Token: 0x0200002A RID: 42
	internal class ArenaPreloadView : MissionView
	{
		// Token: 0x06000116 RID: 278 RVA: 0x0000CB38 File Offset: 0x0000AD38
		public override void OnPreMissionTick(float dt)
		{
			if (!this._preloadDone)
			{
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				if (Mission.Current.GetMissionBehavior<ArenaPracticeFightMissionController>() != null)
				{
					foreach (CharacterObject characterObject in ArenaPracticeFightMissionController.GetParticipantCharacters(Settlement.CurrentSettlement))
					{
						list.Add(characterObject);
					}
					list.Add(CharacterObject.PlayerCharacter);
				}
				TournamentBehavior missionBehavior = Mission.Current.GetMissionBehavior<TournamentBehavior>();
				if (missionBehavior != null)
				{
					foreach (CharacterObject characterObject2 in missionBehavior.GetAllPossibleParticipants())
					{
						list.Add(characterObject2);
					}
				}
				this._helperInstance.PreloadCharacters(list);
				this._preloadDone = true;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000CC1C File Offset: 0x0000AE1C
		public override void OnSceneRenderingStarted()
		{
			this._helperInstance.WaitForMeshesToBeLoaded();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000CC29 File Offset: 0x0000AE29
		public override void OnMissionStateDeactivated()
		{
			base.OnMissionStateDeactivated();
			this._helperInstance.Clear();
		}

		// Token: 0x04000081 RID: 129
		private readonly PreloadHelper _helperInstance = new PreloadHelper();

		// Token: 0x04000082 RID: 130
		private bool _preloadDone;
	}
}
