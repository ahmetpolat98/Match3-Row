using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    private Item _item;

    public Item Item{
        get => _item;
        set{
            if (_item == value) return;
            _item = value;
            icon.sprite = _item.sprite;
        }
    }
    public Image icon;
    public Button button;

    // public Tile Left => x > 0 ? Board.Instance.tiles[y, x-1] : null;
    // public Tile Top => y > 0 ? Board.Instance.tiles[y-1, x] : null;
    // public Tile Right => x < Board.Instance.width - 1 ? Board.Instance.tiles[y, x+1] : null;
    // public Tile Bottom => y < Board.Instance.height - 1 ? Board.Instance.tiles[y+1, x] : null;

    // public Tile[] Neighbours => new[]{
    //     Left,
    //     Top,
    //     Right,
    //     Bottom,
    // };

    private void Start() => button.onClick.AddListener(call:() => Board.Instance.Select(tile:this));
        
    
    
}
