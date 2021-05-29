using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using arbiot;

namespace DepthMenu
{
    /// <summary>
    /// This component tests for depth functionality and enables/disables
    /// a text message on the screen reporting that depth is not suppoted.
    /// </summary>
    public class DepthMenu : Singleton<DepthMenu>
    {
        [SerializeField]
        private Button qualityButton;
        [SerializeField]
        private Button humanStensilButton;
        [SerializeField]
        private Button prefButton;

        private TextMeshProUGUI qualityButtonText, humanStensilButtonText, prefButtonText;

        [SerializeField]
        Text m_DepthAvailabilityInfo;

        /// <summary>
        /// The UI Text used to display information about the availability of depth functionality.
        /// </summary>
        public Text depthAvailabilityInfo
        {
            get { return m_DepthAvailabilityInfo; }
            set { m_DepthAvailabilityInfo = value; }

        }

        // Start is called before the first frame update
        void Start()
        {
            if (AROcclusionQualityController.Instance.IsDepthSupported())
            {
                depthAvailabilityInfo.text = "Depth is supported";
                UpdateQualityText();
            }   
            else
            {
                depthAvailabilityInfo.text = "Depth is not supported";
                qualityButton.interactable = false;
                humanStensilButton.interactable = false;
                prefButton.interactable = false;
            }
            
        }

        private void Awake()
        {
            qualityButtonText = qualityButton.GetComponentInChildren<TextMeshProUGUI>();
            humanStensilButtonText = humanStensilButton.GetComponentInChildren<TextMeshProUGUI>();
            prefButtonText = prefButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        

        // Used to change the quality of Occlusion
        public void ToggleQuality()
        {
            EnvironmentDepthMode depthMode = AROcclusionQualityController.Instance.GetCurrentDepthMode();

            switch (depthMode)
            {
                case EnvironmentDepthMode.Disabled:
                    AROcclusionQualityController.Instance.ChangeQualityTo(EnvironmentDepthMode.Fastest);
                    break;
                case EnvironmentDepthMode.Fastest:
                    AROcclusionQualityController.Instance.ChangeQualityTo(EnvironmentDepthMode.Medium);
                    break;
                case EnvironmentDepthMode.Medium:
                    AROcclusionQualityController.Instance.ChangeQualityTo(EnvironmentDepthMode.Best);
                    break;
                case EnvironmentDepthMode.Best:
                    AROcclusionQualityController.Instance.ChangeQualityTo(EnvironmentDepthMode.Disabled);
                    break;
            }

            UpdateQualityText();
        }

        public void ToggleHuStQuality()
        {
            HumanSegmentationStencilMode humanStensileMode = AROcclusionQualityController.Instance.GetCurrentStencilMode();

            switch (humanStensileMode)
            {
                case HumanSegmentationStencilMode.Disabled:
                    AROcclusionQualityController.Instance.ChangeHuSeStQualityTo(HumanSegmentationStencilMode.Fastest);
                    AROcclusionQualityController.Instance.ChangeHuSeDeQualityTo(HumanSegmentationDepthMode.Fastest);
                    break;
                case HumanSegmentationStencilMode.Fastest:
                    AROcclusionQualityController.Instance.ChangeHuSeStQualityTo(HumanSegmentationStencilMode.Best);
                    AROcclusionQualityController.Instance.ChangeHuSeDeQualityTo(HumanSegmentationDepthMode.Best);
                    break;
                case HumanSegmentationStencilMode.Best:
                    AROcclusionQualityController.Instance.ChangeHuSeStQualityTo(HumanSegmentationStencilMode.Disabled);
                    AROcclusionQualityController.Instance.ChangeHuSeDeQualityTo(HumanSegmentationDepthMode.Disabled);
                    break;
            }

            UpdateQualityText();
        }

        public void TogglePreference()
        {
            OcclusionPreferenceMode prefMode = AROcclusionQualityController.Instance.GetCurrentPreferenceMode();

            switch (prefMode)
            {
                case OcclusionPreferenceMode.PreferEnvironmentOcclusion:
                    AROcclusionQualityController.Instance.ChangeOcclusionPreferenceMode(OcclusionPreferenceMode.PreferHumanOcclusion);
                    break;
                case OcclusionPreferenceMode.PreferHumanOcclusion:
                    AROcclusionQualityController.Instance.ChangeOcclusionPreferenceMode(OcclusionPreferenceMode.PreferEnvironmentOcclusion);
                    break;

            }

            UpdateQualityText();
        }

        private void UpdateQualityText()
        {
            EnvironmentDepthMode newDepthMode = AROcclusionQualityController.Instance.GetCurrentDepthMode();
            qualityButtonText.text = $"Env Depth: {newDepthMode}";

            HumanSegmentationStencilMode newHumaneMode = AROcclusionQualityController.Instance.GetCurrentStencilMode();
            humanStensilButtonText.text = $"Stencil: {newHumaneMode}";

            OcclusionPreferenceMode prefhMode = AROcclusionQualityController.Instance.GetCurrentPreferenceMode();
            prefButtonText.text = $"Pref:  {prefhMode}";
        }
    }
}


