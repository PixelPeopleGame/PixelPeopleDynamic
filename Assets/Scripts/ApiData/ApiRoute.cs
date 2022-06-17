using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ApiRoute
{
    // Author
    // Date Editted, etc
    // [JsonProperty("Name")]
    [JsonIgnore] // enable asap
    public string Name { get; set; }

    [JsonProperty("Waypoints")]
    public IList<ApiWaypoint> Waypoints { get; set; }

    public ApiRoute()
    {
        this.Name = "API2";
        this.Waypoints = new List<ApiWaypoint>();
    }
}
