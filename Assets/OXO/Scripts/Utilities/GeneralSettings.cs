using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Game Settings", menuName = "[Oxo Games]/Game Settings/New Game Settings", order = 1)]
public class GeneralSettings : ScriptableObject
{
   private static string mainPath = "Assets/OXO/";
   public Object Scene;
   
   #region Singleton
   private static GeneralSettings _instance;
   
   public static GeneralSettings Instance
   {
      get
      {
         if (_instance == null)
            _instance = Resources.Load("OXO/Game Settings") as GeneralSettings;
         return _instance;
      }
   }
   #endregion
}
