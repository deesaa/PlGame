using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using AxGrid.Path;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class Player : MonoBehaviourExtBind
    {
        public float stress;

        public float maxStress = 10;
        
        private float moveSpeed = 1f;
        private float cockMoveSpeed = 1.7f;

        public bool isLadderOnMap = false;
        public bool isOilContainerOnMap = false;
        
        public PlayerBehaviour currentPlayerBehaviour;

        public Ladder ladderPrototype;
        public OilContainer oilContainerPrototype;

        public Transform ladderHomePosition;
        public Transform oilContainerHomePosition;

        public Vector3 cockRandomMove;

        public int oilPerLamp = 7;

        public Transform gameStartPos;
        private static readonly int Bottom = Animator.StringToHash("Bottom");
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int Left = Animator.StringToHash("Left");
        private static readonly int Top = Animator.StringToHash("Top");

        public Animator PhoopEffect;

        public AudioSource audioSteps;
        public AudioSource audioLadder;
        public AudioSource audioCockNoise;
        public AudioSource audioAmbient;
        public AudioSource audioScreamsAmbient;

        public bool HasLadder
        {
            get => Model.GetBool("HasLadder");
            set => Model.Set("HasLadder", value);
        }
        public bool HasOil
        {
            get => Model.GetBool("HasOil");
            set => Model.Set("HasOil", value);
        }
        
        public int DayNumber
        {
            get => Model.GetInt("DayNumber");
            set => Model.Set("DayNumber", value);
        }
        
        public float HungerValue
        {
            get => Model.GetInt("HungerValue");
            set => Model.Set("HungerValue", value);
        }

        public float MoneyValue
        {
            get => Model.GetInt("MoneyValue");
            set => Model.Set("MoneyValue", value);
        }
        
        public float OilValue
        {
            get => Model.GetInt("OilValue");
            set => Model.Set("OilValue", value);
        }
        
        public bool PlayerMoveLock
        {
            get => Model.GetBool("PlayerMoveLock");
            set => Model.Set("PlayerMoveLock", value);
        }
        
        public bool PlayerTriggersLock
        {
            get => Model.GetBool("PlayerTriggersLock");
            set => Model.Set("PlayerTriggersLock", value);
        }

        [OnEnable]
        public void onEnable()
        {
            Model.Set("Player", this);

            GetComponent<Collider2D>()
                .OnTriggerStay2DAsObservable()
                .ObserveOnMainThread()
                .Subscribe(x =>
                {
                    if(Model.GetBool("PlayerTriggersLock", false) || !Model.GetBool("IsGameStarted") || Model.GetBool("IsGameOver")) return;
                    
                    if (x.GetComponent<HomeSleepPoint>() != null)
                    {
                        if (Model.GetBool("CanSleep"))
                        {
                            Model.EventManager.Invoke("PlayerReplicaMessage", "Пойти спать? Space", 2.5f);
                            if(Input.GetKey(KeyCode.Space))
                                LaySleep();
                        }
                        else
                        {
                            Model.EventManager.Invoke("PlayerReplicaMessage", "Рановато спать", 2.5f);
                        }
                    }
                    
                    if(Model.GetBool("IsPlayerCock", false)) return;


                    if (x.GetComponent<FirstPlayReplicaTrigger>() != null)
                    {
                        x.GetComponent<FirstPlayReplicaTrigger>().InvokeReplica();
                    }
                    
                    if (x.GetComponent<Ladder>() != null)
                    {
                        HasLadder = true;
                        Destroy(x.gameObject);
                    } 
                    if (x.GetComponent<OilContainer>() != null)
                    {
                        HasOil = true;
                        Destroy(x.gameObject);
                    }
                    if (x.GetComponent<Lamp>() != null)
                    {
                        var lamp = x.GetComponent<Lamp>();

                        if (!HasLadder)
                        {
                            Model.EventManager.Invoke("PlayerReplicaMessage", "Без лестницы не залезу", 2.5f);
                            return;
                        }
                        
                        if (!HasOil)
                        {
                            Model.EventManager.Invoke("PlayerReplicaMessage", "А перезаправить нечем", 2.5f);
                            return;
                        }

                        if (lamp.isLit)
                        {
                            if (OilValue >= oilPerLamp)
                            {
                                ChokeLamp(lamp);
                            }
                            else
                            {
                                Model.EventManager.Invoke("PlayerReplicaMessage", "Керосина не хватает", 2.5f);
                            }
                        }
                    }
                });
            //TODO: Забить на нужные компоненты
        }

        public void ChokeLamp(Lamp lamp)
        {

            OilValue -= oilPerLamp;
            
            audioSteps.gameObject.SetActive(false);
            
            Path = new CPath()
                .Action(() =>
                {
                    PlayerTriggersLock = true;
                    PlayerMoveLock = true;
                    audioLadder.gameObject.SetActive(true);
                    audioSteps.gameObject.SetActive(false);
                    lamp.ladderSprite.gameObject.SetActive(true);
                })
                .EasingLinear(1.5f, 0, 1, value =>
                {
                    transform.position = Vector3.Lerp(lamp.ladderStart.position, lamp.ladderEnd.position, value);
                    Model.Set("PlayerPos", transform.position);
                })
                .Wait(1f)
                .Action(lamp.ChokeLamp)
                .EasingLinear(1.5f, 0, 1, value =>
                {
                    transform.position = Vector3.Lerp(lamp.ladderEnd.position, lamp.ladderStart.position, value);
                    Model.Set("PlayerPos", transform.position);
                })
                .Action(() =>
                {
                    lamp.ladderSprite.gameObject.SetActive(false);
                    audioLadder.gameObject.SetActive(false);
                    PlayerTriggersLock = false;
                    PlayerMoveLock = false;
                });
        }
        
        [OnUpdate]
        public void OnUpdate()
        {
            if(!Model.GetBool("IsGameStarted")) return;
            
            if (Model.GetBool("IsPlayerCock"))
            {
                CockMove();
            }
            else
            {
                HumanMove();
            }
        }

        [Bind]
        public void OnIsGameStartedChanged()
        {
            if(Model.GetBool("IsGameStarted"))
                audioAmbient.gameObject.SetActive(true);
            else 
                audioAmbient.gameObject.SetActive(false);
        }

        public void HumanMove()
        {
//            Log.Debug($"Human Move {PlayerMoveLock}");
            
            if(PlayerMoveLock) return;

            float vert = Input.GetAxisRaw("Vertical");
            float horiz = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(vert) + Mathf.Abs(horiz) > 0.01f)
            {
                transform.Translate(new Vector3(horiz, vert).normalized * moveSpeed * Time.deltaTime);
                audioSteps.gameObject.SetActive(true);
                Model.Set("PlayerPos", transform.position);
            }
            else
            {
                audioSteps.gameObject.SetActive(false);
            }
        }
        
        public void CockMove()
        {
            float vert = Input.GetAxisRaw("Vertical");
            float horiz = Input.GetAxisRaw("Horizontal");
            
            audioSteps.gameObject.SetActive(true);

            Vector3 finalMove;
            if (Model.GetBool("IsGameOver"))
            {
                finalMove = cockRandomMove.normalized;
            }
            else
            {
                finalMove = new Vector3(horiz, vert).normalized + cockRandomMove.normalized * 0.6f;

                finalMove = new Vector3(Mathf.Clamp(finalMove.x, -1.2f, 1.2f), Mathf.Clamp(finalMove.y, -1.2f, 1.2f), 0f);
            }


            var angle = Vector3.SignedAngle(finalMove, Vector3.up, Vector3.forward);

            
            //TODO: Пиздец ну да ладно
            if (InRange(angle, -45f, 45f)) //TOP
            {
                Animator.SetBool(Bottom, false);
                Animator.SetBool(Right, false);
                Animator.SetBool(Left, false);
                Animator.SetBool(Top, true);
                
            }
            else if (InRange(angle, -135, -45))
            {
                Animator.SetBool(Bottom, false);
                Animator.SetBool(Right, false);
                Animator.SetBool(Top, false);
                Animator.SetBool(Left, true);
            } // LEFT
            
            else if (InRange(angle, 45, 135))
            {
                Animator.SetBool(Bottom, false);
                Animator.SetBool(Left, false);
                Animator.SetBool(Top, false);
                Animator.SetBool(Right, true);
            } //RIGHT
            else
            {
                Animator.SetBool(Right, false);
                Animator.SetBool(Left, false);
                Animator.SetBool(Top, false);
                Animator.SetBool(Bottom, true);
            }//BOTTOM


            transform.Translate(finalMove * cockMoveSpeed * Time.deltaTime);
            Model.Set("PlayerPos", transform.position);
        }

        public bool InRange(float x, float point1, float point2) => 
            x >= point1 && x <= point2;


        [OnRefresh(1f)]
        [Bind]
        public void OnRefreshPlayer()
        {
            HungerValue++;
            
            if (Model.GetBool("IsPlayerCock"))
            {
                cockRandomMove = Random.onUnitSphere;
                cockRandomMove = new Vector3(cockRandomMove.x, cockRandomMove.y, 0f).normalized;
                
                if(Random.value >= 0.6)
                    audioCockNoise.Play();
                
                Model.EventManager.Invoke("CockMakeNoise");
            }
        }

        public void LaySleep()
        {
            Log.Debug("LaySleep");
            Model.EventManager.Invoke("PlayerSleep");
        }

        [Bind]
        public void DamagePlayer()
        {
            stress++;
            if(stress >= maxStress)
                Model.EventManager.Invoke("PlayerBecomeCock");
        }

        [Bind]
        public void StartNewDay()
        {
            Log.Debug($"StartNewDay {DayNumber}");
            
            audioSteps.gameObject.SetActive(false);
            audioScreamsAmbient.gameObject.SetActive(false);
            PhoopEffect.gameObject.SetActive(false);

            Model.Set("CanSleep", false);
            
            Model.EventManager.Invoke("PlayerBecomeHuman");

            if ((!HasLadder && !isLadderOnMap) || DayNumber <= 0)
            {
                HasLadder = false;
                Log.Debug("Instance ladder");
                var l = Instantiate(ladderPrototype);
                l.transform.position = ladderHomePosition.position;
                isLadderOnMap = true;
            }
            if ((!HasOil && !isOilContainerOnMap) || DayNumber <= 0)
            {
                HasOil = false;
                var l = Instantiate(oilContainerPrototype);
                l.transform.position = oilContainerHomePosition.position;
                isOilContainerOnMap = true;
            }
        }

        [Bind]
        public void OnSunrise()
        {
            Model.EventManager.Invoke("PlayerBecomeCock");
        }

        [Bind]
        public void PlayerBecomeCock()
        {
            Animator.ResetTrigger("IsHuman");
            Animator.SetTrigger("IsCock");
            
            audioCockNoise.Play();
            audioAmbient.gameObject.SetActive(false);
            audioScreamsAmbient.gameObject.SetActive(true);
            
            PhoopEffect.gameObject.SetActive(true);
            
            Model.Set("IsPlayerCock", true);
            
            Debug.Log("PlayerBecomeCock");
            
            if (HasLadder)
            {
                isLadderOnMap = true;
                HasLadder = false;
                var l = Instantiate(ladderPrototype);
                l.transform.position = this.transform.position;
                Model.EventManager.Invoke("PlayerDropLadder");
            }

            if (HasOil)
            {
                isOilContainerOnMap = true;
                HasOil = false;
                var o = Instantiate(oilContainerPrototype);
                o.transform.position = this.transform.position;
                Model.EventManager.Invoke("PlayerDropOil");
            }
            
            currentPlayerBehaviour = PlayerCockBehaviour.Instance;
            
            Model.EventManager.Invoke("OnRefreshPlayer");
        }

        [Bind]
        public void PlayerBecomeHuman()
        {
            audioAmbient.gameObject.SetActive(true);
            audioScreamsAmbient.gameObject.SetActive(false);
            
            Animator.ResetTrigger("IsCock");
            Animator.SetTrigger("IsHuman");
            PhoopEffect.gameObject.SetActive(false);
            
            Model.Set("IsPlayerCock", false);
            Model.EventManager.Invoke("OnRefreshPlayer");
            currentPlayerBehaviour = PlayerHumanBehaviour.Instance;
        }

        [Bind]
        public void RestartPlayData()
        {
            audioSteps.gameObject.SetActive(false);
            audioLadder.gameObject.SetActive(false);
            audioAmbient.gameObject.SetActive(false);
            audioScreamsAmbient.gameObject.SetActive(false);

            this.transform.position = gameStartPos.position;
            Model.Set("PlayerPos", transform.position);
            OilValue = 72;
            HungerValue = 0;
            MoneyValue = 14;
            isLadderOnMap = false;
            isOilContainerOnMap = false;
            HasLadder = false;
            HasOil = false;
            currentPlayerBehaviour = PlayerHumanBehaviour.Instance;
        } 
    }
    
    public class PlayerHumanBehaviour : PlayerBehaviour
    {
        public static PlayerHumanBehaviour Instance { get; } = new PlayerHumanBehaviour();
    }

    public class PlayerCockBehaviour : PlayerBehaviour
    {
        public static PlayerCockBehaviour Instance { get; } = new PlayerCockBehaviour();
    }

    public class PlayerBehaviour
    {
        
    }
}
