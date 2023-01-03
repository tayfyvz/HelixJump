using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ConfettiManager : Singleton<ConfettiManager>
{
    public List<ParticleSystem> confettiParticlesList;
    public void Play()
    {
        if (confettiParticlesList.Count == 0)
        {
            Debug.Log($"<color=white><b>(!) Couldn't find confetti in the Confetti List.</b> </color>"); return;
        }
        Toggle();
        confettiParticlesList.ForEach(x => x.Play());
    }

    public void Stop()
    {
        if (confettiParticlesList.Count == 0)
        {
            Debug.Log($"<color=white><b>(!) Couldn't find confetti in the Confetti List.</b> </color>"); return;
        }

        Toggle();
        confettiParticlesList.ForEach(x => x.Stop());
    }

    public void Toggle() => confettiParticlesList.ForEach(x => x.gameObject.SetActive(!x.gameObject.activeInHierarchy));
}