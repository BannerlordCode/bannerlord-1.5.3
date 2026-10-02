using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders.ImageIdentifiers
{
	// Token: 0x02000025 RID: 37
	public class CharacterImageTextureProvider : ImageIdentifierTextureProvider
	{
		// Token: 0x0600017C RID: 380 RVA: 0x000091D8 File Offset: 0x000073D8
		protected override void OnCreateImageWithId(string id, string additionalArgs)
		{
			if (string.IsNullOrEmpty(id))
			{
				if (base.ThumbnailCreationData != null)
				{
					CharacterThumbnailCreationData characterThumbnailCreationData;
					if ((characterThumbnailCreationData = base.ThumbnailCreationData as CharacterThumbnailCreationData) == null)
					{
						goto IL_0044;
					}
					CharacterCode characterCode = characterThumbnailCreationData.CharacterCode;
					if (characterCode != null && !characterCode.IsEmpty)
					{
						goto IL_0044;
					}
				}
				base.OnTextureCreated(ThumbnailCacheManager.Current.GetCachedHeroSilhouetteTexture());
				return;
			}
			IL_0044:
			CharacterCode characterCode2 = CharacterCode.CreateFrom(id);
			if (FaceGen.GetMaturityTypeWithAge(characterCode2.BodyProperties.Age) <= BodyMeshMaturityType.Child)
			{
				base.OnTextureCreated(null);
				return;
			}
			int num = -1;
			int num2 = -1;
			if (!string.IsNullOrEmpty(additionalArgs))
			{
				string[] array = additionalArgs.Split(new char[] { ';' });
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[] { '=' });
					if (array2.Length == 2)
					{
						int num4;
						if (array2[0] == "customSizeX")
						{
							int num3;
							if (int.TryParse(array2[1], out num3))
							{
								num = num3;
							}
						}
						else if (array2[0] == "customSizeY" && int.TryParse(array2[1], out num4))
						{
							num2 = num4;
						}
					}
				}
			}
			base.ThumbnailCreationData = new CharacterThumbnailCreationData(characterCode2, new Action<Texture>(base.OnTextureCreated), new Action(base.OnTextureCreationCancelled), base.IsBig, num, num2);
			ThumbnailCacheManager.Current.CreateTexture(base.ThumbnailCreationData);
		}
	}
}
