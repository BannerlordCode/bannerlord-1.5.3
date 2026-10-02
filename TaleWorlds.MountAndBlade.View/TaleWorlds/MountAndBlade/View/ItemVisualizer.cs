using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000021 RID: 33
	public class ItemVisualizer : ScriptComponentBehavior
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x000075D5 File Offset: 0x000057D5
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (Game.Current == null)
			{
				this._editorGameManager = new EditorGameManager();
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000075EF File Offset: 0x000057EF
		protected override void OnEditorTick(float dt)
		{
			if (!this.isFinished && this._editorGameManager != null)
			{
				this.isFinished = !this._editorGameManager.DoLoadingForGameManager();
				if (this.isFinished)
				{
					this.SpawnItems();
				}
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007624 File Offset: 0x00005824
		private void SpawnItems()
		{
			Scene scene = base.GameEntity.Scene;
			IEnumerable<ItemObject> objectTypeList = Game.Current.ObjectManager.GetObjectTypeList<ItemObject>();
			Vec3 vec = new Vec3(100f, 100f, 0f, -1f);
			vec.z = 2f;
			float num = 0f;
			float num2 = 200f;
			foreach (ItemObject itemObject in objectTypeList)
			{
				if (itemObject.MultiMeshName.Length > 0)
				{
					MetaMesh copy = MetaMesh.GetCopy(itemObject.MultiMeshName, true, true);
					if (copy != null)
					{
						GameEntity gameEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(scene, true, true, true);
						gameEntity.EntityFlags |= EntityFlags.DontSaveToScene;
						gameEntity.AddMultiMesh(copy, true);
						gameEntity.Name = itemObject.Name.ToString();
						gameEntity.RecomputeBoundingBox();
						float boundingBoxRadius = gameEntity.GetBoundingBoxRadius();
						if (boundingBoxRadius > num)
						{
							num = boundingBoxRadius;
						}
						vec.x += boundingBoxRadius;
						if (vec.x > num2)
						{
							vec.x = 100f;
							vec.y += num * 3f;
							num = 0f;
						}
						gameEntity.SetLocalPosition(vec);
						vec.x += boundingBoxRadius;
						if (vec.x > num2)
						{
							vec.x = 100f;
							vec.y += num * 3f;
							num = 0f;
						}
					}
				}
			}
		}

		// Token: 0x04000044 RID: 68
		private MBGameManager _editorGameManager;

		// Token: 0x04000045 RID: 69
		private bool isFinished;
	}
}
