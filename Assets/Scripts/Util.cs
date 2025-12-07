using System.Collections;
using UnityEngine;


public static class Util
{
    public static void RunAfter(this MonoBehaviour behviour, float delay, System.Action action)
    {
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        behviour.StartCoroutine(Delay());
    }

    public static void PlayAt(this AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }
}

// Fix https://stackoverflow.com/a/64749403
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}