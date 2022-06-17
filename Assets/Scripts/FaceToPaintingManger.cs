using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToPaintingManger : MonoBehaviour
{
    public Texture2D Base;
    Texture2D Finished;

    Texture2D o;

    [SerializeField]SpriteRenderer[] Paintings;
   

    public int offset = 40;


    // Start is called before the first frame update
    void Awake()
    {

        o = GetPainting();

        foreach (SpriteRenderer p in Paintings)
        {
            p.sprite = Sprite.Create(o, new Rect(0.0f, 0.0f, Finished.width, Finished.height), new Vector2(0.5f, 0.5f), Finished.width, 0, SpriteMeshType.Tight);

            p.sprite.texture.filterMode = FilterMode.Point;
        }


    }

    public Texture2D GetPainting()
    {
        if(Finished == null)
        {
            GeneratePainting();
        }

        return Finished;
    }

    public void GeneratePainting()
    {
        Finished = new Texture2D(Base.width, Base.height);
        Texture2D face = Saves.SaveGameContoller.getImage();
       // Finished

        for (int i = 0; i < Base.width; i++)
        {
            for (int j = 0; j < Base.height; j++)
            {
                if(Base.GetPixel(i,j) == new Color(1, 0, 1, 1)) { 
                Finished.SetPixel(i, j, face.GetPixel(j-offset, i -offset));
                }
                else
                {
                    Finished.SetPixel(i, j, Base.GetPixel(i, j));
                }
            }
        }

        Finished.Apply();

    }
}
