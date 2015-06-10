using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    class MyPlane
    {
        private GameObject referenceObject;
        public MyPlane(GameObject reference)
        {
            referenceObject = reference;
            createPlane();
        }
        public void createPlane()
        {
            float dotProduct = 0;
            Vector3 normal = new Vector3(0, 0, 0);
            Matrix meshTransform = new Matrix();
            foreach (ModelMesh mesh in referenceObject.GameObjectModel.Meshes)
            {
                meshTransform = referenceObject.boneTransformations[mesh.ParentBone.Index] * referenceObject.MatrixWorld;
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {

                    int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(meshPart.VertexOffset * stride, vertexData, 0, meshPart.NumVertices, stride);
                    for (int i = 0; i < vertexData.Count(); i++)
                    {
                        vertexData[i].Position = Vector3.Transform(vertexData[i].Position, meshTransform);
                    }
                    Vector3 vecAB = vertexData[1].Position - vertexData[0].Position;
                    Vector3 vecAC = vertexData[3].Position - vertexData[0].Position;

                    normal = Vector3.Cross(vecAB, vecAC);
                    normal.Normalize();

                    Vector3 tmp = vertexData[0].Position; 
                    dotProduct = Vector3.Dot(-normal, tmp);

                }
            }
            referenceObject.plane = new Plane(normal, dotProduct);
        }
    }
}
