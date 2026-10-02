using System;
using System.Collections.Generic;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000124 RID: 292
	public readonly struct ClanCardSelectionItemInfo
	{
		// Token: 0x06001A6C RID: 6764 RVA: 0x00064028 File Offset: 0x00062228
		public ClanCardSelectionItemInfo(object identifier, TextObject title, ImageIdentifier image, CardSelectionItemSpriteType spriteType, string spriteName, string spriteLabel, IEnumerable<ClanCardSelectionItemPropertyInfo> properties, bool isDisabled, TextObject disabledReason, TextObject actionResult, bool isInitiallySelected = false)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.Image = image;
			this.SpriteType = spriteType;
			this.SpriteName = spriteName;
			this.SpriteLabel = spriteLabel;
			this.Properties = properties;
			this.IsSpecialActionItem = false;
			this.SpecialActionText = null;
			this.IsDisabled = isDisabled;
			this.DisabledReason = disabledReason;
			this.ActionResult = actionResult;
			this.IsInitiallySelected = isInitiallySelected;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000640BC File Offset: 0x000622BC
		public ClanCardSelectionItemInfo(TextObject specialActionText, bool isDisabled, TextObject disabledReason, TextObject actionResult, bool isInitiallySelected = false)
		{
			this.Identifier = null;
			this.Title = null;
			this.Image = null;
			this.SpriteType = CardSelectionItemSpriteType.None;
			this.SpriteName = null;
			this.SpriteLabel = null;
			this.Properties = null;
			this.IsSpecialActionItem = true;
			this.SpecialActionText = specialActionText;
			this.IsDisabled = isDisabled;
			this.DisabledReason = disabledReason;
			this.ActionResult = actionResult;
			this.IsInitiallySelected = isInitiallySelected;
		}

		// Token: 0x04000C1F RID: 3103
		public readonly object Identifier;

		// Token: 0x04000C20 RID: 3104
		public readonly TextObject Title;

		// Token: 0x04000C21 RID: 3105
		public readonly ImageIdentifier Image;

		// Token: 0x04000C22 RID: 3106
		public readonly CardSelectionItemSpriteType SpriteType;

		// Token: 0x04000C23 RID: 3107
		public readonly string SpriteName;

		// Token: 0x04000C24 RID: 3108
		public readonly string SpriteLabel;

		// Token: 0x04000C25 RID: 3109
		public readonly IEnumerable<ClanCardSelectionItemPropertyInfo> Properties;

		// Token: 0x04000C26 RID: 3110
		public readonly bool IsSpecialActionItem;

		// Token: 0x04000C27 RID: 3111
		public readonly TextObject SpecialActionText;

		// Token: 0x04000C28 RID: 3112
		public readonly bool IsDisabled;

		// Token: 0x04000C29 RID: 3113
		public readonly TextObject DisabledReason;

		// Token: 0x04000C2A RID: 3114
		public readonly TextObject ActionResult;

		// Token: 0x04000C2B RID: 3115
		public readonly bool IsInitiallySelected;
	}
}
