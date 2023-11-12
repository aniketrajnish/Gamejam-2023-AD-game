using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using static Unity.Mathematics.MakraMathUtils;
using static RaymarchRenderer;

namespace Unity.Mathematics
{
    public class RaymarchCollision : MonoBehaviour
    {
        [SerializeField] float offset = 1.5f;
        [SerializeField] float maxMovement = 1f;
        [SerializeField] Transform[] bounds;
        Raymarcher raymarcher;
        private void Start()
        {
            raymarcher = Camera.main.GetComponent<Raymarcher>();
        }
        /*private void Update()
        {
            MoveToGround();
            CheckRaymarchDist(bounds);
        }*/
        public float GetShapeDistance(RaymarchRenderer rend, float3 p)
        {
            float wPos = raymarcher.wPos;
            Vector3 wRot = raymarcher.wRot;
            //Vector3 shapePos = rend.transform.position;

            float4 p4 = rotWposW(p, wPos, wRot);

            vector12 dimensions = Helpers.GetDimensionVectors((int)rend.shape, rend.dimensions);

            switch (rend.shape)
            {
                case RaymarchRenderer.Shape.Shpere:
                    return DFs.sdSphere(p4.xyz, dimensions.a, wPos, wRot);
                case RaymarchRenderer.Shape.Torus:
                    return DFs.sdTorus(p4.xyz, new float2(dimensions.a, dimensions.b), wPos, wRot);
                case RaymarchRenderer.Shape.CappedTorus:
                    return DFs.sdCappedTorus(p4.xyz, dimensions.a, dimensions.b, new float2(dimensions.c, dimensions.d), wPos, wRot);
                case RaymarchRenderer.Shape.Link:
                    return DFs.sdLink(p4.xyz, dimensions.a, dimensions.b, dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.Plane:
                    return DFs.sdPlane(p4.xyz, new float3(dimensions.a, dimensions.b, dimensions.c), dimensions.d, wPos, wRot);
                case RaymarchRenderer.Shape.Cone:
                    return DFs.sdCone(p4.xyz, new float2(dimensions.a, dimensions.b), dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.InfCone:
                    return DFs.sdInfCone(p4.xyz, new float2(dimensions.a, dimensions.b), wPos, wRot);
                case RaymarchRenderer.Shape.HexPrism:
                    return DFs.sdHexPrism(p4.xyz, new float2(dimensions.a, dimensions.b), wPos, wRot);
                case RaymarchRenderer.Shape.TriPrism:
                    return DFs.sdTriPrism(p4.xyz, new float2(dimensions.a, dimensions.b), wPos, wRot);
                case RaymarchRenderer.Shape.Capsule:
                    return DFs.sdCapsule(p4.xyz, new float3(dimensions.a, dimensions.b, dimensions.c),
                        new float3(dimensions.d, dimensions.e, dimensions.f),dimensions.g,
                        wPos, wRot);
                case RaymarchRenderer.Shape.InfiniteCylinder:
                    return DFs.sdInfiniteCylinder(p4.xyz, new float3(dimensions.a, dimensions.b, dimensions.c), wPos, wRot);
                case RaymarchRenderer.Shape.Box:
                    return DFs.sdBox(p4.xyz, dimensions.a, wPos, wRot);
                case RaymarchRenderer.Shape.RoundBox:
                    return DFs.sdRoundBox(p4.xyz, dimensions.a, dimensions.b, wPos, wRot);
                case RaymarchRenderer.Shape.RoundedCylinder:
                    return DFs.sdRoundedCylinder(p4.xyz, dimensions.a, dimensions.b, dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.CappedCone:
                    return DFs.sdCappedCone(p4.xyz, dimensions.a, dimensions.b, dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.BoxFrame:
                    return DFs.sdBoxFrame(p4.xyz, new float3(dimensions.a, dimensions.b, dimensions.c), dimensions.d, wPos, wRot);
                case RaymarchRenderer.Shape.SolidAngle:
                    return DFs.sdSolidAngle(p4.xyz, new float2(dimensions.a, dimensions.b), dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.CutSphere:
                    return DFs.sdCutSphere(p4.xyz, dimensions.a, dimensions.b, wPos, wRot);
                case RaymarchRenderer.Shape.CutHollowSphere:
                    return DFs.sdCutHollowSphere(p4.xyz, dimensions.a, dimensions.b, dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.DeathStar:
                    return DFs.sdDeathStar(p4.xyz, dimensions.a, dimensions.b, dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.RoundCone:
                    return DFs.sdRoundCone(p4.xyz, dimensions.a, dimensions.b, dimensions.c, wPos, wRot);
                case RaymarchRenderer.Shape.Ellipsoid:
                    return DFs.sdEllipsoid(p4.xyz, new float3(dimensions.a, dimensions.b, dimensions.c), wPos, wRot);
                case RaymarchRenderer.Shape.Rhombus:
                    return DFs.sdRhombus(p4.xyz, dimensions.a, dimensions.b, dimensions.c, dimensions.d, wPos, wRot);
                case RaymarchRenderer.Shape.Octahedron:
                    return DFs.sdOctahedron(p4.xyz, dimensions.a, wPos, wRot);
                case RaymarchRenderer.Shape.Pyramid:
                    return DFs.sdPyramid(p4.xyz, dimensions.a, wPos, wRot);
                case RaymarchRenderer.Shape.Triangle:
                    return DFs.udTriangle(p4.xyz,
                        new float3(dimensions.a, dimensions.b, dimensions.c),
                        new float3(dimensions.d, dimensions.e, dimensions.f),
                        new float3(dimensions.g, dimensions.h, dimensions.i),
                        wPos, wRot);
                case RaymarchRenderer.Shape.Quad:
                    return DFs.udQuad(p4.xyz,
                        new float3(dimensions.a, dimensions.b, dimensions.c),
                        new float3(dimensions.d, dimensions.e, dimensions.f),
                        new float3(dimensions.g, dimensions.h, dimensions.i),
                        new float3(dimensions.j, dimensions.k, dimensions.l),
                        wPos, wRot);
                case RaymarchRenderer.Shape.Fractal:
                    return DFs.sdFractal(p4.xyz,
                        dimensions.a, dimensions.b, dimensions.c,
                        wPos, wRot);
                case RaymarchRenderer.Shape.Tesseract:
                    return DFs.sdTesseract(p4.xyz,
                        new float4(dimensions.a, dimensions.b, dimensions.c, dimensions.d),
                        wPos, wRot);
                case RaymarchRenderer.Shape.HyperSphere:
                    return DFs.sdHyperSphere(p4.xyz,
                        dimensions.a,
                        wPos, wRot);
                case RaymarchRenderer.Shape.DuoCylinder:
                    return DFs.sdDuoCylinder(p4.xyz,
                        new float2(dimensions.a, dimensions.b),
                        wPos, wRot);
                case RaymarchRenderer.Shape.VerticalCapsule:
                    return DFs.sdVerticalCapsule(p4.xyz,
                        dimensions.a, dimensions.b,
                        wPos, wRot);
                case RaymarchRenderer.Shape.FiveCell:
                    return DFs.sdFiveCell(p4.xyz,
                        new float4(dimensions.a, dimensions.b, dimensions.c, dimensions.d),
                        wPos, wRot);
                case RaymarchRenderer.Shape.SixteenCell:
                    return DFs.sdSixteenCell(p4.xyz,
                        dimensions.a,
                        wPos, wRot);
            }

            return Camera.main.farClipPlane;
        }
        public float DistanceField(Vector3 p)
        {
            float sigmaDist = Mathf.Infinity;
            foreach (var rend in raymarcher.renderers)
            {
                float deltaDist = GetShapeDistance(rend, p);
                switch (rend.operation)
                {
                    case Operation.Union:
                        sigmaDist = Mathf.Min(sigmaDist, deltaDist);
                        break;
                    case Operation.Intersect:
                        sigmaDist = Mathf.Max(sigmaDist, deltaDist);
                        break;
                    case Operation.Subtract:
                        sigmaDist = Mathf.Max(sigmaDist, -deltaDist);
                        break;
                }
            }
            return sigmaDist;
        }
        /*void CheckRaymarchDist(Transform[] ro)
        {

            for (int i = 0; i < ro.Length; i++)
            {
                Vector3 p = ro[i].position;
                //check hit
                float d = DistanceField(p);


                if (d < 0) //hit
                {
                    Debug.Log("hit" + i);
                    //collision
                    transform.Translate(ro[i].forward * d * 1.5f, Space.World);

                }


            }
        }

        //moves the player to the ground
        void MoveToGround()
        {
            Vector3 p = transform.position;
            //check hit

            float d = DistanceField(p);
            d = Mathf.Min(d, maxMovement);
            //Debug.Log(d);
            transform.Translate(Vector3.down * d, Space.World);
        }*/

    }
}

