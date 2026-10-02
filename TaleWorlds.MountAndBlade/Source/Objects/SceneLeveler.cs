using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Source.Objects
{
	// Token: 0x020003D6 RID: 982
	public class SceneLeveler : ScriptComponentBehavior
	{
		// Token: 0x06003706 RID: 14086 RVA: 0x000E3CF4 File Offset: 0x000E1EF4
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(variableName);
			if (num <= 90459893U)
			{
				if (num != 40127036U)
				{
					if (num != 73682274U)
					{
						if (num != 90459893U)
						{
							return;
						}
						if (!(variableName == "CreateLevel2"))
						{
							return;
						}
						this.OnLevelizeButtonPressed(2);
						return;
					}
					else
					{
						if (!(variableName == "CreateLevel3"))
						{
							return;
						}
						this.OnLevelizeButtonPressed(3);
						return;
					}
				}
				else
				{
					if (!(variableName == "CreateLevel1"))
					{
						return;
					}
					this.OnLevelizeButtonPressed(1);
					return;
				}
			}
			else if (num <= 1310461563U)
			{
				if (num != 804927328U)
				{
					if (num != 1310461563U)
					{
						return;
					}
					if (!(variableName == "DeleteLevel1"))
					{
						return;
					}
					this.OnDeleteButtonPressed(1);
					return;
				}
				else
				{
					if (!(variableName == "SelectEntitiesWithoutLevel"))
					{
						return;
					}
					this.OnSelectEntitiesWithoutLevelButtonPressed();
					return;
				}
			}
			else if (num != 1327239182U)
			{
				if (num != 1344016801U)
				{
					return;
				}
				if (!(variableName == "DeleteLevel3"))
				{
					return;
				}
				this.OnDeleteButtonPressed(3);
				return;
			}
			else
			{
				if (!(variableName == "DeleteLevel2"))
				{
					return;
				}
				this.OnDeleteButtonPressed(2);
				return;
			}
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x000E3DEC File Offset: 0x000E1FEC
		private void OnLevelizeButtonPressed(int level)
		{
			if (this.SourceSelectionSetName.IsEmpty<char>())
			{
				MessageManager.DisplayMessage("ApplyToSelectionSet is empty!");
				return;
			}
			if (this.TargetSelectionSetName.IsEmpty<char>())
			{
				MessageManager.DisplayMessage("NewSelectionSetName is empty!");
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			GameEntity.UpgradeLevelMask levelMask = this.GetLevelMask(level);
			List<GameEntity> list = this.CollectEntitiesWithLevel();
			List<GameEntity> list2 = new List<GameEntity>();
			foreach (GameEntity gameEntity in list)
			{
				string text = this.FindPossiblePrefabName(gameEntity);
				if (text.IsEmpty<char>())
				{
					num++;
				}
				else
				{
					GameEntity.UpgradeLevelMask upgradeLevelMask = gameEntity.GetUpgradeLevelMask();
					if ((upgradeLevelMask & levelMask) != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None)
					{
						num2++;
						list2.Add(gameEntity);
					}
					else
					{
						string text2 = this.ConvertPrefabName(text, levelMask);
						GameEntity gameEntity2 = TaleWorlds.Engine.GameEntity.Instantiate(base.Scene, text2, gameEntity.GetGlobalFrame(), true);
						if (gameEntity2 == null)
						{
							num3++;
						}
						else
						{
							num4++;
							GameEntity.UpgradeLevelMask upgradeLevelMask2 = upgradeLevelMask & ~TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1 & ~TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2 & ~TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3;
							upgradeLevelMask2 |= levelMask;
							gameEntity2.SetUpgradeLevelMask(upgradeLevelMask2);
							this.CopyScriptParameters(gameEntity2, gameEntity);
							list2.Add(gameEntity2);
						}
					}
				}
			}
			Debug.Print(string.Concat(new object[] { "Created Entities : ", num4, "\nAlready Visible In Desired Level : ", num2, "\nWithout Prefab For Level : ", num3, "\nWithout Prefab Info : ", num }), 0, Debug.DebugColor.Magenta, 17592186044416UL);
			Utilities.CreateSelectionInEditor(list2, this.TargetSelectionSetName);
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x000E3F98 File Offset: 0x000E2198
		private void CopyScriptParameters(GameEntity entity, GameEntity copyFromEntity)
		{
			if (copyFromEntity.HasScriptComponent("WallSegment") && !entity.HasScriptComponent("WallSegment"))
			{
				entity.CopyScriptComponentFromAnotherEntity(copyFromEntity, "WallSegment");
			}
			if (copyFromEntity.HasScriptComponent("mesh_bender") && !entity.HasScriptComponent("mesh_bender"))
			{
				entity.CopyScriptComponentFromAnotherEntity(copyFromEntity, "mesh_bender");
			}
			int num = 0;
			while (num < entity.ChildCount && num < copyFromEntity.ChildCount)
			{
				this.CopyScriptParameters(entity.GetChild(num), copyFromEntity.GetChild(num));
				num++;
			}
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x000E401F File Offset: 0x000E221F
		private GameEntity.UpgradeLevelMask GetLevelMask(int level)
		{
			if (level == 1)
			{
				return TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1;
			}
			if (level != 2)
			{
				return TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3;
			}
			return TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x000E402E File Offset: 0x000E222E
		private string GetLevelSubString(GameEntity.UpgradeLevelMask levelMask)
		{
			if (levelMask == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1)
			{
				return "_l1";
			}
			if (levelMask == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2)
			{
				return "_l2";
			}
			if (levelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3)
			{
				return "";
			}
			return "_l3";
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x000E4058 File Offset: 0x000E2258
		private string ConvertPrefabName(string prefabName, GameEntity.UpgradeLevelMask newLevelMask)
		{
			string text = prefabName;
			string levelSubString = this.GetLevelSubString(newLevelMask);
			if (newLevelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1)
			{
				text = text.Replace(this.GetLevelSubString(TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1), levelSubString);
			}
			if (newLevelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2)
			{
				text = text.Replace(this.GetLevelSubString(TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2), levelSubString);
			}
			if (newLevelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3)
			{
				text = text.Replace(this.GetLevelSubString(TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3), levelSubString);
			}
			if (text.Equals(prefabName))
			{
				return "";
			}
			return text;
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x000E40B8 File Offset: 0x000E22B8
		private string FindPossiblePrefabName(GameEntity gameEntity)
		{
			string prefabName = gameEntity.GetPrefabName();
			if (prefabName.IsEmpty<char>())
			{
				return gameEntity.GetOldPrefabName();
			}
			return prefabName;
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x000E40DC File Offset: 0x000E22DC
		private void OnDeleteButtonPressed(int level)
		{
			if (this.SourceSelectionSetName.IsEmpty<char>())
			{
				MessageManager.DisplayMessage("ApplyToSelectionSet is empty!");
				return;
			}
			List<GameEntity> list = this.CollectEntitiesWithLevel();
			GameEntity.UpgradeLevelMask levelMask = this.GetLevelMask(level);
			List<GameEntity> list2 = new List<GameEntity>();
			int num = 0;
			int num2 = 0;
			foreach (GameEntity gameEntity in list)
			{
				GameEntity.UpgradeLevelMask upgradeLevelMask = gameEntity.GetUpgradeLevelMask();
				if (upgradeLevelMask == levelMask)
				{
					list2.Add(gameEntity);
					num++;
				}
				else if ((upgradeLevelMask & levelMask) != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None)
				{
					gameEntity.SetUpgradeLevelMask(upgradeLevelMask & ~levelMask);
					num2++;
				}
			}
			Utilities.DeleteEntitiesInEditorScene(list2);
			TextObject textObject = new TextObject("{=!}Deleted entity count : {DELETED_ENTRY_COUNT}", null);
			TextObject textObject2 = new TextObject("{=!}Removed level mask count : {REMOVED_LEVEL_MASK}", null);
			textObject.SetTextVariable("DELETED_ENTRY_COUNT", num);
			textObject2.SetTextVariable("REMOVED_LEVEL_MASK", num2);
			MessageManager.DisplayMessage(textObject.ToString());
			MessageManager.DisplayMessage(textObject2.ToString());
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000E41D8 File Offset: 0x000E23D8
		private void OnSelectEntitiesWithoutLevelButtonPressed()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.Scene.GetEntities(ref list);
			List<GameEntity> list2 = list.FindAll((GameEntity x) => x.GetUpgradeLevelMask() == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None);
			TextObject textObject = new TextObject("{=!}Selected entity count : {SELECTED_ENTITIES}", null);
			textObject.SetTextVariable("SELECTED_ENTITIES", list2.Count);
			MessageManager.DisplayMessage(textObject.ToString());
			if (list2.Count > 0)
			{
				Utilities.SelectEntities(list2);
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x000E4254 File Offset: 0x000E2454
		private List<GameEntity> CollectEntitiesWithLevel()
		{
			List<GameEntity> list = new List<GameEntity>();
			Utilities.GetEntitiesOfSelectionSet(this.SourceSelectionSetName, ref list);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if ((list[i].GetUpgradeLevelMask() & (TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1 | TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2 | TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3)) == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None)
				{
					list.RemoveAt(i);
				}
			}
			return list;
		}

		// Token: 0x040017B0 RID: 6064
		public string SourceSelectionSetName = "";

		// Token: 0x040017B1 RID: 6065
		public string TargetSelectionSetName = "";

		// Token: 0x040017B2 RID: 6066
		public SimpleButton CreateLevel1;

		// Token: 0x040017B3 RID: 6067
		public SimpleButton CreateLevel2;

		// Token: 0x040017B4 RID: 6068
		public SimpleButton CreateLevel3;

		// Token: 0x040017B5 RID: 6069
		public SimpleButton DeleteLevel1;

		// Token: 0x040017B6 RID: 6070
		public SimpleButton DeleteLevel2;

		// Token: 0x040017B7 RID: 6071
		public SimpleButton DeleteLevel3;

		// Token: 0x040017B8 RID: 6072
		public SimpleButton SelectEntitiesWithoutLevel;
	}
}
