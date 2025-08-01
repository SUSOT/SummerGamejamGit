using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPGlitch.Runtime.AnalogGlitch
{
    sealed class AnalogGlitchRenderPass : ScriptableRenderPass, IDisposable
    {
        const string RenderPassName = "AnalogGlitch RenderPass";

        // Shader Property IDs
        static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        static readonly int ScanLineJitterID = Shader.PropertyToID("_ScanLineJitter");
        static readonly int VerticalJumpID = Shader.PropertyToID("_VerticalJump");
        static readonly int HorizontalShakeID = Shader.PropertyToID("_HorizontalShake");
        static readonly int ColorDriftID = Shader.PropertyToID("_ColorDrift");

        readonly ProfilingSampler _profilingSampler;
        readonly Material _glitchMaterial;
        readonly AnalogGlitchVolume _volume;

        readonly int _mainFrameID = Shader.PropertyToID("_MainFrame");

        float _verticalJumpTime;

        bool isActive =>
            _glitchMaterial != null &&
            _volume != null &&
            _volume.IsActive;

        public AnalogGlitchRenderPass(Shader shader)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            _profilingSampler = new ProfilingSampler(RenderPassName);
            _glitchMaterial = CoreUtils.CreateEngineMaterial(shader);

            var volumeStack = VolumeManager.instance.stack;
            _volume = volumeStack.GetComponent<AnalogGlitchVolume>();
        }

        public void Dispose()
        {
            CoreUtils.Destroy(_glitchMaterial);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!isActive ||
                !renderingData.cameraData.postProcessEnabled ||
                renderingData.cameraData.isSceneViewCamera)
            {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get(RenderPassName);
            cmd.Clear();

            using (new ProfilingScope(cmd, _profilingSampler))
            {
                RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
                var descriptor = renderingData.cameraData.cameraTargetDescriptor;
                descriptor.depthBufferBits = 0;

                cmd.GetTemporaryRT(_mainFrameID, descriptor);
                cmd.Blit(source.rt, _mainFrameID);

                var scanLineJitter = _volume.scanLineJitter.value;
                var verticalJump = _volume.verticalJump.value;
                var horizontalShake = _volume.horizontalShake.value;
                var colorDrift = _volume.colorDrift.value;

                _verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

                var slThresh = Mathf.Clamp01(1.0f - scanLineJitter * 1.2f);
                var slDisp = 0.002f + Mathf.Pow(scanLineJitter, 3) * 0.05f;
                _glitchMaterial.SetVector(ScanLineJitterID, new Vector2(slDisp, slThresh));

                var vj = new Vector2(verticalJump, _verticalJumpTime);
                _glitchMaterial.SetVector(VerticalJumpID, vj);
                _glitchMaterial.SetFloat(HorizontalShakeID, horizontalShake * 0.2f);

                var cd = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);
                _glitchMaterial.SetVector(ColorDriftID, cd);

                cmd.SetGlobalTexture(MainTexID, _mainFrameID);
                cmd.Blit(_mainFrameID, source.rt, _glitchMaterial);
                cmd.ReleaseTemporaryRT(_mainFrameID);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
