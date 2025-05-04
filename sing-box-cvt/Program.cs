using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace sing_box_cvt
{
    class SingBoxRule
    {
        public class RuleObject
        {
            [JsonPropertyName("domain")]
            public string[] Domain { get; set; } = [];

            [JsonPropertyName("domain_suffix")]
            public string[] DomainSuffix { get; set; } = [];
        }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("rules")]
        public RuleObject[] Rules { get; set; } = [];
    }


    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            string key = "gfw";
            string url = $"https://github.com/MetaCubeX/meta-rules-dat/raw/sing/geo/geosite/{key}.json";
            
            HttpClient client = new();
            string json = await client.GetStringAsync(url);
            File.WriteAllText($"{key}.json", json);

            SingBoxRule r = JsonSerializer.Deserialize<SingBoxRule>(json)!;
            if (r.Rules.Length == 0) return;


            FileStream fs = new($"{key}.conf", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new(fs);
            foreach (string d in r.Rules[0].Domain)
            {
                sw.WriteLine(d);
            }
            foreach (string d in r.Rules[0].DomainSuffix)
            {
                sw.WriteLine($"*-.{d}");
            }
            sw.Close();
            fs.Close();
        }
    }
}
