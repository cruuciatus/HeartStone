using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardController CC;

    Camera MainCamera;
    Vector3 offset;
    public Transform DefaultParent;
    public Transform DefaultTempCardParent;
    GameObject TempCardGO;
    public GameManager GameManager;
    public bool IsDraggable;

    int startID;

    public void Awake()
    {
        MainCamera = Camera.allCameras[0];
        TempCardGO = GameObject.Find("TempCardGO");
        GameManager = FindObjectOfType<GameManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);

        DefaultParent = DefaultTempCardParent = transform.parent;

        IsDraggable = GameManager.Instance.IsPlayerTurn && ((DefaultParent.GetComponent<DropPlace>().Type == FieldType.selfHand && GameManager.Instance.CurrentGame.Player.Mana >= CC.Card.Manacost)
             || (DefaultParent.GetComponent<DropPlace>().Type == FieldType.selfField && CC.Card.CanAttack));
        


        if (!IsDraggable)
            return;
        startID = transform.GetSiblingIndex();

        if (CC.Card.IsSpell || CC.Card.CanAttack)
            GameManager.Instance.HighLightTargets(CC, true);



        TempCardGO.transform.SetParent(DefaultParent);
        TempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable)
            return;
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;
       
        if (!CC.Card.IsSpell)
        {
            if (TempCardGO.transform.parent != DefaultTempCardParent)
                TempCardGO.transform.SetParent(DefaultTempCardParent);

            if (DefaultParent.GetComponent<DropPlace>().Type != FieldType.selfField)
                CheckPosition();
        }


    }



    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable)
            return;


        GameManager.Instance.HighLightTargets(CC, false);


        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());
        TempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
        TempCardGO.transform.localPosition = new Vector3(4337, 0);
    }

    void CheckPosition()
    {
        int newIndex = DefaultTempCardParent.childCount;

        for (int i = 0; i < DefaultTempCardParent.childCount; i++)
        {
            if (transform.position.x < DefaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;
                if (TempCardGO.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }

        }
        if (TempCardGO.transform.parent == DefaultParent)
        {
            newIndex = startID;
        }
        TempCardGO.transform.SetSiblingIndex(newIndex);

    }
    public void MoveToField(Transform field)
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.DOMove(field.position, .5f);
    }

    public void MoveToTarget(Transform target)
    {
        StartCoroutine(MoveToTargetCor(target));
    }
    IEnumerator MoveToTargetCor(Transform target)
    {
        Vector3 pos = transform.position;
        Transform parent = transform.parent;
        int index = transform.GetSiblingIndex();
        if (transform.parent.GetComponent<HorizontalLayoutGroup>())
        {
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }
       

        transform.SetParent(GameObject.Find("Canvas").transform);

        transform.DOMove(target.position, .25f);
        yield return new WaitForSeconds(.25f);
        transform.DOMove(pos, .25f);
        yield return new WaitForSeconds(.25f);

        transform.SetParent(parent);
        transform.SetSiblingIndex(index);
        if (transform.parent.GetComponent<HorizontalLayoutGroup>())
        {
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }
    }
            
}
