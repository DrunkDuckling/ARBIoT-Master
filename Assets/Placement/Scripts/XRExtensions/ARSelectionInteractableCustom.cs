using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARSelectionInteractableCustom : ARBaseGestureInteractable
{
    
    [SerializeField, Tooltip("The visualization GameObject that will become active when the object is selected.")]
    GameObject m_SelectionVisualization;

    private Button m_delete_button;

    /// <summary>
    /// The visualization <see cref="GameObject"/> that will become active when the object is selected.
    /// </summary>
    public GameObject selectionVisualization
    {
        get => m_SelectionVisualization;
        set => m_SelectionVisualization = value;
    }


    private GameObject _placedObjectPrefab;
    private GameObject _placedObject
    {
        get => _placedObjectPrefab;
        set => _placedObjectPrefab = value;
    }

    private void Start()
    {
        m_delete_button = GameObject.FindGameObjectWithTag("DeleteButton").GetComponent<Button>();
        m_delete_button.onClick.AddListener(DeleteObjectButton);
    }

    bool m_GestureSelected;

    /// <inheritdoc />
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        if (!(interactor is ARGestureInteractor))
            return false;

        return m_GestureSelected;
    }

    /// <inheritdoc />
    protected override bool CanStartManipulationForGesture(TapGesture gesture) => true;

    /// <inheritdoc />
    protected override void OnEndManipulation(TapGesture gesture)
    {
        base.OnEndManipulation(gesture);

        if (gesture.isCanceled)
            return;
        if (gestureInteractor == null)
            return;

        if (gesture.targetObject == gameObject)
        {
            // Toggle selection
            m_GestureSelected = !m_GestureSelected;
            Debug.Log("OnEndManipulation");

            if (m_GestureSelected)
            {
                _placedObject = gameObject;
            }
            if (!m_GestureSelected)
            {
                _placedObject = null;
            }
        }
        else
            m_GestureSelected = false;
    }

    /// <inheritdoc />
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        if (m_SelectionVisualization != null)
            m_SelectionVisualization.SetActive(true);
        Debug.Log("OnSelectEntering");
        m_delete_button.gameObject.SetActive(true);

    }

    /// <inheritdoc />
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        if (m_SelectionVisualization != null)
            m_SelectionVisualization.SetActive(false);
        _placedObject = null;
        m_delete_button.gameObject.SetActive(false);
    }

    public void DeleteObjectButton()
    {
        if(_placedObject != null)
        {
            Destroy(_placedObject);
        }
    }
}