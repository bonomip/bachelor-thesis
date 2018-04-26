using UnityEngine;
using System.Collections;

namespace Application.Component.TankAi {
    public class ProvaMira : MonoBehaviour {
        
        private Tank.Main main;
        private Transform exitHole;
        private Transform gun;
        private Transform turret;   // from -135 left, to -45 right
                                    // local rotation to euler
                                    // left     y   -2.4
                                    // center   y   -1.6
                                    // right    y   -0.8 minus 0.05

    
        private bool[] inputs;      // from 4 to 8
                                    // 4 rotate turret left, 5 right
                                    // 6 rotate gun up, 7 down
                                    // 8 shoot

        void Start(){
            this.inputs = new bool[9];
            this.main = this.gameObject.GetComponent<Tank.Main>();
            this.exitHole = this.main.gun.getExitHole();
            this.gun = this.main.gun.transform;
            this.turret = this.main.turret.transform;

            //Vector3 s = Quaternion.AngleAxis(45, Vector3.up) * turretAim; //45 is top right, -45 is top left
            //Smoke.Create(s+this.turret.position, null);
            





            // creare vec2 V (senza la y) passante per p1 ( turret.position ) e p2 (exitHole.position)
            // creare vec2 W (senza la y) passante per p2 ( exitHole.position ) e p3 (target.position)
            // calcolare l'angolo ß tra V e W, sommarci l'angolo ∂ (la corrente rotazione della torretta)
            // se ß+∂ > 45 il tank deve girare verso sinistra
            // se ß+∂ < -45 il tank deve girare verso destra




            for(int i = 0; i < this.inputs.Length; i++) this.inputs[i] = false;
        }

        public Vector3 turretAim{
            get { return (this.exitHole.position - this.turret.position); }
        }

        private void Update(){
            this.inputs[5] = true;
        }

        private void FixedUpdate(){
            this.main.input(this.inputs);
        }





    }
}