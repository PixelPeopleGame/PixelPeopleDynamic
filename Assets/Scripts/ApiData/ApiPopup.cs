using Newtonsoft.Json;
using System.Collections.Generic;

public enum PopupType
{
    // never change this order!
    Default,
    NuNI,
    Jackson,
    Information,
    Question,
    Instruction,
    NoPopup,
    Link,
    Video,
    Temp
}

public class ApiPopup
{
    [JsonProperty("Id")]
    public int Id { get; set; }

    [JsonIgnore] //[JsonProperty("Id")]
    public int IndexInList { get; set; }

    [JsonIgnore]
    public bool Delete { get; set; }

    [JsonProperty("Title")]
    public string Title { get; set; }

    [JsonProperty("Description")]
    public string Description { get; set; }

    [JsonProperty("Timer")]
    public int Timer { get; set; }

    [JsonProperty("PopupType")]
    public PopupType PopupType { get; set; }

    [JsonProperty("QuestionAnswers")]
    public IList<string> QuestionAnswers { get; set; }

    [JsonProperty("CorrectAnswer")]
    public string CorrectAnswer { get; set; }

    [JsonProperty("Points")]
    public string Points { get; set; }

    [JsonProperty("Link")]
    public string Link { get; set; }

    public ApiPopup()
    {
        this.Title = "New Title";
        this.QuestionAnswers = new List<string>();
        this.Delete = false;
    }
}
