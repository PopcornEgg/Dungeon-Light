using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MUIScrollView : MonoBehaviour
{
    RectTransform content;//内容
    GridLayoutGroup grid;
    ScrollRect scrollRect;

    public RectOffset padding = new RectOffset(0, 0, 0, 0);
    public Vector2 cellSize = Vector2.one;
    public Vector2 spacing = Vector2.one;
    public GridLayoutGroup.Constraint constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    public int constraintCount = 1;
    public bool horizontal = false;
    public bool vertical = false;

    void Start () {

        scrollRect = GetComponent<ScrollRect>();

        if (scrollRect != null) {

            scrollRect.horizontal = horizontal;
            scrollRect.vertical = vertical;
            content = scrollRect.content;

            grid = content.GetComponent<GridLayoutGroup>();
            grid.spacing = spacing;
            grid.padding = padding;
            grid.constraint = constraint;
            grid.constraintCount = constraintCount;
        }
    }

    public void AddItem(GameObject obj)
    {
        if (obj != null) {
            obj.transform.SetParent(content, false);
        }
    }
    public GameObject GetItem(int idx)
    {
        if (idx >= 0 && idx < content.childCount)
            return content.GetChild(idx).gameObject;

        return null;
    }
    public void DelItem(int idx, bool destory = true)
    {
        GameObject obj = GetItem(idx);
        if (obj != null)
        {
            obj.SetActive(false);
            if (destory)
            {
                DestroyImmediate(obj);
            }
        }
    }
}
