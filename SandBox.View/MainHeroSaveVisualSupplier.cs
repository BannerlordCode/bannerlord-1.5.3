using System;
using System.Collections.Generic;
using System.Globalization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.View
{
	// Token: 0x02000007 RID: 7
	public class MainHeroSaveVisualSupplier : IMainHeroVisualSupplier
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002260 File Offset: 0x00000460
		string IMainHeroVisualSupplier.GetMainHeroVisualCode()
		{
			Hero mainHero = Hero.MainHero;
			CharacterObject characterObject = mainHero.CharacterObject;
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(characterObject.Race);
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(1024, "GetMainHeroVisualCode");
			mbstringBuilder.Append<string>("4|");
			mbstringBuilder.Append<string>(MBActionSet.GetActionSet(baseMonsterFromRace.ActionSetCode).GetSkeletonName());
			mbstringBuilder.Append<string>("|");
			Equipment battleEquipment = mainHero.BattleEquipment;
			mbstringBuilder.Append<string>(battleEquipment.GetSkinMeshesMask().ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(mainHero.IsFemale.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(mainHero.CharacterObject.Race.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(battleEquipment.GetUnderwearType(mainHero.IsFemale).ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(battleEquipment.BodyMeshType.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(battleEquipment.HairCoverType.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(battleEquipment.BeardCoverType.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(battleEquipment.BodyDeformType.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(characterObject.FaceDirtAmount.ToString(CultureInfo.InvariantCulture));
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>(mainHero.BodyProperties.ToString());
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>((mainHero.MapFaction != null) ? mainHero.MapFaction.Color.ToString() : "0xFFFFFFFF");
			mbstringBuilder.Append<string>("|");
			mbstringBuilder.Append<string>((mainHero.MapFaction != null) ? mainHero.MapFaction.Color2.ToString() : "0xFFFFFFFF");
			mbstringBuilder.Append<string>("|");
			for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
			{
				ItemObject item = battleEquipment[equipmentIndex].Item;
				string text = ((item != null) ? item.MultiMeshName : "");
				bool flag = item != null && item.IsUsingTeamColor;
				bool flag2 = item != null && item.IsUsingTableau;
				bool flag3 = item != null && item.HasArmorComponent && item.ArmorComponent.MultiMeshHasGenderVariations;
				mbstringBuilder.Append<string>(text + "|");
				mbstringBuilder.Append<string>(flag.ToString() + "|");
				mbstringBuilder.Append<string>(flag3.ToString() + "|");
				mbstringBuilder.Append<string>(flag2.ToString() + "|");
			}
			if (!mainHero.BattleEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty && ActionIndexCache.act_inventory_idle.Index != -1)
			{
				ItemObject item2 = mainHero.BattleEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
				ItemObject item3 = mainHero.BattleEquipment[EquipmentIndex.HorseHarness].Item;
				HorseComponent horseComponent = item2.HorseComponent;
				MBActionSet actionSet = MBActionSet.GetActionSet(item2.HorseComponent.Monster.ActionSetCode);
				mbstringBuilder.Append<string>(actionSet.GetSkeletonName());
				mbstringBuilder.Append<string>("|");
				mbstringBuilder.Append<string>(item2.MultiMeshName);
				mbstringBuilder.Append<string>("|");
				MountCreationKey randomMountKey = MountCreationKey.GetRandomMountKey(item2, characterObject.GetMountKeySeed());
				mbstringBuilder.Append<MountCreationKey>(randomMountKey);
				mbstringBuilder.Append<string>("|");
				if (horseComponent.HorseMaterialNames.Count > 0)
				{
					int num = MathF.Min((int)randomMountKey.MaterialIndex, horseComponent.HorseMaterialNames.Count - 1);
					HorseComponent.MaterialProperty materialProperty = horseComponent.HorseMaterialNames[num];
					mbstringBuilder.Append<string>(materialProperty.Name);
					mbstringBuilder.Append<string>("|");
					uint num2 = uint.MaxValue;
					int num3 = MathF.Min((int)randomMountKey.MeshMultiplierIndex, materialProperty.MeshMultiplier.Count - 1);
					if (num3 != -1)
					{
						num2 = materialProperty.MeshMultiplier[num3].Item1;
					}
					mbstringBuilder.Append(num2);
				}
				else
				{
					mbstringBuilder.Append<string>("|");
				}
				mbstringBuilder.Append<string>("|");
				mbstringBuilder.Append<string>(actionSet.GetAnimationName(in ActionIndexCache.act_inventory_idle));
				mbstringBuilder.Append<string>("|");
				if (item3 != null)
				{
					mbstringBuilder.Append<string>(item3.MultiMeshName);
					mbstringBuilder.Append<string>("|");
					mbstringBuilder.Append<bool>(item3.IsUsingTeamColor);
					mbstringBuilder.Append<string>("|");
					mbstringBuilder.Append<string>(item3.ArmorComponent.ReinsMesh);
					mbstringBuilder.Append<string>("|");
				}
				else
				{
					mbstringBuilder.Append<string>("|||");
				}
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, bool> keyValuePair in horseComponent.AdditionalMeshesNameList)
				{
					if (keyValuePair.Key.Length > 0)
					{
						string text2 = keyValuePair.Key;
						if (item3 == null || !keyValuePair.Value)
						{
							list.Add(text2);
						}
						else
						{
							ArmorComponent armorComponent = item3.ArmorComponent;
							if (armorComponent == null || armorComponent.ManeCoverType != ArmorComponent.HorseHarnessCoverTypes.All)
							{
								ArmorComponent armorComponent2 = item3.ArmorComponent;
								if (armorComponent2 != null && armorComponent2.ManeCoverType > ArmorComponent.HorseHarnessCoverTypes.None)
								{
									object obj = text2;
									object obj2 = "_";
									ArmorComponent.HorseHarnessCoverTypes? horseHarnessCoverTypes;
									if (item3 == null)
									{
										horseHarnessCoverTypes = null;
									}
									else
									{
										ArmorComponent armorComponent3 = item3.ArmorComponent;
										horseHarnessCoverTypes = ((armorComponent3 != null) ? new ArmorComponent.HorseHarnessCoverTypes?(armorComponent3.ManeCoverType) : null);
									}
									text2 = obj + obj2 + horseHarnessCoverTypes;
								}
								list.Add(text2);
							}
						}
					}
				}
				mbstringBuilder.Append(list.Count);
				using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string text3 = enumerator2.Current;
						mbstringBuilder.Append<string>("|");
						mbstringBuilder.Append<string>(text3);
					}
					goto IL_06D9;
				}
			}
			mbstringBuilder.Append<string>("|||||||||0");
			IL_06D9:
			return mbstringBuilder.ToStringAndRelease();
		}
	}
}
