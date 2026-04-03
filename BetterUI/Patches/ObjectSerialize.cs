using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace BetterUI.Patches;

public static class ObjectSerialize
{
	public static byte[] Serialize(this object obj)
	{
		if (obj == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		new BinaryFormatter().Serialize(memoryStream, obj);
		return Compress(memoryStream.ToArray());
	}

	public static object DeSerialize(this byte[] arrBytes)
	{
		using MemoryStream memoryStream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		byte[] array = Decompress(arrBytes);
		memoryStream.Write(array, 0, array.Length);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return binaryFormatter.Deserialize(memoryStream);
	}

	public static byte[] Compress(byte[] input)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
		{
			gZipStream.Write(input, 0, input.Length);
		}
		return memoryStream.ToArray();
	}

	public static byte[] Decompress(byte[] input)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (MemoryStream stream = new MemoryStream(input))
		{
			using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
			gZipStream.CopyTo(memoryStream);
		}
		return memoryStream.ToArray();
	}
}
