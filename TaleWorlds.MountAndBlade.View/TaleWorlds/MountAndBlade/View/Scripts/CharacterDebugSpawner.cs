using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000062 RID: 98
	public class CharacterDebugSpawner : ScriptComponentBehavior
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001BAAA File Offset: 0x00019CAA
		// (set) Token: 0x060003BE RID: 958 RVA: 0x0001BAB2 File Offset: 0x00019CB2
		public uint ClothColor1 { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0001BABB File Offset: 0x00019CBB
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0001BAC3 File Offset: 0x00019CC3
		public uint ClothColor2 { get; private set; }

		// Token: 0x060003C1 RID: 961 RVA: 0x0001BACC File Offset: 0x00019CCC
		protected override void OnInit()
		{
			base.OnInit();
			this.ClothColor1 = uint.MaxValue;
			this.ClothColor2 = uint.MaxValue;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001BAE2 File Offset: 0x00019CE2
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (CharacterDebugSpawner._editorGameManager == null)
			{
				CharacterDebugSpawner._editorGameManager = new EditorGameManager();
			}
			CharacterDebugSpawner._editorGameManagerRefCount++;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001BB08 File Offset: 0x00019D08
		protected override void OnEditorTick(float dt)
		{
			if (!CharacterDebugSpawner.isFinished && CharacterDebugSpawner.gameTickFrameNo != Utilities.EngineFrameNo)
			{
				CharacterDebugSpawner.gameTickFrameNo = Utilities.EngineFrameNo;
				CharacterDebugSpawner.isFinished = !CharacterDebugSpawner._editorGameManager.DoLoadingForGameManager();
			}
			if (Game.Current != null && this._agentVisuals == null)
			{
				this.MovementDirection.x = MBRandom.RandomFloatNormal;
				this.MovementDirection.y = MBRandom.RandomFloatNormal;
				this.MovementDirection.Normalize();
				this.MovementSpeed = MBRandom.RandomFloat * 9f + 1f;
				this.PhaseDiff = MBRandom.RandomFloat;
				this.MovementDirectionChange = MBRandom.RandomFloatNormal * 3.1415927f;
				this.Time = 0f;
				this.ActionSetTimer = 0f;
				this.ActionChangeInterval = MBRandom.RandomFloat * 0.5f + 0.5f;
				this.SpawnCharacter();
			}
			MatrixFrame globalFrame = this._agentVisuals.GetVisuals().GetGlobalFrame();
			this._agentVisuals.GetVisuals().SetFrame(ref globalFrame);
			Vec3 vec = new Vec3(this.MovementDirection, 0f, -1f);
			vec.RotateAboutZ(this.MovementDirectionChange * dt);
			this.MovementDirection.x = vec.x;
			this.MovementDirection.y = vec.y;
			float num = this.MovementSpeed * (MathF.Sin(this.PhaseDiff + this.Time) * 0.5f) + 2f;
			Vec2 vec2 = this.MovementDirection * num;
			this._agentVisuals.SetAgentLocalSpeed(vec2);
			this.Time += dt;
			if (this.Time - this.ActionSetTimer > this.ActionChangeInterval)
			{
				this.ActionSetTimer = this.Time;
				this._agentVisuals.SetAction(in this._actionIndices[MBRandom.RandomInt(this._actionIndices.Length)], 0f, true);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001BCED File Offset: 0x00019EED
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this.Reset();
			CharacterDebugSpawner._editorGameManagerRefCount--;
			if (CharacterDebugSpawner._editorGameManagerRefCount == 0)
			{
				CharacterDebugSpawner._editorGameManager = null;
				CharacterDebugSpawner.isFinished = false;
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001BD1C File Offset: 0x00019F1C
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "LordName")
			{
				BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
				AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
				copyAgentVisualsData.BodyProperties(@object.GetBodyProperties(null, -1)).SkeletonType(@object.IsFemale ? SkeletonType.Female : SkeletonType.Male).ActionSet(MBGlobals.GetActionSetWithSuffix(copyAgentVisualsData.MonsterData, @object.IsFemale, "_poses"))
					.Equipment(@object.Equipment)
					.UseMorphAnims(true);
				this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
				return;
			}
			if (!(variableName == "PoseAction"))
			{
				if (variableName == "IsWeaponWielded")
				{
					BasicCharacterObject object2 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
					this.WieldWeapon(CharacterCode.CreateFrom(object2));
				}
				return;
			}
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals == null)
			{
				return;
			}
			agentVisuals.SetAction(in this.PoseAction, 0f, true);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001BE13 File Offset: 0x0001A013
		public void SetClothColors(uint color1, uint color2)
		{
			this.ClothColor1 = color1;
			this.ClothColor2 = color2;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001BE24 File Offset: 0x0001A024
		public void SpawnCharacter()
		{
			BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(this.LordName);
			if (@object != null)
			{
				CharacterCode characterCode = CharacterCode.CreateFrom(@object);
				this.InitWithCharacter(characterCode);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001BE58 File Offset: 0x0001A058
		public void Reset()
		{
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals == null)
			{
				return;
			}
			agentVisuals.Reset();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001BE6C File Offset: 0x0001A06C
		public void InitWithCharacter(CharacterCode characterCode)
		{
			GameEntity gameEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(base.GameEntity.Scene, false, true, true);
			gameEntity.Name = "TableauCharacterAgentVisualsEntity";
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(characterCode.Race);
			this._agentVisuals = AgentVisuals.Create(new AgentVisualsData().Equipment(characterCode.CalculateEquipment()).BodyProperties(characterCode.BodyProperties).Race(characterCode.Race)
				.Frame(gameEntity.GetGlobalFrame())
				.SkeletonType(characterCode.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Entity(gameEntity)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, characterCode.IsFemale, "_facegen"))
				.ActionCode(in ActionIndexCache.act_inventory_idle_start)
				.Scene(base.GameEntity.Scene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(this.CreateFaceImmediately)
				.Banner(characterCode.Banner)
				.ClothColor1(this.ClothColor1)
				.ClothColor2(this.ClothColor2), "CharacterDebugSpawner", false, false, false);
			this._agentVisuals.SetAction(in this.PoseAction, MBRandom.RandomFloat, true);
			base.GameEntity.AddChild(gameEntity.WeakEntity, false);
			this.WieldWeapon(characterCode);
			this._agentVisuals.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.1f, this._agentVisuals.GetVisuals().GetGlobalFrame(), true);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001BFC8 File Offset: 0x0001A1C8
		public void WieldWeapon(CharacterCode characterCode)
		{
			if (this.IsWeaponWielded)
			{
				int num = -1;
				int num2 = -1;
				Equipment equipment = characterCode.CalculateEquipment();
				for (int i = 0; i < 4; i++)
				{
					ItemObject item = equipment[i].Item;
					if (((item != null) ? item.PrimaryWeapon : null) != null)
					{
						if (num2 == -1 && equipment[i].Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand))
						{
							num2 = i;
						}
						if (num == -1 && equipment[i].Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask))
						{
							num = i;
						}
					}
				}
				if (num != -1 || num2 != -1)
				{
					AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
					copyAgentVisualsData.RightWieldedItemIndex(num).LeftWieldedItemIndex(num2).ActionCode(in this.PoseAction);
					this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
				}
			}
		}

		// Token: 0x0400021C RID: 540
		private readonly ActionIndexCache[] _actionIndices = new ActionIndexCache[]
		{
			ActionIndexCache.Create("act_start_conversation"),
			ActionIndexCache.Create("act_stand_conversation"),
			ActionIndexCache.Create("act_start_angry_conversation"),
			ActionIndexCache.Create("act_stand_angry_conversation"),
			ActionIndexCache.Create("act_start_sad_conversation"),
			ActionIndexCache.Create("act_stand_sad_conversation"),
			ActionIndexCache.Create("act_start_happy_conversation"),
			ActionIndexCache.Create("act_stand_happy_conversation"),
			ActionIndexCache.Create("act_start_busy_conversation"),
			ActionIndexCache.Create("act_stand_busy_conversation"),
			ActionIndexCache.Create("act_explaining_conversation"),
			ActionIndexCache.Create("act_introduction_conversation"),
			ActionIndexCache.Create("act_wondering_conversation"),
			ActionIndexCache.Create("act_unknown_conversation"),
			ActionIndexCache.Create("act_friendly_conversation"),
			ActionIndexCache.Create("act_offer_conversation"),
			ActionIndexCache.Create("act_negative_conversation"),
			ActionIndexCache.Create("act_affermative_conversation"),
			ActionIndexCache.Create("act_secret_conversation"),
			ActionIndexCache.Create("act_remember_conversation"),
			ActionIndexCache.Create("act_laugh_conversation"),
			ActionIndexCache.Create("act_threat_conversation"),
			ActionIndexCache.Create("act_scared_conversation"),
			ActionIndexCache.Create("act_flirty_conversation"),
			ActionIndexCache.Create("act_thanks_conversation"),
			ActionIndexCache.Create("act_farewell_conversation"),
			ActionIndexCache.Create("act_troop_cavalry_sword"),
			ActionIndexCache.Create("act_inventory_idle_start"),
			ActionIndexCache.Create("act_inventory_idle"),
			ActionIndexCache.Create("act_character_developer_idle"),
			ActionIndexCache.Create("act_inventory_cloth_equip"),
			ActionIndexCache.Create("act_inventory_glove_equip"),
			ActionIndexCache.Create("act_jump"),
			ActionIndexCache.Create("act_jump_loop"),
			ActionIndexCache.Create("act_jump_end"),
			ActionIndexCache.Create("act_jump_end_hard"),
			ActionIndexCache.Create("act_jump_left_stance"),
			ActionIndexCache.Create("act_jump_loop_left_stance"),
			ActionIndexCache.Create("act_jump_end_left_stance"),
			ActionIndexCache.Create("act_jump_end_hard_left_stance"),
			ActionIndexCache.Create("act_jump_forward"),
			ActionIndexCache.Create("act_jump_forward_loop"),
			ActionIndexCache.Create("act_jump_forward_end"),
			ActionIndexCache.Create("act_jump_forward_end_hard"),
			ActionIndexCache.Create("act_jump_forward_left_stance"),
			ActionIndexCache.Create("act_jump_forward_loop_left_stance"),
			ActionIndexCache.Create("act_jump_forward_end_left_stance"),
			ActionIndexCache.Create("act_jump_forward_end_hard_left_stance"),
			ActionIndexCache.Create("act_jump_backward"),
			ActionIndexCache.Create("act_jump_backward_loop"),
			ActionIndexCache.Create("act_jump_backward_end"),
			ActionIndexCache.Create("act_jump_backward_end_hard"),
			ActionIndexCache.Create("act_jump_backward_left_stance"),
			ActionIndexCache.Create("act_jump_backward_loop_left_stance"),
			ActionIndexCache.Create("act_jump_backward_end_left_stance"),
			ActionIndexCache.Create("act_jump_backward_end_hard_left_stance"),
			ActionIndexCache.Create("act_jump_forward_right"),
			ActionIndexCache.Create("act_jump_forward_right_left_stance"),
			ActionIndexCache.Create("act_jump_forward_left"),
			ActionIndexCache.Create("act_jump_forward_left_left_stance"),
			ActionIndexCache.Create("act_jump_right"),
			ActionIndexCache.Create("act_jump_right_loop"),
			ActionIndexCache.Create("act_jump_right_end"),
			ActionIndexCache.Create("act_jump_right_end_hard"),
			ActionIndexCache.Create("act_jump_left"),
			ActionIndexCache.Create("act_jump_left_loop"),
			ActionIndexCache.Create("act_jump_left_end"),
			ActionIndexCache.Create("act_jump_left_end_hard"),
			ActionIndexCache.Create("act_jump_loop_long"),
			ActionIndexCache.Create("act_jump_loop_long_left_stance"),
			ActionIndexCache.Create("act_throne_sit_down_from_front"),
			ActionIndexCache.Create("act_throne_stand_up_to_front"),
			ActionIndexCache.Create("act_throne_sit_idle"),
			ActionIndexCache.Create("act_sit_down_from_front"),
			ActionIndexCache.Create("act_sit_down_from_right"),
			ActionIndexCache.Create("act_sit_down_from_left"),
			ActionIndexCache.Create("act_sit_down_on_floor_1"),
			ActionIndexCache.Create("act_sit_down_on_floor_2"),
			ActionIndexCache.Create("act_sit_down_on_floor_3"),
			ActionIndexCache.Create("act_stand_up_to_front"),
			ActionIndexCache.Create("act_stand_up_to_right"),
			ActionIndexCache.Create("act_stand_up_to_left"),
			ActionIndexCache.Create("act_stand_up_floor_1"),
			ActionIndexCache.Create("act_stand_up_floor_2"),
			ActionIndexCache.Create("act_stand_up_floor_3"),
			ActionIndexCache.Create("act_sit_1"),
			ActionIndexCache.Create("act_sit_2"),
			ActionIndexCache.Create("act_sit_3"),
			ActionIndexCache.Create("act_sit_4"),
			ActionIndexCache.Create("act_sit_5"),
			ActionIndexCache.Create("act_sit_6"),
			ActionIndexCache.Create("act_sit_7"),
			ActionIndexCache.Create("act_sit_8"),
			ActionIndexCache.Create("act_sit_idle_on_floor_1"),
			ActionIndexCache.Create("act_sit_idle_on_floor_2"),
			ActionIndexCache.Create("act_sit_idle_on_floor_3"),
			ActionIndexCache.Create("act_sit_conversation")
		};

		// Token: 0x0400021D RID: 541
		public readonly ActionIndexCache PoseAction = ActionIndexCache.act_walk_idle_unarmed;

		// Token: 0x0400021E RID: 542
		public string LordName = "main_hero";

		// Token: 0x0400021F RID: 543
		public bool IsWeaponWielded;

		// Token: 0x04000222 RID: 546
		private Vec2 MovementDirection;

		// Token: 0x04000223 RID: 547
		private float MovementSpeed;

		// Token: 0x04000224 RID: 548
		private float PhaseDiff;

		// Token: 0x04000225 RID: 549
		private float Time;

		// Token: 0x04000226 RID: 550
		private float ActionSetTimer;

		// Token: 0x04000227 RID: 551
		private float ActionChangeInterval;

		// Token: 0x04000228 RID: 552
		private float MovementDirectionChange;

		// Token: 0x04000229 RID: 553
		private static MBGameManager _editorGameManager = null;

		// Token: 0x0400022A RID: 554
		private static int _editorGameManagerRefCount = 0;

		// Token: 0x0400022B RID: 555
		private static bool isFinished = false;

		// Token: 0x0400022C RID: 556
		private static int gameTickFrameNo = -1;

		// Token: 0x0400022D RID: 557
		private bool CreateFaceImmediately = true;

		// Token: 0x0400022E RID: 558
		private AgentVisuals _agentVisuals;
	}
}
