using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000052 RID: 82
	public class MapSiegePOIVM : ViewModel
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x000138AC File Offset: 0x00011AAC
		private SiegeEvent Siege
		{
			get
			{
				return PlayerSiege.PlayerSiegeEvent;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x000138B3 File Offset: 0x00011AB3
		private BattleSideEnum PlayerSide
		{
			get
			{
				return PlayerSiege.PlayerSide;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x000138BA File Offset: 0x00011ABA
		private Settlement Settlement
		{
			get
			{
				return this.Siege.BesiegedSettlement;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x000138C7 File Offset: 0x00011AC7
		public MapSiegePOIVM.POIType Type { get; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x000138CF File Offset: 0x00011ACF
		public int MachineIndex { get; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x000138D7 File Offset: 0x00011AD7
		public float LatestW
		{
			get
			{
				return this._latestW;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x000138DF File Offset: 0x00011ADF
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x000138E7 File Offset: 0x00011AE7
		public SiegeEvent.SiegeEngineConstructionProgress Machine { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000138F0 File Offset: 0x00011AF0
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x000138F8 File Offset: 0x00011AF8
		public MatrixFrame MapSceneLocationFrame { get; private set; }

		// Token: 0x06000519 RID: 1305 RVA: 0x00013904 File Offset: 0x00011B04
		public MapSiegePOIVM(MapSiegePOIVM.POIType type, MatrixFrame mapSceneLocation, Camera mapCamera, int machineIndex, Action<MapSiegePOIVM> onSelection)
		{
			this.Type = type;
			this._onSelection = onSelection;
			this._thisSide = ((this.Type == MapSiegePOIVM.POIType.AttackerRamSiegeMachine || this.Type == MapSiegePOIVM.POIType.AttackerTowerSiegeMachine || this.Type == MapSiegePOIVM.POIType.AttackerRangedSiegeMachine) ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			this.MapSceneLocationFrame = mapSceneLocation;
			this._mapSceneLocation = this.MapSceneLocationFrame.origin;
			this._mapCamera = mapCamera;
			this.MachineIndex = machineIndex;
			Color color;
			if (this._thisSide != BattleSideEnum.Attacker)
			{
				IFaction mapFaction = this.Siege.BesiegedSettlement.MapFaction;
				color = Color.FromUint((mapFaction != null) ? mapFaction.Color : 0U);
			}
			else
			{
				IFaction mapFaction2 = this.Siege.BesiegerCamp.MapFaction;
				color = Color.FromUint((mapFaction2 != null) ? mapFaction2.Color : 0U);
			}
			this.SidePrimaryColor = color;
			Color color2;
			if (this._thisSide != BattleSideEnum.Attacker)
			{
				IFaction mapFaction3 = this.Siege.BesiegedSettlement.MapFaction;
				color2 = Color.FromUint((mapFaction3 != null) ? mapFaction3.Color2 : 0U);
			}
			else
			{
				IFaction mapFaction4 = this.Siege.BesiegerCamp.MapFaction;
				color2 = Color.FromUint((mapFaction4 != null) ? mapFaction4.Color2 : 0U);
			}
			this.SideSecondaryColor = color2;
			this.IsPlayerSidePOI = this.DetermineIfPOIIsPlayerSide();
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00013A32 File Offset: 0x00011C32
		public void ExecuteSelection()
		{
			this._onSelection(this);
			this.IsSelected = true;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00013A48 File Offset: 0x00011C48
		public void UpdateProperties()
		{
			this.Machine = this.GetDesiredMachine();
			this._bindHasItem = this.Type == MapSiegePOIVM.POIType.WallSection || this.Machine != null;
			SiegeEvent.SiegeEngineConstructionProgress machine = this.Machine;
			this._bindIsConstructing = machine != null && !machine.IsActive;
			this.RefreshMachineType();
			this.RefreshHitpoints();
			this.RefreshQueueIndex();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00013AA8 File Offset: 0x00011CA8
		public void RefreshDistanceValue(float newDistance)
		{
			this._bindIsInVisibleRange = newDistance <= 20f;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00013ABC File Offset: 0x00011CBC
		public void RefreshPosition()
		{
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, this._mapSceneLocation, ref this._latestX, ref this._latestY, ref this._latestW);
			if (!MathF.IsValidValue(this._latestX) || !MathF.IsValidValue(this._latestY) || !MathF.IsValidValue(this._latestW))
			{
				this._latestX = -10000f;
				this._latestY = -10000f;
				this._latestW = -1f;
			}
			this._bindWPos = this._latestW;
			this._bindWSign = (int)this._bindWPos;
			this._bindIsInside = this.IsInsideWindow();
			this._bindPosition = new Vec2(this._latestX, this._latestY);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00013B94 File Offset: 0x00011D94
		public void RefreshBinding()
		{
			this.Position = this._bindPosition;
			this.IsInside = this._bindIsInside;
			this.CurrentHitpoints = this._bindCurrentHitpoints;
			this.MaxHitpoints = this._bindMaxHitpoints;
			this.HasItem = this._bindHasItem;
			this.IsConstructing = this._bindIsConstructing;
			this.MachineType = this._bindMachineType;
			this.QueueIndex = this._bindQueueIndex;
			this.IsInVisibleRange = this._bindIsInVisibleRange;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00013C10 File Offset: 0x00011E10
		private void RefreshHitpoints()
		{
			if (this.Siege == null)
			{
				this._bindCurrentHitpoints = 0f;
				this._bindMaxHitpoints = 0f;
				return;
			}
			MapSiegePOIVM.POIType type = this.Type;
			if (type == MapSiegePOIVM.POIType.WallSection)
			{
				MBReadOnlyList<float> settlementWallSectionHitPointsRatioList = this.Settlement.SettlementWallSectionHitPointsRatioList;
				this._bindMaxHitpoints = this.Settlement.MaxWallHitPoints / (float)this.Settlement.WallSectionCount;
				this._bindCurrentHitpoints = settlementWallSectionHitPointsRatioList[this.MachineIndex] * this._bindMaxHitpoints;
				this._bindMachineType = ((this._bindCurrentHitpoints <= 0f) ? 1 : 0);
				return;
			}
			if (type - MapSiegePOIVM.POIType.DefenderSiegeMachine > 3)
			{
				return;
			}
			if (this.Machine == null)
			{
				this._bindCurrentHitpoints = 0f;
				this._bindMaxHitpoints = 0f;
				return;
			}
			if (this.Machine.IsActive)
			{
				this._bindCurrentHitpoints = this.Machine.Hitpoints;
				this._bindMaxHitpoints = this.Machine.MaxHitPoints;
				return;
			}
			if (this.Machine.IsBeingRedeployed)
			{
				this._bindCurrentHitpoints = this.Machine.RedeploymentProgress;
				this._bindMaxHitpoints = 1f;
				return;
			}
			this._bindCurrentHitpoints = this.Machine.Progress;
			this._bindMaxHitpoints = 1f;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00013D40 File Offset: 0x00011F40
		private void RefreshMachineType()
		{
			if (this.Siege == null)
			{
				this._bindMachineType = -1;
				return;
			}
			MapSiegePOIVM.POIType type = this.Type;
			if (type == MapSiegePOIVM.POIType.WallSection)
			{
				this._bindMachineType = 0;
				return;
			}
			if (type - MapSiegePOIVM.POIType.DefenderSiegeMachine > 3)
			{
				return;
			}
			this._bindMachineType = (int)((this.Machine != null) ? this.GetMachineTypeFromId(this.Machine.SiegeEngine.StringId) : MapSiegePOIVM.MachineTypes.None);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00013DA0 File Offset: 0x00011FA0
		private void RefreshQueueIndex()
		{
			int num;
			if (this.Machine == null)
			{
				num = -1;
			}
			else
			{
				num = this.Siege.GetSiegeEventSide(this.PlayerSide).SiegeEngines.DeployedSiegeEngines.Where<SiegeEvent.SiegeEngineConstructionProgress>((SiegeEvent.SiegeEngineConstructionProgress e) => !e.IsActive).ToList<SiegeEvent.SiegeEngineConstructionProgress>().IndexOf(this.Machine);
			}
			this._bindQueueIndex = num;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00013E10 File Offset: 0x00012010
		private bool DetermineIfPOIIsPlayerSide()
		{
			MapSiegePOIVM.POIType type = this.Type;
			if (type > MapSiegePOIVM.POIType.DefenderSiegeMachine)
			{
				return type - MapSiegePOIVM.POIType.AttackerRamSiegeMachine <= 2 && this.PlayerSide == BattleSideEnum.Attacker;
			}
			return this.PlayerSide == BattleSideEnum.Defender;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00013E48 File Offset: 0x00012048
		private bool IsInsideWindow()
		{
			return this._latestX - 100f <= Screen.RealScreenResolutionWidth && this._latestY - 100f <= Screen.RealScreenResolutionHeight && this._latestX + 100f >= 0f && this._latestY + 100f >= 0f;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00013EA6 File Offset: 0x000120A6
		public void ExecuteShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetSiegeEngineInProgressTooltip(this.Machine) });
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00013ECB File Offset: 0x000120CB
		public void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00013ED4 File Offset: 0x000120D4
		private MapSiegePOIVM.MachineTypes GetMachineTypeFromId(string id)
		{
			string text = id.ToLower();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num > 746114623U)
			{
				if (num <= 1820818168U)
				{
					if (num <= 1241455715U)
					{
						if (num != 808481256U)
						{
							if (num != 1241455715U)
							{
								return MapSiegePOIVM.MachineTypes.None;
							}
							if (!(text == "ram"))
							{
								return MapSiegePOIVM.MachineTypes.None;
							}
							return MapSiegePOIVM.MachineTypes.Ram;
						}
						else if (!(text == "fire_ballista"))
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
					}
					else if (num != 1748194790U)
					{
						if (num != 1820818168U)
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						if (!(text == "fire_onager"))
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						return MapSiegePOIVM.MachineTypes.Mangonel;
					}
					else
					{
						if (!(text == "fire_catapult"))
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						return MapSiegePOIVM.MachineTypes.Mangonel;
					}
				}
				else if (num <= 1898442385U)
				{
					if (num != 1839032341U)
					{
						if (num != 1898442385U)
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						if (!(text == "catapult"))
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						return MapSiegePOIVM.MachineTypes.Mangonel;
					}
					else
					{
						if (!(text == "trebuchet"))
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						return MapSiegePOIVM.MachineTypes.Trebuchet;
					}
				}
				else if (num != 2806198843U)
				{
					if (num != 4036530155U)
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
					if (!(text == "ballista"))
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
				}
				else
				{
					if (!(text == "onager"))
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
					return MapSiegePOIVM.MachineTypes.Mangonel;
				}
				return MapSiegePOIVM.MachineTypes.Ballista;
			}
			if (num > 473034592U)
			{
				if (num <= 712590611U)
				{
					if (num != 695812992U)
					{
						if (num != 712590611U)
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
						if (!(text == "siege_tower_level2"))
						{
							return MapSiegePOIVM.MachineTypes.None;
						}
					}
					else if (!(text == "siege_tower_level3"))
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
				}
				else if (num != 729368230U)
				{
					if (num != 746114623U)
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
					if (!(text == "fire_mangonel"))
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
					return MapSiegePOIVM.MachineTypes.Mangonel;
				}
				else if (!(text == "siege_tower_level1"))
				{
					return MapSiegePOIVM.MachineTypes.None;
				}
				return MapSiegePOIVM.MachineTypes.SiegeTower;
			}
			if (num != 6339497U)
			{
				if (num != 390431385U)
				{
					if (num != 473034592U)
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
					if (!(text == "mangonel"))
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
				}
				else
				{
					if (!(text == "bricole"))
					{
						return MapSiegePOIVM.MachineTypes.None;
					}
					return MapSiegePOIVM.MachineTypes.Trebuchet;
				}
			}
			else
			{
				if (!(text == "ladder"))
				{
					return MapSiegePOIVM.MachineTypes.None;
				}
				return MapSiegePOIVM.MachineTypes.Ladder;
			}
			return MapSiegePOIVM.MachineTypes.Mangonel;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000140F8 File Offset: 0x000122F8
		private SiegeEvent.SiegeEngineConstructionProgress GetDesiredMachine()
		{
			if (this.Siege != null)
			{
				switch (this.Type)
				{
				case MapSiegePOIVM.POIType.DefenderSiegeMachine:
					return this.Siege.GetSiegeEventSide(BattleSideEnum.Defender).SiegeEngines.DeployedRangedSiegeEngines[this.MachineIndex];
				case MapSiegePOIVM.POIType.AttackerRamSiegeMachine:
				case MapSiegePOIVM.POIType.AttackerTowerSiegeMachine:
					return this.Siege.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedMeleeSiegeEngines[this.MachineIndex];
				case MapSiegePOIVM.POIType.AttackerRangedSiegeMachine:
					return this.Siege.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.DeployedRangedSiegeEngines[this.MachineIndex];
				}
				return null;
			}
			return null;
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001418D File Offset: 0x0001238D
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00014195 File Offset: 0x00012395
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x000141B8 File Offset: 0x000123B8
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x000141C0 File Offset: 0x000123C0
		public Color SidePrimaryColor
		{
			get
			{
				return this._sidePrimaryColor;
			}
			set
			{
				if (this._sidePrimaryColor != value)
				{
					this._sidePrimaryColor = value;
					base.OnPropertyChangedWithValue(value, "SidePrimaryColor");
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x000141E3 File Offset: 0x000123E3
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x000141EB File Offset: 0x000123EB
		public Color SideSecondaryColor
		{
			get
			{
				return this._sideSecondaryColor;
			}
			set
			{
				if (this._sideSecondaryColor != value)
				{
					this._sideSecondaryColor = value;
					base.OnPropertyChangedWithValue(value, "SideSecondaryColor");
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001420E File Offset: 0x0001240E
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x00014216 File Offset: 0x00012416
		public int QueueIndex
		{
			get
			{
				return this._queueIndex;
			}
			set
			{
				if (this._queueIndex != value)
				{
					this._queueIndex = value;
					base.OnPropertyChangedWithValue(value, "QueueIndex");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00014234 File Offset: 0x00012434
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0001423C File Offset: 0x0001243C
		public int MachineType
		{
			get
			{
				return this._machineType;
			}
			set
			{
				if (this._machineType != value)
				{
					this._machineType = value;
					base.OnPropertyChangedWithValue(value, "MachineType");
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001425A File Offset: 0x0001245A
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00014262 File Offset: 0x00012462
		public float CurrentHitpoints
		{
			get
			{
				return this._currentHitpoints;
			}
			set
			{
				if (this._currentHitpoints != value)
				{
					this._currentHitpoints = value;
					base.OnPropertyChangedWithValue(value, "CurrentHitpoints");
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00014280 File Offset: 0x00012480
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00014288 File Offset: 0x00012488
		public float MaxHitpoints
		{
			get
			{
				return this._maxHitpoints;
			}
			set
			{
				if (this._maxHitpoints != value)
				{
					this._maxHitpoints = value;
					base.OnPropertyChangedWithValue(value, "MaxHitpoints");
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x000142A6 File Offset: 0x000124A6
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x000142AE File Offset: 0x000124AE
		public bool IsPlayerSidePOI
		{
			get
			{
				return this._isPlayerSidePOI;
			}
			set
			{
				if (this._isPlayerSidePOI != value)
				{
					this._isPlayerSidePOI = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerSidePOI");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x000142CC File Offset: 0x000124CC
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x000142D4 File Offset: 0x000124D4
		public bool IsFireVersion
		{
			get
			{
				return this._isFireVersion;
			}
			set
			{
				if (this._isFireVersion != value)
				{
					this._isFireVersion = value;
					base.OnPropertyChangedWithValue(value, "IsFireVersion");
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x000142F2 File Offset: 0x000124F2
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x000142FA File Offset: 0x000124FA
		public bool IsInVisibleRange
		{
			get
			{
				return this._isInVisibleRange;
			}
			set
			{
				if (this._isInVisibleRange != value)
				{
					this._isInVisibleRange = value;
					base.OnPropertyChangedWithValue(value, "IsInVisibleRange");
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00014318 File Offset: 0x00012518
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x00014320 File Offset: 0x00012520
		public bool IsConstructing
		{
			get
			{
				return this._isConstructing;
			}
			set
			{
				if (this._isConstructing != value)
				{
					this._isConstructing = value;
					base.OnPropertyChangedWithValue(value, "IsConstructing");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001433E File Offset: 0x0001253E
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00014346 File Offset: 0x00012546
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00014364 File Offset: 0x00012564
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001436C File Offset: 0x0001256C
		public bool HasItem
		{
			get
			{
				return this._hasItem;
			}
			set
			{
				if (this._hasItem != value)
				{
					this._hasItem = value;
					base.OnPropertyChangedWithValue(value, "HasItem");
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001438A File Offset: 0x0001258A
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x00014392 File Offset: 0x00012592
		public bool IsInside
		{
			get
			{
				return this._isInside;
			}
			set
			{
				if (this._isInside != value)
				{
					this._isInside = value;
					base.OnPropertyChangedWithValue(value, "IsInside");
				}
			}
		}

		// Token: 0x0400028F RID: 655
		private readonly Vec3 _mapSceneLocation;

		// Token: 0x04000290 RID: 656
		private readonly Camera _mapCamera;

		// Token: 0x04000291 RID: 657
		private readonly BattleSideEnum _thisSide;

		// Token: 0x04000292 RID: 658
		private readonly Action<MapSiegePOIVM> _onSelection;

		// Token: 0x04000293 RID: 659
		private float _latestX;

		// Token: 0x04000294 RID: 660
		private float _latestY;

		// Token: 0x04000295 RID: 661
		private float _latestW;

		// Token: 0x04000296 RID: 662
		private float _bindCurrentHitpoints;

		// Token: 0x04000297 RID: 663
		private float _bindMaxHitpoints;

		// Token: 0x04000298 RID: 664
		private float _bindWPos;

		// Token: 0x04000299 RID: 665
		private int _bindWSign;

		// Token: 0x0400029A RID: 666
		private int _bindMachineType = -1;

		// Token: 0x0400029B RID: 667
		private int _bindQueueIndex;

		// Token: 0x0400029C RID: 668
		private bool _bindIsInside;

		// Token: 0x0400029D RID: 669
		private bool _bindHasItem;

		// Token: 0x0400029E RID: 670
		private bool _bindIsConstructing;

		// Token: 0x0400029F RID: 671
		private Vec2 _bindPosition;

		// Token: 0x040002A0 RID: 672
		private bool _bindIsInVisibleRange;

		// Token: 0x040002A1 RID: 673
		private Color _sidePrimaryColor;

		// Token: 0x040002A2 RID: 674
		private Color _sideSecondaryColor;

		// Token: 0x040002A3 RID: 675
		private Vec2 _position;

		// Token: 0x040002A4 RID: 676
		private float _currentHitpoints;

		// Token: 0x040002A5 RID: 677
		private int _machineType = -1;

		// Token: 0x040002A6 RID: 678
		private float _maxHitpoints;

		// Token: 0x040002A7 RID: 679
		private int _queueIndex;

		// Token: 0x040002A8 RID: 680
		private bool _isInside;

		// Token: 0x040002A9 RID: 681
		private bool _hasItem;

		// Token: 0x040002AA RID: 682
		private bool _isConstructing;

		// Token: 0x040002AB RID: 683
		private bool _isPlayerSidePOI;

		// Token: 0x040002AC RID: 684
		private bool _isFireVersion;

		// Token: 0x040002AD RID: 685
		private bool _isInVisibleRange;

		// Token: 0x040002AE RID: 686
		private bool _isSelected;

		// Token: 0x020000AC RID: 172
		public enum POIType
		{
			// Token: 0x04000410 RID: 1040
			WallSection,
			// Token: 0x04000411 RID: 1041
			DefenderSiegeMachine,
			// Token: 0x04000412 RID: 1042
			AttackerRamSiegeMachine,
			// Token: 0x04000413 RID: 1043
			AttackerTowerSiegeMachine,
			// Token: 0x04000414 RID: 1044
			AttackerRangedSiegeMachine
		}

		// Token: 0x020000AD RID: 173
		public enum MachineTypes
		{
			// Token: 0x04000416 RID: 1046
			None = -1,
			// Token: 0x04000417 RID: 1047
			Wall,
			// Token: 0x04000418 RID: 1048
			BrokenWall,
			// Token: 0x04000419 RID: 1049
			Ballista,
			// Token: 0x0400041A RID: 1050
			Trebuchet,
			// Token: 0x0400041B RID: 1051
			Ladder,
			// Token: 0x0400041C RID: 1052
			Ram,
			// Token: 0x0400041D RID: 1053
			SiegeTower,
			// Token: 0x0400041E RID: 1054
			Mangonel
		}
	}
}
