using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200022C RID: 556
	internal class LocatorGrid<T> where T : ILocatable<T>
	{
		// Token: 0x0600217D RID: 8573 RVA: 0x000949C5 File Offset: 0x00092BC5
		internal LocatorGrid(float gridNodeSize = 5f, int gridWidth = 32, int gridHeight = 32)
		{
			this._width = gridWidth;
			this._height = gridHeight;
			this._gridNodeSize = gridNodeSize;
			this._nodes = new T[this._width * this._height];
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000949FA File Offset: 0x00092BFA
		private int MapCoordinates(int x, int y)
		{
			x %= this._width;
			if (x < 0)
			{
				x += this._width;
			}
			y %= this._height;
			if (y < 0)
			{
				y += this._height;
			}
			return y * this._width + x;
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00094A38 File Offset: 0x00092C38
		internal bool CheckWhetherPositionsAreInSameNode(Vec2 pos1, ILocatable<T> locatable)
		{
			int num = this.Pos2NodeIndex(pos1);
			int locatorNodeIndex = locatable.LocatorNodeIndex;
			return num == locatorNodeIndex;
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00094A58 File Offset: 0x00092C58
		internal bool UpdateLocator(T locatable)
		{
			ILocatable<T> locatable2 = locatable;
			Vec2 getPosition2D = locatable2.GetPosition2D;
			int num = this.Pos2NodeIndex(getPosition2D);
			if (num != locatable2.LocatorNodeIndex)
			{
				if (locatable2.LocatorNodeIndex >= 0)
				{
					this.RemoveFromList(locatable2);
				}
				this.AddToList(num, locatable);
				locatable2.LocatorNodeIndex = num;
				return true;
			}
			return false;
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00094AA8 File Offset: 0x00092CA8
		private void RemoveFromList(ILocatable<T> locatable)
		{
			if (this._nodes[locatable.LocatorNodeIndex] == locatable)
			{
				this._nodes[locatable.LocatorNodeIndex] = locatable.NextLocatable;
				locatable.NextLocatable = default(T);
				return;
			}
			ILocatable<T> locatable2;
			if ((locatable2 = this._nodes[locatable.LocatorNodeIndex]) != null)
			{
				while (locatable2.NextLocatable != null)
				{
					if (locatable2.NextLocatable == locatable)
					{
						locatable2.NextLocatable = locatable.NextLocatable;
						locatable.NextLocatable = default(T);
						return;
					}
					locatable2 = locatable2.NextLocatable;
				}
				Debug.FailedAssert("cannot remove party from MapLocator: " + locatable.ToString(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\LocatorGrid.cs", "RemoveFromList", 134);
			}
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00094B78 File Offset: 0x00092D78
		private void AddToList(int nodeIndex, T locator)
		{
			T t = this._nodes[nodeIndex];
			this._nodes[nodeIndex] = locator;
			locator.NextLocatable = t;
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00094BAC File Offset: 0x00092DAC
		private T FindLocatableOnNextNode(ref LocatableSearchData<T> data)
		{
			T t = default(T);
			do
			{
				data.CurrentY++;
				if (data.CurrentY > data.MaxYInclusive)
				{
					data.CurrentY = data.MinY;
					data.CurrentX++;
				}
				if (data.CurrentX <= data.MaxXInclusive)
				{
					int num = this.MapCoordinates(data.CurrentX, data.CurrentY);
					t = this._nodes[num];
				}
			}
			while (t == null && data.CurrentX <= data.MaxXInclusive);
			return t;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00094C38 File Offset: 0x00092E38
		internal T FindNextLocatable(ref LocatableSearchData<T> data)
		{
			if (data.CurrentLocatable != null)
			{
				data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				while (data.CurrentLocatable != null)
				{
					if (data.CurrentLocatable.GetPosition2D.DistanceSquared(data.Position) < data.RadiusSquared)
					{
						break;
					}
					data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				}
			}
			while (data.CurrentLocatable == null && data.CurrentX <= data.MaxXInclusive)
			{
				data.CurrentLocatable = this.FindLocatableOnNextNode(ref data);
				while (data.CurrentLocatable != null && data.CurrentLocatable.GetPosition2D.DistanceSquared(data.Position) >= data.RadiusSquared)
				{
					data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				}
			}
			return (T)((object)data.CurrentLocatable);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00094D20 File Offset: 0x00092F20
		internal LocatableSearchData<T> StartFindingLocatablesAroundPosition(Vec2 position, float radius)
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.GetBoundaries(position, radius, out num, out num2, out num3, out num4);
			return new LocatableSearchData<T>(position, radius, num, num2, num3, num4);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x00094D48 File Offset: 0x00092F48
		internal void RemoveLocatable(T locatable)
		{
			ILocatable<T> locatable2 = locatable;
			if (locatable2.LocatorNodeIndex >= 0)
			{
				this.RemoveFromList(locatable2);
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x00094D6C File Offset: 0x00092F6C
		private void GetBoundaries(Vec2 position, float radius, out int minX, out int minY, out int maxX, out int maxY)
		{
			Vec2 vec = new Vec2(radius, radius);
			this.GetGridIndices(position - vec, out minX, out minY);
			this.GetGridIndices(position + vec, out maxX, out maxY);
			int num = Math.Min(maxX - minX, this._width - 1);
			int num2 = Math.Min(maxY - minY, this._height - 1);
			minX %= this._width;
			minY %= this._height;
			maxX = minX + num;
			maxY = minY + num2;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x00094DF3 File Offset: 0x00092FF3
		private void GetGridIndices(Vec2 position, out int x, out int y)
		{
			x = MathF.Floor(position.x / this._gridNodeSize);
			y = MathF.Floor(position.y / this._gridNodeSize);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x00094E20 File Offset: 0x00093020
		private int Pos2NodeIndex(Vec2 position)
		{
			int num;
			int num2;
			this.GetGridIndices(position, out num, out num2);
			return this.MapCoordinates(num, num2);
		}

		// Token: 0x040009C6 RID: 2502
		private const float DefaultGridNodeSize = 5f;

		// Token: 0x040009C7 RID: 2503
		private const int DefaultGridWidth = 32;

		// Token: 0x040009C8 RID: 2504
		private const int DefaultGridHeight = 32;

		// Token: 0x040009C9 RID: 2505
		private readonly T[] _nodes;

		// Token: 0x040009CA RID: 2506
		private readonly float _gridNodeSize;

		// Token: 0x040009CB RID: 2507
		private readonly int _width;

		// Token: 0x040009CC RID: 2508
		private readonly int _height;
	}
}
