using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPromtDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject prompt;
    [SerializeField]
    private ScriptableValue<FinishingTarget> target;
    [SerializeField]
    private ScriptableValue<bool> isFinishing;

    private void Start()
    {
        ChangePromptVisibility(target.Value, isFinishing.Value);
    }

    private void OnEnable()
    {
        target.ValueChangeEvent.AddListener(OnTargetChanged);
        isFinishing.ValueChangeEvent.AddListener(OnCanFinishChanged);
    }

    private void OnDisable()
    {
        target.ValueChangeEvent.RemoveListener(OnTargetChanged);
        isFinishing.ValueChangeEvent.RemoveListener(OnCanFinishChanged);
    }


    private void OnTargetChanged(FinishingTarget newTarget)
    {
        ChangePromptVisibility(newTarget, isFinishing.Value);
    }
    private void OnCanFinishChanged(bool newCanFinish)
    {
        ChangePromptVisibility(target.Value, newCanFinish);
    }

    private void ChangePromptVisibility(FinishingTarget target, bool isFinishing)
    {
        prompt.SetActive(target != null && !isFinishing);
    }
}
