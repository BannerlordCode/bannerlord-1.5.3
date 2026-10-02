using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000203 RID: 515
	public class DefaultFormationArrangementModel : FormationArrangementModel
	{
		// Token: 0x06001E21 RID: 7713 RVA: 0x00066A38 File Offset: 0x00064C38
		public override List<FormationArrangementModel.ArrangementPosition> GetBannerBearerPositions(Formation formation, int maxCount)
		{
			List<FormationArrangementModel.ArrangementPosition> list = new List<FormationArrangementModel.ArrangementPosition>();
			LineFormation lineFormation;
			if (formation == null || (lineFormation = formation.Arrangement as LineFormation) == null)
			{
				return list;
			}
			DefaultFormationArrangementModel.RelativeFormationPosition[] array = null;
			int num;
			int num2;
			lineFormation.GetFormationInfo(out num, out num2);
			LineFormation lineFormation2 = lineFormation;
			if (lineFormation2 != null)
			{
				if (lineFormation2 is CircularFormation)
				{
					array = DefaultFormationArrangementModel.BannerBearerCircularFormationPositions;
					goto IL_0089;
				}
				if (lineFormation2 is SkeinFormation)
				{
					array = DefaultFormationArrangementModel.BannerBearerSkeinFormationPositions;
					goto IL_0089;
				}
				if (lineFormation2 is SquareFormation)
				{
					array = DefaultFormationArrangementModel.BannerBearerSquareFormationPositions;
					goto IL_0089;
				}
				if (lineFormation2 is TransposedLineFormation)
				{
					goto IL_0089;
				}
				if (lineFormation2 is WedgeFormation)
				{
					goto IL_0089;
				}
			}
			array = DefaultFormationArrangementModel.BannerBearerLineFormationPositions;
			IL_0089:
			int num3 = 0;
			if (array != null)
			{
				foreach (DefaultFormationArrangementModel.RelativeFormationPosition relativeFormationPosition in array)
				{
					if (num3 >= maxCount)
					{
						break;
					}
					FormationArrangementModel.ArrangementPosition arrangementPosition = relativeFormationPosition.GetArrangementPosition(num, num2);
					if (DefaultFormationArrangementModel.SearchOccupiedInLineFormation(lineFormation, arrangementPosition.FileIndex, arrangementPosition.RankIndex, num, relativeFormationPosition.FromLeftFile, out arrangementPosition))
					{
						list.Add(arrangementPosition);
						num3++;
					}
				}
			}
			return list;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00066B38 File Offset: 0x00064D38
		private static bool SearchOccupiedInLineFormation(LineFormation lineFormation, int fileIndex, int rankIndex, int fileCount, bool searchLeftToRight, out FormationArrangementModel.ArrangementPosition foundPosition)
		{
			if (lineFormation.GetUnit(fileIndex, rankIndex) != null)
			{
				foundPosition = new FormationArrangementModel.ArrangementPosition(fileIndex, rankIndex);
				return true;
			}
			int num = MathF.Min(fileIndex + 1, fileCount - 1);
			int num2 = MathF.Max(fileIndex - 1, 0);
			foundPosition = FormationArrangementModel.ArrangementPosition.Invalid;
			if (searchLeftToRight)
			{
				if (DefaultFormationArrangementModel.SearchOccupiedFileLeftToRight(lineFormation, num, rankIndex, fileCount, ref foundPosition))
				{
					return true;
				}
				if (DefaultFormationArrangementModel.SearchOccupiedFileRightToLeft(lineFormation, num2, rankIndex, ref foundPosition))
				{
					return true;
				}
			}
			else
			{
				if (DefaultFormationArrangementModel.SearchOccupiedFileRightToLeft(lineFormation, num2, rankIndex, ref foundPosition))
				{
					return true;
				}
				if (DefaultFormationArrangementModel.SearchOccupiedFileLeftToRight(lineFormation, num, rankIndex, fileCount, ref foundPosition))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x00066BC0 File Offset: 0x00064DC0
		private static bool SearchOccupiedFileRightToLeft(LineFormation lineFormation, int fileIndex, int rankIndex, ref FormationArrangementModel.ArrangementPosition foundPosition)
		{
			for (int i = fileIndex; i >= 0; i--)
			{
				if (lineFormation.GetUnit(i, rankIndex) != null)
				{
					foundPosition = new FormationArrangementModel.ArrangementPosition(i, rankIndex);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x00066BF4 File Offset: 0x00064DF4
		private static bool SearchOccupiedFileLeftToRight(LineFormation lineFormation, int fileIndex, int rankIndex, int fileCount, ref FormationArrangementModel.ArrangementPosition foundPosition)
		{
			for (int i = fileIndex; i < fileCount; i++)
			{
				if (lineFormation.GetUnit(i, rankIndex) != null)
				{
					foundPosition = new FormationArrangementModel.ArrangementPosition(i, rankIndex);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A64 RID: 2660
		private static readonly DefaultFormationArrangementModel.RelativeFormationPosition[] BannerBearerLineFormationPositions = new DefaultFormationArrangementModel.RelativeFormationPosition[]
		{
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, true, 1, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(false, 0, false, 0, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(false, 0, true, 1, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, true, 1, 0.5f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0.5f, 0f)
		};

		// Token: 0x04000A65 RID: 2661
		private static readonly DefaultFormationArrangementModel.RelativeFormationPosition[] BannerBearerCircularFormationPositions = new DefaultFormationArrangementModel.RelativeFormationPosition[]
		{
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0.5f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 1, 0.833f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 1, 0.167f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 2, 0.666f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 2, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 2, 0.333f, 0f)
		};

		// Token: 0x04000A66 RID: 2662
		private static readonly DefaultFormationArrangementModel.RelativeFormationPosition[] BannerBearerSkeinFormationPositions = new DefaultFormationArrangementModel.RelativeFormationPosition[]
		{
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0.5f, 0.5f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(false, 0, false, 0, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0f, 0.5f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(false, 0, false, 0, 0f, 0.5f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, true, 1, 0.5f, 0f)
		};

		// Token: 0x04000A67 RID: 2663
		private static readonly DefaultFormationArrangementModel.RelativeFormationPosition[] BannerBearerSquareFormationPositions = new DefaultFormationArrangementModel.RelativeFormationPosition[]
		{
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 0, 0.5f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 1, 0.833f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 1, 0.167f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 2, 0.666f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 2, 0f, 0f),
			new DefaultFormationArrangementModel.RelativeFormationPosition(true, 0, false, 2, 0.333f, 0f)
		};

		// Token: 0x0200051D RID: 1309
		private struct RelativeFormationPosition
		{
			// Token: 0x06003CB0 RID: 15536 RVA: 0x000F3FE4 File Offset: 0x000F21E4
			public RelativeFormationPosition(bool fromLeftFile, int fileOffset, bool fromFrontRank, int rankOffset, float fileFractionalOffset = 0f, float rankFractionalOffset = 0f)
			{
				this.FromLeftFile = fromLeftFile;
				this.FileOffset = fileOffset;
				this.FileFractionalOffset = MathF.Clamp(fileFractionalOffset, 0f, 1f);
				this.FromFrontRank = fromFrontRank;
				this.RankOffset = rankOffset;
				this.RankFractionalOffset = MathF.Clamp(rankFractionalOffset, 0f, 1f);
			}

			// Token: 0x06003CB1 RID: 15537 RVA: 0x000F403C File Offset: 0x000F223C
			public FormationArrangementModel.ArrangementPosition GetArrangementPosition(int fileCount, int rankCount)
			{
				if (fileCount <= 0 || rankCount <= 0)
				{
					return FormationArrangementModel.ArrangementPosition.Invalid;
				}
				int num;
				int num2;
				if (this.FromLeftFile)
				{
					num = 1;
					num2 = 0;
				}
				else
				{
					num = -1;
					num2 = fileCount - 1;
				}
				int num3 = MathF.Round((float)this.FileOffset + this.FileFractionalOffset * (float)(fileCount - 1));
				int num4 = MBMath.ClampIndex(num2 + num * num3, 0, fileCount);
				int num5;
				int num6;
				if (this.FromFrontRank)
				{
					num5 = 1;
					num6 = 0;
				}
				else
				{
					num5 = -1;
					num6 = rankCount - 1;
				}
				int num7 = MathF.Round((float)this.RankOffset + this.RankFractionalOffset * (float)(rankCount - 1));
				int num8 = MBMath.ClampIndex(num6 + num5 * num7, 0, rankCount);
				return new FormationArrangementModel.ArrangementPosition(num4, num8);
			}

			// Token: 0x04001D7B RID: 7547
			public readonly bool FromLeftFile;

			// Token: 0x04001D7C RID: 7548
			public readonly int FileOffset;

			// Token: 0x04001D7D RID: 7549
			public readonly float FileFractionalOffset;

			// Token: 0x04001D7E RID: 7550
			public readonly bool FromFrontRank;

			// Token: 0x04001D7F RID: 7551
			public readonly int RankOffset;

			// Token: 0x04001D80 RID: 7552
			public readonly float RankFractionalOffset;
		}
	}
}
