using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Start_Menu
{
    public class SceneLoader : MonoBehaviour
    {

        public void LoadScene(string name) => SceneManager.LoadScene(name);

        public void Quit() => Application.Quit();

        public void OpenCredits()
        {
            var canvasO = GameObject.Find("MenuParent");
            var canvas = canvasO.GetComponentsInChildren<Transform>(true);
            foreach (var trans in canvas)
            {
                try
                {
                    trans.gameObject.SetActive(false);
                }
                catch (Exception ignored)
                {
                }
            }
            canvasO.SetActive(true);


            var credO = GameObject.Find("CreditsParent");
            var cred = credO.GetComponentsInChildren<Transform>(true);
            foreach (var trans in cred)
            {
                try
                {
                    trans.gameObject.SetActive(true);
                }
                catch (Exception ignored)
                {
                }
            }
            credO.SetActive(true);
            
           
            
        }
        public void CloseCredits()
        {

            var canvasO = GameObject.Find("MenuParent");
            var canvas = canvasO.GetComponentsInChildren<Transform>(true);
            foreach (var trans in canvas)
            {
                try
                {
                    trans.gameObject.SetActive(true);
                }
                catch (Exception ignored)
                {
                }
            }
            canvasO.SetActive(true);


            var credO = GameObject.Find("CreditsParent");
            var cred = credO.GetComponentsInChildren<Transform>(true);
            foreach (var trans in cred)
            {
                try
                {
                    trans.gameObject.SetActive(false);
                }
                catch (Exception ignored)
                {
                }

                
            }
            credO.SetActive(true);
            
        }
        

    }
}
