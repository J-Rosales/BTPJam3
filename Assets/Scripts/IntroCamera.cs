using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float transitionTime;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private CanvasGroup menuCanvasGroup;        
    Vector3 startPosition;
    Vector3 targetPosition;

    float startSize;
    float targetSize;

    CameraHover hover;
    Camera introCamera;
    float transitionCounter;
    IEnumerator transitionRoutine;

    private void Awake()
    {
        hover = GetComponent<CameraHover>();
        introCamera = GetComponent<Camera>();    
    }

    public void StartIntroTransition()
    {
        hover.enabled = false;
        transitionRoutine = TransitionRoutine();
        StartCoroutine(transitionRoutine);
    }

    public IEnumerator TransitionRoutine()
    {
        startPosition = transform.position;
        startSize = introCamera.orthographicSize;
        targetPosition = gameCamera.transform.position;
        targetSize = gameCamera.orthographicSize;
        while(transitionCounter < transitionTime)
        {

            transitionCounter += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition,
                    transitionCounter / transitionTime);
            introCamera.orthographicSize = Mathf.Lerp(startSize, targetSize,
                    transitionCounter / transitionTime);
            menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f,
                    transitionCounter / transitionTime);
            yield return null;
        }
        
        introCamera.enabled = false;
        gameCamera.enabled = true;
        menuCanvas.gameObject.SetActive(false);
    }
}
