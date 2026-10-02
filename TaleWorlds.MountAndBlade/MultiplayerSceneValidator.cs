using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000340 RID: 832
	public class MultiplayerSceneValidator : ScriptComponentBehavior
	{
		// Token: 0x06002EE7 RID: 12007 RVA: 0x000B5CBE File Offset: 0x000B3EBE
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "SelectFaultyEntities")
			{
				this.SelectInvalidEntities();
			}
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000B5CDC File Offset: 0x000B3EDC
		protected internal override void OnSceneSave(string saveFolder)
		{
			base.OnSceneSave(saveFolder);
			foreach (GameEntity gameEntity in this.GetInvalidEntities())
			{
			}
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000B5D30 File Offset: 0x000B3F30
		private List<GameEntity> GetInvalidEntities()
		{
			List<GameEntity> list = new List<GameEntity>();
			List<GameEntity> list2 = new List<GameEntity>();
			base.Scene.GetEntities(ref list2);
			foreach (GameEntity gameEntity in list2)
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in gameEntity.GetScriptComponents())
				{
					if (scriptComponentBehavior != null && (scriptComponentBehavior.GetType().IsSubclassOf(typeof(MissionObject)) || (scriptComponentBehavior.GetType() == typeof(MissionObject) && scriptComponentBehavior.IsOnlyVisual())))
					{
						list.Add(gameEntity);
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000B5E18 File Offset: 0x000B4018
		private void SelectInvalidEntities()
		{
			base.GameEntity.DeselectEntityOnEditor();
			foreach (GameEntity gameEntity in this.GetInvalidEntities())
			{
				gameEntity.SelectEntityOnEditor();
			}
		}

		// Token: 0x040012A9 RID: 4777
		public SimpleButton SelectFaultyEntities;
	}
}
