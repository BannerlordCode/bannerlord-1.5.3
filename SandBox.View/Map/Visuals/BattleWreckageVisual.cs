using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ObjectSystem;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000062 RID: 98
	public class BattleWreckageVisual : MapEntityVisual<BattleWreckage>
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		private Scene MapScene
		{
			get
			{
				if (this._mapScene == null && Campaign.Current != null && Campaign.Current.MapSceneWrapper != null)
				{
					this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
				}
				return this._mapScene;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0001E206 File Offset: 0x0001C406
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return base.MapEntity.Position;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0001E213 File Offset: 0x0001C413
		public override MapEntityVisual AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0001E216 File Offset: 0x0001C416
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0001E21E File Offset: 0x0001C41E
		public GameEntity Entity { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0001E227 File Offset: 0x0001C427
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0001E22F File Offset: 0x0001C42F
		public bool IsFading { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0001E238 File Offset: 0x0001C438
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0001E240 File Offset: 0x0001C440
		public float WreckageTypeCoefficient { get; private set; }

		// Token: 0x060003D4 RID: 980 RVA: 0x0001E24C File Offset: 0x0001C44C
		public BattleWreckageVisual(BattleWreckage entity)
			: base(entity)
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001E2B0 File Offset: 0x0001C4B0
		public override Vec3 GetVisualPosition()
		{
			return base.MapEntity.Position.AsVec3();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001E2D0 File Offset: 0x0001C4D0
		public override bool IsVisibleOrFadingOut()
		{
			return this._entityAlpha > 0f;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001E2DF File Offset: 0x0001C4DF
		public override void OnHover()
		{
			InformationManager.ShowTooltip(typeof(BattleWreckage), new object[] { base.MapEntity });
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001E2FF File Offset: 0x0001C4FF
		public override bool OnMapClick(bool followModifierUsed)
		{
			MobileParty.MainParty.SetMoveGoToInteractablePoint(base.MapEntity, MobileParty.NavigationType.Default);
			return true;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001E313 File Offset: 0x0001C513
		public override void OnOpenEncyclopedia()
		{
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001E318 File Offset: 0x0001C518
		public void OnStartup()
		{
			switch (base.MapEntity.WreckageTypeCategory)
			{
			case BattleWreckage.WreckageType.Small:
				this.WreckageTypeCoefficient = 1.25f;
				break;
			case BattleWreckage.WreckageType.Normal:
				this.WreckageTypeCoefficient = 1.5f;
				break;
			case BattleWreckage.WreckageType.Epic:
				this.WreckageTypeCoefficient = 2f;
				break;
			}
			this._isLandBattleWreckage = base.MapEntity.Position.IsOnLand;
			this.Entity = this.GetNewGameEntity();
			this.SetInitialPosition();
			this.Entity.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
			this._entityAlpha = 0f;
			if (base.MapEntity.IsVisible)
			{
				this._entityAlpha = 1f;
			}
			MapScreen.VisualsOfEntities.Add(this.Entity.Pointer, this);
			string text = (this._isLandBattleWreckage ? "event:/map/ambient/node/wreckage/wreckage_land" : "event:/map/ambient/node/wreckage/wreckage_sea");
			this._ambientSound = SoundEvent.CreateEventFromString(text, this.MapScene);
			this._ambientSound.PlayInPosition(base.MapEntity.Position.AsVec3());
			if (!base.MapEntity.IsVisible)
			{
				this._ambientSound.Pause();
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001E444 File Offset: 0x0001C644
		public void OnRemoved()
		{
			MapScreen.VisualsOfEntities.Remove(this.Entity.Pointer);
			if (this._agentVisualList.Count != 0)
			{
				foreach (AgentVisuals agentVisuals in this._agentVisualList)
				{
					agentVisuals.Reset();
				}
				this._agentVisualList.Clear();
			}
			this._bannerClothSimulator = null;
			if (this._wreckageEntity != null)
			{
				this._wreckageEntity.ClearComponents();
			}
			GameEntity entity = this.Entity;
			if (entity != null)
			{
				entity.RemoveAllChildren();
			}
			GameEntity entity2 = this.Entity;
			if (entity2 != null)
			{
				entity2.Remove(111);
			}
			this.Entity = null;
			SoundEvent ambientSound = this._ambientSound;
			if (ambientSound != null)
			{
				ambientSound.Release();
			}
			this._ambientSound = null;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001E528 File Offset: 0x0001C728
		internal bool HasVisibilityChanged()
		{
			if (this._lastKnownVisibility != base.MapEntity.IsVisible)
			{
				this._lastKnownVisibility = base.MapEntity.IsVisible;
				this.IsFading = true;
				return true;
			}
			return false;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001E558 File Offset: 0x0001C758
		internal void OnVisibilityChanged()
		{
			SoundEvent ambientSound = this._ambientSound;
			if (ambientSound != null && ambientSound.IsValid)
			{
				if (base.MapEntity.IsVisible)
				{
					this._ambientSound.Resume();
					return;
				}
				this._ambientSound.Pause();
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001E594 File Offset: 0x0001C794
		internal void TickFadingState(float realDt)
		{
			if (base.MapEntity.IsVisible)
			{
				this._entityAlpha = MathF.Min(this._entityAlpha + realDt * 1.5f, 1f);
				this.Entity.SetAlpha(this._entityAlpha);
				if (!this._isInvestigated)
				{
					foreach (AgentVisuals agentVisuals in this._agentVisualList)
					{
						WeakGameEntity weakEntity = agentVisuals.GetWeakEntity();
						if (weakEntity != WeakGameEntity.Invalid)
						{
							weakEntity.SetAlpha(this._entityAlpha);
						}
					}
				}
				if (this._entityAlpha < 1f)
				{
					return;
				}
				this.Entity.EntityFlags &= ~EntityFlags.DoNotTick;
				this.Entity.SetVisibilityExcludeParents(true);
				this.IsFading = false;
				if (this._isInvestigated)
				{
					return;
				}
				using (List<AgentVisuals>.Enumerator enumerator = this._agentVisualList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AgentVisuals agentVisuals2 = enumerator.Current;
						WeakGameEntity weakEntity2 = agentVisuals2.GetWeakEntity();
						if (weakEntity2 != WeakGameEntity.Invalid)
						{
							weakEntity2.SetVisibilityExcludeParents(true);
						}
					}
					return;
				}
			}
			this._entityAlpha = MathF.Max(this._entityAlpha - realDt * 1.5f, 0f);
			this.Entity.SetAlpha(this._entityAlpha);
			if (!this._isInvestigated)
			{
				foreach (AgentVisuals agentVisuals3 in this._agentVisualList)
				{
					WeakGameEntity weakEntity3 = agentVisuals3.GetWeakEntity();
					if (weakEntity3 != WeakGameEntity.Invalid)
					{
						weakEntity3.SetAlpha(this._entityAlpha);
					}
				}
			}
			if (this._entityAlpha <= 0f)
			{
				this.Entity.SetVisibilityExcludeParents(false);
				this.Entity.EntityFlags |= EntityFlags.DoNotTick;
				this.IsFading = false;
				if (!this._isInvestigated)
				{
					foreach (AgentVisuals agentVisuals4 in this._agentVisualList)
					{
						WeakGameEntity weakEntity4 = agentVisuals4.GetWeakEntity();
						if (weakEntity4 != WeakGameEntity.Invalid)
						{
							weakEntity4.SetVisibilityExcludeParents(false);
						}
					}
				}
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001E80C File Offset: 0x0001CA0C
		internal void Tick(float dt, float realDt)
		{
			if (this.IsVisibleOrFadingOut())
			{
				if (this._wreckageEntity == null)
				{
					this.AddWreckageVisual();
				}
				ClothSimulatorComponent bannerClothSimulator = this._bannerClothSimulator;
				if (bannerClothSimulator != null)
				{
					bannerClothSimulator.SetForcedWind(Vec3.Side, false);
				}
				this.RefreshWreckageVisual();
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001E847 File Offset: 0x0001CA47
		private GameEntity GetNewGameEntity()
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(this.MapScene, true, true, true);
			gameEntity.AddSphereAsBody(new Vec3(0f, 0f, 0f, -1f), 1f * this.WreckageTypeCoefficient, BodyFlags.Moveable | BodyFlags.OnlyCollideWithRaycast);
			return gameEntity;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001E888 File Offset: 0x0001CA88
		private void AddWreckageVisual()
		{
			string text = (this._isLandBattleWreckage ? "wreckage_prefab" : "naval_wreckage_prefab");
			this._wreckageEntity = GameEntity.Instantiate(this.MapScene, text, true, true, "");
			this.Entity.AddChild(this._wreckageEntity, false);
			this._isInvestigated = base.MapEntity.IsInvestigated;
			if (this._isInvestigated)
			{
				using (IEnumerator<GameEntity> enumerator = this._wreckageEntity.GetChildren().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameEntity gameEntity = enumerator.Current;
						if (!gameEntity.HasTag("battle_remains"))
						{
							gameEntity.SetVisibilityExcludeParents(false);
						}
					}
					return;
				}
			}
			GameEntity firstChildEntityWithTag = this._wreckageEntity.GetFirstChildEntityWithTag("battle_remains");
			if (firstChildEntityWithTag != null)
			{
				firstChildEntityWithTag.SetVisibilityExcludeParents(false);
			}
			if (this._isLandBattleWreckage)
			{
				this.AddFlagVisual();
			}
			this.AddAgentVisuals();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001E970 File Offset: 0x0001CB70
		private void AddFlagVisual()
		{
			GameEntity firstChildEntityWithTagRecursive = this._wreckageEntity.GetFirstChildEntityWithTagRecursive("flag");
			MetaMesh banner = SandBoxViewHelpers.BannerVisualHelper.GetBanner(new Banner(Banner.CreateOneColoredEmptyBanner(99).BannerCode), "vlandia_tier_1_banner");
			int componentCount = firstChildEntityWithTagRecursive.GetComponentCount(GameEntity.ComponentType.ClothSimulator);
			firstChildEntityWithTagRecursive.AddMultiMesh(banner, true);
			if (firstChildEntityWithTagRecursive.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > componentCount)
			{
				this._bannerClothSimulator = (ClothSimulatorComponent)firstChildEntityWithTagRecursive.GetComponentAtIndex(componentCount, GameEntity.ComponentType.ClothSimulator);
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001E9D8 File Offset: 0x0001CBD8
		private void RefreshWreckageVisual()
		{
			if (base.MapEntity.IsInvestigated && !this._isInvestigated && this._wreckageEntity != null)
			{
				foreach (GameEntity gameEntity in this._wreckageEntity.GetChildren())
				{
					if (gameEntity.HasTag("battle_remains"))
					{
						gameEntity.SetVisibilityExcludeParents(true);
					}
					else
					{
						gameEntity.SetVisibilityExcludeParents(false);
					}
				}
				foreach (AgentVisuals agentVisuals in this._agentVisualList)
				{
					agentVisuals.SetVisible(false);
				}
				this._isInvestigated = base.MapEntity.IsInvestigated;
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
		private void AddAgentVisuals()
		{
			CampaignVec2 campaignVec = new CampaignVec2(Vec2.Zero, this._isLandBattleWreckage);
			MBList<TroopRosterElement> totalDiedInBattle = base.MapEntity.GetTotalDiedInBattle();
			totalDiedInBattle.AddRange(base.MapEntity.GetTotalWoundedInBattle());
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this._wreckageEntity.WeakEntity.GetChildrenRecursive(ref list);
			foreach (WeakGameEntity weakGameEntity in list)
			{
				bool flag = true;
				bool flag2 = false;
				MatrixFrame frame = weakGameEntity.GetFrame();
				BattleWreckage.WreckageType wreckageTypeCategory = base.MapEntity.WreckageTypeCategory;
				if (wreckageTypeCategory != BattleWreckage.WreckageType.Small)
				{
					if (wreckageTypeCategory == BattleWreckage.WreckageType.Normal)
					{
						if (weakGameEntity.HasTag("epic"))
						{
							weakGameEntity.Remove(111);
							flag = false;
						}
					}
				}
				else if (weakGameEntity.HasTag("normal") || weakGameEntity.HasTag("epic"))
				{
					weakGameEntity.Remove(111);
					flag = false;
				}
				if (flag)
				{
					if (weakGameEntity.HasTag("spawn_point"))
					{
						flag2 = true;
					}
					bool flag3;
					MatrixFrame spawnFrame = this.GetSpawnFrame(frame, campaignVec, weakGameEntity.HasTag("horse"), out flag3);
					if (flag3)
					{
						if (flag2)
						{
							AgentVisuals agentVisuals;
							if (this._isLandBattleWreckage)
							{
								if (weakGameEntity.HasTag("horse"))
								{
									agentVisuals = this.CreateMountAgentVisual(spawnFrame);
								}
								else
								{
									MatrixFrame matrixFrame = spawnFrame;
									CharacterObject characterObject;
									if (!totalDiedInBattle.IsEmpty<TroopRosterElement>())
									{
										characterObject = totalDiedInBattle.GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement r) => !r.Character.IsHero).Character;
									}
									else
									{
										characterObject = CharacterObject.FindFirst((CharacterObject x) => !x.IsHero);
									}
									agentVisuals = this.CreateHumanAgentVisual(matrixFrame, characterObject);
								}
							}
							else
							{
								spawnFrame.origin = new Vec3(0f, 0f, 0f, 1f);
								MatrixFrame matrixFrame2 = spawnFrame;
								CharacterObject characterObject2;
								if (!totalDiedInBattle.IsEmpty<TroopRosterElement>())
								{
									characterObject2 = totalDiedInBattle.GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement r) => !r.Character.IsHero).Character;
								}
								else
								{
									characterObject2 = CharacterObject.FindFirst((CharacterObject x) => !x.IsHero);
								}
								agentVisuals = this.CreateHumanAgentVisual(matrixFrame2, characterObject2);
								weakGameEntity.AddChild(agentVisuals.GetWeakEntity(), false);
							}
							agentVisuals.GetWeakEntity().SetVisibilityExcludeParents(base.MapEntity.IsVisible);
							this._agentVisualList.Add(agentVisuals);
						}
					}
					else if (!flag2)
					{
						weakGameEntity.SetVisibilityExcludeParents(false);
					}
				}
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001ED68 File Offset: 0x0001CF68
		private MatrixFrame GetSpawnFrame(MatrixFrame frame, CampaignVec2 campaignPosition, bool isHorseEntity, out bool isValid)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.rotation = frame.rotation;
			identity.origin = frame.origin + this.Entity.GlobalPosition;
			campaignPosition.AddVec2(identity.origin.AsVec2);
			identity.origin.z = campaignPosition.AsVec3().z;
			if (isHorseEntity)
			{
				identity.origin -= identity.rotation.s / 2f;
			}
			isValid = campaignPosition.IsValid();
			campaignPosition.AddVec2(-identity.origin.AsVec2);
			return identity;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001EE24 File Offset: 0x0001D024
		private AgentVisuals CreateHumanAgentVisual(MatrixFrame frame, CharacterObject character)
		{
			Equipment equipment = character.Equipment.Clone(false);
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(character.Race);
			MBActionSet actionSet = MBGlobals.GetActionSet("as_human_warrior");
			float num = (this._isLandBattleWreckage ? 0.3f : 0.15f);
			frame.Rotate(MBRandom.RandomFloatRanged(3.1415927f), in Vec3.Up);
			AgentVisuals agentVisuals = AgentVisuals.Create(new AgentVisualsData().UseMorphAnims(true).Equipment(equipment).BodyProperties(character.GetBodyProperties(character.Equipment, -1))
				.SkeletonType(character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Scale(num)
				.Frame(frame)
				.ActionSet(actionSet)
				.Scene(this.MapScene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(false)
				.HasClippingPlane(true)
				.UseScaledWeapons(true)
				.ClothColor1(4291609515U)
				.ClothColor2(4291609515U)
				.CharacterObjectStringId(character.StringId)
				.Race(character.Race), "BattleWreckageVisual " + character.Name, false, false, false);
			if (agentVisuals != null)
			{
				List<ActionIndexCache> list = (this._isLandBattleWreckage ? this._landActionList : this._navalActionList);
				WeakGameEntity weakEntity = agentVisuals.GetWeakEntity();
				float num2 = MathF.Min(0.25f, 20f);
				Skeleton skeleton = weakEntity.Skeleton;
				int num3 = 0;
				ActionIndexCache randomElement = list.GetRandomElement<ActionIndexCache>();
				skeleton.SetAgentActionChannel(num3, in randomElement, MBRandom.NondeterministicRandomFloat * 0.7f, -0.2f, true, 0f);
				agentVisuals.Tick(null, 0.0001f, false, num2);
				weakEntity.Skeleton.ForceUpdateBoneFrames();
			}
			return agentVisuals;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		private AgentVisuals CreateMountAgentVisual(MatrixFrame frame)
		{
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse");
			Monster monster = @object.HorseComponent.Monster;
			MBActionSet actionSet = MBGlobals.GetActionSet("as_horse");
			Equipment equipment = new Equipment();
			equipment[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(@object, null, null, false);
			ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>("light_harness");
			equipment[EquipmentIndex.HorseHarness] = new EquipmentElement(object2, null, null, false);
			AgentVisuals agentVisuals = AgentVisuals.Create(new AgentVisualsData().Equipment(equipment).Scale(@object.ScaleFactor * 0.3f).Frame(frame)
				.ActionSet(actionSet)
				.Scene(this.MapScene)
				.Monster(monster)
				.PrepareImmediately(false)
				.UseScaledWeapons(true)
				.HasClippingPlane(true)
				.MountCreationKey(MountCreationKey.GetRandomMountKeyString(@object, MBRandom.NondeterministicRandomInt)), "BattleWreckageVisual mount", false, false, false);
			WeakGameEntity weakEntity = agentVisuals.GetWeakEntity();
			weakEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_horse_fall_right_continue, 0f, -0.2f, true, 0f);
			weakEntity.Skeleton.ForceUpdateBoneFrames();
			return agentVisuals;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
		private void SetInitialPosition()
		{
			MatrixFrame matrixFrame = this.CalculateFrame();
			this.Entity.SetFrame(ref matrixFrame, true);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001F0E4 File Offset: 0x0001D2E4
		private MatrixFrame CalculateFrame()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = this.GetVisualPosition();
			return identity;
		}

		// Token: 0x040001ED RID: 493
		private const float FadeSpeed = 1.5f;

		// Token: 0x040001EE RID: 494
		private const int BannerColorIndex = 99;

		// Token: 0x040001EF RID: 495
		private const string LandWreckagePrefabName = "wreckage_prefab";

		// Token: 0x040001F0 RID: 496
		private const string NavalWreckagePrefabName = "naval_wreckage_prefab";

		// Token: 0x040001F1 RID: 497
		private const string bannerMeshName = "vlandia_tier_1_banner";

		// Token: 0x040001F2 RID: 498
		private const string BattleRemainsTag = "battle_remains";

		// Token: 0x040001F3 RID: 499
		private const string FlagTag = "flag";

		// Token: 0x040001F4 RID: 500
		private const string LandSoundPath = "event:/map/ambient/node/wreckage/wreckage_land";

		// Token: 0x040001F5 RID: 501
		private const string NavalSoundPath = "event:/map/ambient/node/wreckage/wreckage_sea";

		// Token: 0x040001F6 RID: 502
		private float _entityAlpha;

		// Token: 0x040001F7 RID: 503
		private bool _lastKnownVisibility;

		// Token: 0x040001F8 RID: 504
		private bool _isInvestigated;

		// Token: 0x040001F9 RID: 505
		private bool _isLandBattleWreckage;

		// Token: 0x040001FA RID: 506
		private GameEntity _wreckageEntity;

		// Token: 0x040001FB RID: 507
		private ClothSimulatorComponent _bannerClothSimulator;

		// Token: 0x040001FC RID: 508
		private readonly List<AgentVisuals> _agentVisualList = new List<AgentVisuals>();

		// Token: 0x040001FD RID: 509
		private readonly List<ActionIndexCache> _landActionList = new List<ActionIndexCache>
		{
			ActionIndexCache.act_wreckage_death_01,
			ActionIndexCache.act_wreckage_death_02
		};

		// Token: 0x040001FE RID: 510
		private readonly List<ActionIndexCache> _navalActionList = new List<ActionIndexCache>
		{
			ActionIndexCache.act_death_swim_1,
			ActionIndexCache.act_death_swim_2
		};

		// Token: 0x040001FF RID: 511
		private Scene _mapScene;

		// Token: 0x04000200 RID: 512
		private SoundEvent _ambientSound;
	}
}
