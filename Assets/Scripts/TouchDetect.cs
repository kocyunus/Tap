using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Oyuncu ekrana dokunduğu zaman dokunduğu nesneyi touchedObject diye tutar bunu
 * oyunun rastgele seçtiği kare ile eşleşip eşleşmediğini kontrol eder
 *
 * 
 * */
public class TouchDetect : MonoBehaviour
{
    Vector3 touchPosWorld;

   
    TouchPhase touchPhase = TouchPhase.Ended;

    void Update()
    {
       
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
        {
           
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

           
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
              
                GameObject touchedObject = hitInformation.transform.gameObject;
                
                if (touchedObject == GameManager._MyInstance._selectBlok)
                {
                    GameManager._MyInstance._levelChanged = true;
                    GameManager._MyInstance._blockSelected = false;
                    GameManager._MyInstance._skoreCount++;
                    GameManager._MyInstance.LevelChanger();  
                }
                else
                {
                   
                    GameManager._MyInstance.GameOver();
                }
          
                Debug.Log("Touched " + touchedObject.transform.name);
                
            }
        }
    }
}
