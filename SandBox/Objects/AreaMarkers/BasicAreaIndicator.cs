using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects.AreaMarkers
{
	// Token: 0x02000044 RID: 68
	public class BasicAreaIndicator : AreaMarker
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000F5C9 File Offset: 0x0000D7C9
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000F5D1 File Offset: 0x0000D7D1
		public bool IsActive { get; private set; } = true;

		// Token: 0x06000294 RID: 660 RVA: 0x0000F5DA File Offset: 0x0000D7DA
		protected override void OnInit()
		{
			this._name = (string.IsNullOrEmpty(this.NameStringId) ? TextObject.GetEmpty() : GameTexts.FindText(this.NameStringId, null));
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000F602 File Offset: 0x0000D802
		public void SetIsActive(bool isActive)
		{
			this.IsActive = isActive;
			Campaign.Current.VisualTrackerManager.SetDirty();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F61A File Offset: 0x0000D81A
		public void SetOverriddenName(TextObject name)
		{
			this._overriddenName = name;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F623 File Offset: 0x0000D823
		public override TextObject GetName()
		{
			if (!TextObject.IsNullOrEmpty(this._overriddenName))
			{
				return this._overriddenName;
			}
			return this._name;
		}

		// Token: 0x04000122 RID: 290
		public string NameStringId = "";

		// Token: 0x04000123 RID: 291
		public string Type;

		// Token: 0x04000124 RID: 292
		[EditorVisibleScriptComponentVariable(false)]
		private TextObject _name;

		// Token: 0x04000125 RID: 293
		private TextObject _overriddenName;
	}
}
