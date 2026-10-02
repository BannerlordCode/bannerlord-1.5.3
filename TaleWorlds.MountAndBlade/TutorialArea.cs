using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000373 RID: 883
	public class TutorialArea : MissionObject
	{
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x000D047C File Offset: 0x000CE67C
		public MBReadOnlyList<TrainingIcon> TrainingIconsReadOnly
		{
			get
			{
				return this._trainingIcons;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x000D0484 File Offset: 0x000CE684
		// (set) Token: 0x060032E1 RID: 13025 RVA: 0x000D048C File Offset: 0x000CE68C
		public TutorialArea.TrainingType TypeOfTraining
		{
			get
			{
				return this._typeOfTraining;
			}
			private set
			{
				this._typeOfTraining = value;
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000D0495 File Offset: 0x000CE695
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.GatherWeapons();
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000D04A4 File Offset: 0x000CE6A4
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				uint num = 4294901760U;
				using (List<TutorialArea.TutorialEntity>.Enumerator enumerator = this._tagWeapon.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TutorialArea.TutorialEntity tutorialEntity = enumerator.Current;
						foreach (Tuple<GameEntity, MatrixFrame> tuple in tutorialEntity.EntityList)
						{
							tuple.Item1.SetContourColor(new uint?(num), true);
							this._highlightedEntities.Add(tuple.Item1);
						}
					}
					return;
				}
			}
			foreach (GameEntity gameEntity in this._highlightedEntities)
			{
				gameEntity.SetContourColor(null, true);
			}
			this._highlightedEntities.Clear();
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000D05C0 File Offset: 0x000CE7C0
		protected internal override void OnInit()
		{
			base.OnInit();
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			foreach (GameEntity gameEntity in list)
			{
				string[] tags = gameEntity.Tags;
				for (int i = 0; i < tags.Length; i++)
				{
					if (tags[i].StartsWith(this._tagPrefix) && gameEntity.HasScriptOfType<WeaponSpawner>())
					{
						gameEntity.GetFirstScriptOfType<WeaponSpawner>().SpawnWeapon();
						break;
					}
				}
			}
			this.GatherWeapons();
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000D0670 File Offset: 0x000CE870
		public override void AfterMissionStart()
		{
			this.DeactivateAllWeapons(true);
			this.MarkTrainingIcons(false);
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000D0680 File Offset: 0x000CE880
		private void GatherWeapons()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			foreach (GameEntity gameEntity in list)
			{
				foreach (string text in gameEntity.Tags)
				{
					TrainingIcon firstScriptOfType = gameEntity.GetFirstScriptOfType<TrainingIcon>();
					if (firstScriptOfType != null)
					{
						if (firstScriptOfType.GetTrainingSubTypeTag().StartsWith(this._tagPrefix))
						{
							this._trainingIcons.Add(firstScriptOfType);
						}
					}
					else if (text == this._tagPrefix + "boundary")
					{
						this.AddBoundary(gameEntity);
					}
					else if (text.StartsWith(this._tagPrefix))
					{
						this.AddTaggedWeapon(gameEntity, text);
					}
				}
			}
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000D0774 File Offset: 0x000CE974
		public void MarkTrainingIcons(bool mark)
		{
			foreach (TrainingIcon trainingIcon in this._trainingIcons)
			{
				trainingIcon.SetMarked(mark);
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x000D07C8 File Offset: 0x000CE9C8
		public TrainingIcon GetActiveTrainingIcon()
		{
			foreach (TrainingIcon trainingIcon in this._trainingIcons)
			{
				if (trainingIcon.GetIsActivated())
				{
					return trainingIcon;
				}
			}
			return null;
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x000D0824 File Offset: 0x000CEA24
		private void AddBoundary(GameEntity boundary)
		{
			this._boundaries.Add(boundary);
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x000D0834 File Offset: 0x000CEA34
		private void AddTaggedWeapon(GameEntity weapon, string tag)
		{
			if (weapon.HasScriptOfType<VolumeBox>())
			{
				this._volumeBoxes.Add(weapon.GetFirstScriptOfType<VolumeBox>());
				return;
			}
			bool flag = false;
			foreach (TutorialArea.TutorialEntity tutorialEntity in this._tagWeapon)
			{
				if (tutorialEntity.Tag == tag)
				{
					tutorialEntity.EntityList.Add(Tuple.Create<GameEntity, MatrixFrame>(weapon, weapon.GetGlobalFrame()));
					if (weapon.HasScriptOfType<DestructableComponent>())
					{
						tutorialEntity.DestructableComponents.Add(weapon.GetFirstScriptOfType<DestructableComponent>());
					}
					else if (weapon.HasScriptOfType<SpawnedItemEntity>())
					{
						tutorialEntity.WeaponList.Add(weapon);
						tutorialEntity.WeaponNames.Add(MBObjectManager.Instance.GetObject<ItemObject>(weapon.GetFirstScriptOfType<SpawnedItemEntity>().WeaponCopy.Item.StringId));
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._tagWeapon.Add(new TutorialArea.TutorialEntity(tag, new List<Tuple<GameEntity, MatrixFrame>> { Tuple.Create<GameEntity, MatrixFrame>(weapon, weapon.GetGlobalFrame()) }, new List<DestructableComponent>(), new List<GameEntity>(), new List<ItemObject>()));
				if (weapon.HasScriptOfType<DestructableComponent>())
				{
					this._tagWeapon[this._tagWeapon.Count - 1].DestructableComponents.Add(weapon.GetFirstScriptOfType<DestructableComponent>());
					return;
				}
				if (weapon.HasScriptOfType<SpawnedItemEntity>())
				{
					this._tagWeapon[this._tagWeapon.Count - 1].WeaponList.Add(weapon);
					this._tagWeapon[this._tagWeapon.Count - 1].WeaponNames.Add(MBObjectManager.Instance.GetObject<ItemObject>(weapon.GetFirstScriptOfType<SpawnedItemEntity>().WeaponCopy.Item.StringId));
				}
			}
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000D0A04 File Offset: 0x000CEC04
		public int GetIndexFromTag(string tag)
		{
			for (int i = 0; i < this._tagWeapon.Count; i++)
			{
				if (this._tagWeapon[i].Tag == tag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x000D0A44 File Offset: 0x000CEC44
		public List<string> GetSubTrainingTags()
		{
			List<string> list = new List<string>();
			foreach (TutorialArea.TutorialEntity tutorialEntity in this._tagWeapon)
			{
				list.Add(tutorialEntity.Tag);
			}
			return list;
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x000D0AA4 File Offset: 0x000CECA4
		public void ActivateTaggedWeapons(int index)
		{
			if (index >= this._tagWeapon.Count)
			{
				return;
			}
			this.DeactivateAllWeapons(false);
			foreach (Tuple<GameEntity, MatrixFrame> tuple in this._tagWeapon[index].EntityList)
			{
				tuple.Item1.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x000D0B1C File Offset: 0x000CED1C
		public void EquipWeaponsToPlayer(int index)
		{
			foreach (GameEntity gameEntity in this._tagWeapon[index].WeaponList)
			{
				bool flag;
				Agent.Main.OnItemPickup(gameEntity.GetFirstScriptOfType<SpawnedItemEntity>(), EquipmentIndex.None, out flag);
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x000D0B88 File Offset: 0x000CED88
		public void DeactivateAllWeapons(bool resetDestructibles)
		{
			foreach (TutorialArea.TutorialEntity tutorialEntity in this._tagWeapon)
			{
				if (resetDestructibles)
				{
					foreach (DestructableComponent destructableComponent in tutorialEntity.DestructableComponents)
					{
						destructableComponent.Reset();
						destructableComponent.HitPoint = 1000000f;
						Markable firstScriptOfType = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
						if (firstScriptOfType != null)
						{
							firstScriptOfType.DisableMarkerActivation();
						}
					}
				}
				foreach (Tuple<GameEntity, MatrixFrame> tuple in tutorialEntity.EntityList)
				{
					if (!tuple.Item1.HasScriptOfType<DestructableComponent>())
					{
						if (tuple.Item1.HasScriptOfType<SpawnedItemEntity>())
						{
							tuple.Item1.GetFirstScriptOfType<SpawnedItemEntity>().StopPhysicsAndSetFrameForClient(tuple.Item2, null);
							tuple.Item1.GetFirstScriptOfType<SpawnedItemEntity>().HasLifeTime = false;
						}
						GameEntity item = tuple.Item1;
						MatrixFrame item2 = tuple.Item2;
						item.SetGlobalFrame(in item2, true);
					}
					tuple.Item1.SetVisibilityExcludeParents(false);
				}
			}
			this.HideBoundaries();
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x000D0D14 File Offset: 0x000CEF14
		public void ActivateBoundaries()
		{
			if (this._boundariesHidden)
			{
				foreach (GameEntity gameEntity in this._boundaries)
				{
					gameEntity.SetVisibilityExcludeParents(true);
				}
				this._boundariesHidden = false;
			}
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x000D0D74 File Offset: 0x000CEF74
		public void HideBoundaries()
		{
			if (!this._boundariesHidden)
			{
				foreach (GameEntity gameEntity in this._boundaries)
				{
					gameEntity.SetVisibilityExcludeParents(false);
				}
				this._boundariesHidden = true;
			}
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x000D0DD4 File Offset: 0x000CEFD4
		public int GetBreakablesCount(int index)
		{
			return this._tagWeapon[index].DestructableComponents.Count;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x000D0DEC File Offset: 0x000CEFEC
		public void MakeDestructible(int index)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				this._tagWeapon[index].DestructableComponents[i].HitPoint = this._tagWeapon[index].DestructableComponents[i].MaxHitPoint;
			}
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x000D0E54 File Offset: 0x000CF054
		public void MarkAllTargets(int index, bool mark)
		{
			foreach (DestructableComponent destructableComponent in this._tagWeapon[index].DestructableComponents)
			{
				if (mark)
				{
					Markable firstScriptOfType = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
					if (firstScriptOfType != null)
					{
						firstScriptOfType.ActivateMarkerFor(3f, 10f);
					}
				}
				else
				{
					Markable firstScriptOfType2 = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
					if (firstScriptOfType2 != null)
					{
						firstScriptOfType2.DisableMarkerActivation();
					}
				}
			}
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000D0EEC File Offset: 0x000CF0EC
		public void ResetMarkingTargetTimers(int index)
		{
			foreach (DestructableComponent destructableComponent in this._tagWeapon[index].DestructableComponents)
			{
				Markable firstScriptOfType = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.ResetPassiveDurationTimer();
				}
			}
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000D0F5C File Offset: 0x000CF15C
		public void MakeInDestructible(int index)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				this._tagWeapon[index].DestructableComponents[i].HitPoint = 1000000f;
			}
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x000D0FAC File Offset: 0x000CF1AC
		public bool AllBreakablesAreBroken(int index)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (!this._tagWeapon[index].DestructableComponents[i].IsDestroyed)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x000D0FFC File Offset: 0x000CF1FC
		public int GetBrokenBreakableCount(int index)
		{
			int num = 0;
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (this._tagWeapon[index].DestructableComponents[i].IsDestroyed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000D1050 File Offset: 0x000CF250
		public int GetUnbrokenBreakableCount(int index)
		{
			int num = 0;
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (!this._tagWeapon[index].DestructableComponents[i].IsDestroyed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000D10A4 File Offset: 0x000CF2A4
		public void ResetBreakables(int index, bool makeIndestructible = true)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (makeIndestructible)
				{
					this._tagWeapon[index].DestructableComponents[i].HitPoint = 1000000f;
				}
				this._tagWeapon[index].DestructableComponents[i].Reset();
			}
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000D1114 File Offset: 0x000CF314
		public bool HasMainAgentPickedAll(int index)
		{
			using (List<GameEntity>.Enumerator enumerator = this._tagWeapon[index].WeaponList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasScriptOfType<SpawnedItemEntity>())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x000D1178 File Offset: 0x000CF378
		public void CheckMainAgentEquipment(int index)
		{
			this._allowedWeaponsHelper.Clear();
			this._allowedWeaponsHelper.AddRange(this._tagWeapon[index].WeaponNames);
			EquipmentIndex i;
			EquipmentIndex j;
			for (i = EquipmentIndex.WeaponItemBeginSlot; i <= EquipmentIndex.Weapon3; i = j + 1)
			{
				if (!Mission.Current.MainAgent.Equipment[i].IsEmpty)
				{
					if (this._allowedWeaponsHelper.Exists((ItemObject x) => x == Mission.Current.MainAgent.Equipment[i].Item))
					{
						this._allowedWeaponsHelper.Remove(Mission.Current.MainAgent.Equipment[i].Item);
					}
					else
					{
						Mission.Current.MainAgent.DropItem(i, WeaponClass.Undefined);
						MBInformationManager.AddQuickInformation(new TextObject("{=3PP01vFv}Keep away from that weapon.", null), 0, null, null, "");
					}
				}
				j = i;
			}
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x000D1278 File Offset: 0x000CF478
		public void CheckWeapons(int index)
		{
			foreach (GameEntity gameEntity in this._tagWeapon[index].WeaponList)
			{
				if (gameEntity.HasScriptOfType<SpawnedItemEntity>())
				{
					gameEntity.GetFirstScriptOfType<SpawnedItemEntity>().HasLifeTime = false;
				}
			}
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x000D12E4 File Offset: 0x000CF4E4
		public bool IsPositionInsideTutorialArea(Vec3 position, out string[] volumeBoxTags)
		{
			foreach (VolumeBox volumeBox in this._volumeBoxes)
			{
				if (volumeBox.IsPointIn(position))
				{
					volumeBoxTags = volumeBox.GameEntity.Tags;
					return true;
				}
			}
			volumeBoxTags = null;
			return false;
		}

		// Token: 0x040015AA RID: 5546
		[EditableScriptComponentVariable(true, "")]
		private TutorialArea.TrainingType _typeOfTraining;

		// Token: 0x040015AB RID: 5547
		[EditableScriptComponentVariable(true, "")]
		private string _tagPrefix = "A_";

		// Token: 0x040015AC RID: 5548
		private readonly List<TutorialArea.TutorialEntity> _tagWeapon = new List<TutorialArea.TutorialEntity>();

		// Token: 0x040015AD RID: 5549
		private readonly List<VolumeBox> _volumeBoxes = new List<VolumeBox>();

		// Token: 0x040015AE RID: 5550
		private readonly List<GameEntity> _boundaries = new List<GameEntity>();

		// Token: 0x040015AF RID: 5551
		private bool _boundariesHidden;

		// Token: 0x040015B0 RID: 5552
		private readonly List<GameEntity> _highlightedEntities = new List<GameEntity>();

		// Token: 0x040015B1 RID: 5553
		private readonly List<ItemObject> _allowedWeaponsHelper = new List<ItemObject>();

		// Token: 0x040015B2 RID: 5554
		private readonly MBList<TrainingIcon> _trainingIcons = new MBList<TrainingIcon>();

		// Token: 0x02000653 RID: 1619
		public enum TrainingType
		{
			// Token: 0x040021BC RID: 8636
			Bow,
			// Token: 0x040021BD RID: 8637
			Melee,
			// Token: 0x040021BE RID: 8638
			Mounted,
			// Token: 0x040021BF RID: 8639
			AdvancedMelee
		}

		// Token: 0x02000654 RID: 1620
		private struct TutorialEntity
		{
			// Token: 0x0600411B RID: 16667 RVA: 0x000FCBD1 File Offset: 0x000FADD1
			public TutorialEntity(string tag, List<Tuple<GameEntity, MatrixFrame>> entityList, List<DestructableComponent> destructableComponents, List<GameEntity> weapon, List<ItemObject> weaponNames)
			{
				this.Tag = tag;
				this.EntityList = entityList;
				this.DestructableComponents = destructableComponents;
				this.WeaponList = weapon;
				this.WeaponNames = weaponNames;
			}

			// Token: 0x040021C0 RID: 8640
			public string Tag;

			// Token: 0x040021C1 RID: 8641
			public List<Tuple<GameEntity, MatrixFrame>> EntityList;

			// Token: 0x040021C2 RID: 8642
			public List<DestructableComponent> DestructableComponents;

			// Token: 0x040021C3 RID: 8643
			public List<GameEntity> WeaponList;

			// Token: 0x040021C4 RID: 8644
			public List<ItemObject> WeaponNames;
		}
	}
}
