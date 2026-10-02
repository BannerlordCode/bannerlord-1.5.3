using System;
using System.Collections.Generic;
using SandBox.BoardGames.MissionLogics;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Missions
{
	// Token: 0x02000017 RID: 23
	public class MissionCampaignView : MissionView
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00005390 File Offset: 0x00003590
		public override void OnMissionScreenPreLoad()
		{
			this._mapScreen = MapScreen.Instance;
			if (this._mapScreen != null && base.Mission.NeedsMemoryCleanup && ScreenManager.ScreenTypeExistsAtList(this._mapScreen))
			{
				this._mapScreen.ClearGPUMemory();
				Utilities.ClearShaderMemory();
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000053CF File Offset: 0x000035CF
		public override void OnMissionScreenFinalize()
		{
			MapScreen mapScreen = this._mapScreen;
			if (((mapScreen != null) ? mapScreen.BannerTexturedMaterialCache : null) != null)
			{
				this._mapScreen.BannerTexturedMaterialCache.Clear();
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000053F8 File Offset: 0x000035F8
		[CommandLineFunctionality.CommandLineArgumentFunction("get_face_and_helmet_info_of_followed_agent", "mission")]
		public static string GetFaceAndHelmetInfoOfFollowedAgent(List<string> strings)
		{
			MissionScreen missionScreen = ScreenManager.TopScreen as MissionScreen;
			if (missionScreen == null)
			{
				return "Only works at missions";
			}
			Agent lastFollowedAgent = missionScreen.LastFollowedAgent;
			if (lastFollowedAgent == null)
			{
				return "An agent needs to be focussed.";
			}
			string text = "";
			text += lastFollowedAgent.BodyPropertiesValue.ToString();
			EquipmentElement equipmentFromSlot = lastFollowedAgent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.NumAllWeaponSlots);
			if (!equipmentFromSlot.IsEmpty)
			{
				text = text + "\n Armor Name: " + equipmentFromSlot.Item.Name.ToString();
				text = text + "\n Mesh Name: " + equipmentFromSlot.Item.MultiMeshName;
			}
			if (lastFollowedAgent.Character != null)
			{
				CharacterObject characterObject = lastFollowedAgent.Character as CharacterObject;
				if (characterObject != null)
				{
					text = text + "\n Troop Id: " + characterObject.StringId;
				}
			}
			TaleWorlds.InputSystem.Input.SetClipboardText(text);
			return "Copied to clipboard:\n" + text;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000054D4 File Offset: 0x000036D4
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._missionMainAgentController = Mission.Current.GetMissionBehavior<MissionMainAgentController>();
			MissionBoardGameLogic missionBehavior = Mission.Current.GetMissionBehavior<MissionBoardGameLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.GameStarted += this._missionMainAgentController.Disable;
				missionBehavior.GameEnded += this._missionMainAgentController.Enable;
			}
		}

		// Token: 0x04000029 RID: 41
		private MapScreen _mapScreen;

		// Token: 0x0400002A RID: 42
		private MissionMainAgentController _missionMainAgentController;
	}
}
