using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Scripts;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000043 RID: 67
	public class CharacterThumbnailCache : ThumbnailCache<CharacterThumbnailCreationData>
	{
		// Token: 0x06000245 RID: 581 RVA: 0x0000F364 File Offset: 0x0000D564
		public CharacterThumbnailCache(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000F36D File Offset: 0x0000D56D
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._characterTableauGPUAllocationIndex = Utilities.RegisterGPUAllocationGroup("CharacterTableauCache");
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000F385 File Offset: 0x0000D585
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000F390 File Offset: 0x0000D590
		protected override TextureCreationInfo OnCreateTexture(CharacterThumbnailCreationData thumbnailCreationData)
		{
			string renderId = thumbnailCreationData.RenderId;
			CharacterCode characterCode = thumbnailCreationData.CharacterCode;
			bool isBig = thumbnailCreationData.IsBig;
			Action<Texture> setAction = thumbnailCreationData.SetAction;
			Action cancelAction = thumbnailCreationData.CancelAction;
			int customSizeX = thumbnailCreationData.CustomSizeX;
			int customSizeY = thumbnailCreationData.CustomSizeY;
			Texture texture;
			if (((IThumbnailCache)this).GetValue(renderId, out texture))
			{
				if (this._renderCallbacks.ContainsKey(renderId))
				{
					this._renderCallbacks[renderId].SetActions.Add(setAction);
					this._renderCallbacks[renderId].CancelActions.Add(cancelAction);
				}
				else if (setAction != null)
				{
					setAction(texture);
				}
				((IThumbnailCache)this).AddReference(renderId);
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			Camera camera = null;
			int num = (isBig ? 0 : 4);
			GameEntity gameEntity = this.CreateCharacterBaseEntity(characterCode, BannerlordTableauManager.TableauCharacterScenes[num], ref camera, isBig);
			gameEntity = this.FillEntityWithPose(characterCode, gameEntity, BannerlordTableauManager.TableauCharacterScenes[num]);
			int num2 = 256;
			int num3 = (isBig ? 120 : 174);
			if (customSizeX > 0)
			{
				num2 = customSizeX;
			}
			if (customSizeY > 0)
			{
				num3 = customSizeY;
			}
			string text = ThumbnailCache<CharacterThumbnailCreationData>.CreateDebugIdFrom(renderId, "cha", "");
			ThumbnailRenderRequest thumbnailRenderRequest = ThumbnailRenderRequest.CreateWithoutTexture(BannerlordTableauManager.TableauCharacterScenes[num], camera, gameEntity, renderId, num2, num3, text, this._characterTableauGPUAllocationIndex);
			this._thumbnailCreatorView.RegisterRenderRequest(ref thumbnailRenderRequest);
			gameEntity.ManualInvalidate();
			this._characterCount++;
			((IThumbnailCache)this).Add(renderId, null);
			((IThumbnailCache)this).AddReference(renderId);
			if (!this._renderCallbacks.ContainsKey(renderId))
			{
				this._renderCallbacks.Add(renderId, RenderCallbackCollection.CreateEmpty());
			}
			this._renderCallbacks[renderId].SetActions.Add(setAction);
			this._renderCallbacks[renderId].CancelActions.Add(cancelAction);
			return TextureCreationInfo.WithNewTexture(null);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000F558 File Offset: 0x0000D758
		protected override bool OnReleaseTexture(CharacterThumbnailCreationData thumbnailCreationData)
		{
			string renderId = thumbnailCreationData.RenderId;
			return ((IThumbnailCache)this).RemoveReference(renderId);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000F574 File Offset: 0x0000D774
		private GameEntity CreateCharacterBaseEntity(CharacterCode characterCode, Scene scene, ref Camera camera, bool isBig)
		{
			string text;
			bool flag;
			this.GetPoseParamsFromCharacterCode(characterCode, out text, out flag);
			string text2 = text + "_pose";
			string text3 = (isBig ? (text + "_cam") : (text + "_cam_small"));
			WeakGameEntity weakGameEntity = scene.FindWeakEntityWithTag(text2);
			if (weakGameEntity == null)
			{
				return null;
			}
			weakGameEntity.SetVisibilityExcludeParents(true);
			GameEntity gameEntity = GameEntity.CopyFromPrefab(weakGameEntity);
			gameEntity.Name = weakGameEntity.Name + "Instance";
			gameEntity.RemoveTag(text2);
			scene.AttachEntity(gameEntity, false);
			gameEntity.SetVisibilityExcludeParents(true);
			weakGameEntity.SetVisibilityExcludeParents(false);
			WeakGameEntity weakGameEntity2 = scene.FindWeakEntityWithTag(text3);
			Vec3 vec = default(Vec3);
			camera = Camera.CreateCamera();
			if (weakGameEntity2 != null)
			{
				weakGameEntity2.GetCameraParamsFromCameraScript(camera, ref vec);
				camera.Frame = weakGameEntity2.GetGlobalFrame();
			}
			return gameEntity;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000F654 File Offset: 0x0000D854
		private void GetPoseParamsFromCharacterCode(CharacterCode characterCode, out string poseName, out bool hasHorse)
		{
			hasHorse = false;
			if (characterCode.IsHero)
			{
				int num = MBRandom.NondeterministicRandomInt % 8;
				poseName = "lord_" + num;
				return;
			}
			poseName = "troop_villager";
			int num2 = -1;
			int num3 = -1;
			Equipment equipment = characterCode.CalculateEquipment();
			switch (characterCode.FormationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.Cavalry:
			case FormationClass.NumberOfDefaultFormations:
			case FormationClass.HeavyInfantry:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
			case FormationClass.NumberOfRegularFormations:
			case FormationClass.Bodyguard:
			{
				for (int i = 0; i < 4; i++)
				{
					ItemObject item = equipment[i].Item;
					if (((item != null) ? item.PrimaryWeapon : null) != null)
					{
						if (num3 == -1 && equipment[i].Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand))
						{
							num3 = i;
						}
						if (num2 == -1 && equipment[i].Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.MeleeWeapon))
						{
							num2 = i;
						}
					}
				}
				break;
			}
			case FormationClass.Ranged:
			case FormationClass.HorseArcher:
			{
				for (int j = 0; j < 4; j++)
				{
					ItemObject item2 = equipment[j].Item;
					if (((item2 != null) ? item2.PrimaryWeapon : null) != null)
					{
						if (num3 == -1 && equipment[j].Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand))
						{
							num3 = j;
						}
						if (num2 == -1 && equipment[j].Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.RangedWeapon))
						{
							num2 = j;
						}
					}
				}
				break;
			}
			}
			if (num2 != -1)
			{
				WeaponClass weaponClass = equipment[num2].Item.PrimaryWeapon.WeaponClass;
				switch (weaponClass)
				{
				case WeaponClass.OneHandedSword:
				case WeaponClass.OneHandedAxe:
					if (num3 == -1)
					{
						poseName = "troop_infantry_sword1h";
						goto IL_0250;
					}
					if (equipment[num3].Item.PrimaryWeapon.IsShield)
					{
						poseName = "troop_infantry_sword1h";
						goto IL_0250;
					}
					goto IL_0250;
				case WeaponClass.TwoHandedSword:
				case WeaponClass.TwoHandedAxe:
				case WeaponClass.TwoHandedMace:
					poseName = "troop_infantry_sword2h";
					goto IL_0250;
				case WeaponClass.Mace:
				case WeaponClass.Pick:
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
				case WeaponClass.SlingStone:
				case WeaponClass.Cartridge:
					goto IL_0250;
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
					poseName = "troop_spear";
					goto IL_0250;
				case WeaponClass.LowGripPolearm:
					break;
				case WeaponClass.Bow:
					poseName = "troop_bow";
					goto IL_0250;
				case WeaponClass.Crossbow:
					poseName = "troop_crossbow";
					goto IL_0250;
				default:
					if (weaponClass != WeaponClass.Javelin)
					{
						goto IL_0250;
					}
					break;
				}
				poseName = "troop_spear";
			}
			IL_0250:
			if (!equipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				if (num2 != -1)
				{
					HorseComponent horseComponent = equipment[EquipmentIndex.ArmorItemEndSlot].Item.HorseComponent;
					bool flag;
					if (horseComponent == null)
					{
						flag = false;
					}
					else
					{
						Monster monster = horseComponent.Monster;
						int? num4 = ((monster != null) ? new int?(monster.FamilyType) : null);
						int num5 = 2;
						flag = (num4.GetValueOrDefault() == num5) & (num4 != null);
					}
					bool flag2 = flag;
					ItemObject.ItemTypeEnum type = equipment[num2].Item.Type;
					if (type != ItemObject.ItemTypeEnum.OneHandedWeapon)
					{
						if (type == ItemObject.ItemTypeEnum.Bow)
						{
							poseName = "troop_cavalry_archer";
						}
						else
						{
							poseName = "troop_cavalry_lance";
						}
					}
					else if (num3 == -1)
					{
						poseName = "troop_cavalry_sword";
					}
					else if (equipment[num3].Item.PrimaryWeapon.IsShield)
					{
						poseName = "troop_cavalry_sword";
					}
					if (flag2)
					{
						poseName = "camel_" + poseName;
					}
				}
				hasHorse = true;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		private GameEntity FillEntityWithPose(CharacterCode characterCode, GameEntity poseEntity, Scene scene)
		{
			if (characterCode.IsEmpty)
			{
				Debug.FailedAssert("Trying to fill entity with empty character code", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\Thumbnails\\CharacterThumbnailCache.cs", "FillEntityWithPose", 306);
				return poseEntity;
			}
			if (string.IsNullOrEmpty(characterCode.EquipmentCode))
			{
				Debug.FailedAssert("Trying to fill entity with invalid equipment code", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\Thumbnails\\CharacterThumbnailCache.cs", "FillEntityWithPose", 312);
				return poseEntity;
			}
			if (FaceGen.GetBaseMonsterFromRace(characterCode.Race) == null)
			{
				Debug.FailedAssert("There are no monster data for the race: " + characterCode.Race, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\Thumbnails\\CharacterThumbnailCache.cs", "FillEntityWithPose", 319);
				return poseEntity;
			}
			if (poseEntity != null)
			{
				string text;
				bool flag;
				this.GetPoseParamsFromCharacterCode(characterCode, out text, out flag);
				CharacterSpawner characterSpawner = poseEntity.GetScriptComponents<CharacterSpawner>().First<CharacterSpawner>();
				characterSpawner.SetCreateFaceImmediately(false);
				characterSpawner.InitWithCharacter(characterCode, false);
			}
			return poseEntity;
		}

		// Token: 0x0400013E RID: 318
		private int _characterCount;

		// Token: 0x0400013F RID: 319
		private int _characterTableauGPUAllocationIndex;
	}
}
