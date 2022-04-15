using UnityEngine;

[CreateAssetMenu(menuName ="RowMatch/Item")]
public class Item : ScriptableObject //The class that specifies the Game Pieces(blue, yellow etc.).
{
    public int id;
    public int value;
    public Sprite sprite;
}
