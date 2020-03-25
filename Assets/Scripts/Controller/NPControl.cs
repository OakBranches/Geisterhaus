using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NPControl : MonoBehaviour
{
    public int NPC_LayerN,Enemy_LayerN;
    private Light2D[,] luzes = new Light2D[2,3];
    public  Light2D Kitchen,Upstair,Downstair,Child,Living,Parent;
    private bool[,] rooms = new bool[2,3]; 
    MiniCameraFrame minicamera;
    public string EnemyType;
    private int[] ghost = new int[2]; 
    string currentRoom;
    private int[] oldghost = new int[2];
    int andar,sala;
    private int[] npc = new int[2];
    public float  speed;
    RoomDriver roomDriver;
    private List<Vector2> lista= new List<Vector2>(); 
    // Start is called before the first frame update
    void Start()
    {
        minicamera = FindObjectOfType<MiniCameraFrame>();
        LightsInit();
        roomsInit();
        npcInit();
        roomDriver = GetComponent<RoomDriver>();
        ////print(npc[0]+" -  "+npc[1]);
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
                    int p=Random.Range(0,1);
                    lista.Add(new Vector2((p>0.5f?1:-1)*11f,piso));
                    //print(p);
                    virtualNPC[0]=a;
                     //print("1 " +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }else{
                    //qqr sala adj.
                    lista.Add(new Vector2((virtualNPC[1]>1?1f:-1f)*41.5f,piso));
                    virtualNPC[1]=1;

                     //print("2 " +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }
            }else{
                //andares iguais
                if(b==1){
                    //pegar a porta para ir para o corredor
                    lista.Add(new Vector2((virtualNPC[1]>b?1:-1)*41.5f,piso));
                    virtualNPC[1]=1;
                     //print("3 "  +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                     
                }else{
                    //seguir reto do corredor
                    if(virtualNPC[1]==1)
                        lista.Add(new Vector2((virtualNPC[1]>b?-1:1)*8.5f,piso));
                    //seguir reto dos quartos
                    else{
                        lista.Add(new Vector2((virtualNPC[1]>b?1:-1)*41.5f,piso));
                    }
                     
                     virtualNPC[1]+=(virtualNPC[1]>b?-1:1);

                     //print("4 "+ virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }

            }

        }
    }

    bool entrando = false;
    void move(){
        npcInit();
        roomDriver.canEnter=false;
        if(lista.Count!=0){
            
           float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, lista[0], step);
            if(gameObject.layer==Enemy_LayerN)
           // print("toem "+transform.position+"quero ir para"+ lista[0]+"ent: "+entrando);
            if(entrando)
                if(new Vector2(transform.position.x,transform.position.y)!= lista[0]){
                        lista.Remove(lista[0]);
                        entrando=false;
                        roomDriver.canEnter=false;
                        ////print("esta igual\n\n\n\n");
                        return;
              
            }
            if(new Vector2(transform.position.x,transform.position.y)== lista[0]){
                roomDriver.canEnter=true;
                //roomDriver.OnTriggerStay2D(GetComponent<Collider2D>());
                ////print("----------------------------------------------------");
                entrando=true;
                ////print("---------"+transform.position+"quero ir para"+ lista[0]+"-----------------------");

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

    void TurnOffLamps(){
        for(int y=0;y<2;y++)
            for(int u=0;u<3;u++)
                if(!(y==0&&u==1)&&rooms[y,u]==true)
                    luzes[y,u].enabled=false;
    }
    void LightsInit(){
        luzes[0,2]=Kitchen;
        luzes[0,0]=Living;
        luzes[0,1]=Downstair;
        luzes[1,0]=Child;
        luzes[1,1]=Upstair;
        luzes[1,2]=Parent;
    }
    void roomsInit(){
        rooms[0,2]=luzes[0,2].enabled;
        rooms[0,0]=luzes[0,0].enabled;
        rooms[0,1]=false;
        rooms[1,0]=luzes[1,0].enabled;
        rooms[1,1]=luzes[1,1].enabled;
        rooms[1,2]=luzes[1,2].enabled;
    }
    void npcInit(){        
        npc[0]= (int)this.transform.position.y>(-10)?1:0;
        npc[1]=(int)this.transform.position.x<-28?0:((int)this.transform.position.x>28?2:1);}
    // Update is called once per frame
    //booleana de estar no mesmo quarto que o fantasma
    bool run = false;
    //contador de reação
    float see = 0;
    void FoundGhost(){
        if(gameObject.layer==NPC_LayerN){
            do{
                andar = ((int)Random.Range(0,10))%2;
                sala = ((int)Random.Range(0,9))%3;
                run =true;
                MOVE(andar,sala);
            }while(andar==ghost[0]&&sala==ghost[1]);
        }else if (gameObject.layer==Enemy_LayerN){
            if(AllLamps()){
                GetComponent<Priest>().SetAttackMode(true);
                print("aqui !!!");
            }else{
                GetComponent<Priest>().SetAttackMode(false);
                NotGhostPlace();
            }
        }
    }
    void RunSuccess(){
        if(gameObject.layer==NPC_LayerN){
           run=false;
           see=Time.time;
        }else if(gameObject.layer==Enemy_LayerN){
           run=false;
           see=Time.time;
        }
    }
    bool AllLamps(){
        for(int y=0;y<2;y++)
            for(int u=0;u<3;u++)
                if(!(y==0&&u==1)&&rooms[y,u]==false)
                    return false;
        return true;
    }
    void NotGhostPlace(){
        if(gameObject.layer==NPC_LayerN){
            see=Time.time;
        }else if(gameObject.layer==Enemy_LayerN){
            
            if(AllLamps()){
                run=true;
                MOVE(ghost[0],ghost[1]);
            }else{
                luzes[npc[0],npc[1]].enabled = true;
                andar = ((int)Random.Range(0,10))%2;
                sala = ((int)Random.Range(0,9))%3;
                run =true;
                MOVE(andar,sala);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TurnOffLamps();

        npcInit();
        if(npc[0]==ghost[0] && npc[1]==ghost[1] && run==false && (Time.time - see>1 ) ){
            print("1"+run+"    "+(Time.time-see));
           FoundGhost();
        }else if(run==true && !( npc[0]==ghost[0] && npc[1]==ghost[1]) ){
            RunSuccess();
             print("2"+run+"    "+(Time.time-see));
        }else if(!( npc[0]==ghost[0] && npc[1]==ghost[1]) ){
            NotGhostPlace();
            print("3"+run+"    "+(Time.time-see)+ "   "+ npc[0]+" " +ghost[0] +" " +npc[1]+" "+ghost[1]) ;
        }

        currentRoom = minicamera.getCurrentRoom();
        ghostInit();
        oldghost[0] = ghost[0];
        oldghost[1] = ghost[1];
        move();
    }

}
