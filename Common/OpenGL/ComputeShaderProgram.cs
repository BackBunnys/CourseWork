using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.OpenGL
{
    public class ComputeShaderProgram : ShaderProgram
    {
        public Vector3i ComputeGroups { get; set; } = Vector3i.One;
        public Vector3 WorkGroupSize
        {
            get
            {
                var param = new int[3];
                GL.GetProgram(Handle, (GetProgramParameterName)All.ComputeWorkGroupSize, param);
                return new Vector3(param[0], param[1], param[2]);
            }
        }

        public static Vector3i MaxWorkGroupSize;

        public ComputeShaderProgram(int handle) : base(handle)
        {
        }

        static ComputeShaderProgram()
        {
            GL.GetInteger((GetIndexedPName)All.MaxComputeWorkGroupCount, 0, out var x);
            GL.GetInteger((GetIndexedPName)All.MaxComputeWorkGroupCount, 1, out var y);
            GL.GetInteger((GetIndexedPName)All.MaxComputeWorkGroupCount, 2, out var z);
            MaxWorkGroupSize = new Vector3i(x, y, z);
        }

        public void Compute()
        {
            Use();
            GL.DispatchCompute(ComputeGroups.X, ComputeGroups.Y, ComputeGroups.Z);
            Disable();
        }

        public void Wait()
        {
            Use();
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
            Disable();
        }
    }
}