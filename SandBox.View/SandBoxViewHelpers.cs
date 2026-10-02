using System;
using Helpers;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace SandBox.View
{
	// Token: 0x0200000C RID: 12
	public class SandBoxViewHelpers
	{
		// Token: 0x02000087 RID: 135
		public static class BannerVisualHelper
		{
			// Token: 0x060005BF RID: 1471 RVA: 0x0002A37C File Offset: 0x0002857C
			public static MetaMesh GetBanner(Banner banner, string bannerMeshName)
			{
				MetaMesh copy = MetaMesh.GetCopy(bannerMeshName, true, false);
				for (int i = 0; i < copy.MeshCount; i++)
				{
					Mesh meshAtIndex = copy.GetMeshAtIndex(i);
					if (!meshAtIndex.HasTag("dont_use_tableau"))
					{
						Material material = meshAtIndex.GetMaterial();
						Material tableauMaterial = null;
						Tuple<Material, Banner> tuple = new Tuple<Material, Banner>(material, banner);
						if (MapScreen.Instance.CharacterBannerMaterialCache.ContainsKey(tuple))
						{
							tableauMaterial = MapScreen.Instance.CharacterBannerMaterialCache[tuple];
						}
						else
						{
							tableauMaterial = material.CreateCopy();
							Action<Texture> action = delegate(Texture tex)
							{
								tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
								uint num = (uint)tableauMaterial.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
								ulong shaderFlags = tableauMaterial.GetShaderFlags();
								tableauMaterial.SetShaderFlags(shaderFlags | (ulong)num);
							};
							BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual("GetBanner");
							banner.GetTableauTextureLarge(in bannerDebugInfo, action);
							MapScreen.Instance.CharacterBannerMaterialCache[tuple] = tableauMaterial;
						}
						meshAtIndex.SetMaterial(tableauMaterial);
					}
				}
				return copy;
			}
		}

		// Token: 0x02000088 RID: 136
		public static class MobilePartyVisualHelper
		{
			// Token: 0x060005C0 RID: 1472 RVA: 0x0002A464 File Offset: 0x00028664
			public static void GetMeleeWeaponToWield(PartyBase party, out int wieldedItemIndex)
			{
				wieldedItemIndex = -1;
				CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(party);
				if (visualPartyLeader != null)
				{
					for (int i = 0; i < 5; i++)
					{
						if (visualPartyLeader.Equipment[i].Item != null && visualPartyLeader.Equipment[i].Item.PrimaryWeapon.IsMeleeWeapon)
						{
							wieldedItemIndex = i;
							return;
						}
					}
				}
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0002A4C4 File Offset: 0x000286C4
			public static AgentVisuals GetHumanAgentPartyVisual(Scene mapScene, MatrixFrame frame, PartyBase party, uint contourColor, ActionIndexCache leaderAction, ref bool clearBannerEntityCache, ref ValueTuple<string, GameEntity> cachedBannerEntity, out float animationDuration)
			{
				IFaction mapFaction = party.MapFaction;
				uint num = ((mapFaction != null) ? mapFaction.Color : 4291609515U);
				IFaction mapFaction2 = party.MapFaction;
				uint num2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 4291609515U);
				CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(party);
				string text = null;
				Hero leaderHero = party.LeaderHero;
				if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
				{
					text = party.LeaderHero.ClanBanner.BannerCode;
				}
				Equipment equipment = visualPartyLeader.Equipment.Clone(false);
				bool flag = !string.IsNullOrEmpty(text) && (((visualPartyLeader.IsPlayerCharacter || visualPartyLeader.HeroObject.Clan == Clan.PlayerClan) && Clan.PlayerClan.Tier >= Campaign.Current.Models.ClanTierModel.BannerEligibleTier) || (!visualPartyLeader.IsPlayerCharacter && (!visualPartyLeader.IsHero || (visualPartyLeader.IsHero && visualPartyLeader.HeroObject.Clan != Clan.PlayerClan))));
				int num3;
				SandBoxViewHelpers.MobilePartyVisualHelper.GetMeleeWeaponToWield(party, out num3);
				int num4 = 4;
				if (flag)
				{
					ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>("campaign_banner_small");
					equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(@object, null, null, false);
				}
				Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(visualPartyLeader.Race);
				MBActionSet actionSetWithSuffix = MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, visualPartyLeader.IsFemale, flag ? "_map_with_banner" : "_map");
				AgentVisualsData agentVisualsData = new AgentVisualsData().UseMorphAnims(true).Equipment(equipment).BodyProperties(visualPartyLeader.GetBodyProperties(visualPartyLeader.Equipment, -1))
					.SkeletonType(visualPartyLeader.IsFemale ? SkeletonType.Female : SkeletonType.Male)
					.Scale(0.3f)
					.Frame(frame)
					.ActionSet(actionSetWithSuffix)
					.Scene(mapScene)
					.Monster(baseMonsterFromRace)
					.PrepareImmediately(false)
					.RightWieldedItemIndex(num3)
					.HasClippingPlane(true)
					.UseScaledWeapons(true)
					.ClothColor1(num)
					.ClothColor2(num2)
					.CharacterObjectStringId(visualPartyLeader.StringId)
					.AddColorRandomness(!visualPartyLeader.IsHero)
					.Race(visualPartyLeader.Race);
				if (flag)
				{
					Banner banner = new Banner(text);
					agentVisualsData.Banner(banner).LeftWieldedItemIndex(num4);
					if (cachedBannerEntity.Item1 == text + "campaign_banner_small")
					{
						agentVisualsData.CachedWeaponEntity(EquipmentIndex.ExtraWeaponSlot, cachedBannerEntity.Item2);
					}
				}
				animationDuration = ((leaderAction != ActionIndexCache.act_none) ? MBActionSet.GetActionAnimationDuration(actionSetWithSuffix, in leaderAction) : 1f);
				AgentVisuals agentVisuals = AgentVisuals.Create(agentVisualsData, "PartyIcon " + visualPartyLeader.Name, false, false, false);
				if (agentVisuals != null)
				{
					if (flag)
					{
						GameEntity entity = agentVisuals.GetEntity();
						GameEntity child = entity.GetChild(entity.ChildCount - 1);
						if (child.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > 0)
						{
							clearBannerEntityCache = false;
							cachedBannerEntity = new ValueTuple<string, GameEntity>(text + "campaign_banner_small", child);
						}
					}
					agentVisuals.GetWeakEntity().SetContourColor(new uint?(contourColor), false);
				}
				return agentVisuals;
			}

			// Token: 0x040002B4 RID: 692
			private const float PartyScale = 0.3f;
		}
	}
}
