using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000064 RID: 100
	public class HandMorphTest : ScriptComponentBehavior
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0001D80C File Offset: 0x0001BA0C
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0001D814 File Offset: 0x0001BA14
		public uint ClothColor1 { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001D81D File Offset: 0x0001BA1D
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0001D825 File Offset: 0x0001BA25
		public uint ClothColor2 { get; private set; }

		// Token: 0x060003E3 RID: 995 RVA: 0x0001D830 File Offset: 0x0001BA30
		protected override void OnInit()
		{
			base.OnInit();
			this.ClothColor1 = uint.MaxValue;
			this.ClothColor2 = uint.MaxValue;
			if (this._agentVisuals == null && !this._characterSpawned)
			{
				this.SpawnCharacter();
				this._characterSpawned = true;
			}
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			this._agentVisuals.GetVisuals().SetFrame(ref globalFrame);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001D88F File Offset: 0x0001BA8F
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (Game.Current == null)
			{
				this._editorGameManager = new EditorGameManager();
			}
			this.ClothColor1 = uint.MaxValue;
			this.ClothColor2 = uint.MaxValue;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001D8B8 File Offset: 0x0001BAB8
		protected override void OnEditorTick(float dt)
		{
			if (!this._isFinished && this._editorGameManager != null)
			{
				this._isFinished = !this._editorGameManager.DoLoadingForGameManager();
			}
			if (Game.Current != null && this._agentVisuals == null && !this._characterSpawned)
			{
				this.SpawnCharacter();
				this._characterSpawned = true;
			}
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			this._agentVisuals.GetVisuals().SetFrame(ref globalFrame);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001D930 File Offset: 0x0001BB30
		public void SpawnCharacter()
		{
			CharacterCode characterCode = CharacterCode.CreateFrom(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_0"));
			this.InitWithCharacter(characterCode);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001D959 File Offset: 0x0001BB59
		public void Reset()
		{
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals == null)
			{
				return;
			}
			agentVisuals.Reset();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001D96C File Offset: 0x0001BB6C
		public void InitWithCharacter(CharacterCode characterCode)
		{
			this.Reset();
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.rotation.s.z = 0f;
			frame.rotation.f.z = 0f;
			frame.rotation.s.Normalize();
			frame.rotation.f.Normalize();
			frame.rotation.u = Vec3.CrossProduct(frame.rotation.s, frame.rotation.f);
			characterCode.BodyProperties = new BodyProperties(new DynamicBodyProperties(20f, 0f, 0f), characterCode.BodyProperties.StaticProperties);
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(characterCode.Race);
			this._agentVisuals = AgentVisuals.Create(new AgentVisualsData().Equipment(characterCode.CalculateEquipment()).BodyProperties(characterCode.BodyProperties).Race(characterCode.Race)
				.SkeletonType(characterCode.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, characterCode.IsFemale, "_facegen"))
				.ActionCode(in this.act_visual_test_morph_animation)
				.Scene(base.GameEntity.Scene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(true)
				.UseMorphAnims(true)
				.ClothColor1(this.ClothColor1)
				.ClothColor2(this.ClothColor2)
				.Frame(frame), "HandMorphTest", false, false, false);
			this._agentVisuals.SetAction(in this.act_defend_up_fist_active, 1f, true);
			MatrixFrame matrixFrame = frame;
			this._agentVisuals.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(1f, matrixFrame, true);
			this._agentVisuals.GetVisuals().SetFrame(ref matrixFrame);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001DB2E File Offset: 0x0001BD2E
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this._agentVisuals.Reset();
		}

		// Token: 0x04000244 RID: 580
		private const bool CreateFaceImmediately = true;

		// Token: 0x04000245 RID: 581
		private readonly ActionIndexCache act_defend_up_fist_active = ActionIndexCache.Create("act_defend_up_fist_active");

		// Token: 0x04000246 RID: 582
		private readonly ActionIndexCache act_visual_test_morph_animation = ActionIndexCache.Create("act_visual_test_morph_animation");

		// Token: 0x04000249 RID: 585
		private MBGameManager _editorGameManager;

		// Token: 0x0400024A RID: 586
		private bool _isFinished;

		// Token: 0x0400024B RID: 587
		private bool _characterSpawned;

		// Token: 0x0400024C RID: 588
		private AgentVisuals _agentVisuals;
	}
}
