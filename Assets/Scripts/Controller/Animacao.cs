using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Animacao : MonoBehaviour
{
    public GameObject pai,mae,filho,ghost,scene,priest;
    public Camera camera;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void CarroChegou(){
    // mae sai
        
        mae.SetActive(true);
        mae.GetComponent<Animator>().Play("sequisabe",4,0.5f);
  
      
    }
    void SaiCrianca(){
        filho.SetActive(true);
        filho.GetComponent<Animator>().Play("boy",4,0.5f);
        
    }
    void Fantasma(){
        ghost.SetActive(true);
        ghost.GetComponent<Animator>().Play("ghost",4,1f);
    }
    void SaiPai(){
    //o pai sai do carro
        pai.SetActive(true);
        pai.GetComponent<Animator>().Play("father",4,0.5f);
    }
    void Entrar(){
        scene.SetActive(false);
        pai.GetComponent<Animator>().SetBool("entrou", true);
        filho.GetComponent<Animator>().SetBool("entrou", true);
        mae.GetComponent<Animator>().SetBool("entrou", true);
    }
    void CloseP(){

        pai.GetComponent<Animator>().SetBool("fan", true);
        camera.GetComponent<Animator>().SetBool("CHILD", true);
    }
    void FINAL(){
        priest.SetActive(true);
        camera.GetComponent<Animator>().SetBool("FAT", true);
    }
    void CloseF(){

        filho.GetComponent<Animator>().SetBool("fan", true);
        camera.GetComponent<Animator>().SetBool("MOM", true);
    }
    void CloseM(){
        mae.GetComponent<Animator>().SetBool("fan", true);
        camera.GetComponent<Animator>().SetBool("fan", true);
    }
    void ChangeScene(){
        SceneManager.LoadScene("Game");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            ChangeScene();
    }
}
