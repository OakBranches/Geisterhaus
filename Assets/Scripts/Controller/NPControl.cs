using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NPControl : MonoBehaviour
{
    public Light2D[] luzes = new Light2D[6];
    private bool[,] rooms = new bool[2,3]; 
    MiniCameraFrame minicamera;
    private int[] ghost = new int[2]; 
    string currentRoom;
    private int[] oldghost = new int[2];
    private int[] npc = new int[2];
    public float  speed;
    RoomDriver roomDriver;
    private List<Vector2> lista= new List<Vector2>(); 
    // Start is called before the first frame update
    void Start()
    {
        minicamera = FindObjectOfType<MiniCameraFrame>();
        roomsInit();
        npcInit();
        roomDriver = GetComponent<RoomDriver>();
        print(npc[0]+" -  "+npc[1]);
    }
    void MOVE(int a,int b){
        lista = new List<Vector2>();
        roomsInit();
        int[] virtualNPC = new int[2];
        virtualNPC[0]=npc[0];
        virtualNPC[1]=npc[1];
        int i=0;
        float piso=0;
        while( (virtualNPC[0] != a || virtualNPC[1] != b)&&i<10){
             i++;
             piso = virtualNPC[0]==1?-3.98f:-33.1f;
            
            //os andares são diferentes
            if (a != virtualNPC[0] ) {
                //go to corredor
                if(virtualNPC[1]==1){
                    //no corredor
                    lista.Add(new Vector2((1>0?1:-1)*11f,piso));
                    virtualNPC[0]=a;
                     print("1 " +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }else{
                    //qqr sala adj.
                    lista.Add(new Vector2((virtualNPC[1]>b?1f:-1f)*41.5f,piso));
                    virtualNPC[0]=1;

                     print("2 " +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }
            }else{
                //andares iguais
                if(b==1){
                    //pegar a porta para ir para o corredor
                    lista.Add(new Vector2((virtualNPC[1]>b?1:-1)*41.5f,piso));
                    virtualNPC[0]=1;
                     print("3 "  +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                     
                }else{
                    //seguir reto do corredor
                    if(virtualNPC[1]==1)
                        lista.Add(new Vector2((virtualNPC[1]>b?-1:1)*8.5f,piso));
                    //seguir reto dos quartos
                    else{
                        lista.Add(new Vector2((virtualNPC[1]>b?1:-1)*41.5f,piso));
                    }
                     
                     virtualNPC[1]+=(virtualNPC[1]>b?-1:1);

                     print("4 "+ virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }

            }

        }
    }
    void move(){

        roomDriver.canEnter=false;
        if(lista.Count!=0){
            float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, lista[0], step);
            print("toem "+transform.position+"quero ir para"+ lista[0]);
            if(new Vector2(transform.position.x,transform.position.y)== lista[0]){
                roomDriver.canEnter=true;
                lista.Remove(lista[0]);
            
            }else{
                roomDriver.canEnter=false;
            }
        }
    }
    void ghostInit(){
        if(currentRoom!=null){
            if(currentRoom.Contains("Kitchen")){
                ghost[0] = 0;
                ghost[1] = 2;
            
            }if(currentRoom.Contains("Living")){
                ghost[0] = 0;
                ghost[1] = 0;
            }if(currentRoom.Contains("Downstair")){
                ghost[0] = 0;
                ghost[1] = 1;
            }if(currentRoom.Contains("Child")){
                ghost[0] = 1;
                ghost[1] = 0;
            }if(currentRoom.Contains("Upstair")){
                ghost[0] = 1;
                ghost[1] = 1;
            }if(currentRoom.Contains("Parent")){
                ghost[0] = 1;
                ghost[1] = 2;
            }
        
        }
    
    }

    void roomsInit(){
        for(int i=0;i<6;i++){
            if(luzes[i].transform.name.Contains("Kitchen"))
                rooms[0,2]=luzes[i].enabled;
            if(luzes[i].transform.name.Contains("Living"))
                rooms[0,0]=luzes[i].enabled;
            if(luzes[i].transform.name.Contains("Main"))
                rooms[0,1]=false;
            if(luzes[i].transform.name.Contains("Child"))
            rooms[1,0]=luzes[i].enabled;
            if(luzes[i].transform.name.Contains("Upstair"))
                rooms[1,1]=luzes[i].enabled;
            if(luzes[i].transform.name.Contains("Parent"))
            rooms[1,2]=luzes[i].enabled;
        }
    }
    void npcInit(){        
        npc[0]= (int)this.transform.position.y>(-10)?1:0;
        npc[1]=(int)this.transform.position.x<-28?0:((int)this.transform.position.x>28?2:1);}
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            MOVE(1,0);
        currentRoom = minicamera.getCurrentRoom();
        ghostInit();
        oldghost[0] = ghost[0];
        oldghost[1] = ghost[1];
        move();
    }

}
