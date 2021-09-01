using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPM88 : MonoBehaviour
{
    public ComputeShader p2gShader;
    public ComputeShader g2pShader;
    public ComputeShader boundaryShader;
    public ComputeShader initGridShader;

    ComputeBuffer particle_pos;
    ComputeBuffer particle_vel;
    ComputeBuffer particle_J;
    ComputeBuffer particle_C;
    ComputeBuffer particle_debug;


    ComputeBuffer grid_vel_x;
    ComputeBuffer grid_vel_y;
    ComputeBuffer grid_mass;
    ComputeBuffer grid_obstacle;

    int kernel_p2gShader;
    int kernel_g2pShader;
    int kernel_boundaryShader;
    int kernel_initGrid;

    int Nx = 64;
    int Ny = 64;
    int grid_num;
    int particle_num = 4096;
    int group_num = 256;

    float dx;
    float particle_vol;
    float dt;
    float E;
    float particle_mass;
    int grid_vel_scale = 100000000;

    float[] particle_pos_data;
    float[] particle_vel_data;
    float[] particle_J_data;
    float[] particle_C_data;
    float[] particle_debug_data;

    int[] grid_data;
    int[] grid_velx_data;
    int[] grid_vely_data;
    int[] obstacle_data;


    [SerializeField] Mesh instance_mesh;
    [SerializeField] Material instance_material;
    ComputeBuffer args_buffer;
    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    int cnt = 0;

    void InitGrid()
    {
        initGridShader.SetBuffer(kernel_initGrid, "grid_vel_x", grid_vel_x);
        initGridShader.SetBuffer(kernel_initGrid, "grid_vel_y", grid_vel_y);
        initGridShader.SetBuffer(kernel_initGrid, "grid_mass", grid_mass);
        initGridShader.Dispatch(kernel_initGrid, grid_num / group_num, 1, 1);
    }
    void Boundary()
    {
        boundaryShader.SetBuffer(kernel_boundaryShader, "grid_vel_x", grid_vel_x);
        boundaryShader.SetBuffer(kernel_boundaryShader, "grid_vel_y", grid_vel_y);
        boundaryShader.SetBuffer(kernel_boundaryShader, "grid_mass", grid_mass);
        boundaryShader.SetBuffer(kernel_boundaryShader, "grid_obstacle", grid_obstacle);
        boundaryShader.SetInt("grid_vel_scale", grid_vel_scale);
        boundaryShader.SetInt("bound", 3);
        boundaryShader.SetInt("Nx", Nx);
        boundaryShader.SetFloat("dt", dt);
        boundaryShader.Dispatch(kernel_boundaryShader, grid_num / group_num, 1, 1);
    }
    void GridToParticle()
    {
        g2pShader.SetBuffer(kernel_g2pShader, "particle_pos", particle_pos);
        g2pShader.SetBuffer(kernel_g2pShader, "particle_vel", particle_vel);
        g2pShader.SetBuffer(kernel_g2pShader, "particle_debug", particle_debug);
        g2pShader.SetBuffer(kernel_g2pShader, "particle_J", particle_J);
        g2pShader.SetBuffer(kernel_g2pShader, "particle_C", particle_C);
        g2pShader.SetBuffer(kernel_g2pShader, "grid_vel_x", grid_vel_x);
        g2pShader.SetBuffer(kernel_g2pShader, "grid_vel_y", grid_vel_y);
        g2pShader.SetFloat("grid_vel_scale", 1.0f / grid_vel_scale);
        g2pShader.SetInt("Nx", Nx);
        g2pShader.SetFloat("dx", dx);
        g2pShader.SetFloat("dt", dt);
        g2pShader.SetFloat("E", E);
        g2pShader.Dispatch(kernel_g2pShader, particle_num / group_num, 1, 1);
    }
    void ParticleToGrid()
    {
        p2gShader.SetBuffer(kernel_p2gShader, "particle_pos", particle_pos);
        p2gShader.SetBuffer(kernel_p2gShader, "particle_vel", particle_vel);
        p2gShader.SetBuffer(kernel_p2gShader, "particle_J", particle_J);
        p2gShader.SetBuffer(kernel_p2gShader, "particle_C", particle_C);
        p2gShader.SetBuffer(kernel_p2gShader, "particle_debug", particle_debug);
        p2gShader.SetBuffer(kernel_p2gShader, "grid_vel_x", grid_vel_x);
        p2gShader.SetBuffer(kernel_p2gShader, "grid_vel_y", grid_vel_y);
        p2gShader.SetBuffer(kernel_p2gShader, "grid_mass", grid_mass);
        p2gShader.SetInt("grid_vel_scale", grid_vel_scale);
        p2gShader.SetInt("Nx", Nx);
        p2gShader.SetFloat("dx", dx);
        p2gShader.SetFloat("dt", dt);
        p2gShader.SetFloat("E", E);
        p2gShader.SetFloat("particle_mass", particle_mass);
        p2gShader.SetFloat("particle_vol", particle_vol);
        p2gShader.Dispatch(kernel_p2gShader, particle_num / group_num, 1, 1);

    }

    void InitBuffer()
    {
        kernel_p2gShader = p2gShader.FindKernel("CSMain");
        kernel_g2pShader = g2pShader.FindKernel("CSMain");
        kernel_boundaryShader = boundaryShader.FindKernel("CSMain");
        kernel_initGrid = initGridShader.FindKernel("CSMain");

        particle_pos = new ComputeBuffer(particle_num, sizeof(float) * 2);
        particle_vel = new ComputeBuffer(particle_num, sizeof(float) * 2);
        particle_J = new ComputeBuffer(particle_num, sizeof(float) * 1);
        particle_C = new ComputeBuffer(particle_num, sizeof(float) * 4);
        particle_debug = new ComputeBuffer(particle_num, sizeof(float) * 1);

        grid_vel_x = new ComputeBuffer(grid_num, sizeof(int));
        grid_vel_y = new ComputeBuffer(grid_num, sizeof(int));
        grid_mass = new ComputeBuffer(grid_num, sizeof(int));
        grid_obstacle = new ComputeBuffer(grid_num, sizeof(int));

        particle_pos_data = new float[particle_num * 2];
        particle_vel_data = new float[particle_num * 2];
        particle_J_data = new float[particle_num];
        particle_C_data = new float[particle_num * 4];
        particle_debug_data = new float[particle_num];
        grid_data = new int[grid_num];
        grid_velx_data = new int[grid_num];
        grid_vely_data = new int[grid_num];
        obstacle_data = new int[grid_num];

        for (int i = 0; i < particle_num; i++)
        {
            particle_pos_data[2 * i + 0] = Random.Range(0.0f, 0.3f) + 0.05f;
            particle_pos_data[2 * i + 1] = Random.Range(0.0f, 0.3f) + 0.35f;
            particle_vel_data[2 * i + 0] = 0;
            particle_vel_data[2 * i + 1] = -1.0f;
            particle_J_data[i] = 1.0f;
            particle_C_data[2 * i + 0] = 0;
            particle_C_data[2 * i + 1] = 0;
            particle_C_data[2 * i + 2] = 0;
            particle_C_data[2 * i + 3] = 0;
        }
        /*
        particle_pos_data[0] = 0.512779f;
        particle_pos_data[1] = 0.236697f;

        particle_pos_data[2] = 0.439735f;
        particle_pos_data[3] = 0.246356f;

        particle_pos_data[4] = 0.395781f;
        particle_pos_data[5] = 0.267424f;

        particle_pos_data[6] = 0.483968f;
        particle_pos_data[7] = 0.503388f;

        particle_pos_data[8] = 0.339202f;
        particle_pos_data[9] = 0.552977f;

        particle_pos_data[10] = 0.485252f;
        particle_pos_data[11] = 0.397898f;

        particle_pos_data[12] = 0.499045f;
        particle_pos_data[13] = 0.235902f;

        particle_pos_data[14] = 0.485169f;
        particle_pos_data[15] = 0.24242f;*/

        particle_pos.SetData(particle_pos_data);
        particle_vel.SetData(particle_vel_data);
        particle_J.SetData(particle_J_data);
        particle_debug.SetData(particle_J_data);
        particle_C.SetData(particle_C_data);

        for (int i = 0; i < grid_num; i++)
        {
            grid_velx_data[i] = 0;
            grid_vely_data[i] = 0;
            grid_data[i] = 0;
            obstacle_data[i] = 0;
            int ix = i % Nx;
            int iy = i / Nx;
            int bound = 3;
            if(ix > Nx*3/4- bound && ix < Nx*3/4 + bound && iy < bound * 3)
            {
                obstacle_data[i] = 1;
            }
        }
        grid_vel_x.SetData(grid_velx_data);
        grid_vel_y.SetData(grid_vely_data);
        grid_mass.SetData(grid_data);
        grid_obstacle.SetData(obstacle_data);
    }


    private void OnDestroy()
    {
        particle_pos.Release();
        particle_vel.Release();
        particle_J.Release();
        particle_C.Release();

        grid_vel_x.Release();
        grid_vel_y.Release();
        grid_mass.Release();
    }
    void Start()
    {
        dx = 1.0f / (float)Nx;
        particle_vol = (dx * dx * 0.25f);
        particle_mass = particle_vol * 1;
        dt = 0.0002f;
        E = 400.0f;
        grid_num = Nx * Ny;

        grid_vel_scale = 100000000;
        grid_vel_scale = 400000000;

        InitBuffer();

        args_buffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        uint numIndices = (uint)instance_mesh.GetIndexCount(0);
        args[0] = numIndices;
        args[1] = (uint)particle_pos.count;
        args_buffer.SetData(args);

        instance_material.SetBuffer("particle_buffer", particle_pos);
        Step();
        //Step();
        cnt = 1;
        Step();
    }

    void Step()
    {
        InitGrid();
        ParticleToGrid();
        particle_debug.GetData(particle_debug_data);
        particle_pos.GetData(particle_pos_data);
        for (int i = 0; i < particle_num; i++)
        {
            //Debug.Log(particle_pos_data[i]);

           //Debug.Log(particle_debug_data[i]);
        }

        Boundary();
        grid_vel_y.GetData(grid_data);
        for (int i = 0; i < grid_num; i++)
        {
            //Debug.Log(particle_pos_data[i]);
            if (grid_data[i] != 0 && cnt == 1)
            {
               //Debug.Log("i = " + i + " = " + grid_data[i]);
            }
        }
        GridToParticle();
        particle_J.GetData(particle_J_data);
        particle_debug.GetData(particle_debug_data);
        for (int i = 0; i < particle_num; i++)
        {
            if (cnt == 1)
            {
                //Debug.Log("i = " + i + " = " + particle_debug_data[i]);
            }
        }
        particle_vel.GetData(particle_vel_data);
        particle_pos.GetData(particle_pos_data);
        for (int i = 0; i < particle_num* 2; i++)
        {
            //if(cnt == 1)
        //   Debug.Log(particle_pos_data[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for(int ite = 0; ite < 5;ite++)
        {

            Step();
        }

        Bounds bounds = new Bounds(Vector3.zero, new Vector3(100, 100, 100));
        Graphics.DrawMeshInstancedIndirect(instance_mesh, 0, instance_material, bounds, args_buffer);
    }
}
