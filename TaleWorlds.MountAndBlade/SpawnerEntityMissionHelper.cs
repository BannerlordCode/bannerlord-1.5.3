using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000360 RID: 864
	public class SpawnerEntityMissionHelper
	{
		// Token: 0x060031D1 RID: 12753 RVA: 0x000CB474 File Offset: 0x000C9674
		public SpawnerEntityMissionHelper(SpawnerBase spawner, bool fireVersion = false)
		{
			this._spawner = spawner;
			this._fireVersion = fireVersion;
			this._ownerEntity = GameEntity.CreateFromWeakEntity(this._spawner.GameEntity);
			this._gameEntityName = this._ownerEntity.Name;
			if (this.SpawnPrefab(this._ownerEntity, this.GetPrefabName()) != null)
			{
				this.SyncMatrixFrames();
			}
			else
			{
				Debug.FailedAssert("Spawner couldn't spawn a proper entity.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SpawnerEntityMissionHelper.cs", ".ctor", 34);
			}
			this._spawner.AssignParameters(this);
			this.CallSetSpawnedFromSpawnerOfScripts();
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000CB508 File Offset: 0x000C9708
		private GameEntity SpawnPrefab(GameEntity parent, string entityName)
		{
			this.InstantiateEntity(parent, entityName);
			this.SpawnedEntity.SetMobility(GameEntity.Mobility.Dynamic);
			this.SpawnedEntity.EntityFlags = this.SpawnedEntity.EntityFlags | EntityFlags.DontSaveToScene;
			parent.AddChild(this.SpawnedEntity, false);
			MatrixFrame identity = MatrixFrame.Identity;
			this.SpawnedEntity.SetFrame(ref identity, true);
			foreach (string text in this._ownerEntity.Tags)
			{
				this.SpawnedEntity.AddTag(text);
			}
			return this.SpawnedEntity;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000CB596 File Offset: 0x000C9796
		protected virtual void InstantiateEntity(GameEntity parent, string entityName)
		{
			this.SpawnedEntity = GameEntity.Instantiate(parent.Scene, entityName, false, true, "");
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000CB5B1 File Offset: 0x000C97B1
		private void RemoveChildEntity(GameEntity child)
		{
			child.CallScriptCallbacks(false);
			child.Remove(85);
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000CB5C4 File Offset: 0x000C97C4
		private void SyncMatrixFrames()
		{
			List<GameEntity> list = new List<GameEntity>();
			this.SpawnedEntity.GetChildrenRecursive(ref list);
			foreach (GameEntity gameEntity in list)
			{
				if (SpawnerEntityMissionHelper.HasField(this._spawner, gameEntity.Name))
				{
					MatrixFrame matrixFrame = (MatrixFrame)SpawnerEntityMissionHelper.GetFieldValue(this._spawner, gameEntity.Name);
					gameEntity.SetFrame(ref matrixFrame, true);
				}
				if (SpawnerEntityMissionHelper.HasField(this._spawner, gameEntity.Name + "_enabled") && !(bool)SpawnerEntityMissionHelper.GetFieldValue(this._spawner, gameEntity.Name + "_enabled"))
				{
					this.RemoveChildEntity(gameEntity);
				}
			}
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000CB6A0 File Offset: 0x000C98A0
		private void CallSetSpawnedFromSpawnerOfScripts()
		{
			foreach (GameEntity gameEntity in this.SpawnedEntity.GetEntityAndChildren())
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in from x in gameEntity.GetScriptComponents()
					where x is ISpawnable
					select x)
				{
					(scriptComponentBehavior as ISpawnable).SetSpawnedFromSpawner();
				}
			}
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000CB74C File Offset: 0x000C994C
		private string GetPrefabName()
		{
			string text;
			if (this._spawner.ToBeSpawnedOverrideName != "")
			{
				text = this._spawner.ToBeSpawnedOverrideName;
			}
			else
			{
				text = this._gameEntityName;
				text = text.Remove(this._gameEntityName.Length - this._gameEntityName.Split(new char[] { '_' }).Last<string>().Length - 1);
			}
			if (this._fireVersion)
			{
				if (this._spawner.ToBeSpawnedOverrideNameForFireVersion != "")
				{
					text = this._spawner.ToBeSpawnedOverrideNameForFireVersion;
				}
				else
				{
					text += "_fire";
				}
			}
			return text;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x000CB7F4 File Offset: 0x000C99F4
		private static object GetFieldValue(object src, string propName)
		{
			return src.GetType().GetField(propName).GetValue(src);
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000CB808 File Offset: 0x000C9A08
		private static bool HasField(object obj, string propertyName)
		{
			return obj.GetType().GetField(propertyName) != null;
		}

		// Token: 0x04001500 RID: 5376
		private const string EnabledSuffix = "_enabled";

		// Token: 0x04001501 RID: 5377
		public GameEntity SpawnedEntity;

		// Token: 0x04001502 RID: 5378
		private GameEntity _ownerEntity;

		// Token: 0x04001503 RID: 5379
		private SpawnerBase _spawner;

		// Token: 0x04001504 RID: 5380
		private string _gameEntityName;

		// Token: 0x04001505 RID: 5381
		private bool _fireVersion;
	}
}
