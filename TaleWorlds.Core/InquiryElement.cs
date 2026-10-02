using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core
{
	// Token: 0x02000091 RID: 145
	public class InquiryElement
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x0001D05F File Offset: 0x0001B25F
		public InquiryElement(object identifier, string title, ImageIdentifier imageIdentifier)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.ImageIdentifier = imageIdentifier;
			this.IsEnabled = true;
			this.Hint = null;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001D08A File Offset: 0x0001B28A
		public InquiryElement(object identifier, string title, ImageIdentifier imageIdentifier, bool isEnabled, string hint)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.ImageIdentifier = imageIdentifier;
			this.IsEnabled = isEnabled;
			this.Hint = hint;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001D0B8 File Offset: 0x0001B2B8
		public bool HasSameContentWith(object other)
		{
			InquiryElement inquiryElement;
			if ((inquiryElement = other as InquiryElement) != null)
			{
				if (this.Title == inquiryElement.Title)
				{
					if (this.ImageIdentifier != null || inquiryElement.ImageIdentifier != null)
					{
						ImageIdentifier imageIdentifier = this.ImageIdentifier;
						if (imageIdentifier == null || !imageIdentifier.Equals(inquiryElement.ImageIdentifier))
						{
							return false;
						}
					}
					if (this.Identifier == inquiryElement.Identifier && this.IsEnabled == inquiryElement.IsEnabled)
					{
						return this.Hint == inquiryElement.Hint;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04000464 RID: 1124
		public readonly string Title;

		// Token: 0x04000465 RID: 1125
		public readonly ImageIdentifier ImageIdentifier;

		// Token: 0x04000466 RID: 1126
		public readonly object Identifier;

		// Token: 0x04000467 RID: 1127
		public readonly bool IsEnabled;

		// Token: 0x04000468 RID: 1128
		public readonly string Hint;
	}
}
