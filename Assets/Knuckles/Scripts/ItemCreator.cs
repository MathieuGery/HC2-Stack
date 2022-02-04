using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection { X, Z }

public class ItemCreator : MonoBehaviour
{
    [SerializeField]
    private Item itemPrefabLevel1;
    [SerializeField]
    private Item itemPrefabLevel2;
    [SerializeField]
    private MoveDirection direction;

    public void CreateItem(int level = 1)
    {
        var item = Instantiate(level == 1 ? itemPrefabLevel1 : itemPrefabLevel2);
        if (Item.LastItem != null && Item.LastItem.gameObject != GameObject.Find("Base"))
        {
            float x = direction == MoveDirection.X ? transform.position.x : Item.LastItem.transform.position.x;
            float z = direction == MoveDirection.Z ? transform.position.z : Item.LastItem.transform.position.z;

            item.transform.position = new Vector3(x, Item.LastItem.transform.position.y + (level == 1 ? itemPrefabLevel1 : itemPrefabLevel2).transform.localScale.y, z);
        } else {
            item.transform.position = transform.position;
        }

        item.Direction = direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, itemPrefabLevel1.transform.localScale);
    }
}