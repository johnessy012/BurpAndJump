using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

    public int SpaceBetweenBlocks = 2;
    public int _currentItemsCreated;

    public bool CanCreateItem = true;

    public void ItemCreated()
    {
        _currentItemsCreated++;
        if (_currentItemsCreated >= SpaceBetweenBlocks)
        {
            CanCreateItem = !CanCreateItem;
            _currentItemsCreated = 0;
        }
    }
}
