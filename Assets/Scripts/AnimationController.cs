using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance;
    private Sequence _sequence;

    [Space]
    [Header("Win Text")]
    public float durationText;
    public string targetText = "You win : ";
    public Text textUI;

    [Space]
    public float durationSots = 1f;
    
    [Space]
    [Header("Move Cube")]
    public float durationCube = 1f;
    public PathType pathType;
    public PathMode pathMode;
    public RectTransform[] cubes;
    public float[] path;
    
    [Space]
    public Image[] decorationCube;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //cube.DOPath(path, duration, pathType, pathMode, 10);
        StartCoroutine(DecorationCubes());
        MoveCube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCube()
    {
        _sequence = DOTween.Sequence();

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Image>().color = GetRandomColor();

            _sequence.Append(cubes[i].DOAnchorPosY(path[0], durationCube, true));
            _sequence.Join(cubes[i].DORotate(new Vector3(0, 0, 90), durationCube));
            _sequence.Join(cubes[i].DOScale(new Vector3(2, 2, 0), durationCube));
            _sequence.AppendInterval(0.1f);
            _sequence.Append(cubes[i].DOAnchorPosY(path[1], durationCube, true));
            _sequence.Join(cubes[i].DORotate(new Vector3(0, 0, 0), durationCube));
            _sequence.Join(cubes[i].DOScale(new Vector3(1, 1, 0), durationCube)).OnComplete(MoveCube);
        }
    }

    public void WinGame(string targetString)
    {
        string text = targetText + targetString + "$";
        string txt = "";

        DOTween.To(
            () => txt,
            x => txt = x,
            text, durationText
            ).OnUpdate(() => textUI.text = txt);
    }

    IEnumerator DecorationCubes()
    {
        while (true)
        {
            for (int i = 0; i < decorationCube.Length; i++)
            {
                Color color = GetRandomColor();
                decorationCube[i].DOColor(color, durationCube);
            }
            
            yield return  new WaitForFixedUpdate();
        }
    }
    
    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }

    public void RotationSlots(Transform obj)
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(obj.transform.DORotate(new Vector3(180, 0, 0), durationSots, RotateMode.Fast));
        _sequence.PrependInterval(0.01f);
        _sequence.Append(obj.transform.DORotate(new Vector3(0, 0, 0), durationSots, RotateMode.Fast));
    }
    
}
