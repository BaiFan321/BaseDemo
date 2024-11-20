using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollList : MonoBehaviour
{
    public InfiniteScrollDirection scrollDirection;

    //content
    private RectTransform content;

    //可视范围，viewPort的父物体
    private RectTransform _viewRange;

    public GameObject SlotPrefab; 

    //每个SlotBundle内Slot的数量
    [SerializeField]private int BundleSlotCount = 5;

    public int SlotTotalCount => itemUIList.Count;

    //每个Slot的大小
    [SerializeField] private Vector2 cellSize = new Vector2(200, 300);
    //每个Slot的间隔
    [SerializeField] private Vector2 cellSpace = new Vector2(20, 30);

    //每个SlotBundle的间隔
    private Vector2 slotBundleSpace;

    private readonly Vector2 HorizontalAnchorMin = new Vector2(0, 0);
    private readonly Vector2 HorizontalAnchorMax = new Vector2(0, 1);
    private readonly Vector2 VerticalAnchorMin = new Vector2(0, 1);
    private readonly Vector2 VerticalAnchorMax = new Vector2(1, 1);

    private readonly Vector2 slotPivot = new Vector2(0, 1);
    private readonly Vector2 slotAnchorMin = new Vector2(0, 1);
    private readonly Vector2 slotAnchorMax = new Vector2(0, 1);

    public List<ItemUI> itemUIList;

    //Content里SlotBundle的数量
    public int SlotBundleCount
    {
        get
        {
            return SlotTotalCount % BundleSlotCount == 0 ? SlotTotalCount / BundleSlotCount : SlotTotalCount / BundleSlotCount + 1;
        }
    }

    private readonly LinkedList<SlotsBundle> slotsBundlesList = new();
    private readonly Queue<SlotsBundle> _slotsBundlesPool = new Queue<SlotsBundle>();


    private void Awake()
    {
        SlotPrefab = Resources.Load<GameObject>("Prefabs\\UI\\BagPanelPrefab\\Slot");
        _viewRange = gameObject.GetComponent<RectTransform>();
        Transform viewPort = _viewRange.Find("Viewport");
        content = viewPort.Find("Content").GetComponent<RectTransform>();
    }

    //初次加载时调用
    public void Initialize(List<ItemUI> itemUIList)
    {
        this.itemUIList = itemUIList;
        slotBundleSpace = new Vector2(cellSize.x + cellSpace.x, cellSize.y + cellSpace.y);

        CaculateContentSize();
        UpdateDisplay();
    }

    public void DataChange(List<ItemUI> itemUIList)
    {
        this.itemUIList = itemUIList;
        CaculateContentSize();
        foreach(SlotsBundle slotsBundle in slotsBundlesList)
        {
            ReleaseSlotsBundle(slotsBundle);
        }
        slotsBundlesList.Clear();
        UpdateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        RemoveHead();
        RemoveTail();
        if(slotsBundlesList.Count == 0)
        {
            RefreshAllSlotBundle();
        }
        else
        {
            AddHead();
            AddTail();
        }
    }

    #region 调整Content中的SlotsBundle

    private void RefreshAllSlotBundle()
    {
        int itemCount = SlotTotalCount;
        Vector2 viewRangeSize = _viewRange.sizeDelta;
        if (scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            Vector2 leftPos = -content.anchoredPosition;
            Vector2 rightPos = new Vector2(leftPos.x + viewRangeSize.x, leftPos.y);
            int startIndex = GetIndex(leftPos);
            int endIndex = GetIndex(rightPos);
            for (int i = startIndex; i < endIndex && i < itemCount; i++)
            {
                Vector2 pos = new Vector2(i * slotBundleSpace.x, content.anchoredPosition.y);
                var bundle = GetSlotsBundle(i, pos, cellSize, cellSpace);
                slotsBundlesList.AddLast(bundle);
            }
        }
        else if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            Vector2 topPos = -content.anchoredPosition;
            Vector2 bottomPos = new Vector2(topPos.x, topPos.y - viewRangeSize.y);
            int startIndex = GetIndex(topPos);
            int endIndex = GetIndex(bottomPos);
            for(int i = startIndex; i < endIndex && i < itemCount; i++)
            {
                Vector2 pos = new Vector2(content.anchoredPosition.x, -i * slotBundleSpace.y);
                var bundle = GetSlotsBundle(i, pos, cellSize, cellSpace);
                slotsBundlesList.AddLast(bundle);
            }
            //Debug.Log("初次加载完成");
        }
    }

    private void AddHead() {
        SlotsBundle bundle = slotsBundlesList.First.Value;

        Vector2 offset = default;

        if(scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            offset = new Vector2(-slotBundleSpace.x, 0);
        }else if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            offset = new Vector2(0, slotBundleSpace.y);
        }

        Vector2 newHeadBundlePos = bundle.position + offset;

        while (OnViewRange(newHeadBundlePos))
        {
            int caculateIndex = GetIndex(newHeadBundlePos);
            int index = bundle.index - 1;
            if (index < 0) break;
            if(caculateIndex != index)
            {
                Debug.LogError("索引值不相等");
            }
            bundle = GetSlotsBundle(index, newHeadBundlePos, cellSize, cellSpace);
            slotsBundlesList.AddFirst(bundle);
            newHeadBundlePos = bundle.position + offset;
        }
    }

    private void RemoveHead() {
        if (slotsBundlesList.Count == 0)
            return;

        if (scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            SlotsBundle bundle = slotsBundlesList.First.Value;
            while (OnViewRangeLeft(bundle.position))
            {
                ReleaseSlotsBundle(bundle);
                slotsBundlesList.RemoveFirst();
                if (slotsBundlesList.Count == 0)
                    break;
                bundle = slotsBundlesList.First.Value;
            }
        }
        else if (scrollDirection == InfiniteScrollDirection.Vertical)
        {
            SlotsBundle bundle = slotsBundlesList.First.Value;
            while (AboveViewRange(bundle.position))
            {
                ReleaseSlotsBundle(bundle);
                slotsBundlesList.RemoveFirst();
                if (slotsBundlesList.Count == 0)
                    break;
                bundle = slotsBundlesList.First.Value;
            }
        }
    }

    private void AddTail() {
        SlotsBundle bundle = slotsBundlesList.Last.Value;

        Vector2 offset = default;

        if (scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            offset = new Vector2(slotBundleSpace.x, 0);
        }
        else if (scrollDirection == InfiniteScrollDirection.Vertical)
        {
            offset = new Vector2(0, -slotBundleSpace.y);
        }

        Vector2 newTailBundlePos = bundle.position + offset;
        //Debug.Log("上一个bundle的position = " + bundle.position);

        while (OnViewRange(newTailBundlePos))
        {
            int caculateIndex = GetIndex(newTailBundlePos);
            int index = bundle.index + 1;
            if (index >= SlotBundleCount)
            {
                break;
            }
            if (caculateIndex != index)
            {
                Debug.LogError("索引值不相等");
            }
            bundle = GetSlotsBundle(index, newTailBundlePos, cellSize, cellSpace);
            slotsBundlesList.AddLast(bundle);
            //Debug.Log("添加的tailBundle.position = " + bundle.Slots[1].GetComponent<RectTransform>().position + "  " + bundle.Slots[1].GetComponent<RectTransform>().anchoredPosition);
            newTailBundlePos = bundle.position + offset;
        }
    }

    private void RemoveTail() {

        if (slotsBundlesList.Count == 0)
            return;

        if (scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            SlotsBundle bundle = slotsBundlesList.Last.Value;
            while (OnViewRangeRight(bundle.position))
            {
                ReleaseSlotsBundle(bundle);
                slotsBundlesList.RemoveLast();
                if (slotsBundlesList.Count == 0)
                    break;
                bundle = slotsBundlesList.Last.Value;
            }
        }
        else if (scrollDirection == InfiniteScrollDirection.Vertical)
        {
            SlotsBundle bundle = slotsBundlesList.Last.Value;
            while (UnderViewRange(bundle.position))
            {
                ReleaseSlotsBundle(bundle);
                slotsBundlesList.RemoveLast();
                if (slotsBundlesList.Count == 0)
                    break;
                bundle = slotsBundlesList.Last.Value;
            }
        }
    }

    private bool OnViewRange(Vector2 pos) {
        if(scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            return !OnViewRangeLeft(pos) && !OnViewRangeRight(pos);
        }else if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            return !AboveViewRange(pos) && !UnderViewRange(pos);
        }
        return false;
    }

    private bool AboveViewRange(Vector2 pos) {
        Vector2 relativePos = CaculateRelativePos(pos);
        return relativePos.y > slotBundleSpace.y;
    }

    private bool UnderViewRange(Vector2 pos) {
        Vector2 relativePos = CaculateRelativePos(pos);
        return relativePos.y < -_viewRange.sizeDelta.y;
    }

    private bool OnViewRangeLeft(Vector2 pos) {
        Vector2 relativePos = CaculateRelativePos(pos);
        return relativePos.x < -slotBundleSpace.x;
    }

    private bool OnViewRangeRight(Vector2 pos)
    {
        Vector2 relativePos = CaculateRelativePos(pos);
        return relativePos.x > -_viewRange.sizeDelta.x;
    }

    private void CaculateContentSize() {
        int slotBundleCount = SlotBundleCount;
        if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            content.anchorMin = VerticalAnchorMin;
            content.anchorMax = VerticalAnchorMax;
            content.sizeDelta = new Vector2(content.sizeDelta.x, slotBundleCount * slotBundleSpace.y);
        }else if(scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            content.anchorMin = HorizontalAnchorMin;
            content.anchorMax = HorizontalAnchorMax;
            content.sizeDelta = new Vector2(slotBundleCount * slotBundleSpace.x, content.sizeDelta.y);
        }
    }

    private void RefreshViewRangeData()
    {

    }

    private int GetIndex(Vector2 pos)
    {
        int index = -1;
        if (scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            index = Mathf.RoundToInt(pos.x / slotBundleSpace.x);

        }else if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            index = Mathf.RoundToInt(-pos.y / slotBundleSpace.y);
        }
        return index;
    }

    private Vector2 CaculateRelativePos(Vector2 pos)
    {
        Vector2 relativePos = default;
        if(scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            relativePos = new Vector2(pos.x + content.anchoredPosition.x, pos.y);

        }else if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            relativePos = new Vector2(pos.x, pos.y + content.anchoredPosition.y);
        }
        return relativePos;
    }
    #endregion

    #region 从对象池中获取SlotBundle
    private void ReleaseSlotsBundle(SlotsBundle slotsBundle)
    {
        slotsBundle.Clear();
        _slotsBundlesPool.Enqueue(slotsBundle);  
    }

    private SlotsBundle GetSlotsBundle(int itemIndex, Vector2 position, Vector2 cellSize, Vector2 cellSpace)
    {
        SlotsBundle bundle;
        Vector2 slotOffset = default;
        if(scrollDirection == InfiniteScrollDirection.Horizontal)
        {
            slotOffset = new Vector2(0, -(cellSize.y + cellSpace.y));
        }else if(scrollDirection == InfiniteScrollDirection.Vertical)
        {
            slotOffset = new Vector2(cellSize.x + cellSpace.x, 0);
        }
        if (_slotsBundlesPool.Count == 0)
        {
            bundle = new SlotsBundle(BundleSlotCount);
            bundle.position = position;
            bundle.index = itemIndex;
            int i = itemIndex * BundleSlotCount;
            int length = itemIndex * BundleSlotCount + bundle.Slots.Length;
            for (int j = 0; j < bundle.Slots.Length && i < length; j++, i++)
            {
                bundle.Slots[j] = InstantiateSlot();
                RectTransform rectTransform = bundle.Slots[j].GetComponent<RectTransform>();
                ResetRectTransform(rectTransform);
                rectTransform.anchoredPosition = position + j * slotOffset;
                if(i < 0 || i >= itemUIList.Count)
                {
                    ResetSlotData(bundle.Slots[j], i);
                    continue;
                }
                ResetSlotData(itemUIList, bundle.Slots[j], i);
            }
        }
        else
        {
            bundle = _slotsBundlesPool.Dequeue();
            bundle.position = position;
            bundle.index = itemIndex;
            //Debug.Log($"从对象池取出 bundle{bundle.index} 的position = {bundle.position}");
            int i = itemIndex * BundleSlotCount;
            int length = itemIndex * BundleSlotCount + bundle.Slots.Length;
            for (int j = 0; j < bundle.Slots.Length && i < length; j++, i++)
            {
                RectTransform rectTransform = bundle.Slots[j].GetComponent<RectTransform>();
                ResetRectTransform(rectTransform);
                rectTransform.anchoredPosition = position + j * slotOffset;
                //Debug.Log($"rectTransform.position = {rectTransform.position}");
                bundle.Slots[j].gameObject.SetActive(true);
                if (i < 0 || i >= itemUIList.Count) 
                {
                    ResetSlotData(bundle.Slots[j], i);
                    continue;
                }
                ResetSlotData(itemUIList, bundle.Slots[j], i);
            }
        }
        return bundle;
    }

    private Slot InstantiateSlot()
    {
        GameObject go = Instantiate(SlotPrefab, content);
        Slot slot = go.GetComponent<Slot>();
        //slot.m_id = itemIndex;
        return slot;
    }

    private void ResetSlotData(List<ItemUI> itemUIList, Slot slot, int index)
    {
        if(index < itemUIList.Count)
        {
            slot.LoadItemUI(itemUIList[index], index / BundleSlotCount);
        }
        else
        {
            Debug.Log("Slot数量超出itemList范围");
        }
        
    }

    private void ResetSlotData(Slot slot, int index)
    {
        slot.Clear(index / BundleSlotCount);
    }

    private void ResetRectTransform(RectTransform rectTransform)
    {
        rectTransform.pivot = slotPivot;
        rectTransform.anchorMin = slotAnchorMin;
        rectTransform.anchorMax = slotAnchorMax;
    }

    #endregion

}
