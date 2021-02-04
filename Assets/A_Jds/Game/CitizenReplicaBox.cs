
using System.Collections.Generic;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class CitizenReplicaBox : MonoBehaviourExtBind
    {
        public RectTransform hideShowTransform;
        public Text messageText;
        private float showTime = 2.6f;
        private float waitTime = 1.7f;
        private float reactDistance = 2f;

        private float chanceToNotReact = 0.35f;

        public static List<string> citizensReplicas = new List<string>()
        {
            "Че..",
            "А?",
            "Да блять!",
            "Ну снова..",
            "Завались!",
            "Революционер!",
            "Как тут спать?",
            "Заткните его!",
            "Опять орет",
            "Сука завали",
            "Аааааааа",
            "Утро доброе блять",
            "Жена, где мое ружье!?",
            "Ну и крики у тебя",
            "Вот я в тюрьме сидел..",
            "Ничтожная дешевка!",
            "Кто тебя из параши выпустил?",
            "Да ну нахер",
            "Еды нет, денег нет и еще это",
            "Халера!",
            "Да когда же это кончится!",
            "Его бы в армию отправить",
            "Приходите голосовать на выборах!",
            "И это все?",
            "Давай еще громче, мудак!",
            "Давай еще громче, мудак!",
            "Как он все время сюда залазит?",
            "Пришло время бить ебало",
            "Щас, штаны натяну и пизда ему",
            "Так хорошо спал..",
            "Да лучше бы я с войны не возвращался!",
            "Проклятый блять",
            "Свинья!",
            "Вали в другое место орать!",
            "Ебанутый",
            "Ты же понимаешь, что ты ебанутый?",
            "Блять, снова работать",
            "О громкий век петушиных оров!",
            "Да вы все сами громче орете!",
            "Соседи, заебали орать!",
            "Сам завались!",
            "Стены трясутся..",
            "Ты, петух!",
            "Завали, мне жены хватает!",
            "Господи, прости его",
            "Вы все ебанутые, я ухожу",
            "I dont want live here..",
            "What am i doing here?",
            "Своего первого петуха я зарежу сейчас",
            "Свали нахуй!",
            "Так хорошо спал..",
            "Почти каждый день орет, заебал",
            "Петушара!",
            "Клянусь, я щас выйду!",
            "Че ему все время надо",
            "Какждый раз под моим окном!",
            "Идем сожжем его!",
            "Распять петуха ебучего!",
            "Ща его успокою",
            "Выйду и отпижжу",
            "Ну все, ты нарвался!",
            "Щас друзей позову и тебе пизда!"
        };

        public int CitizensAwakeCount
        {
            get => Model.GetInt("CitizensAwakeCount");
            set => Model.Set("CitizensAwakeCount", value);
        }


        public bool isEnqueued = false;

        public CitizensReplicasQueue citizensReplicasQueue;

        [Bind]
        public void CockMakeNoise()
        {
            if(Random.value >= chanceToNotReact) return;
            
            Vector3 playerPos = Model.Get<Vector3>("PlayerPos");

            if (Vector3.Distance(playerPos, transform.position) <= reactDistance)
            {
                if(isEnqueued) return;

                
                Model.EventManager.Invoke("OnCitizenAwake");
                
                isEnqueued = true;
                citizensReplicasQueue.CitizenReplicaBoxes.Enqueue(this);
            }
        }

        public void ShowReplica()
        {
            if(Path.IsPlaying) return;
            
            CitizensAwakeCount++;
            
            hideShowTransform.gameObject.SetActive(true);
            messageText.text = citizensReplicas[Random.Range(0, citizensReplicas.Count)];

            Path = CPath.Create()
                .Wait(showTime)
                .Action(() => hideShowTransform.gameObject.SetActive(false))
                .Wait(waitTime)
                .Action(() => isEnqueued = false);
        }
    }
}
