using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000143 RID: 323
	public class ColumnFormation : IFormationArrangement
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0002A59B File Offset: 0x0002879B
		// (set) Token: 0x06000F94 RID: 3988 RVA: 0x0002A5A3 File Offset: 0x000287A3
		public IFormationUnit Vanguard
		{
			get
			{
				return this._vanguard;
			}
			private set
			{
				this.SetVanguard(value);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0002A5AC File Offset: 0x000287AC
		// (set) Token: 0x06000F96 RID: 3990 RVA: 0x0002A5B4 File Offset: 0x000287B4
		public int ColumnCount
		{
			get
			{
				return this.FileCount;
			}
			set
			{
				this.SetColumnCount(value);
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0002A5BD File Offset: 0x000287BD
		protected int FileCount
		{
			get
			{
				return this._units2D.Count1;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0002A5CA File Offset: 0x000287CA
		public int RankCount
		{
			get
			{
				return this._units2D.Count2;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0002A5D7 File Offset: 0x000287D7
		public int VanguardFileIndex
		{
			get
			{
				if (this.FileCount % 2 != 0)
				{
					return this.FileCount / 2;
				}
				if (this.isExpandingFromRightSide)
				{
					return this.FileCount / 2 - 1;
				}
				return this.FileCount / 2;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0002A607 File Offset: 0x00028807
		protected float Distance
		{
			get
			{
				return this.owner.Distance;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0002A614 File Offset: 0x00028814
		public float DistanceMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0002A61B File Offset: 0x0002881B
		protected float Interval
		{
			get
			{
				return this.owner.Interval;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0002A628 File Offset: 0x00028828
		public float IntervalMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0002A630 File Offset: 0x00028830
		public ColumnFormation(IFormation ownerFormation, IFormationUnit vanguard = null, int columnCount = 1)
		{
			this.owner = ownerFormation;
			this._units2D = new MBList2D<IFormationUnit>(columnCount, 1);
			this._units2DWorkspace = new MBList2D<IFormationUnit>(columnCount, 1);
			this.ReconstructUnitsFromUnits2D();
			this._vanguard = vanguard;
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002A688 File Offset: 0x00028888
		public IFormationArrangement Clone(IFormation formation)
		{
			return new ColumnFormation(formation, this.Vanguard, this.ColumnCount);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0002A69C File Offset: 0x0002889C
		public void DeepCopyFrom(IFormationArrangement arrangement)
		{
			this.UnitPositionsOnVanguardFileIndex = (arrangement as ColumnFormation).GetUnitPositionsOnVanguardFileIndex();
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0002A6AF File Offset: 0x000288AF
		// (set) Token: 0x06000FA2 RID: 4002 RVA: 0x0002A6B7 File Offset: 0x000288B7
		public float Width
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

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0002A6C0 File Offset: 0x000288C0
		// (set) Token: 0x06000FA4 RID: 4004 RVA: 0x0002A6F0 File Offset: 0x000288F0
		public float FlankWidth
		{
			get
			{
				return (float)(this.FileCount - 1) * (this.owner.Interval + this.owner.UnitDiameter) + this.owner.UnitDiameter;
			}
			set
			{
				int num = MathF.Max(0, (int)((value - this.owner.UnitDiameter) / (this.owner.Interval + this.owner.UnitDiameter) + 1E-05f)) + 1;
				num = MathF.Max(num, 1);
				this.SetColumnCount(num);
				Action onWidthChanged = this.OnWidthChanged;
				if (onWidthChanged == null)
				{
					return;
				}
				onWidthChanged();
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0002A751 File Offset: 0x00028951
		// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x0002A759 File Offset: 0x00028959
		public List<Vec2> UnitPositionsOnVanguardFileIndex { get; private set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0002A762 File Offset: 0x00028962
		public float Depth
		{
			get
			{
				return this.RankDepth;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0002A76A File Offset: 0x0002896A
		public float RankDepth
		{
			get
			{
				return (float)(this.RankCount - 1) * (this.Distance + this.owner.UnitDiameter) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0002A794 File Offset: 0x00028994
		public float MinimumWidth
		{
			get
			{
				return this.MinimumFlankWidth;
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0002A79C File Offset: 0x0002899C
		public IFormationUnit GetPlayerUnit()
		{
			return this._allUnits.FirstOrDefault<IFormationUnit>((IFormationUnit unit) => unit.IsPlayerUnit);
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0002A7C8 File Offset: 0x000289C8
		public float MaximumWidth
		{
			get
			{
				return (float)(this.UnitCount - 1) * (this.owner.UnitDiameter + this.owner.Interval) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0002A7F8 File Offset: 0x000289F8
		public float MinimumFlankWidth
		{
			get
			{
				return (float)(MathF.Max(1, MathF.Ceiling(MathF.Sqrt((float)(this.UnitCount / ColumnFormation.ArrangementAspectRatio)))) - 1) * (this.owner.UnitDiameter + this.owner.Interval) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0002A849 File Offset: 0x00028A49
		public MBReadOnlyList<IFormationUnit> GetAllUnits()
		{
			return this._allUnits;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0002A851 File Offset: 0x00028A51
		public void GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn)
		{
			allUnitsListToBeFilledIn.Clear();
			allUnitsListToBeFilledIn.AddRange(this._allUnits);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0002A867 File Offset: 0x00028A67
		public MBList<IFormationUnit> GetUnpositionedUnits()
		{
			return null;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0002A86A File Offset: 0x00028A6A
		public bool? IsLoose
		{
			get
			{
				return new bool?(false);
			}
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002A874 File Offset: 0x00028A74
		private bool IsUnitPositionAvailable(int fileIndex, int rankIndex)
		{
			if (this.IsMiddleFrontUnitPositionReserved)
			{
				ValueTuple<int, int> middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
				if (fileIndex == middleFrontUnitPosition.Item1 && rankIndex == middleFrontUnitPosition.Item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002A8A8 File Offset: 0x00028AA8
		private bool GetNextVacancy(out int fileIndex, out int rankIndex)
		{
			if (this.RankCount == 0)
			{
				fileIndex = -1;
				rankIndex = -1;
				return false;
			}
			rankIndex = this.RankCount - 1;
			for (int i = 0; i < this.ColumnCount; i++)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide ^ (this.ColumnCount % 2 == 1));
				fileIndex = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				if (this._units2D[fileIndex, rankIndex] == null && this.IsUnitPositionAvailable(fileIndex, rankIndex))
				{
					return true;
				}
			}
			fileIndex = -1;
			rankIndex = -1;
			return false;
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002A92C File Offset: 0x00028B2C
		private IFormationUnit GetLastUnit()
		{
			if (this.RankCount == 0)
			{
				return null;
			}
			int num = this.RankCount - 1;
			for (int i = this.ColumnCount - 1; i >= 0; i--)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
				int num2 = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				IFormationUnit formationUnit = this._units2D[num2, num];
				if (formationUnit != null)
				{
					return formationUnit;
				}
			}
			return null;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002A98C File Offset: 0x00028B8C
		private void Deepen()
		{
			ColumnFormation.Deepen(this);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0002A994 File Offset: 0x00028B94
		private void ReconstructUnitsFromUnits2D()
		{
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
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0002AA14 File Offset: 0x00028C14
		private static void Deepen(ColumnFormation formation)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount + 1);
			for (int i = 0; i < formation.FileCount; i++)
			{
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, i, 0, formation.RankCount);
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002AA96 File Offset: 0x00028C96
		private void Shorten()
		{
			ColumnFormation.Shorten(this);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0002AAA0 File Offset: 0x00028CA0
		private static void Shorten(ColumnFormation formation)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount - 1);
			for (int i = 0; i < formation.FileCount; i++)
			{
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, i, 0, formation.RankCount - 1);
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002AB24 File Offset: 0x00028D24
		public bool AddUnit(IFormationUnit unit)
		{
			int num = 0;
			bool flag = false;
			while (!flag && num < 100)
			{
				num++;
				if (num > 10)
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\ColumnFormation.cs", "AddUnit", 382);
				}
				int num2;
				int num3;
				if (this.GetNextVacancy(out num2, out num3))
				{
					unit.FormationFileIndex = num2;
					unit.FormationRankIndex = num3;
					this._units2D[num2, num3] = unit;
					this.ReconstructUnitsFromUnits2D();
					flag = true;
				}
				else
				{
					this.Deepen();
				}
			}
			if (flag)
			{
				int num4;
				IFormationUnit unitToFollow = this.GetUnitToFollow(unit, out num4);
				this.SetUnitToFollow(unit, unitToFollow, num4);
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged != null)
				{
					onShapeChanged();
				}
			}
			return flag;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0002ABC6 File Offset: 0x00028DC6
		private IFormationUnit TryGetUnit(int fileIndex, int rankIndex)
		{
			if (fileIndex >= 0 && fileIndex < this.FileCount && rankIndex >= 0 && rankIndex < this.RankCount)
			{
				return this._units2D[fileIndex, rankIndex];
			}
			return null;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0002ABF4 File Offset: 0x00028DF4
		private void AdjustFollowDataOfUnitPosition(int fileIndex, int rankIndex)
		{
			IFormationUnit formationUnit = this._units2D[fileIndex, rankIndex];
			if (fileIndex == this.VanguardFileIndex)
			{
				if (formationUnit != null)
				{
					IFormationUnit formationUnit2 = this.TryGetUnit(fileIndex, rankIndex - 1);
					this.SetUnitToFollow(formationUnit, formationUnit2 ?? this.Vanguard, 0);
				}
				for (int i = 1; i < this.ColumnCount; i++)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
					IFormationUnit formationUnit3 = this._units2D[fileIndex + columnOffsetFromColumnIndex, rankIndex];
					if (formationUnit3 != null)
					{
						this.SetUnitToFollow(formationUnit3, formationUnit ?? this.Vanguard, columnOffsetFromColumnIndex);
					}
				}
				IFormationUnit formationUnit4 = this.TryGetUnit(fileIndex, rankIndex + 1);
				if (formationUnit4 != null)
				{
					this.SetUnitToFollow(formationUnit4, formationUnit ?? this.Vanguard, 0);
					return;
				}
			}
			else if (formationUnit != null)
			{
				IFormationUnit formationUnit5 = this._units2D[this.VanguardFileIndex, rankIndex];
				int columnOffsetFromColumnIndex2 = ColumnFormation.GetColumnOffsetFromColumnIndex(fileIndex, this.isExpandingFromRightSide);
				this.SetUnitToFollow(formationUnit, formationUnit5 ?? this.Vanguard, columnOffsetFromColumnIndex2);
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0002ACE4 File Offset: 0x00028EE4
		private void ShiftUnitsForward(int fileIndex, int rankIndex)
		{
			for (;;)
			{
				IFormationUnit formationUnit = this.TryGetUnit(fileIndex, rankIndex + 1);
				if (formationUnit == null)
				{
					break;
				}
				IFormationUnit formationUnit2 = formationUnit;
				int formationRankIndex = formationUnit2.FormationRankIndex;
				formationUnit2.FormationRankIndex = formationRankIndex - 1;
				this._units2D[fileIndex, rankIndex] = formationUnit;
				this._units2D[fileIndex, rankIndex + 1] = null;
				this.ReconstructUnitsFromUnits2D();
				this.AdjustFollowDataOfUnitPosition(fileIndex, rankIndex);
				rankIndex++;
			}
			int num = 0;
			if (rankIndex == this.RankCount - 1)
			{
				for (int i = 0; i < this.ColumnCount; i++)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
					if (this.VanguardFileIndex + columnOffsetFromColumnIndex == fileIndex)
					{
						num = i + 1;
					}
				}
			}
			IFormationUnit formationUnit3 = null;
			for (int j = this.ColumnCount - 1; j >= num; j--)
			{
				int columnOffsetFromColumnIndex2 = ColumnFormation.GetColumnOffsetFromColumnIndex(j, this.isExpandingFromRightSide);
				int num2 = this.VanguardFileIndex + columnOffsetFromColumnIndex2;
				formationUnit3 = this._units2D[num2, this.RankCount - 1];
				if (formationUnit3 != null)
				{
					break;
				}
			}
			if (formationUnit3 != null)
			{
				this._units2D[formationUnit3.FormationFileIndex, formationUnit3.FormationRankIndex] = null;
				formationUnit3.FormationFileIndex = fileIndex;
				formationUnit3.FormationRankIndex = rankIndex;
				this._units2D[fileIndex, rankIndex] = formationUnit3;
				this.ReconstructUnitsFromUnits2D();
				this.AdjustFollowDataOfUnitPosition(fileIndex, rankIndex);
			}
			if (this.IsLastRankEmpty())
			{
				this.Shorten();
			}
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002AE34 File Offset: 0x00029034
		private void ShiftUnitsBackwardForMakingRoomForVanguard(int fileIndex, int rankIndex)
		{
			if (this.RankCount == 1)
			{
				bool flag = false;
				int num = -1;
				for (int i = 0; i < this.ColumnCount; i++)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
					if (this._units2D[this.VanguardFileIndex + columnOffsetFromColumnIndex, 0] == null)
					{
						flag = true;
						num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
						break;
					}
				}
				if (flag)
				{
					IFormationUnit formationUnit = this._units2D[fileIndex, rankIndex];
					this._units2D[fileIndex, rankIndex] = null;
					this._units2D[num, 0] = formationUnit;
					this.ReconstructUnitsFromUnits2D();
					formationUnit.FormationFileIndex = num;
					formationUnit.FormationRankIndex = 0;
					return;
				}
				ColumnFormation.Deepen(this);
				IFormationUnit formationUnit2 = this._units2D[fileIndex, rankIndex];
				this._units2D[fileIndex, rankIndex] = null;
				this._units2D[fileIndex, rankIndex + 1] = formationUnit2;
				this.ReconstructUnitsFromUnits2D();
				IFormationUnit formationUnit3 = formationUnit2;
				int num2 = formationUnit3.FormationRankIndex;
				formationUnit3.FormationRankIndex = num2 + 1;
				return;
			}
			else
			{
				int num3 = rankIndex;
				IFormationUnit formationUnit4 = null;
				for (rankIndex = this.RankCount - 1; rankIndex >= num3; rankIndex--)
				{
					IFormationUnit formationUnit5 = this._units2D[fileIndex, rankIndex];
					this.TryGetUnit(fileIndex, rankIndex + 1);
					this._units2D[fileIndex, rankIndex] = null;
					if (rankIndex + 1 < this.RankCount)
					{
						IFormationUnit formationUnit6 = formationUnit5;
						int num2 = formationUnit6.FormationRankIndex;
						formationUnit6.FormationRankIndex = num2 + 1;
						this._units2D[fileIndex, rankIndex + 1] = formationUnit5;
					}
					else
					{
						formationUnit4 = formationUnit5;
						if (formationUnit4 != null)
						{
							formationUnit4.FormationFileIndex = -1;
							formationUnit4.FormationRankIndex = -1;
						}
					}
					this.ReconstructUnitsFromUnits2D();
				}
				for (rankIndex = this.RankCount - 1; rankIndex >= num3; rankIndex--)
				{
					this.AdjustFollowDataOfUnitPosition(fileIndex, rankIndex);
				}
				if (formationUnit4 != null)
				{
					this.AddUnit(formationUnit4);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged == null)
				{
					return;
				}
				onShapeChanged();
				return;
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0002AFF8 File Offset: 0x000291F8
		private bool IsLastRankEmpty()
		{
			if (this.RankCount == 0)
			{
				return false;
			}
			for (int i = 0; i < this.FileCount; i++)
			{
				if (this._units2D[i, this.RankCount - 1] != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0002B03C File Offset: 0x0002923C
		public void RemoveUnit(IFormationUnit unit)
		{
			int formationFileIndex = unit.FormationFileIndex;
			int formationRankIndex = unit.FormationRankIndex;
			if (GameNetwork.IsServer)
			{
				MBDebug.Print(string.Concat(new object[] { "Removing unit at ", formationFileIndex, " ", formationRankIndex, " from column arrangement\nFileCount&RankCount: ", this.FileCount, " ", this.RankCount }), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this._units2D[unit.FormationFileIndex, unit.FormationRankIndex] = null;
			this.ReconstructUnitsFromUnits2D();
			this.ShiftUnitsForward(unit.FormationFileIndex, unit.FormationRankIndex);
			if (this.IsLastRankEmpty())
			{
				this.Shorten();
			}
			unit.FormationFileIndex = -1;
			unit.FormationRankIndex = -1;
			this.SetUnitToFollow(unit, null, 0);
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged != null)
			{
				onShapeChanged();
			}
			if (this.Vanguard == unit && !((Agent)unit).IsActive())
			{
				this._vanguard = null;
				if (this.FileCount > 0 && this.RankCount > 0)
				{
					this.AdjustFollowDataOfUnitPosition(formationFileIndex, formationRankIndex);
				}
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002B165 File Offset: 0x00029365
		public IFormationUnit GetUnit(int fileIndex, int rankIndex)
		{
			return this._units2D[fileIndex, rankIndex];
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0002B174 File Offset: 0x00029374
		public void OnBatchRemoveStart()
		{
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0002B176 File Offset: 0x00029376
		public void OnBatchRemoveEnd()
		{
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0002B178 File Offset: 0x00029378
		[Conditional("DEBUG")]
		private void AssertUnitPositions()
		{
			for (int i = 0; i < this.FileCount; i++)
			{
				for (int j = 0; j < this.RankCount; j++)
				{
					IFormationUnit formationUnit = this._units2D[i, j];
				}
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0002B1B8 File Offset: 0x000293B8
		[Conditional("DEBUG")]
		private void AssertUnit(IFormationUnit unit, bool isAssertingFollowed = true)
		{
			if (unit == null)
			{
				return;
			}
			if (isAssertingFollowed)
			{
				int num;
				this.GetUnitToFollow(unit, out num);
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002B1D8 File Offset: 0x000293D8
		private static int GetColumnOffsetFromColumnIndex(int columnIndex, bool isExpandingFromRightSide)
		{
			int num;
			if (isExpandingFromRightSide)
			{
				num = (columnIndex + 1) / 2 * ((columnIndex % 2 == 0) ? (-1) : 1);
			}
			else
			{
				num = (columnIndex + 1) / 2 * ((columnIndex % 2 == 0) ? 1 : (-1));
			}
			return num;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0002B20C File Offset: 0x0002940C
		private IFormationUnit GetUnitToFollow(IFormationUnit unit, out int columnOffset)
		{
			IFormationUnit formationUnit;
			if (unit.FormationFileIndex == this.VanguardFileIndex)
			{
				columnOffset = 0;
				if (unit.FormationRankIndex > 0)
				{
					formationUnit = this._units2D[unit.FormationFileIndex, unit.FormationRankIndex - 1];
				}
				else
				{
					formationUnit = null;
				}
			}
			else
			{
				columnOffset = unit.FormationFileIndex - this.VanguardFileIndex;
				formationUnit = this._units2D[this.VanguardFileIndex, unit.FormationRankIndex];
			}
			if (formationUnit == null)
			{
				formationUnit = this.Vanguard;
			}
			return formationUnit;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0002B285 File Offset: 0x00029485
		private IEnumerable<ValueTuple<int, int>> GetOrderedUnitPositionIndices()
		{
			int num2;
			for (int rankIndex = 0; rankIndex < this.RankCount; rankIndex = num2 + 1)
			{
				for (int columnIndex = 0; columnIndex < this.ColumnCount; columnIndex = num2 + 1)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(columnIndex, this.isExpandingFromRightSide);
					int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
					yield return new ValueTuple<int, int>(num, rankIndex);
					num2 = columnIndex;
				}
				num2 = rankIndex;
			}
			yield break;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0002B298 File Offset: 0x00029498
		private Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			if (this.UnitPositionsOnVanguardFileIndex == null)
			{
				this.UnitPositionsOnVanguardFileIndex = this.GetUnitPositionsOnVanguardFileIndex();
			}
			Vec2 orderPosition = (this.owner as Formation).OrderPosition;
			List<Vec2> unitPositionsOnVanguardFileIndex = this.UnitPositionsOnVanguardFileIndex;
			unitPositionsOnVanguardFileIndex.Insert(0, orderPosition);
			float num = this.Distance + this.owner.UnitDiameter;
			int i = rankIndex;
			int num2 = 1;
			Vec2 vec = unitPositionsOnVanguardFileIndex[0];
			Vec2 vec2 = vec - unitPositionsOnVanguardFileIndex[num2];
			float num3 = vec2.Normalize();
			while (i > 0)
			{
				if (num3 >= num)
				{
					vec += -vec2 * num;
					num3 -= num;
				}
				else
				{
					float num4 = num - num3;
					vec += -vec2 * num3;
					if (++num2 < unitPositionsOnVanguardFileIndex.Count)
					{
						vec2 = vec - unitPositionsOnVanguardFileIndex[num2];
					}
					num3 = vec2.Normalize();
					vec += -vec2 * num4;
					num3 -= num4;
				}
				i--;
			}
			float num5 = (float)(this.FileCount - 1) * (this.Interval + this.owner.UnitDiameter);
			Vec2 vec3 = -vec2.TransformToParentUnitF(new Vec2((float)fileIndex * (this.Interval + this.owner.UnitDiameter) - num5 / 2f, 0f));
			vec += vec3;
			Vec2 vec4 = (this.owner as Formation).Direction.TransformToLocalUnitF(vec - unitPositionsOnVanguardFileIndex[0]);
			unitPositionsOnVanguardFileIndex.RemoveAt(0);
			return vec4;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0002B438 File Offset: 0x00029638
		private Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			return Vec2.Forward;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0002B440 File Offset: 0x00029640
		private WorldPosition? GetWorldPositionOfUnit(int fileIndex, int rankIndex)
		{
			return null;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0002B458 File Offset: 0x00029658
		public Vec2? GetLocalPositionOfUnitOrDefault(int unitIndex)
		{
			ValueTuple<int, int> valueTuple = this.GetOrderedUnitPositionIndices().ElementAtOrValue(unitIndex, new ValueTuple<int, int>(-1, -1));
			Vec2? vec;
			if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				vec = new Vec2?(this.GetLocalPositionOfUnit(item, item2));
			}
			else
			{
				vec = null;
			}
			return vec;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0002B4B4 File Offset: 0x000296B4
		public Vec2? GetLocalDirectionOfUnitOrDefault(int unitIndex)
		{
			ValueTuple<int, int> valueTuple = (from i in this.GetOrderedUnitPositionIndices()
				where this.IsUnitPositionAvailable(i.Item1, i.Item2)
				select i).ElementAtOrValue(unitIndex, new ValueTuple<int, int>(-1, -1));
			Vec2? vec;
			if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				vec = new Vec2?(this.GetLocalDirectionOfUnit(item, item2));
			}
			else
			{
				vec = null;
			}
			return vec;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0002B520 File Offset: 0x00029720
		public WorldPosition? GetWorldPositionOfUnitOrDefault(int unitIndex)
		{
			ValueTuple<int, int> valueTuple = (from i in this.GetOrderedUnitPositionIndices()
				where this.IsUnitPositionAvailable(i.Item1, i.Item2)
				select i).ElementAtOrValue(unitIndex, new ValueTuple<int, int>(-1, -1));
			WorldPosition? worldPosition;
			if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				worldPosition = this.GetWorldPositionOfUnit(item, item2);
			}
			else
			{
				worldPosition = null;
			}
			return worldPosition;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0002B586 File Offset: 0x00029786
		public Vec2? GetLocalPositionOfUnitOrDefault(IFormationUnit unit)
		{
			return new Vec2?(this.GetLocalPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0002B59F File Offset: 0x0002979F
		public Vec2? GetLocalPositionOfUnitOrDefaultWithAdjustment(IFormationUnit unit, float distanceBetweenAgentsAdjustment)
		{
			return new Vec2?(this.GetLocalPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0002B5B8 File Offset: 0x000297B8
		public WorldPosition? GetWorldPositionOfUnitOrDefault(IFormationUnit unit)
		{
			return this.GetWorldPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0002B5CC File Offset: 0x000297CC
		public Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			return new Vec2?(this.GetLocalDirectionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0002B5E8 File Offset: 0x000297E8
		public List<IFormationUnit> GetUnitsToPop(int count)
		{
			List<IFormationUnit> list = new List<IFormationUnit>();
			for (int i = this.RankCount - 1; i >= 0; i--)
			{
				for (int j = this.ColumnCount - 1; j >= 0; j--)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(j, this.isExpandingFromRightSide);
					int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
					IFormationUnit formationUnit = this._units2D[num, i];
					if (formationUnit != null)
					{
						list.Add(formationUnit);
						count--;
						if (count == 0)
						{
							return list;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0002B65F File Offset: 0x0002985F
		public List<IFormationUnit> GetUnitsToPop(int count, Vec3 targetPosition)
		{
			return this.GetUnitsToPop(count);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0002B668 File Offset: 0x00029868
		public IEnumerable<IFormationUnit> GetUnitsToPopWithCondition(int count, Func<IFormationUnit, bool> currentCondition)
		{
			int num2;
			for (int rankIndex = this.RankCount - 1; rankIndex >= 0; rankIndex = num2 - 1)
			{
				for (int columnIndex = this.ColumnCount - 1; columnIndex >= 0; columnIndex = num2 - 1)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(columnIndex, this.isExpandingFromRightSide);
					int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
					IFormationUnit formationUnit = this._units2D[num, rankIndex];
					if (formationUnit != null && currentCondition(formationUnit))
					{
						yield return formationUnit;
						num2 = count;
						count = num2 - 1;
						if (count == 0)
						{
							yield break;
						}
					}
					num2 = columnIndex;
				}
				num2 = rankIndex;
			}
			yield break;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0002B686 File Offset: 0x00029886
		public void SwitchUnitLocations(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			this.SwitchUnitLocationsAux(firstUnit, secondUnit);
			this.AdjustFollowDataOfUnitPosition(firstUnit.FormationFileIndex, firstUnit.FormationRankIndex);
			this.AdjustFollowDataOfUnitPosition(secondUnit.FormationFileIndex, secondUnit.FormationRankIndex);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002B6B4 File Offset: 0x000298B4
		private void SwitchUnitLocationsAux(IFormationUnit firstUnit, IFormationUnit secondUnit)
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
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002B72B File Offset: 0x0002992B
		public void SwitchUnitLocationsWithUnpositionedUnit(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			Debug.FailedAssert("Column formation should NOT have an unpositioned unit", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\ColumnFormation.cs", "SwitchUnitLocationsWithUnpositionedUnit", 1215);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002B748 File Offset: 0x00029948
		public void SwitchUnitLocationsWithBackMostUnit(IFormationUnit unit)
		{
			Agent agent;
			if (this.Vanguard == null || (agent = this.Vanguard as Agent) == null || agent != unit)
			{
				IFormationUnit lastUnit = this.GetLastUnit();
				if (lastUnit != null && unit != null && unit != lastUnit)
				{
					this.SwitchUnitLocations(unit, lastUnit);
				}
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0002B789 File Offset: 0x00029989
		public float GetUnitsDistanceToFrontLine(IFormationUnit unit)
		{
			return -1f;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0002B790 File Offset: 0x00029990
		public Vec2? GetLocalDirectionOfRelativeFormationLocation(IFormationUnit unit)
		{
			return null;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0002B7A8 File Offset: 0x000299A8
		public Vec2? GetLocalWallDirectionOfRelativeFormationLocation(IFormationUnit unit)
		{
			return null;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0002B7BE File Offset: 0x000299BE
		public IEnumerable<Vec2> GetUnavailableUnitPositions()
		{
			yield break;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002B7C7 File Offset: 0x000299C7
		public float GetOccupationWidth(int unitCount)
		{
			return this.FlankWidth;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0002B7D0 File Offset: 0x000299D0
		public Vec2? CreateNewPosition(int unitIndex)
		{
			int num = MathF.Ceiling((float)unitIndex * 1f / (float)this.ColumnCount) + ((unitIndex % this.ColumnCount == 0) ? 1 : 0);
			if (num > this.RankCount)
			{
				this._units2D.ResetWithNewCount(this.ColumnCount, num);
				this.ReconstructUnitsFromUnits2D();
			}
			Vec2? localPositionOfUnitOrDefault = this.GetLocalPositionOfUnitOrDefault(unitIndex);
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return localPositionOfUnitOrDefault;
			}
			onShapeChanged();
			return localPositionOfUnitOrDefault;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0002B83A File Offset: 0x00029A3A
		public void InvalidateCacheOfUnitAux(Vec2 roundedLocalPosition)
		{
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0002B83C File Offset: 0x00029A3C
		public void BeforeFormationFrameChange()
		{
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0002B83E File Offset: 0x00029A3E
		public void OnFormationFrameChanged(bool updateCachedOrderedLocalPositions = false)
		{
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002B840 File Offset: 0x00029A40
		private Vec2 CalculateArrangementOrientation()
		{
			IFormationUnit formationUnit = this.Vanguard ?? this._units2D[this.GetMiddleFrontUnitPosition().Item1, this.GetMiddleFrontUnitPosition().Item2];
			if (formationUnit is Agent && this.owner is Formation)
			{
				return ((formationUnit as Agent).Position.AsVec2 - ((Formation)this.owner).CachedMedianPosition.AsVec2).Normalized();
			}
			Debug.FailedAssert("Unexpected case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\ColumnFormation.cs", "CalculateArrangementOrientation", 1300);
			return this.GetLocalDirectionOfUnit(formationUnit.FormationFileIndex, formationUnit.FormationRankIndex);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0002B8F2 File Offset: 0x00029AF2
		public void OnUnitLostMount(IFormationUnit unit)
		{
			this.RemoveUnit(unit);
			this.AddUnit(unit);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0002B904 File Offset: 0x00029B04
		public bool IsTurnBackwardsNecessary(Vec2 previousPosition, WorldPosition? newPosition, Vec2 previousDirection, bool hasNewDirection, Vec2? newDirection)
		{
			return newPosition != null && this.UnitCount > 0 && this.RankCount > 0 && (newPosition.Value.AsVec2 - previousPosition).LengthSquared >= this.RankDepth * this.RankDepth && MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(this.CalculateArrangementOrientation().RotationInRadians, (newPosition.Value.AsVec2 - (this.owner as Formation).CachedMedianPosition.AsVec2).Normalized().RotationInRadians)) >= 2.3561945f;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0002B9C4 File Offset: 0x00029BC4
		public void TurnBackwards()
		{
			if (!this.IsMiddleFrontUnitPositionReserved && !this._isMiddleFrontUnitPositionUsedByVanguardInFormation && this.RankCount > 1)
			{
				bool isMiddleFrontUnitPositionReserved = this.IsMiddleFrontUnitPositionReserved;
				IFormationUnit vanguard = this._vanguard;
				if (isMiddleFrontUnitPositionReserved)
				{
					this.ReleaseMiddleFrontUnitPosition();
				}
				int rankCount = this.RankCount;
				for (int i = 0; i < rankCount / 2; i++)
				{
					for (int j = 0; j < this.FileCount; j++)
					{
						IFormationUnit formationUnit = this._units2D[j, i];
						int num = rankCount - i - 1;
						int num2 = this.FileCount - j - 1;
						IFormationUnit formationUnit2 = this._units2D[num2, num];
						if (formationUnit2 == null)
						{
							this._units2D[num2, num] = formationUnit;
							this._units2D[j, i] = null;
							if (formationUnit != null)
							{
								formationUnit.FormationFileIndex = num2;
								formationUnit.FormationRankIndex = num;
							}
						}
						else if (formationUnit != null && formationUnit != formationUnit2)
						{
							this.SwitchUnitLocationsAux(formationUnit, formationUnit2);
						}
					}
				}
				for (int k = 0; k < this.FileCount; k++)
				{
					if (this._units2D[k, 0] == null && this._units2D[k, 1] != null)
					{
						for (int l = 1; l < rankCount; l++)
						{
							IFormationUnit formationUnit3 = this._units2D[k, l];
							IFormationUnit formationUnit4 = formationUnit3;
							int formationRankIndex = formationUnit4.FormationRankIndex;
							formationUnit4.FormationRankIndex = formationRankIndex - 1;
							this._units2D[k, l - 1] = formationUnit3;
							this._units2D[k, l] = null;
						}
					}
				}
				this.isExpandingFromRightSide = !this.isExpandingFromRightSide;
				this.ReconstructUnitsFromUnits2D();
				foreach (IFormationUnit formationUnit5 in this.GetAllUnits())
				{
					int num3;
					IFormationUnit unitToFollow = this.GetUnitToFollow(formationUnit5, out num3);
					this.SetUnitToFollow(formationUnit5, unitToFollow, num3);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged != null)
				{
					onShapeChanged();
				}
				if (isMiddleFrontUnitPositionReserved)
				{
					this.ReserveMiddleFrontUnitPosition(vanguard);
				}
				Action onShapeChanged2 = this.OnShapeChanged;
				if (onShapeChanged2 == null)
				{
					return;
				}
				onShapeChanged2();
			}
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0002BBF0 File Offset: 0x00029DF0
		public void OnFormationDispersed()
		{
			foreach (IFormationUnit formationUnit in this.GetAllUnits().ToArray())
			{
				this.SwitchUnitIfLeftBehind(formationUnit);
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0002BC22 File Offset: 0x00029E22
		public void Reset()
		{
			this._units2D.ResetWithNewCount(this.ColumnCount, 1);
			this.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000FE8 RID: 4072 RVA: 0x0002BC4C File Offset: 0x00029E4C
		// (remove) Token: 0x06000FE9 RID: 4073 RVA: 0x0002BC84 File Offset: 0x00029E84
		public event Action OnWidthChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000FEA RID: 4074 RVA: 0x0002BCBC File Offset: 0x00029EBC
		// (remove) Token: 0x06000FEB RID: 4075 RVA: 0x0002BCF4 File Offset: 0x00029EF4
		public event Action OnShapeChanged;

		// Token: 0x06000FEC RID: 4076 RVA: 0x0002BD2C File Offset: 0x00029F2C
		public virtual void RearrangeFrom(IFormationArrangement arrangement)
		{
			if (arrangement is TransposedLineFormation)
			{
				this.FlankWidth = arrangement.FlankWidth;
				return;
			}
			if (arrangement is LineFormation)
			{
				this.FlankWidth = (float)MathF.Max(0, MathF.Ceiling(MathF.Sqrt((float)(arrangement.UnitCount / ColumnFormation.ArrangementAspectRatio))) - 1) * (this.owner.UnitDiameter + this.Interval) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0002BD9B File Offset: 0x00029F9B
		public virtual void RearrangeTo(IFormationArrangement arrangement)
		{
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0002BDA0 File Offset: 0x00029FA0
		public virtual void RearrangeTransferUnits(IFormationArrangement arrangement)
		{
			foreach (ValueTuple<int, int> valueTuple in this.GetOrderedUnitPositionIndices().ToList<ValueTuple<int, int>>())
			{
				IFormationUnit formationUnit = this._units2D[valueTuple.Item1, valueTuple.Item2];
				if (formationUnit != null)
				{
					formationUnit.FormationFileIndex = -1;
					formationUnit.FormationRankIndex = -1;
					this.SetUnitToFollow(formationUnit, null, 0);
					arrangement.AddUnit(formationUnit);
				}
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0002BE2C File Offset: 0x0002A02C
		private void SetVanguard(IFormationUnit vanguard)
		{
			if (this.Vanguard != null || vanguard != null)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.UnitCount > 0)
				{
					if (this.Vanguard == null && vanguard != null)
					{
						flag2 = true;
					}
					else if (this.Vanguard != null && vanguard == null)
					{
						flag = true;
					}
				}
				ValueTuple<int, int> middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
				if (flag)
				{
					Agent agent = this.Vanguard as Agent;
					if (((agent != null) ? agent.Formation : null) == this.owner)
					{
						this.RemoveUnit(this.Vanguard);
						this.AddUnit(this.Vanguard);
					}
					else if (this.RankCount > 0)
					{
						this.ShiftUnitsForward(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
					}
				}
				else if (flag2)
				{
					Agent agent2 = vanguard as Agent;
					if (((agent2 != null) ? agent2.Formation : null) == this.owner)
					{
						this.RemoveUnit(vanguard);
						this.ShiftUnitsBackwardForMakingRoomForVanguard(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
						if (this.RankCount > 0)
						{
							this._units2D[middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2] = vanguard;
							this.ReconstructUnitsFromUnits2D();
							vanguard.FormationFileIndex = middleFrontUnitPosition.Item1;
							vanguard.FormationRankIndex = middleFrontUnitPosition.Item2;
							if (this.RankCount == 2)
							{
								this.AdjustFollowDataOfUnitPosition(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
								this.AdjustFollowDataOfUnitPosition(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2 + 1);
								Action onShapeChanged = this.OnShapeChanged;
								if (onShapeChanged != null)
								{
									onShapeChanged();
								}
							}
						}
						else
						{
							this.AddUnit(vanguard);
						}
					}
					else
					{
						this.ShiftUnitsBackwardForMakingRoomForVanguard(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
					}
				}
				this._vanguard = vanguard;
				if (this.RankCount > 0)
				{
					this.AdjustFollowDataOfUnitPosition(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0002BFD5 File Offset: 0x0002A1D5
		public int UnitCount
		{
			get
			{
				return this.GetAllUnits().Count;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0002BFE2 File Offset: 0x0002A1E2
		public int PositionedUnitCount
		{
			get
			{
				return this.UnitCount;
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002BFEC File Offset: 0x0002A1EC
		protected int GetUnitCountWithOverride()
		{
			int num;
			if (this.owner.OverridenUnitCount != null)
			{
				num = this.owner.OverridenUnitCount.Value;
			}
			else
			{
				num = this.UnitCount;
			}
			return num;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002C02C File Offset: 0x0002A22C
		private void SetColumnCount(int columnCount)
		{
			if (this.ColumnCount != columnCount)
			{
				IFormationUnit[] array = this.GetAllUnits().ToArray();
				this._units2D.ResetWithNewCount(columnCount, 1);
				this.ReconstructUnitsFromUnits2D();
				foreach (IFormationUnit formationUnit in array)
				{
					formationUnit.FormationFileIndex = -1;
					formationUnit.FormationRankIndex = -1;
					this.AddUnit(formationUnit);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged == null)
				{
					return;
				}
				onShapeChanged();
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002C099 File Offset: 0x0002A299
		public void FormFromWidth(float width)
		{
			this.ColumnCount = MathF.Ceiling(width);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0002C0A8 File Offset: 0x0002A2A8
		public IFormationUnit GetNeighborUnitOfLeftSide(IFormationUnit unit)
		{
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

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0002C0F0 File Offset: 0x0002A2F0
		public IFormationUnit GetNeighborUnitOfRightSide(IFormationUnit unit)
		{
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

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002C13A File Offset: 0x0002A33A
		public void ReserveMiddleFrontUnitPosition(IFormationUnit vanguard)
		{
			Agent agent = vanguard as Agent;
			if (((agent != null) ? agent.Formation : null) != this.owner)
			{
				this.IsMiddleFrontUnitPositionReserved = true;
			}
			else
			{
				this._isMiddleFrontUnitPositionUsedByVanguardInFormation = true;
			}
			this.Vanguard = vanguard;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0002C16D File Offset: 0x0002A36D
		public void ReleaseMiddleFrontUnitPosition()
		{
			this.IsMiddleFrontUnitPositionReserved = false;
			this.Vanguard = null;
			this._isMiddleFrontUnitPositionUsedByVanguardInFormation = false;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0002C184 File Offset: 0x0002A384
		private ValueTuple<int, int> GetMiddleFrontUnitPosition()
		{
			return new ValueTuple<int, int>(this.VanguardFileIndex, 0);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0002C192 File Offset: 0x0002A392
		public Vec2 GetLocalPositionOfReservedUnitPosition()
		{
			return Vec2.Zero;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0002C19C File Offset: 0x0002A39C
		public void OnTickOccasionallyOfUnit(IFormationUnit unit, bool arrangementChangeAllowed)
		{
			if (arrangementChangeAllowed && unit.FollowedUnit != this._vanguard && unit.FollowedUnit is Agent && !((Agent)unit.FollowedUnit).IsAIControlled && unit.FollowedUnit.FormationFileIndex >= 0 && unit.FollowedUnit.FormationRankIndex >= 0)
			{
				IFormationUnit followedUnit = unit.FollowedUnit;
				this.RemoveUnit(unit.FollowedUnit);
				this.AddUnit(followedUnit);
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0002C210 File Offset: 0x0002A410
		public void OnTickOccasionally()
		{
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0002C214 File Offset: 0x0002A414
		private MBList<IFormationUnit> GetUnitsBehind(IFormationUnit unit)
		{
			MBList<IFormationUnit> mblist = new MBList<IFormationUnit>();
			bool flag = false;
			for (int i = 0; i < this.ColumnCount; i++)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
				int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				if (num == unit.FormationFileIndex)
				{
					flag = true;
				}
				if (flag && this._units2D[num, unit.FormationRankIndex] != null)
				{
					mblist.Add(this._units2D[num, unit.FormationRankIndex]);
				}
			}
			for (int j = 0; j < this.FileCount; j++)
			{
				for (int k = unit.FormationRankIndex + 1; k < this.RankCount; k++)
				{
					if (this._units2D[j, k] != null)
					{
						mblist.Add(this._units2D[j, k]);
					}
				}
			}
			return mblist;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0002C2E8 File Offset: 0x0002A4E8
		private void SwitchUnitIfLeftBehind(IFormationUnit unit)
		{
			int num;
			IFormationUnit unitToFollow = this.GetUnitToFollow(unit, out num);
			if (unitToFollow == null)
			{
				float num2 = this.owner.UnitDiameter * 2f;
				IFormationUnit formationUnit = this.owner.GetClosestUnitTo(Vec2.Zero, new MBList<IFormationUnit> { unit }, new float?(num2));
				if (formationUnit == null)
				{
					formationUnit = this.owner.GetClosestUnitTo(Vec2.Zero, this.GetUnitsAtRanks(0, this.RankCount - 1), null);
				}
				if (formationUnit != null && formationUnit != unit && formationUnit is Agent && (formationUnit as Agent).IsAIControlled)
				{
					this.SwitchUnitLocations(unit, formationUnit);
					return;
				}
			}
			else
			{
				float num3 = this.GetFollowVector(num).Length * 1.5f;
				IFormationUnit formationUnit2 = this.owner.GetClosestUnitTo(unitToFollow, new MBList<IFormationUnit> { unit }, new float?(num3));
				if (formationUnit2 == null)
				{
					formationUnit2 = this.owner.GetClosestUnitTo(unitToFollow, this.GetUnitsBehind(unit), null);
				}
				Agent agent;
				if (formationUnit2 != null && formationUnit2 != unit && (agent = formationUnit2 as Agent) != null && agent.IsAIControlled)
				{
					this.SwitchUnitLocations(unit, agent);
				}
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0002C41C File Offset: 0x0002A61C
		private void SetUnitToFollow(IFormationUnit unit, IFormationUnit unitToFollow, int columnOffset = 0)
		{
			Vec2 followVector = this.GetFollowVector(columnOffset);
			this.owner.SetUnitToFollow(unit, unitToFollow, followVector);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0002C440 File Offset: 0x0002A640
		private Vec2 GetFollowVector(int columnOffset)
		{
			Vec2 vec;
			if (columnOffset == 0)
			{
				vec = -Vec2.Forward * (this.Distance + this.owner.UnitDiameter);
			}
			else
			{
				vec = Vec2.Side * (float)columnOffset * (this.owner.UnitDiameter + this.Interval);
			}
			return vec;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0002C499 File Offset: 0x0002A699
		public float GetDirectionChangeTendencyOfUnit(IFormationUnit unit)
		{
			if (this.RankCount == 1 || unit.FormationRankIndex == -1)
			{
				return 0f;
			}
			return (float)unit.FormationRankIndex * 1f / (float)(this.RankCount - 1);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0002C4CC File Offset: 0x0002A6CC
		private MBList<IFormationUnit> GetUnitsAtRanks(int rankIndex1, int rankIndex2)
		{
			MBList<IFormationUnit> mblist = new MBList<IFormationUnit>();
			for (int i = 0; i < this.ColumnCount; i++)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
				int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				if (this._units2D[num, rankIndex1] != null)
				{
					mblist.Add(this._units2D[num, rankIndex1]);
				}
			}
			for (int j = 0; j < this.ColumnCount; j++)
			{
				int columnOffsetFromColumnIndex2 = ColumnFormation.GetColumnOffsetFromColumnIndex(j, this.isExpandingFromRightSide);
				int num2 = this.VanguardFileIndex + columnOffsetFromColumnIndex2;
				if (this._units2D[num2, rankIndex2] != null)
				{
					mblist.Add(this._units2D[num2, rankIndex2]);
				}
			}
			return mblist;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0002C57C File Offset: 0x0002A77C
		public IEnumerable<T> GetUnitsAtVanguardFile<T>() where T : IFormationUnit
		{
			int fileIndex = this.VanguardFileIndex;
			int num;
			for (int rankIndex = 0; rankIndex < this.RankCount; rankIndex = num + 1)
			{
				if (rankIndex == 0 && this.Vanguard != null)
				{
					yield return (T)((object)this.Vanguard);
				}
				if (this._units2D[fileIndex, rankIndex] != null)
				{
					yield return (T)((object)this._units2D[fileIndex, rankIndex]);
				}
				num = rankIndex;
			}
			yield break;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0002C58C File Offset: 0x0002A78C
		public void UpdateLocalPositionErrors(bool recalculateErrors)
		{
		}

		// Token: 0x1700039B RID: 923
		// (set) Token: 0x06001005 RID: 4101 RVA: 0x0002C58E File Offset: 0x0002A78E
		bool IFormationArrangement.AreLocalPositionsDirty
		{
			set
			{
			}
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0002C590 File Offset: 0x0002A790
		public List<Vec2> GetUnitPositionsOnVanguardFileIndex()
		{
			IEnumerable<Agent> unitsAtVanguardFile = this.GetUnitsAtVanguardFile<Agent>();
			List<Vec2> list = new List<Vec2>(unitsAtVanguardFile.Count<Agent>());
			foreach (Agent agent in unitsAtVanguardFile)
			{
				list.Add(agent.Position.AsVec2);
			}
			return list;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0002C600 File Offset: 0x0002A800
		void IFormationArrangement.GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn)
		{
			this.GetAllUnits(in allUnitsListToBeFilledIn);
		}

		// Token: 0x040003D2 RID: 978
		public static readonly int ArrangementAspectRatio = 5;

		// Token: 0x040003D3 RID: 979
		private readonly IFormation owner;

		// Token: 0x040003D4 RID: 980
		private IFormationUnit _vanguard;

		// Token: 0x040003D5 RID: 981
		private MBList2D<IFormationUnit> _units2D;

		// Token: 0x040003D6 RID: 982
		private MBList2D<IFormationUnit> _units2DWorkspace;

		// Token: 0x040003D7 RID: 983
		private MBList<IFormationUnit> _allUnits;

		// Token: 0x040003D8 RID: 984
		private bool isExpandingFromRightSide = true;

		// Token: 0x040003D9 RID: 985
		private bool IsMiddleFrontUnitPositionReserved;

		// Token: 0x040003DA RID: 986
		private bool _isMiddleFrontUnitPositionUsedByVanguardInFormation;
	}
}
