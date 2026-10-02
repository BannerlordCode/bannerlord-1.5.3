using System;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction
{
	// Token: 0x0200003F RID: 63
	public interface IInteractionInterfaceHandler
	{
		// Token: 0x0600057D RID: 1405
		void AddInteractionMessage(MissionInteractionItemBaseVM message);

		// Token: 0x0600057E RID: 1406
		void RemoveInteractionMessage(MissionInteractionItemBaseVM message);

		// Token: 0x0600057F RID: 1407
		bool HasInteractionMessage(MissionInteractionItemBaseVM message);
	}
}
