using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033B RID: 827
	public class HideoutSpawnPointGroup : SynchedMissionObject
	{
		// Token: 0x06002EC8 RID: 11976 RVA: 0x000B5064 File Offset: 0x000B3264
		protected internal override void OnInit()
		{
			base.OnInit();
			this._spawnPoints = new GameEntity[4];
			string spawnPointTagAffix = this.Side.ToString().ToLower() + "_";
			string[] array = new string[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = spawnPointTagAffix + ((FormationClass)i).GetName().ToLower();
			}
			IEnumerable<WeakGameEntity> children = base.GameEntity.GetChildren();
			Func<WeakGameEntity, bool> <>9__0;
			Func<WeakGameEntity, bool> func;
			if ((func = <>9__0) == null)
			{
				Func<string, bool> <>9__1;
				func = (<>9__0 = delegate(WeakGameEntity ce)
				{
					IEnumerable<string> tags = ce.Tags;
					Func<string, bool> func2;
					if ((func2 = <>9__1) == null)
					{
						func2 = (<>9__1 = (string t) => t.StartsWith(spawnPointTagAffix));
					}
					return tags.Any<string>(func2);
				});
			}
			foreach (WeakGameEntity weakGameEntity in children.Where<WeakGameEntity>(func))
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (weakGameEntity.HasTag(array[j]))
					{
						this._spawnPoints[j] = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
						break;
					}
				}
			}
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000B5178 File Offset: 0x000B3378
		public MatrixFrame[] GetSpawnPointFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._spawnPoints.Length];
			for (int i = 0; i < this._spawnPoints.Length; i++)
			{
				array[i] = ((this._spawnPoints[i] != null) ? this._spawnPoints[i].GetGlobalFrame() : MatrixFrame.Identity);
			}
			return array;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000B51D4 File Offset: 0x000B33D4
		public void RemoveWithAllChildren()
		{
			base.GameEntity.RemoveAllChildren();
			base.GameEntity.Remove(83);
		}

		// Token: 0x04001288 RID: 4744
		private const int NumberOfDefaultFormations = 4;

		// Token: 0x04001289 RID: 4745
		public BattleSideEnum Side;

		// Token: 0x0400128A RID: 4746
		public int PhaseNumber;

		// Token: 0x0400128B RID: 4747
		private GameEntity[] _spawnPoints;
	}
}
