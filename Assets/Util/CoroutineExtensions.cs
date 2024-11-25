using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.CoroutineExtensions
{
    public static class Extension
    {
        public static Coroutine WaitSecondsToExecute(this MonoBehaviour mono, float waitSeconds, Action function)
        {
            return mono.StartCoroutine(WaitSecondsToExecuteEnumerator(waitSeconds, function));
        }

        private static IEnumerator WaitSecondsToExecuteEnumerator(float waitSeconds, Action function)
        {
            yield return new WaitForSeconds(waitSeconds);

            function?.Invoke();
        }

        public static Coroutine WaitFramesToExecute(this MonoBehaviour mono, int frames, Action function)
        {
            return mono.StartCoroutine(WaitFramesToExecuteEnumerator(frames, function));
        }

        private static IEnumerator WaitFramesToExecuteEnumerator(int frames, Action function)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }

            function.Invoke();
        }
    }

}