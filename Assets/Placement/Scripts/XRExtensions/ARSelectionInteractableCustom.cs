using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARSelectionInteractableCustom : ARBaseGestureInteractable
{
    
    [SerializeField, Tooltip("The visualization GameObject that will become active when the object is selected.")]
    GameObject m_SelectionVisualization;

    /// <summary>
    /// The visualization <see cref="GameObject"/> that will become active when the object is selected.
    /// </summary>
    public GameObject selectionVisualization
    {
        get => m_SelectionVisualization;
        set => m_SelectionVisualization = value;
    }

    // Used to delete selected VO's
    private Button _DeleteObjecBbutton;
    // Selected VO
    private GameObject _selectedVO;
    public GameObject SelectedVO { get { return _selectedVO; } set { _selectedVO = value; } }


    bool m_GestureSelected;

    private void Start()
    {
        _DeleteObjecBbutton = GameObject.FindGameObjectWithTag("DeleteButton").GetComponent<Button>();
        _DeleteObjecBbutton.onClick.AddListener(DeleteSelectedObject);
    }

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
        }
        else
            m_GestureSelected = false;

        _selectedVO = (m_GestureSelected) ? gameObject : null;

    }

    /// <inheritdoc />
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        if (m_SelectionVisualization != null)
            m_SelectionVisualization.SetActive(true);

    }

    /// <inheritdoc />
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        if (m_SelectionVisualization != null)
            m_SelectionVisualization.SetActive(false);
    }

    public void DeleteSelectedObject()
    {
        if(_selectedVO != null) 
            Destroy(_selectedVO);
    }
}