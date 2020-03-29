using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Up : MonoBehaviour
{
    Collider2D collider;
    public float duracao;
    private float  inicio;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        inicio = Time.time;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Player"){
            int j = Random.Range(0,10);
            int i = j%2;
            if(i==1){
                //-0.1 até 1
                if(player.GetComponent<Fire>().projectileTimer >1)
                    player.GetComponent<Fire>().projectileTimer -=0.1f;
            }else{
                //+0.25 até 12
                if(player.GetComponent<Player>().moveSpeed<12)
                    player.GetComponent<Player>().moveSpeed+=0.25f;
            }
            Destroy(this);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time-inicio>duracao)
            Destroy(this);
    }
}
