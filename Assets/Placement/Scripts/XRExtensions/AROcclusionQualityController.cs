using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace arbiot
{
    [RequireComponent(typeof(AROcclusionManager))]
    public class AROcclusionQualityController : Singleton<AROcclusionQualityController>
    {
        private AROcclusionManager _AROcclusionManager;

        void Awake()
        {
            _AROcclusionManager = GetComponent<AROcclusionManager>();
        }

        public bool IsDepthSupported()
        {
            /*bool depthtest =
                ((_AROcclusionManager.descriptor?.supportsHumanSegmentationStencilImage == false) &&
                (_AROcclusionManager.descriptor?.supportsHumanSegmentationDepthImage == false) &&
                (_AROcclusionManager.descriptor?.supportsEnvironmentDepthImage == false));*/

            /*bool test1 = (_AROcclusionManager.descriptor.supportsHumanSegmentationStencilImage);
            bool test2 = (_AROcclusionManager.descriptor.supportsHumanSegmentationDepthImage);*/

            bool test3 = (_AROcclusionManager.descriptor.supportsEnvironmentDepthImage); 

            return test3;
        }

        public bool IsHumanDepthSupported()
        {
            bool test2 = (_AROcclusionManager.descriptor.supportsHumanSegmentationDepthImage);
            return test2;
        }

        public void ChangeQualityTo(EnvironmentDepthMode environmentDepthMode)
        {
            _AROcclusionManager.requestedEnvironmentDepthMode = environmentDepthMode;
        }
        public EnvironmentDepthMode GetCurrentDepthMode()
        {
            return _AROcclusionManager.requestedEnvironmentDepthMode;
        }

        public void ChangeHuSeStQualityTo(HumanSegmentationStencilMode humanSegmentationStencilMode)
        {
            _AROcclusionManager.requestedHumanStencilMode = humanSegmentationStencilMode;
        }

        public HumanSegmentationStencilMode GetCurrentStencilMode()
        {
            return _AROcclusionManager.requestedHumanStencilMode;
        }

        public void ChangeHuSeDeQualityTo(HumanSegmentationDepthMode humanSegmentationDepth)
        {
            _AROcclusionManager.requestedHumanDepthMode = humanSegmentationDepth;
        }

        public HumanSegmentationDepthMode GetCurrentHumanDepthMode()
        {
            return _AROcclusionManager.requestedHumanDepthMode;
        }

        public void ChangeOcclusionPreferenceMode(OcclusionPreferenceMode occlusionPreferenceMode)
        {
            _AROcclusionManager.requestedOcclusionPreferenceMode = occlusionPreferenceMode;
        }

        public OcclusionPreferenceMode GetCurrentPreferenceMode()
        {
            return _AROcclusionManager.requestedOcclusionPreferenceMode;
        }
    }
}
