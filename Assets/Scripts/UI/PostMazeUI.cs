using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PostMazeUI : MonoBehaviour
{

    public string[] pownedInfo;
    string Info;

    [SerializeField] HaveIBeenPownedRetriever HaveIBeenPownedRetriever;
    [SerializeField] private Animator _animation;


    [SerializeField] Text DisplayList;
    [SerializeField] Text mazeCountText;
    [SerializeField] Image DisplayImage;
    [SerializeField] Button _closeButton;

    [SerializeField] GameObject[] childeren;



    private void Awake()
    {

        StartCoroutine(Init());

    }


    private IEnumerator Init()
    {
        _animation.Play("Base Layer.FrameOpenWeb");

        yield return new WaitForSeconds(1.5f);

        foreach(GameObject child in childeren)
        {
            child.SetActive(true);
        }

        this.GetComponent<Image>().color = new Color(0,0,0,1);


        _closeButton.GetComponent<Button>().onClick.AddListener(() => close());

        pownedInfo = HaveIBeenPownedRetriever.GetAll();

        GenerateCompleteString();

        DisplayList.text = Info;

        Texture2D image = Saves.SaveGameContoller.getImage();

        DisplayImage.sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), image.width, 0, SpriteMeshType.Tight);


    }

    private void close()
    {
        _animation.Play("Base Layer.frameClose");
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        mazeCountText.text = "";

        foreach (GameObject child in childeren)
        {
            child.SetActive(false);
        }

        gameObject.SetActive(false);




    }

    private void GenerateCompleteString()
    {
        Info = "";

        foreach (string item in pownedInfo)
        {
            Info += item;
            Info += "\n";
        }
        //Info = Info.Replace('', '\n');
    }
}
