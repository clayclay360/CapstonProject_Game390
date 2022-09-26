using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    //Holds item ids for held items
    int main_hand_id;
    int off_hand_id;

    //quickref for whether hands are full
    bool hands_full;

    // Start is called before the first frame update
    void Start()
    {
        main_hand_id = 0;
        off_hand_id = 0;

        hands_full = false;
    }

    // Update is called once per frame
    void Update()
    {
        //on press throw/swap key, perform action based on hand capacity
        if (Input.GetKeyDown("f"))
        {
            if (hands_full)
            {
                swap_hands();
            }
            else
            {
                throw_knife();
            }
        }
    }

    //if hands are not full, pick up item
    //if hands become full, flip quickref
    public void pickup_item(int itemID)
    {
        if (!hands_full)
        {
            if (main_hand_id == 0)
            {
                main_hand_id = itemID;
            }
            else
            {
                off_hand_id = itemID;
                hands_full = true;
            }
        }
    }


    //public call for item usage interactions
    public void use_item()
    {
        if (!hands_full)
        {
            main_hand_id = 0;
        }
        else
        {
            main_hand_id = off_hand_id;
            off_hand_id = 0;
            hands_full = false;
        }
    }

    //swaps position of held items
    void swap_hands()
    {
        int temp_id = main_hand_id;
        main_hand_id = off_hand_id;
        off_hand_id = temp_id;
    }

    //placeholder knife throw function
    public void throw_knife()
    {
        
    }
}
