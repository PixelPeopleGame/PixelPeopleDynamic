using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum SpecialUI
{
    None,
    MoodMe,
    ARMaze,
    ARDrone
}

public enum VisibleUI
{
    SettingsIcon,
    Minimap,

    // Keep at Bottom
    Dummy
}

public class ApiWaypoint
{
    [JsonProperty("ID")]
    public int Id { get; set; }

    [JsonProperty("Longitude")]
    public double Longitude { get; set; }

    [JsonProperty("Latitude")]
    public double Latitude { get; set; }

    [JsonProperty("Radius")]
    public int Radius { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Popups")]
    public IList<ApiPopup> Popups { get; set; }

    [JsonProperty("VisibleUI")]
    public IList<VisibleUI> UIVisible { get; set; }

    [JsonProperty("SpecialUI")]
    public SpecialUI SpecialUI { get; set; }

    public ApiWaypoint()
    {
        this.Popups = new List<ApiPopup>();
        this.UIVisible = new List<VisibleUI>();
        this.Name = "Waypoint";
    }
}
