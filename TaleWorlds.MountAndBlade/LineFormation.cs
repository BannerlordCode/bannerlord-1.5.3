using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000149 RID: 329
	public class LineFormation : IFormationArrangement
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0002CD56 File Offset: 0x0002AF56
		protected int FileCount
		{
			get
			{
				return this._units2D.Count1;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0002CD63 File Offset: 0x0002AF63
		public int RankCount
		{
			get
			{
				return this._units2D.Count2;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0002CD70 File Offset: 0x0002AF70
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x0002CD78 File Offset: 0x0002AF78
		public bool AreLocalPositionsDirty { protected get; set; }

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0002CD81 File Offset: 0x0002AF81
		protected float Interval
		{
			get
			{
				return this.owner.Interval;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0002CD8E File Offset: 0x0002AF8E
		public virtual float IntervalMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0002CD95 File Offset: 0x0002AF95
		protected float Distance
		{
			get
			{
				return this.owner.Distance;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0002CDA2 File Offset: 0x0002AFA2
		public virtual float DistanceMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0002CDA9 File Offset: 0x0002AFA9
		protected float UnitDiameter
		{
			get
			{
				return this.owner.UnitDiameter;
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0002CDB6 File Offset: 0x0002AFB6
		public int GetFileCountFromWidth(float width)
		{
			return MathF.Max(MathF.Max(0, (int)((width - this.UnitDiameter) / (this.Interval + this.UnitDiameter) + 1E-05f)) + 1, this.MinimumFileCount);
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
		// (set) Token: 0x06001083 RID: 4227 RVA: 0x0002CDF0 File Offset: 0x0002AFF0
		public virtual float Width
		{
			get
			{
				return this.FlankWidth;
			}
			set
			{
				this.FlankWidth = value;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x0002CDF9 File Offset: 0x0002AFF9
		public virtual float Depth
		{
			get
			{
				return this.RankDepth;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0002CE01 File Offset: 0x0002B001
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x0002CE24 File Offset: 0x0002B024
		public float FlankWidth
		{
			get
			{
				return (float)(this.FileCount - 1) * (this.Interval + this.UnitDiameter) + this.UnitDiameter;
			}
			set
			{
				int fileCountFromWidth = this.GetFileCountFromWidth(value);
				if (fileCountFromWidth > this.FileCount)
				{
					LineFormation.WidenFormation(this, fileCountFromWidth - this.FileCount);
				}
				else if (fileCountFromWidth < this.FileCount)
				{
					LineFormation.NarrowFormation(this, this.FileCount - fileCountFromWidth);
				}
				Action onWidthChanged = this.OnWidthChanged;
				if (onWidthChanged != null)
				{
					onWidthChanged();
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged == null)
				{
					return;
				}
				onShapeChanged();
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0002CE8C File Offset: 0x0002B08C
		private int MinimumFileCount
		{
			get
			{
				if (this.IsTransforming)
				{
					return 1;
				}
				int unitCountWithOverride = this.GetUnitCountWithOverride();
				return MathF.Max(1, (int)MathF.Sqrt((float)unitCountWithOverride));
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x0002CEB8 File Offset: 0x0002B0B8
		public float RankDepth
		{
			get
			{
				return (float)(this.RankCount - 1) * (this.Distance + this.UnitDiameter) + this.UnitDiameter;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0002CED8 File Offset: 0x0002B0D8
		public float MinimumFlankWidth
		{
			get
			{
				return (float)(this.MinimumFileCount - 1) * (this.MinimumInterval + this.UnitDiameter) + this.UnitDiameter;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0002CEF8 File Offset: 0x0002B0F8
		public virtual float MinimumWidth
		{
			get
			{
				return this.MinimumFlankWidth;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0002CF00 File Offset: 0x0002B100
		private float MinimumInterval
		{
			get
			{
				return this.owner.MinimumInterval;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x0002CF10 File Offset: 0x0002B110
		public virtual float MaximumWidth
		{
			get
			{
				float num = this.UnitDiameter;
				int unitCountWithOverride = this.GetUnitCountWithOverride();
				if (unitCountWithOverride > 0)
				{
					num += (float)(unitCountWithOverride - 1) * (this.owner.MaximumInterval + this.UnitDiameter);
				}
				return num;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0002CF4C File Offset: 0x0002B14C
		protected int GetUnitCountWithOverride()
		{
			int? overridenUnitCount = this.owner.OverridenUnitCount;
			if (overridenUnitCount == null)
			{
				return this.UnitCount;
			}
			return overridenUnitCount.GetValueOrDefault();
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x0002CF7C File Offset: 0x0002B17C
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x0002CF84 File Offset: 0x0002B184
		public bool IsStaggered
		{
			get
			{
				return this._isStaggered;
			}
			set
			{
				if (this._isStaggered != value)
				{
					this._isStaggered = value;
					Action onShapeChanged = this.OnShapeChanged;
					if (onShapeChanged == null)
					{
						return;
					}
					onShapeChanged();
				}
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x0002CFA8 File Offset: 0x0002B1A8
		public virtual bool? IsLoose
		{
			get
			{
				return null;
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06001091 RID: 4241 RVA: 0x0002CFC0 File Offset: 0x0002B1C0
		// (remove) Token: 0x06001092 RID: 4242 RVA: 0x0002CFF8 File Offset: 0x0002B1F8
		public event Action OnWidthChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06001093 RID: 4243 RVA: 0x0002D030 File Offset: 0x0002B230
		// (remove) Token: 0x06001094 RID: 4244 RVA: 0x0002D068 File Offset: 0x0002B268
		public event Action OnShapeChanged;

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0002D09D File Offset: 0x0002B29D
		// (set) Token: 0x06001096 RID: 4246 RVA: 0x0002D0A5 File Offset: 0x0002B2A5
		public bool PostponeReconstructUnitsFromUnits2D { get; set; }

		// Token: 0x06001097 RID: 4247 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
		public LineFormation(IFormation ownerFormation, bool isStaggered = true)
		{
			this.owner = ownerFormation;
			this.IsStaggered = isStaggered;
			this._units2D = new MBList2D<IFormationUnit>(1, 1);
			this.UnitPositionAvailabilities = new MBList2D<int>(1, 1);
			this._globalPositions = new MBList2D<WorldPosition>(1, 1);
			this._units2DWorkspace = new MBList2D<IFormationUnit>(1, 1);
			this._unitPositionAvailabilitiesWorkspace = new MBList2D<int>(1, 1);
			this._globalPositionsWorkspace = new MBList2D<WorldPosition>(1, 1);
			this._cachedOrderedUnitPositionIndices = new MBArrayList<Vec2i>();
			this._cachedOrderedAndAvailableUnitPositionIndices = new MBArrayList<Vec2i>();
			this._cachedOrderedLocalPositions = new MBArrayList<Vec2>();
			this._unpositionedUnits = new MBList<IFormationUnit>();
			this._displacedUnitsWorkspace = new MBWorkspace<MBQueue<ValueTuple<IFormationUnit, int, int>>>();
			this._finalOccupationsWorkspace = new MBWorkspace<MBArrayList<Vec2i>>();
			this._toBeFilledInGapsWorkspace = new MBWorkspace<MBQueue<Vec2i>>();
			this._finalVacanciesWorkspace = new MBWorkspace<MBArrayList<Vec2i>>();
			this._filledInGapsWorkspace = new MBWorkspace<MBArrayList<Vec2i>>();
			this._toBeEmptiedOutUnitPositionsWorkspace = new MBWorkspace<MBArrayList<Vec2i>>();
			this._filledInUnitPositionsTable = new MBArrayList<bool>();
			this._isFrontUnitDelegate = new Func<Agent, bool>(this.PreferShieldedUnitsOnFront);
			this.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0002D1D2 File Offset: 0x0002B3D2
		protected LineFormation(IFormation ownerFormation, bool isDeformingOnWidthChange, bool isStaggered = true)
			: this(ownerFormation, isStaggered)
		{
			this._isDeformingOnWidthChange = isDeformingOnWidthChange;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0002D1E3 File Offset: 0x0002B3E3
		public virtual IFormationArrangement Clone(IFormation formation)
		{
			return new LineFormation(formation, this._isDeformingOnWidthChange, this.IsStaggered);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
		public virtual void DeepCopyFrom(IFormationArrangement arrangement)
		{
			LineFormation lineFormation = arrangement as LineFormation;
			this.IsStaggered = lineFormation.IsStaggered;
			this.IsTransforming = lineFormation.IsTransforming;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0002D224 File Offset: 0x0002B424
		public void Reset()
		{
			this._units2D = new MBList2D<IFormationUnit>(1, 1);
			this.UnitPositionAvailabilities = new MBList2D<int>(1, 1);
			this._globalPositions = new MBList2D<WorldPosition>(1, 1);
			this._units2DWorkspace = new MBList2D<IFormationUnit>(1, 1);
			this._unitPositionAvailabilitiesWorkspace = new MBList2D<int>(1, 1);
			this._globalPositionsWorkspace = new MBList2D<WorldPosition>(1, 1);
			this._cachedOrderedUnitPositionIndices = new MBArrayList<Vec2i>();
			this._cachedOrderedAndAvailableUnitPositionIndices = new MBArrayList<Vec2i>();
			this._cachedOrderedLocalPositions = new MBArrayList<Vec2>();
			this._unpositionedUnits.Clear();
			this.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0002D2C4 File Offset: 0x0002B4C4
		protected virtual bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (this._isMiddleFrontUnitPositionReserved)
			{
				Vec2i middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
				return fileIndex == middleFrontUnitPosition.Item1 && rankIndex == middleFrontUnitPosition.Item2;
			}
			return false;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0002D2FC File Offset: 0x0002B4FC
		protected virtual void MakeRestrainedPositionsUnavailable()
		{
			if (this._isMiddleFrontUnitPositionReserved)
			{
				Vec2i middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
				this.UnitPositionAvailabilities[middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2] = 1;
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0002D332 File Offset: 0x0002B532
		protected IFormationUnit GetUnitAt(int fileIndex, int rankIndex)
		{
			return this._units2D[fileIndex, rankIndex];
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0002D341 File Offset: 0x0002B541
		public bool IsUnitPositionAvailable(int fileIndex, int rankIndex)
		{
			return this.UnitPositionAvailabilities[fileIndex, rankIndex] == 2;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0002D354 File Offset: 0x0002B554
		private Vec2i GetNearestAvailableNeighbourPositionIndex(int fileIndex, int rankIndex)
		{
			for (int i = 1; i < this.FileCount + this.RankCount; i++)
			{
				bool flag = true;
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = true;
				int num = 0;
				int num2 = 0;
				while (num2 <= i && (flag || flag2 || flag3 || flag4))
				{
					int num3 = i - num2;
					num = num2;
					int num4 = fileIndex - num2;
					int num5 = fileIndex + num2;
					int num6 = rankIndex - num3;
					int num7 = rankIndex + num3;
					if (flag && (num4 < 0 || num6 < 0))
					{
						flag = false;
					}
					if (flag3 && (num4 < 0 || num7 >= this.RankCount))
					{
						flag3 = false;
					}
					if (flag2 && (num5 >= this.FileCount || num6 < 0))
					{
						flag2 = false;
					}
					if (flag4 && (num5 >= this.FileCount || num7 >= this.RankCount))
					{
						flag4 = false;
					}
					if (flag && this.UnitPositionAvailabilities[num4, num6] == 2)
					{
						return new Vec2i(num4, num6);
					}
					if (flag3 && this.UnitPositionAvailabilities[num4, num7] == 2)
					{
						return new Vec2i(num4, num7);
					}
					if (flag2 && this.UnitPositionAvailabilities[num5, num6] == 2)
					{
						return new Vec2i(num5, num6);
					}
					if (flag4 && this.UnitPositionAvailabilities[num5, num7] == 2)
					{
						return new Vec2i(num5, num7);
					}
					num2++;
				}
				flag2 = (flag = (flag3 = (flag4 = true)));
				int num8 = 0;
				while (num8 < i - num && (flag || flag2 || flag3 || flag4))
				{
					int num9 = i - num8;
					int num10 = fileIndex - num9;
					int num11 = fileIndex + num9;
					int num12 = rankIndex - num8;
					int num13 = rankIndex + num8;
					if (flag && (num10 < 0 || num12 < 0))
					{
						flag = false;
					}
					if (flag3 && (num10 < 0 || num13 >= this.RankCount))
					{
						flag3 = false;
					}
					if (flag2 && (num11 >= this.FileCount || num12 < 0))
					{
						flag2 = false;
					}
					if (flag4 && (num11 >= this.FileCount || num13 >= this.RankCount))
					{
						flag4 = false;
					}
					if (flag && this.UnitPositionAvailabilities[num10, num12] == 2)
					{
						return new Vec2i(num10, num12);
					}
					if (flag3 && this.UnitPositionAvailabilities[num10, num13] == 2)
					{
						return new Vec2i(num10, num13);
					}
					if (flag2 && this.UnitPositionAvailabilities[num11, num12] == 2)
					{
						return new Vec2i(num11, num12);
					}
					if (flag4 && this.UnitPositionAvailabilities[num11, num13] == 2)
					{
						return new Vec2i(num11, num13);
					}
					num8++;
				}
			}
			return LineFormation.InvalidPositionIndex;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0002D5BC File Offset: 0x0002B7BC
		private bool GetNextVacancy(out int fileIndex, out int rankIndex)
		{
			int num = this.FileCount * this.RankCount;
			for (int i = 0; i < num; i++)
			{
				Vec2i orderedUnitPositionIndex = this.GetOrderedUnitPositionIndex(i);
				fileIndex = orderedUnitPositionIndex.Item1;
				rankIndex = orderedUnitPositionIndex.Item2;
				if (this._units2D[fileIndex, rankIndex] == null && this.IsUnitPositionAvailable(fileIndex, rankIndex))
				{
					return true;
				}
			}
			fileIndex = -1;
			rankIndex = -1;
			return false;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0002D624 File Offset: 0x0002B824
		private IFormationUnit GetLastUnit()
		{
			int num = -1;
			int num2 = -1;
			IFormationUnit formationUnit = null;
			foreach (IFormationUnit formationUnit2 in this._allUnits)
			{
				int formationFileIndex = formationUnit2.FormationFileIndex;
				int formationRankIndex = formationUnit2.FormationRankIndex;
				int num3 = formationFileIndex + formationRankIndex;
				if (formationRankIndex > num2 || num3 > num)
				{
					num = num3;
					num2 = formationRankIndex;
					formationUnit = formationUnit2;
				}
			}
			return formationUnit;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0002D6A4 File Offset: 0x0002B8A4
		private static Vec2i GetOrderedUnitPositionIndexAux(int fileIndexBegin, int fileIndexEnd, int rankIndexBegin, int rankIndexEnd, int unitIndex)
		{
			int num = fileIndexEnd - fileIndexBegin + 1;
			int num2 = unitIndex / num;
			int num3 = unitIndex - num2 * num;
			if (num % 2 == 1)
			{
				num3 = num / 2 + ((num3 % 2 == 0) ? 1 : (-1)) * (num3 + 1) / 2;
			}
			else
			{
				num3 = num / 2 - 1 + ((num3 % 2 == 0) ? (-1) : 1) * (num3 + 1) / 2;
			}
			return new Vec2i(num3 + fileIndexBegin, num2 + rankIndexBegin);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0002D700 File Offset: 0x0002B900
		private Vec2i GetOrderedUnitPositionIndex(int unitIndex)
		{
			return LineFormation.GetOrderedUnitPositionIndexAux(0, this.FileCount - 1, 0, this.RankCount - 1, unitIndex);
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0002D71A File Offset: 0x0002B91A
		private static IEnumerable<Vec2i> GetOrderedUnitPositionIndicesAux(int fileIndexBegin, int fileIndexEnd, int rankIndexBegin, int rankIndexEnd)
		{
			int fileCount = fileIndexEnd - fileIndexBegin + 1;
			if (fileCount % 2 == 1)
			{
				int centerFileIndex = fileCount / 2;
				int num;
				for (int rankIndex = rankIndexBegin; rankIndex <= rankIndexEnd; rankIndex = num + 1)
				{
					yield return new Vec2i(fileIndexBegin + centerFileIndex, rankIndex);
					for (int fileIndexOffset = 1; fileIndexOffset <= centerFileIndex; fileIndexOffset = num + 1)
					{
						yield return new Vec2i(fileIndexBegin + centerFileIndex - fileIndexOffset, rankIndex);
						if (centerFileIndex + fileIndexOffset < fileCount)
						{
							yield return new Vec2i(fileIndexBegin + centerFileIndex + fileIndexOffset, rankIndex);
						}
						num = fileIndexOffset;
					}
					num = rankIndex;
				}
			}
			else
			{
				int centerFileIndex = fileCount / 2 - 1;
				int num;
				for (int rankIndex = rankIndexBegin; rankIndex <= rankIndexEnd; rankIndex = num + 1)
				{
					yield return new Vec2i(fileIndexBegin + centerFileIndex, rankIndex);
					for (int fileIndexOffset = 1; fileIndexOffset <= centerFileIndex + 1; fileIndexOffset = num + 1)
					{
						yield return new Vec2i(fileIndexBegin + centerFileIndex + fileIndexOffset, rankIndex);
						if (centerFileIndex - fileIndexOffset >= 0)
						{
							yield return new Vec2i(fileIndexBegin + centerFileIndex - fileIndexOffset, rankIndex);
						}
						num = fileIndexOffset;
					}
					num = rankIndex;
				}
			}
			yield break;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0002D73F File Offset: 0x0002B93F
		private IEnumerable<Vec2i> GetOrderedUnitPositionIndices()
		{
			return LineFormation.GetOrderedUnitPositionIndicesAux(0, this.FileCount - 1, 0, this.RankCount - 1);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0002D758 File Offset: 0x0002B958
		public Vec2? GetLocalPositionOfUnitOrDefault(int unitIndex)
		{
			Vec2i vec2i = ((unitIndex < this._cachedOrderedAndAvailableUnitPositionIndices.Count) ? this._cachedOrderedAndAvailableUnitPositionIndices.ElementAt<Vec2i>(unitIndex) : LineFormation.InvalidPositionIndex);
			Vec2? vec;
			if (vec2i != LineFormation.InvalidPositionIndex)
			{
				int item = vec2i.Item1;
				int item2 = vec2i.Item2;
				vec = new Vec2?(this.GetLocalPositionOfUnit(item, item2));
			}
			else
			{
				vec = null;
			}
			return vec;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0002D7BE File Offset: 0x0002B9BE
		public Vec2? GetLocalDirectionOfUnitOrDefault(int unitIndex)
		{
			return new Vec2?(Vec2.Forward);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0002D7CC File Offset: 0x0002B9CC
		public WorldPosition? GetWorldPositionOfUnitOrDefault(int unitIndex)
		{
			Vec2i vec2i = ((unitIndex < this._cachedOrderedAndAvailableUnitPositionIndices.Count) ? this._cachedOrderedAndAvailableUnitPositionIndices.ElementAt<Vec2i>(unitIndex) : LineFormation.InvalidPositionIndex);
			WorldPosition? worldPosition;
			if (vec2i != LineFormation.InvalidPositionIndex)
			{
				int item = vec2i.Item1;
				int item2 = vec2i.Item2;
				worldPosition = new WorldPosition?(this._globalPositions[item, item2]);
			}
			else
			{
				worldPosition = null;
			}
			return worldPosition;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0002D837 File Offset: 0x0002BA37
		public IEnumerable<Vec2> GetUnavailableUnitPositions()
		{
			int num;
			for (int fileIndex = 0; fileIndex < this.FileCount; fileIndex = num + 1)
			{
				for (int rankIndex = 0; rankIndex < this.RankCount; rankIndex = num + 1)
				{
					if (this.UnitPositionAvailabilities[fileIndex, rankIndex] == 1 && !this.IsUnitPositionRestrained(fileIndex, rankIndex))
					{
						yield return this.GetLocalPositionOfUnit(fileIndex, rankIndex);
					}
					num = rankIndex;
				}
				num = fileIndex;
			}
			yield break;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0002D847 File Offset: 0x0002BA47
		private void InsertUnit(IFormationUnit unit, int fileIndex, int rankIndex)
		{
			unit.FormationFileIndex = fileIndex;
			unit.FormationRankIndex = rankIndex;
			this._units2D[fileIndex, rankIndex] = unit;
			this.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0002D87C File Offset: 0x0002BA7C
		public bool AddUnit(IFormationUnit unit)
		{
			bool flag = false;
			while (!flag && !this.AreLastRanksCompletelyUnavailable(3))
			{
				int num;
				int num2;
				if (this.GetNextVacancy(out num, out num2))
				{
					unit.FormationFileIndex = num;
					unit.FormationRankIndex = num2;
					this._units2D[num, num2] = unit;
					this.ReconstructUnitsFromUnits2D();
					flag = true;
				}
				else
				{
					if (!this.IsDeepenApplicable())
					{
						break;
					}
					this.Deepen();
				}
			}
			if (!flag)
			{
				this._unpositionedUnits.Add(unit);
				this.ReconstructUnitsFromUnits2D();
			}
			if (flag)
			{
				if (!(this is TransposedLineFormation) && this.FileCount < this.MinimumFileCount)
				{
					LineFormation.WidenFormation(this, this.MinimumFileCount - this.FileCount);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged != null)
				{
					onShapeChanged();
				}
				Agent agent;
				if ((agent = unit as Agent) != null)
				{
					bool hasMount = agent.HasMount;
					Formation formation;
					if (((formation = this.owner as Formation) != null && formation.CalculateHasSignificantNumberOfMounted) != this._isCavalry)
					{
						this.BatchUnitPositionAvailabilities(true);
					}
					else if (this._isCavalry != hasMount && this.owner is Formation)
					{
						((Formation)this.owner).QuerySystem.ForceExpireCavalryUnitRatio();
						if (((Formation)this.owner).CalculateHasSignificantNumberOfMounted != this._isCavalry)
						{
							this.BatchUnitPositionAvailabilities(true);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0002D9B7 File Offset: 0x0002BBB7
		public void RemoveUnit(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Remove(unit))
			{
				this.ReconstructUnitsFromUnits2D();
				return;
			}
			this.RemoveUnit(unit, true, false);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0002D9D7 File Offset: 0x0002BBD7
		public IFormationUnit GetUnit(int fileIndex, int rankIndex)
		{
			return this._units2D[fileIndex, rankIndex];
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0002D9E8 File Offset: 0x0002BBE8
		public void OnBatchRemoveStart()
		{
			if (this._isBatchRemovingUnits)
			{
				return;
			}
			this._isBatchRemovingUnits = true;
			this._gapFillMinRanksPerFileForBatchRemove.Clear();
			this._batchRemoveInvolvesUnavailablePositions = false;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0002DA0C File Offset: 0x0002BC0C
		public void OnBatchRemoveEnd()
		{
			if (!this._isBatchRemovingUnits)
			{
				return;
			}
			if (this._gapFillMinRanksPerFileForBatchRemove.Count > 0)
			{
				for (int i = 0; i < this._gapFillMinRanksPerFileForBatchRemove.Count; i++)
				{
					int num = this._gapFillMinRanksPerFileForBatchRemove[i];
					if (i < this.FileCount && num < this.RankCount)
					{
						LineFormation.FillInTheGapsOfFile(this, i, num, true);
					}
				}
				this.FillInTheGapsOfFormationAfterRemove(this._batchRemoveInvolvesUnavailablePositions);
				this._gapFillMinRanksPerFileForBatchRemove.Clear();
			}
			this._isBatchRemovingUnits = false;
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0002DA8C File Offset: 0x0002BC8C
		public List<IFormationUnit> GetUnitsToPop(int count)
		{
			List<IFormationUnit> list = new List<IFormationUnit>();
			if (this._unpositionedUnits.Count > 0)
			{
				int num = Math.Min(count, this._unpositionedUnits.Count);
				list.AddRange(this._unpositionedUnits.Take<IFormationUnit>(num));
				count -= num;
			}
			if (count > 0)
			{
				for (int i = this.FileCount * this.RankCount - 1; i >= 0; i--)
				{
					Vec2i orderedUnitPositionIndex = this.GetOrderedUnitPositionIndex(i);
					int item = orderedUnitPositionIndex.Item1;
					int item2 = orderedUnitPositionIndex.Item2;
					if (this._units2D[item, item2] != null)
					{
						list.Add(this._units2D[item, item2]);
						count--;
						if (count == 0)
						{
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0002DB40 File Offset: 0x0002BD40
		private void PickUnitsWithRespectToPosition(Agent agent, float distanceSquared, ref LinkedList<Tuple<IFormationUnit, float>> collection, ref List<IFormationUnit> chosenUnits, int countToChoose, bool chooseClosest)
		{
			if (collection.Count < countToChoose)
			{
				LinkedListNode<Tuple<IFormationUnit, float>> linkedListNode = null;
				for (LinkedListNode<Tuple<IFormationUnit, float>> linkedListNode2 = collection.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
				{
					if (chooseClosest ? (linkedListNode2.Value.Item2 < distanceSquared) : (linkedListNode2.Value.Item2 > distanceSquared))
					{
						linkedListNode = linkedListNode2;
						break;
					}
				}
				if (linkedListNode != null)
				{
					collection.AddBefore(linkedListNode, new LinkedListNode<Tuple<IFormationUnit, float>>(new Tuple<IFormationUnit, float>(agent, distanceSquared)));
					return;
				}
				collection.AddLast(new LinkedListNode<Tuple<IFormationUnit, float>>(new Tuple<IFormationUnit, float>(agent, distanceSquared)));
				return;
			}
			else
			{
				if (chooseClosest ? (distanceSquared < collection.First<Tuple<IFormationUnit, float>>().Item2) : (distanceSquared > collection.First<Tuple<IFormationUnit, float>>().Item2))
				{
					LinkedListNode<Tuple<IFormationUnit, float>> linkedListNode3 = null;
					for (LinkedListNode<Tuple<IFormationUnit, float>> linkedListNode4 = collection.First.Next; linkedListNode4 != null; linkedListNode4 = linkedListNode4.Next)
					{
						if (chooseClosest ? (linkedListNode4.Value.Item2 < distanceSquared) : (linkedListNode4.Value.Item2 > distanceSquared))
						{
							linkedListNode3 = linkedListNode4;
							break;
						}
					}
					if (linkedListNode3 != null)
					{
						collection.AddBefore(linkedListNode3, new LinkedListNode<Tuple<IFormationUnit, float>>(new Tuple<IFormationUnit, float>(agent, distanceSquared)));
					}
					else
					{
						collection.AddLast(new LinkedListNode<Tuple<IFormationUnit, float>>(new Tuple<IFormationUnit, float>(agent, distanceSquared)));
					}
					if (!chooseClosest)
					{
						chosenUnits.Add(collection.First<Tuple<IFormationUnit, float>>().Item1);
					}
					collection.RemoveFirst();
					return;
				}
				if (!chooseClosest)
				{
					chosenUnits.Add(agent);
				}
				return;
			}
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0002DC87 File Offset: 0x0002BE87
		public IEnumerable<IFormationUnit> GetUnitsToPopWithCondition(int count, Func<IFormationUnit, bool> currentCondition)
		{
			IEnumerable<IFormationUnit> unpositionedUnits = this._unpositionedUnits;
			Func<IFormationUnit, bool> <>9__0;
			Func<IFormationUnit, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (IFormationUnit uu) => currentCondition(uu));
			}
			int num;
			foreach (IFormationUnit formationUnit in unpositionedUnits.Where<IFormationUnit>(func))
			{
				yield return formationUnit;
				num = count;
				count = num - 1;
				if (count == 0)
				{
					yield break;
				}
			}
			IEnumerator<IFormationUnit> enumerator = null;
			for (int i = this.FileCount * this.RankCount - 1; i >= 0; i = num - 1)
			{
				Vec2i orderedUnitPositionIndex = this.GetOrderedUnitPositionIndex(i);
				int item = orderedUnitPositionIndex.Item1;
				int item2 = orderedUnitPositionIndex.Item2;
				if (this._units2D[item, item2] != null && currentCondition(this._units2D[item, item2]))
				{
					yield return this._units2D[item, item2];
					num = count;
					count = num - 1;
					if (count == 0)
					{
						yield break;
					}
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0002DCA8 File Offset: 0x0002BEA8
		private void TryToKeepDepth()
		{
			if (this.FileCount > this.MinimumFileCount)
			{
				int num = this.CountUnitsAtRank(this.RankCount - 1);
				int num2 = this.RankCount - 1;
				int num3 = this.FileCount;
				if (this.FileCount > 2)
				{
					num2 *= 2;
					num3 -= 2;
				}
				else
				{
					num3--;
				}
				if (num + num2 <= num3 && MBRandom.RandomInt(this.RankCount * 2) == 0 && this.IsNarrowApplicable((this.FileCount > 2) ? 2 : 1))
				{
					LineFormation.NarrowFormation(this, (this.FileCount > 2) ? 2 : 1);
				}
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0002DD34 File Offset: 0x0002BF34
		public List<IFormationUnit> GetUnitsToPop(int count, Vec3 targetPosition)
		{
			List<IFormationUnit> list = new List<IFormationUnit>();
			if (this._unpositionedUnits.Count > 0)
			{
				int num = Math.Min(count, this._unpositionedUnits.Count);
				if (num < this._unpositionedUnits.Count)
				{
					LinkedList<Tuple<IFormationUnit, float>> linkedList = new LinkedList<Tuple<IFormationUnit, float>>();
					bool flag = (float)num <= (float)this._unpositionedUnits.Count * 0.5f;
					int num2 = (flag ? num : (this._unpositionedUnits.Count - num));
					for (int i = 0; i < this._unpositionedUnits.Count; i++)
					{
						Agent agent = this._unpositionedUnits[i] as Agent;
						if (agent == null)
						{
							if (flag)
							{
								linkedList.AddFirst(new Tuple<IFormationUnit, float>(this._unpositionedUnits[i], float.MinValue));
								if (linkedList.Count > num)
								{
									linkedList.RemoveLast();
								}
							}
							else if (linkedList.Count < num2)
							{
								linkedList.AddLast(new Tuple<IFormationUnit, float>(this._unpositionedUnits[i], float.MinValue));
							}
							else
							{
								list.Add(this._unpositionedUnits[i]);
							}
						}
						else
						{
							float num3 = agent.Position.DistanceSquared(targetPosition);
							this.PickUnitsWithRespectToPosition(agent, num3, ref linkedList, ref list, num2, flag);
						}
					}
					if (flag)
					{
						list.AddRange(linkedList.Select<Tuple<IFormationUnit, float>, IFormationUnit>((Tuple<IFormationUnit, float> tuple) => tuple.Item1));
					}
					count -= num;
				}
				else
				{
					list.AddRange(this._unpositionedUnits.Take<IFormationUnit>(num));
					count -= num;
				}
			}
			if (count > 0)
			{
				int num4 = count;
				int num5 = this.UnitCount - this._unpositionedUnits.Count;
				bool flag2 = num5 == num4;
				bool flag3 = (float)count <= (float)num5 * 0.5f;
				LinkedList<Tuple<IFormationUnit, float>> linkedList2 = (flag2 ? null : new LinkedList<Tuple<IFormationUnit, float>>());
				int num6 = (flag3 ? num4 : (num5 - num4));
				for (int j = this.FileCount * this.RankCount - 1; j >= 0; j--)
				{
					Vec2i orderedUnitPositionIndex = this.GetOrderedUnitPositionIndex(j);
					int item = orderedUnitPositionIndex.Item1;
					int item2 = orderedUnitPositionIndex.Item2;
					if (this._units2D[item, item2] != null)
					{
						if (flag2)
						{
							list.Add(this._units2D[item, item2]);
							count--;
							if (count == 0)
							{
								break;
							}
						}
						else
						{
							Agent agent2 = this._units2D[item, item2] as Agent;
							if (agent2 == null)
							{
								if (flag3)
								{
									linkedList2.AddFirst(new Tuple<IFormationUnit, float>(this._unpositionedUnits[j], float.MinValue));
									if (linkedList2.Count > num4)
									{
										linkedList2.RemoveLast();
									}
								}
								else if (linkedList2.Count < num6)
								{
									linkedList2.AddLast(new Tuple<IFormationUnit, float>(this._unpositionedUnits[j], float.MinValue));
								}
								else
								{
									list.Add(this._unpositionedUnits[j]);
								}
							}
							else
							{
								float num7 = agent2.Position.DistanceSquared(targetPosition);
								this.PickUnitsWithRespectToPosition(agent2, num7, ref linkedList2, ref list, num6, flag3);
							}
						}
					}
				}
				if (!flag2 && flag3)
				{
					list.AddRange(linkedList2.Select<Tuple<IFormationUnit, float>, IFormationUnit>((Tuple<IFormationUnit, float> tuple) => tuple.Item1));
				}
			}
			return list;
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0002E08C File Offset: 0x0002C28C
		private void RemoveUnit(IFormationUnit unit, bool fillInTheGap, bool isRemovingFromAnUnavailablePosition = false)
		{
			if (fillInTheGap)
			{
			}
			int formationFileIndex = unit.FormationFileIndex;
			int formationRankIndex = unit.FormationRankIndex;
			if (unit.FormationFileIndex < 0 || unit.FormationRankIndex < 0 || unit.FormationFileIndex >= this.FileCount || unit.FormationRankIndex >= this.RankCount)
			{
				object[] array = new object[12];
				array[0] = "Unit removed has file-rank indices: ";
				array[1] = unit.FormationFileIndex;
				array[2] = " ";
				array[3] = unit.FormationRankIndex;
				array[4] = " while line formation has file-rank counts of ";
				array[5] = this.FileCount;
				array[6] = " ";
				array[7] = this.RankCount;
				array[8] = " agent state is ";
				int num = 9;
				Agent agent = unit as Agent;
				array[num] = ((agent != null) ? new AgentState?(agent.State) : null);
				array[10] = " unit detachment is ";
				int num2 = 11;
				Agent agent2 = unit as Agent;
				array[num2] = ((agent2 != null) ? agent2.Detachment : null);
				Debug.Print(string.Concat(array), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this._units2D[unit.FormationFileIndex, unit.FormationRankIndex] = null;
			this.ReconstructUnitsFromUnits2D();
			unit.FormationFileIndex = -1;
			unit.FormationRankIndex = -1;
			if (fillInTheGap)
			{
				if (this._isBatchRemovingUnits)
				{
					int num3 = formationFileIndex - this._gapFillMinRanksPerFileForBatchRemove.Count + 1;
					for (int i = 0; i < num3; i++)
					{
						this._gapFillMinRanksPerFileForBatchRemove.Add(int.MaxValue);
					}
					this._gapFillMinRanksPerFileForBatchRemove[formationFileIndex] = MathF.Min(formationRankIndex, this._gapFillMinRanksPerFileForBatchRemove[formationFileIndex]);
					this._batchRemoveInvolvesUnavailablePositions = this._batchRemoveInvolvesUnavailablePositions || isRemovingFromAnUnavailablePosition;
				}
				else
				{
					LineFormation.FillInTheGapsOfFile(this, formationFileIndex, formationRankIndex, true);
					this.FillInTheGapsOfFormationAfterRemove(isRemovingFromAnUnavailablePosition);
				}
			}
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0002E258 File Offset: 0x0002C458
		protected virtual bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			rankIndex = MathF.Round(-localPosition.y / (this.Distance + this.UnitDiameter));
			if (rankIndex >= this.RankCount)
			{
				fileIndex = -1;
				return false;
			}
			if (this.IsStaggered && rankIndex % 2 == 1)
			{
				localPosition.x -= (this.Interval + this.UnitDiameter) * 0.5f;
			}
			float num = (float)(this.FileCount - 1) * (this.Interval + this.UnitDiameter);
			fileIndex = MathF.Round((localPosition.x + num / 2f) / (this.Interval + this.UnitDiameter));
			return fileIndex >= 0 && fileIndex < this.FileCount;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0002E30C File Offset: 0x0002C50C
		protected virtual Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			float num = (float)(this.FileCount - 1) * (this.Interval + this.UnitDiameter);
			Vec2 vec = new Vec2((float)fileIndex * (this.Interval + this.UnitDiameter) - num / 2f, (float)(-(float)rankIndex) * (this.Distance + this.UnitDiameter));
			if (this.IsStaggered && rankIndex % 2 == 1)
			{
				vec.x += (this.Interval + this.UnitDiameter) * 0.5f;
			}
			if ((this.owner as Formation).Team != null && this._units2D[fileIndex, rankIndex] != null)
			{
				vec += (this._units2D[fileIndex, rankIndex] as Agent).LocalPositionError;
			}
			return vec;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0002E3D0 File Offset: 0x0002C5D0
		protected virtual Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			float num = this.Interval + distanceBetweenAgentsAdjustment;
			float num2 = (float)(this.FileCount - 1) * (num + this.UnitDiameter);
			Vec2 vec = new Vec2((float)fileIndex * (num + this.UnitDiameter) - num2 / 2f, (float)(-(float)rankIndex) * (this.Distance + this.UnitDiameter));
			if (this.IsStaggered && rankIndex % 2 == 1)
			{
				vec.x += (num + this.UnitDiameter) * 0.5f;
			}
			if ((this.owner as Formation).Team != null && this._units2D[fileIndex, rankIndex] != null)
			{
				vec += (this._units2D[fileIndex, rankIndex] as Agent).LocalPositionError;
			}
			return vec;
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0002E48B File Offset: 0x0002C68B
		protected virtual Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			return Vec2.Forward;
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0002E494 File Offset: 0x0002C694
		public Vec2? GetLocalPositionOfUnitOrDefault(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			return new Vec2?(this.GetLocalPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0002E4D0 File Offset: 0x0002C6D0
		public Vec2? GetLocalPositionOfUnitOrDefaultWithAdjustment(IFormationUnit unit, float distanceBetweenAgentsAdjustment)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			return new Vec2?(this.GetLocalPositionOfUnitWithAdjustment(unit.FormationFileIndex, unit.FormationRankIndex, distanceBetweenAgentsAdjustment));
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0002E50D File Offset: 0x0002C70D
		public virtual Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			return new Vec2?(Vec2.Forward);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0002E51C File Offset: 0x0002C71C
		public WorldPosition? GetWorldPositionOfUnitOrDefault(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			return new WorldPosition?(this._globalPositions[unit.FormationFileIndex, unit.FormationRankIndex]);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0002E560 File Offset: 0x0002C760
		private void ReconstructUnitsFromUnits2D()
		{
			if (this.PostponeReconstructUnitsFromUnits2D)
			{
				return;
			}
			if (this._allUnits == null)
			{
				this._allUnits = new MBList<IFormationUnit>();
			}
			this._allUnits.Clear();
			for (int i = 0; i < this._units2D.Count1; i++)
			{
				for (int j = 0; j < this._units2D.Count2; j++)
				{
					if (this._units2D[i, j] != null)
					{
						this._allUnits.Add(this._units2D[i, j]);
					}
				}
			}
			for (int k = 0; k < this._unpositionedUnits.Count; k++)
			{
				this._allUnits.Add(this._unpositionedUnits[k]);
			}
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0002E614 File Offset: 0x0002C814
		private void FillInTheGapsOfFormationAfterRemove(bool hasUnavailablePositions)
		{
			this.TryReaddingUnpositionedUnits();
			LineFormation.FillInTheGapsOfMiddleRanks(this, null);
			if (Mission.Current.IsDeploymentFinished)
			{
				this.TryToKeepDepth();
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0002E638 File Offset: 0x0002C838
		private static void WidenFormation(LineFormation formation, int fileCountFromBothFlanks)
		{
			if (fileCountFromBothFlanks % 2 == 0)
			{
				LineFormation.WidenFormation(formation, fileCountFromBothFlanks / 2, fileCountFromBothFlanks / 2);
				return;
			}
			if (formation.FileCount % 2 == 0)
			{
				LineFormation.WidenFormation(formation, fileCountFromBothFlanks / 2 + 1, fileCountFromBothFlanks / 2);
				return;
			}
			LineFormation.WidenFormation(formation, fileCountFromBothFlanks / 2, fileCountFromBothFlanks / 2 + 1);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0002E674 File Offset: 0x0002C874
		private static void WidenFormation(LineFormation formation, int fileCountFromLeftFlank, int fileCountFromRightFlank)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount + fileCountFromLeftFlank + fileCountFromRightFlank, formation.RankCount);
			formation._unitPositionAvailabilitiesWorkspace.ResetWithNewCount(formation.FileCount + fileCountFromLeftFlank + fileCountFromRightFlank, formation.RankCount);
			formation._globalPositionsWorkspace.ResetWithNewCount(formation.FileCount + fileCountFromLeftFlank + fileCountFromRightFlank, formation.RankCount);
			for (int i = 0; i < formation.FileCount; i++)
			{
				int num = i + fileCountFromLeftFlank;
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, num, 0, formation.RankCount);
				formation.UnitPositionAvailabilities.CopyRowTo(i, 0, formation._unitPositionAvailabilitiesWorkspace, num, 0, formation.RankCount);
				formation._globalPositions.CopyRowTo(i, 0, formation._globalPositionsWorkspace, num, 0, formation.RankCount);
				if (fileCountFromLeftFlank > 0)
				{
					for (int j = 0; j < formation.RankCount; j++)
					{
						if (formation._units2D[i, j] != null)
						{
							formation._units2D[i, j].FormationFileIndex += fileCountFromLeftFlank;
						}
					}
				}
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			MBList2D<int> unitPositionAvailabilities = formation.UnitPositionAvailabilities;
			formation.UnitPositionAvailabilities = formation._unitPositionAvailabilitiesWorkspace;
			formation._unitPositionAvailabilitiesWorkspace = unitPositionAvailabilities;
			MBList2D<WorldPosition> globalPositions = formation._globalPositions;
			formation._globalPositions = formation._globalPositionsWorkspace;
			formation._globalPositionsWorkspace = globalPositions;
			formation.BatchUnitPositionAvailabilities(true);
			if (formation._isDeformingOnWidthChange || (fileCountFromLeftFlank + fileCountFromRightFlank) % 2 == 1)
			{
				formation.OnFormationFrameChanged(false);
			}
			else
			{
				LineFormation.ShiftUnitsForwardsForWideningFormation(formation);
				formation.TryReaddingUnpositionedUnits();
				while (formation.RankCount > 1 && formation.IsRankEmpty(formation.RankCount - 1))
				{
					formation.Shorten();
				}
			}
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0002E838 File Offset: 0x0002CA38
		private static void GetToBeFilledInAndToBeEmptiedOutUnitPositions(LineFormation formation, MBQueue<Vec2i> toBeFilledInUnitPositions, MBArrayList<Vec2i> toBeEmptiedOutUnitPositions)
		{
			int num = 0;
			int num2 = formation.FileCount * formation.RankCount - 1;
			for (;;)
			{
				Vec2i orderedUnitPositionIndex = formation.GetOrderedUnitPositionIndex(num);
				int item = orderedUnitPositionIndex.Item1;
				int item2 = orderedUnitPositionIndex.Item2;
				Vec2i orderedUnitPositionIndex2 = formation.GetOrderedUnitPositionIndex(num2);
				int item3 = orderedUnitPositionIndex2.Item1;
				int item4 = orderedUnitPositionIndex2.Item2;
				if (item2 >= item4)
				{
					break;
				}
				if (formation._units2D[item, item2] != null || !formation.IsUnitPositionAvailable(item, item2))
				{
					num++;
				}
				else if (formation._units2D[item3, item4] == null)
				{
					num2--;
				}
				else
				{
					toBeFilledInUnitPositions.Enqueue(new Vec2i(item, item2));
					toBeEmptiedOutUnitPositions.Add(new Vec2i(item3, item4));
					num++;
					num2--;
				}
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0002E8F3 File Offset: 0x0002CAF3
		private static Vec2i GetUnitPositionForFillInFromNearby(LineFormation formation, int relocationFileIndex, int relocationRankIndex, Func<LineFormation, int, int, bool> predicate, bool isRelocationUnavailable = false)
		{
			return LineFormation.GetUnitPositionForFillInFromNearby(formation, relocationFileIndex, relocationRankIndex, predicate, LineFormation.InvalidPositionIndex, isRelocationUnavailable);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0002E908 File Offset: 0x0002CB08
		private static Vec2i GetUnitPositionForFillInFromNearby(LineFormation formation, int relocationFileIndex, int relocationRankIndex, Func<LineFormation, int, int, bool> predicate, Vec2i lastFinalOccupation, bool isRelocationUnavailable = false)
		{
			int fileCount = formation.FileCount;
			int rankCount = formation.RankCount;
			bool flag = relocationFileIndex >= fileCount / 2;
			if (lastFinalOccupation != LineFormation.InvalidPositionIndex)
			{
				flag = lastFinalOccupation.Item1 <= relocationFileIndex;
			}
			for (int i = 1; i <= fileCount + rankCount; i++)
			{
				for (int j = MathF.Min(i, rankCount - 1 - relocationRankIndex); j >= 0; j--)
				{
					int num = i - j;
					if (flag && relocationFileIndex - num >= 0 && predicate(formation, relocationFileIndex - num, relocationRankIndex + j))
					{
						return new Vec2i(relocationFileIndex - num, relocationRankIndex + j);
					}
					if (relocationFileIndex + num < fileCount && predicate(formation, relocationFileIndex + num, relocationRankIndex + j))
					{
						return new Vec2i(relocationFileIndex + num, relocationRankIndex + j);
					}
					if (!flag && relocationFileIndex - num >= 0 && predicate(formation, relocationFileIndex - num, relocationRankIndex + j))
					{
						return new Vec2i(relocationFileIndex - num, relocationRankIndex + j);
					}
				}
			}
			return LineFormation.InvalidPositionIndex;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0002EA00 File Offset: 0x0002CC00
		private static void ShiftUnitsForwardsForWideningFormation(LineFormation formation)
		{
			MBQueue<Vec2i> mbqueue = formation._toBeFilledInGapsWorkspace.StartUsingWorkspace();
			MBArrayList<Vec2i> mbarrayList = formation._finalVacanciesWorkspace.StartUsingWorkspace();
			MBArrayList<Vec2i> mbarrayList2 = formation._filledInGapsWorkspace.StartUsingWorkspace();
			LineFormation.GetToBeFilledInAndToBeEmptiedOutUnitPositions(formation, mbqueue, mbarrayList);
			if (formation._shiftUnitsForwardsPredicateDelegate == null)
			{
				formation._shiftUnitsForwardsPredicateDelegate = new Func<LineFormation, int, int, bool>(LineFormation.<>c.<>9.<ShiftUnitsForwardsForWideningFormation>g__ShiftUnitForwardsPredicate|136_0);
			}
			while (mbqueue.Count > 0)
			{
				Vec2i vec2i = mbqueue.Dequeue();
				Vec2i unitPositionForFillInFromNearby = LineFormation.GetUnitPositionForFillInFromNearby(formation, vec2i.Item1, vec2i.Item2, formation._shiftUnitsForwardsPredicateDelegate, false);
				if (unitPositionForFillInFromNearby != LineFormation.InvalidPositionIndex)
				{
					int item = unitPositionForFillInFromNearby.Item1;
					int item2 = unitPositionForFillInFromNearby.Item2;
					IFormationUnit formationUnit = formation._units2D[item, item2];
					formation.RelocateUnit(formationUnit, vec2i.Item1, vec2i.Item2);
					mbarrayList2.Add(vec2i);
					Vec2i vec2i2 = new Vec2i(item, item2);
					if (!mbarrayList.Contains(vec2i2))
					{
						mbqueue.Enqueue(vec2i2);
					}
				}
			}
			formation._toBeFilledInGapsWorkspace.StopUsingWorkspace();
			formation._finalVacanciesWorkspace.StopUsingWorkspace();
			formation._filledInGapsWorkspace.StopUsingWorkspace();
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0002EB1C File Offset: 0x0002CD1C
		private static void DeepenFormation(LineFormation formation, int rankCountFromFront, int rankCountFromRear)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount + rankCountFromFront + rankCountFromRear);
			formation._unitPositionAvailabilitiesWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount + rankCountFromFront + rankCountFromRear);
			formation._globalPositionsWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount + rankCountFromFront + rankCountFromRear);
			for (int i = 0; i < formation.FileCount; i++)
			{
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, i, rankCountFromFront, formation.RankCount);
				formation.UnitPositionAvailabilities.CopyRowTo(i, 0, formation._unitPositionAvailabilitiesWorkspace, i, rankCountFromFront, formation.RankCount);
				formation._globalPositions.CopyRowTo(i, 0, formation._globalPositionsWorkspace, i, rankCountFromFront, formation.RankCount);
				if (rankCountFromFront > 0)
				{
					for (int j = 0; j < formation.RankCount; j++)
					{
						if (formation._units2D[i, j] != null)
						{
							formation._units2D[i, j].FormationRankIndex += rankCountFromFront;
						}
					}
				}
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			MBList2D<int> unitPositionAvailabilities = formation.UnitPositionAvailabilities;
			formation.UnitPositionAvailabilities = formation._unitPositionAvailabilitiesWorkspace;
			formation._unitPositionAvailabilitiesWorkspace = unitPositionAvailabilities;
			MBList2D<WorldPosition> globalPositions = formation._globalPositions;
			formation._globalPositions = formation._globalPositionsWorkspace;
			formation._globalPositionsWorkspace = globalPositions;
			formation.BatchUnitPositionAvailabilities(true);
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0002EC91 File Offset: 0x0002CE91
		protected virtual bool IsDeepenApplicable()
		{
			return true;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0002EC94 File Offset: 0x0002CE94
		private void Deepen()
		{
			LineFormation.DeepenFormation(this, 0, 1);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0002ECA0 File Offset: 0x0002CEA0
		private static bool DeepenForVacancy(LineFormation formation, int requestedVacancyCount, int fileOffsetFromLeftFlank, int fileOffsetFromRightFlank)
		{
			int num = 0;
			bool? flag = null;
			while (flag == null)
			{
				int num2 = formation.RankCount - 1;
				for (int i = fileOffsetFromLeftFlank; i < formation.FileCount - fileOffsetFromRightFlank; i++)
				{
					if (formation._units2D[i, num2] == null && formation.IsUnitPositionAvailable(i, num2))
					{
						num++;
					}
				}
				if (num >= requestedVacancyCount)
				{
					flag = new bool?(true);
				}
				else if (!formation.AreLastRanksCompletelyUnavailable(3))
				{
					if (formation.IsDeepenApplicable())
					{
						formation.Deepen();
					}
					else
					{
						flag = new bool?(false);
					}
				}
				else
				{
					flag = new bool?(false);
				}
			}
			return flag.Value;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0002ED3B File Offset: 0x0002CF3B
		protected virtual bool IsNarrowApplicable(int amount)
		{
			return true;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0002ED40 File Offset: 0x0002CF40
		private static void NarrowFormation(LineFormation formation, int fileCountFromBothFlanks)
		{
			int num = fileCountFromBothFlanks / 2;
			int num2 = fileCountFromBothFlanks / 2;
			if (fileCountFromBothFlanks % 2 != 0)
			{
				if (formation.FileCount % 2 == 0)
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
			if (formation.IsNarrowApplicable(num + num2))
			{
				LineFormation.NarrowFormation(formation, num, num2);
			}
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0002ED84 File Offset: 0x0002CF84
		private static bool ShiftUnitsBackwardsForNewUnavailableUnitPositions(LineFormation formation)
		{
			MBArrayList<Vec2i> mbarrayList = formation._toBeEmptiedOutUnitPositionsWorkspace.StartUsingWorkspace();
			for (int i = 0; i < formation.FileCount * formation.RankCount; i++)
			{
				Vec2i orderedUnitPositionIndex = formation.GetOrderedUnitPositionIndex(i);
				if (formation._units2D[orderedUnitPositionIndex.Item1, orderedUnitPositionIndex.Item2] != null && !formation.IsUnitPositionAvailable(orderedUnitPositionIndex.Item1, orderedUnitPositionIndex.Item2))
				{
					mbarrayList.Add(orderedUnitPositionIndex);
				}
			}
			bool flag = mbarrayList.Count > 0;
			if (flag)
			{
				MBQueue<ValueTuple<IFormationUnit, int, int>> mbqueue = formation._displacedUnitsWorkspace.StartUsingWorkspace();
				for (int j = mbarrayList.Count - 1; j >= 0; j--)
				{
					Vec2i vec2i = mbarrayList[j];
					IFormationUnit formationUnit = formation._units2D[vec2i.Item1, vec2i.Item2];
					if (formationUnit != null)
					{
						formation.RemoveUnit(formationUnit, false, true);
						mbqueue.Enqueue(ValueTuple.Create<IFormationUnit, int, int>(formationUnit, vec2i.Item1, vec2i.Item2));
					}
				}
				LineFormation.DeepenForVacancy(formation, mbqueue.Count, 0, 0);
				MBArrayList<Vec2i> mbarrayList2 = formation._finalOccupationsWorkspace.StartUsingWorkspace();
				int num = 0;
				int num2 = 0;
				while (num2 < formation.FileCount * formation.RankCount && num < mbqueue.Count)
				{
					Vec2i orderedUnitPositionIndex2 = formation.GetOrderedUnitPositionIndex(num2);
					if (formation._units2D[orderedUnitPositionIndex2.Item1, orderedUnitPositionIndex2.Item2] == null && formation.IsUnitPositionAvailable(orderedUnitPositionIndex2.Item1, orderedUnitPositionIndex2.Item2))
					{
						mbarrayList2.Add(orderedUnitPositionIndex2);
						num++;
					}
					num2++;
				}
				LineFormation.ShiftUnitsBackwardsAux(formation, mbqueue, mbarrayList2);
				formation._displacedUnitsWorkspace.StopUsingWorkspace();
				formation._finalOccupationsWorkspace.StopUsingWorkspace();
			}
			formation._toBeEmptiedOutUnitPositionsWorkspace.StopUsingWorkspace();
			return flag;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0002EF38 File Offset: 0x0002D138
		private static void ShiftUnitsBackwardsForNarrowingFormation(LineFormation formation, int fileCountFromLeftFlank, int fileCountFromRightFlank)
		{
			formation.PostponeReconstructUnitsFromUnits2D = true;
			MBQueue<ValueTuple<IFormationUnit, int, int>> mbqueue = formation._displacedUnitsWorkspace.StartUsingWorkspace();
			foreach (Vec2i vec2i in (from p in formation.GetOrderedUnitPositionIndices()
				where p.Item1 < fileCountFromLeftFlank || p.Item1 >= formation.FileCount - fileCountFromRightFlank
				select p).Reverse<Vec2i>())
			{
				IFormationUnit formationUnit = formation._units2D[vec2i.Item1, vec2i.Item2];
				if (formationUnit != null)
				{
					formation.RemoveUnit(formationUnit, false, false);
					mbqueue.Enqueue(ValueTuple.Create<IFormationUnit, int, int>(formationUnit, vec2i.Item1, vec2i.Item2));
				}
			}
			LineFormation.DeepenForVacancy(formation, mbqueue.Count, fileCountFromLeftFlank, fileCountFromRightFlank);
			IEnumerable<Vec2i> enumerable = (from p in LineFormation.GetOrderedUnitPositionIndicesAux(fileCountFromLeftFlank, formation.FileCount - 1 - fileCountFromRightFlank, 0, formation.RankCount - 1)
				where formation._units2D[p.Item1, p.Item2] == null && formation.IsUnitPositionAvailable(p.Item1, p.Item2)
				select p).Take<Vec2i>(mbqueue.Count);
			MBArrayList<Vec2i> mbarrayList = formation._finalOccupationsWorkspace.StartUsingWorkspace();
			mbarrayList.AddRange(enumerable);
			LineFormation.ShiftUnitsBackwardsAux(formation, mbqueue, mbarrayList);
			formation._displacedUnitsWorkspace.StopUsingWorkspace();
			formation._finalOccupationsWorkspace.StopUsingWorkspace();
			formation.PostponeReconstructUnitsFromUnits2D = false;
			formation.ReconstructUnitsFromUnits2D();
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0002F0EC File Offset: 0x0002D2EC
		private static void ShiftUnitsBackwardsAux(LineFormation formation, MBQueue<ValueTuple<IFormationUnit, int, int>> displacedUnits, MBArrayList<Vec2i> finalOccupations)
		{
			MBArrayList<bool> mbarrayList = new MBArrayList<bool>(new bool[formation.FileCount * formation.RankCount]);
			formation._filledInUnitPositionsTable = mbarrayList;
			if (formation._shiftUnitsBackwardsPredicateDelegate == null)
			{
				formation._shiftUnitsBackwardsPredicateDelegate = new Func<LineFormation, int, int, bool>(LineFormation.<>c.<>9.<ShiftUnitsBackwardsAux>g__ShiftUnitsBackwardsPredicate|145_0);
			}
			while (!displacedUnits.IsEmpty<ValueTuple<IFormationUnit, int, int>>())
			{
				ValueTuple<IFormationUnit, int, int> valueTuple = displacedUnits.Dequeue();
				IFormationUnit item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				Vec2i unitPositionForFillInFromNearby = LineFormation.GetUnitPositionForFillInFromNearby(formation, item2, item3, formation._shiftUnitsBackwardsPredicateDelegate, (finalOccupations.Count == 1) ? finalOccupations[0] : LineFormation.InvalidPositionIndex, true);
				if (unitPositionForFillInFromNearby != LineFormation.InvalidPositionIndex)
				{
					IFormationUnit formationUnit = formation._units2D[unitPositionForFillInFromNearby.Item1, unitPositionForFillInFromNearby.Item2];
					if (formationUnit != null)
					{
						formation.RemoveUnit(formationUnit, false, false);
						displacedUnits.Enqueue(ValueTuple.Create<IFormationUnit, int, int>(formationUnit, unitPositionForFillInFromNearby.Item1, unitPositionForFillInFromNearby.Item2));
					}
					mbarrayList[unitPositionForFillInFromNearby.Item1 + unitPositionForFillInFromNearby.Item2 * formation.FileCount] = true;
					formation.InsertUnit(item, unitPositionForFillInFromNearby.Item1, unitPositionForFillInFromNearby.Item2);
				}
				else
				{
					float num = float.MaxValue;
					Vec2i vec2i = LineFormation.InvalidPositionIndex;
					for (int i = 0; i < finalOccupations.Count; i++)
					{
						if (!mbarrayList[finalOccupations[i].Item1 + finalOccupations[i].Item2 * formation.FileCount])
						{
							float num2 = (float)(MathF.Abs(finalOccupations[i].Item1 - item2) + MathF.Abs(finalOccupations[i].Item2 - item3));
							if (num2 < num)
							{
								num = num2;
								vec2i = finalOccupations[i];
							}
						}
					}
					if (vec2i != LineFormation.InvalidPositionIndex)
					{
						mbarrayList[vec2i.Item1 + vec2i.Item2 * formation.FileCount] = true;
						formation.InsertUnit(item, vec2i.Item1, vec2i.Item2);
					}
					else
					{
						formation._unpositionedUnits.Add(item);
						formation.ReconstructUnitsFromUnits2D();
					}
				}
			}
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0002F30B File Offset: 0x0002D50B
		private static void NarrowFormation(LineFormation formation, int fileCountFromLeftFlank, int fileCountFromRightFlank)
		{
			LineFormation.ShiftUnitsBackwardsForNarrowingFormation(formation, fileCountFromLeftFlank, fileCountFromRightFlank);
			LineFormation.NarrowFormationAux(formation, fileCountFromLeftFlank, fileCountFromRightFlank);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0002F320 File Offset: 0x0002D520
		private static void NarrowFormationAux(LineFormation formation, int fileCountFromLeftFlank, int fileCountFromRightFlank)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount - fileCountFromLeftFlank - fileCountFromRightFlank, formation.RankCount);
			formation._unitPositionAvailabilitiesWorkspace.ResetWithNewCount(formation.FileCount - fileCountFromLeftFlank - fileCountFromRightFlank, formation.RankCount);
			formation._globalPositionsWorkspace.ResetWithNewCount(formation.FileCount - fileCountFromLeftFlank - fileCountFromRightFlank, formation.RankCount);
			for (int i = fileCountFromLeftFlank; i < formation.FileCount - fileCountFromRightFlank; i++)
			{
				int num = i - fileCountFromLeftFlank;
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, num, 0, formation.RankCount);
				formation.UnitPositionAvailabilities.CopyRowTo(i, 0, formation._unitPositionAvailabilitiesWorkspace, num, 0, formation.RankCount);
				formation._globalPositions.CopyRowTo(i, 0, formation._globalPositionsWorkspace, num, 0, formation.RankCount);
				if (fileCountFromLeftFlank > 0)
				{
					for (int j = 0; j < formation.RankCount; j++)
					{
						if (formation._units2D[i, j] != null)
						{
							formation._units2D[i, j].FormationFileIndex -= fileCountFromLeftFlank;
						}
					}
				}
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			MBList2D<int> unitPositionAvailabilities = formation.UnitPositionAvailabilities;
			formation.UnitPositionAvailabilities = formation._unitPositionAvailabilitiesWorkspace;
			formation._unitPositionAvailabilitiesWorkspace = unitPositionAvailabilities;
			MBList2D<WorldPosition> globalPositions = formation._globalPositions;
			formation._globalPositions = formation._globalPositionsWorkspace;
			formation._globalPositionsWorkspace = globalPositions;
			formation.BatchUnitPositionAvailabilities(true);
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged != null)
			{
				onShapeChanged();
			}
			if (formation._isDeformingOnWidthChange || (fileCountFromLeftFlank + fileCountFromRightFlank) % 2 == 1)
			{
				formation.OnFormationFrameChanged(false);
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0002F4B8 File Offset: 0x0002D6B8
		private static void ShortenFormation(LineFormation formation, int front, int rear)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount - front - rear);
			formation._unitPositionAvailabilitiesWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount - front - rear);
			formation._globalPositionsWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount - front - rear);
			for (int i = 0; i < formation.FileCount; i++)
			{
				formation._units2D.CopyRowTo(i, front, formation._units2DWorkspace, i, 0, formation.RankCount - rear - front);
				formation.UnitPositionAvailabilities.CopyRowTo(i, front, formation._unitPositionAvailabilitiesWorkspace, i, 0, formation.RankCount - rear - front);
				formation._globalPositions.CopyRowTo(i, front, formation._globalPositionsWorkspace, i, 0, formation.RankCount - rear - front);
				if (front > 0)
				{
					for (int j = front; j < formation.RankCount - rear; j++)
					{
						if (formation._units2D[i, j] != null)
						{
							formation._units2D[i, j].FormationRankIndex -= front;
						}
					}
				}
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			MBList2D<int> unitPositionAvailabilities = formation.UnitPositionAvailabilities;
			formation.UnitPositionAvailabilities = formation._unitPositionAvailabilitiesWorkspace;
			formation._unitPositionAvailabilitiesWorkspace = unitPositionAvailabilities;
			MBList2D<WorldPosition> globalPositions = formation._globalPositions;
			formation._globalPositions = formation._globalPositionsWorkspace;
			formation._globalPositionsWorkspace = globalPositions;
			formation.BatchUnitPositionAvailabilities(true);
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0002F63B File Offset: 0x0002D83B
		private void Shorten()
		{
			LineFormation.ShortenFormation(this, 0, 1);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0002F648 File Offset: 0x0002D848
		private void GetFrontAndRearOfFile(int fileIndex, out bool isFileEmtpy, out int rankIndexOfFront, out int rankIndexOfRear, bool includeUnavailablePositions = false)
		{
			rankIndexOfFront = -1;
			rankIndexOfRear = this.RankCount;
			for (int i = 0; i < this.RankCount; i++)
			{
				if (this._units2D[fileIndex, i] != null)
				{
					rankIndexOfFront = i;
					break;
				}
			}
			if (includeUnavailablePositions)
			{
				if (rankIndexOfFront != -1)
				{
					for (int j = rankIndexOfFront - 1; j >= 0; j--)
					{
						if (this.IsUnitPositionAvailable(fileIndex, j))
						{
							break;
						}
						rankIndexOfFront = j;
					}
				}
				else
				{
					bool flag = true;
					for (int k = 0; k < this.RankCount; k++)
					{
						if (this.IsUnitPositionAvailable(fileIndex, k))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						rankIndexOfFront = 0;
					}
				}
			}
			for (int l = this.RankCount - 1; l >= 0; l--)
			{
				if (this._units2D[fileIndex, l] != null)
				{
					rankIndexOfRear = l;
					break;
				}
			}
			if (includeUnavailablePositions)
			{
				if (rankIndexOfRear != this.RankCount)
				{
					for (int m = rankIndexOfRear + 1; m < this.RankCount; m++)
					{
						if (this.IsUnitPositionAvailable(fileIndex, m))
						{
							break;
						}
						rankIndexOfRear = m;
					}
				}
				else
				{
					bool flag2 = true;
					for (int n = 0; n < this.RankCount; n++)
					{
						if (this.IsUnitPositionAvailable(fileIndex, n))
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						rankIndexOfRear = this.RankCount - 1;
					}
				}
			}
			if (rankIndexOfFront == -1 && rankIndexOfRear == this.RankCount)
			{
				isFileEmtpy = true;
				return;
			}
			isFileEmtpy = false;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0002F78C File Offset: 0x0002D98C
		private void GetFlanksOfRank(int rankIndex, out bool isRankEmpty, out int fileIndexOfLeftFlank, out int fileIndexOfRightFlank, bool includeUnavailablePositions = false)
		{
			fileIndexOfLeftFlank = -1;
			fileIndexOfRightFlank = this.FileCount;
			for (int i = 0; i < this.FileCount; i++)
			{
				if (this._units2D[i, rankIndex] != null)
				{
					fileIndexOfLeftFlank = i;
					break;
				}
			}
			if (includeUnavailablePositions)
			{
				if (fileIndexOfLeftFlank != -1)
				{
					for (int j = fileIndexOfLeftFlank - 1; j >= 0; j--)
					{
						if (this.IsUnitPositionAvailable(j, rankIndex))
						{
							break;
						}
						fileIndexOfLeftFlank = j;
					}
				}
				else
				{
					bool flag = true;
					for (int k = 0; k < this.FileCount; k++)
					{
						if (this.IsUnitPositionAvailable(k, rankIndex))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						fileIndexOfLeftFlank = 0;
					}
				}
			}
			for (int l = this.FileCount - 1; l >= 0; l--)
			{
				if (this._units2D[l, rankIndex] != null)
				{
					fileIndexOfRightFlank = l;
					break;
				}
			}
			if (includeUnavailablePositions)
			{
				if (fileIndexOfRightFlank != this.FileCount)
				{
					for (int m = fileIndexOfRightFlank + 1; m < this.FileCount; m++)
					{
						if (this.IsUnitPositionAvailable(m, rankIndex))
						{
							break;
						}
						fileIndexOfRightFlank = m;
					}
				}
				else
				{
					bool flag2 = true;
					for (int n = 0; n < this.FileCount; n++)
					{
						if (this.IsUnitPositionAvailable(n, rankIndex))
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						fileIndexOfRightFlank = this.FileCount - 1;
					}
				}
			}
			if (fileIndexOfLeftFlank == -1 && fileIndexOfRightFlank == this.FileCount)
			{
				isRankEmpty = true;
				return;
			}
			isRankEmpty = false;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0002F8CE File Offset: 0x0002DACE
		private static void FillInTheGapsOfFile(LineFormation formation, int fileIndex, int rankIndex = 0, bool isCheckingLastRankForEmptiness = true)
		{
			LineFormation.FillInTheGapsOfFileAux(formation, fileIndex, rankIndex);
			while (isCheckingLastRankForEmptiness && formation.RankCount > 1 && formation.IsRankEmpty(formation.RankCount - 1))
			{
				formation.Shorten();
			}
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0002F8FC File Offset: 0x0002DAFC
		private static void FillInTheGapsOfFileAux(LineFormation formation, int fileIndex, int rankIndex)
		{
			for (;;)
			{
				int num = -1;
				while (rankIndex < formation.RankCount - 1)
				{
					if (formation._units2D[fileIndex, rankIndex] == null && formation.IsUnitPositionAvailable(fileIndex, rankIndex))
					{
						num = rankIndex;
						break;
					}
					rankIndex++;
				}
				int num2 = -1;
				while (rankIndex < formation.RankCount)
				{
					if (formation._units2D[fileIndex, rankIndex] != null)
					{
						num2 = rankIndex;
						break;
					}
					rankIndex++;
				}
				if (num == -1 || num2 == -1)
				{
					break;
				}
				formation.RelocateUnit(formation._units2D[fileIndex, num2], fileIndex, num);
				rankIndex = num + 1;
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0002F984 File Offset: 0x0002DB84
		private static void FillInTheGapsOfMiddleRanks(LineFormation formation, List<IFormationUnit> relocatedUnits = null)
		{
			int num = formation.RankCount - 1;
			for (int i = 0; i < formation.FileCount; i++)
			{
				if (formation._units2D[i, num] == null && !formation.IsFileFullyOccupied(i))
				{
					int num4;
					for (;;)
					{
						bool flag;
						int num2;
						int num3;
						formation.GetFrontAndRearOfFile(i, out flag, out num2, out num3, true);
						if (num3 == num)
						{
							goto IL_00D7;
						}
						num4 = num3 + 1;
						if (flag)
						{
							num4 = -1;
							for (int j = 0; j < formation.RankCount; j++)
							{
								if (formation.IsUnitPositionAvailable(i, j))
								{
									num4 = j;
									break;
								}
							}
						}
						IFormationUnit unitToFillIn = LineFormation.GetUnitToFillIn(formation, i, num4);
						if (unitToFillIn == null)
						{
							break;
						}
						formation.RelocateUnit(unitToFillIn, i, num4);
						if (relocatedUnits != null)
						{
							relocatedUnits.Add(unitToFillIn);
						}
						if (formation.IsRankEmpty(num))
						{
							formation.Shorten();
							num = formation.RankCount - 1;
						}
					}
					for (int k = num4 + 1; k < formation.RankCount; k++)
					{
					}
				}
				IL_00D7:;
			}
			while (formation.RankCount > 1 && formation.IsRankEmpty(formation.RankCount - 1))
			{
				formation.Shorten();
			}
			LineFormation.AlignLastRank(formation);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0002FAA0 File Offset: 0x0002DCA0
		private static void AlignRankToLeft(LineFormation formation, int fileIndex, int rankIndex)
		{
			int num = -1;
			while (fileIndex < formation.FileCount - 1)
			{
				if (formation._units2D[fileIndex, rankIndex] == null && formation.IsUnitPositionAvailable(fileIndex, rankIndex))
				{
					num = fileIndex;
					break;
				}
				fileIndex++;
			}
			int num2 = -1;
			while (fileIndex < formation.FileCount)
			{
				if (formation._units2D[fileIndex, rankIndex] != null)
				{
					num2 = fileIndex;
					break;
				}
				fileIndex++;
			}
			if (num != -1 && num2 != -1)
			{
				formation.RelocateUnit(formation._units2D[num2, rankIndex], num, rankIndex);
				LineFormation.AlignRankToLeft(formation, num + 1, rankIndex);
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0002FB2C File Offset: 0x0002DD2C
		private static void AlignRankToRight(LineFormation formation, int fileIndex, int rankIndex)
		{
			int num = -1;
			while (fileIndex > 0)
			{
				if (formation._units2D[fileIndex, rankIndex] == null && formation.IsUnitPositionAvailable(fileIndex, rankIndex))
				{
					num = fileIndex;
					break;
				}
				fileIndex--;
			}
			int num2 = -1;
			while (fileIndex >= 0)
			{
				if (formation._units2D[fileIndex, rankIndex] != null)
				{
					num2 = fileIndex;
					break;
				}
				fileIndex--;
			}
			if (num != -1 && num2 != -1)
			{
				formation.RelocateUnit(formation._units2D[num2, rankIndex], num, rankIndex);
				LineFormation.AlignRankToRight(formation, num - 1, rankIndex);
			}
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0002FBAC File Offset: 0x0002DDAC
		private static void AlignLastRank(LineFormation formation)
		{
			int num = formation.RankCount - 1;
			bool flag;
			int num2;
			int num3;
			formation.GetFlanksOfRank(num, out flag, out num2, out num3, true);
			if (num == 0 && flag)
			{
				return;
			}
			LineFormation.AlignRankToLeft(formation, num2, num);
			bool flag2 = false;
			bool flag3 = false;
			for (;;)
			{
				formation.GetFlanksOfRank(num, out flag, out num2, out num3, true);
				if (!flag2 && num2 < formation.FileCount - num3 - 1 - 1)
				{
					int num4;
					int num5;
					formation.GetFlanksOfRank(num, out flag, out num4, out num5, false);
					formation.RelocateUnit(formation._units2D[num5, num], num3 + 1, num);
					LineFormation.AlignRankToRight(formation, num3 + 1, num);
					flag3 = true;
				}
				else
				{
					if (flag3 || num2 - 1 <= formation.FileCount - num3 - 1)
					{
						break;
					}
					int num6;
					int num7;
					formation.GetFlanksOfRank(num, out flag, out num6, out num7, false);
					formation.RelocateUnit(formation._units2D[num6, num], num2 - 1, num);
					LineFormation.AlignRankToLeft(formation, num2 - 1, num);
					flag2 = true;
				}
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0002FC88 File Offset: 0x0002DE88
		private int CountUnitsAtRank(int rankIndex)
		{
			int num = 0;
			for (int i = 0; i < this.FileCount; i++)
			{
				if (this._units2D[i, rankIndex] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0002FCBC File Offset: 0x0002DEBC
		private bool IsRankEmpty(int rankIndex)
		{
			for (int i = 0; i < this.FileCount; i++)
			{
				if (this._units2D[i, rankIndex] != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0002FCEC File Offset: 0x0002DEEC
		private bool IsFileFullyOccupied(int fileIndex)
		{
			bool flag = true;
			for (int i = 0; i < this.RankCount; i++)
			{
				if (this._units2D[fileIndex, i] == null && this.IsUnitPositionAvailable(fileIndex, i))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0002FD2C File Offset: 0x0002DF2C
		private bool IsRankFullyOccupied(int rankIndex)
		{
			bool flag = true;
			for (int i = 0; i < this.FileCount; i++)
			{
				if (this._units2D[i, rankIndex] == null && this.IsUnitPositionAvailable(i, rankIndex))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0002FD6C File Offset: 0x0002DF6C
		private static IFormationUnit GetUnitToFillIn(LineFormation formation, int relocationFileIndex, int relocationRankIndex)
		{
			int i = formation.RankCount - 1;
			while (i >= 0)
			{
				if (relocationRankIndex == i)
				{
					return null;
				}
				bool flag;
				int num;
				int num2;
				formation.GetFlanksOfRank(i, out flag, out num, out num2, false);
				if (!flag)
				{
					if (relocationFileIndex > num2)
					{
						return formation._units2D[num2, i];
					}
					if (relocationFileIndex < num)
					{
						return formation._units2D[num, i];
					}
					if (num2 - relocationFileIndex > relocationFileIndex - num)
					{
						return formation._units2D[num, i];
					}
					return formation._units2D[num2, i];
				}
				else
				{
					i--;
				}
			}
			Debug.FailedAssert("This line should not be reached.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\LineFormation.cs", "GetUnitToFillIn", 3202);
			return null;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0002FE08 File Offset: 0x0002E008
		protected void RelocateUnit(IFormationUnit unit, int fileIndex, int rankIndex)
		{
			this._units2D[unit.FormationFileIndex, unit.FormationRankIndex] = null;
			this._units2D[fileIndex, rankIndex] = unit;
			this.ReconstructUnitsFromUnits2D();
			unit.FormationFileIndex = fileIndex;
			unit.FormationRankIndex = rankIndex;
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0002FE5F File Offset: 0x0002E05F
		public int UnitCount
		{
			get
			{
				return this.GetAllUnits().Count;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0002FE6C File Offset: 0x0002E06C
		public int PositionedUnitCount
		{
			get
			{
				return this.UnitCount - this._unpositionedUnits.Count;
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0002FE80 File Offset: 0x0002E080
		public IFormationUnit GetPlayerUnit()
		{
			return this._allUnits.FirstOrDefault<IFormationUnit>((IFormationUnit unit) => unit.IsPlayerUnit);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0002FEAC File Offset: 0x0002E0AC
		public MBReadOnlyList<IFormationUnit> GetAllUnits()
		{
			return this._allUnits;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0002FEB4 File Offset: 0x0002E0B4
		public void GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn)
		{
			allUnitsListToBeFilledIn.Clear();
			allUnitsListToBeFilledIn.AddRange(this._allUnits);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0002FECA File Offset: 0x0002E0CA
		public MBList<IFormationUnit> GetUnpositionedUnits()
		{
			return this._unpositionedUnits;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002FED4 File Offset: 0x0002E0D4
		public Vec2? GetLocalDirectionOfRelativeFormationLocation(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			Vec2 vec = new Vec2((float)unit.FormationFileIndex, (float)(-(float)unit.FormationRankIndex)) - new Vec2((float)(this.FileCount - 1) * 0.5f, (float)(this.RankCount - 1) * -0.5f);
			vec.Normalize();
			return new Vec2?(vec);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002FF44 File Offset: 0x0002E144
		public Vec2? GetLocalWallDirectionOfRelativeFormationLocation(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			Vec2 vec = new Vec2((float)unit.FormationFileIndex, (float)(-(float)unit.FormationRankIndex)) - new Vec2((float)(this.FileCount - 1) * 0.5f, (float)(-(float)this.RankCount));
			vec.Normalize();
			return new Vec2?(vec);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0002FFAD File Offset: 0x0002E1AD
		public void GetFormationInfo(out int fileCount, out int rankCount)
		{
			fileCount = this.FileCount;
			rankCount = this.RankCount;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0002FFC0 File Offset: 0x0002E1C0
		[Conditional("DEBUG")]
		private void AssertUnit(IFormationUnit unit, bool isAssertingUnitPositionAvailability = true)
		{
			if (isAssertingUnitPositionAvailability)
			{
				this.IsUnitPositionRestrained(unit.FormationFileIndex, unit.FormationRankIndex);
				if (this._isMiddleFrontUnitPositionReserved && this.GetMiddleFrontUnitPosition().Item1 == unit.FormationFileIndex)
				{
					bool flag = this.GetMiddleFrontUnitPosition().Item2 == unit.FormationRankIndex;
				}
				this.IsUnitPositionAvailable(unit.FormationFileIndex, unit.FormationRankIndex);
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0003002E File Offset: 0x0002E22E
		[Conditional("DEBUG")]
		private void AssertUnpositionedUnit(IFormationUnit unit)
		{
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00030030 File Offset: 0x0002E230
		public float GetUnitsDistanceToFrontLine(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return -1f;
			}
			return (float)unit.FormationRankIndex * (this.Distance + this.UnitDiameter) + this.UnitDiameter * 0.5f;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00030068 File Offset: 0x0002E268
		public IFormationUnit GetNeighborUnitOfLeftSide(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			int formationRankIndex = unit.FormationRankIndex;
			for (int i = unit.FormationFileIndex - 1; i >= 0; i--)
			{
				if (this._units2D[i, formationRankIndex] != null)
				{
					return this._units2D[i, formationRankIndex];
				}
			}
			return null;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x000300C0 File Offset: 0x0002E2C0
		public IFormationUnit GetNeighborUnitOfRightSide(IFormationUnit unit)
		{
			if (this._unpositionedUnits.Contains(unit))
			{
				return null;
			}
			int formationRankIndex = unit.FormationRankIndex;
			for (int i = unit.FormationFileIndex + 1; i < this.FileCount; i++)
			{
				if (this._units2D[i, formationRankIndex] != null)
				{
					return this._units2D[i, formationRankIndex];
				}
			}
			return null;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0003011C File Offset: 0x0002E31C
		public void SwitchUnitLocationsWithUnpositionedUnit(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			int formationFileIndex = firstUnit.FormationFileIndex;
			int formationRankIndex = firstUnit.FormationRankIndex;
			this._unpositionedUnits.Remove(secondUnit);
			this._units2D[formationFileIndex, formationRankIndex] = secondUnit;
			secondUnit.FormationFileIndex = formationFileIndex;
			secondUnit.FormationRankIndex = formationRankIndex;
			firstUnit.FormationFileIndex = -1;
			firstUnit.FormationRankIndex = -1;
			this._unpositionedUnits.Add(firstUnit);
			this.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00030190 File Offset: 0x0002E390
		public void SwitchUnitLocations(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			int formationFileIndex = firstUnit.FormationFileIndex;
			int formationRankIndex = firstUnit.FormationRankIndex;
			int formationFileIndex2 = secondUnit.FormationFileIndex;
			int formationRankIndex2 = secondUnit.FormationRankIndex;
			this._units2D[formationFileIndex, formationRankIndex] = secondUnit;
			this._units2D[formationFileIndex2, formationRankIndex2] = firstUnit;
			this.ReconstructUnitsFromUnits2D();
			firstUnit.FormationFileIndex = formationFileIndex2;
			firstUnit.FormationRankIndex = formationRankIndex2;
			secondUnit.FormationFileIndex = formationFileIndex;
			secondUnit.FormationRankIndex = formationRankIndex;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000301F8 File Offset: 0x0002E3F8
		public void SwitchUnitLocationsWithBackMostUnit(IFormationUnit unit)
		{
			IFormationUnit lastUnit = this.GetLastUnit();
			if (lastUnit != null && unit != null && unit != lastUnit)
			{
				this.SwitchUnitLocations(unit, lastUnit);
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0003021E File Offset: 0x0002E41E
		public void BeforeFormationFrameChange()
		{
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00030220 File Offset: 0x0002E420
		public void BatchUnitPositionAvailabilities(bool isUpdatingCachedOrderedLocalPositions = true)
		{
			if (isUpdatingCachedOrderedLocalPositions)
			{
				this.AreLocalPositionsDirty = true;
			}
			bool areLocalPositionsDirty = this.AreLocalPositionsDirty;
			this.AreLocalPositionsDirty = false;
			if (areLocalPositionsDirty)
			{
				this._cachedOrderedUnitPositionIndices.Clear();
				for (int i = 0; i < this.FileCount * this.RankCount; i++)
				{
					this._cachedOrderedUnitPositionIndices.Add(this.GetOrderedUnitPositionIndex(i));
				}
				this._cachedOrderedLocalPositions.Clear();
				for (int j = 0; j < this._cachedOrderedUnitPositionIndices.Count; j++)
				{
					Vec2i vec2i = this._cachedOrderedUnitPositionIndices[j];
					this._cachedOrderedLocalPositions.Add(this.GetLocalPositionOfUnit(vec2i.Item1, vec2i.Item2));
				}
			}
			this.MakeRestrainedPositionsUnavailable();
			if (!this.owner.BatchUnitPositions(this._cachedOrderedUnitPositionIndices, this._cachedOrderedLocalPositions, this.UnitPositionAvailabilities, this._globalPositions, this.FileCount, this.RankCount))
			{
				for (int k = 0; k < this.FileCount; k++)
				{
					for (int l = 0; l < this.RankCount; l++)
					{
						this.UnitPositionAvailabilities[k, l] = 1;
					}
				}
			}
			else
			{
				(this.owner as Formation).SetHasPendingUnitPositions(true);
			}
			if (areLocalPositionsDirty)
			{
				this._cachedOrderedAndAvailableUnitPositionIndices.Clear();
				for (int m = 0; m < this._cachedOrderedUnitPositionIndices.Count; m++)
				{
					Vec2i vec2i2 = this._cachedOrderedUnitPositionIndices[m];
					if (this.UnitPositionAvailabilities[vec2i2.Item1, vec2i2.Item2] == 2)
					{
						this._cachedOrderedAndAvailableUnitPositionIndices.Add(vec2i2);
					}
				}
			}
			Formation formation;
			this._isCavalry = (formation = this.owner as Formation) != null && formation.CalculateHasSignificantNumberOfMounted;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000303D0 File Offset: 0x0002E5D0
		public void OnFormationFrameChanged(bool updateCachedOrderedLocalPositions = false)
		{
			this.UnitPositionAvailabilities.Clear();
			this.BatchUnitPositionAvailabilities(updateCachedOrderedLocalPositions);
			bool flag = LineFormation.ShiftUnitsBackwardsForNewUnavailableUnitPositions(this);
			for (int i = 0; i < this.FileCount; i++)
			{
				LineFormation.FillInTheGapsOfFile(this, i, 0, i == this.FileCount - 1);
			}
			bool flag2 = flag;
			bool flag3 = this.TryReaddingUnpositionedUnits();
			if (flag2 && !flag3)
			{
				this.owner.OnUnitAddedOrRemoved();
			}
			LineFormation.FillInTheGapsOfMiddleRanks(this, null);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0003043C File Offset: 0x0002E63C
		private bool TryReaddingUnpositionedUnits()
		{
			bool flag = this._unpositionedUnits.Count > 0;
			for (int i = this._unpositionedUnits.Count - 1; i >= 0; i--)
			{
				i = MathF.Min(i, this._unpositionedUnits.Count - 1);
				if (i < 0)
				{
					break;
				}
				IFormationUnit formationUnit = this._unpositionedUnits[i];
				this.RemoveUnit(formationUnit);
				if (!this.AddUnit(formationUnit))
				{
					break;
				}
			}
			if (flag)
			{
				this.owner.OnUnitAddedOrRemoved();
			}
			return flag;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x000304B4 File Offset: 0x0002E6B4
		private bool AreLastRanksCompletelyUnavailable(int numberOfRanksToCheck = 3)
		{
			bool flag = true;
			if (this.RankCount < numberOfRanksToCheck)
			{
				flag = false;
			}
			else
			{
				for (int i = 0; i < this.FileCount; i++)
				{
					for (int j = this.RankCount - numberOfRanksToCheck; j < this.RankCount; j++)
					{
						if (this.IsUnitPositionAvailable(i, j))
						{
							i = 2147483646;
							flag = false;
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0003050E File Offset: 0x0002E70E
		public void UpdateLocalPositionErrors(bool recalculateErrors)
		{
			if (recalculateErrors)
			{
				(this.owner as Formation).ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.UpdateLocalPositionError();
				}, null);
			}
			this.OnFormationFrameChanged(true);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0003054C File Offset: 0x0002E74C
		[Conditional("DEBUG")]
		private void AssertUnitPositions()
		{
			for (int i = 0; i < this._units2D.Count1; i++)
			{
				for (int j = 0; j < this._units2D.Count2; j++)
				{
					IFormationUnit formationUnit = this._units2D[i, j];
				}
			}
			foreach (IFormationUnit formationUnit2 in this._unpositionedUnits)
			{
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000305D4 File Offset: 0x0002E7D4
		[Conditional("DEBUG")]
		private void AssertFilePositions(int fileIndex)
		{
			bool flag;
			int num;
			int num2;
			this.GetFrontAndRearOfFile(fileIndex, out flag, out num, out num2, true);
			if (!flag)
			{
				for (int i = num; i <= num2; i++)
				{
				}
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00030600 File Offset: 0x0002E800
		[Conditional("DEBUG")]
		private void AssertRankPositions(int rankIndex)
		{
			bool flag;
			int num;
			int num2;
			this.GetFlanksOfRank(rankIndex, out flag, out num, out num2, true);
			if (!flag)
			{
				for (int i = num; i <= num2; i++)
				{
				}
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0003062C File Offset: 0x0002E82C
		public void OnFormationDispersed()
		{
			IEnumerable<Vec2i> enumerable = from i in this.GetOrderedUnitPositionIndices()
				where this.IsUnitPositionAvailable(i.Item1, i.Item2)
				select i;
			MBList<IFormationUnit> mblist = this.GetAllUnits().ToMBList<IFormationUnit>();
			foreach (Vec2i vec2i in enumerable)
			{
				int item = vec2i.Item1;
				int item2 = vec2i.Item2;
				IFormationUnit formationUnit = this._units2D[item, item2];
				if (formationUnit != null)
				{
					IFormationUnit closestUnitTo = this.owner.GetClosestUnitTo(this.GetLocalPositionOfUnit(item, item2), mblist, null);
					mblist[mblist.IndexOf(closestUnitTo)] = null;
					if (formationUnit != closestUnitTo)
					{
						if (closestUnitTo.FormationFileIndex == -1)
						{
							this.SwitchUnitLocationsWithUnpositionedUnit(formationUnit, closestUnitTo);
						}
						else
						{
							this.SwitchUnitLocations(formationUnit, closestUnitTo);
						}
					}
				}
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00030710 File Offset: 0x0002E910
		public void OnUnitLostMount(IFormationUnit unit)
		{
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00030714 File Offset: 0x0002E914
		public bool IsTurnBackwardsNecessary(Vec2 previousPosition, WorldPosition? newPosition, Vec2 previousDirection, bool hasNewDirection, Vec2? newDirection)
		{
			return hasNewDirection && MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(newDirection.Value.RotationInRadians, previousDirection.RotationInRadians)) >= 2.3561945f;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00030754 File Offset: 0x0002E954
		public virtual void TurnBackwards()
		{
			for (int i = 0; i <= this.FileCount / 2; i++)
			{
				int num = i;
				int num2 = this.FileCount - 1 - i;
				for (int j = 0; j < this.RankCount; j++)
				{
					int num3 = j;
					int num4 = this.RankCount - 1 - j;
					IFormationUnit formationUnit = this._units2D[num, num3];
					IFormationUnit formationUnit2 = this._units2D[num2, num4];
					if (formationUnit != formationUnit2)
					{
						if (formationUnit != null && formationUnit2 != null)
						{
							this.SwitchUnitLocations(formationUnit, formationUnit2);
						}
						else if (formationUnit != null)
						{
							if (this.IsUnitPositionAvailable(num2, num4))
							{
								this.RelocateUnit(formationUnit, num2, num4);
							}
						}
						else if (formationUnit2 != null && this.IsUnitPositionAvailable(num, num3))
						{
							this.RelocateUnit(formationUnit2, num, num3);
						}
					}
				}
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00030820 File Offset: 0x0002EA20
		public float GetOccupationWidth(int unitCount)
		{
			if (unitCount < 1)
			{
				return 0f;
			}
			int num = this.FileCount - 1;
			int num2 = 0;
			for (int i = 0; i < this.FileCount * this.RankCount; i++)
			{
				Vec2i orderedUnitPositionIndex = this.GetOrderedUnitPositionIndex(i);
				if (orderedUnitPositionIndex.Item1 < num)
				{
					num = orderedUnitPositionIndex.Item1;
				}
				if (orderedUnitPositionIndex.Item1 > num2)
				{
					num2 = orderedUnitPositionIndex.Item1;
				}
				if (this.IsUnitPositionAvailable(orderedUnitPositionIndex.Item1, orderedUnitPositionIndex.Item2))
				{
					unitCount--;
					if (unitCount == 0)
					{
						break;
					}
				}
			}
			return (float)(num2 - num) * (this.Interval + this.UnitDiameter) + this.UnitDiameter;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000308C0 File Offset: 0x0002EAC0
		public void InvalidateCacheOfUnitAux(Vec2 roundedLocalPosition)
		{
			int num;
			int num2;
			if (this.TryGetUnitPositionIndexFromLocalPosition(roundedLocalPosition, out num, out num2))
			{
				this.UnitPositionAvailabilities[num, num2] = 0;
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x000308E8 File Offset: 0x0002EAE8
		public Vec2? CreateNewPosition(int unitIndex)
		{
			Vec2? vec = null;
			int num = 100;
			while (vec == null && num > 0 && !this.AreLastRanksCompletelyUnavailable(3) && this.IsDeepenApplicable())
			{
				this.Deepen();
				vec = this.GetLocalPositionOfUnitOrDefault(unitIndex);
				num--;
			}
			return vec;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00030933 File Offset: 0x0002EB33
		public virtual void RearrangeFrom(IFormationArrangement arrangement)
		{
			this.BatchUnitPositionAvailabilities(true);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0003093C File Offset: 0x0002EB3C
		public virtual void RearrangeTo(IFormationArrangement arrangement)
		{
			if (arrangement is ColumnFormation)
			{
				this.IsTransforming = true;
				this.ReleaseMiddleFrontUnitPosition();
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00030954 File Offset: 0x0002EB54
		public virtual void RearrangeTransferUnits(IFormationArrangement arrangement)
		{
			LineFormation lineFormation;
			if ((lineFormation = arrangement as LineFormation) != null && !(arrangement is TransposedLineFormation))
			{
				lineFormation._units2D = this._units2D;
				lineFormation._allUnits = this._allUnits;
				lineFormation.UnitPositionAvailabilities = this.UnitPositionAvailabilities;
				lineFormation._globalPositions = this._globalPositions;
				lineFormation._unpositionedUnits = this._unpositionedUnits;
				lineFormation.AreLocalPositionsDirty = true;
				lineFormation.OnFormationFrameChanged(false);
				return;
			}
			for (int i = 0; i < this.FileCount * this.RankCount; i++)
			{
				Vec2i orderedUnitPositionIndex = this.GetOrderedUnitPositionIndex(i);
				int item = orderedUnitPositionIndex.Item1;
				int item2 = orderedUnitPositionIndex.Item2;
				IFormationUnit formationUnit = this._units2D[item, item2];
				if (formationUnit != null)
				{
					formationUnit.FormationFileIndex = -1;
					formationUnit.FormationRankIndex = -1;
					arrangement.AddUnit(formationUnit);
				}
			}
			foreach (IFormationUnit formationUnit2 in this._unpositionedUnits)
			{
				arrangement.AddUnit(formationUnit2);
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00030A68 File Offset: 0x0002EC68
		public static float CalculateWidth(float interval, float unitDiameter, int unitCountOnLine)
		{
			return (float)MathF.Max(0, unitCountOnLine - 1) * (interval + unitDiameter) + unitDiameter;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00030A7A File Offset: 0x0002EC7A
		public void FormFromFlankWidth(int unitCountOnLine, bool skipSingleFileChangesForPerformance = false)
		{
			if (skipSingleFileChangesForPerformance && MathF.Abs(this.FileCount - unitCountOnLine) <= 1)
			{
				return;
			}
			this.FlankWidth = LineFormation.CalculateWidth(this.Interval, this.UnitDiameter, unitCountOnLine);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00030AA8 File Offset: 0x0002ECA8
		public void ReserveMiddleFrontUnitPosition(IFormationUnit vanguard)
		{
			this._isMiddleFrontUnitPositionReserved = true;
			this.OnFormationFrameChanged(false);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00030AB8 File Offset: 0x0002ECB8
		public void ReleaseMiddleFrontUnitPosition()
		{
			this._isMiddleFrontUnitPositionReserved = false;
			this.OnFormationFrameChanged(false);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00030AC8 File Offset: 0x0002ECC8
		private Vec2i GetMiddleFrontUnitPosition()
		{
			return this.GetOrderedUnitPositionIndex(0);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00030AD4 File Offset: 0x0002ECD4
		public Vec2 GetLocalPositionOfReservedUnitPosition()
		{
			Vec2i middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
			return this.GetLocalPositionOfUnit(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00030AFC File Offset: 0x0002ECFC
		private void SwitchFrontUnitTypesToFrontRows()
		{
			if (this.Interval <= 0f)
			{
				return;
			}
			for (int i = 1; i < this.RankCount; i++)
			{
				for (int j = 0; j < this.FileCount; j++)
				{
					Agent agent = this._units2D[j, i] as Agent;
					Agent agent2 = this._units2D[j, i - 1] as Agent;
					if (agent != null && agent2 != null && this._isFrontUnitDelegate(agent) && !this._isFrontUnitDelegate(agent2))
					{
						this.SwitchUnitLocations(agent, agent2);
					}
				}
			}
			for (int k = this.RankCount - 1; k > 0; k--)
			{
				for (int l = 0; l < this.FileCount; l++)
				{
					Agent agent3 = this._units2D[l, k] as Agent;
					if (agent3 != null && this._isFrontUnitDelegate(agent3))
					{
						bool flag = false;
						int num = k - 1;
						for (int m = 0; m < this.FileCount; m++)
						{
							Agent agent4 = this._units2D[m, num] as Agent;
							if (agent4 != null && !this._isFrontUnitDelegate(agent4))
							{
								this.SwitchUnitLocations(agent3, agent4);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00030C47 File Offset: 0x0002EE47
		public void OnTickOccasionally()
		{
			this.SwitchFrontUnitTypesToFrontRows();
			this.UpdateFrontUnitTypeDelegate();
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00030C55 File Offset: 0x0002EE55
		protected virtual void UpdateFrontUnitTypeDelegate()
		{
			if ((this.owner as Formation).QuerySystem.IsUnderCavalryChargeFromFront)
			{
				this._isFrontUnitDelegate = new Func<Agent, bool>(this.PreferBracerUnitsOnFront);
				return;
			}
			this._isFrontUnitDelegate = new Func<Agent, bool>(this.PreferShieldedUnitsOnFront);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00030C93 File Offset: 0x0002EE93
		public virtual float GetDirectionChangeTendencyOfUnit(IFormationUnit unit)
		{
			if (this.RankCount == 1 || unit.FormationRankIndex == -1)
			{
				return 0f;
			}
			return (float)unit.FormationRankIndex * 1f / (float)(this.RankCount - 1);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00030CC4 File Offset: 0x0002EEC4
		public int GetCachedOrderedAndAvailableUnitPositionIndicesCount()
		{
			return this._cachedOrderedAndAvailableUnitPositionIndices.Count;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00030CD1 File Offset: 0x0002EED1
		public Vec2i GetCachedOrderedAndAvailableUnitPositionIndexAt(int i)
		{
			return this._cachedOrderedAndAvailableUnitPositionIndices[i];
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00030CDF File Offset: 0x0002EEDF
		public WorldPosition GetGlobalPositionAtIndex(int indexX, int indexY)
		{
			return this._globalPositions[indexX, indexY];
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00030CEE File Offset: 0x0002EEEE
		protected bool PreferShieldedUnitsOnFront(Agent agent)
		{
			return agent.HasShieldCached;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00030CF6 File Offset: 0x0002EEF6
		protected bool PreferBracerUnitsOnFront(Agent agent)
		{
			return agent.CanPerformBraceCached;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00030D0C File Offset: 0x0002EF0C
		void IFormationArrangement.GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn)
		{
			this.GetAllUnits(in allUnitsListToBeFilledIn);
		}

		// Token: 0x040003DE RID: 990
		protected const int UnitPositionAvailabilityValueOfUnprocessed = 0;

		// Token: 0x040003DF RID: 991
		protected const int UnitPositionAvailabilityValueOfUnavailable = 1;

		// Token: 0x040003E0 RID: 992
		protected const int UnitPositionAvailabilityValueOfAvailable = 2;

		// Token: 0x040003E1 RID: 993
		private static readonly Vec2i InvalidPositionIndex = new Vec2i(-1, -1);

		// Token: 0x040003E2 RID: 994
		protected readonly IFormation owner;

		// Token: 0x040003E3 RID: 995
		private MBList2D<IFormationUnit> _units2D;

		// Token: 0x040003E4 RID: 996
		private MBList2D<IFormationUnit> _units2DWorkspace;

		// Token: 0x040003E5 RID: 997
		private MBList<IFormationUnit> _allUnits;

		// Token: 0x040003E6 RID: 998
		private bool _isBatchRemovingUnits;

		// Token: 0x040003E7 RID: 999
		private readonly List<int> _gapFillMinRanksPerFileForBatchRemove = new List<int>();

		// Token: 0x040003E8 RID: 1000
		private bool _batchRemoveInvolvesUnavailablePositions;

		// Token: 0x040003E9 RID: 1001
		private MBList<IFormationUnit> _unpositionedUnits;

		// Token: 0x040003EA RID: 1002
		protected MBList2D<int> UnitPositionAvailabilities;

		// Token: 0x040003EB RID: 1003
		private MBList2D<int> _unitPositionAvailabilitiesWorkspace;

		// Token: 0x040003EC RID: 1004
		private MBList2D<WorldPosition> _globalPositions;

		// Token: 0x040003ED RID: 1005
		private MBList2D<WorldPosition> _globalPositionsWorkspace;

		// Token: 0x040003EE RID: 1006
		private readonly MBWorkspace<MBQueue<ValueTuple<IFormationUnit, int, int>>> _displacedUnitsWorkspace;

		// Token: 0x040003EF RID: 1007
		private readonly MBWorkspace<MBArrayList<Vec2i>> _finalOccupationsWorkspace;

		// Token: 0x040003F0 RID: 1008
		private readonly MBWorkspace<MBQueue<Vec2i>> _toBeFilledInGapsWorkspace;

		// Token: 0x040003F1 RID: 1009
		private readonly MBWorkspace<MBArrayList<Vec2i>> _finalVacanciesWorkspace;

		// Token: 0x040003F2 RID: 1010
		private readonly MBWorkspace<MBArrayList<Vec2i>> _filledInGapsWorkspace;

		// Token: 0x040003F3 RID: 1011
		private readonly MBWorkspace<MBArrayList<Vec2i>> _toBeEmptiedOutUnitPositionsWorkspace;

		// Token: 0x040003F4 RID: 1012
		private MBArrayList<bool> _filledInUnitPositionsTable;

		// Token: 0x040003F5 RID: 1013
		private MBArrayList<Vec2i> _cachedOrderedUnitPositionIndices;

		// Token: 0x040003F6 RID: 1014
		private MBArrayList<Vec2i> _cachedOrderedAndAvailableUnitPositionIndices;

		// Token: 0x040003F7 RID: 1015
		private MBArrayList<Vec2> _cachedOrderedLocalPositions;

		// Token: 0x040003F8 RID: 1016
		private Func<LineFormation, int, int, bool> _shiftUnitsBackwardsPredicateDelegate;

		// Token: 0x040003F9 RID: 1017
		private Func<LineFormation, int, int, bool> _shiftUnitsForwardsPredicateDelegate;

		// Token: 0x040003FA RID: 1018
		protected Func<Agent, bool> _isFrontUnitDelegate;

		// Token: 0x040003FC RID: 1020
		private bool _isCavalry;

		// Token: 0x040003FD RID: 1021
		private bool _isStaggered = true;

		// Token: 0x04000400 RID: 1024
		private readonly bool _isDeformingOnWidthChange;

		// Token: 0x04000401 RID: 1025
		private bool _isMiddleFrontUnitPositionReserved;

		// Token: 0x04000402 RID: 1026
		protected bool IsTransforming;
	}
}
