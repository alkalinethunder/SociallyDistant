#nullable enable
using Core.DataManagement;
using Core.Serialization;
using UnityEngine.TextCore.LowLevel;

namespace Core.WorldData.Data
{
	public struct WorldPortForwardingRule : ISerializable<WorldRevision>, IDataWithId
	{
		private ObjectId instanceId;
		private ObjectId lanId;
		private ObjectId computerId;
		private ushort localPort;
		private ushort globalPort;

		/// <inheritdoc />
		public ObjectId InstanceId
		{
			get => instanceId;
			set => instanceId = value;
		}

		public ObjectId LanId
		{
			get => lanId;
			set => lanId = value;
		}
		
		public ObjectId ComputerId
		{
			get => computerId;
			set => computerId = value;
		}

		public ushort LocalPort
		{
			get => localPort;
			set => localPort = value;
		}

		public ushort GlobalPort
		{
			get => globalPort;
			set => globalPort = value;
		}

		/// <inheritdoc />
		public void Serialize(IRevisionedSerializer<WorldRevision> serializer)
		{
			SerializationUtility.SerializeAtRevision(ref instanceId, serializer, WorldRevision.AddedPortForwarding, default);
			SerializationUtility.SerializeAtRevision(ref lanId, serializer, WorldRevision.AddedPortForwarding, default);
			SerializationUtility.SerializeAtRevision(ref computerId, serializer, WorldRevision.AddedPortForwarding, default);
			SerializationUtility.SerializeAtRevision(ref localPort, serializer, WorldRevision.AddedPortForwarding, default);
			SerializationUtility.SerializeAtRevision(ref globalPort, serializer, WorldRevision.AddedPortForwarding, default);
		}
	}
}