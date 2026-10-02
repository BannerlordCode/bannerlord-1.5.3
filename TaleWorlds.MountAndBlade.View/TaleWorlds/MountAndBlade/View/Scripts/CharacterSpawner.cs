using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000063 RID: 99
	public class CharacterSpawner : ScriptComponentBehavior
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0001C7C4 File Offset: 0x0001A9C4
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0001C7CC File Offset: 0x0001A9CC
		public uint ClothColor1 { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0001C7D5 File Offset: 0x0001A9D5
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0001C7DD File Offset: 0x0001A9DD
		public uint ClothColor2 { get; private set; }

		// Token: 0x060003D1 RID: 977 RVA: 0x0001C7E6 File Offset: 0x0001A9E6
		protected override void OnInit()
		{
			base.OnInit();
			this.ClothColor1 = uint.MaxValue;
			this.ClothColor2 = uint.MaxValue;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001C7FC File Offset: 0x0001A9FC
		protected void Init()
		{
			this.Active = false;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001C805 File Offset: 0x0001AA05
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (Game.Current == null)
			{
				this._editorGameManager = new EditorGameManager();
				this.isFinished = !this._editorGameManager.DoLoadingForGameManager();
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001C834 File Offset: 0x0001AA34
		protected override void OnEditorTick(float dt)
		{
			if (!this.Enabled)
			{
				return;
			}
			if (!this.isFinished && this._editorGameManager != null)
			{
				this.isFinished = !this._editorGameManager.DoLoadingForGameManager();
			}
			if (Game.Current != null && this._agentVisuals == null)
			{
				this.SpawnCharacter();
				base.GameEntity.SetVisibilityExcludeParents(false);
				if (this._agentEntity != null)
				{
					this._agentEntity.SetVisibilityExcludeParents(false);
				}
				if (this._horseEntity != null)
				{
					this._horseEntity.SetVisibilityExcludeParents(false);
				}
			}
			if (this._agentVisuals != null)
			{
				Skeleton skeleton = this._agentVisuals.GetVisuals().GetSkeleton();
				if (skeleton != null)
				{
					skeleton.Freeze(false);
					skeleton.TickAnimationsAndForceUpdate(0.001f, this._agentVisuals.GetVisuals().GetGlobalFrame(), false);
				}
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001C90B File Offset: 0x0001AB0B
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			if (this._agentVisuals != null)
			{
				this._agentVisuals.Reset();
				this._agentVisuals.GetVisuals().ManualInvalidate();
				this._agentVisuals = null;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001C93E File Offset: 0x0001AB3E
		public void SetCreateFaceImmediately(bool value)
		{
			this.CreateFaceImmediately = value;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001C948 File Offset: 0x0001AB48
		private void Disable()
		{
			if (this._agentEntity != null && this._agentEntity.Parent == base.GameEntity)
			{
				base.GameEntity.RemoveChild(this._agentEntity.WeakEntity, false, false, true, 34);
			}
			if (this._agentVisuals != null)
			{
				this._agentVisuals.Reset();
				this._agentVisuals.GetVisuals().ManualInvalidate();
				this._agentVisuals = null;
			}
			if (this._horseEntity != null && this._horseEntity.Parent == base.GameEntity)
			{
				this._horseEntity.Scene.RemoveEntity(this._horseEntity, 96);
			}
			this.Active = false;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001CA08 File Offset: 0x0001AC08
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "Enabled")
			{
				if (this.Enabled)
				{
					this.Init();
				}
				else
				{
					this.Disable();
				}
			}
			if (!this.Enabled)
			{
				return;
			}
			if (variableName == "LordName" || variableName == "ActionSetSuffix")
			{
				if (this._agentVisuals != null)
				{
					BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
					if (@object != null)
					{
						this.InitWithCharacter(CharacterCode.CreateFrom(@object), true);
						return;
					}
				}
			}
			else if (variableName == "PoseActionForHorse")
			{
				BasicCharacterObject object2 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
				if (object2 != null)
				{
					this.InitWithCharacter(CharacterCode.CreateFrom(object2), true);
					return;
				}
			}
			else if (variableName == "PoseAction")
			{
				if (this._agentVisuals != null)
				{
					BasicCharacterObject object3 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
					if (object3 != null)
					{
						this.InitWithCharacter(CharacterCode.CreateFrom(object3), true);
						return;
					}
				}
			}
			else
			{
				if (variableName == "IsWeaponWielded")
				{
					BasicCharacterObject object4 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
					this.WieldWeapon(CharacterCode.CreateFrom(object4));
					return;
				}
				if (variableName == "AnimationProgress")
				{
					Skeleton skeleton = this._agentVisuals.GetVisuals().GetSkeleton();
					skeleton.Freeze(false);
					skeleton.TickAnimationsAndForceUpdate(0.001f, this._agentVisuals.GetVisuals().GetGlobalFrame(), false);
					skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(this.AnimationProgress, 0f, 1f));
					skeleton.SetUptoDate(false);
					skeleton.Freeze(true);
					return;
				}
				if (variableName == "HorseAnimationProgress")
				{
					if (this._horseEntity != null)
					{
						this._horseEntity.Skeleton.Freeze(false);
						this._horseEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, this._horseEntity.GetGlobalFrame(), false);
						this._horseEntity.Skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(this.HorseAnimationProgress, 0f, 1f));
						this._horseEntity.Skeleton.SetUptoDate(false);
						this._horseEntity.Skeleton.Freeze(true);
						return;
					}
				}
				else if (variableName == "HasMount")
				{
					if (this.HasMount)
					{
						BasicCharacterObject object5 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
						this.SpawnMount(CharacterCode.CreateFrom(object5));
						return;
					}
					if (this._horseEntity != null)
					{
						this._horseEntity.Scene.RemoveEntity(this._horseEntity, 97);
						return;
					}
				}
				else if (variableName == "Active")
				{
					base.GameEntity.SetVisibilityExcludeParents(this.Active);
					if (this._agentEntity != null)
					{
						this._agentEntity.SetVisibilityExcludeParents(this.Active);
					}
					if (this._horseEntity != null)
					{
						this._horseEntity.SetVisibilityExcludeParents(this.Active);
						return;
					}
				}
				else if (variableName == "FaceKeyString")
				{
					BasicCharacterObject object6 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
					if (object6 != null)
					{
						this.InitWithCharacter(CharacterCode.CreateFrom(object6), true);
						return;
					}
				}
				else if (variableName == "WieldOffHand")
				{
					BasicCharacterObject object7 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
					if (object7 != null)
					{
						this.InitWithCharacter(CharacterCode.CreateFrom(object7), true);
					}
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001CD78 File Offset: 0x0001AF78
		public void SetClothColors(uint color1, uint color2)
		{
			this.ClothColor1 = color1;
			this.ClothColor2 = color2;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001CD88 File Offset: 0x0001AF88
		public void SpawnCharacter()
		{
			BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
			if (@object != null)
			{
				CharacterCode characterCode = CharacterCode.CreateFrom(@object);
				this.InitWithCharacter(characterCode, true);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
		public void InitWithCharacter(CharacterCode characterCode, bool useBodyProperties = false)
		{
			base.GameEntity.BreakPrefab();
			if (this._agentEntity != null && this._agentEntity.Parent == base.GameEntity)
			{
				base.GameEntity.RemoveChild(this._agentEntity.WeakEntity, false, false, true, 35);
			}
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals != null)
			{
				agentVisuals.Reset();
			}
			AgentVisuals agentVisuals2 = this._agentVisuals;
			if (agentVisuals2 != null)
			{
				MBAgentVisuals visuals = agentVisuals2.GetVisuals();
				if (visuals != null)
				{
					visuals.ManualInvalidate();
				}
			}
			if (this._horseEntity != null && this._horseEntity.Parent == base.GameEntity)
			{
				this._horseEntity.Scene.RemoveEntity(this._horseEntity, 98);
			}
			this._agentEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(base.GameEntity.Scene, false, true, true);
			this._agentEntity.Name = "TableauCharacterAgentVisualsEntity";
			this._spawnFrame = this._agentEntity.GetFrame();
			BodyProperties bodyProperties = characterCode.BodyProperties;
			if (useBodyProperties)
			{
				BodyProperties.FromString(this.BodyPropertiesString, out bodyProperties);
			}
			if (characterCode.Color1 != 4294967295U)
			{
				this.ClothColor1 = characterCode.Color1;
			}
			if (characterCode.Color2 != 4294967295U)
			{
				this.ClothColor2 = characterCode.Color2;
			}
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(characterCode.Race);
			this._agentVisuals = AgentVisuals.Create(new AgentVisualsData().Equipment(characterCode.CalculateEquipment()).BodyProperties(bodyProperties).Race(characterCode.Race)
				.Frame(this._spawnFrame)
				.Scale(1f)
				.SkeletonType(characterCode.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Entity(this._agentEntity)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, characterCode.IsFemale, this.ActionSetSuffix))
				.ActionCode(in ActionIndexCache.act_inventory_idle_start)
				.Scene(base.GameEntity.Scene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(this.CreateFaceImmediately)
				.Banner(characterCode.Banner)
				.ClothColor1(this.ClothColor1)
				.ClothColor2(this.ClothColor2)
				.UseMorphAnims(true), "TableauCharacterAgentVisuals", false, false, false);
			AgentVisuals agentVisuals3 = this._agentVisuals;
			ActionIndexCache actionIndexCache = ActionIndexCache.Create(this.PoseAction);
			agentVisuals3.SetAction(in actionIndexCache, MBMath.ClampFloat(this.AnimationProgress, 0f, 1f), true);
			base.GameEntity.AddChild(this._agentEntity.WeakEntity, false);
			this.WieldWeapon(characterCode);
			if (characterCode.FormationClass == FormationClass.Ranged)
			{
				Equipment equipment = this._agentVisuals.GetEquipment();
				for (int i = 0; i < 4; i++)
				{
					ItemObject item = equipment[i].Item;
					if (((item != null) ? item.PrimaryWeapon : null) != null)
					{
						string text = "Ranged primary weapon: ";
						ItemObject item2 = equipment[i].Item;
						MBDebug.Print(text + ((item2 != null) ? item2.StringId : null) + "\n", 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
			}
			MatrixFrame identity = MatrixFrame.Identity;
			this._agentVisuals.GetVisuals().SetFrame(ref identity);
			if (this.HasMount)
			{
				this.SpawnMount(characterCode);
			}
			base.GameEntity.SetVisibilityExcludeParents(true);
			this._agentEntity.SetVisibilityExcludeParents(true);
			if (this._horseEntity != null)
			{
				this._horseEntity.SetVisibilityExcludeParents(true);
			}
			this._agentEntity.CheckResources(true, true);
			Skeleton skeleton = this._agentVisuals.GetVisuals().GetSkeleton();
			skeleton.Freeze(false);
			skeleton.TickAnimationsAndForceUpdate(0.001f, this._agentVisuals.GetVisuals().GetGlobalFrame(), false);
			skeleton.SetUptoDate(false);
			skeleton.Freeze(true);
			foreach (WeakGameEntity weakGameEntity in this._agentEntity.WeakEntity.GetChildren())
			{
				weakGameEntity.SetBoundingboxDirty();
			}
			skeleton.Freeze(false);
			skeleton.TickAnimationsAndForceUpdate(0.001f, this._agentVisuals.GetVisuals().GetGlobalFrame(), false);
			skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(this.AnimationProgress, 0f, 1f));
			skeleton.SetUptoDate(false);
			skeleton.Freeze(true);
			this._agentEntity.SetBoundingboxDirty();
			foreach (WeakGameEntity weakGameEntity2 in this._agentEntity.WeakEntity.GetChildren())
			{
				weakGameEntity2.SetBoundingboxDirty();
			}
			skeleton.ManualInvalidate();
			if (this._horseEntity != null)
			{
				this._horseEntity.Skeleton.Freeze(false);
				this._horseEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, this._horseEntity.GetGlobalFrame(), false);
				this._horseEntity.Skeleton.SetUptoDate(false);
				this._horseEntity.Skeleton.Freeze(true);
				this._horseEntity.SetBoundingboxDirty();
				this._horseEntity.SetBoundingboxDirty();
				foreach (WeakGameEntity weakGameEntity3 in this._horseEntity.WeakEntity.GetChildren())
				{
					weakGameEntity3.SetBoundingboxDirty();
				}
			}
			if (this._horseEntity != null)
			{
				this._horseEntity.Skeleton.Freeze(false);
				this._horseEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, this._horseEntity.GetGlobalFrame(), false);
				this._horseEntity.Skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(this.HorseAnimationProgress, 0f, 1f));
				this._horseEntity.Skeleton.SetUptoDate(false);
				this._horseEntity.Skeleton.Freeze(true);
				this._horseEntity.SetBoundingboxDirty();
				foreach (WeakGameEntity weakGameEntity4 in this._horseEntity.WeakEntity.GetChildren())
				{
					weakGameEntity4.SetBoundingboxDirty();
				}
			}
			base.GameEntity.SetBoundingboxDirty();
			if (!base.GameEntity.Scene.IsEditorScene())
			{
				if (this._agentEntity != null)
				{
					this._agentEntity.ManualInvalidate();
				}
				if (this._horseEntity != null)
				{
					this._horseEntity.ManualInvalidate();
				}
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001D478 File Offset: 0x0001B678
		private void WieldWeapon(CharacterCode characterCode)
		{
			if (this.IsWeaponWielded)
			{
				WeaponFlags weaponFlags = (WeaponFlags)0UL;
				switch (characterCode.FormationClass)
				{
				case FormationClass.Infantry:
				case FormationClass.Cavalry:
				case FormationClass.NumberOfDefaultFormations:
				case FormationClass.HeavyInfantry:
				case FormationClass.LightCavalry:
				case FormationClass.NumberOfRegularFormations:
				case FormationClass.Bodyguard:
					weaponFlags = WeaponFlags.MeleeWeapon;
					break;
				case FormationClass.Ranged:
				case FormationClass.HorseArcher:
					weaponFlags = WeaponFlags.RangedWeapon;
					break;
				}
				int num = -1;
				int num2 = -1;
				WeaponComponentData weaponComponentData = null;
				Equipment equipment = characterCode.CalculateEquipment();
				for (int i = 0; i < 4; i++)
				{
					ItemObject item = equipment[i].Item;
					if (((item != null) ? item.PrimaryWeapon : null) != null)
					{
						if (num2 == -1 && equipment[i].Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand) && this.WieldOffHand)
						{
							num2 = i;
						}
						if (num == -1 && equipment[i].Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(weaponFlags))
						{
							num = i;
							weaponComponentData = equipment[i].Item.PrimaryWeapon;
						}
					}
				}
				if (this.WieldOffHand && weaponFlags == WeaponFlags.RangedWeapon && weaponComponentData != null)
				{
					for (int j = 0; j < 4; j++)
					{
						ItemObject item2 = equipment[j].Item;
						WeaponComponentData weaponComponentData2 = ((item2 != null) ? item2.PrimaryWeapon : null);
						if (weaponComponentData2 != null && weaponComponentData2.IsAmmo && weaponComponentData2.WeaponClass == weaponComponentData.AmmoClass)
						{
							num2 = j;
						}
					}
				}
				if (num != -1 || num2 != -1)
				{
					AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
					AgentVisualsData agentVisualsData = copyAgentVisualsData.RightWieldedItemIndex(num).LeftWieldedItemIndex(num2);
					ActionIndexCache actionIndexCache = ActionIndexCache.Create(this.PoseAction);
					agentVisualsData.ActionCode(in actionIndexCache).Frame(this._spawnFrame);
					this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
				}
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001D640 File Offset: 0x0001B840
		private void SpawnMount(CharacterCode characterCode)
		{
			Equipment equipment = characterCode.CalculateEquipment();
			ItemObject item = equipment[EquipmentIndex.ArmorItemEndSlot].Item;
			if (item == null)
			{
				this.HasMount = false;
				return;
			}
			this._horseEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(base.GameEntity.Scene, false, true, true);
			this._horseEntity.Name = "MountEntity";
			Monster monster = item.HorseComponent.Monster;
			MBActionSet actionSet = MBActionSet.GetActionSet(monster.ActionSetCode);
			this._horseEntity.CreateAgentSkeleton(actionSet.GetSkeletonName(), false, actionSet, monster.MonsterUsage, monster);
			this._horseEntity.CopyComponentsToSkeleton();
			Skeleton skeleton = this._horseEntity.Skeleton;
			int num = 0;
			ActionIndexCache actionIndexCache = ActionIndexCache.Create(this.PoseActionForHorse);
			skeleton.SetAgentActionChannel(num, in actionIndexCache, MBMath.ClampFloat(this.HorseAnimationProgress, 0f, 1f), -0.2f, true, 0f);
			base.GameEntity.AddChild(this._horseEntity.WeakEntity, false);
			MountVisualCreator.AddMountMeshToEntity(this._horseEntity, equipment[10].Item, equipment[11].Item, MountCreationKey.GetRandomMountKeyString(equipment[10].Item, MBRandom.RandomInt()), null);
			this._horseEntity.SetVisibilityExcludeParents(true);
			this._horseEntity.Skeleton.TickAnimations(0.01f, this._agentVisuals.GetVisuals().GetGlobalFrame(), true);
		}

		// Token: 0x0400022F RID: 559
		public bool Enabled;

		// Token: 0x04000230 RID: 560
		public string PoseAction = "act_walk_idle_unarmed";

		// Token: 0x04000231 RID: 561
		public string LordName = "main_hero_for_perf";

		// Token: 0x04000232 RID: 562
		public string ActionSetSuffix = "_facegen";

		// Token: 0x04000233 RID: 563
		public string PoseActionForHorse = "horse_stand_3";

		// Token: 0x04000234 RID: 564
		public string BodyPropertiesString = "<BodyProperties version=\"4\" age=\"23.16\" weight=\"0.3333\" build=\"0\" key=\"00000C07000000010011111211151111000701000010000000111011000101000000500202111110000000000000000000000000000000000000000000A00000\" />";

		// Token: 0x04000235 RID: 565
		public bool IsWeaponWielded;

		// Token: 0x04000236 RID: 566
		public bool HasMount;

		// Token: 0x04000237 RID: 567
		public bool WieldOffHand = true;

		// Token: 0x04000238 RID: 568
		public float AnimationProgress;

		// Token: 0x04000239 RID: 569
		public float HorseAnimationProgress;

		// Token: 0x0400023C RID: 572
		private MBGameManager _editorGameManager;

		// Token: 0x0400023D RID: 573
		private bool isFinished;

		// Token: 0x0400023E RID: 574
		private bool CreateFaceImmediately = true;

		// Token: 0x0400023F RID: 575
		private AgentVisuals _agentVisuals;

		// Token: 0x04000240 RID: 576
		private GameEntity _agentEntity;

		// Token: 0x04000241 RID: 577
		private GameEntity _horseEntity;

		// Token: 0x04000242 RID: 578
		public bool Active;

		// Token: 0x04000243 RID: 579
		private MatrixFrame _spawnFrame;
	}
}
