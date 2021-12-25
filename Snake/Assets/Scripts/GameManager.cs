using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   
   public List<GameObject> Cubes;
   public GameObject SnakeController;
   public GameObject blackPanel;
   public GameObject CubeParticle;
   public GameObject ScorePanel;
   public GameObject confettiParticle;
   public GameObject CoinPrefab;
   
   [HideInInspector] public int score;
   [HideInInspector] public bool finished = false;
   private Color panelColor;
   private float colorAlpha;
   
   private void Start()
   {
      Instance = this;
      
      panelColor = Color.black;
   }

   private void Update()
   {
      CheckFinish();
      ScorePanel.GetComponentInChildren<TextMeshProUGUI>().text = score.ToString(); // Skorun ekrandaki texte aktarılması
   }
   private void CheckFinish()
   {
      if (!finished && Cubes.Count == 0)
      {
         finished = true;
         SnakeController.GetComponent<SnakeController>().head.GetComponent<Rigidbody>().isKinematic = true;
      }
      if (finished)
      {
         FinishLevel();
      }
   }
   private void FinishLevel()
   {
      //Siyah arkaplanı saydamdan mata çevirerek aktif etme
      colorAlpha = Mathf.SmoothDamp(colorAlpha, 1,ref colorAlpha, Time.deltaTime*2);
      panelColor.a = colorAlpha;
      blackPanel.GetComponent<Image>().color = panelColor;
      
      if(panelColor.a >.9f)
      { 
         confettiParticle.SetActive(true); // Confetti
      }
   }

   public void coinScale()
   {
      StartCoroutine(takeCoin());
   }
   IEnumerator takeCoin()
   {
      for (int i = 1; i < SnakeController.GetComponent<SnakeController>().BodyParts.Count-1; i++)
      {
         if (DOTween.IsTweening(i)) // scale işlemi önceden başladıysa resetleme
         {
            SnakeController.GetComponent<SnakeController>().spline.SetPointSize(i,SnakeController.GetComponent<SnakeController>().splineSizes[i]);
         }
         
         float splineSize = SnakeController.GetComponent<SnakeController>().spline.GetPointSize(i);
         float endSize = splineSize * 1.75f;
         
         DOTween.To(()=> splineSize, x=> splineSize = x, endSize, .05f).SetLoops(2,LoopType.Yoyo).SetId(i).OnUpdate(() =>
         {
            SnakeController.GetComponent<SnakeController>().spline.SetPointSize(i,splineSize);
         });
         
         yield return new WaitForSecondsRealtime(.12f);
      }
   }
   
   
}
