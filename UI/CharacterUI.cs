using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterUI : SingletonMonobehaviour<CharacterUI>
{
    [SerializeField] private Transform abilitiesParent;
    [SerializeField] private AbilityDisplay abilityDisplayPrefab;
    [SerializeField] private List<AbilityDisplay> displayedAbilities = new List<AbilityDisplay>();
    [SerializeField] private Character characterReference;
    public void Initialise(Character character)
    {
        if(characterReference == character)
        {
            return;
        }
        characterReference = character;
        if(displayedAbilities.Count > 0)
        {
            while (displayedAbilities.Count > 0)
            {
                displayedAbilities[0].UnslotDice();
                Destroy(displayedAbilities[0].gameObject);
                displayedAbilities.RemoveAt(0);
            }
        }
        foreach(DiceAbility ability in character.abilities)
        {
            AbilityDisplay newDisplay = Instantiate(abilityDisplayPrefab,abilitiesParent);
            newDisplay.Initialise(ability);
            displayedAbilities.Add(newDisplay);
        }
    }
}
