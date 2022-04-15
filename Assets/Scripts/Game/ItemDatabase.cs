using UnityEngine;

//Items are read from the file containing the items. And it is transferred into the game.
public class ItemDatabase
{
   public static Item[] Items{get; private set;}

   [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() => Items = Resources.LoadAll<Item>(path:"Items/");
}
