using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Managers.Vibration
{
    public static class VibrationManager
    {
        #region Variables

        private const string VibrationEnabledKey = "isVibrationEnabled";
        
        #endregion

        #region MyRegion

        public static int IsVibrationEnabled => PlayerPrefs.GetInt(VibrationEnabledKey, 1);
        
        #endregion
        
        private static void PlayerPrefsSave(int truthValue)
        {
            if (truthValue == 1 || truthValue == 0)
            {
                PlayerPrefs.SetInt(VibrationEnabledKey, truthValue);
            }
            else
            {
                Debug.LogError("You are setted invalid truth value");
            }
        }

        public static void ControlVibration()
        {
            int truthValue = IsVibrationEnabled;

            if (truthValue == 1)
            {
                truthValue = 0;
            }
            else
            {
                truthValue = 1;
            }

            PlayerPrefsSave(truthValue);
        }
        
#if MOREMOUNTAINS_NICEVIBRATIONS
         public static void PlayVibration(HapticTypes type)
         {
             if (IsVibrationEnabled == 1)
             {
                 MMVibrationManager.Haptic(type);
             }
         }
#endif
    }

}
