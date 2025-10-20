//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class TreeSpawner : MonoBehaviour
//{
//    public GameObject tree1Object;
//    //public GameObject tree2Object;
//    public int treesCount = 30;
//    public float radius = 35f;
//    public float diffTree = 0.6f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        for (int i = 0; i < treesCount; i++)
//        {
//            float angle = 2 * Mathf.PI * i / treesCount;
//            Vector3 position1 = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));

//            //Vector3 position2 = new Vector3(radius * Mathf.Cos(angle+diffTree), 0, radius * Mathf.Sin(angle+ diffTree));

//            Instantiate(tree1Object, position1, Quaternion.Euler(0,angle,0));

//            //Instantiate(tree2Object, position2, Quaternion.identity);
//        }
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject tree1Object;
    public int treesCount = 30;
    public float radius = 35f;

    void Start()
    {
        for (int i = 0; i < treesCount; i++)
        {
            // Calcul de la position sur le cercle
            float angle = 2 * Mathf.PI * i / treesCount;
            Vector3 position = new Vector3(
                radius * Mathf.Cos(angle),
                0,
                radius * Mathf.Sin(angle)
            );

            // Création de la rotation vers le centre
            Quaternion rotation = Quaternion.LookRotation(-position.normalized, Vector3.up);

            // Instanciation de l’arbre
            Instantiate(tree1Object, position, rotation, transform);
        }
    }
}
