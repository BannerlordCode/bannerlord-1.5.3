using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation.Persuasion
{
	// Token: 0x020002B2 RID: 690
	public class PersuasionTask
	{
		// Token: 0x060024F6 RID: 9462 RVA: 0x0009FB2B File Offset: 0x0009DD2B
		public PersuasionTask(int reservationType)
		{
			this.Options = new MBList<PersuasionOptionArgs>();
			this.ReservationType = reservationType;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x0009FB45 File Offset: 0x0009DD45
		public void AddOptionToTask(PersuasionOptionArgs option)
		{
			this.Options.Add(option);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x0009FB54 File Offset: 0x0009DD54
		public void BlockAllOptions()
		{
			foreach (PersuasionOptionArgs persuasionOptionArgs in this.Options)
			{
				persuasionOptionArgs.BlockTheOption(true);
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0009FBA8 File Offset: 0x0009DDA8
		public void UnblockAllOptions()
		{
			foreach (PersuasionOptionArgs persuasionOptionArgs in this.Options)
			{
				persuasionOptionArgs.BlockTheOption(false);
			}
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x0009FBFC File Offset: 0x0009DDFC
		public void ApplyEffects(float moveToNextStageChance, float blockRandomOptionChance)
		{
			if (moveToNextStageChance > MBRandom.RandomFloat)
			{
				this.BlockAllOptions();
				return;
			}
			if (blockRandomOptionChance > MBRandom.RandomFloat)
			{
				PersuasionOptionArgs randomElementWithPredicate = this.Options.GetRandomElementWithPredicate<PersuasionOptionArgs>((PersuasionOptionArgs x) => !x.IsBlocked);
				if (randomElementWithPredicate == null)
				{
					return;
				}
				randomElementWithPredicate.BlockTheOption(true);
			}
		}

		// Token: 0x04000B2C RID: 2860
		public readonly MBList<PersuasionOptionArgs> Options;

		// Token: 0x04000B2D RID: 2861
		public TextObject SpokenLine;

		// Token: 0x04000B2E RID: 2862
		public TextObject ImmediateFailLine;

		// Token: 0x04000B2F RID: 2863
		public TextObject FinalFailLine;

		// Token: 0x04000B30 RID: 2864
		public TextObject TryLaterLine;

		// Token: 0x04000B31 RID: 2865
		public readonly int ReservationType;
	}
}
