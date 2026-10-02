using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000378 RID: 888
	public class DuelZoneLandmark : ScriptComponentBehavior, IFocusable
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x000D1570 File Offset: 0x000CF770
		public FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.None;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x000D1573 File Offset: 0x000CF773
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000D1576 File Offset: 0x000CF776
		public void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000D1578 File Offset: 0x000CF778
		public void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000D157A File Offset: 0x000CF77A
		public TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return null;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000D157D File Offset: 0x000CF77D
		public TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x040015B6 RID: 5558
		public TroopType ZoneTroopType;
	}
}
