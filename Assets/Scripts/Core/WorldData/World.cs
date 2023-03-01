﻿#nullable enable

using Core.DataManagement;
using Core.Serialization;
using Core.Systems;
using Core.WorldData.Data;

namespace Core.WorldData
{
	public class World
	{
		public readonly DataTable<WorldComputerData, WorldRevision> Computers;

		public World(UniqueIntGenerator instanceIdGenerator, DataEventDispatcher eventDispatcher)
		{
			Computers = new DataTable<WorldComputerData, WorldRevision>(instanceIdGenerator, eventDispatcher);
		}

		public void Serialize(IRevisionedSerializer<WorldRevision> serializer)
		{
			Computers.Serialize(serializer, WorldRevision.AddedComputers);
		}

		public void Wipe()
		{
			// You must wipe the world in reverse order of how you would create or serialize it.
			// This ensures proper handling of deleting objects that depend on other objects.
			this.Computers.Clear();
		}
	}
}