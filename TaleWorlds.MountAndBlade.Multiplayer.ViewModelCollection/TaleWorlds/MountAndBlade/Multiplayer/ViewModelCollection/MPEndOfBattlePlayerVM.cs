using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000A RID: 10
	public class MPEndOfBattlePlayerVM : MPPlayerVM
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00003A34 File Offset: 0x00001C34
		public MPEndOfBattlePlayerVM(MissionPeer peer, int displayedScore, int placement)
			: base(peer)
		{
			this._placement = placement;
			this._displayedScore = displayedScore;
			BasicCharacterObject @object = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			@object.UpdatePlayerCharacterBodyProperties(peer.Peer.BodyProperties, peer.Peer.Race, peer.Peer.IsFemale);
			@object.Age = peer.Peer.BodyProperties.Age;
			base.RefreshPreview(@object, peer.Peer.BodyProperties.DynamicProperties, peer.Peer.IsFemale);
			this.RefreshValues();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._scoreTextObj.SetTextVariable("SCORE", this._displayedScore);
			this.ScoreText = this._scoreTextObj.ToString();
			this.PlacementText = Common.ToRoman(this._placement);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003B30 File Offset: 0x00001D30
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003B38 File Offset: 0x00001D38
		[DataSourceProperty]
		public string PlacementText
		{
			get
			{
				return this._placementText;
			}
			set
			{
				if (value != this._placementText)
				{
					this._placementText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlacementText");
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003B5B File Offset: 0x00001D5B
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003B63 File Offset: 0x00001D63
		[DataSourceProperty]
		public string ScoreText
		{
			get
			{
				return this._scoreText;
			}
			set
			{
				if (value != this._scoreText)
				{
					this._scoreText = value;
					base.OnPropertyChangedWithValue<string>(value, "ScoreText");
				}
			}
		}

		// Token: 0x0400004D RID: 77
		private readonly int _placement;

		// Token: 0x0400004E RID: 78
		private readonly int _displayedScore;

		// Token: 0x0400004F RID: 79
		private TextObject _scoreTextObj = new TextObject("{=Kvqb1lQR}{SCORE} Score", null);

		// Token: 0x04000050 RID: 80
		private string _placementText;

		// Token: 0x04000051 RID: 81
		private string _scoreText;
	}
}
