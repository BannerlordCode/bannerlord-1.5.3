using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000226 RID: 550
	public sealed class Formation : IFormation
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001FF7 RID: 8183 RVA: 0x0006EA50 File Offset: 0x0006CC50
		// (remove) Token: 0x06001FF8 RID: 8184 RVA: 0x0006EA88 File Offset: 0x0006CC88
		public event Action<Formation, Agent> OnUnitAdded;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001FF9 RID: 8185 RVA: 0x0006EAC0 File Offset: 0x0006CCC0
		// (remove) Token: 0x06001FFA RID: 8186 RVA: 0x0006EAF8 File Offset: 0x0006CCF8
		public event Action<Formation, Agent> OnUnitRemoved;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001FFB RID: 8187 RVA: 0x0006EB30 File Offset: 0x0006CD30
		// (remove) Token: 0x06001FFC RID: 8188 RVA: 0x0006EB68 File Offset: 0x0006CD68
		public event Action<Formation, Agent> OnUnitAttached;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001FFD RID: 8189 RVA: 0x0006EBA0 File Offset: 0x0006CDA0
		// (remove) Token: 0x06001FFE RID: 8190 RVA: 0x0006EBD8 File Offset: 0x0006CDD8
		public event Action<Formation> OnUnitCountChanged;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001FFF RID: 8191 RVA: 0x0006EC10 File Offset: 0x0006CE10
		// (remove) Token: 0x06002000 RID: 8192 RVA: 0x0006EC48 File Offset: 0x0006CE48
		public event Action<Formation> OnUnitSpacingChanged;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06002001 RID: 8193 RVA: 0x0006EC80 File Offset: 0x0006CE80
		// (remove) Token: 0x06002002 RID: 8194 RVA: 0x0006ECB8 File Offset: 0x0006CEB8
		public event Action<Formation> OnTick;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002003 RID: 8195 RVA: 0x0006ECF0 File Offset: 0x0006CEF0
		// (remove) Token: 0x06002004 RID: 8196 RVA: 0x0006ED28 File Offset: 0x0006CF28
		public event Action<Formation> OnWidthChanged;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06002005 RID: 8197 RVA: 0x0006ED60 File Offset: 0x0006CF60
		// (remove) Token: 0x06002006 RID: 8198 RVA: 0x0006ED98 File Offset: 0x0006CF98
		public event Action<Formation, MovementOrder.MovementOrderEnum> OnBeforeMovementOrderApplied;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06002007 RID: 8199 RVA: 0x0006EDD0 File Offset: 0x0006CFD0
		// (remove) Token: 0x06002008 RID: 8200 RVA: 0x0006EE08 File Offset: 0x0006D008
		public event Action<Formation, ArrangementOrder.ArrangementOrderEnum> OnAfterArrangementOrderApplied;

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x0006EE3D File Offset: 0x0006D03D
		// (set) Token: 0x0600200A RID: 8202 RVA: 0x0006EE45 File Offset: 0x0006D045
		public Formation.RetreatPositionCacheSystem RetreatPositionCache { get; private set; } = new Formation.RetreatPositionCacheSystem(2);

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600200B RID: 8203 RVA: 0x0006EE4E File Offset: 0x0006D04E
		// (set) Token: 0x0600200C RID: 8204 RVA: 0x0006EE56 File Offset: 0x0006D056
		public FormationClass RepresentativeClass { get; private set; } = FormationClass.NumberOfAllFormations;

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x0006EE5F File Offset: 0x0006D05F
		// (set) Token: 0x0600200E RID: 8206 RVA: 0x0006EE67 File Offset: 0x0006D067
		public bool IsAIControlled { get; private set; } = true;

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x0006EE70 File Offset: 0x0006D070
		// (set) Token: 0x06002010 RID: 8208 RVA: 0x0006EE78 File Offset: 0x0006D078
		public Vec2 Direction { get; private set; }

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x0006EE81 File Offset: 0x0006D081
		// (set) Token: 0x06002012 RID: 8210 RVA: 0x0006EE89 File Offset: 0x0006D089
		public int UnitSpacing { get; private set; }

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x0006EE92 File Offset: 0x0006D092
		// (set) Token: 0x06002014 RID: 8212 RVA: 0x0006EE9A File Offset: 0x0006D09A
		public object MovementOrderPositionLock { get; private set; } = new object();

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x0006EEA3 File Offset: 0x0006D0A3
		// (set) Token: 0x06002016 RID: 8214 RVA: 0x0006EEAB File Offset: 0x0006D0AB
		public object SimulationFormationLock { get; private set; } = new object();

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x0006EEB4 File Offset: 0x0006D0B4
		public int CountOfUnits
		{
			get
			{
				return this.Arrangement.UnitCount + this._detachedUnits.Count;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x0006EECD File Offset: 0x0006D0CD
		public int CountOfDetachedUnits
		{
			get
			{
				return this._detachedUnits.Count;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x0006EEDA File Offset: 0x0006D0DA
		public int CountOfUndetachableNonPlayerUnits
		{
			get
			{
				return this._undetachableNonPlayerUnitCount;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0006EEE2 File Offset: 0x0006D0E2
		public int CountOfUnitsWithoutDetachedOnes
		{
			get
			{
				return this.Arrangement.UnitCount + this._looseDetachedUnits.Count;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600201B RID: 8219 RVA: 0x0006EEFB File Offset: 0x0006D0FB
		public MBReadOnlyList<IFormationUnit> UnitsWithoutLooseDetachedOnes
		{
			get
			{
				return this.Arrangement.GetAllUnits();
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x0006EF08 File Offset: 0x0006D108
		public int CountOfUnitsWithoutLooseDetachedOnes
		{
			get
			{
				return this.Arrangement.UnitCount;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x0006EF15 File Offset: 0x0006D115
		public int CountOfDetachableNonPlayerUnits
		{
			get
			{
				return this.Arrangement.UnitCount - ((this.IsPlayerTroopInFormation || this.HasPlayerControlledTroop) ? 1 : 0) - this.CountOfUndetachableNonPlayerUnits;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x0006EF3E File Offset: 0x0006D13E
		public Vec2 OrderPosition
		{
			get
			{
				return this._orderPosition.AsVec2;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x0006EF4B File Offset: 0x0006D14B
		public Vec3 OrderGroundPosition
		{
			get
			{
				return this._orderPosition.GetGroundVec3();
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002020 RID: 8224 RVA: 0x0006EF58 File Offset: 0x0006D158
		public Vec3 OrderGroundPositionMT
		{
			get
			{
				return this._orderPosition.GetGroundVec3MT();
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x0006EF65 File Offset: 0x0006D165
		public bool OrderPositionIsValid
		{
			get
			{
				return this._orderPosition.IsValid;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x0006EF72 File Offset: 0x0006D172
		public float Depth
		{
			get
			{
				return this.Arrangement.Depth;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x0006EF7F File Offset: 0x0006D17F
		public float MinimumWidth
		{
			get
			{
				return this.Arrangement.MinimumWidth;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x0006EF8C File Offset: 0x0006D18C
		public float MaximumWidth
		{
			get
			{
				return this.Arrangement.MaximumWidth;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x0006EF99 File Offset: 0x0006D199
		public float UnitDiameter
		{
			get
			{
				return Formation.GetDefaultUnitDiameter(this.CalculateHasSignificantNumberOfMounted && !(this.RidingOrder == RidingOrder.RidingOrderDismount));
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x0006EFC0 File Offset: 0x0006D1C0
		public Vec2 CurrentDirection
		{
			get
			{
				return (this.QuerySystem.EstimatedDirection * 0.8f + this.Direction * 0.2f).Normalized();
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x0006EFFF File Offset: 0x0006D1FF
		public Vec2 SmoothedAverageUnitPosition
		{
			get
			{
				return this._smoothedAverageUnitPosition;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x0006F007 File Offset: 0x0006D207
		public MBReadOnlyList<Agent> LooseDetachedUnits
		{
			get
			{
				return this._looseDetachedUnits;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x0006F00F File Offset: 0x0006D20F
		public MBReadOnlyList<Agent> DetachedUnits
		{
			get
			{
				return this._detachedUnits;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x0006F017 File Offset: 0x0006D217
		// (set) Token: 0x0600202B RID: 8235 RVA: 0x0006F01F File Offset: 0x0006D21F
		public AttackEntityOrderSecondaryDetachment AttackEntityOrderSecondaryDetachment { get; private set; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x0006F028 File Offset: 0x0006D228
		// (set) Token: 0x0600202D RID: 8237 RVA: 0x0006F030 File Offset: 0x0006D230
		public FormationAI AI { get; private set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x0006F039 File Offset: 0x0006D239
		// (set) Token: 0x0600202F RID: 8239 RVA: 0x0006F044 File Offset: 0x0006D244
		public Formation TargetFormation
		{
			get
			{
				return this._targetFormation;
			}
			private set
			{
				if (this._targetFormation != value)
				{
					this._targetFormation = value;
					this.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						Formation value2 = value;
						agent.SetTargetFormationIndex((value2 != null) ? value2.Index : (-1));
					}, null);
				}
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x0006F08B File Offset: 0x0006D28B
		// (set) Token: 0x06002031 RID: 8241 RVA: 0x0006F093 File Offset: 0x0006D293
		public FormationQuerySystem QuerySystem { get; private set; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x0006F09C File Offset: 0x0006D29C
		// (set) Token: 0x06002033 RID: 8243 RVA: 0x0006F0A4 File Offset: 0x0006D2A4
		public Formation.FormationIntegrityDataGroup CachedFormationIntegrityData { get; private set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002034 RID: 8244 RVA: 0x0006F0AD File Offset: 0x0006D2AD
		// (set) Token: 0x06002035 RID: 8245 RVA: 0x0006F0B5 File Offset: 0x0006D2B5
		public Vec2 CachedAveragePosition { get; private set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002036 RID: 8246 RVA: 0x0006F0BE File Offset: 0x0006D2BE
		// (set) Token: 0x06002037 RID: 8247 RVA: 0x0006F0C6 File Offset: 0x0006D2C6
		public WorldPosition CachedMedianPosition { get; private set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x0006F0CF File Offset: 0x0006D2CF
		// (set) Token: 0x06002039 RID: 8249 RVA: 0x0006F0D7 File Offset: 0x0006D2D7
		public Vec2 CachedCurrentVelocity { get; private set; }

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x0006F0E0 File Offset: 0x0006D2E0
		// (set) Token: 0x0600203B RID: 8251 RVA: 0x0006F0E8 File Offset: 0x0006D2E8
		public float CachedMovementSpeed { get; private set; } = 1f;

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x0006F0F1 File Offset: 0x0006D2F1
		// (set) Token: 0x0600203D RID: 8253 RVA: 0x0006F0F9 File Offset: 0x0006D2F9
		public float CachedClosestEnemyFormationDistanceSquared { get; private set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x0006F102 File Offset: 0x0006D302
		public FormationQuerySystem CachedClosestEnemyFormation
		{
			get
			{
				Formation cachedClosestEnemyFormation = this._cachedClosestEnemyFormation;
				if (cachedClosestEnemyFormation == null)
				{
					return null;
				}
				return cachedClosestEnemyFormation.QuerySystem;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x0006F115 File Offset: 0x0006D315
		public MBReadOnlyList<IDetachment> Detachments
		{
			get
			{
				return this._detachments;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x0006F11D File Offset: 0x0006D31D
		// (set) Token: 0x06002041 RID: 8257 RVA: 0x0006F125 File Offset: 0x0006D325
		public int? OverridenUnitCount { get; private set; }

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x0006F12E File Offset: 0x0006D32E
		// (set) Token: 0x06002043 RID: 8259 RVA: 0x0006F136 File Offset: 0x0006D336
		public bool IsSpawning { get; private set; }

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x0006F13F File Offset: 0x0006D33F
		// (set) Token: 0x06002045 RID: 8261 RVA: 0x0006F147 File Offset: 0x0006D347
		public bool IsAITickedAfterSplit { get; set; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x0006F150 File Offset: 0x0006D350
		// (set) Token: 0x06002047 RID: 8263 RVA: 0x0006F158 File Offset: 0x0006D358
		public bool HasPlayerControlledTroop { get; private set; }

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x0006F161 File Offset: 0x0006D361
		// (set) Token: 0x06002049 RID: 8265 RVA: 0x0006F169 File Offset: 0x0006D369
		public bool IsPlayerTroopInFormation { get; private set; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x0006F172 File Offset: 0x0006D372
		// (set) Token: 0x0600204B RID: 8267 RVA: 0x0006F17A File Offset: 0x0006D37A
		public bool ContainsAgentVisuals { get; set; }

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x0006F183 File Offset: 0x0006D383
		// (set) Token: 0x0600204D RID: 8269 RVA: 0x0006F18B File Offset: 0x0006D38B
		public Agent PlayerOwner
		{
			get
			{
				return this._playerOwner;
			}
			set
			{
				this._playerOwner = value;
				this.SetControlledByAI(value == null, false);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600204F RID: 8271 RVA: 0x0006F1D7 File Offset: 0x0006D3D7
		// (set) Token: 0x0600204E RID: 8270 RVA: 0x0006F19F File Offset: 0x0006D39F
		public string BannerCode
		{
			get
			{
				return this._bannerCode;
			}
			set
			{
				this._bannerCode = value;
				if (GameNetwork.IsServer)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new InitializeFormation(this, this.Team.TeamIndex, this._bannerCode));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x0006F1DF File Offset: 0x0006D3DF
		public bool IsSplittableByAI
		{
			get
			{
				return this.IsAIOwned && this.IsConvenientForTransfer;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002051 RID: 8273 RVA: 0x0006F1F4 File Offset: 0x0006D3F4
		public bool IsAIOwned
		{
			get
			{
				return !this._enforceNotSplittableByAI && (this.IsAIControlled || (!this.Team.IsPlayerGeneral && (!this.Team.IsPlayerSergeant || this.PlayerOwner != Agent.Main)));
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002052 RID: 8274 RVA: 0x0006F243 File Offset: 0x0006D443
		public bool IsConvenientForTransfer
		{
			get
			{
				return Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege || this.Team.Side != BattleSideEnum.Attacker || this.QuerySystem.InsideCastleUnitCountIncludingUnpositioned == 0;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x0006F270 File Offset: 0x0006D470
		public Vec2 OrderLocalAveragePosition
		{
			get
			{
				if (this._orderLocalAveragePositionIsDirty)
				{
					this._orderLocalAveragePositionIsDirty = false;
					this._orderLocalAveragePosition = default(Vec2);
					if (this.UnitsWithoutLooseDetachedOnes.Count > 0)
					{
						int num = 0;
						foreach (IFormationUnit formationUnit in this.UnitsWithoutLooseDetachedOnes)
						{
							Vec2? localPositionOfUnitOrDefault = this.Arrangement.GetLocalPositionOfUnitOrDefault(formationUnit);
							if (localPositionOfUnitOrDefault != null)
							{
								this._orderLocalAveragePosition += localPositionOfUnitOrDefault.Value;
								num++;
							}
						}
						if (num > 0)
						{
							this._orderLocalAveragePosition *= 1f / (float)num;
						}
					}
				}
				return this._orderLocalAveragePosition;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x0006F344 File Offset: 0x0006D544
		// (set) Token: 0x06002055 RID: 8277 RVA: 0x0006F34C File Offset: 0x0006D54C
		public FacingOrder FacingOrder { get; private set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x0006F355 File Offset: 0x0006D555
		// (set) Token: 0x06002057 RID: 8279 RVA: 0x0006F35D File Offset: 0x0006D55D
		public ArrangementOrder ArrangementOrder { get; private set; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x0006F366 File Offset: 0x0006D566
		// (set) Token: 0x06002059 RID: 8281 RVA: 0x0006F36E File Offset: 0x0006D56E
		public FormOrder FormOrder { get; private set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x0006F377 File Offset: 0x0006D577
		// (set) Token: 0x0600205B RID: 8283 RVA: 0x0006F37F File Offset: 0x0006D57F
		public RidingOrder RidingOrder { get; private set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x0006F388 File Offset: 0x0006D588
		// (set) Token: 0x0600205D RID: 8285 RVA: 0x0006F390 File Offset: 0x0006D590
		public FiringOrder FiringOrder { get; private set; }

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x0006F399 File Offset: 0x0006D599
		private bool IsSimulationFormation
		{
			get
			{
				return this.Team == null;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x0006F3A4 File Offset: 0x0006D5A4
		public bool HasAnyMountedUnit
		{
			get
			{
				if (this._overridenHasAnyMountedUnit != null)
				{
					return this._overridenHasAnyMountedUnit.Value;
				}
				int num = (int)(this.QuerySystem.RangedCavalryUnitRatioReadOnly * (float)this.CountOfUnits + 1E-05f);
				int num2 = (int)(this.QuerySystem.CavalryUnitRatioReadOnly * (float)this.CountOfUnits + 1E-05f);
				return num + num2 > 0;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x0006F404 File Offset: 0x0006D604
		// (set) Token: 0x06002061 RID: 8289 RVA: 0x0006F411 File Offset: 0x0006D611
		public float Width
		{
			get
			{
				return this.Arrangement.Width;
			}
			private set
			{
				this.Arrangement.Width = value;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x0006F41F File Offset: 0x0006D61F
		public bool IsDeployment
		{
			get
			{
				return Mission.Current.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x0006F42E File Offset: 0x0006D62E
		public FormationClass LogicalClass
		{
			get
			{
				return this._logicalClass;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002064 RID: 8292 RVA: 0x0006F436 File Offset: 0x0006D636
		public IEnumerable<FormationClass> SecondaryLogicalClasses
		{
			get
			{
				FormationClass primaryLogicalClass = this.LogicalClass;
				if (primaryLogicalClass == FormationClass.NumberOfAllFormations)
				{
					yield break;
				}
				List<ValueTuple<FormationClass, int>> list = new List<ValueTuple<FormationClass, int>>();
				for (int i = 0; i < this._logicalClassCounts.Length; i++)
				{
					if (this._logicalClassCounts[i] > 0)
					{
						list.Add(new ValueTuple<FormationClass, int>((FormationClass)i, this._logicalClassCounts[i]));
					}
				}
				if (list.Count > 0)
				{
					list.Sort(Comparer<ValueTuple<FormationClass, int>>.Create(delegate([TupleElementNames(new string[] { "fClass", "count" })] ValueTuple<FormationClass, int> x, [TupleElementNames(new string[] { "fClass", "count" })] ValueTuple<FormationClass, int> y)
					{
						if (x.Item2 < y.Item2)
						{
							return 1;
						}
						if (x.Item2 <= y.Item2)
						{
							return 0;
						}
						return -1;
					}));
					foreach (ValueTuple<FormationClass, int> valueTuple in list)
					{
						if (valueTuple.Item1 != primaryLogicalClass)
						{
							yield return valueTuple.Item1;
						}
					}
					List<ValueTuple<FormationClass, int>>.Enumerator enumerator = default(List<ValueTuple<FormationClass, int>>.Enumerator);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x0006F446 File Offset: 0x0006D646
		// (set) Token: 0x06002066 RID: 8294 RVA: 0x0006F450 File Offset: 0x0006D650
		public IFormationArrangement Arrangement
		{
			get
			{
				return this._arrangement;
			}
			set
			{
				if (this._arrangement != null)
				{
					this._arrangement.OnWidthChanged -= this.Arrangement_OnWidthChanged;
					this._arrangement.OnShapeChanged -= this.Arrangement_OnShapeChanged;
				}
				this._arrangement = value;
				if (this._arrangement != null)
				{
					this._arrangement.OnWidthChanged += this.Arrangement_OnWidthChanged;
					this._arrangement.OnShapeChanged += this.Arrangement_OnShapeChanged;
				}
				this.Arrangement_OnWidthChanged();
				this.Arrangement_OnShapeChanged();
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x0006F4DC File Offset: 0x0006D6DC
		public FormationClass PhysicalClass
		{
			get
			{
				return this.QuerySystem.MainClass;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x0006F4E9 File Offset: 0x0006D6E9
		public IEnumerable<FormationClass> SecondaryPhysicalClasses
		{
			get
			{
				FormationClass primaryPhysicalClass = this.PhysicalClass;
				if (primaryPhysicalClass != FormationClass.Infantry && this.QuerySystem.InfantryUnitRatio > 0f)
				{
					yield return FormationClass.Infantry;
				}
				if (primaryPhysicalClass != FormationClass.Ranged && this.QuerySystem.RangedUnitRatio > 0f)
				{
					yield return FormationClass.Ranged;
				}
				if (primaryPhysicalClass != FormationClass.Cavalry && this.QuerySystem.CavalryUnitRatio > 0f)
				{
					yield return FormationClass.Cavalry;
				}
				if (primaryPhysicalClass != FormationClass.HorseArcher && this.QuerySystem.RangedCavalryUnitRatio > 0f)
				{
					yield return FormationClass.HorseArcher;
				}
				yield break;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x0006F4FC File Offset: 0x0006D6FC
		public float Interval
		{
			get
			{
				if (this.CalculateHasSignificantNumberOfMounted && !(this.RidingOrder == RidingOrder.RidingOrderDismount))
				{
					return Formation.CavalryInterval(this.UnitSpacing) * this.Arrangement.IntervalMultiplier;
				}
				return Formation.InfantryInterval(this.UnitSpacing) * this.Arrangement.IntervalMultiplier;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x0006F552 File Offset: 0x0006D752
		public bool CalculateHasSignificantNumberOfMounted
		{
			get
			{
				if (this._overridenHasAnyMountedUnit != null)
				{
					return this._overridenHasAnyMountedUnit.Value;
				}
				return this.QuerySystem.CavalryUnitRatio + this.QuerySystem.RangedCavalryUnitRatio >= 0.1f;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x0006F590 File Offset: 0x0006D790
		public float Distance
		{
			get
			{
				if (this.CalculateHasSignificantNumberOfMounted && !(this.RidingOrder == RidingOrder.RidingOrderDismount))
				{
					return Formation.CavalryDistance(this.UnitSpacing) * this.Arrangement.DistanceMultiplier;
				}
				return Formation.InfantryDistance(this.UnitSpacing) * this.Arrangement.DistanceMultiplier;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x0006F5E8 File Offset: 0x0006D7E8
		public Vec2 CurrentPosition
		{
			get
			{
				ColumnFormation columnFormation;
				if ((columnFormation = this.Arrangement as ColumnFormation) != null)
				{
					if (this.CountOfUnitsWithoutDetachedOnes <= 0)
					{
						return this.OrderPosition;
					}
					Agent agent = (columnFormation.GetUnit(columnFormation.VanguardFileIndex, 0) ?? columnFormation.Vanguard) as Agent;
					if (agent != null)
					{
						return agent.Position.AsVec2;
					}
				}
				return this.CachedAveragePosition + this.CurrentDirection.TransformToParentUnitF(-this.OrderLocalAveragePosition);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x0006F667 File Offset: 0x0006D867
		// (set) Token: 0x0600206E RID: 8302 RVA: 0x0006F66F File Offset: 0x0006D86F
		public Agent Captain
		{
			get
			{
				return this._captain;
			}
			set
			{
				if (this._captain != value)
				{
					this._captain = value;
					this.OnCaptainChanged();
				}
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x0006F687 File Offset: 0x0006D887
		public float MinimumDistance
		{
			get
			{
				return Formation.GetDefaultMinimumUnitDistance(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount));
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002070 RID: 8304 RVA: 0x0006F6AC File Offset: 0x0006D8AC
		public bool IsLoose
		{
			get
			{
				return ArrangementOrder.GetUnitLooseness(this.ArrangementOrder.OrderEnum);
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x0006F6BE File Offset: 0x0006D8BE
		public float MinimumInterval
		{
			get
			{
				return Formation.GetDefaultMinimumUnitInterval(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount));
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x0006F6E3 File Offset: 0x0006D8E3
		public float MaximumInterval
		{
			get
			{
				return Formation.GetDefaultUnitInterval(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount), ArrangementOrder.GetUnitSpacingOf(this.ArrangementOrder.OrderEnum));
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x0006F718 File Offset: 0x0006D918
		public float MaximumDistance
		{
			get
			{
				return Formation.GetDefaultUnitDistance(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount), ArrangementOrder.GetUnitSpacingOf(this.ArrangementOrder.OrderEnum));
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x0006F74D File Offset: 0x0006D94D
		// (set) Token: 0x06002075 RID: 8309 RVA: 0x0006F755 File Offset: 0x0006D955
		internal bool PostponeCostlyOperations { get; private set; }

		// Token: 0x06002076 RID: 8310 RVA: 0x0006F760 File Offset: 0x0006D960
		public Formation(Team team, int index)
		{
			this.Team = team;
			this.Index = index;
			this.FormationIndex = (FormationClass)index;
			this.IsSpawning = false;
			this.Reset();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0006F808 File Offset: 0x0006DA08
		~Formation()
		{
			if (!this.IsSimulationFormation)
			{
				Formation._simulationFormationTemp = null;
			}
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0006F83C File Offset: 0x0006DA3C
		bool IFormation.GetIsLocalPositionAvailable(Vec2 localPosition, Vec2? nearestAvailableUnitPositionLocal)
		{
			Vec2 vec = this.Direction.TransformToParentUnitF(localPosition);
			WorldPosition worldPosition = this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.NavMeshVec3);
			worldPosition.SetVec2(this.OrderPosition + vec);
			WorldPosition worldPosition2 = WorldPosition.Invalid;
			if (nearestAvailableUnitPositionLocal != null)
			{
				vec = this.Direction.TransformToParentUnitF(nearestAvailableUnitPositionLocal.Value);
				worldPosition2 = this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.NavMeshVec3);
				worldPosition2.SetVec2(this.OrderPosition + vec);
			}
			float num = MathF.Abs(localPosition.x) + MathF.Abs(localPosition.y) + (this.Interval + this.Distance) * 2f;
			return Mission.Current.IsFormationUnitPositionAvailableMT(ref this._orderPosition, ref worldPosition, ref worldPosition2, num, this.Team);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x0006F900 File Offset: 0x0006DB00
		IFormationUnit IFormation.GetClosestUnitTo(Vec2 localPosition, MBList<IFormationUnit> unitsWithSpaces, float? maxDistance)
		{
			Vec2 vec = this.Direction.TransformToParentUnitF(localPosition);
			Vec2 vec2 = this.OrderPosition + vec;
			return this.GetClosestUnitToAux(vec2, unitsWithSpaces, maxDistance);
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x0006F934 File Offset: 0x0006DB34
		IFormationUnit IFormation.GetClosestUnitTo(IFormationUnit targetUnit, MBList<IFormationUnit> unitsWithSpaces, float? maxDistance)
		{
			return this.GetClosestUnitToAux(((Agent)targetUnit).Position.AsVec2, unitsWithSpaces, maxDistance);
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0006F95C File Offset: 0x0006DB5C
		void IFormation.SetUnitToFollow(IFormationUnit unit, IFormationUnit toFollow, Vec2 vector)
		{
			Agent agent = unit as Agent;
			Agent agent2 = toFollow as Agent;
			agent.SetColumnwiseFollowAgent(agent2, ref vector);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x0006F980 File Offset: 0x0006DB80
		bool IFormation.BatchUnitPositions(MBArrayList<Vec2i> orderedPositionIndices, MBArrayList<Vec2> orderedLocalPositions, MBList2D<int> availabilityTable, MBList2D<WorldPosition> globalPositionTable, int fileCount, int rankCount)
		{
			if (this._orderPosition.IsValid && this._orderPosition.GetNavMesh() != UIntPtr.Zero)
			{
				Mission.Current.BatchFormationUnitPositions(orderedPositionIndices, orderedLocalPositions, availabilityTable, globalPositionTable, this._orderPosition, this.Direction, fileCount, rankCount, Mission.Current.IsSiegeBattle);
				return true;
			}
			return false;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x0006F9DD File Offset: 0x0006DBDD
		public WorldPosition CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
			{
				if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
				{
					this._orderPosition.GetGroundVec3();
				}
			}
			else
			{
				this._orderPosition.GetNavMeshVec3();
			}
			return this._orderPosition;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x0006FA09 File Offset: 0x0006DC09
		public WorldPosition CreateNewOrderWorldPositionMT(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
			{
				if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
				{
					this._orderPosition.GetGroundVec3MT();
				}
			}
			else
			{
				this._orderPosition.GetNavMeshVec3MT();
			}
			return this._orderPosition;
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x0006FA38 File Offset: 0x0006DC38
		public void SetMovementOrder(MovementOrder input)
		{
			Action<Formation, MovementOrder.MovementOrderEnum> onBeforeMovementOrderApplied = this.OnBeforeMovementOrderApplied;
			if (onBeforeMovementOrderApplied != null)
			{
				onBeforeMovementOrderApplied(this, input.OrderEnum);
			}
			if (input.OrderEnum == MovementOrder.MovementOrderEnum.Invalid)
			{
				input = MovementOrder.MovementOrderStop;
			}
			bool flag = !this._movementOrder.AreOrdersPracticallySame(this._movementOrder, input, this.IsAIControlled);
			if (flag)
			{
				this._movementOrder.OnCancel(this);
			}
			if (flag)
			{
				if (MovementOrder.GetMovementOrderDefensivenessChange(this._movementOrder.OrderEnum, input.OrderEnum) != 0)
				{
					if (MovementOrder.GetMovementOrderDefensiveness(input.OrderEnum) == 0)
					{
						this._formationOrderDefensivenessFactor = 0;
					}
					else
					{
						this._formationOrderDefensivenessFactor = MovementOrder.GetMovementOrderDefensiveness(input.OrderEnum) + ArrangementOrder.GetArrangementOrderDefensiveness(this.ArrangementOrder.OrderEnum);
					}
					this.UpdateAgentDrivenPropertiesBasedOnOrderDefensiveness();
				}
				this._movementOrder = input;
				this._movementOrder.OnApply(this);
			}
			this.SetTargetFormation(null);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x0006FB08 File Offset: 0x0006DD08
		public void SetFacingOrder(FacingOrder order)
		{
			this.FacingOrder = order;
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0006FB14 File Offset: 0x0006DD14
		public void SetArrangementOrder(ArrangementOrder order)
		{
			if (order.OrderType != this.ArrangementOrder.OrderType)
			{
				this.ArrangementOrder.OnCancel(this);
				int arrangementOrderDefensivenessChange = ArrangementOrder.GetArrangementOrderDefensivenessChange(this.ArrangementOrder.OrderEnum, order.OrderEnum);
				if (arrangementOrderDefensivenessChange != 0 && MovementOrder.GetMovementOrderDefensiveness(this._movementOrder.OrderEnum) != 0)
				{
					this._formationOrderDefensivenessFactor += arrangementOrderDefensivenessChange;
					this.UpdateAgentDrivenPropertiesBasedOnOrderDefensiveness();
				}
				this.ArrangementOrder = order;
				this.ArrangementOrder.OnApply(this);
				Action<Formation, ArrangementOrder.ArrangementOrderEnum> onAfterArrangementOrderApplied = this.OnAfterArrangementOrderApplied;
				if (onAfterArrangementOrderApplied != null)
				{
					onAfterArrangementOrderApplied(this, this.ArrangementOrder.OrderEnum);
				}
				if (this.FormOrder.OrderEnum == FormOrder.FormOrderEnum.Custom && order.OrderEnum != ArrangementOrder.ArrangementOrderEnum.Column)
				{
					this.SetFormOrder(FormOrder.FormOrderCustom(this.CalculateDesiredWidth()), false);
				}
				this.QuerySystem.Expire();
				if (this.CountOfUnitsWithoutLooseDetachedOnes > 0)
				{
					this.ForceCalculateCaches();
					return;
				}
			}
			else
			{
				this.ArrangementOrder.SoftUpdate(this);
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0006FC10 File Offset: 0x0006DE10
		public void SetFormOrder(FormOrder order, bool updateDesiredFileCount = true)
		{
			if (order.OrderEnum == FormOrder.FormOrderEnum.Custom && updateDesiredFileCount)
			{
				this._desiredFileCount = (int)((order.CustomFlankWidth - this.UnitDiameter) / (this.UnitDiameter + this.Interval)) + 1;
			}
			this.FormOrder = order;
			this.FormOrder.OnApply(this);
			this.QuerySystem.Expire();
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x0006FC70 File Offset: 0x0006DE70
		public void SetRidingOrder(RidingOrder order)
		{
			if (this.RidingOrder != order)
			{
				this.RidingOrder = order;
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.SetRidingOrder(order.OrderEnum);
				}, null);
				this.Arrangement_OnShapeChanged();
			}
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x0006FCC4 File Offset: 0x0006DEC4
		public void SetFiringOrder(FiringOrder order)
		{
			if (this.FiringOrder != order)
			{
				this.FiringOrder = order;
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.SetFiringOrder(order.OrderEnum);
				}, null);
			}
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x0006FD10 File Offset: 0x0006DF10
		public void SetControlledByAI(bool isControlledByAI, bool enforceNotSplittableByAI = false)
		{
			if (this.IsAIControlled != isControlledByAI)
			{
				this.IsAIControlled = isControlledByAI;
				if (this.IsAIControlled)
				{
					if (this.AI.ActiveBehavior != null && this.CountOfUnits > 0)
					{
						bool forceTickOccasionally = Mission.Current.ForceTickOccasionally;
						Mission.Current.ForceTickOccasionally = true;
						BehaviorComponent activeBehavior = this.AI.ActiveBehavior;
						this.AI.Tick();
						Mission.Current.ForceTickOccasionally = forceTickOccasionally;
						if (activeBehavior == this.AI.ActiveBehavior)
						{
							this.AI.ActiveBehavior.OnBehaviorActivated();
						}
						this.SetMovementOrder(this.AI.ActiveBehavior.CurrentOrder);
					}
					this._enforceNotSplittableByAI = enforceNotSplittableByAI;
					return;
				}
				this._enforceNotSplittableByAI = false;
				FormationAI ai = this.AI;
				if (ai == null)
				{
					return;
				}
				BehaviorComponent activeBehavior2 = ai.ActiveBehavior;
				if (activeBehavior2 == null)
				{
					return;
				}
				activeBehavior2.OnLostAIControl();
			}
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x0006FDE4 File Offset: 0x0006DFE4
		public void SetTargetFormation(Formation targetFormation)
		{
			this.TargetFormation = targetFormation;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0006FDED File Offset: 0x0006DFED
		public void OnDeploymentFinished()
		{
			FormationAI ai = this.AI;
			if (ai != null)
			{
				ai.OnDeploymentFinished();
			}
			OrderController.TryCancelStopOrder(this);
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0006FE06 File Offset: 0x0006E006
		public void ResetArrangementOrderTickTimer()
		{
			this._arrangementOrderTickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x0006FE24 File Offset: 0x0006E024
		public void SetPositioning(WorldPosition? position = null, Vec2? direction = null, int? unitSpacing = null)
		{
			Vec2 orderPosition = this.OrderPosition;
			Vec2 direction2 = this.Direction;
			WorldPosition? worldPosition = null;
			bool flag = false;
			bool flag2 = false;
			if (position != null && position.Value.IsValid)
			{
				if (!this.HasBeenPositioned && !this.IsSimulationFormation)
				{
					this.HasBeenPositioned = true;
				}
				if (!position.Value.AsVec2.NearlyEquals(this.OrderPosition, 0.001f))
				{
					if (!Mission.Current.IsPositionInsideBoundaries(position.Value.AsVec2))
					{
						Vec2 closestBoundaryPosition = Mission.Current.GetClosestBoundaryPosition(position.Value.AsVec2);
						if (this.OrderPosition != closestBoundaryPosition)
						{
							WorldPosition value = position.Value;
							value.SetVec2(closestBoundaryPosition);
							worldPosition = new WorldPosition?(value);
						}
					}
					else
					{
						worldPosition = position;
					}
					if (!this.IsSimulationFormation && position.Value.AsVec2.DistanceSquared(this.OrderPosition) > this.UnitDiameter * this.UnitDiameter * 25f)
					{
						this.Arrangement.UpdateLocalPositionErrors(true);
					}
				}
			}
			if (direction != null && !this.Direction.NearlyEquals(direction.Value, 0.01f))
			{
				flag = true;
			}
			if (unitSpacing != null && this.UnitSpacing != unitSpacing.Value)
			{
				flag2 = true;
				if (!this.IsSimulationFormation)
				{
					this.Arrangement.UpdateLocalPositionErrors(false);
				}
			}
			if (worldPosition != null || flag || flag2)
			{
				this.Arrangement.BeforeFormationFrameChange();
				if (worldPosition != null)
				{
					this._orderPosition = worldPosition.Value;
				}
				if (flag)
				{
					this.Direction = direction.Value;
				}
				if (flag2)
				{
					this.UnitSpacing = unitSpacing.Value;
					Action<Formation> onUnitSpacingChanged = this.OnUnitSpacingChanged;
					if (onUnitSpacingChanged != null)
					{
						onUnitSpacingChanged(this);
					}
					this.Arrangement_OnShapeChanged();
					this.Arrangement.AreLocalPositionsDirty = true;
				}
				if (!this.IsSimulationFormation && this.Arrangement.IsTurnBackwardsNecessary(orderPosition, worldPosition, direction2, flag, direction))
				{
					this.Arrangement.TurnBackwards();
				}
				this.Arrangement.OnFormationFrameChanged(false);
				if (worldPosition != null)
				{
					this.ArrangementOrder.OnOrderPositionChanged(this, orderPosition);
				}
			}
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00070074 File Offset: 0x0006E274
		public int GetCountOfUnitsWithCondition(Func<Agent, bool> function)
		{
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (function((Agent)formationUnit))
				{
					num++;
				}
			}
			foreach (Agent agent in this._detachedUnits)
			{
				if (function(agent))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00070124 File Offset: 0x0006E324
		public readonly ref MovementOrder GetReadonlyMovementOrderReference()
		{
			return ref this._movementOrder;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0007012C File Offset: 0x0006E32C
		public Agent GetFirstUnit()
		{
			return this.GetUnitWithIndex(0);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00070135 File Offset: 0x0006E335
		public int GetCountOfUnitsBelongingToLogicalClass(FormationClass logicalClass)
		{
			return this._logicalClassCounts[(int)logicalClass];
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x00070140 File Offset: 0x0006E340
		public int GetCountOfUnitsBelongingToPhysicalClass(FormationClass physicalClass, bool excludeBannerBearers)
		{
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				bool flag = false;
				switch (physicalClass)
				{
				case FormationClass.Infantry:
					flag = (excludeBannerBearers ? QueryLibrary.IsInfantryWithoutBanner((Agent)formationUnit) : QueryLibrary.IsInfantry((Agent)formationUnit));
					break;
				case FormationClass.Ranged:
					flag = (excludeBannerBearers ? QueryLibrary.IsRangedWithoutBanner((Agent)formationUnit) : QueryLibrary.IsRanged((Agent)formationUnit));
					break;
				case FormationClass.Cavalry:
					flag = (excludeBannerBearers ? QueryLibrary.IsCavalryWithoutBanner((Agent)formationUnit) : QueryLibrary.IsCavalry((Agent)formationUnit));
					break;
				case FormationClass.HorseArcher:
					flag = (excludeBannerBearers ? QueryLibrary.IsRangedCavalryWithoutBanner((Agent)formationUnit) : QueryLibrary.IsRangedCavalry((Agent)formationUnit));
					break;
				}
				if (flag)
				{
					num++;
				}
			}
			foreach (Agent agent in this._detachedUnits)
			{
				bool flag2 = false;
				switch (physicalClass)
				{
				case FormationClass.Infantry:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsInfantryWithoutBanner(agent) : QueryLibrary.IsInfantry(agent));
					break;
				case FormationClass.Ranged:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsRangedWithoutBanner(agent) : QueryLibrary.IsRanged(agent));
					break;
				case FormationClass.Cavalry:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsCavalryWithoutBanner(agent) : QueryLibrary.IsCavalry(agent));
					break;
				case FormationClass.HorseArcher:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsRangedCavalryWithoutBanner(agent) : QueryLibrary.IsRangedCavalry(agent));
					break;
				}
				if (flag2)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000702F4 File Offset: 0x0006E4F4
		public void SetSpawnIndex(int value = 0)
		{
			this._currentSpawnIndex = value;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000702FD File Offset: 0x0006E4FD
		public int GetNextSpawnIndex()
		{
			int currentSpawnIndex = this._currentSpawnIndex;
			this._currentSpawnIndex++;
			return currentSpawnIndex;
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00070314 File Offset: 0x0006E514
		public Agent GetUnitWithIndex(int unitIndex)
		{
			if (this.Arrangement.GetAllUnits().Count > unitIndex)
			{
				return (Agent)this.Arrangement.GetAllUnits()[unitIndex];
			}
			unitIndex -= this.Arrangement.GetAllUnits().Count;
			if (this._detachedUnits.Count > unitIndex)
			{
				return this._detachedUnits[unitIndex];
			}
			return null;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x0007037C File Offset: 0x0006E57C
		public Vec2 GetAveragePositionOfUnits(bool excludeDetachedUnits, bool excludePlayer)
		{
			int num = (excludeDetachedUnits ? this.CountOfUnitsWithoutDetachedOnes : this.CountOfUnits);
			if (num > 0)
			{
				Vec2 vec = Vec2.Zero;
				foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
				{
					Agent agent = (Agent)formationUnit;
					if (!excludePlayer || !agent.IsMainAgent)
					{
						vec += agent.Position.AsVec2;
					}
					else
					{
						num--;
					}
				}
				if (excludeDetachedUnits)
				{
					using (List<Agent>.Enumerator enumerator2 = this._looseDetachedUnits.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Agent agent2 = enumerator2.Current;
							vec += agent2.Position.AsVec2;
						}
						goto IL_0112;
					}
				}
				foreach (Agent agent3 in this._detachedUnits)
				{
					vec += agent3.Position.AsVec2;
				}
				IL_0112:
				if (num > 0)
				{
					return vec * (1f / (float)num);
				}
			}
			return Vec2.Invalid;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000704DC File Offset: 0x0006E6DC
		public Agent GetMedianAgent(bool excludeDetachedUnits, bool excludePlayer, Vec2 averagePosition)
		{
			excludeDetachedUnits = excludeDetachedUnits && this.CountOfUnitsWithoutDetachedOnes > 0;
			excludePlayer = excludePlayer && (this.CountOfUndetachableNonPlayerUnits > 0 || this.CountOfDetachableNonPlayerUnits > 0);
			float num = float.MaxValue;
			Agent agent = null;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent2 = (Agent)formationUnit;
				if (!excludePlayer || !agent2.IsMainAgent)
				{
					float num2 = agent2.Position.AsVec2.DistanceSquared(averagePosition);
					if (agent2 == this._lastMedianAgent)
					{
						num2 *= 0.8f;
					}
					if (num2 <= num)
					{
						agent = agent2;
						num = num2;
					}
				}
			}
			if (excludeDetachedUnits)
			{
				using (List<Agent>.Enumerator enumerator2 = this._looseDetachedUnits.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Agent agent3 = enumerator2.Current;
						float num3 = agent3.Position.AsVec2.DistanceSquared(averagePosition);
						if (agent3 == this._lastMedianAgent)
						{
							num3 *= 0.8f;
						}
						if (num3 <= num)
						{
							agent = agent3;
							num = num3;
						}
					}
					goto IL_018D;
				}
			}
			foreach (Agent agent4 in this._detachedUnits)
			{
				float num4 = agent4.Position.AsVec2.DistanceSquared(averagePosition);
				if (agent4 == this._lastMedianAgent)
				{
					num4 *= 0.8f;
				}
				if (num4 <= num)
				{
					agent = agent4;
					num = num4;
				}
			}
			IL_018D:
			this._lastMedianAgent = agent;
			return agent;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000706A8 File Offset: 0x0006E8A8
		public bool HasUnitWithLastRecievedAttackType(Agent.LastRecievedAttackType type, float timeLimit = 3f)
		{
			float currentTime = Mission.Current.CurrentTime;
			switch (type)
			{
			case Agent.LastRecievedAttackType.None:
				goto IL_0157;
			case Agent.LastRecievedAttackType.MeleeHit:
				goto IL_00C2;
			case Agent.LastRecievedAttackType.RangedHit:
				goto IL_010E;
			case Agent.LastRecievedAttackType.MeleeContact:
			{
				using (List<IFormationUnit>.Enumerator enumerator = this.Arrangement.GetAllUnits().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Agent)enumerator.Current).LastRecievedMeleeContactTime > currentTime - timeLimit)
						{
							return true;
						}
					}
					return false;
				}
				break;
			}
			case Agent.LastRecievedAttackType.RangedContact:
				break;
			default:
				return false;
			}
			using (List<IFormationUnit>.Enumerator enumerator = this.Arrangement.GetAllUnits().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Agent)enumerator.Current).LastRecievedRangedContactTime > currentTime - timeLimit)
					{
						return true;
					}
				}
				return false;
			}
			IL_00C2:
			using (List<IFormationUnit>.Enumerator enumerator = this.Arrangement.GetAllUnits().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Agent)enumerator.Current).LastRecievedMeleeHitTime > currentTime - timeLimit)
					{
						return true;
					}
				}
				return false;
			}
			IL_010E:
			using (List<IFormationUnit>.Enumerator enumerator = this.Arrangement.GetAllUnits().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Agent)enumerator.Current).LastRecievedRangedHitTime > currentTime - timeLimit)
					{
						return true;
					}
				}
				return false;
			}
			IL_0157:
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				if (agent.LastRecievedMeleeContactTime < currentTime - timeLimit && agent.LastRecievedRangedContactTime < currentTime - timeLimit && agent.LastRecievedMeleeHitTime < currentTime - timeLimit && agent.LastRecievedRangedHitTime < currentTime - timeLimit)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000708B8 File Offset: 0x0006EAB8
		public Agent.LastRecievedAttackType GetLastRecievedHitTypeOfUnits(float timeLimit = 3f)
		{
			float currentTime = Mission.Current.CurrentTime;
			int num = 0;
			int num2 = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				if (agent.LastRecievedMeleeHitTime > currentTime - timeLimit)
				{
					num++;
				}
				if (agent.LastRecievedRangedHitTime > currentTime - timeLimit)
				{
					num2++;
				}
			}
			foreach (Agent agent2 in this.DetachedUnits)
			{
				if (agent2.LastRecievedMeleeHitTime > currentTime - timeLimit)
				{
					num++;
				}
				if (agent2.LastRecievedRangedHitTime > currentTime - timeLimit)
				{
					num2++;
				}
			}
			if (num >= num2)
			{
				return Agent.LastRecievedAttackType.MeleeHit;
			}
			return Agent.LastRecievedAttackType.RangedHit;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00070998 File Offset: 0x0006EB98
		public Agent.LastRecievedAttackType GetLastRecievedContactTypeOfUnits(float timeLimit = 3f)
		{
			float currentTime = Mission.Current.CurrentTime;
			int num = 0;
			int num2 = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				if (agent.LastRecievedMeleeContactTime > currentTime - timeLimit)
				{
					num++;
				}
				if (agent.LastRecievedRangedContactTime > currentTime - timeLimit)
				{
					num2++;
				}
			}
			foreach (Agent agent2 in this.DetachedUnits)
			{
				if (agent2.LastRecievedMeleeContactTime > currentTime - timeLimit)
				{
					num++;
				}
				if (agent2.LastRecievedRangedContactTime > currentTime - timeLimit)
				{
					num2++;
				}
			}
			if (num >= num2)
			{
				return Agent.LastRecievedAttackType.MeleeContact;
			}
			return Agent.LastRecievedAttackType.RangedContact;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00070A78 File Offset: 0x0006EC78
		public Agent.MovementBehaviorType GetMovementTypeOfUnits()
		{
			float curMissionTime = Mission.Current.CurrentTime;
			int retreatingCount = 0;
			int attackingCount = 0;
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				if (agent.IsAIControlled && (agent.IsRetreating() || (agent.Formation != null && agent.Formation._movementOrder.OrderType == OrderType.Retreat)))
				{
					int num = retreatingCount;
					retreatingCount = num + 1;
				}
				if (curMissionTime - agent.LastMeleeHitTime < 3f)
				{
					int num = attackingCount;
					attackingCount = num + 1;
				}
			}, null);
			if (this.CountOfUnits > 0 && (float)retreatingCount / (float)this.CountOfUnits > 0.3f)
			{
				return Agent.MovementBehaviorType.Flee;
			}
			if (attackingCount > 0)
			{
				return Agent.MovementBehaviorType.Engaged;
			}
			return Agent.MovementBehaviorType.Idle;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00070AE9 File Offset: 0x0006ECE9
		public IEnumerable<Agent> GetUnitsWithoutDetachedOnes()
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				yield return formationUnit as Agent;
			}
			List<IFormationUnit>.Enumerator enumerator = default(List<IFormationUnit>.Enumerator);
			int num;
			for (int i = 0; i < this._looseDetachedUnits.Count; i = num + 1)
			{
				yield return this._looseDetachedUnits[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00070AFC File Offset: 0x0006ECFC
		public Vec2 GetWallDirectionOfRelativeFormationLocation(Agent unit)
		{
			if (unit.IsDetachedFromFormation)
			{
				return Vec2.Invalid;
			}
			Vec2? localWallDirectionOfRelativeFormationLocation = this.Arrangement.GetLocalWallDirectionOfRelativeFormationLocation(unit);
			if (localWallDirectionOfRelativeFormationLocation != null)
			{
				return this.Direction.TransformToParentUnitF(localWallDirectionOfRelativeFormationLocation.Value);
			}
			return Vec2.Invalid;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00070B48 File Offset: 0x0006ED48
		public Vec2 GetDirectionOfUnit(Agent unit)
		{
			if (unit.IsDetachedFromFormation)
			{
				return unit.GetMovementDirection();
			}
			Vec2? localDirectionOfUnitOrDefault = this.Arrangement.GetLocalDirectionOfUnitOrDefault(unit);
			if (localDirectionOfUnitOrDefault != null)
			{
				return this.Direction.TransformToParentUnitF(localDirectionOfUnitOrDefault.Value);
			}
			return unit.GetMovementDirection();
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00070B98 File Offset: 0x0006ED98
		private WorldPosition GetOrderPositionOfUnitAux(Agent unit)
		{
			WorldPosition? worldPositionOfUnitOrDefault = this.Arrangement.GetWorldPositionOfUnitOrDefault(unit);
			if (worldPositionOfUnitOrDefault != null)
			{
				return worldPositionOfUnitOrDefault.Value;
			}
			if (!this.OrderPositionIsValid)
			{
				WorldPosition worldPosition = unit.GetWorldPosition();
				Debug.Print(string.Concat(new object[]
				{
					"Formation order position is not valid. Team: ",
					this.Team.TeamIndex,
					", Formation: ",
					(int)this.FormationIndex,
					"Unit Pos: ",
					worldPosition.GetGroundVec3(),
					"Mission Mode: ",
					Mission.Current.Mode
				}), 0, Debug.DebugColor.Yellow, 17592186044416UL);
			}
			Mission mission = unit.Mission;
			WorldPosition worldPosition2 = this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3);
			if (mission.IsTeleportingAgents)
			{
				float num = MathF.Sqrt((float)this.Arrangement.GetUnpositionedUnits().Count);
				Vec2 vec = new Vec2((MBRandom.RandomFloat - 0.5f) * num, (MBRandom.RandomFloat - 0.5f) * num);
				WorldPosition worldPosition3 = new WorldPosition(Mission.Current.Scene, worldPosition2.GetVec3WithoutValidity() + new Vec3(vec, 0f, -1f));
				if (mission.IsFormationUnitPositionAvailable(ref worldPosition3, this.Team))
				{
					return worldPosition3;
				}
				if (mission.IsFormationUnitPositionAvailable(ref worldPosition2, this.Team))
				{
					return worldPosition2;
				}
				return unit.GetWorldPosition();
			}
			else
			{
				if (unit.Mission.IsFormationUnitPositionAvailable(ref worldPosition2, this.Team))
				{
					return worldPosition2;
				}
				return unit.GetWorldPosition();
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00070D25 File Offset: 0x0006EF25
		public MovementOrder.MovementStateEnum GetMovementState()
		{
			return this._movementOrder.MovementState;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00070D34 File Offset: 0x0006EF34
		public WorldPosition GetOrderPositionOfUnit(Agent unit)
		{
			if (this._movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.Follow && this._movementOrder._targetAgent != null && this._movementOrder._targetAgent == unit)
			{
				return unit.GetWorldPosition();
			}
			if (unit.IsDetachedFromFormation && (this._movementOrder.MovementState != MovementOrder.MovementStateEnum.Charge || !unit.Detachment.IsLoose || unit.Mission.Mode == MissionMode.Deployment || this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.None).IsValid))
			{
				WorldFrame? detachmentFrame = this.GetDetachmentFrame(unit);
				if (detachmentFrame == null)
				{
					return WorldPosition.Invalid;
				}
				return detachmentFrame.GetValueOrDefault().Origin;
			}
			else
			{
				if (unit.Mission.IsDeploymentFinished && unit.GetAgentFlags().HasAnyFlag(AgentFlag.UnreachableViaNavMesh))
				{
					return WorldPosition.Invalid;
				}
				switch (this._movementOrder.MovementState)
				{
				case MovementOrder.MovementStateEnum.Charge:
					if (unit.Mission.Mode == MissionMode.Deployment)
					{
						return this.GetOrderPositionOfUnitAux(unit);
					}
					if (!this.OrderPositionIsValid)
					{
						WorldPosition worldPosition = unit.GetWorldPosition();
						Debug.Print(string.Concat(new object[]
						{
							"Formation order position is not valid. Team: ",
							this.Team.TeamIndex,
							", Formation: ",
							(int)this.FormationIndex,
							"Unit Pos: ",
							worldPosition.GetGroundVec3()
						}), 0, Debug.DebugColor.Yellow, 17592186044416UL);
					}
					return this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.None);
				case MovementOrder.MovementStateEnum.Hold:
					return this.GetOrderPositionOfUnitAux(unit);
				case MovementOrder.MovementStateEnum.Retreat:
					return WorldPosition.Invalid;
				case MovementOrder.MovementStateEnum.StandGround:
					return unit.GetWorldPosition();
				default:
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Formation.cs", "GetOrderPositionOfUnit", 1813);
					return WorldPosition.Invalid;
				}
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00070EF4 File Offset: 0x0006F0F4
		public Vec2 GetCurrentGlobalPositionOfUnit(Agent unit, bool blendWithOrderDirection)
		{
			if (unit.IsDetachedFromFormation)
			{
				return unit.Position.AsVec2;
			}
			Vec2? localPositionOfUnitOrDefaultWithAdjustment = this.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(unit, blendWithOrderDirection ? ((this.QuerySystem.EstimatedInterval - this.Interval) * 0.9f) : 0f);
			if (localPositionOfUnitOrDefaultWithAdjustment != null)
			{
				return (blendWithOrderDirection ? this.CurrentDirection : this.QuerySystem.EstimatedDirection).TransformToParentUnitF(localPositionOfUnitOrDefaultWithAdjustment.Value) + this.CurrentPosition;
			}
			return unit.Position.AsVec2;
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00070F90 File Offset: 0x0006F190
		public float GetAverageMaximumMovementSpeedOfUnits()
		{
			if (this.CountOfUnitsWithoutDetachedOnes == 0)
			{
				return 0.1f;
			}
			float num = 0f;
			foreach (Agent agent in this.GetUnitsWithoutDetachedOnes())
			{
				num += agent.GetMaximumForwardUnlimitedSpeed();
			}
			return num / (float)this.CountOfUnitsWithoutDetachedOnes;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00070FFC File Offset: 0x0006F1FC
		private void CacheMovementSpeedOfUnits()
		{
			float? num;
			float? num2;
			this.ArrangementOrder.GetMovementSpeedRestriction(out num, out num2);
			if (num == null && num2 == null)
			{
				num = new float?(1f);
			}
			if (num2 != null)
			{
				if (this.CountOfUnits == 0)
				{
					this.CachedMovementSpeed = 0.1f;
					return;
				}
				IEnumerable<Agent> enumerable;
				if (this.CountOfUnitsWithoutDetachedOnes != 0)
				{
					enumerable = this.GetUnitsWithoutDetachedOnes();
				}
				else
				{
					IEnumerable<Agent> enumerable2 = this._detachedUnits;
					enumerable = enumerable2;
				}
				float num3 = enumerable.Min<Agent>((Agent u) => u.WalkSpeedCached);
				this.CachedMovementSpeed = num3 * num2.Value;
				return;
			}
			else
			{
				if (this.CountOfUnits == 0)
				{
					this.CachedMovementSpeed = 0.1f;
					return;
				}
				IEnumerable<Agent> enumerable3;
				if (this.CountOfUnitsWithoutDetachedOnes != 0)
				{
					enumerable3 = this.GetUnitsWithoutDetachedOnes();
				}
				else
				{
					IEnumerable<Agent> enumerable2 = this._detachedUnits;
					enumerable3 = enumerable2;
				}
				float num4 = enumerable3.Average<Agent>((Agent u) => u.GetMaximumForwardUnlimitedSpeed());
				Formation.FormationIntegrityDataGroup cachedFormationIntegrityData = this.CachedFormationIntegrityData;
				if (cachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents < cachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents * 0.5f)
				{
					this.CachedMovementSpeed = num4 * num.Value;
					return;
				}
				this.CachedMovementSpeed = num4;
				return;
			}
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00071130 File Offset: 0x0006F330
		private void CacheClosestEnemyFormation()
		{
			float num = float.MaxValue;
			this._cachedClosestEnemyFormation = null;
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.IsEnemyOf(this.Team))
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							float num2 = formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(this.CachedAveragePosition, this.CachedMedianPosition.GetNavMeshZ(), -1f));
							if (num2 < num)
							{
								num = num2;
								this._cachedClosestEnemyFormation = formation;
							}
						}
					}
				}
			}
			this.CachedClosestEnemyFormationDistanceSquared = num;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x0007123C File Offset: 0x0006F43C
		private void CacheFormationIntegrityData()
		{
			bool flag = false;
			if (this.CountOfUnitsWithoutLooseDetachedOnes > 0)
			{
				float num = 0f;
				MBReadOnlyList<IFormationUnit> allUnits = this.Arrangement.GetAllUnits();
				int num2 = 0;
				float num3 = this.QuerySystem.EstimatedInterval - this.Interval;
				foreach (IFormationUnit formationUnit in allUnits)
				{
					Agent agent = (Agent)formationUnit;
					Vec2? localPositionOfUnitOrDefaultWithAdjustment = this.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(agent, num3);
					if (localPositionOfUnitOrDefaultWithAdjustment != null)
					{
						Vec2 vec = this.QuerySystem.EstimatedDirection.TransformToParentUnitF(localPositionOfUnitOrDefaultWithAdjustment.Value) + this.CurrentPosition;
						num2++;
						num += (vec - agent.Position.AsVec2).LengthSquared;
					}
				}
				if (num2 > 0)
				{
					float num4 = num / (float)num2 * 4f;
					float num5 = 0f;
					float num6 = 0f;
					Vec2 vec2 = Vec2.Zero;
					float num7 = 0f;
					num2 = 0;
					foreach (IFormationUnit formationUnit2 in allUnits)
					{
						Agent agent2 = (Agent)formationUnit2;
						Vec2? localPositionOfUnitOrDefaultWithAdjustment2 = this.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(agent2, num3);
						if (localPositionOfUnitOrDefaultWithAdjustment2 != null)
						{
							float lengthSquared = (this.QuerySystem.EstimatedDirection.TransformToParentUnitF(localPositionOfUnitOrDefaultWithAdjustment2.Value) + this.CurrentPosition - agent2.Position.AsVec2).LengthSquared;
							if (lengthSquared < num4)
							{
								if (lengthSquared > num6)
								{
									num6 = lengthSquared;
								}
								num5 += lengthSquared;
								vec2 += agent2.AverageVelocity.AsVec2;
								num7 += agent2.GetMaximumForwardUnlimitedSpeed();
								num2++;
							}
						}
					}
					if (num2 > 0)
					{
						vec2 *= 1f / (float)num2;
						num5 /= (float)num2;
						num7 /= (float)num2;
						this.CachedFormationIntegrityData = new Formation.FormationIntegrityDataGroup(vec2, MathF.Sqrt(num5), MathF.Sqrt(num6), num7);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.CachedFormationIntegrityData = new Formation.FormationIntegrityDataGroup(Vec2.Zero, 0f, 0f, 0f);
			}
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000714A4 File Offset: 0x0006F6A4
		private void CacheAverageAndMedianPositionAndVelocity()
		{
			Vec2 vec = ((this.CountOfUnitsWithoutDetachedOnes > 1) ? this.GetAveragePositionOfUnits(true, true) : ((this.CountOfUnitsWithoutDetachedOnes > 0) ? this.GetAveragePositionOfUnits(true, false) : this.OrderPosition));
			float currentTime = Mission.Current.CurrentTime;
			float num = currentTime - this._lastAveragePositionCacheTime;
			if (num > 0f)
			{
				this.CachedCurrentVelocity = (vec - this.CachedAveragePosition) * (1f / num);
			}
			this._lastAveragePositionCacheTime = currentTime;
			this.CachedAveragePosition = vec;
			this.CachedMedianPosition = ((this.CountOfUnitsWithoutDetachedOnes == 0) ? ((this.CountOfUnits == 0) ? this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None) : ((this.CountOfUnits == 1) ? this.GetFirstUnit().GetWorldPosition() : this.GetMedianAgent(false, true, this.CachedAveragePosition).GetWorldPosition())) : ((this.CountOfUnitsWithoutDetachedOnes == 1) ? this.GetMedianAgent(true, false, this.CachedAveragePosition).GetWorldPosition() : this.GetMedianAgent(true, true, this.CachedAveragePosition).GetWorldPosition()));
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000715A4 File Offset: 0x0006F7A4
		public float GetFormationPower()
		{
			float sum = 0f;
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				sum += agent.CharacterPowerCached;
			}, null);
			return sum;
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000715DC File Offset: 0x0006F7DC
		public float GetFormationMeleeFightingPower()
		{
			float sum = 0f;
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				sum += agent.CharacterPowerCached * ((this.FormationIndex == FormationClass.Ranged || this.FormationIndex == FormationClass.HorseArcher) ? 0.4f : 1f);
			}, null);
			return sum;
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x0007161C File Offset: 0x0006F81C
		internal IDetachment GetDetachmentForDebug(Agent agent)
		{
			return this.Detachments.FirstOrDefault<IDetachment>((IDetachment d) => d.IsAgentUsingOrInterested(agent));
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x0007164D File Offset: 0x0006F84D
		public WorldFrame? GetDetachmentFrame(Agent agent)
		{
			return agent.Detachment.GetAgentFrame(agent);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x0007165C File Offset: 0x0006F85C
		public Vec2 GetMiddleFrontUnitPositionOffset()
		{
			Vec2 localPositionOfReservedUnitPosition = this.Arrangement.GetLocalPositionOfReservedUnitPosition();
			return this.Direction.TransformToParentUnitF(localPositionOfReservedUnitPosition);
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00071684 File Offset: 0x0006F884
		public List<IFormationUnit> GetUnitsToPopWithReferencePosition(int count, Vec3 targetPosition)
		{
			int num = MathF.Min(count, this.Arrangement.UnitCount);
			List<IFormationUnit> list = ((num == 0) ? new List<IFormationUnit>() : this.Arrangement.GetUnitsToPop(num, targetPosition));
			int num2 = count - list.Count;
			if (num2 > 0)
			{
				List<Agent> list2 = this._looseDetachedUnits.Take<Agent>(num2).ToList<Agent>();
				num2 -= list2.Count;
				list.AddRange(list2);
			}
			if (num2 > 0)
			{
				IEnumerable<Agent> enumerable = this._detachedUnits.Take<Agent>(num2);
				num2 -= enumerable.Count<Agent>();
				list.AddRange(enumerable);
			}
			return list;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00071710 File Offset: 0x0006F910
		public List<IFormationUnit> GetUnitsToPop(int count)
		{
			int num = MathF.Min(count, this.Arrangement.UnitCount);
			List<IFormationUnit> list = ((num == 0) ? new List<IFormationUnit>() : this.Arrangement.GetUnitsToPop(num));
			int num2 = count - list.Count;
			if (num2 > 0)
			{
				List<Agent> list2 = this._looseDetachedUnits.Take<Agent>(num2).ToList<Agent>();
				num2 -= list2.Count;
				list.AddRange(list2);
			}
			if (num2 > 0)
			{
				IEnumerable<Agent> enumerable = this._detachedUnits.Take<Agent>(num2);
				num2 -= enumerable.Count<Agent>();
				list.AddRange(enumerable);
			}
			return list;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x0007179A File Offset: 0x0006F99A
		public IEnumerable<ValueTuple<WorldPosition, Vec2>> GetUnavailableUnitPositionsAccordingToNewOrder(Formation simulationFormation, in WorldPosition position, in Vec2 direction, float width, int unitSpacing)
		{
			return Formation.GetUnavailableUnitPositionsAccordingToNewOrder(this, simulationFormation, position, direction, this.Arrangement, width, unitSpacing);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000717BC File Offset: 0x0006F9BC
		public void GetUnitSpawnFrameWithIndex(int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitCount, int unitSpacing, bool isMountedFormation, out WorldPosition? unitSpawnPosition, out Vec2? unitSpawnDirection)
		{
			float num;
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(null, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, unitCount, isMountedFormation, this.Index, out unitSpawnPosition, out unitSpawnDirection, out num);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000717EC File Offset: 0x0006F9EC
		public void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitSpacing, out WorldPosition? unitSpawnPosition, out Vec2? unitSpawnDirection)
		{
			float num;
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, this.Arrangement.UnitCount, this.HasAnyMountedUnit, this.Index, out unitSpawnPosition, out unitSpawnDirection, out num);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x0007182C File Offset: 0x0006FA2C
		public void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitSpacing, int overridenUnitCount, out WorldPosition? unitPosition, out Vec2? unitDirection)
		{
			float num;
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, overridenUnitCount, this.HasAnyMountedUnit, this.Index, out unitPosition, out unitDirection, out num);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00071864 File Offset: 0x0006FA64
		public void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitSpacing, out WorldPosition? unitSpawnPosition, out Vec2? unitSpawnDirection, out float actualWidth)
		{
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, this.Arrangement.UnitCount, this.HasAnyMountedUnit, this.Index, out unitSpawnPosition, out unitSpawnDirection, out actualWidth);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000718A4 File Offset: 0x0006FAA4
		public bool HasUnitsWithCondition(Func<Agent, bool> function)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (function((Agent)formationUnit))
				{
					return true;
				}
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				if (function(this._detachedUnits[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00071938 File Offset: 0x0006FB38
		public bool HasUnitsWithCondition(Func<Agent, bool> function, out Agent result)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (function((Agent)formationUnit))
				{
					result = (Agent)formationUnit;
					return true;
				}
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				if (function(this._detachedUnits[i]))
				{
					result = this._detachedUnits[i];
					return true;
				}
			}
			result = null;
			return false;
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000719E4 File Offset: 0x0006FBE4
		public bool HasAnyEnemyFormationsThatIsNotEmpty()
		{
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.IsEnemyOf(this.Team))
				{
					using (List<Formation>.Enumerator enumerator2 = team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.CountOfUnits > 0)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00071A8C File Offset: 0x0006FC8C
		public bool HasUnitWithConditionLimitedRandom(Func<Agent, bool> function, int startingIndex, int willBeCheckedUnitCount, out Agent resultAgent)
		{
			int unitCount = this.Arrangement.UnitCount;
			int count = this._detachedUnits.Count;
			if (unitCount + count <= willBeCheckedUnitCount)
			{
				return this.HasUnitsWithCondition(function, out resultAgent);
			}
			for (int i = 0; i < willBeCheckedUnitCount; i++)
			{
				if (startingIndex < unitCount)
				{
					int num = MBRandom.RandomInt(unitCount);
					if (function((Agent)this.Arrangement.GetAllUnits()[num]))
					{
						resultAgent = (Agent)this.Arrangement.GetAllUnits()[num];
						return true;
					}
				}
				else if (count > 0)
				{
					int num = MBRandom.RandomInt(count);
					if (function(this._detachedUnits[num]))
					{
						resultAgent = this._detachedUnits[num];
						return true;
					}
				}
			}
			resultAgent = null;
			return false;
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00071B48 File Offset: 0x0006FD48
		public int[] CollectUnitIndices()
		{
			if (this._agentIndicesCache == null || this._agentIndicesCache.Length != this.CountOfUnits)
			{
				this._agentIndicesCache = new int[this.CountOfUnits];
			}
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				this._agentIndicesCache[num] = ((Agent)formationUnit).Index;
				num++;
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				this._agentIndicesCache[num] = this._detachedUnits[i].Index;
				num++;
			}
			return this._agentIndicesCache;
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00071C14 File Offset: 0x0006FE14
		public void ApplyActionOnEachUnit(Action<Agent> action, Agent ignoreAgent = null)
		{
			if (ignoreAgent == null)
			{
				foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
				{
					Agent agent = (Agent)formationUnit;
					action(agent);
				}
				for (int i = 0; i < this._detachedUnits.Count; i++)
				{
					action(this._detachedUnits[i]);
				}
				return;
			}
			foreach (IFormationUnit formationUnit2 in this.Arrangement.GetAllUnits())
			{
				Agent agent2 = (Agent)formationUnit2;
				if (agent2 != ignoreAgent)
				{
					action(agent2);
				}
			}
			for (int j = 0; j < this._detachedUnits.Count; j++)
			{
				Agent agent3 = this._detachedUnits[j];
				if (agent3 != ignoreAgent)
				{
					action(agent3);
				}
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00071D24 File Offset: 0x0006FF24
		public void ApplyActionOnEachAttachedUnit(Action<Agent> action)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				action(agent);
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00071D84 File Offset: 0x0006FF84
		public void ApplyActionOnEachDetachedUnit(Action<Agent> action)
		{
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				action(this._detachedUnits[i]);
			}
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00071DBC File Offset: 0x0006FFBC
		public void ApplyActionOnEachUnitViaBackupList(Action<Agent> action)
		{
			if (this.Arrangement.GetAllUnits().Count > 0)
			{
				foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits().ToArray())
				{
					action((Agent)formationUnit);
				}
			}
			if (this._detachedUnits.Count > 0)
			{
				foreach (Agent agent in this._detachedUnits.ToArray())
				{
					action(agent);
				}
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00071E40 File Offset: 0x00070040
		public void ApplyActionOnEachUnit(Action<Agent, List<WorldPosition>> action, List<WorldPosition> list)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				action((Agent)formationUnit, list);
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				action(this._detachedUnits[i], list);
			}
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00071EC8 File Offset: 0x000700C8
		public int CountUnitsOnNavMeshIDMod10(int navMeshID, bool includeOnlyPositionedUnits)
		{
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (((Agent)formationUnit).GetCurrentNavigationFaceId() % 10 == navMeshID && (!includeOnlyPositionedUnits || this.Arrangement.GetUnpositionedUnits() == null || this.Arrangement.GetUnpositionedUnits().IndexOf(formationUnit) < 0))
				{
					num++;
				}
			}
			if (!includeOnlyPositionedUnits)
			{
				using (List<Agent>.Enumerator enumerator2 = this._detachedUnits.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetCurrentNavigationFaceId() % 10 == navMeshID)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00071FA4 File Offset: 0x000701A4
		public void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
			AgentControllerType controller = agent.Controller;
			if (oldController != AgentControllerType.Player && controller == AgentControllerType.Player)
			{
				this.HasPlayerControlledTroop = true;
				if (!GameNetwork.IsMultiplayer)
				{
					this.TryRelocatePlayerUnit();
				}
				if (!agent.IsDetachableFromFormation)
				{
					this.OnUndetachableNonPlayerUnitRemoved(agent);
					return;
				}
			}
			else if (oldController == AgentControllerType.Player && controller != AgentControllerType.Player)
			{
				this.HasPlayerControlledTroop = false;
				if (!agent.IsDetachableFromFormation)
				{
					this.OnUndetachableNonPlayerUnitAdded(agent);
				}
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00072002 File Offset: 0x00070202
		public void OnMassUnitTransferStart()
		{
			this.PostponeCostlyOperations = true;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x0007200C File Offset: 0x0007020C
		public void OnMassUnitTransferEnd()
		{
			this.ReapplyFormOrder();
			this.QuerySystem.Expire();
			this.Team.QuerySystem.ExpireAfterUnitAddRemove();
			if (this._logicalClassNeedsUpdate)
			{
				this.CalculateLogicalClass();
			}
			if (this.CountOfUnits == 0)
			{
				this.RepresentativeClass = FormationClass.NumberOfAllFormations;
			}
			if (Mission.Current.IsTeleportingAgents)
			{
				this.SetPositioning(new WorldPosition?(this._orderPosition), null, null);
				this.Arrangement.OnFormationFrameChanged(false);
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				this.SetHasPendingUnitPositions(false);
			}
			this.PostponeCostlyOperations = false;
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000720C7 File Offset: 0x000702C7
		public void OnBatchUnitRemovalStart()
		{
			this.PostponeCostlyOperations = true;
			this.Arrangement.OnBatchRemoveStart();
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000720DC File Offset: 0x000702DC
		public void OnBatchUnitRemovalEnd()
		{
			this.Arrangement.OnBatchRemoveEnd();
			if (this.PostponeCostlyOperations)
			{
				this.ReapplyFormOrder();
				this.QuerySystem.ExpireAfterUnitAddRemove();
				this.Team.QuerySystem.ExpireAfterUnitAddRemove();
				if (this._logicalClassNeedsUpdate)
				{
					this.CalculateLogicalClass();
				}
				this.PostponeCostlyOperations = false;
			}
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00072134 File Offset: 0x00070334
		public void OnUnitAddedOrRemoved()
		{
			if (!this.PostponeCostlyOperations)
			{
				this.ReapplyFormOrder();
				this.QuerySystem.ExpireAfterUnitAddRemove();
				Team team = this.Team;
				if (team != null)
				{
					team.QuerySystem.ExpireAfterUnitAddRemove();
				}
			}
			this.CacheAverageAndMedianPositionAndVelocity();
			this.CacheMovementSpeedOfUnits();
			this.CacheFormationIntegrityData();
			float currentTime = Mission.Current.CurrentTime;
			this._cachedMovementSpeedUpdateTimer.Reset(currentTime);
			this._cachedFormationIntegrityDataUpdateTimer.Reset(currentTime);
			this._cachedPositionAndVelocityUpdateTimer.Reset(currentTime);
			Action<Formation> onUnitCountChanged = this.OnUnitCountChanged;
			if (onUnitCountChanged == null)
			{
				return;
			}
			onUnitCountChanged(this);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000721C2 File Offset: 0x000703C2
		public void OnAgentLostMount(Agent agent)
		{
			if (!agent.IsDetachedFromFormation)
			{
				this._arrangement.OnUnitLostMount(agent);
			}
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000721D8 File Offset: 0x000703D8
		public void OnFormationDispersed()
		{
			this.Arrangement.OnFormationDispersed();
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.ForceUpdateCachedAndFormationValues(true, false);
			}, null);
			this.SetHasPendingUnitPositions(false);
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00072212 File Offset: 0x00070412
		public void OnUnitDetachmentChanged(Agent unit, bool isOldDetachmentLoose, bool isNewDetachmentLoose)
		{
			if (isOldDetachmentLoose && !isNewDetachmentLoose)
			{
				this._looseDetachedUnits.Remove(unit);
				return;
			}
			if (!isOldDetachmentLoose && isNewDetachmentLoose)
			{
				this._looseDetachedUnits.Add(unit);
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x0007223C File Offset: 0x0007043C
		public void OnUndetachableNonPlayerUnitAdded(Agent unit)
		{
			if (unit.Formation == this && !unit.IsPlayerControlled)
			{
				this._undetachableNonPlayerUnitCount++;
			}
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00072263 File Offset: 0x00070463
		public void OnUndetachableNonPlayerUnitRemoved(Agent unit)
		{
			if (unit.Formation == this && !unit.IsPlayerControlled)
			{
				this._undetachableNonPlayerUnitCount--;
			}
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0007228A File Offset: 0x0007048A
		public void ResetMovementOrderPositionCache()
		{
			this._movementOrder.ResetPositionCache();
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00072298 File Offset: 0x00070498
		public void TestTaskForce(Agent victimAgent, Agent attackerAgent)
		{
			if ((this._movementOrder.MovementState == MovementOrder.MovementStateEnum.Hold || this._movementOrder.MovementState == MovementOrder.MovementStateEnum.StandGround) && !victimAgent.IsDetachedFromFormation && victimAgent.IsDetachableFromFormation && victimAgent.IsAIAtMoveDestination() && Mission.Current.Scene.DoesPathExistBetweenPositions(attackerAgent.GetWorldPosition(), victimAgent.GetWorldPosition()))
			{
				TaskForceDetachment taskForceDetachment;
				this._taskForces.TryGetValue(attackerAgent, out taskForceDetachment);
				if (taskForceDetachment == null)
				{
					if (this.QuerySystem.LocalEnemyPower <= attackerAgent.CharacterPowerCached)
					{
						float num = attackerAgent.Position.DistanceSquared(victimAgent.Position);
						if (num <= 400f || (attackerAgent.Formation != null && (attackerAgent.Formation.CountOfUnits == 1 || num <= attackerAgent.Position.AsVec2.DistanceSquared(attackerAgent.Formation.CachedAveragePosition) * 0.25f)) || num <= attackerAgent.Position.AsVec2.DistanceSquared(attackerAgent.Team.QuerySystem.AveragePosition) * 0.25f)
						{
							this._tempAgentList.Clear();
							Mission.Current.GetNearbyEnemyAgents(attackerAgent.Position.AsVec2, 10f, this.Team, this._tempAgentList);
							if (this._tempAgentList.Count <= 1)
							{
								TaskForceDetachment taskForceDetachment2 = new TaskForceDetachment(victimAgent, attackerAgent);
								this.JoinDetachment(taskForceDetachment2);
								this._taskForces.Add(attackerAgent, taskForceDetachment2);
								return;
							}
						}
					}
				}
				else if (taskForceDetachment.IsUsedByFormation(victimAgent.Formation))
				{
					taskForceDetachment.AddReinforcementAgent(victimAgent);
				}
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00072434 File Offset: 0x00070634
		public void Reset()
		{
			this.Arrangement = new LineFormation(this, true);
			this._arrangementOrderTickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._arrangementTickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._cachedFormationIntegrityDataUpdateTimer = new Timer(Mission.Current.CurrentTime, 0.9f + MBRandom.RandomFloat * 0.2f, true);
			this._cachedPositionAndVelocityUpdateTimer = new Timer(Mission.Current.CurrentTime, 0.075f + MBRandom.RandomFloat * 0.05f, true);
			this._cachedMovementSpeedUpdateTimer = new Timer(Mission.Current.CurrentTime, 1.9f + MBRandom.RandomFloat * 0.2f, true);
			this._cachedClosestEnemyFormationUpdateTimer = new Timer(Mission.Current.CurrentTime, 1.4f + MBRandom.RandomFloat * 0.2f, true);
			this._checkTaskForceDetachmentsTimer = new Timer(Mission.Current.CurrentTime, 0.9f + MBRandom.RandomFloat * 0.2f, true);
			this.ResetAux();
			this.FacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			this._enforceNotSplittableByAI = false;
			this.ContainsAgentVisuals = false;
			this.PlayerOwner = null;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00072570 File Offset: 0x00070770
		public IEnumerable<Formation> Split(int count = 2)
		{
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				formation.PostponeCostlyOperations = true;
			}
			IEnumerable<Formation> enumerable = this.Team.MasterOrderController.SplitFormation(this, count);
			if (enumerable.Count<Formation>() > 1 && this.Team != null)
			{
				foreach (Formation formation2 in enumerable)
				{
					formation2.QuerySystem.Expire();
				}
			}
			foreach (Formation formation3 in this.Team.FormationsIncludingEmpty)
			{
				formation3.CalculateLogicalClass();
				formation3.PostponeCostlyOperations = false;
			}
			if (this.CountOfUnits == 0)
			{
				this.RepresentativeClass = FormationClass.NumberOfAllFormations;
			}
			return enumerable;
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00072680 File Offset: 0x00070880
		public void TransferUnits(Formation target, int unitCount)
		{
			this.PostponeCostlyOperations = true;
			target.PostponeCostlyOperations = true;
			this.Team.MasterOrderController.TransferUnits(this, target, unitCount);
			this.CalculateLogicalClass();
			target.CalculateLogicalClass();
			if (this.CountOfUnits == 0)
			{
				this.RepresentativeClass = FormationClass.NumberOfAllFormations;
			}
			this.PostponeCostlyOperations = false;
			target.PostponeCostlyOperations = false;
			this.QuerySystem.Expire();
			target.QuerySystem.Expire();
			this.Team.QuerySystem.ExpireAfterUnitAddRemove();
			target.Team.QuerySystem.ExpireAfterUnitAddRemove();
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00072710 File Offset: 0x00070910
		public void TransferUnitsAux(Formation target, int unitCount, bool isPlayerOrder, bool useSelectivePop)
		{
			if (!isPlayerOrder && !this.IsSplittableByAI)
			{
				return;
			}
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print(string.Concat(new object[]
			{
				this.Team.Side,
				" ",
				this.FormationIndex.GetName(),
				" transfers ",
				unitCount,
				" units to ",
				target.FormationIndex.GetName()
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			if (unitCount == 0)
			{
				return;
			}
			if (target.CountOfUnits == 0)
			{
				target.CopyOrdersFrom(this);
				target.SetPositioning(new WorldPosition?(this._orderPosition), new Vec2?(this.Direction), new int?(this.UnitSpacing));
			}
			BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
			List<IFormationUnit> list;
			if (battleBannerBearersModel.GetFormationBanner(this) == null)
			{
				list = (useSelectivePop ? this.GetUnitsToPopWithReferencePosition(unitCount, target.OrderPositionIsValid ? target.OrderPosition.ToVec3(0f) : target.CachedMedianPosition.GetGroundVec3()) : this.GetUnitsToPop(unitCount).ToList<IFormationUnit>());
			}
			else
			{
				List<Agent> formationBannerBearers = battleBannerBearersModel.GetFormationBannerBearers(this);
				int num = Math.Min(this.CountOfUnits, unitCount + formationBannerBearers.Count);
				list = (useSelectivePop ? this.GetUnitsToPopWithReferencePosition(num, target.OrderPositionIsValid ? target.OrderPosition.ToVec3(0f) : target.CachedMedianPosition.GetGroundVec3()) : this.GetUnitsToPop(num).ToList<IFormationUnit>());
				foreach (Agent agent in formationBannerBearers)
				{
					if (list.Count <= unitCount)
					{
						break;
					}
					list.Remove(agent);
				}
				if (list.Count > unitCount)
				{
					int num2 = list.Count - unitCount;
					list.RemoveRange(list.Count - num2, num2);
				}
			}
			if (battleBannerBearersModel.GetFormationBanner(target) != null)
			{
				foreach (Agent agent2 in battleBannerBearersModel.GetFormationBannerBearers(target))
				{
					if (agent2.Formation == this && !list.Contains(agent2))
					{
						int num3 = list.FindIndex(delegate(IFormationUnit unit)
						{
							Agent agent3;
							return (agent3 = unit as Agent) != null && agent3.Banner == null;
						});
						if (num3 < 0)
						{
							break;
						}
						list[num3] = agent2;
					}
				}
			}
			foreach (IFormationUnit formationUnit in list)
			{
				((Agent)formationUnit).Formation = target;
			}
			this.Team.TriggerOnFormationsChanged(this);
			this.Team.TriggerOnFormationsChanged(target);
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00072AD8 File Offset: 0x00070CD8
		[Conditional("DEBUG")]
		public void DebugArrangements()
		{
			foreach (Team team in Mission.Current.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.AgentVisuals.SetContourColor(null, true);
						}, null);
					}
				}
			}
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.AgentVisuals.SetContourColor(new uint?(4294901760U), true);
			}, null);
			Vec3 vec = this.Direction.ToVec3(0f);
			vec.RotateAboutZ(1.5707964f);
			bool isSimulationFormation = this.IsSimulationFormation;
			vec * this.Width * 0.5f;
			this.Direction.ToVec3(0f) * this.Depth * 0.5f;
			bool orderPositionIsValid = this.OrderPositionIsValid;
			this.CachedMedianPosition.SetVec2(this.CurrentPosition);
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				WorldPosition orderPositionOfUnit = this.GetOrderPositionOfUnit(agent);
				if (orderPositionOfUnit.IsValid)
				{
					Vec2 vec2 = this.GetDirectionOfUnit(agent);
					vec2.Normalize();
					vec2 *= 0.1f;
					orderPositionOfUnit.GetGroundVec3() + vec2.ToVec3(0f);
					orderPositionOfUnit.GetGroundVec3() - vec2.LeftVec().ToVec3(0f);
					orderPositionOfUnit.GetGroundVec3() + vec2.LeftVec().ToVec3(0f);
					string.Concat(new object[]
					{
						"(",
						((IFormationUnit)agent).FormationFileIndex,
						",",
						((IFormationUnit)agent).FormationRankIndex,
						")"
					});
				}
			}, null);
			bool orderPositionIsValid2 = this.OrderPositionIsValid;
			foreach (IDetachment detachment in this.Detachments)
			{
				UsableMachine usableMachine = detachment as UsableMachine;
				RangedSiegeWeapon rangedSiegeWeapon = detachment as RangedSiegeWeapon;
			}
			if (this.Arrangement is ColumnFormation)
			{
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.GetFollowedUnit();
					string.Concat(new object[]
					{
						"(",
						((IFormationUnit)agent).FormationFileIndex,
						",",
						((IFormationUnit)agent).FormationRankIndex,
						")"
					});
				}, null);
			}
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00072CD4 File Offset: 0x00070ED4
		public void AddUnit(Agent unit)
		{
			bool countOfUnits = this.CountOfUnits != 0;
			if (this.Arrangement.AddUnit(unit) && Mission.Current.HasMissionBehavior<AmmoSupplyLogic>() && Mission.Current.GetMissionBehavior<AmmoSupplyLogic>().IsAgentEligibleForAmmoSupply(unit))
			{
				unit.SetScriptedCombatFlags(unit.GetScriptedCombatFlags() | Agent.AISpecialCombatModeFlags.IgnoreAmmoLimitForRangeCalculation);
				unit.ResetAiWaitBeforeShootFactor();
				unit.UpdateAgentStats();
			}
			if (unit.IsPlayerControlled)
			{
				this.HasPlayerControlledTroop = true;
			}
			if (unit.IsPlayerTroop)
			{
				this.IsPlayerTroopInFormation = true;
			}
			if (!unit.IsDetachableFromFormation && !unit.IsPlayerControlled)
			{
				this.OnUndetachableNonPlayerUnitAdded(unit);
			}
			if (unit.Character != null)
			{
				FormationClass formationClass = this.Team.Mission.GetAgentTroopClass(this.Team.Side, unit.Character).DefaultClass();
				this._logicalClassCounts[(int)formationClass]++;
				if (this._logicalClass != formationClass)
				{
					if (this.PostponeCostlyOperations)
					{
						this._logicalClassNeedsUpdate = true;
					}
					else
					{
						this.CalculateLogicalClass();
						this._logicalClassNeedsUpdate = false;
					}
				}
			}
			this._movementOrder.OnUnitJoinOrLeave(this, unit, true);
			Formation targetFormation = this.TargetFormation;
			unit.SetTargetFormationIndex((targetFormation != null) ? targetFormation.Index : (-1));
			unit.SetFiringOrder(this.FiringOrder.OrderEnum);
			unit.SetRidingOrder(this.RidingOrder.OrderEnum);
			this.OnUnitAddedOrRemoved();
			Action<Formation, Agent> onUnitAdded = this.OnUnitAdded;
			if (onUnitAdded != null)
			{
				onUnitAdded(this, unit);
			}
			if (!countOfUnits && this.CountOfUnits > 0)
			{
				TeamAIComponent teamAI = this.Team.TeamAI;
				if (teamAI == null)
				{
					return;
				}
				teamAI.OnUnitAddedToFormationForTheFirstTime(this);
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00072E50 File Offset: 0x00071050
		public void RemoveUnit(Agent unit)
		{
			if (unit.IsDetachedFromFormation)
			{
				unit.Detachment.RemoveAgent(unit);
				this._detachedUnits.Remove(unit);
				this._looseDetachedUnits.Remove(unit);
				unit.Detachment = null;
				unit.SetDetachmentWeight(-1f);
			}
			else
			{
				this.Arrangement.RemoveUnit(unit);
			}
			if (unit.Character != null)
			{
				FormationClass formationClass = this.Team.Mission.GetAgentTroopClass(this.Team.Side, unit.Character).DefaultClass();
				this._logicalClassCounts[(int)formationClass]--;
				if (this._logicalClass == formationClass)
				{
					if (this.PostponeCostlyOperations)
					{
						this._logicalClassNeedsUpdate = true;
					}
					else
					{
						this.CalculateLogicalClass();
						this._logicalClassNeedsUpdate = false;
					}
				}
			}
			if (unit.IsPlayerTroop)
			{
				this.IsPlayerTroopInFormation = false;
			}
			if (unit.IsPlayerControlled)
			{
				this.HasPlayerControlledTroop = false;
			}
			if (unit == this.Captain && !unit.CanLeadFormationsRemotely)
			{
				this.Captain = null;
			}
			if (!unit.IsDetachableFromFormation && !unit.IsPlayerControlled)
			{
				this.OnUndetachableNonPlayerUnitRemoved(unit);
			}
			if (Mission.Current.Mode != MissionMode.Deployment && !this.IsAIControlled && this.CountOfUnits == 0)
			{
				this.SetControlledByAI(true, false);
			}
			this._movementOrder.OnUnitJoinOrLeave(this, unit, false);
			unit.SetTargetFormationIndex(-1);
			unit.SetFiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill);
			unit.SetRidingOrder(RidingOrder.RidingOrderEnum.Free);
			this.OnUnitAddedOrRemoved();
			Action<Formation, Agent> onUnitRemoved = this.OnUnitRemoved;
			if (onUnitRemoved == null)
			{
				return;
			}
			onUnitRemoved(this, unit);
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00072FBF File Offset: 0x000711BF
		public void DetachUnit(Agent unit, bool isLoose)
		{
			this.Arrangement.RemoveUnit(unit);
			this._detachedUnits.Add(unit);
			if (isLoose)
			{
				this._looseDetachedUnits.Add(unit);
			}
			unit.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefaultDetached);
			this.OnUnitAttachedOrDetached();
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00072FF8 File Offset: 0x000711F8
		public void AttachUnit(Agent unit)
		{
			this._detachedUnits.Remove(unit);
			this._looseDetachedUnits.Remove(unit);
			this.Arrangement.AddUnit(unit);
			unit.Detachment = null;
			unit.SetDetachmentWeight(-1f);
			this._movementOrder.OnUnitJoinOrLeave(this, unit, true);
			this.OnUnitAttachedOrDetached();
			Action<Formation, Agent> onUnitAttached = this.OnUnitAttached;
			if (onUnitAttached == null)
			{
				return;
			}
			onUnitAttached(this, unit);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00073064 File Offset: 0x00071264
		public void SwitchUnitLocations(Agent firstUnit, Agent secondUnit)
		{
			if (!firstUnit.IsDetachedFromFormation && !secondUnit.IsDetachedFromFormation && (((IFormationUnit)firstUnit).FormationFileIndex != -1 || ((IFormationUnit)secondUnit).FormationFileIndex != -1))
			{
				if (((IFormationUnit)firstUnit).FormationFileIndex == -1)
				{
					this.Arrangement.SwitchUnitLocationsWithUnpositionedUnit(secondUnit, firstUnit);
					return;
				}
				if (((IFormationUnit)secondUnit).FormationFileIndex == -1)
				{
					this.Arrangement.SwitchUnitLocationsWithUnpositionedUnit(firstUnit, secondUnit);
					return;
				}
				this.Arrangement.SwitchUnitLocations(firstUnit, secondUnit);
			}
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000730CE File Offset: 0x000712CE
		public void ForceCalculateCaches()
		{
			this.CacheAverageAndMedianPositionAndVelocity();
			this.CacheClosestEnemyFormation();
			this.CacheFormationIntegrityData();
			this.CacheMovementSpeedOfUnits();
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000730E8 File Offset: 0x000712E8
		public void Tick(float dt)
		{
			float currentTime = Mission.Current.CurrentTime;
			if (this._cachedPositionAndVelocityUpdateTimer.Check(currentTime))
			{
				this.CacheAverageAndMedianPositionAndVelocity();
			}
			if (this._cachedClosestEnemyFormationUpdateTimer.Check(currentTime) || this._cachedClosestEnemyFormation == null || this._cachedClosestEnemyFormation.CountOfUnits == 0)
			{
				this.CacheClosestEnemyFormation();
			}
			if (this._cachedFormationIntegrityDataUpdateTimer.Check(currentTime))
			{
				this.CacheFormationIntegrityData();
			}
			if (this._cachedMovementSpeedUpdateTimer.Check(currentTime))
			{
				this.CacheMovementSpeedOfUnits();
			}
			if (this.Team.HasTeamAi && (this.IsAIControlled || this.Team.IsPlayerSergeant))
			{
				this.AI.Tick();
			}
			else
			{
				this.IsAITickedAfterSplit = true;
			}
			int num = 0;
			while (!this._movementOrder.IsApplicable(this) && num++ < 10)
			{
				this.SetMovementOrder(this._movementOrder.GetSubstituteOrder(this));
			}
			Formation targetFormation = this.TargetFormation;
			if (targetFormation != null && targetFormation.CountOfUnits <= 0)
			{
				this.TargetFormation = null;
			}
			if (this._arrangementOrderTickOccasionallyTimer.Check(currentTime))
			{
				this.ArrangementOrder.TickOccasionally(this);
			}
			if (this._arrangementTickOccasionallyTimer.Check(currentTime))
			{
				this.Arrangement.OnTickOccasionally();
			}
			this._movementOrder.Tick(this);
			WorldPosition worldPosition = this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.None);
			Vec2 direction = this.FacingOrder.GetDirection(this, this._movementOrder._targetAgent);
			if (worldPosition.IsValid || direction.IsValid)
			{
				this.SetPositioning(new WorldPosition?(worldPosition), new Vec2?(direction), null);
			}
			this.TickDetachments(dt);
			if (this._checkTaskForceDetachmentsTimer.Check(currentTime))
			{
				this.CheckTaskForceDetachments();
			}
			Action<Formation> onTick = this.OnTick;
			if (onTick != null)
			{
				onTick(this);
			}
			if (this._hasPendingUnitPositions)
			{
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				this.SetHasPendingUnitPositions(false);
			}
			this.SmoothAverageUnitPosition(dt);
			if (this._isArrangementShapeChanged)
			{
				this._isArrangementShapeChanged = false;
			}
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000732FD File Offset: 0x000714FD
		public void SetHasPendingUnitPositions(bool hasPendingUnitPositions)
		{
			this._hasPendingUnitPositions = hasPendingUnitPositions;
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00073308 File Offset: 0x00071508
		public void JoinDetachment(IDetachment detachment)
		{
			if (!this.Team.DetachmentManager.ContainsDetachment(detachment))
			{
				this.Team.DetachmentManager.MakeDetachment(detachment);
			}
			this._detachments.Add(detachment);
			this.Team.DetachmentManager.OnFormationJoinDetachment(this, detachment);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00073357 File Offset: 0x00071557
		public void FormAttackEntityDetachment(GameEntity targetEntity)
		{
			this.AttackEntityOrderSecondaryDetachment = new AttackEntityOrderSecondaryDetachment(targetEntity);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00073365 File Offset: 0x00071565
		public void LeaveDetachment(IDetachment detachment)
		{
			detachment.OnFormationLeave(this);
			this._detachments.Remove(detachment);
			this.Team.DetachmentManager.OnFormationLeaveDetachment(this, detachment);
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x0007338D File Offset: 0x0007158D
		public void DisbandAttackEntityDetachment()
		{
			if (this.AttackEntityOrderSecondaryDetachment != null)
			{
				this.AttackEntityOrderSecondaryDetachment.Disband(this);
				this.AttackEntityOrderSecondaryDetachment = null;
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000733AC File Offset: 0x000715AC
		public void Rearrange(IFormationArrangement arrangement)
		{
			if (this.Arrangement.GetType() == arrangement.GetType())
			{
				return;
			}
			IFormationArrangement arrangement2 = this.Arrangement;
			this.Arrangement = arrangement;
			arrangement2.RearrangeTo(arrangement);
			arrangement.RearrangeFrom(arrangement2);
			arrangement2.RearrangeTransferUnits(arrangement);
			this.ReapplyFormOrder();
			this._movementOrder.OnArrangementChanged(this);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0007340C File Offset: 0x0007160C
		public void TickForColumnArrangementInitialPositioning(Formation formation)
		{
			if (!this.IsDeployment && (this.CachedFormationIntegrityData.MaxDeviationOfPositionExcludeFarAgents < MathF.Max(this.Distance, this.Interval) || (this.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents < MathF.Max(this.Distance, this.Interval) && this.OrderPosition.DistanceSquared(this.CurrentPosition) > this.Arrangement.RankDepth * this.Arrangement.RankDepth)))
			{
				this.ArrangementOrder.RearrangeAux(this, true);
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x0007349C File Offset: 0x0007169C
		public float CalculateFormationDirectionEnforcingFactorForRank(int rankIndex)
		{
			if (rankIndex == -1)
			{
				return 0f;
			}
			return this.ArrangementOrder.CalculateFormationDirectionEnforcingFactorForRank(rankIndex, this.Arrangement.RankCount);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000734CD File Offset: 0x000716CD
		public void BeginSpawn(int unitCount, bool isMounted)
		{
			this.IsSpawning = true;
			this.OverridenUnitCount = new int?(unitCount);
			this._overridenHasAnyMountedUnit = new bool?(isMounted);
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000734F0 File Offset: 0x000716F0
		public void EndSpawn()
		{
			this.IsSpawning = false;
			this.OverridenUnitCount = null;
			this._overridenHasAnyMountedUnit = null;
			this.Arrangement.UpdateLocalPositionErrors(true);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x0007352B File Offset: 0x0007172B
		public override int GetHashCode()
		{
			return (int)(this.Team.TeamIndex * 10 + this.FormationIndex);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00073542 File Offset: 0x00071742
		internal bool IsUnitDetachedForDebug(Agent unit)
		{
			return this._detachedUnits.Contains(unit);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00073550 File Offset: 0x00071750
		internal IEnumerable<IFormationUnit> GetUnitsToPopWithPriorityFunction(int count, Func<Agent, int> priorityFunction, List<Agent> excludedHeroes, bool excludeBannerman)
		{
			Formation.<>c__DisplayClass398_0 CS$<>8__locals1 = new Formation.<>c__DisplayClass398_0();
			CS$<>8__locals1.excludedHeroes = excludedHeroes;
			CS$<>8__locals1.excludeBannerman = excludeBannerman;
			CS$<>8__locals1.priorityFunction = priorityFunction;
			List<IFormationUnit> list = new List<IFormationUnit>();
			if (count <= 0)
			{
				return list;
			}
			CS$<>8__locals1.selectCondition = (Agent agent) => !CS$<>8__locals1.excludedHeroes.Contains(agent3) && (!CS$<>8__locals1.excludeBannerman || agent3.Banner == null);
			List<Agent> list2 = (from unit in this._arrangement.GetAllUnits().Concat<IFormationUnit>(this._detachedUnits).Where<IFormationUnit>(delegate(IFormationUnit unit)
				{
					Agent agent3;
					return (agent3 = unit as Agent) != null && CS$<>8__locals1.selectCondition(agent3);
				})
				select unit as Agent).ToList<Agent>();
			if (list2.IsEmpty<Agent>())
			{
				return list;
			}
			int num = count;
			CS$<>8__locals1.bestFit = int.MaxValue;
			while (num > 0 && CS$<>8__locals1.bestFit > 0 && list2.Count > 0)
			{
				Formation.<>c__DisplayClass398_1 CS$<>8__locals2 = new Formation.<>c__DisplayClass398_1();
				Formation.<>c__DisplayClass398_0 CS$<>8__locals3 = CS$<>8__locals1;
				IEnumerable<Agent> enumerable = list2;
				Func<Agent, int> func;
				if ((func = CS$<>8__locals1.<>9__3) == null)
				{
					func = (CS$<>8__locals1.<>9__3 = (Agent unit) => CS$<>8__locals1.priorityFunction(unit));
				}
				CS$<>8__locals3.bestFit = enumerable.Max<Agent>(func);
				Formation.<>c__DisplayClass398_1 CS$<>8__locals4 = CS$<>8__locals2;
				Func<IFormationUnit, bool> func2;
				if ((func2 = CS$<>8__locals1.<>9__4) == null)
				{
					func2 = (CS$<>8__locals1.<>9__4 = delegate(IFormationUnit unit)
					{
						Agent agent2;
						return (agent2 = unit as Agent) != null && CS$<>8__locals1.selectCondition(agent2) && CS$<>8__locals1.priorityFunction(agent2) == CS$<>8__locals1.bestFit;
					});
				}
				CS$<>8__locals4.bestFitCondition = func2;
				int num2 = Math.Min(num, this._arrangement.GetAllUnits().Count<IFormationUnit>((IFormationUnit unit) => CS$<>8__locals2.bestFitCondition(unit)));
				if (num2 > 0)
				{
					IEnumerable<IFormationUnit> toPop2 = this._arrangement.GetUnitsToPopWithCondition(num2, CS$<>8__locals2.bestFitCondition);
					if (!toPop2.IsEmpty<IFormationUnit>())
					{
						list.AddRange(toPop2);
						num -= toPop2.Count<IFormationUnit>();
						list2.RemoveAll((Agent unit) => toPop2.Contains(unit));
					}
				}
				if (num > 0)
				{
					IEnumerable<Agent> toPop3 = this._looseDetachedUnits.Where<Agent>((Agent agent) => CS$<>8__locals2.bestFitCondition(agent)).Take<Agent>(num);
					if (!toPop3.IsEmpty<Agent>())
					{
						list.AddRange(toPop3);
						num -= toPop3.Count<Agent>();
						list2.RemoveAll((Agent unit) => toPop3.Contains(unit));
					}
				}
				if (num > 0)
				{
					IEnumerable<Agent> toPop = this._detachedUnits.Where<Agent>((Agent agent) => CS$<>8__locals2.bestFitCondition(agent)).Take<Agent>(num);
					if (!toPop.IsEmpty<Agent>())
					{
						list.AddRange(toPop);
						num -= toPop.Count<Agent>();
						list2.RemoveAll((Agent unit) => toPop.Contains(unit));
					}
				}
			}
			return list;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000737E0 File Offset: 0x000719E0
		internal void TransferUnitsWithPriorityFunction(Formation target, int unitCount, Func<Agent, int> priorityFunction, bool excludeBannerman, List<Agent> excludedAgents)
		{
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print(string.Concat(new object[]
			{
				this.Team.Side.ToString(),
				" ",
				this.FormationIndex.GetName(),
				" transfers ",
				unitCount,
				" units to ",
				target.FormationIndex.GetName()
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			if (unitCount == 0)
			{
				return;
			}
			if (target.CountOfUnits == 0)
			{
				target.CopyOrdersFrom(this);
				target.SetPositioning(new WorldPosition?(this._orderPosition), new Vec2?(this.Direction), new int?(this.UnitSpacing));
			}
			foreach (IFormationUnit formationUnit in new List<IFormationUnit>(this.GetUnitsToPopWithPriorityFunction(unitCount, priorityFunction, excludedAgents, excludeBannerman)))
			{
				((Agent)formationUnit).Formation = target;
			}
			this.Team.TriggerOnFormationsChanged(this);
			this.Team.TriggerOnFormationsChanged(target);
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000739F0 File Offset: 0x00071BF0
		private IFormationUnit GetClosestUnitToAux(Vec2 position, MBReadOnlyList<IFormationUnit> unitsWithSpaces, float? maxDistance)
		{
			if (unitsWithSpaces == null)
			{
				unitsWithSpaces = this.Arrangement.GetAllUnits();
			}
			IFormationUnit formationUnit = null;
			float num = ((maxDistance != null) ? (maxDistance.Value * maxDistance.Value) : float.MaxValue);
			for (int i = 0; i < unitsWithSpaces.Count; i++)
			{
				IFormationUnit formationUnit2 = unitsWithSpaces[i];
				if (formationUnit2 != null)
				{
					float num2 = ((Agent)formationUnit2).Position.AsVec2.DistanceSquared(position);
					if (num > num2)
					{
						num = num2;
						formationUnit = formationUnit2;
					}
				}
			}
			return formationUnit;
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00073A78 File Offset: 0x00071C78
		private void CopyOrdersFrom(Formation target)
		{
			this.SetMovementOrder(target._movementOrder);
			this.SetFormOrder(target.FormOrder, true);
			this.SetPositioning(null, null, new int?(target.UnitSpacing));
			this.SetRidingOrder(target.RidingOrder);
			this.SetFiringOrder(target.FiringOrder);
			this.SetControlledByAI(target.IsAIControlled || !target.Team.IsPlayerGeneral, false);
			if (target.AI.Side != FormationAI.BehaviorSide.BehaviorSideNotSet)
			{
				this.AI.Side = target.AI.Side;
			}
			this.SetMovementOrder(target._movementOrder);
			this.TargetFormation = target.TargetFormation;
			this.FacingOrder = target.FacingOrder;
			this.SetArrangementOrder(target.ArrangementOrder);
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00073B50 File Offset: 0x00071D50
		private void TickDetachments(float dt)
		{
			if (!this.IsDeployment)
			{
				for (int i = this._detachments.Count - 1; i >= 0; i--)
				{
					IDetachment detachment = this._detachments[i];
					UsableMachine usableMachine = detachment as UsableMachine;
					if (((usableMachine != null) ? usableMachine.Ai : null) != null)
					{
						usableMachine.Ai.Tick(null, (usableMachine.UserFormations.Count > 1) ? this : null, this.Team, dt);
						if (usableMachine.Ai.HasActionCompleted || (usableMachine.IsDisabledForBattleSideAI(this.Team.Side) && usableMachine.ShouldAutoLeaveDetachmentWhenDisabled(this.Team.Side)))
						{
							this.LeaveDetachment(detachment);
						}
					}
				}
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00073C08 File Offset: 0x00071E08
		private void CheckTaskForceDetachments()
		{
			this._temporaryTaskForceDetachmentList.Clear();
			if (this._movementOrder.MovementState != MovementOrder.MovementStateEnum.Hold && this._movementOrder.MovementState != MovementOrder.MovementStateEnum.StandGround)
			{
				using (Dictionary<Agent, TaskForceDetachment>.Enumerator enumerator = this._taskForces.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Agent, TaskForceDetachment> keyValuePair = enumerator.Current;
						this._temporaryTaskForceDetachmentList.Add(keyValuePair.Value);
					}
					goto IL_00B7;
				}
			}
			foreach (KeyValuePair<Agent, TaskForceDetachment> keyValuePair2 in this._taskForces)
			{
				if (keyValuePair2.Value.CalculateShouldBeDisbanded())
				{
					this._temporaryTaskForceDetachmentList.Add(keyValuePair2.Value);
				}
			}
			IL_00B7:
			foreach (TaskForceDetachment taskForceDetachment in this._temporaryTaskForceDetachmentList)
			{
				this.LeaveDetachment(taskForceDetachment);
				this._taskForces.Remove(taskForceDetachment.TargetAgent);
			}
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00073D40 File Offset: 0x00071F40
		[Conditional("DEBUG")]
		private void TickOrderDebug()
		{
			WorldPosition cachedMedianPosition = this.CachedMedianPosition;
			WorldPosition worldPosition = this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
			cachedMedianPosition.SetVec2(this.CachedAveragePosition);
			if (worldPosition.IsValid)
			{
				if (!this._movementOrder.GetPosition(this).IsValid)
				{
					if (this.AI != null)
					{
						BehaviorComponent activeBehavior = this.AI.ActiveBehavior;
						return;
					}
				}
				else if (this.AI != null)
				{
					BehaviorComponent activeBehavior2 = this.AI.ActiveBehavior;
					return;
				}
			}
			else if (this.AI != null)
			{
				BehaviorComponent activeBehavior3 = this.AI.ActiveBehavior;
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00073DC6 File Offset: 0x00071FC6
		[Conditional("DEBUG")]
		private void TickDebug(float dt)
		{
			if (!MBDebug.IsDisplayingHighLevelAI)
			{
				return;
			}
			if (!this.IsSimulationFormation && this._movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.FollowEntity)
			{
				string name = this._movementOrder.TargetEntity.Name;
			}
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00073DF7 File Offset: 0x00071FF7
		private void OnUnitAttachedOrDetached()
		{
			this.ReapplyFormOrder();
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00073DFF File Offset: 0x00071FFF
		[Conditional("DEBUG")]
		private void DebugAssertDetachments()
		{
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00073E01 File Offset: 0x00072001
		private void SetOrderPosition(WorldPosition pos)
		{
			this._orderPosition = pos;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00073E0A File Offset: 0x0007200A
		private int GetHeroPointForCaptainSelection(Agent agent)
		{
			return agent.Character.Level + 100 * agent.Character.GetSkillValue(DefaultSkills.Charm);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00073E2B File Offset: 0x0007202B
		private void OnCaptainChanged()
		{
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.UpdateAgentProperties();
			}, null);
			Mission.Current.OnFormationCaptainChanged(this);
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00073E5E File Offset: 0x0007205E
		private void UpdateAgentDrivenPropertiesBasedOnOrderDefensiveness()
		{
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.Defensiveness = (float)this._formationOrderDefensivenessFactor;
			}, null);
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00073E74 File Offset: 0x00072074
		private void ResetAux()
		{
			if (this._detachments != null)
			{
				for (int i = this._detachments.Count - 1; i >= 0; i--)
				{
					this.LeaveDetachment(this._detachments[i]);
				}
			}
			else
			{
				this._detachments = new MBList<IDetachment>();
			}
			if (this._taskForces != null)
			{
				this._taskForces.Clear();
			}
			else
			{
				this._taskForces = new Dictionary<Agent, TaskForceDetachment>();
			}
			if (this._temporaryTaskForceDetachmentList == null)
			{
				this._temporaryTaskForceDetachmentList = new MBList<TaskForceDetachment>();
			}
			this._detachedUnits = new MBList<Agent>();
			this._looseDetachedUnits = new MBList<Agent>();
			this.AttackEntityOrderSecondaryDetachment = null;
			this._tempAgentList = new MBList<Agent>();
			this.AI = new FormationAI(this);
			this.QuerySystem = new FormationQuerySystem(this);
			this.SetPositioning(null, new Vec2?(Vec2.Forward), new int?(1));
			this.SetMovementOrder(MovementOrder.MovementOrderStop);
			if (this._overridenHasAnyMountedUnit != null)
			{
				bool? overridenHasAnyMountedUnit = this._overridenHasAnyMountedUnit;
				bool flag = true;
				if ((overridenHasAnyMountedUnit.GetValueOrDefault() == flag) & (overridenHasAnyMountedUnit != null))
				{
					this.SetArrangementOrder(ArrangementOrder.ArrangementOrderSkein);
					goto IL_012A;
				}
			}
			this.SetFormOrder(FormOrder.FormOrderWide, true);
			this.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			IL_012A:
			this.SetRidingOrder(RidingOrder.RidingOrderFree);
			this.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			this.Width = 0f * (this.Interval + this.UnitDiameter) + this.UnitDiameter;
			this.HasBeenPositioned = false;
			this._currentSpawnIndex = 0;
			this.IsPlayerTroopInFormation = false;
			this.HasPlayerControlledTroop = false;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00073FFD File Offset: 0x000721FD
		private void ResetForSimulation()
		{
			this.Arrangement.Reset();
			this.ResetAux();
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00074010 File Offset: 0x00072210
		private void TryRelocatePlayerUnit()
		{
			if (this.HasPlayerControlledTroop || this.IsPlayerTroopInFormation)
			{
				IFormationUnit playerUnit = this.Arrangement.GetPlayerUnit();
				if (playerUnit != null && playerUnit.FormationFileIndex >= 0 && playerUnit.FormationRankIndex >= 0)
				{
					this.Arrangement.SwitchUnitLocationsWithBackMostUnit(playerUnit);
				}
			}
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x0007405C File Offset: 0x0007225C
		private void ReapplyFormOrder()
		{
			FormOrder formOrder = this.FormOrder;
			if (this.FormOrder.OrderEnum == FormOrder.FormOrderEnum.Custom && this.ArrangementOrder.OrderEnum != ArrangementOrder.ArrangementOrderEnum.Circle)
			{
				formOrder.CustomFlankWidth = this.Arrangement.FlankWidth;
			}
			this.SetFormOrder(formOrder, false);
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000740A5 File Offset: 0x000722A5
		private float CalculateDesiredWidth()
		{
			return MathF.Max(0f, (float)(this._desiredFileCount - 1) * (this.UnitDiameter + this.Interval) + this.UnitDiameter);
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000740D0 File Offset: 0x000722D0
		private void CalculateLogicalClass()
		{
			int num = 0;
			FormationClass formationClass = FormationClass.NumberOfAllFormations;
			for (int i = 0; i < this._logicalClassCounts.Length; i++)
			{
				FormationClass formationClass2 = (FormationClass)i;
				int num2 = this._logicalClassCounts[i];
				if (num2 > num)
				{
					num = num2;
					formationClass = formationClass2;
				}
			}
			this._logicalClass = formationClass;
			if (this._logicalClass != FormationClass.NumberOfAllFormations)
			{
				this.RepresentativeClass = this._logicalClass;
			}
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00074128 File Offset: 0x00072328
		private void SmoothAverageUnitPosition(float dt)
		{
			this._smoothedAverageUnitPosition = ((!this._smoothedAverageUnitPosition.IsValid) ? this.CachedAveragePosition : Vec2.Lerp(this._smoothedAverageUnitPosition, this.CachedAveragePosition, dt * 3f));
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x0007415D File Offset: 0x0007235D
		private void Arrangement_OnWidthChanged()
		{
			Action<Formation> onWidthChanged = this.OnWidthChanged;
			if (onWidthChanged == null)
			{
				return;
			}
			onWidthChanged(this);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00074170 File Offset: 0x00072370
		private void Arrangement_OnShapeChanged()
		{
			this._orderLocalAveragePositionIsDirty = true;
			this._isArrangementShapeChanged = true;
			if (!GameNetwork.IsMultiplayer)
			{
				this.TryRelocatePlayerUnit();
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00074190 File Offset: 0x00072390
		public static float GetLastSimulatedFormationsOccupationWidthIfLesserThanActualWidth(Formation simulationFormation)
		{
			float occupationWidth = simulationFormation.Arrangement.GetOccupationWidth(simulationFormation.OverridenUnitCount.GetValueOrDefault());
			if (simulationFormation.Width > occupationWidth)
			{
				return occupationWidth;
			}
			return -1f;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000741C8 File Offset: 0x000723C8
		public static List<WorldFrame> GetFormationFramesForBeforeFormationCreation(float width, int manCount, bool areMounted, WorldPosition spawnOrigin, Mat3 spawnRotation)
		{
			List<Formation.AgentArrangementData> list = new List<Formation.AgentArrangementData>();
			Formation formation = new Formation(null, -1);
			formation.SetOrderPosition(spawnOrigin);
			formation.Direction = spawnRotation.f.AsVec2.Normalized();
			LineFormation lineFormation = new LineFormation(formation, true);
			lineFormation.Width = width;
			for (int i = 0; i < manCount; i++)
			{
				list.Add(new Formation.AgentArrangementData(i, lineFormation));
			}
			lineFormation.OnFormationFrameChanged(false);
			foreach (Formation.AgentArrangementData agentArrangementData in list)
			{
				lineFormation.AddUnit(agentArrangementData);
			}
			List<WorldFrame> list2 = new List<WorldFrame>();
			int cachedOrderedAndAvailableUnitPositionIndicesCount = lineFormation.GetCachedOrderedAndAvailableUnitPositionIndicesCount();
			for (int j = 0; j < cachedOrderedAndAvailableUnitPositionIndicesCount; j++)
			{
				Vec2i cachedOrderedAndAvailableUnitPositionIndexAt = lineFormation.GetCachedOrderedAndAvailableUnitPositionIndexAt(j);
				WorldPosition globalPositionAtIndex = lineFormation.GetGlobalPositionAtIndex(cachedOrderedAndAvailableUnitPositionIndexAt.X, cachedOrderedAndAvailableUnitPositionIndexAt.Y);
				list2.Add(new WorldFrame(spawnRotation, globalPositionAtIndex));
			}
			return list2;
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000742CC File Offset: 0x000724CC
		public static float GetDefaultUnitDiameter(bool isMounted)
		{
			if (isMounted)
			{
				return ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.QuadrupedalRadius) * 2f;
			}
			return ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius) * 2f;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000742F4 File Offset: 0x000724F4
		public static float GetDefaultMinimumUnitInterval(bool isMounted)
		{
			if (!isMounted)
			{
				return Formation.InfantryInterval(0);
			}
			return Formation.CavalryInterval(0);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00074306 File Offset: 0x00072506
		public static float GetDefaultUnitInterval(bool isMounted, int unitSpacing)
		{
			if (!isMounted)
			{
				return Formation.InfantryInterval(unitSpacing);
			}
			return Formation.CavalryInterval(unitSpacing);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x00074318 File Offset: 0x00072518
		public static float GetDefaultMinimumUnitDistance(bool isMounted)
		{
			if (!isMounted)
			{
				return Formation.InfantryDistance(0);
			}
			return Formation.CavalryDistance(0);
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x0007432A File Offset: 0x0007252A
		public static float GetDefaultUnitDistance(bool isMounted, int unitSpacing)
		{
			if (!isMounted)
			{
				return Formation.InfantryDistance(unitSpacing);
			}
			return Formation.CavalryDistance(unitSpacing);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x0007433C File Offset: 0x0007253C
		public static float GetDefaultFileWidth(int fileUnitCount, int unitSpacing, bool isMounted)
		{
			float defaultUnitInterval = Formation.GetDefaultUnitInterval(isMounted, unitSpacing);
			float defaultUnitDiameter = Formation.GetDefaultUnitDiameter(isMounted);
			return (float)(fileUnitCount - 1) * (defaultUnitInterval + defaultUnitDiameter);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00074360 File Offset: 0x00072560
		public static float GetDefaultRankDepth(int rankUnitCount, int unitSpacing, bool isMounted)
		{
			float defaultUnitDistance = Formation.GetDefaultUnitDistance(isMounted, unitSpacing);
			float defaultUnitDiameter = Formation.GetDefaultUnitDiameter(isMounted);
			return (float)(rankUnitCount - 1) * (defaultUnitDistance + defaultUnitDiameter);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00074384 File Offset: 0x00072584
		public static float InfantryInterval(int unitSpacing)
		{
			return 0.38f * (float)unitSpacing;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x0007438E File Offset: 0x0007258E
		public static float CavalryInterval(int unitSpacing)
		{
			return 0.18f + 0.32f * (float)unitSpacing;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0007439E File Offset: 0x0007259E
		public static float InfantryDistance(int unitSpacing)
		{
			return 0.4f * (float)unitSpacing;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000743A8 File Offset: 0x000725A8
		public static float CavalryDistance(int unitSpacing)
		{
			return 1.7f + 0.3f * (float)unitSpacing;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000743B8 File Offset: 0x000725B8
		public static bool IsDefenseRelatedAIDrivenComponent(DrivenProperty drivenProperty)
		{
			return drivenProperty == DrivenProperty.AIDecideOnAttackChance || drivenProperty == DrivenProperty.AIAttackOnDecideChance || drivenProperty == DrivenProperty.AIAttackOnParryChance || drivenProperty == DrivenProperty.AiUseShieldAgainstEnemyMissileProbability || drivenProperty == DrivenProperty.AiDefendWithShieldDecisionChanceValue;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000743D4 File Offset: 0x000725D4
		private static void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, IFormationArrangement arrangement, float width, int unitSpacing, int unitCount, bool isMounted, int index, out WorldPosition? unitPosition, out Vec2? unitDirection, out float actualWidth)
		{
			unitPosition = null;
			unitDirection = null;
			if (simulationFormation == null)
			{
				if (Formation._simulationFormationTemp == null || Formation._simulationFormationUniqueIdentifier != index)
				{
					Formation._simulationFormationTemp = new Formation(null, -1);
				}
				simulationFormation = Formation._simulationFormationTemp;
			}
			object simulationFormationLock = simulationFormation.SimulationFormationLock;
			lock (simulationFormationLock)
			{
				if (simulationFormation.UnitSpacing == unitSpacing && (MathF.Abs(simulationFormation.Width - width + 1E-05f) < simulationFormation.Interval + simulationFormation.UnitDiameter - 1E-05f || (width < simulationFormation.MinimumWidth && MathF.Abs(simulationFormation.Width - simulationFormation.MinimumWidth) < 1E-05f)) && simulationFormation.OrderPositionIsValid)
				{
					Vec3 orderGroundPositionMT = simulationFormation.OrderGroundPositionMT;
					WorldPosition worldPosition = formationPosition;
					Vec3 vec = worldPosition.GetGroundVec3MT();
					if (orderGroundPositionMT.NearlyEquals(in vec, 0.1f) && simulationFormation.Direction.NearlyEquals(formationDirection, 0.1f) && !(simulationFormation.Arrangement.GetType() != arrangement.GetType()))
					{
						goto IL_0273;
					}
				}
				simulationFormation._overridenHasAnyMountedUnit = new bool?(isMounted);
				simulationFormation.ResetForSimulation();
				simulationFormation.SetPositioning(null, null, new int?(unitSpacing));
				simulationFormation.OverridenUnitCount = new int?(unitCount);
				simulationFormation.SetPositioning(new WorldPosition?(formationPosition), new Vec2?(formationDirection), null);
				simulationFormation.Rearrange(arrangement.Clone(simulationFormation));
				simulationFormation.Arrangement.DeepCopyFrom(arrangement);
				simulationFormation.Width = width;
				Formation._simulationFormationUniqueIdentifier = index;
				ColumnFormation columnFormation;
				if ((columnFormation = arrangement as ColumnFormation) != null && arrangement.RankCount > 1)
				{
					Vec3 vec = ((columnFormation.Vanguard ?? arrangement.GetUnit(columnFormation.VanguardFileIndex, 0)) as Agent).Position;
					Vec2 asVec = vec.AsVec2;
					vec = (arrangement.GetUnit(columnFormation.VanguardFileIndex, 1) as Agent).Position;
					Vec2 asVec2 = vec.AsVec2;
					Vec2 vec2 = (asVec - asVec2).Normalized();
					WorldPosition worldPosition = formationPosition;
					Vec2 vec3 = (worldPosition.AsVec2 - asVec).Normalized();
					if (arrangement.IsTurnBackwardsNecessary(asVec, new WorldPosition?(formationPosition), vec2, true, new Vec2?(vec3)))
					{
						(simulationFormation.Arrangement as ColumnFormation).UnitPositionsOnVanguardFileIndex.Reverse();
					}
				}
				IL_0273:
				actualWidth = simulationFormation.Width;
				if (width >= actualWidth)
				{
					Vec2? vec4 = simulationFormation.Arrangement.GetLocalPositionOfUnitOrDefault(unitIndex);
					if (vec4 == null)
					{
						vec4 = simulationFormation.Arrangement.CreateNewPosition(unitIndex);
					}
					if (vec4 != null)
					{
						Vec2 vec5 = simulationFormation.Direction.TransformToParentUnitF(vec4.Value);
						WorldPosition worldPosition2 = simulationFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
						worldPosition2.SetVec2(worldPosition2.AsVec2 + vec5);
						unitPosition = new WorldPosition?(worldPosition2);
						unitDirection = new Vec2?(formationDirection);
					}
				}
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00074714 File Offset: 0x00072914
		private static IEnumerable<ValueTuple<WorldPosition, Vec2>> GetUnavailableUnitPositionsAccordingToNewOrder(Formation formation, Formation simulationFormation, WorldPosition position, Vec2 direction, IFormationArrangement arrangement, float width, int unitSpacing)
		{
			if (simulationFormation == null)
			{
				if (Formation._simulationFormationTemp == null || Formation._simulationFormationUniqueIdentifier != formation.Index)
				{
					Formation._simulationFormationTemp = new Formation(null, -1);
				}
				simulationFormation = Formation._simulationFormationTemp;
			}
			object obj = simulationFormation.SimulationFormationLock;
			lock (obj)
			{
				if (simulationFormation.UnitSpacing == unitSpacing && MathF.Abs(simulationFormation.Width - width) < simulationFormation.Interval + simulationFormation.UnitDiameter && simulationFormation.OrderPositionIsValid)
				{
					Vec3 orderGroundPositionMT = simulationFormation.OrderGroundPositionMT;
					Vec3 groundVec3MT = position.GetGroundVec3MT();
					if (orderGroundPositionMT.NearlyEquals(in groundVec3MT, 0.1f) && simulationFormation.Direction.NearlyEquals(direction, 0.1f) && !(simulationFormation.Arrangement.GetType() != arrangement.GetType()))
					{
						goto IL_0233;
					}
				}
				simulationFormation._overridenHasAnyMountedUnit = new bool?(formation.HasAnyMountedUnit);
				simulationFormation.ResetForSimulation();
				simulationFormation.SetPositioning(null, null, new int?(unitSpacing));
				simulationFormation.OverridenUnitCount = new int?(formation.CountOfUnitsWithoutDetachedOnes);
				simulationFormation.SetPositioning(new WorldPosition?(position), new Vec2?(direction), null);
				simulationFormation.Rearrange(arrangement.Clone(simulationFormation));
				simulationFormation.Arrangement.DeepCopyFrom(arrangement);
				simulationFormation.Width = width;
				Formation._simulationFormationUniqueIdentifier = formation.Index;
				IL_0233:
				IEnumerable<Vec2> unavailableUnitPositions = simulationFormation.Arrangement.GetUnavailableUnitPositions();
				foreach (Vec2 vec in unavailableUnitPositions)
				{
					Vec2 vec2 = simulationFormation.Direction.TransformToParentUnitF(vec);
					WorldPosition worldPosition = simulationFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
					worldPosition.SetVec2(worldPosition.AsVec2 + vec2);
					yield return new ValueTuple<WorldPosition, Vec2>(worldPosition, direction);
				}
				IEnumerator<Vec2> enumerator = null;
			}
			obj = null;
			yield break;
			yield break;
		}

		// Token: 0x04000B06 RID: 2822
		public const float AveragePositionCalculatePeriod = 0.1f;

		// Token: 0x04000B07 RID: 2823
		public const int MinimumUnitSpacing = 0;

		// Token: 0x04000B08 RID: 2824
		public const int RetreatPositionDistanceCacheCount = 2;

		// Token: 0x04000B09 RID: 2825
		public const float RetreatPositionCacheUseDistanceSquared = 400f;

		// Token: 0x04000B0A RID: 2826
		private static Formation _simulationFormationTemp;

		// Token: 0x04000B0B RID: 2827
		private static int _simulationFormationUniqueIdentifier;

		// Token: 0x04000B16 RID: 2838
		public readonly Team Team;

		// Token: 0x04000B17 RID: 2839
		public readonly int Index;

		// Token: 0x04000B18 RID: 2840
		public readonly FormationClass FormationIndex;

		// Token: 0x04000B19 RID: 2841
		public Banner Banner;

		// Token: 0x04000B1A RID: 2842
		public bool HasBeenPositioned;

		// Token: 0x04000B1B RID: 2843
		public Vec2? ReferencePosition;

		// Token: 0x04000B1D RID: 2845
		private bool _logicalClassNeedsUpdate;

		// Token: 0x04000B1E RID: 2846
		private FormationClass _logicalClass = FormationClass.NumberOfAllFormations;

		// Token: 0x04000B1F RID: 2847
		private readonly int[] _logicalClassCounts = new int[4];

		// Token: 0x04000B20 RID: 2848
		private Agent _playerOwner;

		// Token: 0x04000B21 RID: 2849
		private string _bannerCode;

		// Token: 0x04000B23 RID: 2851
		private bool _enforceNotSplittableByAI = true;

		// Token: 0x04000B24 RID: 2852
		private WorldPosition _orderPosition;

		// Token: 0x04000B27 RID: 2855
		private Vec2 _orderLocalAveragePosition;

		// Token: 0x04000B28 RID: 2856
		private bool _orderLocalAveragePositionIsDirty = true;

		// Token: 0x04000B29 RID: 2857
		private int _formationOrderDefensivenessFactor = 2;

		// Token: 0x04000B2A RID: 2858
		private MovementOrder _movementOrder;

		// Token: 0x04000B2B RID: 2859
		private Timer _arrangementOrderTickOccasionallyTimer;

		// Token: 0x04000B2C RID: 2860
		private Timer _arrangementTickOccasionallyTimer;

		// Token: 0x04000B2D RID: 2861
		private Agent _captain;

		// Token: 0x04000B2E RID: 2862
		private Vec2 _smoothedAverageUnitPosition = Vec2.Invalid;

		// Token: 0x04000B2F RID: 2863
		private MBList<IDetachment> _detachments;

		// Token: 0x04000B30 RID: 2864
		private IFormationArrangement _arrangement;

		// Token: 0x04000B31 RID: 2865
		private int[] _agentIndicesCache;

		// Token: 0x04000B32 RID: 2866
		private MBList<Agent> _detachedUnits;

		// Token: 0x04000B33 RID: 2867
		private int _undetachableNonPlayerUnitCount;

		// Token: 0x04000B34 RID: 2868
		private MBList<Agent> _looseDetachedUnits;

		// Token: 0x04000B35 RID: 2869
		private bool? _overridenHasAnyMountedUnit;

		// Token: 0x04000B36 RID: 2870
		private bool _isArrangementShapeChanged;

		// Token: 0x04000B37 RID: 2871
		private bool _hasPendingUnitPositions;

		// Token: 0x04000B38 RID: 2872
		private int _currentSpawnIndex;

		// Token: 0x04000B39 RID: 2873
		private int _desiredFileCount;

		// Token: 0x04000B3A RID: 2874
		private MBList<Agent> _tempAgentList;

		// Token: 0x04000B3B RID: 2875
		private Agent _lastMedianAgent;

		// Token: 0x04000B3C RID: 2876
		private MBList<TaskForceDetachment> _temporaryTaskForceDetachmentList;

		// Token: 0x04000B3D RID: 2877
		private Dictionary<Agent, TaskForceDetachment> _taskForces;

		// Token: 0x04000B42 RID: 2882
		private Formation _targetFormation;

		// Token: 0x04000B45 RID: 2885
		private Timer _cachedFormationIntegrityDataUpdateTimer;

		// Token: 0x04000B49 RID: 2889
		private Timer _cachedPositionAndVelocityUpdateTimer;

		// Token: 0x04000B4A RID: 2890
		private float _lastAveragePositionCacheTime;

		// Token: 0x04000B4C RID: 2892
		private Timer _cachedMovementSpeedUpdateTimer;

		// Token: 0x04000B4E RID: 2894
		private Formation _cachedClosestEnemyFormation;

		// Token: 0x04000B4F RID: 2895
		private Timer _cachedClosestEnemyFormationUpdateTimer;

		// Token: 0x04000B50 RID: 2896
		private Timer _checkTaskForceDetachmentsTimer;

		// Token: 0x0200052C RID: 1324
		private class AgentArrangementData : IFormationUnit
		{
			// Token: 0x17000A6D RID: 2669
			// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x000F42DD File Offset: 0x000F24DD
			// (set) Token: 0x06003CD1 RID: 15569 RVA: 0x000F42E5 File Offset: 0x000F24E5
			public IFormationArrangement Formation { get; private set; }

			// Token: 0x17000A6E RID: 2670
			// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x000F42EE File Offset: 0x000F24EE
			// (set) Token: 0x06003CD3 RID: 15571 RVA: 0x000F42F6 File Offset: 0x000F24F6
			public int FormationFileIndex { get; set; } = -1;

			// Token: 0x17000A6F RID: 2671
			// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x000F42FF File Offset: 0x000F24FF
			// (set) Token: 0x06003CD5 RID: 15573 RVA: 0x000F4307 File Offset: 0x000F2507
			public int FormationRankIndex { get; set; } = -1;

			// Token: 0x17000A70 RID: 2672
			// (get) Token: 0x06003CD6 RID: 15574 RVA: 0x000F4310 File Offset: 0x000F2510
			public IFormationUnit FollowedUnit { get; }

			// Token: 0x17000A71 RID: 2673
			// (get) Token: 0x06003CD7 RID: 15575 RVA: 0x000F4318 File Offset: 0x000F2518
			public bool IsShieldUsageEncouraged
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000A72 RID: 2674
			// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x000F431B File Offset: 0x000F251B
			public bool IsPlayerUnit
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06003CD9 RID: 15577 RVA: 0x000F431E File Offset: 0x000F251E
			public AgentArrangementData(int index, IFormationArrangement arrangement)
			{
				this.Formation = arrangement;
			}
		}

		// Token: 0x0200052D RID: 1325
		public struct FormationIntegrityDataGroup
		{
			// Token: 0x06003CDA RID: 15578 RVA: 0x000F433B File Offset: 0x000F253B
			public FormationIntegrityDataGroup(Vec2 averageVelocityExcludeFarAgents, float deviationOfPositionsExcludeFarAgents, float maxDeviationOfPositionExcludeFarAgents, float averageMaxUnlimitedSpeedExcludeFarAgents)
			{
				this.AverageVelocityExcludeFarAgents = averageVelocityExcludeFarAgents;
				this.DeviationOfPositionsExcludeFarAgents = deviationOfPositionsExcludeFarAgents;
				this.MaxDeviationOfPositionExcludeFarAgents = maxDeviationOfPositionExcludeFarAgents;
				this.AverageMaxUnlimitedSpeedExcludeFarAgents = averageMaxUnlimitedSpeedExcludeFarAgents;
			}

			// Token: 0x04001DA1 RID: 7585
			public Vec2 AverageVelocityExcludeFarAgents;

			// Token: 0x04001DA2 RID: 7586
			public float DeviationOfPositionsExcludeFarAgents;

			// Token: 0x04001DA3 RID: 7587
			public float MaxDeviationOfPositionExcludeFarAgents;

			// Token: 0x04001DA4 RID: 7588
			public float AverageMaxUnlimitedSpeedExcludeFarAgents;
		}

		// Token: 0x0200052E RID: 1326
		public class RetreatPositionCacheSystem
		{
			// Token: 0x06003CDB RID: 15579 RVA: 0x000F435A File Offset: 0x000F255A
			public RetreatPositionCacheSystem(int cacheCount)
			{
				this._retreatPositionDistance = new List<ValueTuple<Vec2, WorldPosition>>(2);
			}

			// Token: 0x06003CDC RID: 15580 RVA: 0x000F4370 File Offset: 0x000F2570
			public WorldPosition GetRetreatPositionFromCache(Vec2 agentPosition)
			{
				for (int i = this._retreatPositionDistance.Count - 1; i >= 0; i--)
				{
					if (this._retreatPositionDistance[i].Item1.DistanceSquared(agentPosition) < 400f)
					{
						return this._retreatPositionDistance[i].Item2;
					}
				}
				return WorldPosition.Invalid;
			}

			// Token: 0x06003CDD RID: 15581 RVA: 0x000F43CD File Offset: 0x000F25CD
			public void AddNewPositionToCache(Vec2 agentPostion, WorldPosition retreatingPosition)
			{
				if (this._retreatPositionDistance.Count >= 2)
				{
					this._retreatPositionDistance.RemoveAt(0);
				}
				this._retreatPositionDistance.Add(new ValueTuple<Vec2, WorldPosition>(agentPostion, retreatingPosition));
			}

			// Token: 0x04001DA5 RID: 7589
			private List<ValueTuple<Vec2, WorldPosition>> _retreatPositionDistance;
		}
	}
}
