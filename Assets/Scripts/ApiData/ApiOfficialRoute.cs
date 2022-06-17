using Newtonsoft.Json;

public class ApiOfficialRoute
{
    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("URL")]
    public string URL { get; set; }

    [JsonProperty("Description")]
    public string Description { get; set; }

    [JsonProperty("ImageURL")]
    public string ImageURL { get; set; }

}