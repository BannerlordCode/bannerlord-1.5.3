using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x0200005E RID: 94
	public class PopupSceneCameraPath : ScriptComponentBehavior
	{
		// Token: 0x06000390 RID: 912 RVA: 0x0001AC99 File Offset: 0x00018E99
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001ACAD File Offset: 0x00018EAD
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001ACB8 File Offset: 0x00018EB8
		public void Initialize()
		{
			if (this.SkeletonName != "" && (base.GameEntity.Skeleton == null || base.GameEntity.Skeleton.GetName() != this.SkeletonName))
			{
				base.GameEntity.CreateSimpleSkeleton(this.SkeletonName);
			}
			else if (this.SkeletonName == "" && base.GameEntity.Skeleton != null)
			{
				base.GameEntity.RemoveSkeleton();
			}
			if (this.LookAtEntity != "")
			{
				this._lookAtEntity = base.GameEntity.Scene.GetFirstEntityWithName(this.LookAtEntity);
			}
			this._transitionState[0].path = ((this.InitialPath == "") ? null : base.GameEntity.Scene.GetPathWithName(this.InitialPath));
			this._transitionState[0].animationName = this.InitialAnimationClip;
			this._transitionState[0].startTime = this.InitialPathStartTime;
			this._transitionState[0].duration = this.InitialPathDuration;
			this._transitionState[0].interpolation = this.InitialInterpolation;
			this._transitionState[0].fadeCamera = this.InitialFadeOut;
			this._transitionState[0].soundEvent = this.InitialSound;
			this._transitionState[1].path = ((this.PositivePath == "") ? null : base.GameEntity.Scene.GetPathWithName(this.PositivePath));
			this._transitionState[1].animationName = this.PositiveAnimationClip;
			this._transitionState[1].startTime = this.PositivePathStartTime;
			this._transitionState[1].duration = this.PositivePathDuration;
			this._transitionState[1].interpolation = this.PositiveInterpolation;
			this._transitionState[1].fadeCamera = this.PositiveFadeOut;
			this._transitionState[1].soundEvent = this.PositiveSound;
			this._transitionState[2].path = ((this.NegativePath == "") ? null : base.GameEntity.Scene.GetPathWithName(this.NegativePath));
			this._transitionState[2].animationName = this.NegativeAnimationClip;
			this._transitionState[2].startTime = this.NegativePathStartTime;
			this._transitionState[2].duration = this.NegativePathDuration;
			this._transitionState[2].interpolation = this.NegativeInterpolation;
			this._transitionState[2].fadeCamera = this.NegativeFadeOut;
			this._transitionState[2].soundEvent = this.NegativeSound;
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = base.GameEntity.GlobalPosition;
			SoundManager.SetListenerFrame(identity);
			List<GameEntity> list = new List<GameEntity>();
			base.Scene.GetAllEntitiesWithScriptComponent<PopupSceneSkeletonAnimationScript>(ref list);
			list.ForEach(delegate(GameEntity e)
			{
				this._skeletonAnims.Add(e.GetFirstScriptOfType<PopupSceneSkeletonAnimationScript>());
			});
			this._skeletonAnims.ForEach(delegate(PopupSceneSkeletonAnimationScript s)
			{
				s.Initialize();
			});
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001B054 File Offset: 0x00019254
		private void SetState(int state)
		{
			if (base.GameEntity.Skeleton != null && !string.IsNullOrEmpty(this._transitionState[state].animationName))
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._transitionState[state].animationName, 0, 1f, -1f, 0f);
			}
			this._currentState = state;
			this._transitionState[state].alpha = 0f;
			if (this._transitionState[state].path != null)
			{
				this._transitionState[state].totalDistance = this._transitionState[state].path.GetTotalLength();
			}
			if (this._transitionState[state].soundEvent != "")
			{
				SoundEvent activeSoundEvent = this._activeSoundEvent;
				if (activeSoundEvent != null)
				{
					activeSoundEvent.Stop();
				}
				this._activeSoundEvent = SoundEvent.CreateEventFromString(this._transitionState[state].soundEvent, null);
				if (this._isReady)
				{
					SoundEvent activeSoundEvent2 = this._activeSoundEvent;
					if (activeSoundEvent2 != null)
					{
						activeSoundEvent2.Play();
					}
				}
			}
			this.UpdateCamera(0f, ref this._transitionState[state]);
			this._skeletonAnims.ForEach(delegate(PopupSceneSkeletonAnimationScript s)
			{
				s.SetState(state);
			});
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001B1F2 File Offset: 0x000193F2
		public void SetInitialState()
		{
			this.SetState(0);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001B1FB File Offset: 0x000193FB
		public void SetPositiveState()
		{
			this.SetState(1);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001B204 File Offset: 0x00019404
		public void SetNegativeState()
		{
			this.SetState(2);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001B20D File Offset: 0x0001940D
		public void SetIsReady(bool isReady)
		{
			if (this._isReady != isReady)
			{
				if (isReady)
				{
					SoundEvent activeSoundEvent = this._activeSoundEvent;
					if (activeSoundEvent != null && !activeSoundEvent.IsPlaying())
					{
						this._activeSoundEvent.Play();
					}
				}
				this._isReady = isReady;
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001B245 File Offset: 0x00019445
		public float GetCameraFade()
		{
			return this._cameraFadeValue;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001B250 File Offset: 0x00019450
		public void Destroy()
		{
			SoundEvent activeSoundEvent = this._activeSoundEvent;
			if (activeSoundEvent != null)
			{
				activeSoundEvent.Stop();
			}
			for (int i = 0; i < 3; i++)
			{
				this._transitionState[i].path = null;
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001B28C File Offset: 0x0001948C
		private float InQuadBlend(float t)
		{
			return t * t;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001B291 File Offset: 0x00019491
		private float OutQuadBlend(float t)
		{
			return t * (2f - t);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001B29C File Offset: 0x0001949C
		private float InOutQuadBlend(float t)
		{
			if (t >= 0.5f)
			{
				return -1f + (4f - 2f * t) * t;
			}
			return 2f * t * t;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001B2C8 File Offset: 0x000194C8
		private MatrixFrame CreateLookAt(Vec3 position, Vec3 target, Vec3 upVector)
		{
			Vec3 vec = target - position;
			vec.Normalize();
			Vec3 vec2 = Vec3.CrossProduct(vec, upVector);
			vec2.Normalize();
			Vec3 vec3 = Vec3.CrossProduct(vec2, vec);
			float x = vec2.x;
			float y = vec2.y;
			float z = vec2.z;
			float num = 0f;
			float x2 = vec3.x;
			float y2 = vec3.y;
			float z2 = vec3.z;
			float num2 = 0f;
			float num3 = -vec.x;
			float num4 = -vec.y;
			float num5 = -vec.z;
			float num6 = 0f;
			float x3 = position.x;
			float y3 = position.y;
			float z3 = position.z;
			float num7 = 1f;
			return new MatrixFrame(x, y, z, num, x2, y2, z2, num2, num3, num4, num5, num6, x3, y3, z3, num7);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001B39B File Offset: 0x0001959B
		private float Clamp(float x, float a, float b)
		{
			if (x < a)
			{
				return a;
			}
			if (x <= b)
			{
				return x;
			}
			return b;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001B3AA File Offset: 0x000195AA
		private float SmoothStep(float edge0, float edge1, float x)
		{
			x = this.Clamp((x - edge0) / (edge1 - edge0), 0f, 1f);
			return x * x * (3f - 2f * x);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001B3D8 File Offset: 0x000195D8
		private void UpdateCamera(float dt, ref PopupSceneCameraPath.PathAnimationState state)
		{
			GameEntity gameEntity = base.GameEntity.Scene.FindEntityWithTag("camera_instance");
			if (gameEntity == null)
			{
				return;
			}
			state.alpha += dt;
			if (state.alpha > state.startTime + state.duration)
			{
				state.alpha = state.startTime + state.duration;
			}
			float num = this.SmoothStep(state.startTime, state.startTime + state.duration, state.alpha);
			switch (state.interpolation)
			{
			case PopupSceneCameraPath.InterpolationType.EaseIn:
				num = this.InQuadBlend(num);
				break;
			case PopupSceneCameraPath.InterpolationType.EaseOut:
				num = this.OutQuadBlend(num);
				break;
			case PopupSceneCameraPath.InterpolationType.EaseInOut:
				num = this.InOutQuadBlend(num);
				break;
			}
			state.easedAlpha = num;
			if (state.fadeCamera)
			{
				this._cameraFadeValue = num;
			}
			if (base.GameEntity.Skeleton != null && !string.IsNullOrEmpty(state.animationName))
			{
				MatrixFrame matrixFrame = base.GameEntity.Skeleton.GetBoneEntitialFrame((sbyte)this.BoneIndex);
				matrixFrame = this._localFrameIdentity.TransformToParent(in matrixFrame);
				MatrixFrame matrixFrame2 = default(MatrixFrame);
				matrixFrame2.rotation = matrixFrame.rotation;
				matrixFrame2.rotation.u = -matrixFrame.rotation.s;
				matrixFrame2.rotation.f = -matrixFrame.rotation.u;
				matrixFrame2.rotation.s = matrixFrame.rotation.f;
				matrixFrame2.origin = matrixFrame.origin + this.AttachmentOffset;
				gameEntity.SetFrame(ref matrixFrame2, true);
				SoundManager.SetListenerFrame(matrixFrame2);
				return;
			}
			if (state.path != null)
			{
				float num2 = num * state.totalDistance;
				Vec3 origin = state.path.GetFrameForDistance(num2).origin;
				MatrixFrame matrixFrame3 = gameEntity.GetGlobalFrame();
				if (this._lookAtEntity != null)
				{
					matrixFrame3 = this.CreateLookAt(origin, this._lookAtEntity.GetGlobalFrame().origin, Vec3.Up);
				}
				else
				{
					matrixFrame3.origin = origin;
				}
				gameEntity.SetGlobalFrame(in matrixFrame3, true);
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001B607 File Offset: 0x00019807
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001B611 File Offset: 0x00019811
		protected override void OnTick(float dt)
		{
			this.UpdateCamera(dt, ref this._transitionState[this._currentState]);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001B62B File Offset: 0x0001982B
		protected override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.OnTick(dt);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001B63C File Offset: 0x0001983C
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			this.Initialize();
			if (variableName == "TestInitial")
			{
				this.SetState(0);
			}
			if (variableName == "TestPositive")
			{
				this.SetState(1);
			}
			if (variableName == "TestNegative")
			{
				this.SetState(2);
			}
		}

		// Token: 0x040001E9 RID: 489
		public string LookAtEntity = "";

		// Token: 0x040001EA RID: 490
		public string SkeletonName = "";

		// Token: 0x040001EB RID: 491
		public int BoneIndex;

		// Token: 0x040001EC RID: 492
		public Vec3 AttachmentOffset = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x040001ED RID: 493
		public string InitialPath = "";

		// Token: 0x040001EE RID: 494
		public string InitialAnimationClip = "";

		// Token: 0x040001EF RID: 495
		public string InitialSound = "event:/mission/siege/siegetower/doorland";

		// Token: 0x040001F0 RID: 496
		public float InitialPathStartTime;

		// Token: 0x040001F1 RID: 497
		public float InitialPathDuration = 1f;

		// Token: 0x040001F2 RID: 498
		public PopupSceneCameraPath.InterpolationType InitialInterpolation;

		// Token: 0x040001F3 RID: 499
		public bool InitialFadeOut;

		// Token: 0x040001F4 RID: 500
		public string PositivePath = "";

		// Token: 0x040001F5 RID: 501
		public string PositiveAnimationClip = "";

		// Token: 0x040001F6 RID: 502
		public string PositiveSound = "";

		// Token: 0x040001F7 RID: 503
		public float PositivePathStartTime;

		// Token: 0x040001F8 RID: 504
		public float PositivePathDuration = 1f;

		// Token: 0x040001F9 RID: 505
		public PopupSceneCameraPath.InterpolationType PositiveInterpolation;

		// Token: 0x040001FA RID: 506
		public bool PositiveFadeOut;

		// Token: 0x040001FB RID: 507
		public string NegativePath = "";

		// Token: 0x040001FC RID: 508
		public string NegativeAnimationClip = "";

		// Token: 0x040001FD RID: 509
		public string NegativeSound = "";

		// Token: 0x040001FE RID: 510
		public float NegativePathStartTime;

		// Token: 0x040001FF RID: 511
		public float NegativePathDuration = 1f;

		// Token: 0x04000200 RID: 512
		public PopupSceneCameraPath.InterpolationType NegativeInterpolation;

		// Token: 0x04000201 RID: 513
		public bool NegativeFadeOut;

		// Token: 0x04000202 RID: 514
		private bool _isReady;

		// Token: 0x04000203 RID: 515
		public SimpleButton TestInitial;

		// Token: 0x04000204 RID: 516
		public SimpleButton TestPositive;

		// Token: 0x04000205 RID: 517
		public SimpleButton TestNegative;

		// Token: 0x04000206 RID: 518
		private MatrixFrame _localFrameIdentity = MatrixFrame.Identity;

		// Token: 0x04000207 RID: 519
		private GameEntity _lookAtEntity;

		// Token: 0x04000208 RID: 520
		private int _currentState;

		// Token: 0x04000209 RID: 521
		private float _cameraFadeValue;

		// Token: 0x0400020A RID: 522
		private List<PopupSceneSkeletonAnimationScript> _skeletonAnims = new List<PopupSceneSkeletonAnimationScript>();

		// Token: 0x0400020B RID: 523
		private SoundEvent _activeSoundEvent;

		// Token: 0x0400020C RID: 524
		private readonly PopupSceneCameraPath.PathAnimationState[] _transitionState = new PopupSceneCameraPath.PathAnimationState[3];

		// Token: 0x020000D2 RID: 210
		public enum InterpolationType
		{
			// Token: 0x040003BB RID: 955
			Linear,
			// Token: 0x040003BC RID: 956
			EaseIn,
			// Token: 0x040003BD RID: 957
			EaseOut,
			// Token: 0x040003BE RID: 958
			EaseInOut
		}

		// Token: 0x020000D3 RID: 211
		public struct PathAnimationState
		{
			// Token: 0x040003BF RID: 959
			public Path path;

			// Token: 0x040003C0 RID: 960
			public string animationName;

			// Token: 0x040003C1 RID: 961
			public float totalDistance;

			// Token: 0x040003C2 RID: 962
			public float startTime;

			// Token: 0x040003C3 RID: 963
			public float duration;

			// Token: 0x040003C4 RID: 964
			public float alpha;

			// Token: 0x040003C5 RID: 965
			public float easedAlpha;

			// Token: 0x040003C6 RID: 966
			public bool fadeCamera;

			// Token: 0x040003C7 RID: 967
			public PopupSceneCameraPath.InterpolationType interpolation;

			// Token: 0x040003C8 RID: 968
			public string soundEvent;
		}
	}
}
