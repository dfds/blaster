using System;
using System.Text.Json;

namespace Blaster.WebApi.Features.Capabilities
{
	public static class JsonObjectTools
	{
		public static object GetValueFromJsonElement(JsonElement val)
		{
			switch (val.ValueKind)
			{
				case JsonValueKind.String:
					return val.GetString();
				case JsonValueKind.True:
					return val.GetBoolean();
				case JsonValueKind.False:
					return val.GetBoolean();
				case JsonValueKind.Number:
					var oki = val.TryGetInt32(out var vali);
					if (oki)
					{
						return vali;
					}

					var okl = val.TryGetInt64(out var vall);
					if (okl)
					{
						return vall;
					}

					var okd = val.TryGetDouble(out var vald);
					if (okd)
					{
						return vald;
					}
					throw new Exception("Unexpected JSON value type");
				default:
					throw new Exception("Unexpected JSON value type");
			}
		}
	}
}
