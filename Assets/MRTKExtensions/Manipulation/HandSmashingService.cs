using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace MRTKExtensions.Manipulation
{
	[MixedRealityExtensionService(SupportedPlatforms.WindowsStandalone|SupportedPlatforms.MacStandalone|SupportedPlatforms.LinuxStandalone|SupportedPlatforms.WindowsUniversal)]
	public class HandSmashingService : BaseExtensionService, IHandSmashingService
	{
		private HandSmashingServiceProfile handSmashingServiceProfile;
        private IMixedRealityHandJointService handJointService;

        public HandSmashingService(string name,  uint priority,  BaseMixedRealityProfile profile) : base(name, priority, profile) 
		{
			handSmashingServiceProfile = (HandSmashingServiceProfile)profile;
		}

        private IMixedRealityHandJointService HandJointService
            => handJointService ?? 
               (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

        private Vector3? lastRightPosition;
        private Vector3? lastLeftPosition;


        public override void Update()
        {
            base.Update();
            if (HandJointService == null)
            {
                return;
            }

            lastRightPosition = ApplySmashMovement(Handedness.Right, lastRightPosition);
            lastLeftPosition = ApplySmashMovement(Handedness.Left, lastLeftPosition);
        }

        private Vector3? ApplySmashMovement(Handedness handedness, Vector3? previousHandLocation)
        {
            Vector3? currentHandPosition = null;
            if ((handSmashingServiceProfile.TrackedHands & handedness) == handedness)
            {
                if (HandJointService.IsHandTracked(handedness))
                {
                    currentHandPosition = 
                        HandJointService.RequestJointTransform(
                                TrackedHandJoint.Palm, handedness)
                        .position;
                    TryApplyForceFromVectors(previousHandLocation, currentHandPosition);
                }
            }

            return currentHandPosition;
        }

        private void TryApplyForceFromVectors(Vector3? previousHandLocation, Vector3? currentHandPosition)
        {
            if (previousHandLocation != null && currentHandPosition != null)
            {
                var handVector = currentHandPosition.Value - previousHandLocation.Value;
                var distanceMoved = Mathf.Abs(handVector.magnitude);
                if (Physics.SphereCast(currentHandPosition.Value, handSmashingServiceProfile.SmashAreaSize, 
                                     handVector, out var hitInfo, 
                                     distanceMoved * handSmashingServiceProfile.ProjectionDistanceMultiplier))                
                {
                    if (hitInfo.rigidbody != null)
                    {
                        hitInfo.rigidbody.AddForceAtPosition(
                            handVector * handSmashingServiceProfile.ForceMultiplier,
                                 hitInfo.transform.position);
                    }
                }
            }
        }
	}
}
