using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum MoveDirection { X, Z }

public class Item : MonoBehaviour
{
    public static Item CurrentItem { get; private set; }
    public static Item LastItem { get; private set; }
    public MoveDirection Direction { get; set; }

    [SerializeField]
    private float moveSpeed = 1f;
    
    private void OnEnable()
    {
        if (LastItem == null)
            LastItem = GameObject.Find("Base").GetComponent<Item>();
        CurrentItem = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
        transform.localScale = new Vector3(LastItem.transform.localScale.x, transform.localScale.y, LastItem.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal bool Stop()
    {
        float initialMoveSpeed = moveSpeed;
        moveSpeed = 0;
        float hangover = (Direction == MoveDirection.Z ?
            transform.position.z - LastItem.transform.position.z :
            transform.position.x - LastItem.transform.position.x);

        if (Mathf.Abs(hangover) >= (Direction == MoveDirection.Z ? LastItem.transform.localScale.z : LastItem.transform.localScale.x))
        {
            this.gameObject.AddComponent<Rigidbody>();
            this.gameObject.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
            Destroy(this.gameObject, 1f);
            LastItem = null;
            CurrentItem = null;
            return true;
        }

        if (initialMoveSpeed != 0)
        {
            float direction = hangover > 0 ? 1f : -1f;
            if (Direction == MoveDirection.Z)
                SplitItemOnZ(hangover, direction);
            else
                SplitItemOnX(hangover, direction);
        } else
        {
            YsoCorp.GameUtils.YCManager.instance.OnGameStarted(Game.Level);
        }

        LastItem = this;
        return false;
    }

    private void SplitItemOnX(float hangover, float direction)
    {
        float newXSize = LastItem.transform.localScale.x - Mathf.Abs(hangover);
        float fallingItemSize = transform.localScale.x - newXSize;

        float newXPosition = LastItem.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float itemEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingItemXPosition = itemEdge + fallingItemSize / 2f * direction;

        DropItemEdge(fallingItemXPosition, fallingItemSize);
    }


    private void SplitItemOnZ(float hangover, float direction)
    {
        float newZSize = LastItem.transform.localScale.z - Mathf.Abs(hangover);
        float fallingItemSize = transform.localScale.z - newZSize;

        float newZPosition = LastItem.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float itemEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingItemZPosition = itemEdge + fallingItemSize / 2f * direction;

        DropItemEdge(fallingItemZPosition, fallingItemSize);
    }

    private void DropItemEdge(float fallingItemZPosition, float fallingItemSize)
    {
        var item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (Direction == MoveDirection.Z)
        {
            item.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingItemSize);
            item.transform.position = new Vector3(transform.position.x, transform.position.y, fallingItemZPosition);
        } else {
            item.transform.localScale = new Vector3(fallingItemSize, transform.localScale.y, transform.localScale.z);
            item.transform.position = new Vector3(fallingItemZPosition, transform.position.y, transform.position.z);
        }

        item.AddComponent<Rigidbody>();
        item.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(item.gameObject, 1f);
    }

    private void Update()
    {
        if (Direction == MoveDirection.Z)
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        else
            transform.position += transform.right * Time.deltaTime * moveSpeed;
    }
}
