using System;
using UnityEngine;

namespace OOOneUnityTools
{
    public class AutoOrderInLayer : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        public Transform pivot;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            //若沒有指定座標參照物，則用自己的座標
            if (pivot == null)
                pivot = gameObject.transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            autoOrderInLayer();
        }

        void autoOrderInLayer()
        {
            //每個Frame把物件按照先後關係在orderInLayer中排序
            int objZ = Convert.ToInt32(pivot.position.z * 100.0f);
            int orderInLayer = -objZ;
            switch (gameObject.tag)
            {
                //因Light物件沒有SpriteRender，所以註解掉。
                /*case "Lights":
                    orderInLayer += 9;
                    break;*/
                case "Ef_SpriteLighting":
                    orderInLayer += 8;
                    break;
                case "Ef_SpriteSolid":
                    orderInLayer += 7;
                    break;
                default:
                    break;
            }

            spriteRenderer.sortingOrder = orderInLayer;
        }
    }
}