using UnityEngine;

namespace Managers.Sound
{
    public static class SoundManager
    {
        #region Variables

        private const string SoundEnabledKey = "isSoundEnabled";

        #endregion

        #region Properties

        public static int IsSoundsEnabled => PlayerPrefs.GetInt(SoundEnabledKey, 1);
        
        #endregion


        private static void PlayerPrefsSave(int truthValue)
        {
            if (truthValue == 1 || truthValue == 0)
            {
                PlayerPrefs.SetInt(SoundEnabledKey, truthValue);
            }
            else
            {
                Debug.LogError("You are setted invalid truth value");
            }
        }


        public static void ControlSound()
        {
            int truthValue = IsSoundsEnabled;

            if (truthValue == 1)
            {
                truthValue = 0;
                AudioListener.pause = true;
            }
            else
            {
                truthValue = 1;
                AudioListener.pause = false;
            }

            PlayerPrefsSave(truthValue);
        }
    }
}