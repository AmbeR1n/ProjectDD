using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;

public class GagManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> gags = new();
    [SerializeField] private readonly float tweenTime = 1f;
    private GagPosition gagPosition;
    private float xLeft, xRight, y;
    private RectTransform currentGag;
    private int currentGagIndex;
    private float randomTime;
    private float livingTime;
    public UnityEvent<int> OnEasterGagClick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetRandomTime();
        livingTime = 30f; // Set the living time to 30 seconds
        y = Screen.height; // Set the y position to be above the screen height
        xLeft = 0; // Set the left x position to be 0
        xRight = Screen.width; // Set the right x position to be the screen width
        foreach (var gag in gags)
        {
            gag.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (randomTime > 0)
        {
            randomTime -= Time.deltaTime;
        }
        else
        {
            if (currentGag == null || !currentGag.gameObject.activeSelf)
            {
                GetRandomGag();
            }
            else
            {
                livingTime -= Time.deltaTime;
                if (livingTime <= 0)
                {
                    currentGag.gameObject.SetActive(false);
                    currentGag.GetComponent<Button>().onClick.RemoveAllListeners();
                    livingTime = 30f; // Reset the living time to 30 seconds
                    GetRandomTime();
                    currentGag = null; // Set currentGag to null to allow for a new gag to be selected
                }
            }
        }
    }

    private void GetRandomGag()
    {
        currentGagIndex = Random.Range(0, gags.Count);
        currentGag = gags[currentGagIndex];
        currentGag.transform.position = GetRandomXY();
        MoveGag();
        currentGag.gameObject.SetActive(true);
        currentGag.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnEasterGagClick?.Invoke(currentGagIndex);
            currentGag.GetComponent<Button>().onClick.RemoveAllListeners();
            gags.Remove(currentGag);
            Destroy(currentGag.gameObject);
            currentGag = null; // Set currentGag to null to allow for a new gag to be selected
            GetRandomTime();
            livingTime = 30f; // Reset the living time to 30 seconds
        });
    }
    Vector3 GetRandomXY()
    {
        float newX = Random.Range(xLeft-currentGag.rect.width/2, xRight + currentGag.rect.width/2);
        float newY = Random.Range(280 + currentGag.rect.height, y+currentGag.rect.height/2);
        if (newY > y-currentGag.rect.height/2-50)
        {
            newY = y + currentGag.rect.height;
            gagPosition = GagPosition.Up;
            return new Vector3(newX, newY, 0);
        }
        if (Random.Range(0, 99) < 50)
        {
            newX = xLeft - currentGag.rect.width/2;
            gagPosition = GagPosition.Left;
        }
        else
        {
            newX = xRight + currentGag.rect.width/2;
            gagPosition = GagPosition.Right;
        }
        return new Vector3(newX, newY, 0);
    }

    void GetRandomTime()
    {
        randomTime = (float)Random.Range(30, 60);
    }

    void MoveGag()
    {
        switch (gagPosition)
        {
            case GagPosition.Left:
                currentGag.transform.DOMoveX(currentGag.transform.position.x + currentGag.rect.width, tweenTime);
                break;
            case GagPosition.Right:
                currentGag.transform.DOMoveX(currentGag.transform.position.x - currentGag.rect.width, tweenTime);
                break;
            case GagPosition.Up:
                currentGag.transform.DOMoveY(currentGag.transform.position.y - currentGag.rect.height, tweenTime);
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