using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GoogleApi {
    public static class JsonHelper {
        public static T DeserealizeJson<T>(string authorizedJson) {
            using(var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(authorizedJson))) {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}