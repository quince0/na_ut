using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStats))]
public class DragonController : MonoBehaviour
{
    class ParametersAnimations
    {
        public string parameterType;
        public string parameterName;

        public ParametersAnimations(string parameterType, string parameterName)
        {
            this.parameterType = parameterType;
            this.parameterName = parameterName;
        }
    }
    Dictionary<string, ParametersAnimations> dragonParaAnim = new Dictionary<string, ParametersAnimations>();
    delegate ParametersAnimations ValueTurnDelegate(Dictionary<string, ParametersAnimations> valueTurn_90);

    public bool debug = true;
    
    //Rigidbody myRigidbody;
    EnemyStats dragon;
    Animator dragonAnimator;

    public GameObject objetive;

    #region VarsDistanceToObjetive
    [Range(3.0f, 10.0f)]
    public float distanceToGetObjetive = 4.0f;
    public float distanceToNotMove = 12.0f;
    [Range(1.0f, 2.0f)]
    public float stopDistance = 2.0f;
    #endregion VarsDistanceToObjetive

    #region VarsAngles
    public float turnSpeed = 29.42105f;
    [Range(10.0f, 60.0f)]
    public float anglesToStop = 30.0f;
    float anglesStartTurn90 = 60.0f;
    float speedPercentInTurn90 = 0.1f;
    float anglesRotateWhileAttack = 45.0f;
    float anglesBtwHeadTail = 135.0f; // anglesBtwHeadTail = 180 - anglesRotateWhileAttack
    #endregion VarsAngles

    #region VarsForSmooth
    [Range(0.0f, 1.0f)]
    public float speedSmoothTime = 0.1f;
    public float speedSmoothTimeAnimation = 0.0f;
    float speedSmoothVelocity = 0.0f;
    public float turnSmoothTimeAnimation = 0.0f;
    #endregion VarsForSmooth

    float currentSpeed;


    // Use this for initialization
    void Start()
    {
        
        //myRigidbody = GetComponent<Rigidbody>();

        dragon = GetComponent<EnemyStats>();
        dragonAnimator = GetComponent<Animator>();

        dragonParaAnim["locomotion"] = new ParametersAnimations ("bool", "locomotion");
        dragonParaAnim["speedPercent"] = new ParametersAnimations("float", "speedPercent");
        dragonParaAnim["turn_L_90"] = new ParametersAnimations("bool", "turn_L_90");
        dragonParaAnim["turn_R_90"] = new ParametersAnimations("bool", "turn_R_90");
        dragonParaAnim["die"] = new ParametersAnimations("bool", "die");
        dragonParaAnim["hurted_0"] = new ParametersAnimations("bool", "hurted_0");
        dragonParaAnim["attack_lash"] = new ParametersAnimations("bool", "attack_lash");
        dragonParaAnim["attack_double_bite"] = new ParametersAnimations("bool", "attack_double_bite");
        dragonParaAnim["attack_single_bite_L"] = new ParametersAnimations("bool", "attack_single_bite_L");
        dragonParaAnim["attack_single_bite_R"] = new ParametersAnimations("bool", "attack_single_bite_R");

        dragonAnimator.SetBool(dragonParaAnim["locomotion"].parameterName, true);

        StartCoroutine(EnemyActs());
    }

    IEnumerator EnemyActs()
    {
        float displacementFromTarget;

        while (true)
        {
            displacementFromTarget = (objetive.transform.position - transform.position).magnitude;
            if (dragonAnimator.GetBool(dragonParaAnim["locomotion"].parameterName))
            {
                yield return StartCoroutine(TurnToTarget());
                yield return StartCoroutine(MoveToTarget());
            }
            if (displacementFromTarget <= distanceToGetObjetive)
            {
                yield return StartCoroutine(AttackToTarget());
            }
        }
    }


    IEnumerator TurnToTarget()
    {

        // We get the way to move the enemy towards objective.
        Vector3 directionToTarget = (objetive.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));

        Quaternion currentRotation = transform.rotation;
        float diferenceAngles = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y);

        // If the necessary rotation is small, the enemy can run while turning, instead of turning in stopped
        if (diferenceAngles > anglesToStop || diferenceAngles < -anglesToStop)
        {
            while (diferenceAngles > 0.05f || diferenceAngles < -0.05f)
            {
                directionToTarget = (objetive.transform.position - transform.position).normalized;
                targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
                currentRotation = transform.rotation;
                
                diferenceAngles = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y);
                if (diferenceAngles > anglesStartTurn90)
                {
                    yield return StartCoroutine(TurnTo90Degrees(currentRotation, Quaternion.Euler(0.0f, currentRotation.eulerAngles.y + 90.0f, 0.0f), dragonParaAnim => dragonParaAnim["turn_R_90"]));
                }
                else if (diferenceAngles < -anglesStartTurn90)
                {
                    yield return StartCoroutine(TurnTo90Degrees(currentRotation, Quaternion.Euler(0.0f, currentRotation.eulerAngles.y + -90.0f, 0.0f), dragonParaAnim => dragonParaAnim["turn_L_90"]));
                }
                else
                {
                    dragonAnimator.SetFloat(dragonParaAnim["speedPercent"].parameterName, speedPercentInTurn90, speedSmoothTimeAnimation, Time.deltaTime);
                    transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, turnSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }

    IEnumerator TurnTo90Degrees(Quaternion pastCurrentRotation, Quaternion targetRotation, ValueTurnDelegate valueTurn_90)
    {
        Quaternion currentRotation = transform.rotation;

        dragonAnimator.SetBool(valueTurn_90(dragonParaAnim).parameterName, true);
        
        float diferenceAngles = Quaternion.Angle(currentRotation, targetRotation);
        while (diferenceAngles > 0.05f)
        {
            currentRotation = transform.rotation;

            diferenceAngles = Quaternion.Angle(currentRotation, targetRotation);
            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }
        dragonAnimator.SetBool(valueTurn_90(dragonParaAnim).parameterName, false);
    }

    IEnumerator MoveToTarget()
    {
        // We get the way to move the enemy towards objective.
        Vector3 displacementFromTarget = objetive.transform.position - transform.position;
        Vector3 directionToTarget = displacementFromTarget.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        //print("Hello I am TurnToTarget coroutine");

        Quaternion currentRotation = transform.rotation;

        // We get the current speed of the enemy, if it is accelerating or decelerating the speed will change
        float diferenceAngles = Quaternion.Angle(currentRotation, targetRotation);
        float speedTarget = ((displacementFromTarget.magnitude <= distanceToGetObjetive ||
                              diferenceAngles <= -anglesToStop || diferenceAngles >= anglesToStop) ? 0.0f : dragon.speed);

        while (speedTarget >= 0.01f || currentSpeed >= 0.01f)
        {
            displacementFromTarget = objetive.transform.position - transform.position;
            directionToTarget = displacementFromTarget.normalized;
            targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));

            currentRotation = transform.rotation;

            diferenceAngles = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y);
            speedTarget = ((displacementFromTarget.magnitude <= distanceToGetObjetive ||
                            diferenceAngles <= -anglesToStop || diferenceAngles >= anglesToStop) ? 0.0f : dragon.speed);


            currentSpeed = Mathf.SmoothDamp(currentSpeed, speedTarget, ref speedSmoothVelocity, speedSmoothTime);

            if (displacementFromTarget.magnitude <= stopDistance)
            {
                currentSpeed = 0.0f;
            }

            // We calcule the velocity and set the displacement
            Vector3 velocity = Vector3.forward * currentSpeed;//directionToTarget * currentSpeed;
            transform.Translate(velocity * Time.deltaTime);


            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, turnSpeed * Time.deltaTime);
            // We set the speedPercent value for locomotion animation
            float animationSpeedPercent = currentSpeed / dragon.speed;
            dragonAnimator.SetFloat(dragonParaAnim["speedPercent"].parameterName, animationSpeedPercent, speedSmoothTimeAnimation, Time.deltaTime);
            yield return null;
        }
    }
    
    IEnumerator AttackToTarget()
    {
        Vector3 displacementFromTarget;
        Vector3 directionFromTarget;
        Quaternion targetRotation;
        float diferenceAngles;
        do
        {
            displacementFromTarget = objetive.transform.position - transform.position;
            directionFromTarget = (displacementFromTarget).normalized;
            targetRotation = Quaternion.LookRotation(new Vector3(directionFromTarget.x, 0, directionFromTarget.z));

            diferenceAngles = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y);
            if (diferenceAngles > anglesBtwHeadTail || diferenceAngles < -anglesBtwHeadTail)
            {
                ActualizeCurrentAnimation(dragonParaAnim["attack_lash"]);
            }
            else
            {
                bool turOrFalse = Random.value > 0.5f;
                if (turOrFalse)
                {
                    if (diferenceAngles > 0)
                    {
                        ActualizeCurrentAnimation(dragonParaAnim["attack_single_bite_R"]);
                    }
                    else
                    {
                        ActualizeCurrentAnimation(dragonParaAnim["attack_single_bite_L"]);
                    }
                }
                else
                {
                    ActualizeCurrentAnimation(dragonParaAnim["attack_double_bite"]);
                }
            }
            // We wait for end the current animation
            yield return new WaitForSeconds(dragonAnimator.GetCurrentAnimatorStateInfo(0).length);

            // If the dragon wants to make consecutive attacks and it is close enough to the objective

            displacementFromTarget = objetive.transform.position - transform.position;
            directionFromTarget = (displacementFromTarget).normalized;
            targetRotation = Quaternion.LookRotation(new Vector3(directionFromTarget.x, 0, directionFromTarget.z));
            diferenceAngles = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y);
            
            if (displacementFromTarget.magnitude < distanceToNotMove &&
                ((diferenceAngles > anglesRotateWhileAttack || diferenceAngles < -anglesRotateWhileAttack) &&
                 diferenceAngles > (180.0f - anglesRotateWhileAttack) || diferenceAngles < (-180.0f + anglesRotateWhileAttack)))
            {
                yield return null;
            }
            else
            {
                ActualizeCurrentAnimation(dragonParaAnim["locomotion"]);
            }
        } while (dragonAnimator.GetBool(dragonParaAnim["locomotion"].parameterName) == false);
    }

    void ActualizeCurrentAnimation(ParametersAnimations actualAnimation)
    {
        foreach (ParametersAnimations nameAnimation in dragonParaAnim.Values)
        {
            if (nameAnimation.parameterType == "bool")
            {
                dragonAnimator.SetBool(nameAnimation.parameterName, false);
            }
        }

        dragonAnimator.SetBool(actualAnimation.parameterName, true);
    }
}
