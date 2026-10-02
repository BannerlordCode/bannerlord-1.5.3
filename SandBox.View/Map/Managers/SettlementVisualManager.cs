using System;
using System.Collections.Generic;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Managers
{
	// Token: 0x0200007A RID: 122
	public class SettlementVisualManager : EntityVisualManagerBase<PartyBase>
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x000280B7 File Offset: 0x000262B7
		public override int Priority
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000280BB File Offset: 0x000262BB
		public static SettlementVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<SettlementVisualManager>();
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000280C8 File Offset: 0x000262C8
		public override void OnTick(float realDt, float dt)
		{
			this._dirtyPartyVisualCount = -1;
			TWParallel.For(0, this._visualsFlattened.Count, delegate(int startInclusive, int endExclusive)
			{
				for (int j = startInclusive; j < endExclusive; j++)
				{
					this._visualsFlattened[j].Tick(dt, ref this._dirtyPartyVisualCount, ref this._dirtyPartiesList);
				}
			}, 16);
			for (int i = 0; i < this._dirtyPartyVisualCount + 1; i++)
			{
				this._dirtyPartiesList[i].ValidateIsDirty();
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00028130 File Offset: 0x00026330
		public override bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			bool flag = false;
			for (int i = entityCount - 1; i >= 0; i--)
			{
				UIntPtr uintPtr = intersectedEntityIDs[i];
				if (uintPtr != UIntPtr.Zero)
				{
					MapEntityVisual mapEntityVisual;
					if (MapScreen.VisualsOfEntities.TryGetValue(uintPtr, out mapEntityVisual) && mapEntityVisual is SettlementVisual && mapEntityVisual.IsVisibleOrFadingOut())
					{
						if (hoveredVisual == null)
						{
							hoveredVisual = mapEntityVisual;
						}
						selectedVisual = mapEntityVisual;
					}
					if (PlayerSiege.PlayerSiegeEvent != null && ScreenManager.FirstHitLayer == MapScreen.Instance.SceneLayer && MapScreen.FrameAndVisualOfEngines.ContainsKey(uintPtr))
					{
						flag = true;
						this.HandleSiegeEngineHover(uintPtr);
					}
				}
			}
			if (!flag)
			{
				this.HandleSiegeEngineHoverEnd();
			}
			return selectedVisual != null;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000281C8 File Offset: 0x000263C8
		public override void OnFrameTick(float dt)
		{
			this.RefreshMapSiegeOverlayRequired();
			if (PlayerSiege.PlayerSiegeEvent != null && this._playerSiegeMachineSlotMeshesAdded)
			{
				this.TickSiegeMachineCircles();
			}
			if (GameStateManager.Current.ActiveStateDisabledByUser)
			{
				this.HandleSiegeEngineHoverEnd();
			}
			this._timeSinceCreation += dt;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00028208 File Offset: 0x00026408
		public override bool OnMouseClick(MapEntityVisual visualOfSelectedEntity, Vec3 intersectionPoint, PathFaceRecord mouseOverFaceIndex, bool isDoubleClick)
		{
			bool flag = false;
			if (MapScreen.Instance.MapState.AtMenu && this._hoveredSiegeEntityID != UIntPtr.Zero)
			{
				Tuple<MatrixFrame, SettlementVisual> tuple = MapScreen.FrameAndVisualOfEngines[this._hoveredSiegeEntityID];
				MapScreen.Instance.OnSiegeEngineFrameClick(tuple.Item1);
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00028260 File Offset: 0x00026460
		public override MapEntityVisual<PartyBase> GetVisualOfEntity(PartyBase partyBase)
		{
			SettlementVisual settlementVisual;
			this._settlementVisuals.TryGetValue(partyBase, out settlementVisual);
			return settlementVisual;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0002827D File Offset: 0x0002647D
		public SettlementVisual GetSettlementVisual(Settlement settlement)
		{
			return this._settlementVisuals[settlement.Party];
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00028290 File Offset: 0x00026490
		protected override void OnInitialize()
		{
			base.OnInitialize();
			foreach (Settlement settlement in Settlement.All)
			{
				this.AddNewPartyVisualForParty(settlement.Party);
			}
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000282F8 File Offset: 0x000264F8
		protected override void OnFinalize()
		{
			foreach (SettlementVisual settlementVisual in this._settlementVisuals.Values)
			{
				settlementVisual.ReleaseResources();
			}
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00028358 File Offset: 0x00026558
		private void TickSiegeMachineCircles()
		{
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			bool flag = playerSiegeEvent != null && playerSiegeEvent.IsPlayerSiegeEvent && Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(playerSiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero;
			Settlement besiegedSettlement = playerSiegeEvent.BesiegedSettlement;
			SettlementVisual settlementVisual = this.GetSettlementVisual(besiegedSettlement);
			Tuple<MatrixFrame, SettlementVisual> tuple = null;
			if (this._hoveredSiegeEntityID != UIntPtr.Zero)
			{
				tuple = MapScreen.FrameAndVisualOfEngines[this._hoveredSiegeEntityID];
			}
			for (int i = 0; i < settlementVisual.GetDefenderRangedSiegeEngineFrames().Length; i++)
			{
				bool flag2 = playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).SiegeEngines.DeployedRangedSiegeEngines[i] == null;
				bool flag3 = PlayerSiege.PlayerSide > BattleSideEnum.Defender;
				string desiredMaterialName = this.GetDesiredMaterialName(true, false, false);
				Decal decal = this._defenderMachinesCircleEntities[i].GetComponentAtIndex(0, GameEntity.ComponentType.Decal) as Decal;
				Material material = decal.GetMaterial();
				if (((material != null) ? material.Name : null) != desiredMaterialName)
				{
					decal.SetMaterial(Material.GetFromResource(desiredMaterialName));
				}
				bool flag4 = tuple != null && this._defenderMachinesCircleEntities[i].GetGlobalFrame().NearlyEquals(tuple.Item1, 1E-05f);
				uint desiredDecalColor = this.GetDesiredDecalColor(flag4, flag3, flag2, flag);
				if (desiredDecalColor != decal.GetFactor1())
				{
					decal.SetFactor1(desiredDecalColor);
				}
			}
			for (int j = 0; j < settlementVisual.GetAttackerRangedSiegeEngineFrames().Length; j++)
			{
				bool flag5 = playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedRangedSiegeEngines[j] == null;
				bool flag6 = PlayerSiege.PlayerSide != BattleSideEnum.Attacker;
				string desiredMaterialName2 = this.GetDesiredMaterialName(true, true, false);
				Decal decal2 = this._attackerRangedMachinesCircleEntities[j].GetComponentAtIndex(0, GameEntity.ComponentType.Decal) as Decal;
				Material material2 = decal2.GetMaterial();
				if (((material2 != null) ? material2.Name : null) != desiredMaterialName2)
				{
					decal2.SetMaterial(Material.GetFromResource(desiredMaterialName2));
				}
				bool flag7 = tuple != null && this._attackerRangedMachinesCircleEntities[j].GetGlobalFrame().NearlyEquals(tuple.Item1, 1E-05f);
				uint desiredDecalColor2 = this.GetDesiredDecalColor(flag7, flag6, flag5, flag);
				if (desiredDecalColor2 != decal2.GetFactor1())
				{
					decal2.SetFactor1(desiredDecalColor2);
				}
			}
			for (int k = 0; k < settlementVisual.GetAttackerBatteringRamSiegeEngineFrames().Length; k++)
			{
				bool flag8 = playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedMeleeSiegeEngines[k] == null;
				bool flag9 = PlayerSiege.PlayerSide != BattleSideEnum.Attacker;
				string desiredMaterialName3 = this.GetDesiredMaterialName(false, true, false);
				Decal decal3 = this._attackerRamMachinesCircleEntities[k].GetComponentAtIndex(0, GameEntity.ComponentType.Decal) as Decal;
				Material material3 = decal3.GetMaterial();
				if (((material3 != null) ? material3.Name : null) != desiredMaterialName3)
				{
					decal3.SetMaterial(Material.GetFromResource(desiredMaterialName3));
				}
				bool flag10 = tuple != null && this._attackerRamMachinesCircleEntities[k].GetGlobalFrame().NearlyEquals(tuple.Item1, 1E-05f);
				uint desiredDecalColor3 = this.GetDesiredDecalColor(flag10, flag9, flag8, flag);
				if (desiredDecalColor3 != decal3.GetFactor1())
				{
					decal3.SetFactor1(desiredDecalColor3);
				}
			}
			for (int l = 0; l < settlementVisual.GetAttackerTowerSiegeEngineFrames().Length; l++)
			{
				bool flag11 = playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedMeleeSiegeEngines[settlementVisual.GetAttackerBatteringRamSiegeEngineFrames().Length + l] == null;
				bool flag12 = PlayerSiege.PlayerSide != BattleSideEnum.Attacker;
				string desiredMaterialName4 = this.GetDesiredMaterialName(false, true, true);
				Decal decal4 = this._attackerTowerMachinesCircleEntities[l].GetComponentAtIndex(0, GameEntity.ComponentType.Decal) as Decal;
				Material material4 = decal4.GetMaterial();
				if (((material4 != null) ? material4.Name : null) != desiredMaterialName4)
				{
					decal4.SetMaterial(Material.GetFromResource(desiredMaterialName4));
				}
				bool flag13 = tuple != null && this._attackerTowerMachinesCircleEntities[l].GetGlobalFrame().NearlyEquals(tuple.Item1, 1E-05f);
				uint desiredDecalColor4 = this.GetDesiredDecalColor(flag13, flag12, flag11, flag);
				if (desiredDecalColor4 != decal4.GetFactor1())
				{
					decal4.SetFactor1(desiredDecalColor4);
				}
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00028750 File Offset: 0x00026950
		private void AddNewPartyVisualForParty(PartyBase partyBase)
		{
			SettlementVisual settlementVisual = new SettlementVisual(partyBase);
			settlementVisual.OnStartup();
			this._settlementVisuals.Add(partyBase, settlementVisual);
			this._visualsFlattened.Add(settlementVisual);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00028784 File Offset: 0x00026984
		private uint GetDesiredDecalColor(bool isHovered, bool isEnemy, bool isEmpty, bool isPlayerLeader)
		{
			if (isEnemy)
			{
				return 4287064638U;
			}
			if (isHovered && isPlayerLeader)
			{
				return 4293956364U;
			}
			if (!isEmpty)
			{
				return 4283683126U;
			}
			if (isPlayerLeader)
			{
				float num = MathF.PingPong(0f, 0.5f, this._timeSinceCreation) / 0.5f;
				Color color = Color.FromUint(4278394186U);
				Color color2 = Color.FromUint(4284320212U);
				return Color.Lerp(color, color2, num).ToUnsignedInteger();
			}
			return 4278394186U;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000287F9 File Offset: 0x000269F9
		private string GetDesiredMaterialName(bool isRanged, bool isAttacker, bool isTower)
		{
			if (isRanged)
			{
				if (!isAttacker)
				{
					return "decal_defender_ranged_siege";
				}
				return "decal_siege_ranged";
			}
			else
			{
				if (!isTower)
				{
					return "decal_siege_ram";
				}
				return "decal_siege_tower";
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0002881C File Offset: 0x00026A1C
		private void RemoveSiegeCircleVisuals()
		{
			if (this._playerSiegeMachineSlotMeshesAdded)
			{
				MapScene mapScene = Campaign.Current.MapSceneWrapper as MapScene;
				for (int i = 0; i < this._defenderMachinesCircleEntities.Length; i++)
				{
					this._defenderMachinesCircleEntities[i].SetVisibilityExcludeParents(false);
					mapScene.Scene.RemoveEntity(this._defenderMachinesCircleEntities[i], 107);
					this._defenderMachinesCircleEntities[i] = null;
				}
				for (int j = 0; j < this._attackerRamMachinesCircleEntities.Length; j++)
				{
					this._attackerRamMachinesCircleEntities[j].SetVisibilityExcludeParents(false);
					mapScene.Scene.RemoveEntity(this._attackerRamMachinesCircleEntities[j], 108);
					this._attackerRamMachinesCircleEntities[j] = null;
				}
				for (int k = 0; k < this._attackerTowerMachinesCircleEntities.Length; k++)
				{
					this._attackerTowerMachinesCircleEntities[k].SetVisibilityExcludeParents(false);
					mapScene.Scene.RemoveEntity(this._attackerTowerMachinesCircleEntities[k], 109);
					this._attackerTowerMachinesCircleEntities[k] = null;
				}
				for (int l = 0; l < this._attackerRangedMachinesCircleEntities.Length; l++)
				{
					this._attackerRangedMachinesCircleEntities[l].SetVisibilityExcludeParents(false);
					mapScene.Scene.RemoveEntity(this._attackerRangedMachinesCircleEntities[l], 110);
					this._attackerRangedMachinesCircleEntities[l] = null;
				}
				this._playerSiegeMachineSlotMeshesAdded = false;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00028950 File Offset: 0x00026B50
		private void RefreshMapSiegeOverlayRequired()
		{
			MapScreen.Instance.MapCameraView.OnRefreshMapSiegeOverlayRequired(this._mapSiegeOverlayView == null);
			if (this._playerSiegeMachineSlotMeshesAdded && PlayerSiege.PlayerSiegeEvent != null)
			{
				Settlement besiegedSettlement = PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				if (besiegedSettlement != null && besiegedSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall)
				{
					this.RemoveSiegeCircleVisuals();
					this._playerSiegeMachineSlotMeshesAdded = false;
					return;
				}
			}
			if (PlayerSiege.PlayerSiegeEvent == null && this._mapSiegeOverlayView != null)
			{
				MapScreen.Instance.RemoveMapView(this._mapSiegeOverlayView);
				this._mapSiegeOverlayView = null;
				if (this._playerSiegeMachineSlotMeshesAdded)
				{
					this.RemoveSiegeCircleVisuals();
					this._playerSiegeMachineSlotMeshesAdded = false;
					return;
				}
			}
			else if (PlayerSiege.PlayerSiegeEvent != null && this._mapSiegeOverlayView == null)
			{
				this._mapSiegeOverlayView = MapScreen.Instance.AddMapView<MapSiegeOverlayView>(Array.Empty<object>());
				if (!this._playerSiegeMachineSlotMeshesAdded)
				{
					this.InitializeSiegeCircleVisuals();
					this._playerSiegeMachineSlotMeshesAdded = true;
				}
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00028A24 File Offset: 0x00026C24
		private void InitializeSiegeCircleVisuals()
		{
			Settlement besiegedSettlement = PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
			SettlementVisual settlementVisual = this.GetSettlementVisual(besiegedSettlement);
			MapScene mapScene = Campaign.Current.MapSceneWrapper as MapScene;
			MatrixFrame[] array = settlementVisual.GetDefenderRangedSiegeEngineFrames();
			this._defenderMachinesCircleEntities = new GameEntity[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				MatrixFrame matrixFrame = array[i];
				this._defenderMachinesCircleEntities[i] = GameEntity.CreateEmpty(mapScene.Scene, true, true, true);
				this._defenderMachinesCircleEntities[i].Name = "dRangedMachineCircle_" + i;
				Decal decal = Decal.CreateDecal(null);
				decal.SetMaterial(Material.GetFromResource("decal_defender_ranged_siege"));
				decal.SetFactor1Linear(4287064638U);
				this._defenderMachinesCircleEntities[i].AddComponent(decal);
				MatrixFrame matrixFrame2 = matrixFrame;
				if (this._isNewDecalScaleImplementationEnabled)
				{
					Vec3 vec = new Vec3(0.25f, 0.25f, 0.25f, -1f);
					matrixFrame2.Scale(in vec);
				}
				this._defenderMachinesCircleEntities[i].SetGlobalFrame(in matrixFrame2, true);
				this._defenderMachinesCircleEntities[i].SetVisibilityExcludeParents(true);
				mapScene.Scene.AddDecalInstance(decal, "editor_set", true);
			}
			array = settlementVisual.GetAttackerBatteringRamSiegeEngineFrames();
			this._attackerRamMachinesCircleEntities = new GameEntity[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				MatrixFrame matrixFrame3 = array[j];
				this._attackerRamMachinesCircleEntities[j] = GameEntity.CreateEmpty(mapScene.Scene, true, true, true);
				this._attackerRamMachinesCircleEntities[j].Name = "InitializeSiegeCircleVisuals";
				this._attackerRamMachinesCircleEntities[j].Name = "aRamMachineCircle_" + j;
				Decal decal2 = Decal.CreateDecal(null);
				decal2.SetMaterial(Material.GetFromResource("decal_siege_ram"));
				decal2.SetFactor1Linear(4287064638U);
				this._attackerRamMachinesCircleEntities[j].AddComponent(decal2);
				MatrixFrame matrixFrame4 = matrixFrame3;
				if (this._isNewDecalScaleImplementationEnabled)
				{
					Vec3 vec = new Vec3(0.38f, 0.38f, 0.38f, -1f);
					matrixFrame4.Scale(in vec);
				}
				this._attackerRamMachinesCircleEntities[j].SetGlobalFrame(in matrixFrame4, true);
				this._attackerRamMachinesCircleEntities[j].SetVisibilityExcludeParents(true);
				mapScene.Scene.AddDecalInstance(decal2, "editor_set", true);
			}
			array = settlementVisual.GetAttackerTowerSiegeEngineFrames();
			this._attackerTowerMachinesCircleEntities = new GameEntity[array.Length];
			for (int k = 0; k < array.Length; k++)
			{
				MatrixFrame matrixFrame5 = array[k];
				this._attackerTowerMachinesCircleEntities[k] = GameEntity.CreateEmpty(mapScene.Scene, true, true, true);
				this._attackerTowerMachinesCircleEntities[k].Name = "aTowerMachineCircle_" + k;
				Decal decal3 = Decal.CreateDecal(null);
				decal3.SetMaterial(Material.GetFromResource("decal_siege_tower"));
				decal3.SetFactor1Linear(4287064638U);
				this._attackerTowerMachinesCircleEntities[k].AddComponent(decal3);
				MatrixFrame matrixFrame6 = matrixFrame5;
				if (this._isNewDecalScaleImplementationEnabled)
				{
					Vec3 vec = new Vec3(0.38f, 0.38f, 0.38f, -1f);
					matrixFrame6.Scale(in vec);
				}
				this._attackerTowerMachinesCircleEntities[k].SetGlobalFrame(in matrixFrame6, true);
				this._attackerTowerMachinesCircleEntities[k].SetVisibilityExcludeParents(true);
				mapScene.Scene.AddDecalInstance(decal3, "editor_set", true);
			}
			array = settlementVisual.GetAttackerRangedSiegeEngineFrames();
			this._attackerRangedMachinesCircleEntities = new GameEntity[array.Length];
			for (int l = 0; l < array.Length; l++)
			{
				MatrixFrame matrixFrame7 = array[l];
				this._attackerRangedMachinesCircleEntities[l] = GameEntity.CreateEmpty(mapScene.Scene, true, true, true);
				this._attackerRangedMachinesCircleEntities[l].Name = "aRangedMachineCircle_" + l;
				Decal decal4 = Decal.CreateDecal(null);
				decal4.SetMaterial(Material.GetFromResource("decal_siege_ranged"));
				decal4.SetFactor1Linear(4287064638U);
				this._attackerRangedMachinesCircleEntities[l].AddComponent(decal4);
				MatrixFrame matrixFrame8 = matrixFrame7;
				if (this._isNewDecalScaleImplementationEnabled)
				{
					Vec3 vec = new Vec3(0.38f, 0.38f, 0.38f, -1f);
					matrixFrame8.Scale(in vec);
				}
				this._attackerRangedMachinesCircleEntities[l].SetGlobalFrame(in matrixFrame8, true);
				this._attackerRangedMachinesCircleEntities[l].SetVisibilityExcludeParents(true);
				mapScene.Scene.AddDecalInstance(decal4, "editor_set", true);
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00028E6C File Offset: 0x0002706C
		private void HandleSiegeEngineHover(UIntPtr newID)
		{
			if (this._hoveredSiegeEntityID != newID)
			{
				this._hoveredSiegeEntityID = newID;
				Tuple<MatrixFrame, SettlementVisual> tuple = MapScreen.FrameAndVisualOfEngines[this._hoveredSiegeEntityID];
				tuple.Item2.OnMapHoverSiegeEngine(tuple.Item1);
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00028EB0 File Offset: 0x000270B0
		private void HandleSiegeEngineHoverEnd()
		{
			if (this._hoveredSiegeEntityID != UIntPtr.Zero)
			{
				MapScreen.FrameAndVisualOfEngines[this._hoveredSiegeEntityID].Item2.OnMapHoverSiegeEngineEnd();
				this._hoveredSiegeEntityID = UIntPtr.Zero;
			}
		}

		// Token: 0x0400026B RID: 619
		private const string _emptyAttackerRangedDecalMaterialName = "decal_siege_ranged";

		// Token: 0x0400026C RID: 620
		private const string _attackerRamMachineDecalMaterialName = "decal_siege_ram";

		// Token: 0x0400026D RID: 621
		private const string _attackerTowerMachineDecalMaterialName = "decal_siege_tower";

		// Token: 0x0400026E RID: 622
		private const string _attackerRangedMachineDecalMaterialName = "decal_siege_ranged";

		// Token: 0x0400026F RID: 623
		private const string _defenderRangedMachineDecalMaterialName = "decal_defender_ranged_siege";

		// Token: 0x04000270 RID: 624
		private const uint _preperationOrEnemySiegeEngineDecalColor = 4287064638U;

		// Token: 0x04000271 RID: 625
		private const uint _normalStartSiegeEngineDecalColor = 4278394186U;

		// Token: 0x04000272 RID: 626
		private const float _defenderMachineCircleDecalScale = 0.25f;

		// Token: 0x04000273 RID: 627
		private const float _attackerMachineDecalScale = 0.38f;

		// Token: 0x04000274 RID: 628
		private bool _isNewDecalScaleImplementationEnabled;

		// Token: 0x04000275 RID: 629
		private const uint _normalEndSiegeEngineDecalColor = 4284320212U;

		// Token: 0x04000276 RID: 630
		private const uint _hoveredSiegeEngineDecalColor = 4293956364U;

		// Token: 0x04000277 RID: 631
		private const uint _withMachineSiegeEngineDecalColor = 4283683126U;

		// Token: 0x04000278 RID: 632
		private const float _machineDecalAnimLoopTime = 0.5f;

		// Token: 0x04000279 RID: 633
		private readonly Dictionary<PartyBase, SettlementVisual> _settlementVisuals = new Dictionary<PartyBase, SettlementVisual>();

		// Token: 0x0400027A RID: 634
		private readonly List<SettlementVisual> _visualsFlattened = new List<SettlementVisual>();

		// Token: 0x0400027B RID: 635
		private int _dirtyPartyVisualCount;

		// Token: 0x0400027C RID: 636
		private SettlementVisual[] _dirtyPartiesList = new SettlementVisual[2500];

		// Token: 0x0400027D RID: 637
		private UIntPtr _hoveredSiegeEntityID;

		// Token: 0x0400027E RID: 638
		private bool _playerSiegeMachineSlotMeshesAdded;

		// Token: 0x0400027F RID: 639
		private MapView _mapSiegeOverlayView;

		// Token: 0x04000280 RID: 640
		private GameEntity[] _defenderMachinesCircleEntities;

		// Token: 0x04000281 RID: 641
		private GameEntity[] _attackerRamMachinesCircleEntities;

		// Token: 0x04000282 RID: 642
		private GameEntity[] _attackerTowerMachinesCircleEntities;

		// Token: 0x04000283 RID: 643
		private GameEntity[] _attackerRangedMachinesCircleEntities;

		// Token: 0x04000284 RID: 644
		private float _timeSinceCreation;
	}
}
