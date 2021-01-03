using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace MRTKExtensions.Manipulation
{
	[MixedRealityServiceProfile(typeof(IHandSmashingService))]
	[CreateAssetMenu(fileName = "HandSmashingServiceProfile", 
        menuName = "MRTKExtensions/HandSmashingService Configuration Profile")]
	public class HandSmashingServiceProfile : BaseMixedRealityProfile
    {
        [SerializeField]
        private float forceMultiplier = 100;

        public float ForceMultiplier => forceMultiplier;

        [SerializeField]
        private float smashAreaSize = 0.02f;

        public float SmashAreaSize => smashAreaSize;

        [SerializeField]
        private float projectionDistanceMultiplier = 1.1f;

        public float ProjectionDistanceMultiplier => projectionDistanceMultiplier;

        [SerializeField] 
        private Handedness trackedHands = Handedness.Both;

        public Handedness TrackedHands => trackedHands;
    }
}