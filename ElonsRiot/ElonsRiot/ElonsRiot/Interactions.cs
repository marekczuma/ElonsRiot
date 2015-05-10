using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public  class Interactions
    {
        InterationTypes interactionType;
        GameObject referenceObject;
        public Interactions(InterationTypes interType, GameObject gameObj)
        {
            this.interactionType = interType;
            this.referenceObject = gameObj;
        }


        public delegate void InteractionsDelegate(GameObject gameObject);
        public  event InteractionsDelegate InteractionEvent;
        private int p;
        private GameObject gameObject;
        public void Add()
        {
            if (interactionType == InterationTypes.door)
            {
                InteractionEvent += Methods.MoveDoor;
            }
            else if (interactionType == InterationTypes.box)
            {
                InteractionEvent += Methods.MoveBox;
            }
        }
        public void CallInteraction()
        {
            if (InteractionEvent != null)
                InteractionEvent(referenceObject);
        }
    }
    public enum InterationTypes
    {
        door,
        box
    }
    public static class Methods
    {
        static Player referencePlayer;
       public static void setPlayer(Player p)
       {
           referencePlayer = p;
           k = 0;
       }
       static int k;
        internal static void MoveBox(GameObject gameObject)
        {
            Vector3 newPosition = referencePlayer.newPosition - referencePlayer.oldPosition;
            float x=0, y =0 , z =0;
            if(newPosition.Z == 0)
            {
                z = 0;
            }
            else
            {
                if(newPosition.Z < 0)
                {
                    x =0.2f;
                }
                else { x=-0.2f;}
            }
            gameObject.ChangePosition(new Vector3(x,y,z));
           
            Debug.WriteLine("sie zrobilem " + k);
            k++;
        }

        internal static void MoveDoor(GameObject gameObject)
        {
            gameObject.ChangePosition(new Vector3(0f, 0f, 0.2f));
        }
    }
}
