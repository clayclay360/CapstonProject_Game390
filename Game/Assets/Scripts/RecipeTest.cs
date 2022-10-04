using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeTest : MonoBehaviour
{
    // NEED A SPECIFIC WAY TO DISTINGUISH DIFFERENT INVENTORY ITEMS AND UTILITIES
    public Dictionary<float, KeyValuePair<InventoryItem, Utility>> recipeSteps = new Dictionary<float, KeyValuePair<InventoryItem, Utility>>(); //List of recipe steps, ordered by subsection
    public Dictionary<KeyValuePair<InventoryItem, Utility>, float> componentsToNumber = new Dictionary<KeyValuePair<InventoryItem, Utility>, float>();
    public Dictionary<float, float> stepRequirements = new Dictionary<float, float>(); //Key == step #, Value == step requirement #
    public Dictionary<float, bool> completedSteps = new Dictionary<float, bool>(); // step #, completed true/false
    public Dictionary<InventoryItem, Utility> availableSteps = new Dictionary<InventoryItem, Utility>();

    private void Start()
    {
        recipeSteps.Add(1.0f, new KeyValuePair<InventoryItem, Utility>());
    }

    private void CheckStepCompletion(InventoryItem playerInvItem, Utility interactingUtil)
    {
        foreach (KeyValuePair<InventoryItem, Utility> step in availableSteps)
        {
            if (step.Key == playerInvItem && step.Value == interactingUtil)
            {
                completedSteps[componentsToNumber[new KeyValuePair<InventoryItem, Utility>(playerInvItem, interactingUtil)]] = true;
                availableSteps.Remove(step.Key);
                CheckNextStep();
            }
        }
    }

    private void CheckNextStep()
    {
        foreach (KeyValuePair<float, float> stepReq in stepRequirements)
        {
            if (completedSteps[stepReq.Value])
            {
                availableSteps.Add(recipeSteps[stepReq.Key].Key, recipeSteps[stepReq.Key].Value);
            }
        }
    }
}
