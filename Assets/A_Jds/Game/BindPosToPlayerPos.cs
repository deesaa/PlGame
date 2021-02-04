using System;
using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class BindPosToPlayerPos : MonoBehaviourExtBind
    {
        public Transform camera;
        public Vector3 targetPos;

        [Bind]
        public void OnPlayerPosChanged()
        {
            if(Model.GetBool("CameraBindLock", false)) return;
            
            targetPos = Model.Get<Vector3>("PlayerPos");
        }

        [OnUpdate]
        public void update()
        {
            if(Model.GetBool("CameraBindLock", false)) return;
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPos, 0.05f);
        }
        

        /*private void LateUpdate()
        {
            if(Model.GetBool("CameraBindLock", false)) return;
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
        }*/

        /*[OnUpdate]
        public void OnUpdate()
        {
            if(Model.GetBool("CameraBindLock", false)) return;
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
        }*/
    }
}
