using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public TypeCard cardType;
    
    public int[] coinMultPerLevel;
    public int[] hitsMultPerLevel;
    public int[] scoreMultPerLevel;
    public int[] upgradeCostPerLevel;
   
    public int upgradeLevel;
    private int originalUpgradeLevel;

    
    
    // Como los ScriptablesObjects no guardan los datos en memoria, si no en
    // el Asset, quería reiniciarlos siempre a 0 para probar en el editor.
    // Sin embargo, si este codigo se ejecuta tambien se reinicia para el juego
    // y por tanto no se guardan los datos. Espero que no lo tengas mucho en cuenta :)
    
    // private void OnEnable()
    // {
    //     originalUpgradeLevel = upgradeLevel;
    // }
    // private void OnDisable()
    // {
    //     upgradeLevel = originalUpgradeLevel;
    // }
}
