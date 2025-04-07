using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;

public class GagManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> gags = new();
    [SerializeField] private float tweenTime = 1f;
    [SerializeField] private float livingTime;
    [SerializeField] private float randomTimeMin;
    [SerializeField] private float randomTimeMax;
    private GagPosition gagPosition;
    private float xLeft, xRight, yDown, yUp;
    private RectTransform currentGag;
    private int currentGagIndex;
    private float randomTimer;
    private float livingTimer;
    [SerializeField] private UnityEvent<int> OnEasterGagClick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        livingTimer = livingTime;
        GetRandomTime();

        var parentRect = transform.GetComponent<RectTransform>();
        yUp = parentRect.rect.height/2;
        yDown = -parentRect.rect.height/2;
        xRight = parentRect.rect.width/2;
        xLeft = -parentRect.rect.width/2;
        
        foreach (var gag in gags)
        {
            gag.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (randomTimer > 0)
        {
            randomTimer -= Time.deltaTime;
            return;
        }

        if (currentGag == null)
        {
            GetRandomGag();
        }

        if (livingTimer > 0)
        {
            livingTimer -= Time.deltaTime;
            return;
        }

        currentGag.gameObject.SetActive(false);
        currentGag.GetComponent<Button>().onClick.RemoveAllListeners();
        livingTimer = livingTime; // Reset the living timer to living time
        GetRandomTime();
        currentGag = null; // Set currentGag to null to allow for a new gag to be selected
    }

    private void GetRandomGag()
    {
        currentGagIndex = Random.Range(0, gags.Count);
        currentGag = gags[currentGagIndex];
        currentGag.anchoredPosition = GetRandomXY();
        currentGag.gameObject.SetActive(true);
        currentGag.GetComponent<Button>().onClick.AddListener(GagClicked);
        MoveGag();
    }

    private void GagClicked()
    {
        OnEasterGagClick?.Invoke(currentGagIndex);
        currentGag.GetComponent<Button>().onClick.RemoveAllListeners();
        gags.Remove(currentGag);
        Destroy(currentGag.gameObject);
        currentGag = null; // Set currentGag to null to allow for a new gag to be selected
        GetRandomTime();
        livingTime = 30f; // Reset the living time to 30 seconds
    }

    Vector3 GetRandomXY()
    {
        float width = currentGag.rect.width * currentGag.localScale.x;
        float height = currentGag.rect.height * currentGag.localScale.y;
        float newX = Random.Range(xLeft + width/2, xRight - width/2);
        float newY = Random.Range(yDown + height/2, yUp + height/2);
        if (newY > yUp-height/2-10)
        {
            newY = yUp + height/2;
            gagPosition = GagPosition.Up;
            return new Vector3(newX, newY, 0);
        }
        if (Random.Range(0, 99) < 50)
        {
            newX = xLeft - width/2;
            gagPosition = GagPosition.Left;
        }
        else
        {
            newX = xRight + width/2;
            gagPosition = GagPosition.Right;
        }
        return new Vector3(newX, newY, 0);
    }

    void GetRandomTime()
    {
        randomTimer = (float)Random.Range(3, 10);
    }

    void MoveGag()
    {
        float width = currentGag.rect.width * currentGag.localScale.x;
        float height = currentGag.rect.height * currentGag.localScale.y;
        switch (gagPosition)
        {
            case GagPosition.Left:
                currentGag.DOAnchorPosX(currentGag.anchoredPosition.x + width, tweenTime);
                Debug.Log(currentGag.anchoredPosition.x);
                Debug.Log(width);
                Debug.Log(currentGag.anchoredPosition.x + width);
                break;
            case GagPosition.Right:
                currentGag.DOAnchorPosX(currentGag.anchoredPosition.x - width, tweenTime);
                Debug.Log(currentGag.anchoredPosition.x);
                Debug.Log(width);
                Debug.Log(currentGag.anchoredPosition.x - width);
                break;
            case GagPosition.Up:
                currentGag.DOAnchorPosY(currentGag.anchoredPosition.y - height, tweenTime);
                Debug.Log(currentGag.anchoredPosition.y);
                Debug.Log(height);
                Debug.Log(currentGag.anchoredPosition.y + height);
                break;
            default:
                break;
        };
    }
 
}

enum GagPosition
{
    Left,
    Right,
    Up,
}