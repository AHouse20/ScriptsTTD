using UnityEngine;
using TurnBasedStrategyFramework.Unity.Units;
using TurnBasedStrategyFramework.Common.Units;
using System;
using System.Collections.Generic;
using TurnBasedStrategyFramework.Unity.Utilities;
using TurnBasedStrategyFramework.Common.Units.Abilities;
using UnityEngine.Events;
using TurnBasedStrategyFramework.Common.Cells;
public class Character : TTDUnit
{
    public AbilityAddedEvent OnAbilityAdded = new AbilityAddedEvent();

    private Animator animator;
    public CharacterDetails characterDetails;
    public List<DiceAbility> abilities;
    private bool isSelected = false;
    public override void Initialise(UnitDetails _characterDetails)
    {
        characterDetails = _characterDetails as CharacterDetails;
        base.Initialise(characterDetails);
        MaxMovementPoints = characterDetails.stamina;
        MovementPoints = MaxMovementPoints;
        foreach(AbilityDetailsSO ability in characterDetails.startingAbilities)
        {
            if (ability != null)
            {
                AddAbility(ability);
            }
        }

        if(model.GetComponent<Animator>()!= null)
        {
            animator = model.GetComponent<Animator>();
        }
        else if(model.GetComponentInChildren<Animator>()!= null)
        {
            animator = model.GetComponentInChildren<Animator>();
        }

        if(animator != null) animator.runtimeAnimatorController = characterDetails.animator;
    }

    public void AddAbility(AbilityDetailsSO abilityDetails)
    {
        GameObject newAbilityGO = Instantiate(new GameObject(), this.transform);
        newAbilityGO.AddComponent<DiceAbility>();
        DiceAbility newAbility = (DiceAbility)newAbilityGO.GetComponent<DiceAbility>();
        newAbility.name = abilityDetails.abilityName;
        newAbility.Initialise(abilityDetails);
        newAbility.UnitReference = this;
        newAbility.characterReference = this;
        abilities.Add(newAbility);
        newAbility.OnAbilityUsed.AddListener(OnAbilityUsed);
        RegisterAbility(newAbility, GameManager.Instance.gridController);
        OnAbilityAdded.Invoke(newAbility);
    }
    private void OnEnable()
    {
        UnitLeftCell += OnUnitLeftCell;
        UnitAttacked += OnUnitAttacked;
        UnitSelected += OnUnitSelected;
        UnitDeselected += OnUnitDeselected;
        HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        UnitLeftCell -= OnUnitLeftCell;
        UnitAttacked -= OnUnitAttacked;
        UnitSelected -= OnUnitSelected;
        UnitDeselected -= OnUnitDeselected;
        HealthChanged -= OnHealthChanged;
    }

    private void LookAtCell(ICell cell)
    {
        Vector3 lookVector = cell.WorldPosition.ToVector3();
        lookVector.y = model.transform.position.y;
        model.transform.LookAt(lookVector);
    }

    private void OnAbilityUsed(DiceAbility ability, AbilityUsedEventArgs args)
    {
        animator.SetTrigger("Attack");
        LookAtCell(args.targetCell);
    }

    private void OnHealthChanged(HealthChangedEventArgs args)
    {
        if(args.HealthChangeAmount < 0)
        {
            AudioManager.Instance.PlayOneShot(characterDetails.hurtSound, transform.position);
        }
    }

    private void OnUnitDeselected(IUnit unit)
    {
        animator.SetBool("Ready", false);
        isSelected = false;
    }

    private void OnUnitSelected(IUnit unit)
    {
        if (!isSelected)
        {
            AudioManager.Instance.PlayOneShot(characterDetails.readySound, transform.position);
            animator.SetBool("Ready", true);
            CharacterUI.Instance.Initialise(this);
            isSelected = true;
        }

    }

    private void OnUnitAttacked(UnitAttackedEventArgs args)
    {
        animator.SetTrigger("TakeDamage");
    }
    private void OnUnitLeftCell(UnitChangedGridPositionEventArgs args)
    {
        LookAtCell(args.EnteredCell);
    }
}
