using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000007 RID: 7
	public class MissionDuelPeerMarkerVM : ViewModel
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002D1C File Offset: 0x00000F1C
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002D24 File Offset: 0x00000F24
		public MissionPeer TargetPeer { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002D2D File Offset: 0x00000F2D
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002D35 File Offset: 0x00000F35
		public float Distance { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002D3E File Offset: 0x00000F3E
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00002D46 File Offset: 0x00000F46
		public bool IsInDuel { get; private set; }

		// Token: 0x06000030 RID: 48 RVA: 0x00002D50 File Offset: 0x00000F50
		public MissionDuelPeerMarkerVM(MissionPeer peer)
		{
			this.TargetPeer = peer;
			this.Bounty = (peer.Representative as DuelMissionRepresentative).Bounty;
			this.IsEnabled = true;
			TargetIconType iconType = MultiplayerClassDivisions.GetMPHeroClassForPeer(this.TargetPeer, false).IconType;
			this.CompassElement = new MPTeammateCompassTargetVM(iconType, Color.White.ToUnsignedInteger(), Color.White.ToUnsignedInteger(), new Banner(), true);
			this.SelectedPerks = new MBBindingList<MPPerkVM>();
			this.RefreshPerkSelection();
			this.RefreshValues();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002DDC File Offset: 0x00000FDC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.TargetPeer.DisplayedName;
			this._acceptDuelRequestText = new TextObject("{=tidE1V1k}Accept duel", null);
			this._sendDuelRequestText = new TextObject("{=YLPJWgqF}Challenge", null);
			this._waitingForDuelResponseText = new TextObject("{=MPgnsZoo}Waiting for response", null);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002E34 File Offset: 0x00001034
		public void OnTick(float dt)
		{
			if (Agent.Main != null && this.TargetPeer.ControlledAgent != null)
			{
				this.Distance = this._latestW;
			}
			if (this.HasSentDuelRequest)
			{
				this._currentDuelRequestTimeRemaining -= dt;
				GameTexts.SetVariable("SECONDS", (int)this._currentDuelRequestTimeRemaining);
				GameTexts.SetVariable("ACTION", this._waitingForDuelResponseText);
				this.ActionDescriptionText = new TextObject("{=HXWpxvgT}{ACTION} ({SECONDS})", null).ToString();
				if (this._currentDuelRequestTimeRemaining <= 0f)
				{
					this.HasSentDuelRequest = false;
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002EC4 File Offset: 0x000010C4
		public void UpdateScreenPosition(Camera missionCamera)
		{
			if (this.TargetPeer.ControlledAgent == null)
			{
				return;
			}
			Vec3 vec = this.TargetPeer.ControlledAgent.GetWorldPosition().GetGroundVec3();
			vec += new Vec3(0f, 0f, this.TargetPeer.ControlledAgent.GetEyeGlobalHeight(), -1f);
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreen(missionCamera, vec, ref this._latestX, ref this._latestY, ref this._latestW);
			this.ScreenPosition = new Vec2(this._latestX, this._latestY);
			this.IsAgentInScreenBoundaries = this._latestX <= Screen.RealScreenResolutionWidth && this._latestY <= Screen.RealScreenResolutionHeight && this._latestX + 200f >= 0f && this._latestY + 100f >= 0f;
			this._wPosAfterPositionCalculation = ((this._latestW < 0f) ? (-1f) : 1.1f);
			this.WSign = (int)this._wPosAfterPositionCalculation;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002FF0 File Offset: 0x000011F0
		private void OnInteractionChanged()
		{
			this.ActionDescriptionText = "";
			if (this.HasDuelRequestForPlayer)
			{
				string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
				GameTexts.SetVariable("KEY", keyHyperlinkText);
				GameTexts.SetVariable("ACTION", this._acceptDuelRequestText);
				this.ActionDescriptionText = GameTexts.FindText("str_key_action", null).ToString();
				return;
			}
			if (this.HasSentDuelRequest)
			{
				this._currentDuelRequestTimeRemaining = 10f;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000306C File Offset: 0x0000126C
		private void SetFocused(bool isFocused)
		{
			if (!this.HasDuelRequestForPlayer && !this.HasSentDuelRequest)
			{
				if (isFocused)
				{
					string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
					GameTexts.SetVariable("KEY", keyHyperlinkText);
					GameTexts.SetVariable("ACTION", this._sendDuelRequestText);
					this.ActionDescriptionText = GameTexts.FindText("str_key_action", null).ToString();
					return;
				}
				this.ActionDescriptionText = string.Empty;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030E0 File Offset: 0x000012E0
		public void UpdateBounty()
		{
			this.Bounty = (this.TargetPeer.Representative as DuelMissionRepresentative).Bounty;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003100 File Offset: 0x00001300
		private void UpdateTracked()
		{
			if (!this.IsEnabled)
			{
				this.IsTracked = false;
			}
			else if (this.HasDuelRequestForPlayer || this.HasSentDuelRequest || this.IsFocused)
			{
				this.IsTracked = true;
			}
			else
			{
				this.IsTracked = false;
			}
			this.ShouldShowInformation = this.IsTracked || this.IsFocused;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000315D File Offset: 0x0000135D
		public void OnDuelStarted()
		{
			this.IsEnabled = false;
			this.IsInDuel = true;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000316D File Offset: 0x0000136D
		public void OnDuelEnded()
		{
			this.IsEnabled = true;
			this.IsInDuel = false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000317D File Offset: 0x0000137D
		public void UpdateCurentDuelStatus(bool isInDuel)
		{
			this.IsInDuel = isInDuel;
			this.IsEnabled = !this.IsInDuel;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003198 File Offset: 0x00001398
		public void RefreshPerkSelection()
		{
			this.SelectedPerks.Clear();
			this.TargetPeer.RefreshSelectedPerks();
			foreach (MPPerkObject mpperkObject in this.TargetPeer.SelectedPerks)
			{
				this.SelectedPerks.Add(new MPPerkVM(null, mpperkObject, true, 0));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003214 File Offset: 0x00001414
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000321C File Offset: 0x0000141C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.UpdateTracked();
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003240 File Offset: 0x00001440
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00003248 File Offset: 0x00001448
		[DataSourceProperty]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChangedWithValue(value, "IsTracked");
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003266 File Offset: 0x00001466
		// (set) Token: 0x06000041 RID: 65 RVA: 0x0000326E File Offset: 0x0000146E
		[DataSourceProperty]
		public bool ShouldShowInformation
		{
			get
			{
				return this._shouldShowInformation;
			}
			set
			{
				if (value != this._shouldShowInformation)
				{
					this._shouldShowInformation = value;
					base.OnPropertyChangedWithValue(value, "ShouldShowInformation");
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000328C File Offset: 0x0000148C
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003294 File Offset: 0x00001494
		[DataSourceProperty]
		public bool IsAgentInScreenBoundaries
		{
			get
			{
				return this._isAgentInScreenBoundaries;
			}
			set
			{
				if (value != this._isAgentInScreenBoundaries)
				{
					this._isAgentInScreenBoundaries = value;
					base.OnPropertyChangedWithValue(value, "IsAgentInScreenBoundaries");
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000032B2 File Offset: 0x000014B2
		// (set) Token: 0x06000045 RID: 69 RVA: 0x000032BA File Offset: 0x000014BA
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
					this.SetFocused(value);
					this.UpdateTracked();
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000032E5 File Offset: 0x000014E5
		// (set) Token: 0x06000047 RID: 71 RVA: 0x000032ED File Offset: 0x000014ED
		[DataSourceProperty]
		public bool HasDuelRequestForPlayer
		{
			get
			{
				return this._hasDuelRequestForPlayer;
			}
			set
			{
				if (value != this._hasDuelRequestForPlayer)
				{
					this._hasDuelRequestForPlayer = value;
					base.OnPropertyChangedWithValue(value, "HasDuelRequestForPlayer");
					this.OnInteractionChanged();
					this.UpdateTracked();
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003317 File Offset: 0x00001517
		// (set) Token: 0x06000049 RID: 73 RVA: 0x0000331F File Offset: 0x0000151F
		[DataSourceProperty]
		public bool HasSentDuelRequest
		{
			get
			{
				return this._hasSentDuelRequest;
			}
			set
			{
				if (value != this._hasSentDuelRequest)
				{
					this._hasSentDuelRequest = value;
					base.OnPropertyChangedWithValue(value, "HasSentDuelRequest");
					this.OnInteractionChanged();
					this.UpdateTracked();
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003349 File Offset: 0x00001549
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003351 File Offset: 0x00001551
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003374 File Offset: 0x00001574
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000337C File Offset: 0x0000157C
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000339F File Offset: 0x0000159F
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000033A7 File Offset: 0x000015A7
		[DataSourceProperty]
		public int Bounty
		{
			get
			{
				return this._bounty;
			}
			set
			{
				if (value != this._bounty)
				{
					this._bounty = value;
					base.OnPropertyChangedWithValue(value, "Bounty");
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000033C5 File Offset: 0x000015C5
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000033CD File Offset: 0x000015CD
		[DataSourceProperty]
		public int PreferredArenaType
		{
			get
			{
				return this._preferredArenaType;
			}
			set
			{
				if (value != this._preferredArenaType)
				{
					this._preferredArenaType = value;
					base.OnPropertyChangedWithValue(value, "PreferredArenaType");
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000033EB File Offset: 0x000015EB
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000033F3 File Offset: 0x000015F3
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003411 File Offset: 0x00001611
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003419 File Offset: 0x00001619
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003454 File Offset: 0x00001654
		// (set) Token: 0x06000057 RID: 87 RVA: 0x0000345C File Offset: 0x0000165C
		[DataSourceProperty]
		public MPTeammateCompassTargetVM CompassElement
		{
			get
			{
				return this._compassElement;
			}
			set
			{
				if (value != this._compassElement)
				{
					this._compassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "CompassElement");
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000058 RID: 88 RVA: 0x0000347A File Offset: 0x0000167A
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003482 File Offset: 0x00001682
		[DataSourceProperty]
		public MBBindingList<MPPerkVM> SelectedPerks
		{
			get
			{
				return this._selectedPerks;
			}
			set
			{
				if (value != this._selectedPerks)
				{
					this._selectedPerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPerkVM>>(value, "SelectedPerks");
				}
			}
		}

		// Token: 0x0400001F RID: 31
		private float _currentDuelRequestTimeRemaining;

		// Token: 0x04000020 RID: 32
		private float _latestX;

		// Token: 0x04000021 RID: 33
		private float _latestY;

		// Token: 0x04000022 RID: 34
		private float _latestW;

		// Token: 0x04000023 RID: 35
		private float _wPosAfterPositionCalculation;

		// Token: 0x04000024 RID: 36
		private TextObject _acceptDuelRequestText;

		// Token: 0x04000025 RID: 37
		private TextObject _sendDuelRequestText;

		// Token: 0x04000026 RID: 38
		private TextObject _waitingForDuelResponseText;

		// Token: 0x04000028 RID: 40
		private bool _isEnabled;

		// Token: 0x04000029 RID: 41
		private bool _isTracked;

		// Token: 0x0400002A RID: 42
		private bool _shouldShowInformation;

		// Token: 0x0400002B RID: 43
		private bool _isAgentInScreenBoundaries;

		// Token: 0x0400002C RID: 44
		private bool _isFocused;

		// Token: 0x0400002D RID: 45
		private bool _hasDuelRequestForPlayer;

		// Token: 0x0400002E RID: 46
		private bool _hasSentDuelRequest;

		// Token: 0x0400002F RID: 47
		private string _name;

		// Token: 0x04000030 RID: 48
		private string _actionDescriptionText;

		// Token: 0x04000031 RID: 49
		private int _bounty;

		// Token: 0x04000032 RID: 50
		private int _preferredArenaType;

		// Token: 0x04000033 RID: 51
		private int _wSign;

		// Token: 0x04000034 RID: 52
		private Vec2 _screenPosition;

		// Token: 0x04000035 RID: 53
		private MPTeammateCompassTargetVM _compassElement;

		// Token: 0x04000036 RID: 54
		private MBBindingList<MPPerkVM> _selectedPerks;
	}
}
