using System;
using SandBox.View.Map.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000068 RID: 104
	public class TrackVisual : MapEntityVisual<Track>
	{
		// Token: 0x06000476 RID: 1142 RVA: 0x00024EF0 File Offset: 0x000230F0
		public TrackVisual(Track track)
			: base(track)
		{
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00024EF9 File Offset: 0x000230F9
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return base.MapEntity.Position;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00024F06 File Offset: 0x00023106
		public override MapEntityVisual AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00024F09 File Offset: 0x00023109
		public override Vec3 GetVisualPosition()
		{
			return base.MapEntity.Position.AsVec3();
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00024F1B File Offset: 0x0002311B
		public override bool IsVisibleOrFadingOut()
		{
			return base.MapEntity.IsDetected;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00024F28 File Offset: 0x00023128
		public override void OnHover()
		{
			InformationManager.ShowTooltip(typeof(Track), new object[] { base.MapEntity });
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00024F48 File Offset: 0x00023148
		public override bool OnMapClick(bool followModifierUsed)
		{
			return false;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00024F4B File Offset: 0x0002314B
		public override void OnOpenEncyclopedia()
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00024F4D File Offset: 0x0002314D
		public override void ReleaseResources()
		{
			MapTracksVisualManager.Current.ReleaseResources(base.MapEntity);
		}

		// Token: 0x04000235 RID: 565
		private static TextObject _defaultTrackTitle = new TextObject("{=maptrack}Track", null);
	}
}
