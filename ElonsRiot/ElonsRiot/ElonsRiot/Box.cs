using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class Box
    {
        public Box()
        {
            ActualCorners = null;
            this.referencePlayer = null;
            this.corners = null;
            this.min = new Vector3(0, 0, 0);
            this.max = new Vector3(0, 0, 0);
            this.center = new Vector3(0, 0, 0);
            this.radiuses = new float[3];
            this.actualRadiuses = null;
        }
        public Vector3 center;
        public float[] radiuses;
        public Vector3 center2;
        Vector3 halfWidth;
        Player referencePlayer;
        GameObject referenceObject;
        public Vector3[] ActualCorners;
        public List<Vector3> actualRadiuses;
        public Vector3[] corners;
        public Vector3 min, max;
        public int length;
        public Plane plane;
            
        public Box(Player gameObj)
        {
            ActualCorners = new Vector3[6];
            this.referencePlayer = gameObj;
            this.referenceObject = gameObj;
      //      createBoudingBox();
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.min = gameObj.boundingBox.Min;
            this.max = gameObj.boundingBox.Max;
            this.center = gameObj.center;
            this.actualRadiuses = new List<Vector3>();
            this.radiuses = new float[3];
            GetRadius();
            CreateRadiuses();

        }
        public Box(GameObject gameObj, Player pla)
        {
            
            ActualCorners = new Vector3[6];
            this.referenceObject = gameObj;
            createBoudingBox();
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.min = gameObj.boundingBox.Min;
            this.max = gameObj.boundingBox.Max;
            this.referencePlayer = pla;
            this.center = gameObj.center;
            this.actualRadiuses = new List<Vector3>();
            this.radiuses = new float[3];
            GetRadius();
            CreateRadiuses();
        }
        public Box(GameObject gameObj,BoundingBox box, Player pla)
        {
            
            ActualCorners = new Vector3[6];
            this.referenceObject = gameObj;
            createBoudingBox();
            this.corners = new Vector3[8];
            this.corners = box.GetCorners();
            this.min = box.Min;
            this.max = box.Max;
            this.referencePlayer = pla;
            this.center = gameObj.center;
            this.actualRadiuses = new List<Vector3>();
            this.radiuses = new float[3];
            GetRadius();
            CreateRadiuses();
        }
        public void createBoudingBox()
        {
           referenceObject.Initialize();
            Matrix tmp = Matrix.Identity;
            if ((referenceObject.Rotation.Y != 0 && referenceObject.ObjectPath != "3D/ludzik/dude"))
            {
                tmp = referenceObject.MatrixWorld;
            }
            else
            {
                tmp = Matrix.CreateScale(referenceObject.Scale) * Matrix.CreateTranslation(referenceObject.Position);
            }



            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Matrix meshTransform = new Matrix();
            foreach (ModelMesh mesh in referenceObject.GameObjectModel.Meshes)
            {
                meshTransform = referenceObject.boneTransformations[mesh.ParentBone.Index] * tmp;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {

                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    // We have to use this as we do not know the make up of the vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                    // Find minimum and maximum xyz values for this mesh part
                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        // update our values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }

                // transform by mesh bone matrix
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);
            }
            if (referenceObject.ObjectPath == "3D/ludzik/dude")
            {
                meshMax.Z += 2;
                meshMin.Z -= 2;
                meshMax.Y += 2;
                meshMin.Y -= 2;
            }
         /*   if (referenceObject.ObjectPath == "3D/sciana/wall")
            {
                meshMin.X += 2;
            }*/
            referenceObject.boundingBox = new BoundingBox(meshMin, meshMax);
        }
        public void GetRadius()
        {
            center2.X = (corners[1].X + corners[7].X) / 2;
            center2.Y = (corners[1].Y + corners[7].Y) / 2;
            center2.Z = (corners[1].Z + corners[7].Z) / 2;
         //   max = referenceObject.boundingBox.Max;
           // min = referenceObject.boundingBox.Min;
        }

        public void CheckWhichCorners()
        {
            GetRadius();
            Vector3 tmp = new Vector3(0, 0, 0);
            //ruszyl się w lewo (-x)
            if (referencePlayer.oldPosition.X > referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (-z) ok
                    length = 2;
                    actualRadiuses.Clear();
                      tmp.X = (corners[4].X + corners[6].X) / 2;
                      tmp.Y = (corners[4].Y + corners[6].Y) / 2;
                      tmp.Z = (corners[4].Z + corners[6].Z) / 2;
                      actualRadiuses.Add(tmp);
                      tmp.X = (corners[4].X + corners[3].X) / 2;
                      tmp.Y = (corners[4].Y + corners[3].Y) / 2;
                      tmp.Z = (corners[4].Z + corners[3].Z) / 2;
                      actualRadiuses.Add(tmp);
                 /*   actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);*/
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (+z) ok
                     length = 2;
                     actualRadiuses.Clear();
                     tmp.X = (corners[1].X + corners[3].X) / 2;
                     tmp.Y = (corners[1].Y + corners[3].Y) / 2;
                     tmp.Z = (corners[1].Z + corners[3].Z) / 2;
                     actualRadiuses.Add(tmp);
                     tmp.X = (corners[4].X + corners[3].X) / 2;
                     tmp.Y = (corners[4].Y + corners[3].Y) / 2;
                     tmp.Z = (corners[4].Z + corners[3].Z) / 2;
                     actualRadiuses.Add(tmp);
               /*   actualRadiuses.Clear();
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);*/

                }
                else
                {
                    //tylko w lewo (-x) ok
                      length = 1;
                      actualRadiuses.Clear();
                      tmp.X = (corners[0].X + corners[7].X) / 2;
                      tmp.Y = (corners[0].Y + corners[7].Y) / 2;
                      tmp.Z = (corners[0].Z + corners[7].Z) / 2;
                      actualRadiuses.Add(tmp);
                    actualRadiuses.Clear();
            /*        actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);*/
                }
            }
            //ruszyl sie w prawo.. (+x) ok
            else if (referencePlayer.oldPosition.X < referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (-z) ok
                     length = 2;
                      actualRadiuses.Clear();
                      tmp.X = (corners[5].X + corners[2].X) / 2;
                      tmp.Y = (corners[5].Y + corners[2].Y) / 2;
                      tmp.Z = (corners[5].Z + corners[2].Z) / 2;
                      actualRadiuses.Add(tmp);
                      tmp.X = (corners[5].X + corners[7].X) / 2;
                      tmp.Y = (corners[5].Y + corners[7].Y) / 2;
                      tmp.Z = (corners[5].Z + corners[7].Z) / 2;
                      actualRadiuses.Add(tmp);
                 /*   actualRadiuses.Clear();
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);*/
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (+z) ok
                       length = 2;
                       actualRadiuses.Clear();
                       tmp.X = (corners[1].X + corners[3].X) / 2;
                       tmp.Y = (corners[1].Y + corners[3].Y) / 2;
                       tmp.Z = (corners[1].Z + corners[3].Z) / 2;
                       actualRadiuses.Add(tmp);
                       tmp.X = (corners[1].X + corners[6].X) / 2;
                       tmp.Y = (corners[1].Y + corners[6].Y) / 2;
                       tmp.Z = (corners[1].Z + corners[6].Z) / 2;
                       actualRadiuses.Add(tmp);
             /*       actualRadiuses.Clear();
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);*/
                }
                else
                {
                    //tylko w prawo (+x) ok
                    length = 1;
                    actualRadiuses.Clear();
                    tmp.X = (corners[1].X + corners[6].X) / 2;
                    tmp.Y = (corners[1].Y + corners[6].Y) / 2;
                    tmp.Z = (corners[1].Z + corners[6].Z) / 2;
                    actualRadiuses.Add(tmp);
              /*      actualRadiuses.Clear();
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);*/
                }
            }
            else
            {
                //ruszyl sie w tyl (-z) ok
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                     length = 1;
                     actualRadiuses.Clear();
                     tmp.X = (corners[5].X + corners[7].X) / 2;
                     tmp.Y = (corners[5].Y + corners[7].Y) / 2;
                     tmp.Z = (corners[5].Z + corners[7].Z) / 2;
                     actualRadiuses.Add(tmp);
                   /* actualRadiuses.Clear();
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);*/
                }
                //ruszył się w przód (+z) ok
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                     length = 1;
                     actualRadiuses.Clear();
                     tmp.X = (corners[1].X + corners[3].X) / 2;
                     tmp.Y = (corners[1].Y + corners[3].Y) / 2;
                     tmp.Z = (corners[1].Z + corners[3].Z) / 2;
                     actualRadiuses.Add(tmp); 
             /*       actualRadiuses.Clear();
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);*/
                }
                else
                {
                    //wgl się nie ruszył
                }
            }

        }

        public void CheckWhichCornersForObjects()
        {
            Vector3 tmp = new Vector3(0, 0, 0);
            GetRadius();
            //ruszyl się w lewo (-x)
            if (referencePlayer.oldPosition.X > referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (-z) ok
                        length = 2;
                        actualRadiuses.Clear();
                        tmp.X = (corners[1].X + corners[3].X) / 2;
                        tmp.Y = (corners[1].Y + corners[3].Y) / 2;
                        tmp.Z = (corners[1].Z + corners[3].Z) / 2;
                        actualRadiuses.Add(tmp);
                        tmp.X = (corners[1].X + corners[6].X) / 2;
                        tmp.Y = (corners[1].Y + corners[6].Y) / 2;
                        tmp.Z = (corners[1].Z + corners[6].Z) / 2;
                        actualRadiuses.Add(tmp);
                  /*  actualRadiuses.Clear();
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);*/
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (+z) ok
                      length = 2;
                      actualRadiuses.Clear();
                      tmp.X = (corners[5].X + corners[2].X) / 2;
                      tmp.Y = (corners[5].Y + corners[2].Y) / 2;
                      tmp.Z = (corners[5].Z + corners[2].Z) / 2;
                      actualRadiuses.Add(tmp);
                      tmp.X = (corners[5].X + corners[7].X) / 2;
                      tmp.Y = (corners[5].Y + corners[7].Y) / 2;
                      tmp.Z = (corners[5].Z + corners[7].Z) / 2;
                      actualRadiuses.Add(tmp);
                 /*   actualRadiuses.Clear();
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);*/
                }
                else
                {
                    //tylko w lewo (-x) ok
                     length = 1;
                      actualRadiuses.Clear();
                      tmp.X = (corners[1].X + corners[6].X) / 2;
                      tmp.Y = (corners[1].Y + corners[6].Y) / 2;
                      tmp.Z = (corners[1].Z + corners[6].Z) / 2;
                      actualRadiuses.Add(tmp);
                /*    actualRadiuses.Clear();
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);*/
                }
            }
            //ruszyl sie w prawo.. (+x) ok
            else if (referencePlayer.oldPosition.X < referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (-z) ok
                      length = 2;
                      actualRadiuses.Clear();
                      tmp.X = (corners[1].X + corners[3].X) / 2;
                      tmp.Y = (corners[1].Y + corners[3].Y) / 2;
                      tmp.Z = (corners[1].Z + corners[3].Z) / 2;
                      actualRadiuses.Add(tmp);
                      tmp.X = (corners[4].X + corners[3].X) / 2;
                      tmp.Y = (corners[4].Y + corners[3].Y) / 2;
                      tmp.Z = (corners[4].Z + corners[3].Z) / 2;
                      actualRadiuses.Add(tmp);
              /*      actualRadiuses.Clear();
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);*/
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (+z) ok
                     length = 2;
                      actualRadiuses.Clear();
                      tmp.X = (corners[4].X + corners[6].X) / 2;
                      tmp.Y = (corners[4].Y + corners[6].Y) / 2;
                      tmp.Z = (corners[4].Z + corners[6].Z) / 2;
                      actualRadiuses.Add(tmp);
                      tmp.X = (corners[4].X + corners[3].X) / 2;
                      tmp.Y = (corners[4].Y + corners[3].Y) / 2;
                      tmp.Z = (corners[4].Z + corners[3].Z) / 2;
                      actualRadiuses.Add(tmp);
                  /*  actualRadiuses.Clear();
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);*/
                }
                else
                {
                    //tylko w prawo (+x)
                     length = 1;
                     actualRadiuses.Clear();
                     tmp.X = (corners[0].X + corners[7].X) / 2;
                     tmp.Y = (corners[0].Y + corners[7].Y) / 2;
                     tmp.Z = (corners[0].Z + corners[7].Z) / 2;
                     actualRadiuses.Add(tmp);
                 /*   actualRadiuses.Clear();
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);*/
                }
            }
            else
            {
                //ruszyl sie w tyl (-z)
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    length = 1;
                     actualRadiuses.Clear();
                     tmp.X = (corners[1].X + corners[3].X) / 2;
                     tmp.Y = (corners[1].Y + corners[3].Y) / 2;
                     tmp.Z = (corners[1].Z + corners[3].Z) / 2;
                     actualRadiuses.Add(tmp);
               /*     actualRadiuses.Clear();
                    actualRadiuses.Add(corners[5]);
                    actualRadiuses.Add(corners[6]);
                    actualRadiuses.Add(corners[4]);
                    actualRadiuses.Add(corners[7]);*/
                }
                //ruszył się w przód (+z)
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                     length = 1;
                     actualRadiuses.Clear();
                     tmp.X = (corners[5].X + corners[7].X) / 2;
                     tmp.Y = (corners[5].Y + corners[7].Y) / 2;
                     tmp.Z = (corners[5].Z + corners[7].Z) / 2;
                     actualRadiuses.Add(tmp);
                   /* actualRadiuses.Clear();
                    actualRadiuses.Add(corners[0]);
                    actualRadiuses.Add(corners[3]);
                    actualRadiuses.Add(corners[1]);
                    actualRadiuses.Add(corners[2]);*/
                }
                else
                {
                    //wgl się nie ruszył
                }
            }

        }

        public void CreateRadiuses()
        {
            radiuses[0] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[6].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[6].Y), 2)
             +   Math.Pow(Convert.ToDouble(corners[7].Z - corners[6].Z),2));//x
            radiuses[1] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[4].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[4].Y), 2)
             + Math.Pow(Convert.ToDouble(corners[7].Z - corners[4].Z), 2));//y
            radiuses[2] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[3].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[3].Y), 2)
             + Math.Pow(Convert.ToDouble(corners[7].Z - corners[3].Z), 2));//z
            radiuses[0] = radiuses[0] / 2;
            radiuses[1] = radiuses[1] / 2;
            radiuses[2] = radiuses[2] / 2;
        }
        public float getHeight()
        {
            return Math.Abs(corners[4].Y - corners[7].Y);

        }
        public void createAAPlane()
        {
             float dotProduct = 0;
            Vector3 normal = new Vector3(0, 0, 0);

                    Vector3 vecAB = corners[7] - corners[0];
                    Vector3 vecAC = corners[5] - corners[0];
                    
                    // Cross vecAB and vecAC
                    normal = Vector3.Cross(vecAB, vecAC);
                    normal.Normalize();

                    Vector3 tmp2 = corners[0];
                    dotProduct = Vector3.Dot(normal, tmp2);

                    plane = new Plane(normal, dotProduct);
                
        }
        public void getEquationOfPlane(Vector3 a, Vector3 b)
        {
            
        }

    }
}

