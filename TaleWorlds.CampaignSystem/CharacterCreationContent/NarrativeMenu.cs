using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x0200021C RID: 540
	public sealed class NarrativeMenu
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x00093EB3 File Offset: 0x000920B3
		public List<NarrativeMenuCharacter> Characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x00093EBB File Offset: 0x000920BB
		public MBReadOnlyList<NarrativeMenuOption> CharacterCreationMenuOptions
		{
			get
			{
				return this._characterCreationMenuOptions;
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00093EC4 File Offset: 0x000920C4
		public NarrativeMenu(string stringId, string inputMenuId, string outputMenuId, TextObject title, TextObject description, List<NarrativeMenuCharacter> characters, NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate getNarrativeMenuCharacterArgs)
		{
			this.StringId = stringId;
			this.InputMenuId = inputMenuId;
			this.OutputMenuId = outputMenuId;
			this.Title = title;
			this.Description = description;
			this._characters = characters;
			this.GetNarrativeMenuCharacterArgs = getNarrativeMenuCharacterArgs;
			this._characterCreationMenuOptions = new MBList<NarrativeMenuOption>();
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00093F17 File Offset: 0x00092117
		public void AddNarrativeMenuOption(NarrativeMenuOption narrativeMenuOption)
		{
			this._characterCreationMenuOptions.Add(narrativeMenuOption);
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00093F25 File Offset: 0x00092125
		public void RemoveNarrativeMenuOption(NarrativeMenuOption narrativeMenuOption)
		{
			this._characterCreationMenuOptions.Remove(narrativeMenuOption);
		}

		// Token: 0x04000987 RID: 2439
		public readonly string StringId;

		// Token: 0x04000988 RID: 2440
		public readonly string InputMenuId;

		// Token: 0x04000989 RID: 2441
		public readonly string OutputMenuId;

		// Token: 0x0400098A RID: 2442
		public readonly TextObject Title;

		// Token: 0x0400098B RID: 2443
		public readonly TextObject Description;

		// Token: 0x0400098C RID: 2444
		private readonly List<NarrativeMenuCharacter> _characters;

		// Token: 0x0400098D RID: 2445
		private readonly MBList<NarrativeMenuOption> _characterCreationMenuOptions;

		// Token: 0x0400098E RID: 2446
		public readonly NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate GetNarrativeMenuCharacterArgs;

		// Token: 0x02000637 RID: 1591
		// (Invoke) Token: 0x06005339 RID: 21305
		public delegate List<NarrativeMenuCharacterArgs> GetNarrativeMenuCharacterArgsDelegate(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager);
	}
}
