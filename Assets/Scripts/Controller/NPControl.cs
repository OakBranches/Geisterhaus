using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NPControl : MonoBehaviour
{
    public int NPC_LayerN, Enemy_LayerN;

    private Light2D[,] luzes = new Light2D[2,3];
    public  Light2D Kitchen, Upstair, Downstair, Child, Living, Parent;

    private bool[,] rooms = new bool[2,3]; 
    MiniCameraFrame minicamera;
    public string EnemyType;
    private int[] ghost = new int[2]; 
    Enemy enemy;
    public Animator animator;
    string currentRoom;
    private int[] oldghost = new int[2];
    int andar, sala;
    private int[] npc = new int[2];
    public float  speed;
    RoomDriver roomDriver;
    GameObject player;
    private List<Vector2> lista = new List<Vector2>(); 
    public bool facingRight { get; private set; }
    bool entrando = false;
    float tamanhoDaSala = 16f;
    bool geradoAleatoriamente = false;

    // Start is called before the first frame update
    void Start()
    {   
        FindLights();
        facingRight = true;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        minicamera = FindObjectOfType<MiniCameraFrame>();
        LightsInit();
        roomsInit();
        npcInit();
        roomDriver = GetComponent<RoomDriver>();
        enemy = (gameObject.layer == Enemy_LayerN ? GetComponent<Enemy>() : null);
        TurnOffLamps();
        //print(npc[0]+" -  "+npc[1]);
    }

    void FindLights()
    {
        GameObject lights = GameObject.FindGameObjectWithTag("LightContainer");
        foreach (Transform child in lights.transform)
        {
            if (child.name == "Kitchen Light")
            {
                Kitchen = child.GetComponent<Light2D>();
            }
            else if (child.name == "Upstairs Corridor Light")
            {
                Upstair = child.GetComponent<Light2D>();
            }
            else if (child.name == "Parents' Bedroom Light")
            {
                Parent = child.GetComponent<Light2D>();
            }
            else if (child.name == "Main Light")
            {
                Downstair = child.GetComponent<Light2D>();
            }
            else if (child.name == "Child's Bedroom Light")
            {
                Child = child.GetComponent<Light2D>();   
            }
            else if (child.name == "Living Room Light")
            {
                Living = child.GetComponent<Light2D>();
            }
        }
    }

    void MOVE(int a, int b)
    {
        MOVE(a, b, Vector2.zero);
    }

    void MOVE(int a, int b, Vector2 toAdd)
    {
        lista = new List<Vector2>();
        roomsInit();
        int[] virtualNPC = new int[2];
        virtualNPC[0] = npc[0];
        virtualNPC[1] = npc[1];
        int i = 0;
        float piso = 0;
        if (toAdd != Vector2.zero)
        {
            lista.Add(toAdd);
        }

        while ((virtualNPC[0] != a || virtualNPC[1] != b) && i < 10)
        {
            i++;
            piso = virtualNPC[0] == 1 ? -4f : -33f;
            if (gameObject.name == "Boy")
            {
                piso -= 0.5f;
            }
            
            //os andares são diferentes
            if (a != virtualNPC[0])
            {
                //go to corredor
                if(virtualNPC[1] == 1)
                {
                    //no corredor
                    int p = Random.Range(0,1);
                    lista.Add(new Vector2((p > 0.5f ? 1 : -1) * 11f, piso));
                    //print(p);
                    virtualNPC[0] = a;
                     //print("1 " + virtualNPC[0] +"  to em  " + virtualNPC[1] );
                }
                else
                {
                    //qqr sala adj.
                    lista.Add(new Vector2((virtualNPC[1]>1?1f:-1f)*41.5f,piso));
                    virtualNPC[1] = 1;

                     //print("2 " + virtualNPC[0] +"  to em  " + virtualNPC[1] );
                }
            }
            else
            {
                //andares iguais
                if(b==1)
                {
                    //pegar a porta para ir para o corredor
                    lista.Add(new Vector2((virtualNPC[1]>b?1:-1)*41.5f,piso));
                    virtualNPC[1]=1;
                    //print("3 "  +virtualNPC[0] +"  to em  " +virtualNPC[1] );
                     
                }
                else
                {
                    //seguir reto do corredor
                    if(virtualNPC[1]==1)
                        lista.Add(new Vector2((virtualNPC[1]>b?-1:1)*8.5f,piso));
                    //seguir reto dos quartos
                    else
                    {
                        lista.Add(new Vector2((virtualNPC[1]>b?1:-1)*41.5f,piso));
                    }
                     
                    virtualNPC[1]+=(virtualNPC[1]>b?-1:1);

                    //print("4 "+ virtualNPC[0] +"  to em  " +virtualNPC[1] );
                }

            }

        }
    }

    void move(){
        npcInit();
        roomDriver.canEnter=false;
        bool o = gameObject.layer != Enemy_LayerN ? true : !(enemy.getAttackMode());

        if(o){
            if (lista.Count != 0)
            {
                CalculateMovement();
                //if(gameObject.layer==Enemy_LayerN)
                //print("to em " + transform.position + "quero ir para" + lista[0] + " ent: "+entrando);
                if (entrando)
                {
                    entrando = false;
                    roomDriver.canEnter = false;
                    lista.Remove(lista[0]);
                    return;
                }
                if (new Vector2(transform.position.x, transform.position.y) == lista[0])
                {
                    if (!geradoAleatoriamente)
                    {
                        roomDriver.canEnter=true;
                        entrando = true;
                    }
                    else if (geradoAleatoriamente)
                    {
                        geradoAleatoriamente = false;
                        lista.Remove(lista[0]);
                    }
                    ////print("---------"+transform.position+"quero ir para"+ lista[0]+"-----------------------");
                }
            }
            else if ((ghost[0] != npc[0] || ghost[1] != npc[1])
                && gameObject.layer != Enemy_LayerN)
            {
                float toWhere = Random.Range((npc[1] - 1) * 50.5f - tamanhoDaSala / 2,
                    (npc[1] - 1) * 50.5f + tamanhoDaSala / 2f);
                    
                geradoAleatoriamente = true;
                lista.Add(new Vector2(toWhere, transform.position.y));
            }
        }
        else if(gameObject.layer == Enemy_LayerN){
            if(enemy.getAttackMode() && npc[0] == ghost[0] && npc[1] == ghost[1]){
                enemy.AttackMove(speed);  
            }
        }
    }
    void ghostInit(){
        if(currentRoom!=null){
            if(currentRoom.Contains("Kitchen")){
                ghost[0] = 0;
                ghost[1] = 2;
            }else if(currentRoom.Contains("Living")){
                ghost[0] = 0;
                ghost[1] = 0;
            }else if(currentRoom.Contains("Downstair")){
                ghost[0] = 0;
                ghost[1] = 1;
            }else if(currentRoom.Contains("Child")){
                ghost[0] = 1;
                ghost[1] = 0;
            }else if(currentRoom.Contains("Upstair")){
                ghost[0] = 1;
                ghost[1] = 1;
            }else if(currentRoom.Contains("Parent")){
                ghost[0] = 1;
                ghost[1] = 2;
            }
        }
    }

    void TurnOffLamps()
    {
        for(int y=0;y<2;y++)
            for(int u=0;u<3;u++)
                if(!(y==0&&u==1)&&rooms[y,u]==true)
                    luzes[y,u].enabled = false;
    }

    void LightsInit()
    {
        luzes[0,0]=Living;
        luzes[0,1]=Downstair;
        luzes[0,2]=Kitchen;
        luzes[1,0]=Child;
        luzes[1,1]=Upstair;
        luzes[1,2]=Parent;
    }

    void roomsInit()
    {
        rooms[0,2]=luzes[0,2].enabled;
        rooms[0,0]=luzes[0,0].enabled;
        rooms[0,1]=false;
        rooms[1,0]=luzes[1,0].enabled;
        rooms[1,1]=luzes[1,1].enabled;
        rooms[1,2]=luzes[1,2].enabled;
    }

    void npcInit()
    {        
        npc[0]= (int) this.transform.position.y > -10 ? 1 : 0;
        npc[1]= (int) this.transform.position.x < -28 ? 0 : 
            ((int) this.transform.position.x > 28 ? 2 : 1);
        //print("------------->"+(int)this.transform.position.x);
    }

    // Update is called once per frame
    //booleana de estar no mesmo quarto que o fantasma
    bool run = false;
    //contador de reação
    float see = 0;

    void FoundGhost(){
        if(gameObject.layer == NPC_LayerN && !geradoAleatoriamente){
            do
            {
                andar = ((int)Random.Range(0,10))%2;
                sala = ((int)Random.Range(0,9))%3;
                run = true;
                MOVE(andar, sala);
            }
            while (andar == ghost[0] && sala == ghost[1]);
        }
        else if (gameObject.layer == Enemy_LayerN)
        {
            npcInit();
        
            enemy.SetAttackMode(true);
            //print("aqui !!!");s
        }
    }
    
    Vector3 oldP;
    void RunSuccess(){
        if (gameObject.layer == NPC_LayerN){
           run = false;
           see = Time.time;
        }
        else if (gameObject.layer == Enemy_LayerN)
        {
            see = Time.time;
            if (entrando == true)
                run = false;
        }

        if (oldP == transform.position && AllLamps() &&
            run == true && !enemy.getAttackMode() && lista.Count == 0)
        {
            //print("bugfix");
            MOVE(ghost[0],ghost[1]);
        }
        oldP=transform.position;
    }

    bool AllLamps(){
        for (int y = 0; y < 2; y++)
            for (int u = 0; u < 3; u++)
                if (!(y == 0 && u == 1) && rooms[y, u] == false)
                    return false;
        return true;
    }

    void NotGhostPlace(){
        if(gameObject.layer == NPC_LayerN)
        {
            see = Time.time;
        }
        else if(gameObject.layer == Enemy_LayerN){

            if(AllLamps())
            {
                run = true;
                MOVE(ghost[0], ghost[1]);
            }
            else
            {
                if(run == false)
                {
                    do
                    {
                        andar = ((int)Random.Range(0, 10))%2;
                        sala = ((int)Random.Range(0, 9))%3;
                        run =true;
                        if (!luzes[npc[0], npc[1]].enabled)
                            MOVE(andar, sala, luzes[npc[0], npc[1]].transform.position);
                        else
                            MOVE(andar, sala);
                    }
                    while(luzes[andar,sala].enabled == true && !AllLamps());
                }
            }
        }
    }
    void Update()
    {
        npcInit();
        if (npc[0] == ghost[0] && npc[1] == ghost[1] &&
            (run == false && (Time.time - see > 1 ) ||
            gameObject.layer == Enemy_LayerN) &&
            !geradoAleatoriamente)
        {
            FoundGhost();
        }
        else if(run==true && !(npc[0]==ghost[0] && npc[1]==ghost[1]))
        { 
            RunSuccess();
            if(gameObject.layer==Enemy_LayerN)
            {
                enemy.SetAttackMode(false);
            }
        }
        else if (!(npc[0] == ghost[0] && npc[1] == ghost[1]))
        {
            NotGhostPlace();
            if (gameObject.layer==Enemy_LayerN)
            {
                enemy.SetAttackMode(false);
            }
        }

        currentRoom = minicamera.getCurrentRoom();
        ghostInit();
        oldghost[0] = ghost[0];
        oldghost[1] = ghost[1];
        
        move();
    }

    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1f;
        transform.localScale = newScale;
        facingRight = !facingRight;
    }

    void CalculateMovement()
    {
        float step =  speed * Time.deltaTime; // calculate distance to move
        if (geradoAleatoriamente)
        {
            step /= 2f;
        }

        float toWhere = lista[0].x - transform.position.x;
        Vector3 velocity = new Vector3(speed, 0f, 0f);
        if (toWhere < -0.05)
        {
            velocity.x *= -1f;
        }

        if (-0.05f < toWhere && toWhere < 0.05f)
        {
            velocity.x = 0f;
        }

        if ((velocity.x < 0f && facingRight) ||
            (velocity.x > 0f && !facingRight))
        {
            Flip();
        }

        transform.position = Vector3.MoveTowards(transform.position, lista[0], step);

        if (velocity.x != 0)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }
}