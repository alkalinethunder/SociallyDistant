#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Core.Config
{
	public static class Registry
	{
		private static bool initialized;
		private static JObject currentRegistry = new JObject();

		public static event Action? Updated;
		
		public static string GameDataPath => Application.persistentDataPath.Replace('/', Path.DirectorySeparatorChar);
		public static string RegistryFilePath => Path.Combine(GameDataPath, "settings.json");
		public static void Initialize()
		{
			ThrowIfInitialized();

			initialized = true;
			
			LoadRegistry();
		}

		public static void Reload()
		{
			LoadRegistry();
			Updated?.Invoke();
		}

		public static IEnumerable<KeyValuePair<string, JToken?>> GetAllKeys()
		{
			ThrowIfNotInitialized();

			foreach (KeyValuePair<string, JToken?> key in currentRegistry)
			{
				yield return key;
			}
		}

		public static bool HasKey(string key)
			=> GetAllKeys().Any(x => x.Key == key);

		public static void ResetKey(string key)
		{
			ThrowIfNotInitialized();

			if (!HasKey(key))
				return;

			currentRegistry.Remove(key);
			SaveRegistry();
			Updated?.Invoke();
		}
		
		public static bool HasValue<T>(string key)
		{
			ThrowIfNotInitialized();

			return TryGetValue<T>(key, out _);
		}
		
		public static bool TryGetValue<T>(string key, out T? value)
		{
			ThrowIfNotInitialized();
			value = default;

			if (currentRegistry.TryGetValue(key, out JToken? jobj))
			{
				RegistryValue<T>? registryValue = jobj.ToObject<RegistryValue<T>>();
				if (registryValue is null)
					return false;

				value = registryValue.Value;
				return true;
			}

			return false;
		}

		public static void GetValueOrSetDefault<T>(string key, T defaultValue, out T? value)
		{
			if (!TryGetValue(key, out value))
			{
				value = defaultValue;
				SetValue(key, value);
			}
		}
		
		public static void SetValue<T>(string key, T value)
		{
			ThrowIfNotInitialized();

			if (currentRegistry.ContainsKey(key))
			{
				if (value is null)
					currentRegistry.Remove(key);
				else
					currentRegistry[key] = JObject.FromObject(new RegistryValue<T>
					{
						Value = value
					});
			}
			else
			{
				if (value is not null)
					currentRegistry.Add(key, JObject.FromObject(new RegistryValue<T>
					{
						Value = value
					}));
			}

			SaveRegistry();
			Updated?.Invoke();
		}

		public static void SaveRegistry()
		{
			string json = currentRegistry.ToString();
			File.WriteAllText(RegistryFilePath, json);
		}
		
		private static void WipeRegistry()
		{
			currentRegistry = new JObject();
		}

		private static void LoadRegistry()
		{
			bool success = false;
			if (File.Exists(RegistryFilePath))
			{
				string json = File.ReadAllText(RegistryFilePath);

				try
				{
					currentRegistry = JObject.Parse(json);
					success = true;
				}
				catch (Exception ex)
				{
					Debug.LogError("Registry failed to load, see upcoming exception.");
					Debug.LogException(ex);
				}
			}

			if (!success)
				WipeRegistry();
		}

		private static void ThrowIfNotInitialized()
		{
			if (!initialized)
				throw new InvalidOperationException("Registry has not been initialized.");
		}
		
		private static void ThrowIfInitialized()
		{
			if (initialized)
				throw new InvalidOperationException("The registry has already been initialized.");
		}
	}
}