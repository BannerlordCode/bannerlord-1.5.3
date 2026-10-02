using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000005 RID: 5
	public class MissionDuelLandmarkMarkerVM : ViewModel
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000209B File Offset: 0x0000029B
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020A3 File Offset: 0x000002A3
		public bool IsInScreenBoundaries { get; private set; }

		// Token: 0x06000006 RID: 6 RVA: 0x000020AC File Offset: 0x000002AC
		public MissionDuelLandmarkMarkerVM(GameEntity entity)
		{
			this.Entity = entity;
			this.FocusableComponent = this.Entity.GetFirstScriptOfType<DuelZoneLandmark>();
			this.TroopType = (int)this.Entity.GetFirstScriptOfType<DuelZoneLandmark>().ZoneTroopType;
			this.RefreshValues();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E8 File Offset: 0x000002E8
		public override void RefreshValues()
		{
			base.RefreshValues();
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
			GameTexts.SetVariable("KEY", keyHyperlinkText);
			GameTexts.SetVariable("ACTION", new TextObject("{=7jMnNlXG}Change Arena Preference", null));
			this.ActionDescriptionText = GameTexts.FindText("str_key_action", null).ToString();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002148 File Offset: 0x00000348
		public void UpdateScreenPosition(Camera missionCamera)
		{
			Vec3 globalPosition = this.Entity.GlobalPosition;
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreen(missionCamera, globalPosition, ref this._latestX, ref this._latestY, ref this._latestW);
			this.IsInScreenBoundaries = this._latestW > 0f && (this._latestX <= Screen.RealScreenResolutionWidth && this._latestY <= Screen.RealScreenResolutionHeight && this._latestX + 200f >= 0f) && this._latestY + 100f >= 0f;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000003F8
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002200 File Offset: 0x00000400
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000221E File Offset: 0x0000041E
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002226 File Offset: 0x00000426
		[DataSourceProperty]
		public int TroopType
		{
			get
			{
				return this._troopType;
			}
			set
			{
				if (value != this._troopType)
				{
					this._troopType = value;
					base.OnPropertyChangedWithValue(value, "TroopType");
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002244 File Offset: 0x00000444
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000224C File Offset: 0x0000044C
		[DataSourceProperty]
		public string ActionDescriptionText
		{
			get
			{
				return this._actionDescriptionText;
			}
			set
			{
				if (value != this._actionDescriptionText)
				{
					this._actionDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionDescriptionText");
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public readonly GameEntity Entity;

		// Token: 0x04000002 RID: 2
		public readonly IFocusable FocusableComponent;

		// Token: 0x04000003 RID: 3
		private float _latestX;

		// Token: 0x04000004 RID: 4
		private float _latestY;

		// Token: 0x04000005 RID: 5
		private float _latestW;

		// Token: 0x04000007 RID: 7
		private bool _isFocused;

		// Token: 0x04000008 RID: 8
		private int _troopType;

		// Token: 0x04000009 RID: 9
		private string _actionDescriptionText;
	}
}
