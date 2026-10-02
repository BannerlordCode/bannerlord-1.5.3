using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects.AreaMarkers
{
	// Token: 0x02000043 RID: 67
	public class AnimatedBasicAreaIndicator : AreaMarker
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000F539 File Offset: 0x0000D739
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000F541 File Offset: 0x0000D741
		public bool IsActive { get; private set; } = true;

		// Token: 0x0600028D RID: 653 RVA: 0x0000F54A File Offset: 0x0000D74A
		protected override void OnInit()
		{
			this._name = (string.IsNullOrEmpty(this.NameStringId) ? TextObject.GetEmpty() : GameTexts.FindText(this.NameStringId, null));
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000F572 File Offset: 0x0000D772
		public void SetIsActive(bool isActive)
		{
			this.IsActive = isActive;
			Campaign.Current.VisualTrackerManager.SetDirty();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000F58A File Offset: 0x0000D78A
		public void SetOverriddenName(TextObject name)
		{
			this._overriddenName = name;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F593 File Offset: 0x0000D793
		public override TextObject GetName()
		{
			if (!TextObject.IsNullOrEmpty(this._overriddenName))
			{
				return this._overriddenName;
			}
			return this._name;
		}

		// Token: 0x0400011D RID: 285
		public string NameStringId = "";

		// Token: 0x0400011E RID: 286
		public string Type;

		// Token: 0x0400011F RID: 287
		[EditorVisibleScriptComponentVariable(false)]
		private TextObject _name;

		// Token: 0x04000120 RID: 288
		private TextObject _overriddenName;
	}
}
