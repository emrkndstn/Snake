using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cubes : MonoBehaviour
{
    public LayerMask snakeLayer;
    public Transform F_RayPoint, B_RayPoint, R_RayPoint, L_RayPoint;

    private Vector3 squeezePos;
    private bool squeezed = false;
    private bool destroyCall = false;
    void Update()
    {
        if (!squeezed) // Yılan tarafından sarılmadıysa
        {
            if (DetectSnake()) // Sarılmayı kontrol etme
            {
                squeezed = true;
                squeezePos /= 4; // Sarmanın orta noktasınu bulma
            }
        }
        else if (squeezed && !destroyCall)
        {
            destroyCall = true;
            DestroyCube();
        }
    }


    private bool DetectSnake()
    {
        //Kutunun çevresine ray atarak yılan tarafından sarıldığını kontrol etme
        squeezePos=Vector3.zero; // Sarmanın merkezine reset atma
        int hitCount = 0;
        RaycastHit hit;
        
        Debug.DrawRay(F_RayPoint.position, F_RayPoint.forward * .45f, Color.yellow);
        if (Physics.Raycast(F_RayPoint.position, F_RayPoint.forward, out hit, 1, snakeLayer))
        {
            hitCount++;
            squeezePos += hit.point; //Saran parçaların pozisyonun eklenmesi
        }
        
        Debug.DrawRay(B_RayPoint.position, -B_RayPoint.forward * .45f, Color.yellow);
        if (Physics.Raycast(B_RayPoint.position, -B_RayPoint.forward, out hit, 1, snakeLayer))
        {
            hitCount++;
            squeezePos += hit.point;
        }
        
        Debug.DrawRay(R_RayPoint.position, R_RayPoint.right * .45f, Color.yellow);
        if (Physics.Raycast(R_RayPoint.position, R_RayPoint.right, out hit, 1, snakeLayer))
        {
            hitCount++;
            squeezePos += hit.point;
        }
        
        Debug.DrawRay(L_RayPoint.position, -L_RayPoint.right * .45f, Color.yellow);
        if (Physics.Raycast(L_RayPoint.position, -L_RayPoint.right, out hit, 1, snakeLayer))
        {
            hitCount++;
            squeezePos += hit.point;
        }
        
        if (hitCount == 4) return true;
        else return false;
    }
    
    private void DestroyCube()
    {
        transform.DOScale(1.5f, .5f).SetEase(Ease.Linear);
        transform.DOMove(new Vector3(squeezePos.x, transform.position.y, squeezePos.z), .5f).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                GameManager.Instance.score += 100;
                GameManager.Instance.Cubes.Remove(this.gameObject);
                Instantiate(GameManager.Instance.CubeParticle, transform.position, Quaternion.identity);
                for (int i = 0; i < 3; i++)
                {
                    GameObject coin = Instantiate(GameManager.Instance.CoinPrefab, transform.position, Quaternion.identity);
                    coin.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5,5),5,Random.Range(-5,5)),ForceMode.VelocityChange);
                }
                Destroy(this.gameObject);
            });
    }
    
    
}
