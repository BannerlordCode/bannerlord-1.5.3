using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x020004A3 RID: 1187
	public class DeclareWarBarterable : Barterable
	{
		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06004C01 RID: 19457 RVA: 0x00181E5A File Offset: 0x0018005A
		public override string StringID
		{
			get
			{
				return "declare_war_barterable";
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x00181E61 File Offset: 0x00180061
		// (set) Token: 0x06004C03 RID: 19459 RVA: 0x00181E69 File Offset: 0x00180069
		public IFaction DeclaringFaction { get; private set; }

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x00181E72 File Offset: 0x00180072
		// (set) Token: 0x06004C05 RID: 19461 RVA: 0x00181E7A File Offset: 0x0018007A
		public IFaction OtherFaction { get; private set; }

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x00181E83 File Offset: 0x00180083
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=GZwNgIon}Declare war against {OTHER_FACTION}", null);
				textObject.SetTextVariable("OTHER_FACTION", this.OtherFaction.Name);
				return textObject;
			}
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x00181EA7 File Offset: 0x001800A7
		public DeclareWarBarterable(IFaction declaringFaction, IFaction otherFaction)
			: base(declaringFaction.Leader, null)
		{
			this.DeclaringFaction = declaringFaction;
			this.OtherFaction = otherFaction;
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x00181EC4 File Offset: 0x001800C4
		public override void Apply()
		{
			DeclareWarAction.ApplyByDefault(base.OriginalOwner.MapFaction, this.OtherFaction.MapFaction);
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x00181EE4 File Offset: 0x001800E4
		public override int GetUnitValueForFaction(IFaction faction)
		{
			int num = 0;
			Clan clan = ((faction is Clan) ? ((Clan)faction) : ((Kingdom)faction).RulingClan);
			if (faction.MapFaction == base.OriginalOwner.MapFaction)
			{
				TextObject textObject;
				num = (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(base.OriginalOwner.MapFaction, this.OtherFaction.MapFaction, clan, out textObject, false);
			}
			else if (faction.MapFaction == this.OtherFaction.MapFaction)
			{
				TextObject textObject;
				num = (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(this.OtherFaction.MapFaction, base.OriginalOwner.MapFaction, clan, out textObject, false);
			}
			return num;
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x00181F98 File Offset: 0x00180198
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}
	}
}
