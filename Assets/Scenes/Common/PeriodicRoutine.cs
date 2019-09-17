using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonScript
{
    public class PeriodicRoutine
    {
        private float period;
        private float timeElapsed;
        public delegate IEnumerator enumerateCallBack();
        private List<enumerateCallBack> routines;
        public PeriodicRoutine(float period) {
            this.period = period;
            routines = new List<enumerateCallBack>();
        }

        public IEnumerator run() {
            timeElapsed += Time.deltaTime;
            if (timeElapsed < period) yield break;
            timeElapsed = 0.0f;
            foreach (enumerateCallBack routine in routines)
            {
                yield return routine();
            }
        }

        public void setRoutine(enumerateCallBack routine) {
            routines.Add(routine);
        }
    }
}
