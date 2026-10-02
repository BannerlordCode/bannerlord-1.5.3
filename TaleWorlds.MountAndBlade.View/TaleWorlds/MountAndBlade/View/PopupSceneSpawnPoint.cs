using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Scripts;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000020 RID: 32
	public class PopupSceneSpawnPoint : ScriptComponentBehavior
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00006DAB File Offset: 0x00004FAB
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00006DB3 File Offset: 0x00004FB3
		public CompositeComponent AddedPrefabComponent { get; private set; }

		// Token: 0x060000DF RID: 223 RVA: 0x00006DBC File Offset: 0x00004FBC
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public void InitializeWithAgentVisuals(AgentVisuals humanVisuals, AgentVisuals mountVisuals = null)
		{
			this._humanAgentVisuals = humanVisuals;
			this._mountAgentVisuals = mountVisuals;
			this._initialStateActionCode = ActionIndexCache.Create(this.InitialAction);
			this._positiveStateActionCode = ((this.PositiveAction == "") ? this._initialStateActionCode : ActionIndexCache.Create(this.PositiveAction));
			this._negativeStateActionCode = ((this.NegativeAction == "") ? this._initialStateActionCode : ActionIndexCache.Create(this.NegativeAction));
			bool flag = !string.IsNullOrEmpty(this.RightHandWieldedItem);
			bool flag2 = !string.IsNullOrEmpty(this.LeftHandWieldedItem);
			if (flag2 || flag)
			{
				AgentVisualsData copyAgentVisualsData = this._humanAgentVisuals.GetCopyAgentVisualsData();
				Equipment equipment = this._humanAgentVisuals.GetEquipment().Clone(false);
				if (flag)
				{
					equipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>(this.RightHandWieldedItem), null, null, false);
				}
				if (flag2)
				{
					equipment[EquipmentIndex.Weapon1] = new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>(this.LeftHandWieldedItem), null, null, false);
				}
				int num = (flag ? 0 : (-1));
				int num2 = (flag2 ? 1 : (-1));
				copyAgentVisualsData.RightWieldedItemIndex(num).LeftWieldedItemIndex(num2).Equipment(equipment);
				this._humanAgentVisuals.Refresh(false, copyAgentVisualsData, false);
			}
			else
			{
				AgentVisualsData copyAgentVisualsData2 = this._humanAgentVisuals.GetCopyAgentVisualsData();
				Equipment equipment2 = this._humanAgentVisuals.GetEquipment().Clone(false);
				copyAgentVisualsData2.RightWieldedItemIndex(-1).LeftWieldedItemIndex(-1).Equipment(equipment2);
				this._humanAgentVisuals.Refresh(false, copyAgentVisualsData2, false);
			}
			if (this.PrefabItem != "")
			{
				if (!TaleWorlds.Engine.GameEntity.PrefabExists(this.PrefabItem))
				{
					MBDebug.ShowWarning(string.Concat(new string[]
					{
						"Cannot find prefab '",
						this.PrefabItem,
						"' for popup agent '",
						base.GameEntity.Name,
						"'"
					}));
				}
				else
				{
					this.AddedPrefabComponent = this._humanAgentVisuals.AddPrefabToAgentVisualBoneByBoneType(this.PrefabItem, this.PrefabBone);
					if (this.AddedPrefabComponent != null)
					{
						MatrixFrame frame = this.AddedPrefabComponent.Frame;
						MatrixFrame identity = MatrixFrame.Identity;
						identity.origin = this.AttachedPrefabOffset;
						this.AddedPrefabComponent.Frame = (in identity) * (in frame);
					}
				}
			}
			foreach (GameEntity gameEntity in base.GameEntity.Scene.FindEntitiesWithTag(base.GameEntity.Name))
			{
				PopupSceneSequence firstScriptOfType = gameEntity.GetFirstScriptOfType<PopupSceneSequence>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.InitializeWithAgentVisuals(humanVisuals);
				}
			}
			AgentVisuals humanAgentVisuals = this._humanAgentVisuals;
			if (humanAgentVisuals != null)
			{
				MBAgentVisuals visuals = humanAgentVisuals.GetVisuals();
				if (visuals != null)
				{
					visuals.CheckResources(true);
				}
			}
			AgentVisuals mountAgentVisuals = this._mountAgentVisuals;
			if (mountAgentVisuals != null)
			{
				MBAgentVisuals visuals2 = mountAgentVisuals.GetVisuals();
				if (visuals2 != null)
				{
					visuals2.CheckResources(true);
				}
			}
			AgentVisuals mountAgentVisuals2 = this._mountAgentVisuals;
			if (mountAgentVisuals2 != null)
			{
				mountAgentVisuals2.Tick(null, 0.0001f, false, 0f);
			}
			AgentVisuals mountAgentVisuals3 = this._mountAgentVisuals;
			if (mountAgentVisuals3 != null)
			{
				GameEntity entity = mountAgentVisuals3.GetEntity();
				if (entity != null)
				{
					Skeleton skeleton = entity.Skeleton;
					if (skeleton != null)
					{
						skeleton.ForceUpdateBoneFrames();
					}
				}
			}
			AgentVisuals humanAgentVisuals2 = this._humanAgentVisuals;
			if (humanAgentVisuals2 != null)
			{
				humanAgentVisuals2.Tick(this._mountAgentVisuals, 0.0001f, false, 0f);
			}
			AgentVisuals humanAgentVisuals3 = this._humanAgentVisuals;
			if (humanAgentVisuals3 == null)
			{
				return;
			}
			GameEntity entity2 = humanAgentVisuals3.GetEntity();
			if (entity2 == null)
			{
				return;
			}
			Skeleton skeleton2 = entity2.Skeleton;
			if (skeleton2 == null)
			{
				return;
			}
			skeleton2.ForceUpdateBoneFrames();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000715C File Offset: 0x0000535C
		public void SetInitialState()
		{
			if (this._initialStateActionCode != ActionIndexCache.act_none)
			{
				float num = (this.StartWithRandomProgress ? MBRandom.RandomFloatRanged(0.5f) : 0f);
				AgentVisuals humanAgentVisuals = this._humanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					humanAgentVisuals.SetAction(in this._initialStateActionCode, num, true);
				}
				AgentVisuals mountAgentVisuals = this._mountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					mountAgentVisuals.SetAction(in this._initialStateActionCode, num, true);
				}
			}
			if (!string.IsNullOrEmpty(this.InitialFaceAnimCode))
			{
				this._humanAgentVisuals.GetVisuals().GetSkeleton().SetFacialAnimation(Agent.FacialAnimChannel.Mid, this.InitialFaceAnimCode, false, true);
			}
			foreach (GameEntity gameEntity in base.GameEntity.Scene.FindEntitiesWithTag(base.GameEntity.Name))
			{
				PopupSceneSequence firstScriptOfType = gameEntity.GetFirstScriptOfType<PopupSceneSequence>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.SetInitialState();
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007258 File Offset: 0x00005458
		public void SetPositiveState()
		{
			if (this._positiveStateActionCode != ActionIndexCache.act_none)
			{
				AgentVisuals humanAgentVisuals = this._humanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					humanAgentVisuals.SetAction(in this._positiveStateActionCode, 0f, true);
				}
				AgentVisuals mountAgentVisuals = this._mountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					mountAgentVisuals.SetAction(in this._positiveStateActionCode, 0f, true);
				}
			}
			if (!string.IsNullOrEmpty(this.PositiveFaceAnimCode))
			{
				this._humanAgentVisuals.GetVisuals().GetSkeleton().SetFacialAnimation(Agent.FacialAnimChannel.Mid, this.PositiveFaceAnimCode, false, true);
			}
			foreach (GameEntity gameEntity in base.GameEntity.Scene.FindEntitiesWithTag(base.GameEntity.Name))
			{
				PopupSceneSequence firstScriptOfType = gameEntity.GetFirstScriptOfType<PopupSceneSequence>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.SetPositiveState();
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007340 File Offset: 0x00005540
		public void SetNegativeState()
		{
			if (this._negativeStateActionCode != ActionIndexCache.act_none)
			{
				AgentVisuals humanAgentVisuals = this._humanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					humanAgentVisuals.SetAction(in this._negativeStateActionCode, 0f, true);
				}
				AgentVisuals mountAgentVisuals = this._mountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					mountAgentVisuals.SetAction(in this._negativeStateActionCode, 0f, true);
				}
			}
			if (!string.IsNullOrEmpty(this.NegativeFaceAnimCode))
			{
				this._humanAgentVisuals.GetVisuals().GetSkeleton().SetFacialAnimation(Agent.FacialAnimChannel.Mid, this.NegativeFaceAnimCode, false, true);
			}
			foreach (GameEntity gameEntity in base.GameEntity.Scene.FindEntitiesWithTag(base.GameEntity.Name))
			{
				PopupSceneSequence firstScriptOfType = gameEntity.GetFirstScriptOfType<PopupSceneSequence>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.SetNegativeState();
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007428 File Offset: 0x00005628
		public void Destroy()
		{
			AgentVisuals humanAgentVisuals = this._humanAgentVisuals;
			if (humanAgentVisuals != null)
			{
				humanAgentVisuals.Reset();
			}
			this._humanAgentVisuals = null;
			AgentVisuals mountAgentVisuals = this._mountAgentVisuals;
			if (mountAgentVisuals != null)
			{
				mountAgentVisuals.Reset();
			}
			this._mountAgentVisuals = null;
			this._initialStateActionCode = ActionIndexCache.act_none;
			this._positiveStateActionCode = ActionIndexCache.act_none;
			this._negativeStateActionCode = ActionIndexCache.act_none;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007486 File Offset: 0x00005686
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007490 File Offset: 0x00005690
		protected override void OnTick(float dt)
		{
			AgentVisuals mountAgentVisuals = this._mountAgentVisuals;
			if (mountAgentVisuals != null)
			{
				mountAgentVisuals.Tick(null, dt, false, 0f);
			}
			AgentVisuals mountAgentVisuals2 = this._mountAgentVisuals;
			if (mountAgentVisuals2 != null)
			{
				GameEntity entity = mountAgentVisuals2.GetEntity();
				if (entity != null)
				{
					Skeleton skeleton = entity.Skeleton;
					if (skeleton != null)
					{
						skeleton.ForceUpdateBoneFrames();
					}
				}
			}
			AgentVisuals humanAgentVisuals = this._humanAgentVisuals;
			if (humanAgentVisuals != null)
			{
				humanAgentVisuals.Tick(this._mountAgentVisuals, dt, false, 0f);
			}
			AgentVisuals humanAgentVisuals2 = this._humanAgentVisuals;
			if (humanAgentVisuals2 == null)
			{
				return;
			}
			GameEntity entity2 = humanAgentVisuals2.GetEntity();
			if (entity2 == null)
			{
				return;
			}
			Skeleton skeleton2 = entity2.Skeleton;
			if (skeleton2 == null)
			{
				return;
			}
			skeleton2.ForceUpdateBoneFrames();
		}

		// Token: 0x04000032 RID: 50
		public string InitialAction = "";

		// Token: 0x04000033 RID: 51
		public string NegativeAction = "";

		// Token: 0x04000034 RID: 52
		public string InitialFaceAnimCode = "";

		// Token: 0x04000035 RID: 53
		public string PositiveFaceAnimCode = "";

		// Token: 0x04000036 RID: 54
		public string NegativeFaceAnimCode = "";

		// Token: 0x04000037 RID: 55
		public string PositiveAction = "";

		// Token: 0x04000038 RID: 56
		public string LeftHandWieldedItem = "";

		// Token: 0x04000039 RID: 57
		public string RightHandWieldedItem = "";

		// Token: 0x0400003A RID: 58
		public string BannerTagToUseForAddedPrefab = "";

		// Token: 0x0400003B RID: 59
		public bool StartWithRandomProgress;

		// Token: 0x0400003C RID: 60
		public Vec3 AttachedPrefabOffset = Vec3.Zero;

		// Token: 0x0400003D RID: 61
		public string PrefabItem = "";

		// Token: 0x0400003E RID: 62
		public HumanBone PrefabBone = HumanBone.ItemR;

		// Token: 0x0400003F RID: 63
		private AgentVisuals _mountAgentVisuals;

		// Token: 0x04000040 RID: 64
		private AgentVisuals _humanAgentVisuals;

		// Token: 0x04000041 RID: 65
		private ActionIndexCache _initialStateActionCode = ActionIndexCache.act_none;

		// Token: 0x04000042 RID: 66
		private ActionIndexCache _positiveStateActionCode = ActionIndexCache.act_none;

		// Token: 0x04000043 RID: 67
		private ActionIndexCache _negativeStateActionCode = ActionIndexCache.act_none;
	}
}
