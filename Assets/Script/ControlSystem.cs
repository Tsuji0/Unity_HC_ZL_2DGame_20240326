﻿using UnityEngine;

namespace Tsuji
{
    public class ControlSystem : MonoBehaviour
    {
        #region 資料區域
        //private 私人:只有這個腳本能存取的資料
        //float 浮點數:存放小數點資料
        //SerializeField 序列化欄位:顯示此資料屬性面板
        //Header 標題:在屬性面板上顯示標題文字
        //Range 範圍:設定數值資料範圍
        [SerializeField, Header("移動速度"), Range(0, 50)]
        private float moveSpeed = 3.5f;
        [SerializeField, Header("跳躍高度"), Range(0, 1000)]
        private float jump = 350;
        [SerializeField, Header("剛體元件")]
        private Rigidbody rig;
        [SerializeField, Header("動畫元件")]
        private Animator ani;
        [SerializeField, Header("檢查地板尺寸")]
        private Vector3 checkGroundSize;
        [SerializeField, Header("檢查地板位移")]
        private Vector3 checkGroundOffset;
        [SerializeField, Header("地板圖層")]
        private LayerMask layerGround;

        //string 字串:存放文字資料
        private string parMove = "移動數值";
        private string parJump = "觸發跳躍";
        #endregion

        //繪製圖示事件:在編輯器 Unity 內繪製圖示輔助
        private void OnDrawGizmos()
        {
            //決定顏色(紅，綠，藍，透明度) 0 ~ 1
            Gizmos.color = new Color(1, 0.2f, 0.2f, 0.5f);
            //繪製圖示(角色座標 + 位移，尺寸)
            Gizmos.DrawCube(transform .position + checkGroundOffset , checkGroundSize ); 
        }

        //喚醒事件:播放遊戲後執行一次
        private void Awake()
        {    
            //print("ZZZZZZ");
            //print("<color=#ffffff>喚醒事件</color>");
        }

        //開始事件:喚醒事件後執行一次
        private void Start()
        {
            //print("<color=#ffff37>開始事件</color>");
        }

        //更新事件:開始事件後一秒約60次
        private void Update()
        {
            //軸向名稱
            //上下 WS - 垂直 (Vertical)
            //左右 AD - 水平 (Horizontal)
            //左 A -1, 右 D +1, 沒按 0

            //區域變數 h = 輸入 的 取得軸向(軸向名稱)
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            MoveAndAnimation(h, v);
            Flip(h);
            Jump();
        }

        /// <summary>
        /// 翻面方法
        /// </summary>
        /// <param name="h">玩家的水平線</param>
        private void Flip(float h)
        {
            //如果 h > 0 角度設定為0 (右)
            //如果 h < 0 角度設定為180(左)
            //三元運算子
            //布林值 ? 布林值為真結果 : 布林值為假結果

            //如果 h 絕對值 後 < 0.1 就跳出
            if (Mathf.Abs(h) < 0.1f) return;
            //整數 角度 = 當 h > 0 結果 0 度角，否則 180 度角
            int angle = h > 0 ? 0 : 180;


            //控制角度的方法
            //transform 此物件的變形資訊 : 座標、角度、尺寸
            //eulerAngles 歐拉角度
            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        /// <summary>
        /// 移動與動畫
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        /// 
        private void MoveAndAnimation(float h, float v)
        {
            //剛體 的 加速度 = 三維向量
            rig.velocity = new Vector3(h * moveSpeed ,rig.velocity.y , v * moveSpeed);
            //magnitude 將三維向量轉為浮點數
            //動畫 的 設定浮點數(參數名稱，浮點數值)
            ani.SetFloat(parMove, rig.velocity.magnitude / moveSpeed);
        }

       
        /// <summary>
        /// 是否在地板上
        /// </summary>
        /// <returns></returns>
        private bool IsGrounded()
        {
            //物理，立方體覆蓋(座標，尺寸半徑，角度，指定圖層)
            Collider[] hits = Physics.OverlapBox(
                transform.position + checkGroundOffset, 
                checkGroundSize / 2, Quaternion.identity);

            //print($"<color=#3f6>碰到的物件:{hits[0]?.name}</color>");
            //碰到物件的數量 大於0 表示有碰到地板


            return hits.Length > 0;
        }

        private void Jump()
        {  
            //如果 在地板上 並按下空白鍵
            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                //跳躍
                rig.AddForce(Vector3.up * jump);
                //動畫 設定觸發參數(觸發參數名稱)
                ani.SetTrigger(parJump);
            }
        
        }
           
        
        
    }
}
