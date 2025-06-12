using UnityEngine;

namespace Util
{
    public class SquishAndStretch : MonoBehaviour
    {
        [SerializeField] private GameObject warpObject;    // the fuel display rect transform
        [SerializeField] private Vector3 squish;            // the mask of the fuel bar
        [SerializeField] private Vector3 stretch;           // the mask of the fuel bar
        [SerializeField] private float warpTime;           // the mask of the fuel bar

        public void WarpObject()
        {
            LeanTween.cancel(warpObject);
            
            LeanTween.scale(warpObject, squish,warpTime/3).setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() => 
                    LeanTween.scale(warpObject, stretch,warpTime/3).setEase(LeanTweenType.easeOutBack)
                        .setOnComplete(() => 
                            LeanTween.scale(warpObject, Vector3.one, warpTime/3).setEase(LeanTweenType.easeOutBack)));
        }
    }
}
