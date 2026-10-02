using System;
using TaleWorlds.MountAndBlade.Missions.Hints;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000042 RID: 66
	internal class MissionHintInteractionItemVM : MissionInteractionItemBaseVM
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x00015999 File Offset: 0x00013B99
		public MissionHintInteractionItemVM(MissionHint hint)
		{
			this.Hint = hint;
			this.RefreshValues();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000159AE File Offset: 0x00013BAE
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Message = this.Hint.Description.ToString();
		}

		// Token: 0x04000293 RID: 659
		public readonly MissionHint Hint;
	}
}
