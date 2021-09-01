using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3 : MonoBehaviour
{
    // Start is called before the first frame update
    public float v00, v01, v02, v10, v11, v12, v20, v21, v22;

    public Matrix3x3()
    {
        v00 = v01 = v02 = 0;
        v10 = v11 = v12 = 0;
        v20 = v21 = v22 = 0;
    }

    public Matrix3x3(Vector3 a,Vector3 b,Vector3 c)
    {
        v00 = a.x;
        v01 = a.y;
        v02 = a.z;

        v10 = b.x;
        v11 = b.y;
        v12 = b.z;

        v20 = c.x;
        v21 = c.y;
        v22 = c.z;
    }

    public Matrix3x3(float v0, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8)
    {
        v00 = v0;
        v01 = v1;
        v02 = v2;
        v10 = v3;
        v11 = v4;
        v12 = v5;
        v20 = v6;
        v21 = v7;
        v22 = v8;
    }

    public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = a.v00 + b.v00;
        res.v01 = a.v01 + b.v01;
        res.v02 = a.v02 + b.v02;
        res.v10 = a.v10 + b.v10;
        res.v11 = a.v11 + b.v11;
        res.v12 = a.v12 + b.v12;
        res.v20 = a.v20 + b.v20;
        res.v21 = a.v21 + b.v21;
        res.v22 = a.v22 + b.v22;
        return res;
    }

    public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = a.v00 - b.v00;
        res.v01 = a.v01 - b.v01;
        res.v02 = a.v02 - b.v02;
        res.v10 = a.v10 - b.v10;
        res.v11 = a.v11 - b.v11;
        res.v12 = a.v12 - b.v12;
        res.v20 = a.v20 - b.v20;
        res.v21 = a.v21 - b.v21;
        res.v22 = a.v22 - b.v22;
        return res;
    }
    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = a.v00 * b.v00 + a.v01 * b.v10 + a.v02 * b.v20;
        res.v01 = a.v00 * b.v01 + a.v01 * b.v11 + a.v02 * b.v21;
        res.v02 = a.v00 * b.v02 + a.v01 * b.v12 + a.v02 * b.v22;

        res.v10 = a.v10 * b.v00 + a.v11 * b.v10 + a.v12 * b.v20;
        res.v11 = a.v10 * b.v01 + a.v11 * b.v11 + a.v12 * b.v21;
        res.v12 = a.v10 * b.v02 + a.v11 * b.v12 + a.v12 * b.v22;

        res.v20 = a.v20 * b.v20 + a.v21 * b.v10 + a.v22 * b.v20;
        res.v21 = a.v20 * b.v21 + a.v21 * b.v11 + a.v22 * b.v21;
        res.v22 = a.v20 * b.v22 + a.v21 * b.v12 + a.v22 * b.v22;
        return res;
    }

    public static Vector3 operator *(Matrix3x3 a, Vector3 b)
    {
        Vector3 res = new Vector3();
        res.x = a.v00 * b.x + a.v01 * b.y + a.v02 * b.z;
        res.y = a.v10 * b.x + a.v11 * b.y + a.v12 * b.z;
        res.z = a.v20 * b.x + a.v21 * b.y + a.v22 * b.z;
        return res;
    }
    public static Matrix3x3 operator *(Matrix3x3 a, float scaler)
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = a.v00 * scaler;
        res.v01 = a.v01 * scaler;
        res.v02 = a.v02 * scaler;
        res.v10 = a.v10 * scaler;
        res.v11 = a.v11 * scaler;
        res.v12 = a.v12 * scaler;
        res.v20 = a.v20 * scaler;
        res.v21 = a.v21 * scaler;
        res.v22 = a.v22 * scaler;
        return res;
    }
    public float determinant()
    {
        float term0 = v00 * (v11 * v22 - v12 * v21);
        float term1 = v01 * (v12 * v20 - v10 * v22);
        float term2 = v02 * (v10 * v21 - v11 * v20);
        return term0 + term1 + term2;
    }
    public Matrix3x3 transpose()
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = v00;
        res.v11 = v11;
        res.v22 = v22;

        res.v10 = v01;
        res.v20 = v02;
        res.v01 = v10;
        res.v02 = v20;
        res.v12 = v21;
        res.v21 = v12;
        return res;
    }

    public void AssembleVectors(Vector3 a, Vector3 b, Vector3 c)
    {
        v00 = a.x;
        v01 = a.y;
        v02 = a.z;

        v10 = b.x;
        v11 = b.y;
        v12 = b.z;

        v20 = c.x;
        v21 = c.y;
        v22 = c.z;
    }
    public Matrix3x3 inverse()
    {
        Matrix3x3 res = new Matrix3x3();
        float det_inv = 1.0f / determinant();
        res.v00 = (v11 * v22 - v12 * v21) * det_inv;
        res.v01 = (v02 * v21 - v01 * v22) * det_inv;
        res.v02 = (v01 * v12 - v02 * v11) * det_inv;

        res.v10 = (v12 * v20 - v10 * v22) * det_inv;
        res.v11 = (v00 * v22 - v02 * v20) * det_inv;
        res.v12 = (v02 * v10 - v00 * v12) * det_inv;

        res.v20 = (v10 * v21 - v11 * v20) * det_inv;
        res.v21 = (v01 * v20 - v00 * v21) * det_inv;
        res.v22 = (v00 * v11 - v01 * v10) * det_inv;
        return res;
    }
    public Matrix3x3 identity()
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = res.v11 = res.v22 = 1;
        return res;
    }

    public Matrix3x3 OutterProduct(float u0, float u1, float u2, float v0, float v1, float v2)
    {
        Matrix3x3 res = new Matrix3x3();
        res.v00 = u0 * v0;
        res.v01 = u0 * v1;
        res.v02 = u0 * v2;
        res.v10 = u1 * v0;
        res.v11 = u1 * v1;
        res.v12 = u1 * v2;
        res.v20 = u2 * v0;
        res.v21 = u2 * v1;
        res.v22 = u2 * v2;
        return res;

    }

    public void DebugMatrix(Matrix3x3 A, string name)
    {
        Debug.Log("          [ " + A.v00 + " " + A.v01 + " " + A.v02 + "]");
        Debug.Log(name + "=  [ " + A.v10 + " " + A.v11 + " " + A.v12 + "]");
        Debug.Log("          [ " + A.v20 + " " + A.v21 + " " + A.v22 + "]");
    }
    public void QRdecomposition(Matrix3x3 A, ref Matrix3x3 Q, ref Matrix3x3 R)
    {
        Q = identity();


        // 第一遍 householder
        float norm = 0;
        norm = A.v00 * A.v00 + A.v10 * A.v10 + A.v20 * A.v20;

        norm = Mathf.Sqrt(norm);
        float scalar = 1.0f / (Mathf.Sign(A.v00) * norm + A.v00);
        float v0 = 1, v1 = A.v10 * scalar, v2 = A.v20 * scalar;

        float inner_product = v0 * v0 + v1 * v1 + v2 * v2;
        Matrix3x3 outter_product = OutterProduct(v0, v1, v2, v0, v1, v2);
        Matrix3x3 H = identity() - outter_product * (2 / inner_product);
        Q = Q * H;
        A = H * A;

        // 第二遍 householder
        H = identity();
        norm = Mathf.Sqrt(A.v11 * A.v11 + A.v21 * A.v21);
        v0 = 1;
        v1 = A.v21 / (A.v11 + Mathf.Sign(A.v11) * norm);

        inner_product = 1.0f / (v0 * v0 + v1 * v1);

        H.v11 = H.v11 - 2.0f * inner_product * v0 * v0;
        H.v12 = H.v12 - 2.0f * inner_product * v0 * v1;
        H.v21 = H.v21 - 2.0f * inner_product * v1 * v0;
        H.v22 = H.v22 - 2.0f * inner_product * v1 * v1;

        // 结果
        Q = Q * H;
        R = H * A;
    }
    public float trace()
    {
        return v00 + v11 + v22;
    }
    public Vector3 computeEigenValue(Matrix3x3 A)
    {
        Vector3 eigenValue = new Vector3();
        Matrix3x3 R_old = new Matrix3x3();
        Matrix3x3 R = new Matrix3x3();
        Matrix3x3 Q = new Matrix3x3();
        for (int ite = 0; ite < 100; ite++)
        {
            QRdecomposition(A, ref Q, ref R);
            float residual = Mathf.Abs(R.trace() - R_old.trace());
            if (residual < 1e-10) break;
            R_old = R;
            A = R * Q;
        }
        eigenValue.x = R.v00;
        eigenValue.y = R.v11;
        eigenValue.z = R.v22;
        return eigenValue;
    }
    private Vector3 computeEigenVector(Matrix3x3 A)
    {
        float scalar = A.v10 / A.v00;
        A.v10 = A.v10 - A.v00 * scalar;
        A.v11 = A.v11 - A.v01 * scalar;
        A.v12 = A.v12 - A.v02 * scalar;
        Vector3 res = new Vector3();
        res.z = 1;
        res.y = -A.v12 / A.v11;
        res.x = (A.v01 * res.y + A.v02 * res.z) / A.v00;
        float norm = Mathf.Sqrt(res.x * res.x + res.y * res.y + res.z * res.z);
        return res / norm;
    }

    private bool IsDiagonal(Matrix3x3 A)
    {

        if (Mathf.Abs(A.v00 * A.v11 * A.v22 - A.determinant()) < 1e-6)
            return true;
        return false;
    }

    public void svd3x3(Matrix3x3 A, ref Matrix3x3 U, ref Matrix3x3 sigma, ref Matrix3x3 Vt)
    {
        if(IsDiagonal(A) == true)
        {
            sigma = A;
            U = identity();
            Vt = identity();
            return;
        }

        Matrix3x3 AtA = A.transpose() * A;
        Vector3 eigenValue = computeEigenValue(AtA);
        Matrix3x3 eigenVector = new Matrix3x3();
        Vector3 tempVec0, tempVec1, tempVec2;
        Matrix3x3 tempMat;

        tempMat = AtA - identity() * Mathf.Abs(eigenValue.x);
        tempVec0 = computeEigenVector(tempMat);

        tempMat = AtA - identity() * Mathf.Abs(eigenValue.y);
        tempVec1 = computeEigenVector(tempMat);

        tempMat = AtA - identity() * Mathf.Abs(eigenValue.z);
        tempVec2 = computeEigenVector(tempMat);

        eigenVector.AssembleVectors(tempVec0, tempVec1, tempVec2);

        sigma = new Matrix3x3();
        sigma.v00 = Mathf.Sqrt(Mathf.Abs(eigenValue.x));
        sigma.v11 = Mathf.Sqrt(Mathf.Abs(eigenValue.y));
        sigma.v22 = Mathf.Sqrt(Mathf.Abs(eigenValue.z));

        Vt = eigenVector;

        tempVec0 = A * tempVec0 * (1 / sigma.v00);
        tempVec1 = A * tempVec1 * (1 / sigma.v11);
        tempVec2 = A * tempVec2 * (1 / sigma.v22);

        U.AssembleVectors(tempVec0, tempVec1, tempVec2);
    }
    public void testSVD()
    {
        Matrix3x3 A = new Matrix3x3(0, 1, 1, 1.414f, 2, 0, 0, 1, 1);
        Matrix3x3 U = new Matrix3x3();
        Matrix3x3 sigma = new Matrix3x3();
        Matrix3x3 Vt = new Matrix3x3();
        A.svd3x3(A, ref U, ref sigma, ref Vt);
        Debug.Log(sigma.v00);//2.828
        Debug.Log(sigma.v11);//1.414
        Debug.Log(sigma.v22);//0
    }

    public void testINV()
    {
        Matrix3x3 A = new Matrix3x3(4,2,3,4,5,6,7,8,9);
        DebugMatrix(A.inverse(),"aINV");
    }
}
