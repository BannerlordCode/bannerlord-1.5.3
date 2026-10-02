using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000041 RID: 65
	public class SkillIconVisualWidget : Widget
	{
		// Token: 0x060003C3 RID: 963 RVA: 0x0000C03A File Offset: 0x0000A23A
		public SkillIconVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000C04C File Offset: 0x0000A24C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._requiresRefresh)
			{
				if (this.SkillId == null)
				{
					Debug.FailedAssert("SkillIconVisualWidget.OnLateUpdate called before SkillId has been set, or SkillId is set to null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\SkillIconVisualWidget.cs", "OnLateUpdate", 21);
					this._requiresRefresh = false;
					return;
				}
				string text = "SPGeneral\\Skills\\gui_skills_icon_" + this.SkillId.ToLower();
				if (this.UseSmallestVariation && base.Context.SpriteData.GetSprite(text + "_tiny") != null)
				{
					base.Sprite = base.Context.SpriteData.GetSprite(text + "_tiny");
				}
				else if (this.UseSmallVariation && base.Context.SpriteData.GetSprite(text + "_small") != null)
				{
					base.Sprite = base.Context.SpriteData.GetSprite(text + "_small");
				}
				else if (base.Context.SpriteData.GetSprite(text) != null)
				{
					base.Sprite = base.Context.SpriteData.GetSprite(text);
				}
				this._requiresRefresh = false;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0000C168 File Offset: 0x0000A368
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x0000C170 File Offset: 0x0000A370
		[Editor(false)]
		public string SkillId
		{
			get
			{
				return this._skillId;
			}
			set
			{
				if (this._skillId != value)
				{
					this._skillId = value;
					base.OnPropertyChanged<string>(value, "SkillId");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000C19A File Offset: 0x0000A39A
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0000C1A2 File Offset: 0x0000A3A2
		[Editor(false)]
		public bool UseSmallVariation
		{
			get
			{
				return this._useSmallVariation;
			}
			set
			{
				if (this._useSmallVariation != value)
				{
					this._useSmallVariation = value;
					base.OnPropertyChanged(value, "UseSmallVariation");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000C1C7 File Offset: 0x0000A3C7
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0000C1CF File Offset: 0x0000A3CF
		[Editor(false)]
		public bool UseSmallestVariation
		{
			get
			{
				return this._useSmallestVariation;
			}
			set
			{
				if (this._useSmallestVariation != value)
				{
					this._useSmallestVariation = value;
					base.OnPropertyChanged(value, "UseSmallestVariation");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x04000191 RID: 401
		private bool _requiresRefresh = true;

		// Token: 0x04000192 RID: 402
		private string _skillId;

		// Token: 0x04000193 RID: 403
		private bool _useSmallVariation;

		// Token: 0x04000194 RID: 404
		private bool _useSmallestVariation;
	}
}
