using System;
using System.Runtime.InteropServices;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000B9 RID: 185
	public struct MissionInitializerRecord : ISerializableObject
	{
		// Token: 0x0600099D RID: 2461 RVA: 0x0001F69C File Offset: 0x0001D89C
		public MissionInitializerRecord(string name)
		{
			this.TerrainType = -1;
			this.DamageToFriendsMultiplier = 1f;
			this.DamageFromPlayerToFriendsMultiplier = 1f;
			this.NeedsRandomTerrain = false;
			this.RandomTerrainSeed = 0;
			this.SceneName = name;
			this.SceneLevels = "";
			this.PlayingInCampaignMode = false;
			this.EnableSceneRecording = false;
			this.SceneUpgradeLevel = 0;
			this.SceneHasMapPatch = false;
			this.PatchCoordinates = Vec2.Zero;
			this.PatchEncounterDir = Vec2.Zero;
			this.DoNotUseLoadingScreen = false;
			this.DisableDynamicPointlightShadows = false;
			this.DecalAtlasGroup = 0;
			this.AtmosphereOnCampaign = AtmosphereInfo.GetInvalidAtmosphereInfo();
			this.DisableCorpseFadeOut = false;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001F740 File Offset: 0x0001D940
		void ISerializableObject.DeserializeFrom(IReader reader)
		{
			this.SceneName = reader.ReadString();
			this.SceneLevels = reader.ReadString();
			reader.ReadFloat();
			this.NeedsRandomTerrain = reader.ReadBool();
			this.RandomTerrainSeed = reader.ReadInt();
			this.EnableSceneRecording = reader.ReadBool();
			this.SceneUpgradeLevel = reader.ReadInt();
			this.PlayingInCampaignMode = reader.ReadBool();
			this.DisableDynamicPointlightShadows = reader.ReadBool();
			this.DoNotUseLoadingScreen = reader.ReadBool();
			this.DisableCorpseFadeOut = reader.ReadBool();
			if (reader.ReadBool())
			{
				this.AtmosphereOnCampaign = AtmosphereInfo.GetInvalidAtmosphereInfo();
				this.AtmosphereOnCampaign.DeserializeFrom(reader);
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001F7EC File Offset: 0x0001D9EC
		void ISerializableObject.SerializeTo(IWriter writer)
		{
			writer.WriteString(this.SceneName);
			writer.WriteString(this.SceneLevels);
			writer.WriteFloat(6f);
			writer.WriteBool(this.NeedsRandomTerrain);
			writer.WriteInt(this.RandomTerrainSeed);
			writer.WriteBool(this.EnableSceneRecording);
			writer.WriteInt(this.SceneUpgradeLevel);
			writer.WriteBool(this.PlayingInCampaignMode);
			writer.WriteBool(this.DisableDynamicPointlightShadows);
			writer.WriteBool(this.DoNotUseLoadingScreen);
			writer.WriteBool(this.DisableCorpseFadeOut);
			writer.WriteInt(this.DecalAtlasGroup);
			bool isValid = this.AtmosphereOnCampaign.IsValid;
			writer.WriteBool(isValid);
			if (isValid)
			{
				this.AtmosphereOnCampaign.SerializeTo(writer);
			}
		}

		// Token: 0x04000543 RID: 1347
		public int TerrainType;

		// Token: 0x04000544 RID: 1348
		public float DamageToFriendsMultiplier;

		// Token: 0x04000545 RID: 1349
		public float DamageFromPlayerToFriendsMultiplier;

		// Token: 0x04000546 RID: 1350
		[MarshalAs(UnmanagedType.U1)]
		public bool NeedsRandomTerrain;

		// Token: 0x04000547 RID: 1351
		public int RandomTerrainSeed;

		// Token: 0x04000548 RID: 1352
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string SceneName;

		// Token: 0x04000549 RID: 1353
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string SceneLevels;

		// Token: 0x0400054A RID: 1354
		[MarshalAs(UnmanagedType.U1)]
		public bool PlayingInCampaignMode;

		// Token: 0x0400054B RID: 1355
		[MarshalAs(UnmanagedType.U1)]
		public bool EnableSceneRecording;

		// Token: 0x0400054C RID: 1356
		public int SceneUpgradeLevel;

		// Token: 0x0400054D RID: 1357
		[MarshalAs(UnmanagedType.U1)]
		public bool SceneHasMapPatch;

		// Token: 0x0400054E RID: 1358
		public Vec2 PatchCoordinates;

		// Token: 0x0400054F RID: 1359
		public Vec2 PatchEncounterDir;

		// Token: 0x04000550 RID: 1360
		[MarshalAs(UnmanagedType.U1)]
		public bool DoNotUseLoadingScreen;

		// Token: 0x04000551 RID: 1361
		[MarshalAs(UnmanagedType.U1)]
		public bool DisableDynamicPointlightShadows;

		// Token: 0x04000552 RID: 1362
		[MarshalAs(UnmanagedType.I1)]
		public bool DisableCorpseFadeOut;

		// Token: 0x04000553 RID: 1363
		public int DecalAtlasGroup;

		// Token: 0x04000554 RID: 1364
		public AtmosphereInfo AtmosphereOnCampaign;
	}
}
