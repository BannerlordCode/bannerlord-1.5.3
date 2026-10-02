using System;
using System.IO;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006F RID: 111
	public class ItemCatalogController : MissionLogic
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001AC2E File Offset: 0x00018E2E
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0001AC36 File Offset: 0x00018E36
		public MBReadOnlyList<ItemObject> AllItems { get; private set; }

		// Token: 0x06000478 RID: 1144 RVA: 0x0001AC40 File Offset: 0x00018E40
		public ItemCatalogController()
		{
			this._campaign = Campaign.Current;
			this._game = Game.Current;
			this.timer = new Timer(base.Mission.CurrentTime, 1f, true);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001AC8C File Offset: 0x00018E8C
		public override void AfterStart()
		{
			base.AfterStart();
			base.Mission.SetMissionMode(MissionMode.Battle, true);
			this.AllItems = Game.Current.ObjectManager.GetObjectTypeList<ItemObject>();
			if (!this._campaign.IsSinglePlayerReferencesInitialized)
			{
				this._campaign.InitializeSinglePlayerReferences();
			}
			CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
			MobileParty.MainParty.MemberRoster.AddToCounts(playerCharacter, 1, false, 0, 0, true, -1);
			if (!base.Mission.Teams.IsEmpty<Team>())
			{
				throw new MBIllegalValueException("Number of teams is not 0.");
			}
			base.Mission.Teams.Add(BattleSideEnum.Defender, 4284776512U, uint.MaxValue, null, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, 4281877080U, uint.MaxValue, null, true, false, true);
			base.Mission.PlayerTeam = base.Mission.AttackerTeam;
			EquipmentElement equipmentElement = playerCharacter.Equipment[0];
			EquipmentElement equipmentElement2 = playerCharacter.Equipment[1];
			EquipmentElement equipmentElement3 = playerCharacter.Equipment[2];
			EquipmentElement equipmentElement4 = playerCharacter.Equipment[3];
			EquipmentElement equipmentElement5 = playerCharacter.Equipment[4];
			playerCharacter.Equipment[0] = equipmentElement;
			playerCharacter.Equipment[1] = equipmentElement2;
			playerCharacter.Equipment[2] = equipmentElement3;
			playerCharacter.Equipment[3] = equipmentElement4;
			playerCharacter.Equipment[4] = equipmentElement5;
			ItemObject itemObject = this.AllItems[0];
			Equipment equipment = new Equipment();
			equipment.AddEquipmentToSlotWithoutAgent(this.GetEquipmentIndexOfItem(itemObject), new EquipmentElement(this.AllItems[0], null, null, false));
			AgentBuildData agentBuildData = new AgentBuildData(playerCharacter);
			agentBuildData.Equipment(equipment);
			Mission mission = base.Mission;
			AgentBuildData agentBuildData2 = agentBuildData.Team(base.Mission.AttackerTeam);
			Vec3 vec = new Vec3(15f, 12f, 1f, -1f);
			this._playerAgent = mission.SpawnAgent(agentBuildData2.InitialPosition(in vec).InitialDirection(in Vec2.Forward).Controller(AgentControllerType.Player), false, null, null);
			this._playerAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			this._playerAgent.Health = 10000f;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001AEA8 File Offset: 0x000190A8
		private EquipmentIndex GetEquipmentIndexOfItem(ItemObject item)
		{
			if (item.ItemFlags.HasAnyFlag(ItemFlags.DropOnWeaponChange | ItemFlags.DropOnAnyAction))
			{
				return EquipmentIndex.ExtraWeaponSlot;
			}
			switch (item.ItemType)
			{
			case ItemObject.ItemTypeEnum.Horse:
				return EquipmentIndex.ArmorItemEndSlot;
			case ItemObject.ItemTypeEnum.OneHandedWeapon:
			case ItemObject.ItemTypeEnum.TwoHandedWeapon:
			case ItemObject.ItemTypeEnum.Polearm:
			case ItemObject.ItemTypeEnum.Arrows:
			case ItemObject.ItemTypeEnum.Bolts:
			case ItemObject.ItemTypeEnum.SlingStones:
			case ItemObject.ItemTypeEnum.Shield:
			case ItemObject.ItemTypeEnum.Bow:
			case ItemObject.ItemTypeEnum.Crossbow:
			case ItemObject.ItemTypeEnum.Sling:
			case ItemObject.ItemTypeEnum.Thrown:
			case ItemObject.ItemTypeEnum.Pistol:
			case ItemObject.ItemTypeEnum.Musket:
			case ItemObject.ItemTypeEnum.Bullets:
				return EquipmentIndex.WeaponItemBeginSlot;
			case ItemObject.ItemTypeEnum.HeadArmor:
				return EquipmentIndex.NumAllWeaponSlots;
			case ItemObject.ItemTypeEnum.BodyArmor:
				return EquipmentIndex.Body;
			case ItemObject.ItemTypeEnum.LegArmor:
				return EquipmentIndex.Leg;
			case ItemObject.ItemTypeEnum.HandArmor:
				return EquipmentIndex.Gloves;
			case ItemObject.ItemTypeEnum.Animal:
				return EquipmentIndex.ArmorItemEndSlot;
			case ItemObject.ItemTypeEnum.Cape:
				return EquipmentIndex.Cape;
			case ItemObject.ItemTypeEnum.HorseHarness:
				return EquipmentIndex.HorseHarness;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\ItemCatalogController.cs", "GetEquipmentIndexOfItem", 138);
			return EquipmentIndex.None;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001AF70 File Offset: 0x00019170
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this.timer.Check(base.Mission.CurrentTime))
			{
				if (!Directory.Exists("ItemCatalog"))
				{
					Directory.CreateDirectory("ItemCatalog");
				}
				ItemCatalogController.BeforeCatalogTickDelegate beforeCatalogTick = this.BeforeCatalogTick;
				if (beforeCatalogTick != null)
				{
					beforeCatalogTick(this.curItemIndex);
				}
				this.timer.Reset(base.Mission.CurrentTime);
				MatrixFrame matrixFrame = default(MatrixFrame);
				matrixFrame.origin = new Vec3(10000f, 10000f, 10000f, -1f);
				matrixFrame.rotation = Mat3.Identity;
				this._playerAgent.AgentVisuals.SetFrame(ref matrixFrame);
				this._playerAgent.TeleportToPosition(matrixFrame.origin);
				Blow blow = new Blow(this._playerAgent.Index);
				blow.DamageType = DamageTypes.Blunt;
				blow.BaseMagnitude = 1E+09f;
				blow.GlobalPosition = this._playerAgent.Position;
				this._playerAgent.Die(blow, Agent.KillInfo.Backstabbed);
				this._playerAgent = null;
				for (int i = base.Mission.Agents.Count - 1; i >= 0; i--)
				{
					Agent agent = base.Mission.Agents[i];
					Blow blow2 = new Blow(agent.Index)
					{
						DamageType = DamageTypes.Blunt,
						BaseMagnitude = 1E+09f,
						GlobalPosition = agent.Position
					};
					agent.TeleportToPosition(matrixFrame.origin);
					agent.Die(blow2, Agent.KillInfo.Backstabbed);
				}
				ItemObject itemObject = this.AllItems[this.curItemIndex];
				Equipment equipment = new Equipment();
				equipment.AddEquipmentToSlotWithoutAgent(this.GetEquipmentIndexOfItem(itemObject), new EquipmentElement(itemObject, null, null, false));
				AgentBuildData agentBuildData = new AgentBuildData(this._game.PlayerTroop);
				agentBuildData.Equipment(equipment);
				Mission mission = base.Mission;
				AgentBuildData agentBuildData2 = agentBuildData.Team(base.Mission.AttackerTeam);
				Vec3 vec = new Vec3(15f, 12f, 1f, -1f);
				this._playerAgent = mission.SpawnAgent(agentBuildData2.InitialPosition(in vec).InitialDirection(in Vec2.Forward).Controller(AgentControllerType.Player), false, null, null);
				this._playerAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				this._playerAgent.Health = 10000f;
				Action afterCatalogTick = this.AfterCatalogTick;
				if (afterCatalogTick != null)
				{
					afterCatalogTick();
				}
				this.curItemIndex++;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600047C RID: 1148 RVA: 0x0001B1E8 File Offset: 0x000193E8
		// (remove) Token: 0x0600047D RID: 1149 RVA: 0x0001B220 File Offset: 0x00019420
		public event ItemCatalogController.BeforeCatalogTickDelegate BeforeCatalogTick;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600047E RID: 1150 RVA: 0x0001B258 File Offset: 0x00019458
		// (remove) Token: 0x0600047F RID: 1151 RVA: 0x0001B290 File Offset: 0x00019490
		public event Action AfterCatalogTick;

		// Token: 0x04000260 RID: 608
		private Campaign _campaign;

		// Token: 0x04000261 RID: 609
		private Game _game;

		// Token: 0x04000262 RID: 610
		private Agent _playerAgent;

		// Token: 0x04000264 RID: 612
		private int curItemIndex = 1;

		// Token: 0x04000265 RID: 613
		private Timer timer;

		// Token: 0x02000171 RID: 369
		// (Invoke) Token: 0x06000EA0 RID: 3744
		public delegate void BeforeCatalogTickDelegate(int currentItemIndex);
	}
}
