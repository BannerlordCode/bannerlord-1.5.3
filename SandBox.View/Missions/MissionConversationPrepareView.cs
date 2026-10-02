using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace SandBox.View.Missions
{
	// Token: 0x02000019 RID: 25
	public class MissionConversationPrepareView : MissionView
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00005D0C File Offset: 0x00003F0C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._conversationMissionLogic = base.Mission.GetMissionBehavior<ConversationMissionLogic>();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005D28 File Offset: 0x00003F28
		public override void AfterStart()
		{
			base.AfterStart();
			if (this._conversationMissionLogic != null)
			{
				GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("banner_with_faction_color");
				if (gameEntity != null)
				{
					if (this._conversationMissionLogic.OtherSideConversationData.Character.IsHero)
					{
						PartyBase party = this._conversationMissionLogic.OtherSideConversationData.Party;
						Banner banner;
						if ((banner = ((party != null) ? party.Banner : null)) == null)
						{
							PartyBase party2 = this._conversationMissionLogic.PlayerConversationData.Party;
							banner = ((party2 != null) ? party2.Banner : null);
						}
						Banner banner2 = banner;
						if (banner2 != null)
						{
							this.SetOwnerBanner(gameEntity, banner2);
							return;
						}
					}
					else
					{
						gameEntity.Remove(112);
					}
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005DD0 File Offset: 0x00003FD0
		private void SetOwnerBanner(GameEntity bannerEntity, Banner ownerBanner)
		{
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			ownerBanner.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture tex)
			{
				this.OnTextureRendered(tex, bannerEntity);
			});
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005E18 File Offset: 0x00004018
		private void OnTextureRendered(Texture tex, GameEntity bannerEntity)
		{
			List<Mesh> list = bannerEntity.GetAllMeshesWithTag("banner_with_faction_color").ToList<Mesh>();
			if (list.IsEmpty<Mesh>())
			{
				list.Add(bannerEntity.GetFirstMesh());
			}
			foreach (Mesh mesh in list)
			{
				Material material = mesh.GetMaterial().CreateCopy();
				material.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
				uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
				ulong shaderFlags = material.GetShaderFlags();
				material.SetShaderFlags(shaderFlags | (ulong)num);
				mesh.SetMaterial(material);
			}
		}

		// Token: 0x04000032 RID: 50
		public const string BannerTagId = "banner_with_faction_color";

		// Token: 0x04000033 RID: 51
		private ConversationMissionLogic _conversationMissionLogic;
	}
}
