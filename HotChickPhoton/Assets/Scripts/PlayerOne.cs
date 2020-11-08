using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOne : MonoBehaviour
{

    public static int activePlayerIdx = 0;
    // //If the scrap number is less than the following number, the system will respawn for the player
    // public int respawnThreshold = 3; 

    // enum State
    // {
    //     hand,
    //     hammer,
    //     stunned
    // }

    // //Is this player the player on this machine?
    // public bool active = true;
    // [SerializeField]
    // Reticle reticle;
    // [SerializeField]
    // Transform hand;
    // [SerializeField]
    // Transform table;
    // [SerializeField]
    // float tableRadius;
    // float reticleRadius;
    // [SerializeField]
    // float bedBubbleRadius;
    // [SerializeField]
    // GameObject hammer;
    // [SerializeField]
    // Text stunText;
    // [SerializeField]
    // float hammerCooldown;
    // float hammerCurrentTime = 0;
    // [SerializeField]
    // Text hammerText;
    // [SerializeField]
    // float makeBadCooldown;
    // float makeBadCurrentTime = 0;
    // [SerializeField]
    // Text makeBadText;

    // [SerializeField]
    // Transform bedBoy;

    // [SerializeField]
    // public PlayerOne leftPlayer;
    // [SerializeField]
    // public PlayerOne rightPlayer;
    // [SerializeField]
    // NameText[] names;
    // [SerializeField]
    // GameObject highlightSphere;
    // [SerializeField]
    // GameObject dreamObject;
    // [SerializeField]
    // Transform head;
    // [SerializeField]
    // float headAngleDelta;
    // Quaternion headTargetRotation;

    // [SerializeField]
    // float headMinX;
    // [SerializeField]
    // float headMaxX;
    // [SerializeField]
    // float headMinY;
    // [SerializeField]
    // float headMaxY;

    // Scrap heldScrap = null;

    // Vector3 tableOffset;
    // State currentState = State.hand;
    // State stateBeforeStun = State.hand;

    // Scrap[] currentTriple = null;
    // bool reticleOnTable = true;
    // PlayerOne highlightPlayer = null;

    // Camera myCamera;

    // public bool highlight = false;

    // float headUpdateTime = 0.1f;
    // float headUpdateCurrentTime = 0;

    // float stunTime = 2.5f;
    // float currentStunTime = 0;

    // [SerializeField]
    // GameObject stunnedVisual;

    [SerializeField]
    int index;

    //public static GameObject textBox;

    // public bool actionWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        // tableOffset = table.position - transform.position;
        // if (Mathf.Abs(tableOffset.x) >= Mathf.Abs(tableOffset.z)) tableOffset.z = 0;
        // else tableOffset.x = 0;
        // tableOffset.y = 0;
        // tableOffset.Normalize();
        // reticleRadius = reticle.transform.localScale.x * 0.5f;
        // myCamera = transform.GetChild(0).GetComponent<Camera>();
        // if (index == activePlayerIdx)
        //     active = true;
        // if (!active)
        //     myCamera.gameObject.SetActive(false);
        // if (active)
        //     bedBoy.transform.rotation = Quaternion.Euler(0, 90 + 90 * index, 0);

        // //Print Nick Name
        // ActionManager.instance.SetNickname(index);
    }

    public static void SetText(bool isChick){
        GameObject textBox = GameObject.Find("Text");
        if(isChick){
            textBox.GetComponent<Text>().text = "Chick";
        }
        else
        {
            textBox.GetComponent<Text>().text = "Farmer";
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (active)
    //     {
    //         //UpdateReticle();
    //         if (!actionWaiting)
    //         {
    //             UpdateTriple();
    //             if (Input.GetMouseButtonDown(0))
    //             {
    //                 if (currentState == State.hand)
    //                     ChangeHeldScrap();
    //                 if (currentState == State.hammer && hammerCurrentTime <= 0)
    //                     UseHammer();
    //             }
    //             if(Input.GetMouseButtonDown(1) && currentState == State.hand )
    //             {
    //                 if (GameState.imposter && heldScrap != null && !heldScrap.bad && makeBadCurrentTime <= 0)
    //                     MakeOrbBad();
    //                 else
    //                     TryBreakScrap();
    //             }
    //             if (Input.GetKeyDown(KeyCode.Space))
    //             {
    //                 ActionManager.instance.Initiate(index, "Toggle", -1, Vector3.zero, null);
    //             }
    //         }
    //     }
    //     UpdateStun();
    //     head.localRotation = Quaternion.Slerp(head.localRotation, headTargetRotation, headAngleDelta * Time.deltaTime);
    //     UpdateHammerCooldown();
    //     UpdateMakeBadCooldown();
    // }

    // void UpdateReticle()
    // {
    //     reticleOnTable = true;
    //     RaycastHit hit;
    //     if (Physics.Raycast(myCamera.ScreenPointToRay(Input.mousePosition), out hit, 100000, 1 << 8))
    //     {
    //         Vector3 tablePos = table.position - tableOffset * reticleRadius;
    //         Vector3 hitPoint = new Vector3(hit.point.x, 0.5f, hit.point.z);
    //         Vector3 vec = (hitPoint - table.position);
    //         if (Vector3.Dot(vec, tableOffset) > 0)
    //         {
    //             vec -= tableOffset * Vector3.Dot(vec, tableOffset);
    //         }
    //         if (Mathf.Abs(vec.x) > tableRadius - reticleRadius || Mathf.Abs(vec.z) > tableRadius - reticleRadius)
    //             reticleOnTable = false;
    //         vec.x = Mathf.Sign(vec.x) * Mathf.Min(Mathf.Abs(vec.x), tableRadius - reticleRadius);
    //         vec.z = Mathf.Sign(vec.z) * Mathf.Min(Mathf.Abs(vec.z), tableRadius - reticleRadius);
    //         Vector3 vec2 = vec - tableOffset * reticleRadius;
    //         Vector3 vecXZ = new Vector3(vec2.x, 0, vec2.z);
    //         vec2 -= vecXZ;
    //         vec2 += vecXZ.normalized * Mathf.Max(reticleRadius + bedBubbleRadius, vecXZ.magnitude);
    //         reticle.transform.position = table.position + vec2;
    //         Vector3 side = Vector3.Cross(tableOffset, Vector3.up).normalized;
    //         headUpdateCurrentTime -= Time.deltaTime;
    //         if (headUpdateCurrentTime < 0)
    //         {
    //             ActionManager.instance.UpdateHeadRotation(index, Quaternion.Euler(headMinX + (headMaxX - headMinX) * Vector3.Dot(vec2, tableOffset.normalized) * -0.66f, headMinY + (headMaxY - headMinY) * Vector3.Dot(vec2, side) * 0.66f, 0));
    //             headUpdateCurrentTime = headUpdateTime;
    //         }
    //         if (Vector3.Dot(side, vec) >= 0)
    //             highlightPlayer = leftPlayer;
    //         else
    //             highlightPlayer = rightPlayer;
    //     }
    //     else
    //     {
    //         reticleOnTable = false;
    //     }
    // }

    // public void UpdateHeadRotation( Quaternion newRotation )
    // {
    //     headTargetRotation = newRotation;
    // }

    // public void displayNickname(string nickName){
    //     PlayerOne activePlayer = ActionManager.instance.players[activePlayerIdx];
    //     if( this == activePlayer.leftPlayer )
    //     {
    //         names[0].Setup(index, nickName);
    //     }
    //     else if( this == activePlayer.rightPlayer )
    //     {
    //         names[2].Setup(index, nickName);
    //     }
    //     else if( this != activePlayer )
    //     {
    //         names[1].Setup(index, nickName);
    //     }
    // }

    // void UpdateTriple()
    // {
    //     leftPlayer.UnHighlight();
    //     rightPlayer.UnHighlight();
    //     if (currentTriple != null)
    //     {
    //         foreach (Scrap S in currentTriple)
    //         {
    //             if( S != null && !S.Equals(null) )
    //                 S.BuildUnHighlight();
    //         }
    //     }
    //     currentTriple = null;
    //     if (currentState == State.hammer)
    //     {
    //         if (reticleOnTable)
    //         {
    //             currentTriple = reticle.GetScrapTriple(table, tableOffset);
    //             if (currentTriple != null)
    //             {
    //                 foreach (Scrap S in currentTriple)
    //                     S.BuildHighlight();
    //             }
    //         }
    //         else
    //         {
    //             highlightPlayer.Highlight();
    //         }
    //     }
    // }

    // void CreateDream()
    // {
    //     string[] inputArray = new string[3];
    //     for( int i = 0; i < 3; i++ )
    //     {
    //         inputArray[i] = currentTriple[i].ID;
    //     }
    //     ActionManager.instance.Initiate(index, "Dream", -1, Vector3.zero, inputArray);
    // }

    // public void ToggleHammer()
    // {
    //     if( currentState == State.hammer )
    //     {
    //         SwitchToHand();
    //     }
    //     else if( currentState == State.hand )
    //     {
    //         SwitchToHammer();
    //     }
    // }

    // void UseHammer()
    // {
    //     if (currentTriple != null)
    //         CreateDream();
    //     else if (highlightPlayer != null)
    //         ActionManager.instance.Initiate(index, "Stun", highlightPlayer.index, Vector3.zero, null);
    // }

    // void MakeOrbBad()
    // {
    //     ActionManager.instance.Initiate(index, "Bad", int.Parse(heldScrap.ID), Vector3.zero, null);
    // }

    // void ChangeHeldScrap()
    // {
    //     if( heldScrap != null )
    //     {
    //         ActionManager.instance.Initiate(index, "Throw", -1, reticle.transform.position + Vector3.up * 0.075f, null);
    //     }
    //     else if( reticle.HasScrap() )
    //     {
    //         print("Picking up scrap: " + reticle.GetScrap().ID + " parsed number: " + reticle.GetScrap().ID);
    //         ActionManager.instance.Initiate(index, "Pick", int.Parse(reticle.GetScrap().ID), Vector3.zero, null);
    //     }
    // }

    // void TryBreakScrap()
    // {
    //     if( heldScrap == null && reticle.HasScrap() )
    //     {
    //         ActionManager.instance.Initiate(index, "Break", int.Parse(reticle.GetScrap().ID), Vector3.zero, null);
    //     }
    // }

    // //Start action methods
    
    // public void BreakScrap( Scrap S )
    // {
    //     S.Break();
    //     CheckSpawnNewScrap();
    // }

    // public void PickUpScrap( Scrap S )
    // {
    //     S.InitUnHighlight();
    //     heldScrap = S;
    //     heldScrap.Attach(hand);
    // }

    // public void ThrowScrap( Vector3 target )
    // {
    //     heldScrap.InitUnHighlight();
    //     heldScrap.Throw(target);
    //     heldScrap = null;
    // }

    // void StartHammerCooldown()
    // {
    //     hammerCurrentTime = hammerCooldown;
    //     if( index == activePlayerIdx )
    //     {
    //         hammerText.gameObject.SetActive(true);
    //         hammerText.text = "Hammer Used\n" + hammerCooldown;
    //     }
    // }

    // void StartMakeBadCooldown()
    // {
    //     makeBadCurrentTime = makeBadCooldown;
    //     if( index == activePlayerIdx )
    //     {
    //         makeBadText.gameObject.SetActive(true);
    //         makeBadText.text = "Make Orb Bad Used\n" + makeBadCooldown;
    //     }
    // }

    // public void CreateDream( Scrap[] scrapArray )
    // {
    //     StartHammerCooldown();
    //     bool good = true;
    //     Vector3 averagePos = Vector3.zero;
    //     foreach (Scrap S in scrapArray)
    //     {
    //         averagePos += S.transform.position;
    //         if (S.bad)
    //             good = false;
    //         S.Remove();
    //     }
    //     averagePos /= 3;
    //     Dream newDream = Instantiate(dreamObject).GetComponent<Dream>();
    //     newDream.transform.position = averagePos;
    //     newDream.SetStatus(good);
    //     currentTriple = null;
    //     if (good)
    //     {
    //         GameState.state.totalGoodDreams++;
    //         GameState.state.playerGoodDreams[index]++;
    //     }
    //     else
    //     {
    //         GameState.state.totalBadDreams++;
    //         GameState.state.playerBadDreams[index]++;
    //     }
    //     GameState.state.UpdateScoreUI();
    //     CheckSpawnNewScrap();
    //     Tutorial.instance.dreamMade = true;
    // }

    // void CheckSpawnNewScrap()
    // {
    //     bool hasType0 = false;
    //     bool hasType1 = false;
    //     bool hasType2 = false;
    //     foreach( Scrap S in ScrapSpawner.allScrap.Values )
    //     {
    //         if (S.GetScrapType() == 0) hasType0 = true;
    //         if (S.GetScrapType() == 1) hasType1 = true;
    //         if (S.GetScrapType() == 2) hasType2 = true;
    //     }
    //     if ( !hasType0 || !hasType1 || !hasType2 || ScrapSpawner.allScrap.Count <= respawnThreshold)
    //     {
    //         GameState.state.spawnNewScrap();
    //     }
    // }

    // public void StunPlayer( PlayerOne targetPlayer )
    // {
    //     StartHammerCooldown();
    //     targetPlayer.SwitchToStunned();
    //     Tutorial.instance.stunStatus[index] = true;
    // }

    // public void InitThrowScrap()
    // {
    //     heldScrap.InitHighlight();
    // }

    // public void CancelThrowScrap()
    // {
    //     heldScrap.InitUnHighlight();
    // }

    // public void MakeScrapBad()
    // {
    //     heldScrap.MakeBad();
    //     StartMakeBadCooldown();
    // }

    // //End action methods

    // public bool IsStunned()
    // {
    //     return currentState == State.stunned;
    // }

    // void UpdateStun()
    // {
    //     if( currentStunTime > 0 )
    //     {
    //         currentStunTime -= Time.deltaTime;
    //         if (index == activePlayerIdx)
    //             stunText.text = "Stunned\n" + currentStunTime.ToString("F1");
    //         if (currentStunTime <= 0)
    //         {
    //             if (index == activePlayerIdx)
    //                 stunText.gameObject.SetActive(false);
    //             switch (stateBeforeStun)
    //             {
    //                 case State.hand:
    //                     SwitchToHand();
    //                     break;
    //                 case State.hammer:
    //                     SwitchToHammer();
    //                     break;
    //             }
    //         }
    //     }
    // }

    // void UpdateHammerCooldown()
    // {
    //     if (hammerCurrentTime > 0)
    //     {
    //         hammerCurrentTime = Mathf.Max(0, hammerCurrentTime - Time.deltaTime);
    //         if (index == activePlayerIdx)
    //         {
    //             hammerText.text = "Hammer Used\n" + hammerCurrentTime.ToString("F1");
    //             if (hammerCurrentTime <= 0)
    //                 hammerText.gameObject.SetActive(false);
    //         }
    //     }
    // }

    // void UpdateMakeBadCooldown()
    // {
    //     if( makeBadCurrentTime > 0 )
    //     {
    //         makeBadCurrentTime = Mathf.Max(0, makeBadCurrentTime - Time.deltaTime);
    //         if( index == activePlayerIdx )
    //         {
    //             makeBadText.text = "Make Orb Bad Used\n" + makeBadCurrentTime.ToString("F1");
    //             if (makeBadCurrentTime <= 0)
    //                 makeBadText.gameObject.SetActive(false);
    //         }
    //     }
    // }

    // public void SwitchToHammer()
    // {
    //     currentState = State.hammer;
    //     hammer.gameObject.SetActive(true);
    //     stunnedVisual.SetActive(false);
    // }

    // public void SwitchToHand()
    // {
    //     currentState = State.hand;
    //     hammer.gameObject.SetActive(false);
    //     stunnedVisual.SetActive(false);
    // }

    // public void SwitchToStunned()
    // {
    //     if( currentState != State.stunned ) stateBeforeStun = currentState;
    //     currentState = State.stunned;
    //     hammer.gameObject.SetActive(false);
    //     currentStunTime = stunTime;
    //     stunnedVisual.SetActive(true);
    //     if( index == activePlayerIdx )
    //     {
    //         stunText.gameObject.SetActive(true);
    //         stunText.text = "Stunned!\n" + stunTime;
    //     }
    // }

    // public void Highlight()
    // {
    //     highlight = true;
    //     highlightSphere.SetActive(true);
    // }

    // public void UnHighlight()
    // {
    //     highlight = false;
    //     highlightSphere.SetActive(false);
    // }

    // public void SetActive()
    // {
    //     active = true;
    //     activePlayerIdx = index;
    // }

    // public bool IsHammer()
    // {
    //     return currentState == State.hammer;
    // }

    // public bool IsHoldingScrap( Scrap S )
    // {
    //     return S == heldScrap;
    // }
}
