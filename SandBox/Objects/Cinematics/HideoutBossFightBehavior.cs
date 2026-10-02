using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.Objects.Cinematics
{
	// Token: 0x02000041 RID: 65
	public class HideoutBossFightBehavior : ScriptComponentBehavior
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000E73D File Offset: 0x0000C93D
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000E72D File Offset: 0x0000C92D
		public int PerturbSeed
		{
			get
			{
				return this._perturbSeed;
			}
			private set
			{
				this._perturbSeed = value;
				this.ReSeedPerturbRng(0);
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000E748 File Offset: 0x0000C948
		public void GetPlayerFrames(out MatrixFrame initialFrame, out MatrixFrame targetFrame, float perturbAmount = 0f)
		{
			this.ReSeedPerturbRng(0);
			Vec3 vec;
			this.ComputePerturbedSpawnOffset(perturbAmount, out vec);
			float num = 3.1415927f;
			float innerRadius = this.InnerRadius;
			Vec3 vec2 = vec - this.WalkDistance * Vec3.Forward;
			this.ComputeSpawnWorldFrame(num, innerRadius, in vec2, out initialFrame);
			this.ComputeSpawnWorldFrame(3.1415927f, this.InnerRadius, in vec, out targetFrame);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
		public void GetBossFrames(out MatrixFrame initialFrame, out MatrixFrame targetFrame, float perturbAmount = 0f)
		{
			this.ReSeedPerturbRng(1);
			Vec3 vec;
			this.ComputePerturbedSpawnOffset(perturbAmount, out vec);
			float num = 0f;
			float innerRadius = this.InnerRadius;
			Vec3 vec2 = vec + this.WalkDistance * Vec3.Forward;
			this.ComputeSpawnWorldFrame(num, innerRadius, in vec2, out initialFrame);
			this.ComputeSpawnWorldFrame(0f, this.InnerRadius, in vec, out targetFrame);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000E800 File Offset: 0x0000CA00
		public void GetAllyFrames(out List<MatrixFrame> initialFrames, out List<MatrixFrame> targetFrames, int agentCount = 10, float agentOffsetAngle = 0.15707964f, float perturbAmount = 0f)
		{
			this.ReSeedPerturbRng(2);
			initialFrames = this.ComputeSpawnWorldFrames(agentCount, this.OuterRadius, -this.WalkDistance * Vec3.Forward, 3.1415927f, agentOffsetAngle, perturbAmount).ToList<MatrixFrame>();
			this.ReSeedPerturbRng(2);
			targetFrames = this.ComputeSpawnWorldFrames(agentCount, this.OuterRadius, Vec3.Zero, 3.1415927f, agentOffsetAngle, perturbAmount).ToList<MatrixFrame>();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E86C File Offset: 0x0000CA6C
		public void GetBanditFrames(out List<MatrixFrame> initialFrames, out List<MatrixFrame> targetFrames, int agentCount = 10, float agentOffsetAngle = 0.15707964f, float perturbAmount = 0f)
		{
			this.ReSeedPerturbRng(3);
			initialFrames = this.ComputeSpawnWorldFrames(agentCount, this.OuterRadius, this.WalkDistance * Vec3.Forward, 0f, agentOffsetAngle, perturbAmount).ToList<MatrixFrame>();
			this.ReSeedPerturbRng(3);
			targetFrames = this.ComputeSpawnWorldFrames(agentCount, this.OuterRadius, Vec3.Zero, 0f, agentOffsetAngle, perturbAmount).ToList<MatrixFrame>();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
		public void GetAlliesInitialFrame(out MatrixFrame frame)
		{
			float num = 3.1415927f;
			float outerRadius = this.OuterRadius;
			Vec3 vec = -this.WalkDistance * Vec3.Forward;
			this.ComputeSpawnWorldFrame(num, outerRadius, in vec, out frame);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000E90C File Offset: 0x0000CB0C
		public void GetBanditsInitialFrame(out MatrixFrame frame)
		{
			float num = 0f;
			float outerRadius = this.OuterRadius;
			Vec3 vec = this.WalkDistance * Vec3.Forward;
			this.ComputeSpawnWorldFrame(num, outerRadius, in vec, out frame);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000E940 File Offset: 0x0000CB40
		public bool IsWorldPointInsideCameraVolume(in Vec3 worldPoint)
		{
			Vec3 vec = base.GameEntity.GetGlobalFrame().TransformToLocal(in worldPoint);
			return this.IsLocalPointInsideCameraVolume(in vec);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E970 File Offset: 0x0000CB70
		public bool ClampWorldPointToCameraVolume(in Vec3 worldPoint, out Vec3 clampedPoint)
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 vec = globalFrame.TransformToLocal(in worldPoint);
			bool flag = this.IsLocalPointInsideCameraVolume(in vec);
			if (flag)
			{
				clampedPoint = worldPoint;
				return flag;
			}
			float num = 5f;
			float num2 = this.OuterRadius + this.WalkDistance;
			vec.x = MathF.Clamp(vec.x, -num, num);
			vec.y = MathF.Clamp(vec.y, -num2, num2);
			vec.z = MathF.Clamp(vec.z, 0f, 5f);
			clampedPoint = globalFrame.TransformToParent(in vec);
			return flag;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000EA1C File Offset: 0x0000CC1C
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "ShowPreview")
			{
				this.UpdatePreview();
				this.TogglePreviewVisibility(this.ShowPreview);
				return;
			}
			if (this.ShowPreview && (variableName == "InnerRadius" || variableName == "OuterRadius" || variableName == "WalkDistance"))
			{
				this.UpdatePreview();
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000EA88 File Offset: 0x0000CC88
		protected override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (this.ShowPreview)
			{
				MatrixFrame frame = base.GameEntity.GetFrame();
				if (!this._previousEntityFrame.origin.NearlyEquals(in frame.origin, 1E-05f) || !this._previousEntityFrame.rotation.NearlyEquals(in frame.rotation, 1E-05f))
				{
					this._previousEntityFrame = frame;
					this.UpdatePreview();
				}
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000EB02 File Offset: 0x0000CD02
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this.RemovePreview();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000EB14 File Offset: 0x0000CD14
		private void UpdatePreview()
		{
			if (this._previewEntities == null)
			{
				this.GeneratePreview();
			}
			GameEntity previewEntities = this._previewEntities;
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			previewEntities.SetGlobalFrame(in matrixFrame, true);
			MatrixFrame identity = MatrixFrame.Identity;
			MatrixFrame identity2 = MatrixFrame.Identity;
			this.GetPlayerFrames(out identity, out identity2, 0.25f);
			this._previewPlayer.InitialEntity.SetGlobalFrame(in identity, true);
			this._previewPlayer.TargetEntity.SetGlobalFrame(in identity2, true);
			List<MatrixFrame> list;
			List<MatrixFrame> list2;
			this.GetAllyFrames(out list, out list2, 10, 0.15707964f, 0.25f);
			int num = 0;
			foreach (HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo hideoutBossFightPreviewEntityInfo in this._previewAllies)
			{
				GameEntity initialEntity = hideoutBossFightPreviewEntityInfo.InitialEntity;
				matrixFrame = list[num];
				initialEntity.SetGlobalFrame(in matrixFrame, true);
				GameEntity targetEntity = hideoutBossFightPreviewEntityInfo.TargetEntity;
				matrixFrame = list2[num];
				targetEntity.SetGlobalFrame(in matrixFrame, true);
				num++;
			}
			this.GetBossFrames(out identity, out identity2, 0.25f);
			this._previewBoss.InitialEntity.SetGlobalFrame(in identity, true);
			this._previewBoss.TargetEntity.SetGlobalFrame(in identity2, true);
			List<MatrixFrame> list3;
			List<MatrixFrame> list4;
			this.GetBanditFrames(out list3, out list4, 10, 0.15707964f, 0.25f);
			int num2 = 0;
			foreach (HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo hideoutBossFightPreviewEntityInfo2 in this._previewBandits)
			{
				GameEntity initialEntity2 = hideoutBossFightPreviewEntityInfo2.InitialEntity;
				matrixFrame = list3[num2];
				initialEntity2.SetGlobalFrame(in matrixFrame, true);
				GameEntity targetEntity2 = hideoutBossFightPreviewEntityInfo2.TargetEntity;
				matrixFrame = list4[num2];
				targetEntity2.SetGlobalFrame(in matrixFrame, true);
				num2++;
			}
			MatrixFrame frame = this._previewCamera.GetFrame();
			Vec3 scaleVector = frame.rotation.GetScaleVector();
			Vec3 vec = Vec3.Forward * (this.OuterRadius + this.WalkDistance) + Vec3.Side * 5f + Vec3.Up * 5f;
			Vec3 vec2 = new Vec3(vec.x / scaleVector.x, vec.y / scaleVector.y, vec.z / scaleVector.z, -1f);
			frame.rotation.ApplyScaleLocal(in vec2);
			this._previewCamera.SetFrame(ref frame, true);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000ED90 File Offset: 0x0000CF90
		private void GeneratePreview()
		{
			Scene scene = base.GameEntity.Scene;
			this._previewEntities = TaleWorlds.Engine.GameEntity.CreateEmpty(scene, false, true, true);
			this._previewEntities.EntityFlags |= EntityFlags.DontSaveToScene;
			MatrixFrame identity = MatrixFrame.Identity;
			this._previewEntities.SetFrame(ref identity, true);
			MatrixFrame globalFrame = this._previewEntities.GetGlobalFrame();
			GameEntity gameEntity = TaleWorlds.Engine.GameEntity.Instantiate(scene, "hideout_boss_fight_preview_boss", globalFrame, true);
			this._previewEntities.AddChild(gameEntity, false);
			GameEntity gameEntity2;
			GameEntity gameEntity3;
			this.ReadPrefabEntity(gameEntity, out gameEntity2, out gameEntity3);
			this._previewBoss = new HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo(gameEntity, gameEntity2, gameEntity3);
			GameEntity gameEntity4 = TaleWorlds.Engine.GameEntity.Instantiate(scene, "hideout_boss_fight_preview_player", globalFrame, true);
			this._previewEntities.AddChild(gameEntity4, false);
			GameEntity gameEntity5;
			GameEntity gameEntity6;
			this.ReadPrefabEntity(gameEntity4, out gameEntity5, out gameEntity6);
			this._previewPlayer = new HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo(gameEntity4, gameEntity5, gameEntity6);
			for (int i = 0; i < 10; i++)
			{
				GameEntity gameEntity7 = TaleWorlds.Engine.GameEntity.Instantiate(scene, "hideout_boss_fight_preview_ally", globalFrame, true);
				this._previewEntities.AddChild(gameEntity7, false);
				GameEntity gameEntity8;
				GameEntity gameEntity9;
				this.ReadPrefabEntity(gameEntity7, out gameEntity8, out gameEntity9);
				this._previewAllies.Add(new HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo(gameEntity7, gameEntity8, gameEntity9));
			}
			for (int j = 0; j < 10; j++)
			{
				GameEntity gameEntity10 = TaleWorlds.Engine.GameEntity.Instantiate(scene, "hideout_boss_fight_preview_bandit", globalFrame, true);
				this._previewEntities.AddChild(gameEntity10, false);
				GameEntity gameEntity11;
				GameEntity gameEntity12;
				this.ReadPrefabEntity(gameEntity10, out gameEntity11, out gameEntity12);
				this._previewBandits.Add(new HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo(gameEntity10, gameEntity11, gameEntity12));
			}
			this._previewCamera = TaleWorlds.Engine.GameEntity.Instantiate(scene, "hideout_boss_fight_camera_preview", globalFrame, true);
			this._previewEntities.AddChild(this._previewCamera, false);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000EF2B File Offset: 0x0000D12B
		private void RemovePreview()
		{
			if (this._previewEntities != null)
			{
				this._previewEntities.Remove(90);
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000EF48 File Offset: 0x0000D148
		private void TogglePreviewVisibility(bool value)
		{
			if (this._previewEntities != null)
			{
				this._previewEntities.SetVisibilityExcludeParents(value);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000EF64 File Offset: 0x0000D164
		private void ReadPrefabEntity(GameEntity entity, out GameEntity initialEntity, out GameEntity targetEntity)
		{
			GameEntity firstChildEntityWithTag = entity.GetFirstChildEntityWithTag("initial_frame");
			if (firstChildEntityWithTag == null)
			{
				Debug.FailedAssert("Prefab entity " + entity.Name + " is not a spawn prefab with an initial frame entity", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Objects\\Cinematics\\HideoutBossFightBehavior.cs", "ReadPrefabEntity", 389);
			}
			GameEntity firstChildEntityWithTag2 = entity.GetFirstChildEntityWithTag("target_frame");
			if (firstChildEntityWithTag2 == null)
			{
				Debug.FailedAssert("Prefab entity " + entity.Name + " is not a spawn prefab with an target frame entity", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Objects\\Cinematics\\HideoutBossFightBehavior.cs", "ReadPrefabEntity", 395);
			}
			initialEntity = firstChildEntityWithTag;
			targetEntity = firstChildEntityWithTag2;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		private void FindRadialPlacementFrame(float angle, float radius, out MatrixFrame frame)
		{
			float num;
			float num2;
			MathF.SinCos(angle, out num, out num2);
			Vec3 vec = num2 * Vec3.Forward + num * Vec3.Side;
			Vec3 vec2 = radius * vec;
			Vec3 vec3 = ((num2 > 0f) ? (-1f) : 1f) * Vec3.Forward;
			Mat3 mat = Mat3.CreateMat3WithForward(in vec3);
			frame = new MatrixFrame(in mat, in vec2);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000F068 File Offset: 0x0000D268
		private void SnapOnClosestCollider(ref MatrixFrame frameWs)
		{
			Scene scene = base.GameEntity.Scene;
			Vec3 origin = frameWs.origin;
			origin.z += 5f;
			Vec3 vec = origin;
			float num = 500f;
			vec.z -= num;
			float num2;
			if (scene.RayCastForClosestEntityOrTerrain(origin, vec, out num2, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags))
			{
				frameWs.origin.z = origin.z - num2;
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000F0D7 File Offset: 0x0000D2D7
		private void ReSeedPerturbRng(int seedOffset = 0)
		{
			this._perturbRng = new Random(this._perturbSeed + seedOffset);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000F0EC File Offset: 0x0000D2EC
		private void ComputeSpawnWorldFrame(float localAngle, float localRadius, in Vec3 localOffset, out MatrixFrame worldFrame)
		{
			MatrixFrame matrixFrame;
			this.FindRadialPlacementFrame(localAngle, localRadius, out matrixFrame);
			matrixFrame.origin += localOffset;
			worldFrame = base.GameEntity.GetGlobalFrame().TransformToParent(in matrixFrame);
			this.SnapOnClosestCollider(ref worldFrame);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000F147 File Offset: 0x0000D347
		private IEnumerable<MatrixFrame> ComputeSpawnWorldFrames(int spawnCount, float localRadius, Vec3 localOffset, float localBaseAngle, float localOffsetAngle, float localPerturbAmount = 0f)
		{
			float[] localPlacementAngles = new float[]
			{
				localBaseAngle + localOffsetAngle / 2f,
				localBaseAngle - localOffsetAngle / 2f
			};
			int angleIndex = 0;
			MatrixFrame identity = MatrixFrame.Identity;
			Vec3 zero = Vec3.Zero;
			int num2;
			for (int i = 0; i < spawnCount; i = num2 + 1)
			{
				this.ComputePerturbedSpawnOffset(localPerturbAmount, out zero);
				float num = localPlacementAngles[angleIndex];
				Vec3 vec = zero + localOffset;
				this.ComputeSpawnWorldFrame(num, localRadius, in vec, out identity);
				yield return identity;
				localPlacementAngles[angleIndex] += (float)((angleIndex == 0) ? 1 : (-1)) * localOffsetAngle;
				angleIndex = (angleIndex + 1) % 2;
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000F184 File Offset: 0x0000D384
		private void ComputePerturbedSpawnOffset(float perturbAmount, out Vec3 perturbVector)
		{
			perturbVector = Vec3.Zero;
			perturbAmount = MathF.Abs(perturbAmount);
			if (perturbAmount > 1E-05f)
			{
				float num;
				float num2;
				MathF.SinCos(6.2831855f * this._perturbRng.NextFloat(), out num, out num2);
				perturbVector.x = perturbAmount * num2;
				perturbVector.y = perturbAmount * num;
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		private bool IsLocalPointInsideCameraVolume(in Vec3 localPoint)
		{
			float num = 5f;
			float num2 = this.OuterRadius + this.WalkDistance;
			return localPoint.x >= -num && localPoint.x <= num && localPoint.y >= -num2 && localPoint.y <= num2 && localPoint.z >= 0f && localPoint.z <= 5f;
		}

		// Token: 0x040000FD RID: 253
		private const int PreviewPerturbSeed = 0;

		// Token: 0x040000FE RID: 254
		private const float PreviewPerturbAmount = 0.25f;

		// Token: 0x040000FF RID: 255
		private const int PreviewTroopCount = 10;

		// Token: 0x04000100 RID: 256
		private const float PreviewPlacementAngle = 0.15707964f;

		// Token: 0x04000101 RID: 257
		private const string InitialFrameTag = "initial_frame";

		// Token: 0x04000102 RID: 258
		private const string TargetFrameTag = "target_frame";

		// Token: 0x04000103 RID: 259
		private const string BossPreviewPrefab = "hideout_boss_fight_preview_boss";

		// Token: 0x04000104 RID: 260
		private const string PlayerPreviewPrefab = "hideout_boss_fight_preview_player";

		// Token: 0x04000105 RID: 261
		private const string AllyPreviewPrefab = "hideout_boss_fight_preview_ally";

		// Token: 0x04000106 RID: 262
		private const string BanditPreviewPrefab = "hideout_boss_fight_preview_bandit";

		// Token: 0x04000107 RID: 263
		private const string PreviewCameraPrefab = "hideout_boss_fight_camera_preview";

		// Token: 0x04000108 RID: 264
		public const float MaxCameraHeight = 5f;

		// Token: 0x04000109 RID: 265
		public const float MaxCameraWidth = 10f;

		// Token: 0x0400010A RID: 266
		public float InnerRadius = 2.5f;

		// Token: 0x0400010B RID: 267
		public float OuterRadius = 6f;

		// Token: 0x0400010C RID: 268
		public float WalkDistance = 3f;

		// Token: 0x0400010D RID: 269
		public bool ShowPreview;

		// Token: 0x0400010E RID: 270
		private int _perturbSeed;

		// Token: 0x0400010F RID: 271
		private Random _perturbRng = new Random(0);

		// Token: 0x04000110 RID: 272
		private MatrixFrame _previousEntityFrame = MatrixFrame.Identity;

		// Token: 0x04000111 RID: 273
		private GameEntity _previewEntities;

		// Token: 0x04000112 RID: 274
		private List<HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo> _previewAllies = new List<HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo>();

		// Token: 0x04000113 RID: 275
		private List<HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo> _previewBandits = new List<HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo>();

		// Token: 0x04000114 RID: 276
		private HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo _previewBoss = HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo.Invalid;

		// Token: 0x04000115 RID: 277
		private HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo _previewPlayer = HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo.Invalid;

		// Token: 0x04000116 RID: 278
		private GameEntity _previewCamera;

		// Token: 0x02000157 RID: 343
		private readonly struct HideoutBossFightPreviewEntityInfo
		{
			// Token: 0x17000139 RID: 313
			// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0006614D File Offset: 0x0006434D
			public static HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo Invalid
			{
				get
				{
					return new HideoutBossFightBehavior.HideoutBossFightPreviewEntityInfo(null, null, null);
				}
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00066157 File Offset: 0x00064357
			public bool IsValid
			{
				get
				{
					return this.BaseEntity == null;
				}
			}

			// Token: 0x06000E70 RID: 3696 RVA: 0x00066165 File Offset: 0x00064365
			public HideoutBossFightPreviewEntityInfo(GameEntity baseEntity, GameEntity initialEntity, GameEntity targetEntity)
			{
				this.BaseEntity = baseEntity;
				this.InitialEntity = initialEntity;
				this.TargetEntity = targetEntity;
			}

			// Token: 0x04000698 RID: 1688
			public readonly GameEntity BaseEntity;

			// Token: 0x04000699 RID: 1689
			public readonly GameEntity InitialEntity;

			// Token: 0x0400069A RID: 1690
			public readonly GameEntity TargetEntity;
		}

		// Token: 0x02000158 RID: 344
		private enum HideoutSeedPerturbOffset
		{
			// Token: 0x0400069C RID: 1692
			Player,
			// Token: 0x0400069D RID: 1693
			Boss,
			// Token: 0x0400069E RID: 1694
			Ally,
			// Token: 0x0400069F RID: 1695
			Bandit
		}
	}
}
