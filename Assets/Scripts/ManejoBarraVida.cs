using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ManejoBarraVida : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float maximoVida;
    [SerializeField] private BarraVida barraVida;

    // Start is called before the first frame update
    void Start()
    {
        vida=maximoVida;
        barraVida.InicializarBarraVida(vida);
    }

    // Update is called once per frame
    public void TomarDaño(float daño)
    {
        vida -= daño;
        barraVida.CambiarVidaActual(vida);
        if (vida <= 0) 
        { 
        Destroy(gameObject);
        }
    }


}
