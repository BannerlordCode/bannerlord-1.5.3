using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000388 RID: 904
	public class VertexAnimator : SynchedMissionObject
	{
		// Token: 0x06003459 RID: 13401 RVA: 0x000D7C4C File Offset: 0x000D5E4C
		public VertexAnimator()
		{
			this.Speed = 20f;
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x000D7C6A File Offset: 0x000D5E6A
		private void SetIsPlaying(bool value)
		{
			if (this._isPlaying != value)
			{
				this._isPlaying = value;
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000D7C88 File Offset: 0x000D5E88
		protected internal override void OnInit()
		{
			base.OnInit();
			this.RefreshEditDataUsers();
			this.SetIsPlaying(true);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000D7CA9 File Offset: 0x000D5EA9
		protected internal override void OnEditorInit()
		{
			this.OnInit();
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x000D7CB1 File Offset: 0x000D5EB1
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (this._isPlaying)
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000D7CCC File Offset: 0x000D5ECC
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._isPlaying)
			{
				if (this._curAnimTime < (float)this.BeginKey)
				{
					this._curAnimTime = (float)this.BeginKey;
				}
				base.GameEntity.SetMorphFrameOfComponents(this._curAnimTime);
				this._curAnimTime += dt * this.Speed;
				if (this._curAnimTime > (float)this.EndKey)
				{
					if (this._curAnimTime > (float)this.EndKey && this._playOnce)
					{
						this.SetIsPlaying(false);
						this._curAnimTime = (float)this.EndKey;
						base.GameEntity.SetMorphFrameOfComponents(this._curAnimTime);
						return;
					}
					int num = 0;
					while (this._curAnimTime > (float)this.EndKey && ++num < 100)
					{
						this._curAnimTime = (float)this.BeginKey + (this._curAnimTime - (float)this.EndKey);
					}
				}
			}
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x000D7DB7 File Offset: 0x000D5FB7
		public void PlayOnce()
		{
			this.Play();
			this._playOnce = true;
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x000D7DC6 File Offset: 0x000D5FC6
		public void Pause()
		{
			this.SetIsPlaying(false);
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x000D7DCF File Offset: 0x000D5FCF
		public void Play()
		{
			this.Stop();
			this.Resume();
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x000D7DDD File Offset: 0x000D5FDD
		public void Resume()
		{
			this.SetIsPlaying(true);
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000D7DE8 File Offset: 0x000D5FE8
		public void Stop()
		{
			this.SetIsPlaying(false);
			this._curAnimTime = (float)this.BeginKey;
			Mesh firstMesh = base.GameEntity.GetFirstMesh();
			if (firstMesh != null)
			{
				firstMesh.MorphTime = this._curAnimTime;
			}
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000D7E30 File Offset: 0x000D6030
		public void StopAndGoToEnd()
		{
			this.SetIsPlaying(false);
			this._curAnimTime = (float)this.EndKey;
			Mesh firstMesh = base.GameEntity.GetFirstMesh();
			if (firstMesh != null)
			{
				firstMesh.MorphTime = this._curAnimTime;
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000D7E75 File Offset: 0x000D6075
		public void SetAnimation(int beginKey, int endKey, float speed)
		{
			this.BeginKey = beginKey;
			this.EndKey = endKey;
			this.Speed = speed;
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x000D7E8C File Offset: 0x000D608C
		public void SetAnimationSynched(int beginKey, int endKey, float speed)
		{
			if (beginKey != this.BeginKey || endKey != this.EndKey || speed != this.Speed)
			{
				this.BeginKey = beginKey;
				this.EndKey = endKey;
				this.Speed = speed;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectVertexAnimation(base.Id, beginKey, endKey, speed));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000D7EF0 File Offset: 0x000D60F0
		public void SetProgressSynched(float value)
		{
			if (MathF.Abs(this.Progress - value) > 0.0001f)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectVertexAnimationProgress(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.Progress = value;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003468 RID: 13416 RVA: 0x000D7F3D File Offset: 0x000D613D
		// (set) Token: 0x06003469 RID: 13417 RVA: 0x000D7F5C File Offset: 0x000D615C
		private float Progress
		{
			get
			{
				return (this._curAnimTime - (float)this.BeginKey) / (float)(this.EndKey - this.BeginKey);
			}
			set
			{
				this._curAnimTime = (float)this.BeginKey + value * (float)(this.EndKey - this.BeginKey);
				Mesh firstMesh = base.GameEntity.GetFirstMesh();
				if (firstMesh != null)
				{
					firstMesh.MorphTime = this._curAnimTime;
				}
			}
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000D7FAC File Offset: 0x000D61AC
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			int count = this._animatedMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				this._animatedMeshes[i].ReleaseEditDataUser();
			}
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000D7FEC File Offset: 0x000D61EC
		protected internal override void OnEditorTick(float dt)
		{
			int componentCount = base.GameEntity.GetComponentCount(TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh);
			bool flag = false;
			for (int i = 0; i < componentCount; i++)
			{
				MetaMesh metaMesh = base.GameEntity.GetComponentAtIndex(i, TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh) as MetaMesh;
				for (int j = 0; j < metaMesh.MeshCount; j++)
				{
					int count = this._animatedMeshes.Count;
					bool flag2 = false;
					for (int k = 0; k < count; k++)
					{
						if (metaMesh.GetMeshAtIndex(j) == this._animatedMeshes[k])
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.RefreshEditDataUsers();
			}
			this.OnTick(dt);
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000D80A0 File Offset: 0x000D62A0
		private void RefreshEditDataUsers()
		{
			foreach (Mesh mesh in this._animatedMeshes)
			{
				mesh.ReleaseEditDataUser();
			}
			this._animatedMeshes.Clear();
			int componentCount = base.GameEntity.GetComponentCount(TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh);
			for (int i = 0; i < componentCount; i++)
			{
				MetaMesh metaMesh = base.GameEntity.GetComponentAtIndex(i, TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh) as MetaMesh;
				for (int j = 0; j < metaMesh.MeshCount; j++)
				{
					Mesh meshAtIndex = metaMesh.GetMeshAtIndex(j);
					meshAtIndex.AddEditDataUser();
					meshAtIndex.HintVerticesDynamic();
					meshAtIndex.HintIndicesDynamic();
					this._animatedMeshes.Add(meshAtIndex);
					Mesh baseMesh = meshAtIndex.GetBaseMesh();
					if (baseMesh != null)
					{
						baseMesh.AddEditDataUser();
						this._animatedMeshes.Add(baseMesh);
					}
				}
			}
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000D81A0 File Offset: 0x000D63A0
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteIntToPacket(this.BeginKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.EndKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionBasic.VertexAnimationSpeedCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Progress, CompressionBasic.AnimationProgressCompressionInfo);
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000D81F4 File Offset: 0x000D63F4
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			VertexAnimator.VertexAnimatorRecord vertexAnimatorRecord = (VertexAnimator.VertexAnimatorRecord)synchedMissionObjectReadableRecord.Item2;
			this.BeginKey = vertexAnimatorRecord.BeginKey;
			this.EndKey = vertexAnimatorRecord.EndKey;
			this.Speed = vertexAnimatorRecord.Speed;
			this.Progress = vertexAnimatorRecord.Progress;
		}

		// Token: 0x04001623 RID: 5667
		public float Speed;

		// Token: 0x04001624 RID: 5668
		public int BeginKey;

		// Token: 0x04001625 RID: 5669
		public int EndKey;

		// Token: 0x04001626 RID: 5670
		private bool _playOnce;

		// Token: 0x04001627 RID: 5671
		private float _curAnimTime;

		// Token: 0x04001628 RID: 5672
		private bool _isPlaying;

		// Token: 0x04001629 RID: 5673
		private readonly List<Mesh> _animatedMeshes = new List<Mesh>();

		// Token: 0x02000665 RID: 1637
		[DefineSynchedMissionObjectType(typeof(VertexAnimator))]
		public struct VertexAnimatorRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AEC RID: 2796
			// (get) Token: 0x06004141 RID: 16705 RVA: 0x000FCF85 File Offset: 0x000FB185
			// (set) Token: 0x06004142 RID: 16706 RVA: 0x000FCF8D File Offset: 0x000FB18D
			public int BeginKey { get; private set; }

			// Token: 0x17000AED RID: 2797
			// (get) Token: 0x06004143 RID: 16707 RVA: 0x000FCF96 File Offset: 0x000FB196
			// (set) Token: 0x06004144 RID: 16708 RVA: 0x000FCF9E File Offset: 0x000FB19E
			public int EndKey { get; private set; }

			// Token: 0x17000AEE RID: 2798
			// (get) Token: 0x06004145 RID: 16709 RVA: 0x000FCFA7 File Offset: 0x000FB1A7
			// (set) Token: 0x06004146 RID: 16710 RVA: 0x000FCFAF File Offset: 0x000FB1AF
			public float Speed { get; private set; }

			// Token: 0x17000AEF RID: 2799
			// (get) Token: 0x06004147 RID: 16711 RVA: 0x000FCFB8 File Offset: 0x000FB1B8
			// (set) Token: 0x06004148 RID: 16712 RVA: 0x000FCFC0 File Offset: 0x000FB1C0
			public float Progress { get; private set; }

			// Token: 0x06004149 RID: 16713 RVA: 0x000FCFCC File Offset: 0x000FB1CC
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.BeginKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref bufferReadValid);
				this.EndKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref bufferReadValid);
				this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.VertexAnimationSpeedCompressionInfo, ref bufferReadValid);
				this.Progress = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}
	}
}
