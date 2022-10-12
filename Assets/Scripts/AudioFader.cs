using System.Collections;
using UnityEngine;

public static class AudioFader
{


    public static IEnumerator Fade(AudioSource aS, float duration, float targetVolume)
    {
        
        
        float currentTime = 0;
        float start = aS.volume;
        
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            aS.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

