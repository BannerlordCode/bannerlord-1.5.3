using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003B5 RID: 949
	public class SiegeMachineStonePile : UsableMachine, ISpawnable
	{
		// Token: 0x06003604 RID: 13828 RVA: 0x000DEE7D File Offset: 0x000DD07D
		protected internal override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000DEE88 File Offset: 0x000DD088
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=jfcceEoE}{PILE_TYPE} Pile", null);
				textObject.SetTextVariable("PILE_TYPE", new TextObject("{=1CPdu9K0}Stone", null));
				return textObject;
			}
			return null;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000DEECF File Offset: 0x000DD0CF
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (gameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
				textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				return textObject;
			}
			return null;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000DEF0F File Offset: 0x000DD10F
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x000DEF18 File Offset: 0x000DD118
		public override OrderType GetOrder(BattleSideEnum side)
		{
			return OrderType.None;
		}

		// Token: 0x0400170B RID: 5899
		private bool _spawnedFromSpawner;
	}
}
