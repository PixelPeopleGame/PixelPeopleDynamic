using Newtonsoft.Json;
using System.Collections.Generic;

public class ApiOfficialRoutes
{
    [JsonProperty("Routes")]
    public IList<ApiOfficialRoute> Routes { get; set; }
}