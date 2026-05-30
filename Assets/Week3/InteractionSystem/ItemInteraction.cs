/*
    Item interaction class which inherits from master class. 

    When the item is collected it is added to player's inventory, then, optionaly, can be destroyed.
*/

using System.Collections;
using UnityEngine;

public class ItemInteraction : InteractableMasterClass
{

    [SerializeField] private bool destroyObject = true;
    [SerializeField] private float timeBeforeDestorying = 2.5f;
    private Coroutine deactivateCoroutine;
    public override void InteractAction(GameObject player)
    {
        base.InteractAction(player);

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        inventory.AddItemToInventory(ID, Name);

        Debug.Log("Added " + Name + " to inventory!");

        if (destroyObject)
            deactivateCoroutine = StartCoroutine(DeactivateItem(this));

    }

    private IEnumerator DeactivateItem(ItemInteraction interaction)
    {
        foreach (var rend in interaction.GetComponentsInChildren<Renderer>(true))
            rend.enabled = false;

        foreach (var col in interaction.GetComponentsInChildren<Collider>(true))
            col.enabled = false;

        this.interactable = false;

        yield return new WaitForSeconds(timeBeforeDestorying);

        interaction.gameObject.SetActive(false);

        Destroy(this.gameObject);

        deactivateCoroutine = null;

    }
}
