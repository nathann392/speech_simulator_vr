using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceController : MonoBehaviour
{

    //Array containing students
    private Transform StudentParent;
    private List<Transform> ActiveStudents = new List<Transform>();

    //Personality Type String for listing personality types
    public List<string> PersonalityTypes;

    //Private Variables for Student Reaction Management
    private float previousQuality = 0.0f;
    private float currentQuality = 0.0f;
    private Component speechGradeCalculator;
    private GameObject simulationController;
    private int reactionState = 0;
    private bool changedStudent = false;
    private int skippedstudents = 0;
    private int numberToSkip = 0;

    //Enum to determine basic types
    //[SerializeField] public enum personalityType : int
    //{
    //    Random,
    //    Focused,
    //    UnFocused
    //}

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Intialized Audience Controller");
        Debug.Log("Randomizing Student Participants");
        randomizeStudents();

        speechGradeCalculator = GameObject.Find("SpeechGradeCalculator").transform.GetComponent<SpeechGradeCalculator>();
        simulationController = GameObject.Find("SimulationController");

        InvokeRepeating("UpdateStudentReactions", 2.0f,3.0f);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generate and Randomize Student Objects
    void randomizeStudents()
    {
        //Pull All Students
        Debug.Log("Pulling All Students");
        StudentParent = GameObject.Find("Students").transform;

        for(int i = 0; i<StudentParent.childCount; i++)
        {
            //if (StudentParent.GetChild(i).name != "AudienceController")
            //{
                ActiveStudents.Add(StudentParent.GetChild(i));
                //Debug.Log("Added Student: " + StudentParent.transform.GetChild(i).name);

                //Randomize Each Student Mesh
                /* This functionality will have to remove the ENTIRE model from the Students Parent Gameobject
                 * It will then need to dynamically create Student models
                 * 
                 * Alternatively we can recreate the models in Unity as non-batched objects (Not preferable)
                 * */
                ActiveStudents[i].gameObject.GetComponent<BBKStudent>().generateStudent();
                    
              

                //Assign Each Student a "Personality"
            //}
        }
        
    }

    void UpdateStudentReactions()
    {
        //Only update the Audience if the class is active
        if (simulationController.GetComponent<SimulationController>().IsSessionActive())
        {
            currentQuality = speechGradeCalculator.GetComponent<SpeechGradeCalculator>().GetQuality();

            if (previousQuality != currentQuality)
            {
                Debug.Log("Updating Students");
                if (currentQuality < 0.333)
                {
                    //"Great Start!"
                    reactionState = 2;

                }
                else if (currentQuality < 0.667)
                {
                    //"Good Job!"
                    reactionState = 1;
                }
                else
                {
                    //"Excellent!"
                    reactionState = 0;
                }

                changedStudent = false;
                skippedstudents = 0;
                numberToSkip = Random.Range(1, ActiveStudents.Count);

                foreach (Transform student in ActiveStudents)
                {
                    if (reactionState != student.GetComponent<BBKStudent>().returnReactionState() && !changedStudent)
                    {
                        student.GetComponent<BBKStudent>().changeReaction(reactionState);
                        changedStudent = true;
                    }
                    else
                    {
                        if (skippedstudents < numberToSkip)
                        {
                            skippedstudents += 1;
                        }
                        else
                        {
                            changedStudent = false;
                        }
                    }
                }

                previousQuality = currentQuality;
            }
            else
            {
                Debug.Log("Quality has not changed; No Audience Updated");
            }
            //Randomly pick students to update Who are NOT already reacting to the current poor performance
        }
    }
}
