using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x0200003A RID: 58
	public class MissionGenericMarkerTargetVM : MissionNameMarkerTargetBaseVM
	{
		// Token: 0x06000427 RID: 1063 RVA: 0x00011905 File Offset: 0x0000FB05
		public MissionGenericMarkerTargetVM(string identifier, string nameType, string iconType, Vec3 position, TextObject name)
		{
			this.Identifier = identifier;
			base.NameType = nameType;
			base.IconType = iconType;
			this._position = position;
			this._name = name;
			this.RefreshValues();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00011938 File Offset: 0x0000FB38
		public override bool Equals(MissionNameMarkerTargetBaseVM other)
		{
			MissionGenericMarkerTargetVM missionGenericMarkerTargetVM;
			return (missionGenericMarkerTargetVM = other as MissionGenericMarkerTargetVM) != null && missionGenericMarkerTargetVM.Identifier == this.Identifier;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00011962 File Offset: 0x0000FB62
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._position + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001197B File Offset: 0x0000FB7B
		protected override TextObject GetName()
		{
			return this._name;
		}

		// Token: 0x04000226 RID: 550
		public readonly string Identifier;

		// Token: 0x04000227 RID: 551
		private readonly Vec3 _position;

		// Token: 0x04000228 RID: 552
		private readonly TextObject _name;
	}
}
