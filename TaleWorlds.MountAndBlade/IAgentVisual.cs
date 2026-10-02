using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CF RID: 719
	public interface IAgentVisual
	{
		// Token: 0x060029BC RID: 10684
		void SetAction(in ActionIndexCache actionName, float startProgress = 0f, bool forceFaceMorphRestart = true);

		// Token: 0x060029BD RID: 10685
		MBAgentVisuals GetVisuals();

		// Token: 0x060029BE RID: 10686
		MatrixFrame GetFrame();

		// Token: 0x060029BF RID: 10687
		BodyProperties GetBodyProperties();

		// Token: 0x060029C0 RID: 10688
		void SetBodyProperties(BodyProperties bodyProperties);

		// Token: 0x060029C1 RID: 10689
		bool GetIsFemale();

		// Token: 0x060029C2 RID: 10690
		string GetCharacterObjectID();

		// Token: 0x060029C3 RID: 10691
		void SetCharacterObjectID(string id);

		// Token: 0x060029C4 RID: 10692
		Equipment GetEquipment();

		// Token: 0x060029C5 RID: 10693
		void SetClothingColors(uint color1, uint color2);

		// Token: 0x060029C6 RID: 10694
		void GetClothingColors(out uint color1, out uint color2);

		// Token: 0x060029C7 RID: 10695
		AgentVisualsData GetCopyAgentVisualsData();

		// Token: 0x060029C8 RID: 10696
		void Refresh(bool needBatchedVersionForWeaponMeshes, AgentVisualsData data, bool forceUseFaceCache = false);
	}
}
