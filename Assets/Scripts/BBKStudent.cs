using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBKStudent : MonoBehaviour
{
 
    //[SerializeField] private AudienceController.personalityType currentType = 0; 
    [SerializeField] private StudentPersonalityTypes currentType = 0; 
    private int randomModelNumber;
    private int randomPersonalityNumber;
    private Animator currentAnimator;
    private RuntimeAnimatorController newAnimator;
    private int animationState;
    private string animationTrigger = "Attentive";

    // Start is called before the first frame update
    void Start()
    {
        currentAnimator = transform.gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Randomizes model, personality, and other assets
    public int generateStudent()
    {
        
        //Debug.Log("Gengerating " + gameObject.name);

        try 
        {
            //Saves editor Model's set position
            Vector3 oldVec = transform.GetChild(0).position;
            Quaternion oldRot = transform.GetChild(0).rotation;
            Vector3 oldScale = transform.GetChild(0).localScale;
            
            //Destroys editor model
            Destroy(transform.GetChild(0).gameObject);

            //Replaces editor model with random prefab from the model stubs
            Instantiate(retrieveRandomMesh(), oldVec, oldRot, transform);

            //transform.localScale = oldScale;
            
            if(currentType == 0)
            {
                currentType = getRandomPersonality();
            }

            //Update Animation Controller to reflect new personality
            currentAnimator = transform.gameObject.GetComponentInChildren<Animator>();


            Debug.Log("Pre:" + currentAnimator.runtimeAnimatorController.ToString());

            //if (System.IO.File.Exists("Assets/Resources/" + currentType.ToString() + ".controller"))
            //{
                newAnimator = Resources.Load(currentType.ToString()) as RuntimeAnimatorController;

                //currentAnimator.runtimeAnimatorController = newAnimator;

                //currentAnimator.Rebind();
                Debug.Log(currentType.ToString());
                currentAnimator.SetBool(currentType.ToString(),true);

                //Set Default Animation State
                currentAnimator.SetTrigger(animationTrigger);

                Debug.Log("Post:" + currentAnimator.runtimeAnimatorController.ToString());
            //}



            //Debug.Log(transform.GetChild(0).position); 

            return 1;
        }
        catch(System.Exception ex)
        {
            Debug.Log("Error in generation of " + gameObject.name + ": " + ex.ToString());
            return 0;
        }
    }

    //Randomly selects and returns a prefab model from Model Stubs object
    private GameObject retrieveRandomMesh()
    {
        randomModelNumber = Random.Range(0, GameObject.Find("Model Stubs").transform.childCount - 1);

        //Debug.Log("Randomly Chose this mesh: " + GameObject.Find("Model Stubs").transform.GetChild(randomModelNumber).name);

        return GameObject.Find("Model Stubs").transform.GetChild(randomModelNumber).gameObject;
    }

    //Randomly selects and returns a personality type from the AudienceController enumeration personalityType
    private StudentPersonalityTypes getRandomPersonality()
    {
        randomPersonalityNumber = Random.Range(1, StudentPersonalityTypes.GetNames(typeof(StudentPersonalityTypes)).Length);

        return (StudentPersonalityTypes)randomPersonalityNumber;
    }

    public void changeReaction(int qualityChange)
    {
        currentAnimator = transform.gameObject.GetComponentInChildren<Animator>();
        currentAnimator.ResetTrigger(animationTrigger.ToString());
        
        animationTrigger = "";
        animationState = qualityChange;

        //Update animation if different quality level
        switch (qualityChange)
        {
            case 1:
                animationTrigger = "Distracted";
                break;
            case 2:
                animationTrigger = "Disruptive";
                break;
            default:
                animationTrigger = "Attentive";
                break;
        }
        currentAnimator.SetTrigger(animationTrigger.ToString());

        Debug.Log("Updated Student:" + gameObject.name);
    }

    public int returnReactionState()
    {
        return animationState;
    }
}
