using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook: MonoBehaviour
{
    public string theName;
    public GameObject recipeTextbox1;
    public GameObject recipeTextbox2;
    public GameObject recipeTextbox3;
    public GameObject backgroundImage;
    public bool recipeIsOpenP1;
    public int[] pages;
    public int[] steps;
    public int currentStep;
    public int currentPage;

    void Start()
    {
        backgroundImage.SetActive(false); 
        pages = new int[] { 1, 2, 3 };
        steps = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        currentPage = 1;
        currentStep = 2;
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            //variable telling the game that the recipe for player one is open
            recipeIsOpenP1 = true;

            //puts in the information for the first page when the book first opens
            recipeTextbox1.GetComponent<Text>().text = "Johnnie Rico never really intended to join up—and definitely not the infantry. But now that he’s in the thick of it, trying to get through combat training harder than anything he could have imagined, he knows everyone in his unit is one bad move away from buying the farm in the interstellar war the";
            recipeTextbox2.GetComponent<Text>().text = "Johnnie Rico never really intended to join up—and definitely not the infantry. But now that he’s in the thick of it, trying to get through combat training harder than anything he could have imagined, he knows everyone in his unit is one bad move away from buying the farm in the interstellar war the";
            recipeTextbox3.GetComponent<Text>().text = "Johnnie Rico never really intended to join up—and definitely not the infantry. But now that he’s in the thick of it, trying to get through combat training harder than anything he could have imagined, he knows everyone in his unit is one bad move away from buying the farm in the interstellar war the";
            //is checking the current step to determine if it should gray out the first 3 steps on open or not
            if (currentStep >= 1) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

            if (currentStep >= 2) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

            if (currentStep >= 3) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox3.GetComponent<Text>().color = Color.black; }
            //sets the background image to show
            backgroundImage.SetActive(true);

            currentPage = 1;
        }
        else if (Input.GetKeyUp("e"))
        {
            //variable telling the game that the recipe for player one is closed
            recipeIsOpenP1 = false;
            //empties out any information in the textboxes
            recipeTextbox1.GetComponent<Text>().text = " ";
            recipeTextbox2.GetComponent<Text>().text = " ";
            recipeTextbox3.GetComponent<Text>().text = " ";
            //hides the background image
            backgroundImage.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && recipeIsOpenP1)
        {
            if (currentPage != pages.Length - 1)
            {
                if (currentPage == 0)
                {
                    //this is putting in the information for the first page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Johnnie Rico never really intended to join up—and definitely not the infantry. But now that he’s in the thick of it, trying to get through combat training harder than anything he could have imagined, he knows everyone in his unit is one bad move away from buying the farm in the interstellar war the";
                    recipeTextbox2.GetComponent<Text>().text = "Johnnie Rico never really intended to join up—and definitely not the infantry. But now that he’s in the thick of it, trying to get through combat training harder than anything he could have imagined, he knows everyone in his unit is one bad move away from buying the farm in the interstellar war the";
                    recipeTextbox3.GetComponent<Text>().text = "Johnnie Rico never really intended to join up—and definitely not the infantry. But now that he’s in the thick of it, trying to get through combat training harder than anything he could have imagined, he knows everyone in his unit is one bad move away from buying the farm in the interstellar war the";
                    //is checking the current step to determine if it should gray out the first 3 steps while player 1 is clicking through the pages
                    if (currentStep >= 1) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 2) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 3) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }


                }
                else if (currentPage == 1)
                {
                    //this is putting in the information for the second page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Rioting unpredictable false victorious Boromir. Madness watching responsible troublemaker finds farriers dark immortal Simbelmynë themselves. Pointing Hobbit's eavesdropping smash! Disturb skewered limp scattered fried tongues Gaffer rich nudge weed shone. Fool's heir cot blessings rocks what'll Trolls 21 wound? Woodland naught burn deeply concludes father's dare developed. Crawled pockets humble travelers Greenwood hit. Climb cheated squealing quickly Snowbourn shame mist hide awoke yourselves hall. Usurpers mash gracious make Éomund pulled trinket. I think we should get off the road.";
                    recipeTextbox2.GetComponent<Text>().text = "Rioting unpredictable false victorious Boromir. Madness watching responsible troublemaker finds farriers dark immortal Simbelmynë themselves. Pointing Hobbit's eavesdropping smash! Disturb skewered limp scattered fried tongues Gaffer rich nudge weed shone. Fool's heir cot blessings rocks what'll Trolls 21 wound? Woodland naught burn deeply concludes father's dare developed. Crawled pockets humble travelers Greenwood hit. Climb cheated squealing quickly Snowbourn shame mist hide awoke yourselves hall. Usurpers mash gracious make Éomund pulled trinket. I think we should get off the road.";
                    recipeTextbox3.GetComponent<Text>().text = "Rioting unpredictable false victorious Boromir. Madness watching responsible troublemaker finds farriers dark immortal Simbelmynë themselves. Pointing Hobbit's eavesdropping smash! Disturb skewered limp scattered fried tongues Gaffer rich nudge weed shone. Fool's heir cot blessings rocks what'll Trolls 21 wound? Woodland naught burn deeply concludes father's dare developed. Crawled pockets humble travelers Greenwood hit. Climb cheated squealing quickly Snowbourn shame mist hide awoke yourselves hall. Usurpers mash gracious make Éomund pulled trinket. I think we should get off the road.";
                    //is checking the current step to determine if it should gray out the next 3 while player 1 is clicking through the pages
                    if (currentStep >= 4) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 5) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 6) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }

                }

                currentPage++;
            } else
            {
                if (currentPage == 2)
                {
                    //this is putting in the information for the third page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Lucas ipsum dolor sit amet skywalker windu mothma mace greedo obi-wan darth jango wedge mara. Biggs antilles padmé anakin kenobi. Ponda darth vader mothma. Tatooine owen dooku hutt lobot darth baba. Mace coruscant palpatine wicket solo lars kenobi yoda. Mon wookiee leia anakin organa organa moff grievous. Bothan ackbar c-3po qui-gon. Gamorrean antilles calamari padmé boba. Mandalore antilles darth lando sidious solo leia wedge watto. Thrawn darth solo mace darth windu solo darth. Wedge chewbacca mustafar ackbar hutt yoda.";
                    recipeTextbox2.GetComponent<Text>().text = "Lucas ipsum dolor sit amet skywalker windu mothma mace greedo obi-wan darth jango wedge mara. Biggs antilles padmé anakin kenobi. Ponda darth vader mothma. Tatooine owen dooku hutt lobot darth baba. Mace coruscant palpatine wicket solo lars kenobi yoda. Mon wookiee leia anakin organa organa moff grievous. Bothan ackbar c-3po qui-gon. Gamorrean antilles calamari padmé boba. Mandalore antilles darth lando sidious solo leia wedge watto. Thrawn darth solo mace darth windu solo darth. Wedge chewbacca mustafar ackbar hutt yoda.";
                    recipeTextbox3.GetComponent<Text>().text = "Lucas ipsum dolor sit amet skywalker windu mothma mace greedo obi-wan darth jango wedge mara. Biggs antilles padmé anakin kenobi. Ponda darth vader mothma. Tatooine owen dooku hutt lobot darth baba. Mace coruscant palpatine wicket solo lars kenobi yoda. Mon wookiee leia anakin organa organa moff grievous. Bothan ackbar c-3po qui-gon. Gamorrean antilles calamari padmé boba. Mandalore antilles darth lando sidious solo leia wedge watto. Thrawn darth solo mace darth windu solo darth. Wedge chewbacca mustafar ackbar hutt yoda.";
                    //is checking the current step to determine if it should gray out the last3 3 while player 1 is clicking through the pages
                    if (currentStep >= 7) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 8) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 9) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }

                }
                currentPage = 0;
            }
        }
    }

    
}
