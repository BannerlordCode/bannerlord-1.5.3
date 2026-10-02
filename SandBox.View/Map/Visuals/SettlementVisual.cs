using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Helpers;
using SandBox.ViewModelCollection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000067 RID: 103
	public class SettlementVisual : MapEntityVisual<PartyBase>
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00021DB1 File Offset: 0x0001FFB1
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00021DB9 File Offset: 0x0001FFB9
		private List<GameEntity> TownPhysicalEntities { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00021DC2 File Offset: 0x0001FFC2
		public override MapEntityVisual AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00021DC5 File Offset: 0x0001FFC5
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return ((IInteractablePoint)base.MapEntity).GetInteractionPosition(MobileParty.MainParty);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00021DD8 File Offset: 0x0001FFD8
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00021E26 File Offset: 0x00020026
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00021E2E File Offset: 0x0002002E
		public GameEntity StrategicEntity { get; private set; }

		// Token: 0x0600044C RID: 1100 RVA: 0x00021E37 File Offset: 0x00020037
		public SettlementVisual(PartyBase entity)
			: base(entity)
		{
			this._siegeRangedMachineEntities = new List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>>();
			this._siegeMeleeMachineEntities = new List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>>();
			this._siegeMissileEntities = new List<ValueTuple<GameEntity, BattleSideEnum, int>>();
			this.CircleLocalFrame = MatrixFrame.Identity;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00021E77 File Offset: 0x00020077
		public override bool IsEnemyOf(IFaction faction)
		{
			return FactionManager.IsAtWarAgainstFaction(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00021E8F File Offset: 0x0002008F
		public override bool IsInSameFaction(IFaction faction)
		{
			return DiplomacyHelper.IsSameFactionAndNotEliminated(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00021EA7 File Offset: 0x000200A7
		public override bool IsAllyOf(IFaction faction)
		{
			return DiplomacyHelper.HasAllianceWithFaction(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00021EC0 File Offset: 0x000200C0
		internal void OnPartyRemoved()
		{
			if (this.StrategicEntity != null)
			{
				MapScreen.VisualsOfEntities.Remove(this.StrategicEntity.Pointer);
				foreach (GameEntity gameEntity in this.StrategicEntity.GetChildren())
				{
					MapScreen.VisualsOfEntities.Remove(gameEntity.Pointer);
				}
				this.ReleaseResources();
				this.StrategicEntity.Remove(111);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00021F54 File Offset: 0x00020154
		public override Vec3 GetVisualPosition()
		{
			return base.MapEntity.Position.AsVec3();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00021F74 File Offset: 0x00020174
		public override bool IsVisibleOrFadingOut()
		{
			return base.MapEntity.IsVisible;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00021F84 File Offset: 0x00020184
		public override void OnHover()
		{
			if (base.MapEntity.MapEvent != null)
			{
				InformationManager.ShowTooltip(typeof(MapEvent), new object[] { base.MapEntity.MapEvent });
				return;
			}
			if (base.MapEntity.IsSettlement && base.MapEntity.IsVisible)
			{
				if (base.MapEntity.Settlement.SiegeEvent != null)
				{
					InformationManager.ShowTooltip(typeof(SiegeEvent), new object[] { base.MapEntity.Settlement.SiegeEvent });
					return;
				}
				InformationManager.ShowTooltip(typeof(Settlement), new object[] { base.MapEntity.Settlement });
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0002203C File Offset: 0x0002023C
		public override void OnTrackAction()
		{
			Settlement settlement = base.MapEntity.Settlement;
			if (settlement != null)
			{
				if (Campaign.Current.VisualTrackerManager.CheckTracked(settlement))
				{
					Campaign.Current.VisualTrackerManager.RemoveTrackedObject(settlement, false);
					return;
				}
				Campaign.Current.VisualTrackerManager.RegisterObject(settlement);
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0002208C File Offset: 0x0002028C
		public override bool OnMapClick(bool followModifierUsed)
		{
			if (followModifierUsed)
			{
				TextObject textObject;
				if (Campaign.Current.Models.EncounterModel.CanMainHeroDoParleyWithParty(base.MapEntity, out textObject))
				{
					base.MapScreen.BeginParleyWith(base.MapEntity);
				}
				else if (!TextObject.IsNullOrEmpty(textObject))
				{
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
			}
			else if (base.MapEntity.IsVisible)
			{
				bool flag;
				MobileParty.NavigationType navigationType;
				bool flag2;
				NavigationHelper.GetInteractionDataForMainParty(base.MapEntity.Settlement, out flag, out navigationType, out flag2);
				if (flag)
				{
					MobileParty.MainParty.SetMoveGoToSettlement(base.MapEntity.Settlement, navigationType, flag2);
				}
			}
			return true;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00022123 File Offset: 0x00020323
		public override void OnOpenEncyclopedia()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(base.MapEntity.Settlement.EncyclopediaLink);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00022144 File Offset: 0x00020344
		public override void ReleaseResources()
		{
			this.RemoveSiege();
			this.ResetPartyIcon();
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00022152 File Offset: 0x00020352
		private void ResetPartyIcon()
		{
			if (this.StrategicEntity != null)
			{
				if ((this.StrategicEntity.EntityFlags & EntityFlags.Ignore) != (EntityFlags)0U)
				{
					this.StrategicEntity.RemoveFromPredisplayEntity();
				}
				this.StrategicEntity.ClearComponents();
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0002218C File Offset: 0x0002038C
		internal void ValidateIsDirty()
		{
			this.RefreshPartyIcon();
			if (base.MapEntity.IsVisible)
			{
				this.StrategicEntity.SetVisibilityExcludeParents(true);
				this.StrategicEntity.SetAlpha(1f);
				this.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
				return;
			}
			this.StrategicEntity.SetAlpha(0f);
			this.StrategicEntity.SetVisibilityExcludeParents(false);
			this.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00022213 File Offset: 0x00020413
		internal Dictionary<int, List<GameEntity>> GetGateBannerEntitiesWithLevels()
		{
			return this._gateBannerEntitiesWithLevels;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0002221C File Offset: 0x0002041C
		public Vec3 GetBannerPositionForParty(MobileParty mobileParty)
		{
			if (mobileParty.CurrentSettlement == base.MapEntity.Settlement && base.MapEntity.Settlement.IsFortification && this._gateBannerEntitiesWithLevels != null && !this._gateBannerEntitiesWithLevels.IsEmpty<KeyValuePair<int, List<GameEntity>>>())
			{
				int wallLevel = base.MapEntity.Settlement.Town.GetWallLevel();
				int count = this._gateBannerEntitiesWithLevels[wallLevel].Count;
				if (this._gateBannerEntitiesWithLevels[wallLevel].Count > 0)
				{
					int num = 0;
					foreach (MobileParty mobileParty2 in base.MapEntity.Settlement.Parties)
					{
						if (mobileParty2 == mobileParty)
						{
							break;
						}
						Hero leaderHero = mobileParty2.LeaderHero;
						if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
						{
							num++;
						}
					}
					GameEntity gameEntity = this._gateBannerEntitiesWithLevels[wallLevel][num % count];
					GameEntity child = gameEntity.GetChild(0);
					MatrixFrame matrixFrame = ((child != null) ? child.GetGlobalFrame() : gameEntity.GetGlobalFrame());
					num /= count;
					int num2 = base.MapEntity.Settlement.Parties.Count<MobileParty>(delegate(MobileParty p)
					{
						Hero leaderHero2 = p.LeaderHero;
						return ((leaderHero2 != null) ? leaderHero2.ClanBanner : null) != null;
					});
					float num3 = 0.75f / (float)MathF.Max(1, num2 / (count * 2));
					int num4 = ((num % 2 == 0) ? (-1) : 1);
					Vec3 vec = matrixFrame.rotation.f / 2f * (float)num4;
					if (vec.Length < matrixFrame.rotation.s.Length)
					{
						vec = matrixFrame.rotation.s / 2f * (float)num4;
					}
					return matrixFrame.origin + vec * (float)((num + 1) / 2) * (float)(num % 2 * 2 - 1) * num3 * (float)num4;
				}
				Debug.FailedAssert(string.Format("{0} - has no Banner Entities at level {1}.", base.MapEntity.Settlement.Name, wallLevel), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Visuals\\SettlementVisual.cs", "GetBannerPositionForParty", 304);
			}
			return Vec3.Invalid;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00022478 File Offset: 0x00020678
		internal void OnMapHoverSiegeEngineEnd()
		{
			this._hoveredSiegeEntityFrame = MatrixFrame.Identity;
			MBInformationManager.HideInformations();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0002248C File Offset: 0x0002068C
		private void RefreshPartyIcon()
		{
			if (base.MapEntity.IsVisualDirty)
			{
				base.MapEntity.OnVisualsUpdated();
				this.RemoveSiege();
				this.StrategicEntity.RemoveAllParticleSystems();
				this.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
				if (base.MapEntity.Settlement.IsFortification)
				{
					this.UpdateDefenderSiegeEntitiesCache();
				}
				this.AddSiegeIconComponents(base.MapEntity);
				this.SetSettlementLevelVisibility();
				this.RefreshWallState();
				this.RefreshTownPhysicalEntitiesState(base.MapEntity);
				this.RefreshSiegePreparations(base.MapEntity);
				bool flag = false;
				if (base.MapEntity.Settlement.IsVillage)
				{
					MapEvent mapEvent = base.MapEntity.MapEvent;
					if (mapEvent != null && mapEvent.IsRaid)
					{
						this.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
						this.StrategicEntity.AddParticleSystemComponent("psys_fire_smoke_env_point");
						if ((this.StrategicEntity.EntityFlags & EntityFlags.Ignore) != (EntityFlags)0U)
						{
							this.StrategicEntity.RemoveFromPredisplayEntity();
						}
						flag = true;
					}
					else if (base.MapEntity.Settlement.IsRaided)
					{
						this.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
						this.StrategicEntity.AddParticleSystemComponent("map_icon_village_plunder_fx");
						if ((this.StrategicEntity.EntityFlags & EntityFlags.Ignore) != (EntityFlags)0U)
						{
							this.StrategicEntity.RemoveFromPredisplayEntity();
						}
						flag = true;
					}
				}
				if (!flag && (this.StrategicEntity.EntityFlags & EntityFlags.Ignore) == (EntityFlags)0U)
				{
					this.StrategicEntity.SetAsPredisplayEntity();
				}
				this.StrategicEntity.CheckResources(true, false);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00022628 File Offset: 0x00020828
		internal void OnStartup()
		{
			bool flag = false;
			this.StrategicEntity = this.MapScene.GetCampaignEntityWithName(base.MapEntity.Id);
			if (this.StrategicEntity == null)
			{
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				string stringId = base.MapEntity.Settlement.StringId;
				CampaignVec2 position = base.MapEntity.Settlement.Position;
				mapSceneWrapper.AddNewEntityToMapScene(stringId, in position);
				this.StrategicEntity = this.MapScene.GetCampaignEntityWithName(base.MapEntity.Id);
			}
			bool flag2 = false;
			if (base.MapEntity.Settlement.IsFortification)
			{
				List<GameEntity> list = new List<GameEntity>();
				this.StrategicEntity.GetChildrenRecursive(ref list);
				this.PopulateSiegeEngineFrameListsFromChildren(list);
				this.UpdateDefenderSiegeEntitiesCache();
				this.TownPhysicalEntities = list.FindAll((GameEntity x) => x.HasTag("bo_town"));
				List<GameEntity> list2 = new List<GameEntity>();
				Dictionary<int, List<GameEntity>> dictionary = new Dictionary<int, List<GameEntity>>
				{
					{
						1,
						new List<GameEntity>()
					},
					{
						2,
						new List<GameEntity>()
					},
					{
						3,
						new List<GameEntity>()
					}
				};
				foreach (GameEntity gameEntity in list)
				{
					if (gameEntity.HasTag("main_map_city_gate"))
					{
						NavigationHelper.IsPositionValidForNavigationType(new CampaignVec2(gameEntity.GetGlobalFrame().origin.AsVec2, true), MobileParty.NavigationType.Default);
						flag2 = true;
						list2.Add(gameEntity);
					}
					if (gameEntity.HasTag("map_settlement_circle"))
					{
						this.CircleLocalFrame = gameEntity.GetGlobalFrame();
						flag = true;
						gameEntity.SetVisibilityExcludeParents(false);
						list2.Add(gameEntity);
					}
					if (gameEntity.HasTag("map_banner_placeholder"))
					{
						int upgradeLevelOfEntity = gameEntity.Parent.GetUpgradeLevelOfEntity();
						if (upgradeLevelOfEntity == 0)
						{
							dictionary[1].Add(gameEntity);
							dictionary[2].Add(gameEntity);
							dictionary[3].Add(gameEntity);
						}
						else
						{
							dictionary[upgradeLevelOfEntity].Add(gameEntity);
						}
						list2.Add(gameEntity);
					}
				}
				this._gateBannerEntitiesWithLevels = dictionary;
				if (base.MapEntity.Settlement.IsFortification)
				{
					List<MatrixFrame> list3;
					List<MatrixFrame> list4;
					Campaign.Current.MapSceneWrapper.GetSiegeCampFrames(base.MapEntity.Settlement, out list3, out list4);
					base.MapEntity.Settlement.Town.BesiegerCampPositions1 = list3.ToArray();
					base.MapEntity.Settlement.Town.BesiegerCampPositions2 = list4.ToArray();
				}
				foreach (GameEntity gameEntity2 in list2)
				{
					gameEntity2.Remove(112);
				}
				if (!flag2 && !base.MapEntity.Settlement.IsTown)
				{
					bool isCastle = base.MapEntity.Settlement.IsCastle;
				}
				bool flag3 = false;
				if (base.MapEntity.IsSettlement)
				{
					foreach (GameEntity gameEntity3 in this.StrategicEntity.GetChildren())
					{
						if (gameEntity3.HasTag("main_map_city_port"))
						{
							NavigationHelper.IsPositionValidForNavigationType(new CampaignVec2(gameEntity3.GetGlobalFrame().origin.AsVec2, false), MobileParty.NavigationType.Naval);
							flag3 = true;
						}
					}
					if ((flag3 || !base.MapEntity.Settlement.HasPort) && flag3)
					{
						bool hasPort = base.MapEntity.Settlement.HasPort;
					}
				}
			}
			if (!flag)
			{
				this.CircleLocalFrame = MatrixFrame.Identity;
				MatrixFrame circleLocalFrame = this.CircleLocalFrame;
				Mat3 rotation = circleLocalFrame.rotation;
				if (base.MapEntity.Settlement.IsVillage)
				{
					rotation.ApplyScaleLocal(1.75f);
				}
				else if (base.MapEntity.Settlement.IsTown)
				{
					rotation.ApplyScaleLocal(5.75f);
				}
				else if (base.MapEntity.Settlement.IsCastle)
				{
					rotation.ApplyScaleLocal(2.75f);
				}
				else
				{
					rotation.ApplyScaleLocal(1.75f);
				}
				circleLocalFrame.rotation = rotation;
				this.CircleLocalFrame = circleLocalFrame;
			}
			this.StrategicEntity.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
			this.StrategicEntity.SetReadyToRender(true);
			this.StrategicEntity.SetEntityEnvMapVisibility(false);
			List<GameEntity> list5 = new List<GameEntity>();
			this.StrategicEntity.GetChildrenRecursive(ref list5);
			if (!MapScreen.VisualsOfEntities.ContainsKey(this.StrategicEntity.Pointer))
			{
				MapScreen.VisualsOfEntities.Add(this.StrategicEntity.Pointer, this);
			}
			foreach (GameEntity gameEntity4 in list5)
			{
				if (!MapScreen.VisualsOfEntities.ContainsKey(gameEntity4.Pointer) && !MapScreen.FrameAndVisualOfEngines.ContainsKey(gameEntity4.Pointer))
				{
					MapScreen.VisualsOfEntities.Add(gameEntity4.Pointer, this);
				}
			}
			this.StrategicEntity.SetAsPredisplayEntity();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00022B68 File Offset: 0x00020D68
		internal void Tick(float dt, ref int dirtyPartiesCount, ref SettlementVisual[] dirtyPartiesList)
		{
			if (this.StrategicEntity == null)
			{
				return;
			}
			if (base.MapEntity.IsVisualDirty)
			{
				int num = Interlocked.Increment(ref dirtyPartiesCount);
				dirtyPartiesList[num] = this;
			}
			else
			{
				double toHours = CampaignTime.Now.ToHours;
				foreach (ValueTuple<GameEntity, BattleSideEnum, int> valueTuple in this._siegeMissileEntities)
				{
					GameEntity item = valueTuple.Item1;
					ISiegeEventSide siegeEventSide = base.MapEntity.Settlement.SiegeEvent.GetSiegeEventSide(valueTuple.Item2);
					int item2 = valueTuple.Item3;
					bool flag = false;
					if (siegeEventSide.SiegeEngineMissiles.Count > item2)
					{
						SiegeEvent.SiegeEngineMissile siegeEngineMissile = siegeEventSide.SiegeEngineMissiles[item2];
						double toHours2 = siegeEngineMissile.CollisionTime.ToHours;
						SettlementVisual.SiegeBombardmentData siegeBombardmentData;
						this.CalculateDataAndDurationsForSiegeMachine(siegeEngineMissile.ShooterSlotIndex, siegeEngineMissile.ShooterSiegeEngineType, siegeEventSide.BattleSide, siegeEngineMissile.TargetType, siegeEngineMissile.TargetSlotIndex, out siegeBombardmentData);
						float num2 = siegeBombardmentData.MissileSpeed * MathF.Cos(siegeBombardmentData.LaunchAngle);
						if (toHours > toHours2 - (double)siegeBombardmentData.TotalDuration)
						{
							bool flag2 = toHours - (double)dt > toHours2 - (double)siegeBombardmentData.FlightDuration && toHours - (double)dt < toHours2;
							bool flag3 = toHours > toHours2 - (double)siegeBombardmentData.FlightDuration && toHours < toHours2;
							if (flag3)
							{
								flag = true;
								float num3 = (float)(toHours - (toHours2 - (double)siegeBombardmentData.FlightDuration));
								float num4 = siegeBombardmentData.MissileSpeed * MathF.Sin(siegeBombardmentData.LaunchAngle);
								Vec2 vec = new Vec2(num2 * num3, num4 * num3 - siegeBombardmentData.Gravity * 0.5f * num3 * num3);
								Vec3 vec2 = siegeBombardmentData.LaunchGlobalPosition + siegeBombardmentData.TargetAlignedShooterGlobalFrame.rotation.f.NormalizedCopy() * vec.x + siegeBombardmentData.TargetAlignedShooterGlobalFrame.rotation.u.NormalizedCopy() * vec.y;
								float num5 = num3 + 0.1f;
								Vec2 vec3 = new Vec2(num2 * num5, num4 * num5 - siegeBombardmentData.Gravity * 0.5f * num5 * num5);
								Vec3 vec4 = siegeBombardmentData.LaunchGlobalPosition + siegeBombardmentData.TargetAlignedShooterGlobalFrame.rotation.f.NormalizedCopy() * vec3.x + siegeBombardmentData.TargetAlignedShooterGlobalFrame.rotation.u.NormalizedCopy() * vec3.y;
								Mat3 rotation = item.GetGlobalFrame().rotation;
								rotation.f = vec4 - vec2;
								rotation.Orthonormalize();
								Vec3 vec5 = base.MapScreen.PrefabEntityCache.GetScaleForSiegeEngine(siegeEngineMissile.ShooterSiegeEngineType, siegeEventSide.BattleSide);
								rotation.ApplyScaleLocal(in vec5);
								MatrixFrame matrixFrame = new MatrixFrame(in rotation, in vec2);
								item.SetGlobalFrame(in matrixFrame, true);
							}
							item.WeakEntity.GetChild(0).SetVisibilityExcludeParents(flag3);
							int num6 = -1;
							if (!flag2 && flag3)
							{
								if (siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.Ballista || siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.FireBallista)
								{
									num6 = MiscSoundContainer.SoundCodeAmbientNodeSiegeBallistaFire;
								}
								else if (siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.Catapult || siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.FireCatapult || siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.Onager || siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.FireOnager)
								{
									num6 = MiscSoundContainer.SoundCodeAmbientNodeSiegeMangonelFire;
								}
								else
								{
									num6 = MiscSoundContainer.SoundCodeAmbientNodeSiegeTrebuchetFire;
								}
							}
							else if (flag2 && !flag3)
							{
								this.StrategicEntity.Scene.CreateBurstParticle(ParticleSystemManager.GetRuntimeIdByName((siegeEngineMissile.TargetType == SiegeBombardTargets.RangedEngines) ? "psys_game_ballista_destruction" : "psys_campaign_boulder_stone_coll"), item.GetGlobalFrame());
								num6 = ((siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.Ballista || siegeEngineMissile.ShooterSiegeEngineType == DefaultSiegeEngineTypes.FireBallista) ? MiscSoundContainer.SoundCodeAmbientNodeSiegeBallistaHit : MiscSoundContainer.SoundCodeAmbientNodeSiegeBoulderHit);
							}
							MBSoundEvent.PlaySound(num6, item.GlobalPosition);
							if (toHours >= toHours2 - (double)(siegeBombardmentData.TotalDuration - siegeBombardmentData.RotationDuration - siegeBombardmentData.ReloadDuration))
							{
								if (toHours < toHours2 - (double)(siegeBombardmentData.TotalDuration - siegeBombardmentData.RotationDuration - siegeBombardmentData.ReloadDuration - siegeBombardmentData.AimingDuration))
								{
									if (siegeEventSide.SiegeEngines.DeployedRangedSiegeEngines[siegeEngineMissile.ShooterSlotIndex] == null || siegeEventSide.SiegeEngines.DeployedRangedSiegeEngines[siegeEngineMissile.ShooterSlotIndex].SiegeEngine != siegeEngineMissile.ShooterSiegeEngineType)
									{
										goto IL_066E;
									}
									using (List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>>.Enumerator enumerator2 = this._siegeRangedMachineEntities.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity> valueTuple2 = enumerator2.Current;
											if (!flag && valueTuple2.Item2 == siegeEventSide.BattleSide && valueTuple2.Item3 == siegeEngineMissile.ShooterSlotIndex)
											{
												GameEntity item3 = valueTuple2.Item5;
												if (item3 != null)
												{
													flag = true;
													GameEntity gameEntity = item;
													MatrixFrame matrixFrame2 = item3.GetGlobalFrame();
													MatrixFrame matrixFrame3 = item3.Skeleton.GetBoneEntitialFrame(Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapProjectileBoneIndex(siegeEngineMissile.ShooterSiegeEngineType, siegeEventSide.BattleSide), false);
													matrixFrame2 = matrixFrame2.TransformToParent(in matrixFrame3);
													gameEntity.SetGlobalFrame(in matrixFrame2, true);
												}
											}
										}
										goto IL_066E;
									}
								}
								if (toHours < toHours2 - (double)(siegeBombardmentData.TotalDuration - siegeBombardmentData.RotationDuration - siegeBombardmentData.ReloadDuration - siegeBombardmentData.AimingDuration - siegeBombardmentData.FireDuration) && !flag3 && siegeEventSide.SiegeEngines.DeployedRangedSiegeEngines[siegeEngineMissile.ShooterSlotIndex] != null && siegeEventSide.SiegeEngines.DeployedRangedSiegeEngines[siegeEngineMissile.ShooterSlotIndex].SiegeEngine == siegeEngineMissile.ShooterSiegeEngineType)
								{
									foreach (ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity> valueTuple3 in this._siegeRangedMachineEntities)
									{
										if (!flag && valueTuple3.Item2 == siegeEventSide.BattleSide && valueTuple3.Item3 == siegeEngineMissile.ShooterSlotIndex)
										{
											GameEntity item4 = valueTuple3.Item5;
											if (item4 != null)
											{
												flag = true;
												GameEntity gameEntity2 = item;
												MatrixFrame matrixFrame3 = item4.GetGlobalFrame();
												MatrixFrame matrixFrame2 = item4.Skeleton.GetBoneEntitialFrame(Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapProjectileBoneIndex(siegeEngineMissile.ShooterSiegeEngineType, siegeEventSide.BattleSide), false);
												matrixFrame3 = matrixFrame3.TransformToParent(in matrixFrame2);
												gameEntity2.SetGlobalFrame(in matrixFrame3, true);
											}
										}
									}
								}
							}
						}
					}
					IL_066E:
					item.SetVisibilityExcludeParents(flag);
				}
				foreach (ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity> valueTuple4 in this._siegeRangedMachineEntities)
				{
					GameEntity item5 = valueTuple4.Item1;
					BattleSideEnum item6 = valueTuple4.Item2;
					int item7 = valueTuple4.Item3;
					GameEntity item8 = valueTuple4.Item5;
					SiegeEngineType siegeEngine = base.MapEntity.Settlement.SiegeEvent.GetSiegeEventSide(item6).SiegeEngines.DeployedRangedSiegeEngines[item7].SiegeEngine;
					if (item8 != null)
					{
						Skeleton skeleton = item8.Skeleton;
						string siegeEngineMapFireAnimationName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapFireAnimationName(siegeEngine, item6);
						string siegeEngineMapReloadAnimationName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapReloadAnimationName(siegeEngine, item6);
						SiegeEvent.RangedSiegeEngine rangedSiegeEngine = base.MapEntity.Settlement.SiegeEvent.GetSiegeEventSide(item6).SiegeEngines.DeployedRangedSiegeEngines[item7].RangedSiegeEngine;
						SettlementVisual.SiegeBombardmentData siegeBombardmentData2;
						this.CalculateDataAndDurationsForSiegeMachine(item7, siegeEngine, item6, rangedSiegeEngine.CurrentTargetType, rangedSiegeEngine.CurrentTargetIndex, out siegeBombardmentData2);
						MatrixFrame shooterGlobalFrame = siegeBombardmentData2.ShooterGlobalFrame;
						if (rangedSiegeEngine.PreviousTargetIndex >= 0)
						{
							Vec3 vec6;
							if (rangedSiegeEngine.PreviousDamagedTargetType == SiegeBombardTargets.Wall)
							{
								vec6 = this._defenderBreachableWallEntitiesCacheForCurrentLevel[rangedSiegeEngine.PreviousTargetIndex].GlobalPosition;
							}
							else
							{
								vec6 = ((item6 == BattleSideEnum.Attacker) ? this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[rangedSiegeEngine.PreviousTargetIndex].GetGlobalFrame().origin : this._attackerRangedEngineSpawnEntities[rangedSiegeEngine.PreviousTargetIndex].GetGlobalFrame().origin);
							}
							Vec3 vec5 = vec6 - shooterGlobalFrame.origin;
							shooterGlobalFrame.rotation.f.AsVec2 = vec5.AsVec2;
							shooterGlobalFrame.rotation.f.NormalizeWithoutChangingZ();
							shooterGlobalFrame.rotation.Orthonormalize();
						}
						item5.SetGlobalFrame(in shooterGlobalFrame, true);
						skeleton.TickAnimations(dt, MatrixFrame.Identity, false);
						double toHours3 = rangedSiegeEngine.NextProjectileCollisionTime.ToHours;
						if (toHours > toHours3 - (double)siegeBombardmentData2.TotalDuration)
						{
							if (toHours < toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration))
							{
								Vec3 vec5 = siegeBombardmentData2.TargetPosition - shooterGlobalFrame.origin;
								float rotationInRadians = vec5.AsVec2.RotationInRadians;
								float rotationInRadians2 = shooterGlobalFrame.rotation.f.AsVec2.RotationInRadians;
								float num7 = rotationInRadians - rotationInRadians2;
								float num8 = MathF.Abs(num7);
								float num9 = (float)(toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration) - toHours);
								if (num8 > num9 * 2f)
								{
									shooterGlobalFrame.rotation.f.AsVec2 = Vec2.FromRotation(rotationInRadians2 + (float)MathF.Sign(num7) * (num8 - num9 * 2f));
									shooterGlobalFrame.rotation.f.NormalizeWithoutChangingZ();
									shooterGlobalFrame.rotation.Orthonormalize();
									item5.SetGlobalFrame(in shooterGlobalFrame, true);
								}
							}
							else if (toHours < toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration - siegeBombardmentData2.ReloadDuration))
							{
								item5.SetGlobalFrame(in siegeBombardmentData2.TargetAlignedShooterGlobalFrame, true);
								skeleton.SetAnimationAtChannel(siegeEngineMapReloadAnimationName, 0, 1f, 0f, (float)((toHours - (toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration))) / (double)siegeBombardmentData2.ReloadDuration));
							}
							else if (toHours < toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration - siegeBombardmentData2.ReloadDuration - siegeBombardmentData2.AimingDuration))
							{
								item5.SetGlobalFrame(in siegeBombardmentData2.TargetAlignedShooterGlobalFrame, true);
								skeleton.SetAnimationAtChannel(siegeEngineMapReloadAnimationName, 0, 1f, 0f, 1f);
							}
							else if (toHours < toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration - siegeBombardmentData2.ReloadDuration - siegeBombardmentData2.AimingDuration - siegeBombardmentData2.FireDuration))
							{
								item5.SetGlobalFrame(in siegeBombardmentData2.TargetAlignedShooterGlobalFrame, true);
								skeleton.SetAnimationAtChannel(siegeEngineMapFireAnimationName, 0, 1f, 0f, (float)((toHours - (toHours3 - (double)(siegeBombardmentData2.TotalDuration - siegeBombardmentData2.RotationDuration - siegeBombardmentData2.ReloadDuration - siegeBombardmentData2.AimingDuration))) / (double)siegeBombardmentData2.FireDuration));
							}
							else
							{
								item5.SetGlobalFrame(in siegeBombardmentData2.TargetAlignedShooterGlobalFrame, true);
								skeleton.SetAnimationAtChannel(siegeEngineMapFireAnimationName, 0, 1f, 0f, 1f);
							}
						}
					}
				}
			}
			if (base.MapEntity.LevelMaskIsDirty)
			{
				this.RefreshLevelMask();
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000236C8 File Offset: 0x000218C8
		internal void OnMapHoverSiegeEngine(MatrixFrame engineFrame)
		{
			if (PlayerSiege.PlayerSiegeEvent == null)
			{
				return;
			}
			for (int i = 0; i < this._attackerBatteringRamSpawnEntities.Length; i++)
			{
				MatrixFrame globalFrame = this._attackerBatteringRamSpawnEntities[i].GetGlobalFrame();
				if (globalFrame.NearlyEquals(engineFrame, 1E-05f))
				{
					if ((in this._hoveredSiegeEntityFrame) != (in globalFrame))
					{
						SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress = PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedMeleeSiegeEngines[i];
						InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetSiegeEngineInProgressTooltip(siegeEngineConstructionProgress) });
					}
					return;
				}
			}
			for (int j = 0; j < this._attackerSiegeTowerSpawnEntities.Length; j++)
			{
				MatrixFrame globalFrame2 = this._attackerSiegeTowerSpawnEntities[j].GetGlobalFrame();
				if (globalFrame2.NearlyEquals(engineFrame, 1E-05f))
				{
					if ((in this._hoveredSiegeEntityFrame) != (in globalFrame2))
					{
						SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress2 = PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedMeleeSiegeEngines[this._attackerBatteringRamSpawnEntities.Length + j];
						InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetSiegeEngineInProgressTooltip(siegeEngineConstructionProgress2) });
					}
					return;
				}
			}
			for (int k = 0; k < this._attackerRangedEngineSpawnEntities.Length; k++)
			{
				MatrixFrame globalFrame3 = this._attackerRangedEngineSpawnEntities[k].GetGlobalFrame();
				if (globalFrame3.NearlyEquals(engineFrame, 1E-05f))
				{
					if ((in this._hoveredSiegeEntityFrame) != (in globalFrame3))
					{
						SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress3 = PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedRangedSiegeEngines[k];
						InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetSiegeEngineInProgressTooltip(siegeEngineConstructionProgress3) });
					}
					return;
				}
			}
			for (int l = 0; l < this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel.Length; l++)
			{
				MatrixFrame globalFrame4 = this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[l].GetGlobalFrame();
				if (globalFrame4.NearlyEquals(engineFrame, 1E-05f))
				{
					if ((in this._hoveredSiegeEntityFrame) != (in globalFrame4))
					{
						SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress4 = PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).SiegeEngines.DeployedRangedSiegeEngines[l];
						InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetSiegeEngineInProgressTooltip(siegeEngineConstructionProgress4) });
					}
					return;
				}
			}
			for (int m = 0; m < this._defenderBreachableWallEntitiesCacheForCurrentLevel.Length; m++)
			{
				MatrixFrame globalFrame5 = this._defenderBreachableWallEntitiesCacheForCurrentLevel[m].GetGlobalFrame();
				if (globalFrame5.NearlyEquals(engineFrame, 1E-05f))
				{
					if ((in this._hoveredSiegeEntityFrame) != (in globalFrame5) && base.MapEntity.IsSettlement)
					{
						InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetWallSectionTooltip(base.MapEntity.Settlement, m) });
					}
					return;
				}
			}
			this._hoveredSiegeEntityFrame = MatrixFrame.Identity;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0002395C File Offset: 0x00021B5C
		private void RemoveSiege()
		{
			foreach (ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity> valueTuple in this._siegeRangedMachineEntities)
			{
				this.StrategicEntity.RemoveChild(valueTuple.Item1, false, false, true, 36);
			}
			foreach (ValueTuple<GameEntity, BattleSideEnum, int> valueTuple2 in this._siegeMissileEntities)
			{
				this.StrategicEntity.RemoveChild(valueTuple2.Item1, false, false, true, 37);
			}
			foreach (ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity> valueTuple3 in this._siegeMeleeMachineEntities)
			{
				this.StrategicEntity.RemoveChild(valueTuple3.Item1, false, false, true, 38);
			}
			this._siegeRangedMachineEntities.Clear();
			this._siegeMeleeMachineEntities.Clear();
			this._siegeMissileEntities.Clear();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00023A84 File Offset: 0x00021C84
		private void AddSiegeIconComponents(PartyBase party)
		{
			if (party.Settlement.IsUnderSiege)
			{
				int num = -1;
				if (party.Settlement.SiegeEvent.BesiegedSettlement.IsTown || party.Settlement.SiegeEvent.BesiegedSettlement.IsCastle)
				{
					num = party.Settlement.SiegeEvent.BesiegedSettlement.Town.GetWallLevel();
				}
				SiegeEvent.SiegeEngineConstructionProgress[] deployedRangedSiegeEngines = party.Settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedRangedSiegeEngines;
				for (int i = 0; i < deployedRangedSiegeEngines.Length; i++)
				{
					SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress = deployedRangedSiegeEngines[i];
					if (siegeEngineConstructionProgress != null && siegeEngineConstructionProgress.IsActive && i < this._attackerRangedEngineSpawnEntities.Length)
					{
						MatrixFrame globalFrame = this._attackerRangedEngineSpawnEntities[i].GetGlobalFrame();
						globalFrame.rotation.MakeUnit();
						this.AddSiegeMachine(deployedRangedSiegeEngines[i].SiegeEngine, globalFrame, BattleSideEnum.Attacker, num, i);
					}
				}
				SiegeEvent.SiegeEngineConstructionProgress[] deployedMeleeSiegeEngines = party.Settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedMeleeSiegeEngines;
				for (int j = 0; j < deployedMeleeSiegeEngines.Length; j++)
				{
					SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress2 = deployedMeleeSiegeEngines[j];
					if (siegeEngineConstructionProgress2 != null && siegeEngineConstructionProgress2.IsActive)
					{
						if (deployedMeleeSiegeEngines[j].SiegeEngine == DefaultSiegeEngineTypes.SiegeTower)
						{
							int num2 = j - this._attackerBatteringRamSpawnEntities.Length;
							if (num2 >= 0)
							{
								MatrixFrame globalFrame2 = this._attackerSiegeTowerSpawnEntities[num2].GetGlobalFrame();
								globalFrame2.rotation.MakeUnit();
								this.AddSiegeMachine(deployedMeleeSiegeEngines[j].SiegeEngine, globalFrame2, BattleSideEnum.Attacker, num, j);
							}
						}
						else if (deployedMeleeSiegeEngines[j].SiegeEngine == DefaultSiegeEngineTypes.Ram || deployedMeleeSiegeEngines[j].SiegeEngine == DefaultSiegeEngineTypes.ImprovedRam)
						{
							int num3 = j;
							if (num3 >= 0)
							{
								MatrixFrame globalFrame3 = this._attackerBatteringRamSpawnEntities[num3].GetGlobalFrame();
								globalFrame3.rotation.MakeUnit();
								this.AddSiegeMachine(deployedMeleeSiegeEngines[j].SiegeEngine, globalFrame3, BattleSideEnum.Attacker, num, j);
							}
						}
					}
				}
				SiegeEvent.SiegeEngineConstructionProgress[] deployedRangedSiegeEngines2 = party.Settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).SiegeEngines.DeployedRangedSiegeEngines;
				for (int k = 0; k < deployedRangedSiegeEngines2.Length; k++)
				{
					SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress3 = deployedRangedSiegeEngines2[k];
					if (siegeEngineConstructionProgress3 != null && siegeEngineConstructionProgress3.IsActive)
					{
						MatrixFrame globalFrame4 = this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[k].GetGlobalFrame();
						globalFrame4.rotation.MakeUnit();
						this.AddSiegeMachine(deployedRangedSiegeEngines2[k].SiegeEngine, globalFrame4, BattleSideEnum.Defender, num, k);
					}
				}
				for (int l = 0; l < 2; l++)
				{
					BattleSideEnum battleSideEnum = ((l == 0) ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
					MBReadOnlyList<SiegeEvent.SiegeEngineMissile> siegeEngineMissiles = party.Settlement.SiegeEvent.GetSiegeEventSide(battleSideEnum).SiegeEngineMissiles;
					for (int m = 0; m < siegeEngineMissiles.Count; m++)
					{
						this.AddSiegeMissile(siegeEngineMissiles[m].ShooterSiegeEngineType, this.StrategicEntity.GetGlobalFrame(), battleSideEnum, m);
					}
				}
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00023D48 File Offset: 0x00021F48
		private void AddSiegeMachine(SiegeEngineType type, MatrixFrame globalFrame, BattleSideEnum side, int wallLevel, int slotIndex)
		{
			string siegeEngineMapPrefabName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapPrefabName(type, wallLevel, side);
			GameEntity gameEntity = GameEntity.Instantiate(this.MapScene, siegeEngineMapPrefabName, true, true, "");
			if (gameEntity != null)
			{
				this.StrategicEntity.AddChild(gameEntity, false);
				MatrixFrame matrixFrame;
				gameEntity.GetLocalFrame(out matrixFrame);
				GameEntity gameEntity2 = gameEntity;
				MatrixFrame matrixFrame2 = globalFrame.TransformToParent(in matrixFrame);
				gameEntity2.SetGlobalFrame(in matrixFrame2, true);
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				gameEntity.WeakEntity.GetChildrenRecursive(ref list);
				GameEntity gameEntity3 = null;
				if (list.Any<WeakGameEntity>((WeakGameEntity entity) => entity.HasTag("siege_machine_mapicon_skeleton")))
				{
					WeakGameEntity weakGameEntity = list.Find((WeakGameEntity entity) => entity.HasTag("siege_machine_mapicon_skeleton"));
					if (weakGameEntity.Skeleton != null)
					{
						gameEntity3 = GameEntity.CreateFromWeakEntity(weakGameEntity);
						string siegeEngineMapFireAnimationName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapFireAnimationName(type, side);
						gameEntity3.Skeleton.SetAnimationAtChannel(siegeEngineMapFireAnimationName, 0, 1f, 0f, 1f);
					}
				}
				if (type.IsRanged)
				{
					this._siegeRangedMachineEntities.Add(ValueTuple.Create<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>(gameEntity, side, slotIndex, globalFrame, gameEntity3));
					return;
				}
				this._siegeMeleeMachineEntities.Add(ValueTuple.Create<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>(gameEntity, side, slotIndex, globalFrame, gameEntity3));
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00023EA4 File Offset: 0x000220A4
		private void AddSiegeMissile(SiegeEngineType type, MatrixFrame globalFrame, BattleSideEnum side, int missileIndex)
		{
			string siegeEngineMapProjectilePrefabName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapProjectilePrefabName(type);
			GameEntity gameEntity = GameEntity.Instantiate(this.MapScene, siegeEngineMapProjectilePrefabName, true, true, "");
			if (gameEntity != null)
			{
				this._siegeMissileEntities.Add(ValueTuple.Create<GameEntity, BattleSideEnum, int>(gameEntity, side, missileIndex));
				this.StrategicEntity.AddChild(gameEntity, false);
				this.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
				MatrixFrame matrixFrame;
				gameEntity.GetLocalFrame(out matrixFrame);
				GameEntity gameEntity2 = gameEntity;
				MatrixFrame matrixFrame2 = globalFrame.TransformToParent(in matrixFrame);
				gameEntity2.SetGlobalFrame(in matrixFrame2, true);
				gameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00023F3E File Offset: 0x0002213E
		private void SetLevelMask(uint newMask)
		{
			this._currentLevelMask = newMask;
			base.MapEntity.SetVisualAsDirty();
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00023F54 File Offset: 0x00022154
		private void RefreshLevelMask()
		{
			uint num = 0U;
			if (base.MapEntity.Settlement.IsVillage)
			{
				if (base.MapEntity.Settlement.Village.VillageState == Village.VillageStates.Looted)
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("looted");
				}
				else
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("civilian");
				}
				num |= SettlementVisual.GetLevelOfProduction(base.MapEntity.Settlement);
			}
			else if (base.MapEntity.Settlement.IsTown || base.MapEntity.Settlement.IsCastle)
			{
				if (base.MapEntity.Settlement.Town.GetWallLevel() == 1)
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_1");
				}
				else if (base.MapEntity.Settlement.Town.GetWallLevel() == 2)
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_2");
				}
				else if (base.MapEntity.Settlement.Town.GetWallLevel() == 3)
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_3");
				}
				if (base.MapEntity.Settlement.SiegeEvent != null)
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("siege");
				}
				else
				{
					num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("civilian");
				}
			}
			else if (base.MapEntity.Settlement.IsHideout)
			{
				num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_1");
			}
			if (this._currentLevelMask != num)
			{
				this.SetLevelMask(num);
			}
			base.MapEntity.OnLevelMaskUpdated();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00024118 File Offset: 0x00022318
		private static uint GetLevelOfProduction(Settlement settlement)
		{
			uint num = 0U;
			if (settlement.Village.Hearth < 200f)
			{
				num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_1");
			}
			else if (settlement.Village.Hearth < 600f)
			{
				num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_2");
			}
			else
			{
				num |= Campaign.Current.MapSceneWrapper.GetSceneLevel("level_3");
			}
			return num;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00024198 File Offset: 0x00022398
		private void SetSettlementLevelVisibility()
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this.StrategicEntity.WeakEntity.GetChildrenRecursive(ref list);
			foreach (WeakGameEntity weakGameEntity in list)
			{
				if ((weakGameEntity.GetUpgradeLevelMask() & (GameEntity.UpgradeLevelMask)this._currentLevelMask) == (GameEntity.UpgradeLevelMask)this._currentLevelMask)
				{
					weakGameEntity.SetVisibilityExcludeParents(true);
					weakGameEntity.SetPhysicsState(true, true);
				}
				else
				{
					weakGameEntity.SetVisibilityExcludeParents(false);
					weakGameEntity.SetPhysicsState(false, true);
				}
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00024234 File Offset: 0x00022434
		private void PopulateSiegeEngineFrameListsFromChildren(List<GameEntity> children)
		{
			this._attackerRangedEngineSpawnEntities = (from e in children.FindAll((GameEntity x) => x.Tags.Any<string>((string t) => t.Contains("map_siege_engine")))
				orderby e.Tags.First<string>((string s) => s.Contains("map_siege_engine"))
				select e).ToArray<GameEntity>();
			foreach (GameEntity gameEntity in this._attackerRangedEngineSpawnEntities)
			{
				if (gameEntity.ChildCount > 0 && !MapScreen.FrameAndVisualOfEngines.ContainsKey(gameEntity.GetChild(0).Pointer))
				{
					MapScreen.FrameAndVisualOfEngines.Add(gameEntity.GetChild(0).Pointer, new Tuple<MatrixFrame, SettlementVisual>(gameEntity.GetGlobalFrame(), this));
				}
			}
			this._defenderRangedEngineSpawnEntitiesForAllLevels = (from e in children.FindAll((GameEntity x) => x.Tags.Any<string>((string t) => t.Contains("map_defensive_engine")))
				orderby e.Tags.First<string>((string s) => s.Contains("map_defensive_engine"))
				select e).ToArray<GameEntity>();
			foreach (GameEntity gameEntity2 in this._defenderRangedEngineSpawnEntitiesForAllLevels)
			{
				if (gameEntity2.ChildCount > 0 && !MapScreen.FrameAndVisualOfEngines.ContainsKey(gameEntity2.GetChild(0).Pointer))
				{
					MapScreen.FrameAndVisualOfEngines.Add(gameEntity2.GetChild(0).Pointer, new Tuple<MatrixFrame, SettlementVisual>(gameEntity2.GetGlobalFrame(), this));
				}
			}
			this._attackerBatteringRamSpawnEntities = children.FindAll((GameEntity x) => x.HasTag("map_siege_ram")).ToArray();
			foreach (GameEntity gameEntity3 in this._attackerBatteringRamSpawnEntities)
			{
				if (gameEntity3.ChildCount > 0 && !MapScreen.FrameAndVisualOfEngines.ContainsKey(gameEntity3.GetChild(0).Pointer))
				{
					MapScreen.FrameAndVisualOfEngines.Add(gameEntity3.GetChild(0).Pointer, new Tuple<MatrixFrame, SettlementVisual>(gameEntity3.GetGlobalFrame(), this));
				}
			}
			this._attackerSiegeTowerSpawnEntities = children.FindAll((GameEntity x) => x.HasTag("map_siege_tower")).ToArray();
			foreach (GameEntity gameEntity4 in this._attackerSiegeTowerSpawnEntities)
			{
				if (gameEntity4.ChildCount > 0 && !MapScreen.FrameAndVisualOfEngines.ContainsKey(gameEntity4.GetChild(0).Pointer))
				{
					MapScreen.FrameAndVisualOfEngines.Add(gameEntity4.GetChild(0).Pointer, new Tuple<MatrixFrame, SettlementVisual>(gameEntity4.GetGlobalFrame(), this));
				}
			}
			this._defenderBreachableWallEntitiesForAllLevels = children.FindAll((GameEntity x) => x.HasTag("map_breachable_wall")).ToArray();
			foreach (GameEntity gameEntity5 in this._defenderBreachableWallEntitiesForAllLevels)
			{
				if (gameEntity5.ChildCount > 0 && !MapScreen.FrameAndVisualOfEngines.ContainsKey(gameEntity5.GetChild(0).Pointer))
				{
					MapScreen.FrameAndVisualOfEngines.Add(gameEntity5.GetChild(0).Pointer, new Tuple<MatrixFrame, SettlementVisual>(gameEntity5.GetGlobalFrame(), this));
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00024554 File Offset: 0x00022754
		private void UpdateDefenderSiegeEntitiesCache()
		{
			GameEntity.UpgradeLevelMask upgradeLevelMask = GameEntity.UpgradeLevelMask.None;
			if (base.MapEntity.IsSettlement && base.MapEntity.Settlement.IsFortification)
			{
				if (base.MapEntity.Settlement.Town.GetWallLevel() == 1)
				{
					upgradeLevelMask = GameEntity.UpgradeLevelMask.Level1;
				}
				else if (base.MapEntity.Settlement.Town.GetWallLevel() == 2)
				{
					upgradeLevelMask = GameEntity.UpgradeLevelMask.Level2;
				}
				else if (base.MapEntity.Settlement.Town.GetWallLevel() == 3)
				{
					upgradeLevelMask = GameEntity.UpgradeLevelMask.Level3;
				}
			}
			this._currentSettlementUpgradeLevelMask = upgradeLevelMask;
			this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel = this._defenderRangedEngineSpawnEntitiesForAllLevels.Where<GameEntity>((GameEntity e) => (e.GetUpgradeLevelMask() & this._currentSettlementUpgradeLevelMask) == this._currentSettlementUpgradeLevelMask).ToArray<GameEntity>();
			this._defenderBreachableWallEntitiesCacheForCurrentLevel = this._defenderBreachableWallEntitiesForAllLevels.Where<GameEntity>((GameEntity e) => (e.GetUpgradeLevelMask() & this._currentSettlementUpgradeLevelMask) == this._currentSettlementUpgradeLevelMask).ToArray<GameEntity>();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00024620 File Offset: 0x00022820
		private void RefreshWallState()
		{
			if (this._defenderBreachableWallEntitiesForAllLevels != null)
			{
				PartyBase mapEntity = base.MapEntity;
				MBReadOnlyList<float> mbreadOnlyList;
				if (((mapEntity != null) ? mapEntity.Settlement : null) == null || (base.MapEntity.Settlement != null && !base.MapEntity.Settlement.IsFortification))
				{
					mbreadOnlyList = null;
				}
				else
				{
					mbreadOnlyList = base.MapEntity.Settlement.SettlementWallSectionHitPointsRatioList;
				}
				if (mbreadOnlyList != null)
				{
					if (mbreadOnlyList.Count == 0)
					{
						Debug.FailedAssert(string.Concat(new object[]
						{
							"Town (",
							base.MapEntity.Settlement.Name.ToString(),
							") doesn't have wall entities defined for it's current level(",
							base.MapEntity.Settlement.Town.GetWallLevel(),
							")"
						}), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Visuals\\SettlementVisual.cs", "RefreshWallState", 1301);
						return;
					}
					for (int i = 0; i < this._defenderBreachableWallEntitiesForAllLevels.Length; i++)
					{
						bool flag = mbreadOnlyList[i % mbreadOnlyList.Count] <= 0f;
						foreach (WeakGameEntity weakGameEntity in this._defenderBreachableWallEntitiesForAllLevels[i].WeakEntity.GetChildren())
						{
							if (weakGameEntity.HasTag("map_solid_wall"))
							{
								weakGameEntity.SetVisibilityExcludeParents(!flag);
							}
							else if (weakGameEntity.HasTag("map_broken_wall"))
							{
								weakGameEntity.SetVisibilityExcludeParents(flag);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000247A8 File Offset: 0x000229A8
		private void RefreshTownPhysicalEntitiesState(PartyBase party)
		{
			if (((party != null) ? party.Settlement : null) != null && party.Settlement.IsFortification && this.TownPhysicalEntities != null)
			{
				if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSiegeEvent.BesiegedSettlement == party.Settlement)
				{
					this.TownPhysicalEntities.ForEach(delegate(GameEntity p)
					{
						p.AddBodyFlags(BodyFlags.Disabled, true);
					});
					return;
				}
				this.TownPhysicalEntities.ForEach(delegate(GameEntity p)
				{
					p.RemoveBodyFlags(BodyFlags.Disabled, true);
				});
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0002484C File Offset: 0x00022A4C
		private void RefreshSiegePreparations(PartyBase party)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this.StrategicEntity.WeakEntity.GetChildrenRecursive(ref list);
			List<WeakGameEntity> list2 = list.FindAll((WeakGameEntity x) => x.HasTag("siege_preparation"));
			bool flag = false;
			if (party.Settlement != null && party.Settlement.IsUnderSiege)
			{
				SiegeEvent.SiegeEngineConstructionProgress siegePreparations = party.Settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.SiegePreparations;
				if (siegePreparations != null && siegePreparations.Progress >= 1f)
				{
					flag = true;
					foreach (WeakGameEntity weakGameEntity in list2)
					{
						weakGameEntity.SetVisibilityExcludeParents(true);
					}
				}
			}
			if (!flag)
			{
				foreach (WeakGameEntity weakGameEntity2 in list2)
				{
					weakGameEntity2.SetVisibilityExcludeParents(false);
				}
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0002496C File Offset: 0x00022B6C
		public MatrixFrame[] GetAttackerTowerSiegeEngineFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._attackerSiegeTowerSpawnEntities.Length];
			for (int i = 0; i < this._attackerSiegeTowerSpawnEntities.Length; i++)
			{
				array[i] = this._attackerSiegeTowerSpawnEntities[i].GetGlobalFrame();
			}
			return array;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000249B0 File Offset: 0x00022BB0
		public MatrixFrame[] GetAttackerBatteringRamSiegeEngineFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._attackerBatteringRamSpawnEntities.Length];
			for (int i = 0; i < this._attackerBatteringRamSpawnEntities.Length; i++)
			{
				array[i] = this._attackerBatteringRamSpawnEntities[i].GetGlobalFrame();
			}
			return array;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000249F4 File Offset: 0x00022BF4
		public MatrixFrame[] GetAttackerRangedSiegeEngineFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._attackerRangedEngineSpawnEntities.Length];
			for (int i = 0; i < this._attackerRangedEngineSpawnEntities.Length; i++)
			{
				array[i] = this._attackerRangedEngineSpawnEntities[i].GetGlobalFrame();
			}
			return array;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00024A38 File Offset: 0x00022C38
		public MatrixFrame[] GetDefenderRangedSiegeEngineFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel.Length];
			for (int i = 0; i < this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel.Length; i++)
			{
				array[i] = this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[i].GetGlobalFrame();
			}
			return array;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00024A7C File Offset: 0x00022C7C
		public MatrixFrame[] GetBreachableWallFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._defenderBreachableWallEntitiesCacheForCurrentLevel.Length];
			for (int i = 0; i < this._defenderBreachableWallEntitiesCacheForCurrentLevel.Length; i++)
			{
				array[i] = this._defenderBreachableWallEntitiesCacheForCurrentLevel[i].GetGlobalFrame();
			}
			return array;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00024AC0 File Offset: 0x00022CC0
		private void CalculateDataAndDurationsForSiegeMachine(int machineSlotIndex, SiegeEngineType machineType, BattleSideEnum side, SiegeBombardTargets targetType, int targetSlotIndex, out SettlementVisual.SiegeBombardmentData bombardmentData)
		{
			bombardmentData = default(SettlementVisual.SiegeBombardmentData);
			MatrixFrame matrixFrame = ((side == BattleSideEnum.Defender) ? this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[machineSlotIndex].GetGlobalFrame() : this._attackerRangedEngineSpawnEntities[machineSlotIndex].GetGlobalFrame());
			matrixFrame.rotation.MakeUnit();
			bombardmentData.ShooterGlobalFrame = matrixFrame;
			string siegeEngineMapFireAnimationName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapFireAnimationName(machineType, side);
			string siegeEngineMapReloadAnimationName = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineMapReloadAnimationName(machineType, side);
			bombardmentData.ReloadDuration = MBAnimation.GetAnimationDuration(siegeEngineMapReloadAnimationName) * 0.25f;
			bombardmentData.AimingDuration = 0.25f;
			bombardmentData.RotationDuration = 0.4f;
			bombardmentData.FireDuration = MBAnimation.GetAnimationDuration(siegeEngineMapFireAnimationName) * 0.25f;
			float animationParameter = MBAnimation.GetAnimationParameter1(siegeEngineMapFireAnimationName);
			bombardmentData.MissileLaunchDuration = bombardmentData.FireDuration * animationParameter;
			bombardmentData.MissileSpeed = 14f;
			bombardmentData.Gravity = ((machineType == DefaultSiegeEngineTypes.Ballista || machineType == DefaultSiegeEngineTypes.FireBallista) ? 10f : 40f);
			if (targetType == SiegeBombardTargets.RangedEngines)
			{
				bombardmentData.TargetPosition = ((side == BattleSideEnum.Attacker) ? this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[targetSlotIndex].GetGlobalFrame().origin : this._attackerRangedEngineSpawnEntities[targetSlotIndex].GetGlobalFrame().origin);
			}
			else if (targetType == SiegeBombardTargets.Wall)
			{
				bombardmentData.TargetPosition = this._defenderBreachableWallEntitiesCacheForCurrentLevel[targetSlotIndex].GlobalPosition;
			}
			else if (targetSlotIndex == -1)
			{
				bombardmentData.TargetPosition = Vec3.Zero;
			}
			else
			{
				bombardmentData.TargetPosition = ((side == BattleSideEnum.Attacker) ? this._defenderRangedEngineSpawnEntitiesCacheForCurrentLevel[targetSlotIndex].GetGlobalFrame().origin : this._attackerRangedEngineSpawnEntities[targetSlotIndex].GetGlobalFrame().origin);
				bombardmentData.TargetPosition += (bombardmentData.TargetPosition - bombardmentData.ShooterGlobalFrame.origin).NormalizedCopy() * 2f;
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				CampaignVec2 campaignVec = new CampaignVec2(bombardmentData.TargetPosition.AsVec2, true);
				mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref bombardmentData.TargetPosition.z);
			}
			bombardmentData.TargetAlignedShooterGlobalFrame = bombardmentData.ShooterGlobalFrame;
			bombardmentData.TargetAlignedShooterGlobalFrame.rotation.f.AsVec2 = (bombardmentData.TargetPosition - bombardmentData.ShooterGlobalFrame.origin).AsVec2;
			bombardmentData.TargetAlignedShooterGlobalFrame.rotation.f.NormalizeWithoutChangingZ();
			bombardmentData.TargetAlignedShooterGlobalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			bombardmentData.LaunchGlobalPosition = bombardmentData.TargetAlignedShooterGlobalFrame.TransformToParent(in base.MapScreen.PrefabEntityCache.GetLaunchEntitialFrameForSiegeEngine(machineType, side).origin);
			float lengthSquared = (bombardmentData.LaunchGlobalPosition.AsVec2 - bombardmentData.TargetPosition.AsVec2).LengthSquared;
			float num = MathF.Sqrt(lengthSquared);
			float num2 = bombardmentData.LaunchGlobalPosition.z - bombardmentData.TargetPosition.z;
			float num3 = bombardmentData.MissileSpeed * bombardmentData.MissileSpeed;
			float num4 = num3 * num3;
			float num5 = num4 - bombardmentData.Gravity * (bombardmentData.Gravity * lengthSquared - 2f * num2 * num3);
			if (num5 >= 0f)
			{
				bombardmentData.LaunchAngle = MathF.Atan((num3 - MathF.Sqrt(num5)) / (bombardmentData.Gravity * num));
			}
			else
			{
				bombardmentData.Gravity = 1f;
				num5 = num4 - bombardmentData.Gravity * (bombardmentData.Gravity * lengthSquared - 2f * num2 * num3);
				bombardmentData.LaunchAngle = MathF.Atan((num3 - MathF.Sqrt(num5)) / (bombardmentData.Gravity * num));
			}
			float num6 = bombardmentData.MissileSpeed * MathF.Cos(bombardmentData.LaunchAngle);
			bombardmentData.FlightDuration = num / num6;
			bombardmentData.TotalDuration = bombardmentData.RotationDuration + bombardmentData.ReloadDuration + bombardmentData.AimingDuration + bombardmentData.MissileLaunchDuration + bombardmentData.FlightDuration;
		}

		// Token: 0x0400021A RID: 538
		private const string CircleTag = "map_settlement_circle";

		// Token: 0x0400021B RID: 539
		private const string BannerPlaceHolderTag = "map_banner_placeholder";

		// Token: 0x0400021C RID: 540
		private const string MapSiegeEngineTag = "map_siege_engine";

		// Token: 0x0400021D RID: 541
		private const string MapBreachableWallTag = "map_breachable_wall";

		// Token: 0x0400021E RID: 542
		private const string MapDefenderEngineTag = "map_defensive_engine";

		// Token: 0x0400021F RID: 543
		private const string MapSiegeEngineRamTag = "map_siege_ram";

		// Token: 0x04000220 RID: 544
		private const string TownPhysicalTag = "bo_town";

		// Token: 0x04000221 RID: 545
		private const string MapSiegeEngineTowerTag = "map_siege_tower";

		// Token: 0x04000222 RID: 546
		private const string MapPreparationTag = "siege_preparation";

		// Token: 0x04000223 RID: 547
		private const string BurnedTag = "looted";

		// Token: 0x04000224 RID: 548
		private GameEntity[] _attackerRangedEngineSpawnEntities;

		// Token: 0x04000225 RID: 549
		private GameEntity[] _attackerBatteringRamSpawnEntities;

		// Token: 0x04000226 RID: 550
		private GameEntity[] _defenderBreachableWallEntitiesCacheForCurrentLevel;

		// Token: 0x04000227 RID: 551
		private GameEntity[] _attackerSiegeTowerSpawnEntities;

		// Token: 0x04000228 RID: 552
		private GameEntity[] _defenderRangedEngineSpawnEntitiesForAllLevels;

		// Token: 0x04000229 RID: 553
		private GameEntity[] _defenderRangedEngineSpawnEntitiesCacheForCurrentLevel;

		// Token: 0x0400022A RID: 554
		private GameEntity[] _defenderBreachableWallEntitiesForAllLevels;

		// Token: 0x0400022C RID: 556
		private readonly List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>> _siegeRangedMachineEntities;

		// Token: 0x0400022D RID: 557
		private readonly List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame, GameEntity>> _siegeMeleeMachineEntities;

		// Token: 0x0400022E RID: 558
		private readonly List<ValueTuple<GameEntity, BattleSideEnum, int>> _siegeMissileEntities;

		// Token: 0x0400022F RID: 559
		private Dictionary<int, List<GameEntity>> _gateBannerEntitiesWithLevels;

		// Token: 0x04000230 RID: 560
		private uint _currentLevelMask;

		// Token: 0x04000231 RID: 561
		private MatrixFrame _hoveredSiegeEntityFrame = MatrixFrame.Identity;

		// Token: 0x04000232 RID: 562
		private GameEntity.UpgradeLevelMask _currentSettlementUpgradeLevelMask;

		// Token: 0x04000233 RID: 563
		private Scene _mapScene;

		// Token: 0x020000BD RID: 189
		private struct SiegeBombardmentData
		{
			// Token: 0x040003B4 RID: 948
			public Vec3 LaunchGlobalPosition;

			// Token: 0x040003B5 RID: 949
			public Vec3 TargetPosition;

			// Token: 0x040003B6 RID: 950
			public MatrixFrame ShooterGlobalFrame;

			// Token: 0x040003B7 RID: 951
			public MatrixFrame TargetAlignedShooterGlobalFrame;

			// Token: 0x040003B8 RID: 952
			public float MissileSpeed;

			// Token: 0x040003B9 RID: 953
			public float Gravity;

			// Token: 0x040003BA RID: 954
			public float LaunchAngle;

			// Token: 0x040003BB RID: 955
			public float RotationDuration;

			// Token: 0x040003BC RID: 956
			public float ReloadDuration;

			// Token: 0x040003BD RID: 957
			public float AimingDuration;

			// Token: 0x040003BE RID: 958
			public float MissileLaunchDuration;

			// Token: 0x040003BF RID: 959
			public float FireDuration;

			// Token: 0x040003C0 RID: 960
			public float FlightDuration;

			// Token: 0x040003C1 RID: 961
			public float TotalDuration;
		}
	}
}
