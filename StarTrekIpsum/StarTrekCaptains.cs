using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StarTrekIpsum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StarTrekCaptain
    {
        Kirk,
        Picard,
        Sisko,
        Janeway,
        Archer,
    }
}
