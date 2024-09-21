using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProjetoCore2.Factories
{
    public class JsonSerializerOptionsFactory
    {
        public static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return options;
        }
    }
}
