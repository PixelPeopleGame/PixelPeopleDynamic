using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;
using CurrentRoute;

namespace Saves
{
    public static class SaveGameContoller
    {

        static Save Player = new Save();

        public static GameObject NewGameUI;
        public static GameObject MainUI;
        static GameObject LoginUI;

        static string Path = Application.persistentDataPath + "/Save.json";
        static string PathImg = Application.persistentDataPath + "/Image.png";
        private static Texture2D PlayerImage;

        public static void Init()
        {
            loadPlayer();

            if (Player.CurrentLocationID > 0 && Player.isGameDone == false)
            {
                

                NewGameUI = GameObject.FindWithTag("UI_NewGame");
                MainUI = GameObject.FindWithTag("UI_Main");
                LoginUI = GameObject.FindWithTag("UI_Login");

                NewGameUI.GetComponent<Canvas>().enabled = true;
                MainUI.GetComponent<Canvas>().enabled = false;
                

                Button _newGame = GameObject.FindGameObjectWithTag("Button_NewGame").GetComponent<Button>();

                _newGame.onClick.AddListener(() =>
                    {
                        Player = new Save();
                        PlayerImage = null;
                        Player.hasImage = false;
                        NewGameUI.GetComponent<Canvas>().enabled = false;
                        MainUI.GetComponent<Canvas>().enabled = true;
                        SavePlayer(0);

                    });

                Button _oldGame = GameObject.FindGameObjectWithTag("Button_OldGame").GetComponent<Button>();

                _oldGame.onClick.AddListener(() =>
                    {
                        WaypointController.Instance.SetWaypointByID(Player.CurrentLocationID);
                        
                        NewGameUI.GetComponent<Canvas>().enabled = false;
                        MainUI.GetComponent<Canvas>().enabled = true;
                        LoginUI.SetActive(false);
                    });
            }
            else
            {
                Player = new Save();
            }
        }

        public static void loadPlayer()
        {
            string json = File.ReadAllText(Path);
            Player = JsonUtility.FromJson<Save>(json);

            if (Player.hasImage == true)
            {
                try
                {
                    byte[] imagebytes = File.ReadAllBytes(PathImg);

                    PlayerImage = new Texture2D(640, 480);

                    PlayerImage.LoadImage(imagebytes);
                }
                catch (Exception o)
                {
                    File.WriteAllText(Application.persistentDataPath + "/ERROR2.json", o.Message);

                    Player.hasImage = false;
                    
                }

            }


        }
        public static void FinishGame()
        {
            Player.isGameDone = true;
        }

        public static Texture2D getImage()
        {
            try
            {
                if (PlayerImage != null)
                {
                    return PlayerImage;
                }
                else
                {
                    if (Player.hasImage == true)
                    {
                        byte[] imagebytes = File.ReadAllBytes(PathImg);
                        //imagebyte

                        PlayerImage = new Texture2D(640, 480);

                        PlayerImage.LoadImage(imagebytes);

                        return PlayerImage;
                    }
                }
                return null;
            }
            catch (Exception o)
            {
                File.WriteAllText(Application.persistentDataPath + "/ERROR.json",o.Message);

                return null;
            }


        }

        public static void SavePlayer(int id)
        {
            try
            {
                Player.CurrentLocationID = id;

                string Json = JsonUtility.ToJson(Player);

                File.WriteAllText(Path, Json);

            }
            catch (Exception ex)
            {
                Debug.Log("ERROR: " + ex);
            }
        }

        public static void SaveImage(Texture2D Image)
        {

            PlayerImage = Image;

            Player.hasImage = true;

            byte[] bytes = Image.EncodeToPNG();

            File.WriteAllBytes(PathImg, bytes);

        }

        public static void SetMail(string mail)
        {
            Player.Email = mail;
        }
        public static string getMail()
        {
            return Player.Email;
        }

    }

    [Serializable]
    public class Save
    {
        public int CurrentLocationID = 0;
        public string Email = "";
        public bool hasImage = false;
        public bool isGameDone = false;
    }
}
