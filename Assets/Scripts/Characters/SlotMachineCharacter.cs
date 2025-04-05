using UnityEngine;

public class SlotMachineCharacter : BaseCharacter
{
    protected SlotMachine slotMachine;

    protected override void Start()
    {
        base.Start();// Initializes health, UI, etc.
        slotMachine = GetComponent<SlotMachine>();

        if (slotMachine == null)
        {
            Debug.LogError("SlotMachine component missing on " + gameObject.name);
        }
    }

    public override void Attack(BaseCharacter target)
    {
        if (slotMachine == null)
        {
            Debug.LogError("SlotMachine component missing!");
            return;
        }

        slotMachine.Spin();// Spin the slots
        int damage = slotMachine.CalculateTotal();// Get damage value

        Debug.Log($"{gameObject.name} attacks {target.gameObject.name} for {damage} damage!");
        target.TakeDamage(damage);// Apply damage to the target
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);// Applies damage + updates UI
    }
}
