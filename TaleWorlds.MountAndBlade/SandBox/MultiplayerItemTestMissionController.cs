using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x020000E0 RID: 224
	public class MultiplayerItemTestMissionController : MissionLogic
	{
		// Token: 0x06000925 RID: 2341 RVA: 0x0000F508 File Offset: 0x0000D708
		public MultiplayerItemTestMissionController(BasicCultureObject culture)
		{
			this._culture = culture;
			if (!MultiplayerItemTestMissionController._initializeFlag)
			{
				Game.Current.ObjectManager.LoadXML("MPCharacters", false);
				MultiplayerItemTestMissionController._initializeFlag = true;
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0000F579 File Offset: 0x0000D779
		public override void AfterStart()
		{
			this.GetAllTroops();
			this.SpawnMainAgent();
			this.SpawnMultiplayerTroops();
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0000F58D File Offset: 0x0000D78D
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0000F59C File Offset: 0x0000D79C
		private void SpawnMultiplayerTroops()
		{
			foreach (BasicCharacterObject basicCharacterObject in this._troops)
			{
				Vec3 vec;
				Vec2 vec2;
				this.GetNextSpawnFrame(out vec, out vec2);
				foreach (Equipment equipment in basicCharacterObject.BattleEquipments)
				{
					base.Mission.SpawnAgent(new AgentBuildData(new BasicBattleAgentOrigin(basicCharacterObject)).Equipment(equipment).InitialPosition(in vec).InitialDirection(in vec2), false, null, null);
					vec += new Vec3(0f, 2f, 0f, -1f);
				}
				foreach (Equipment equipment2 in basicCharacterObject.CivilianEquipments)
				{
					base.Mission.SpawnAgent(new AgentBuildData(new BasicBattleAgentOrigin(basicCharacterObject)).Equipment(equipment2).InitialPosition(in vec).InitialDirection(in vec2), false, null, null);
					vec += new Vec3(0f, 2f, 0f, -1f);
				}
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0000F730 File Offset: 0x0000D930
		private void GetNextSpawnFrame(out Vec3 position, out Vec2 direction)
		{
			this._coordinate += new Vec3(3f, 0f, 0f, -1f);
			if (this._coordinate.x > (float)this._mapHorizontalEndCoordinate)
			{
				this._coordinate.x = 3f;
				this._coordinate.y = this._coordinate.y + 3f;
			}
			position = this._coordinate;
			direction = new Vec2(0f, -1f);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
		private XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			string text = new StreamReader(path).ReadToEnd();
			xmlDocument.LoadXml(text);
			return xmlDocument;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0000F804 File Offset: 0x0000DA04
		private void SpawnMainAgent()
		{
			if (this.mainAgent == null || this.mainAgent.State != AgentState.Active)
			{
				BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("main_hero");
				Mission mission = base.Mission;
				AgentBuildData agentBuildData = new AgentBuildData(new BasicBattleAgentOrigin(@object)).Team(base.Mission.DefenderTeam);
				Vec3 vec = new Vec3(200f + (float)MBRandom.RandomInt(15), 200f + (float)MBRandom.RandomInt(15), 1f, -1f);
				this.mainAgent = mission.SpawnAgent(agentBuildData.InitialPosition(in vec).InitialDirection(in Vec2.Forward).Controller(AgentControllerType.Player), false, null, null);
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
		private void GetAllTroops()
		{
			foreach (object obj in this.LoadXmlFile(BasePath.Name + "/Modules/Native/ModuleData/mpcharacters.xml").DocumentElement.SelectNodes("NPCCharacter"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlAttributeCollection attributes = xmlNode.Attributes;
				if (((attributes != null) ? attributes["occupation"] : null) != null && xmlNode.Attributes["occupation"].InnerText == "Soldier")
				{
					string innerText = xmlNode.Attributes["id"].InnerText;
					BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(innerText);
					if (@object != null && @object.Culture == this._culture)
					{
						this._troops.Add(@object);
					}
				}
			}
		}

		// Token: 0x04000219 RID: 537
		private Agent mainAgent;

		// Token: 0x0400021A RID: 538
		private BasicCultureObject _culture;

		// Token: 0x0400021B RID: 539
		private List<BasicCharacterObject> _troops = new List<BasicCharacterObject>();

		// Token: 0x0400021C RID: 540
		private const float HorizontalGap = 3f;

		// Token: 0x0400021D RID: 541
		private const float VerticalGap = 3f;

		// Token: 0x0400021E RID: 542
		private Vec3 _coordinate = new Vec3(200f, 200f, 0f, -1f);

		// Token: 0x0400021F RID: 543
		private int _mapHorizontalEndCoordinate = 800;

		// Token: 0x04000220 RID: 544
		private static bool _initializeFlag;
	}
}
