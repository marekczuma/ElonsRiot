using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    static class OBBox
    {
        internal static void createOBBBox(GameObject gameObj)

        {
            gameObj.boneTransformations = new Matrix[gameObj.GameObjectModel.Bones.Count];
            gameObj.GameObjectModel.CopyAbsoluteBoneTransformsTo(gameObj.boneTransformations);
            
            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Matrix meshTransform = new Matrix();
            foreach (ModelMesh mesh in gameObj.GameObjectModel.Meshes)
            {
                meshTransform = gameObj.boneTransformations[mesh.ParentBone.Index] * gameObj.MatrixWorld;
                
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

            // boundingBox = new BoundingBox(new Vector3(-x/2,-y/2,-z/2),new Vector3(x/2,y/2,z/2));
            gameObj.obbox = new BoundingBox(meshMin, meshMax);
            
        }

    }
}
