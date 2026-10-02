using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200001D RID: 29
	public class PreloadHelper
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00006318 File Offset: 0x00004518
		public void PreloadCharacters(List<BasicCharacterObject> characters)
		{
			Utilities.EnableGlobalEditDataCacher();
			foreach (BasicCharacterObject basicCharacterObject in characters)
			{
				foreach (Equipment equipment in basicCharacterObject.BattleEquipments)
				{
					this.AddEquipment(equipment);
				}
				if (Mission.Current != null && Mission.Current.DoesMissionRequireCivilianEquipment)
				{
					foreach (Equipment equipment2 in basicCharacterObject.CivilianEquipments)
					{
						this.AddEquipment(equipment2);
					}
				}
				if (Mission.Current != null)
				{
					List<EquipmentElement> extraEquipmentElementsForCharacter = Mission.Current.GetExtraEquipmentElementsForCharacter(basicCharacterObject, true);
					if (extraEquipmentElementsForCharacter != null)
					{
						int count = extraEquipmentElementsForCharacter.Count;
						for (int i = 0; i < count; i++)
						{
							ItemObject item = extraEquipmentElementsForCharacter[i].Item;
							this.AddItemObject(item);
						}
					}
				}
			}
			Utilities.DisableGlobalEditDataCacher();
			this.PreloadMeshesAndPhysics();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006454 File Offset: 0x00004654
		public void WaitForMeshesToBeLoaded()
		{
			int num;
			do
			{
				num = 0;
				foreach (ValueTuple<MetaMesh, bool, bool> valueTuple in this._uniqueMetaMeshes)
				{
					num += valueTuple.Item1.CheckResources();
				}
				foreach (string text in this._uniqueDynamicPhysicsShapeName)
				{
					num += ((PhysicsShape.GetFromResource(text, true) == null) ? 1 : 0);
				}
				Thread.Sleep(1);
			}
			while (num != 0);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006510 File Offset: 0x00004710
		public void PreloadEquipments(List<Equipment> equipments)
		{
			foreach (Equipment equipment in equipments)
			{
				this.AddEquipment(equipment);
			}
			this.PreloadMeshesAndPhysics();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006564 File Offset: 0x00004764
		public void PreloadItems(List<ItemObject> items)
		{
			Utilities.EnableGlobalEditDataCacher();
			for (int i = 0; i < items.Count; i++)
			{
				this.AddItemObject(items[i]);
			}
			Utilities.DisableGlobalEditDataCacher();
			this.PreloadMeshesAndPhysics();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000065A0 File Offset: 0x000047A0
		private void AddEquipment(Equipment equipment)
		{
			for (int i = 0; i < 12; i++)
			{
				ItemObject item = equipment.GetEquipmentFromSlot((EquipmentIndex)i).Item;
				if (item != null)
				{
					this.AddItemObject(item);
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000065D4 File Offset: 0x000047D4
		private void AddItemObject(ItemObject item)
		{
			if (item == null)
			{
				return;
			}
			bool isUsingTableau = item.IsUsingTableau;
			bool isUsingTeamColor = item.IsUsingTeamColor;
			this.RegisterMetaMeshUsageIfValid(item.MultiMeshName, isUsingTableau, isUsingTeamColor);
			this.RegisterMetaMeshUsageIfValid(item.HolsterMeshName, isUsingTableau, isUsingTeamColor);
			if (item.WeaponComponent != null)
			{
				if (item.IsCraftedWeapon)
				{
					this.RegisterMetaMeshUsageIfValid(item.GetHolsterWithWeaponMeshIfExists(), isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.GetHolsterMeshIfExists(), isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.GetFlyingMeshIfExists(), isUsingTableau, isUsingTeamColor);
				}
				else
				{
					this.RegisterMetaMeshUsageIfValid(item.HolsterWithWeaponMeshName, isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.HolsterMeshName, isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.FlyingMeshName, isUsingTableau, isUsingTeamColor);
				}
			}
			else
			{
				if (item.HasHorseComponent)
				{
					using (List<KeyValuePair<string, bool>>.Enumerator enumerator = item.HorseComponent.AdditionalMeshesNameList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, bool> keyValuePair = enumerator.Current;
							this.RegisterMetaMeshUsageIfValid(keyValuePair.Key, isUsingTableau, isUsingTeamColor);
						}
						goto IL_0127;
					}
				}
				if (item.HasArmorComponent && !string.IsNullOrEmpty(item.ArmorComponent.ReinsMesh))
				{
					this.RegisterMetaMeshUsageIfValid(item.ArmorComponent.ReinsMesh, isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.ArmorComponent.ReinsRopeMesh, isUsingTableau, isUsingTeamColor);
				}
			}
			IL_0127:
			if (!this._loadedItems.Contains(item))
			{
				this.RegisterMetaMeshUsageIfValid(item.GetMultiMesh(false, false, true), isUsingTableau, isUsingTeamColor);
				if (item.HasArmorComponent)
				{
					this.RegisterMetaMeshUsageIfValid(item.GetMultiMesh(false, true, true), isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.GetMultiMesh(true, false, true), isUsingTableau, isUsingTeamColor);
					this.RegisterMetaMeshUsageIfValid(item.GetMultiMesh(true, true, true), isUsingTableau, isUsingTeamColor);
				}
				this._loadedItems.Add(item);
			}
			this.RegisterPhysicsBodyUsageIfValid(this._uniqueDynamicPhysicsShapeName, item.CollisionBodyName);
			this.RegisterPhysicsBodyUsageIfValid(this._uniqueDynamicPhysicsShapeName, item.BodyName);
			this.RegisterPhysicsBodyUsageIfValid(this._uniqueDynamicPhysicsShapeName, item.HolsterBodyName);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000067B8 File Offset: 0x000049B8
		public void PreloadEntities(List<WeakGameEntity> entities)
		{
			for (int i = 0; i < entities.Count; i++)
			{
				WeakGameEntity weakGameEntity = entities[i];
				for (int j = 0; j < weakGameEntity.MultiMeshComponentCount; j++)
				{
					MetaMesh metaMesh = weakGameEntity.GetMetaMesh(j);
					this.RegisterMetaMeshUsageIfValid(metaMesh, false, false);
				}
				PhysicsShape bodyShape = weakGameEntity.GetBodyShape();
				if (bodyShape != null)
				{
					this.RegisterPhysicsBodyUsageIfValid(this._uniqueDynamicPhysicsShapeName, bodyShape.GetName());
				}
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006828 File Offset: 0x00004A28
		public void PreloadMeshesAndPhysics()
		{
			foreach (ValueTuple<MetaMesh, bool, bool> valueTuple in this._uniqueMetaMeshes)
			{
				MetaMesh item = valueTuple.Item1;
				item.PreloadForRendering();
				item.PreloadShaders(valueTuple.Item2, valueTuple.Item3);
			}
			foreach (string text in this._uniqueDynamicPhysicsShapeName)
			{
				PhysicsShape.AddPreloadQueueWithName(text, new Vec3(1f, 1f, 1f, -1f));
			}
			PhysicsShape.ProcessPreloadQueue();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000068F0 File Offset: 0x00004AF0
		public void Clear()
		{
			this._uniqueMetaMeshNames.Clear();
			this._uniqueDynamicPhysicsShapeName.Clear();
			this._uniqueMetaMeshes.Clear();
			PhysicsShape.UnloadDynamicBodies();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006918 File Offset: 0x00004B18
		private void RegisterMetaMeshUsageIfValid(string metaMeshName, bool useTableau, bool useTeamColor)
		{
			ValueTuple<string, bool, bool> valueTuple = new ValueTuple<string, bool, bool>(metaMeshName, useTableau, useTeamColor);
			if (!string.IsNullOrEmpty(metaMeshName) && !this._uniqueMetaMeshNames.Contains(valueTuple))
			{
				this.RegisterMetaMeshUsageIfValid(MetaMesh.GetCopy(metaMeshName, false, true), useTableau, useTeamColor);
				this._uniqueMetaMeshNames.Add(valueTuple);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006964 File Offset: 0x00004B64
		private void RegisterMetaMeshUsageIfValid(MetaMesh metaMesh, bool useTableau, bool useTeamColor)
		{
			if (metaMesh != null)
			{
				ValueTuple<MetaMesh, bool, bool> valueTuple = new ValueTuple<MetaMesh, bool, bool>(metaMesh, useTableau, useTeamColor);
				this._uniqueMetaMeshes.Add(valueTuple);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006991 File Offset: 0x00004B91
		private void RegisterPhysicsBodyUsageIfValid(HashSet<string> uniquePhysicsShapeName, string physicsShape)
		{
			if (!string.IsNullOrWhiteSpace(physicsShape))
			{
				uniquePhysicsShapeName.Add(physicsShape);
			}
		}

		// Token: 0x04000020 RID: 32
		private readonly HashSet<ValueTuple<string, bool, bool>> _uniqueMetaMeshNames = new HashSet<ValueTuple<string, bool, bool>>();

		// Token: 0x04000021 RID: 33
		private readonly HashSet<string> _uniqueDynamicPhysicsShapeName = new HashSet<string>();

		// Token: 0x04000022 RID: 34
		private readonly HashSet<ValueTuple<MetaMesh, bool, bool>> _uniqueMetaMeshes = new HashSet<ValueTuple<MetaMesh, bool, bool>>();

		// Token: 0x04000023 RID: 35
		private readonly HashSet<ItemObject> _loadedItems = new HashSet<ItemObject>();
	}
}
