using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace SandBox.View.Missions
{
	// Token: 0x02000023 RID: 35
	[DefaultView]
	public class MissionSettlementPrepareView : MissionView
	{
		// Token: 0x060000DD RID: 221 RVA: 0x0000A112 File Offset: 0x00008312
		public override void AfterStart()
		{
			base.AfterStart();
			this.SetOwnerBanner();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000A120 File Offset: 0x00008320
		private void SetOwnerBanner()
		{
			Campaign campaign = Campaign.Current;
			if (campaign != null && campaign.GameMode == CampaignGameMode.Campaign)
			{
				Clan clan;
				if (Settlement.CurrentSettlement == null)
				{
					PartyBase parleyedParty = Campaign.Current.GetCampaignBehavior<IParleyCampaignBehavior>().GetParleyedParty();
					if (parleyedParty == null)
					{
						clan = null;
					}
					else
					{
						Hero owner = parleyedParty.Owner;
						clan = ((owner != null) ? owner.Clan : null);
					}
				}
				else
				{
					clan = Settlement.CurrentSettlement.OwnerClan;
				}
				Clan clan2 = clan;
				if (((clan2 != null) ? clan2.Banner : null) != null && base.Mission.Scene != null)
				{
					foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag("bd_banner_b"))
					{
						Action<Texture> action = delegate(Texture tex)
						{
							Material material = Mesh.GetFromResource("bd_banner_b").GetMaterial();
							uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
							ulong shaderFlags = material.GetShaderFlags();
							material.SetShaderFlags(shaderFlags | (ulong)num);
							material.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
						};
						Banner banner = clan2.Banner;
						BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
						banner.GetTableauTextureLarge(in bannerDebugInfo, action);
					}
				}
			}
		}

		// Token: 0x0400007D RID: 125
		public const string BannerTagId = "bd_banner_b";
	}
}
