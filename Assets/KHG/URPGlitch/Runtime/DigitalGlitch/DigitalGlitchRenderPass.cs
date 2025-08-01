using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPGlitch.Runtime.DigitalGlitch
{
    sealed class DigitalGlitchRenderPass : ScriptableRenderPass, IDisposable
    {
        const string RenderPassName = "DigitalGlitch RenderPass";

        // Shader property IDs
        static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        static readonly int NoiseTexID = Shader.PropertyToID("_NoiseTex");
        static readonly int TrashTexID = Shader.PropertyToID("_TrashTex");
        static readonly int IntensityID = Shader.PropertyToID("_Intensity");

        readonly ProfilingSampler _profilingSampler;
        readonly System.Random _random;

        readonly Material _glitchMaterial;
        readonly Texture2D _noiseTexture;
        readonly DigitalGlitchVolume _volume;

        // Temporary RT IDs
        readonly int _mainFrameID = Shader.PropertyToID("_MainFrame");
        readonly int _trashFrame1ID = Shader.PropertyToID("_TrashFrame1");
        readonly int _trashFrame2ID = Shader.PropertyToID("_TrashFrame2");

        bool isActive =>
            _glitchMaterial != null &&
            _volume != null &&
            _volume.IsActive;

        public DigitalGlitchRenderPass(Shader shader)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            _profilingSampler = new ProfilingSampler(RenderPassName);
            _random = new System.Random();
            _glitchMaterial = CoreUtils.CreateEngineMaterial(shader);

            _noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.DontSave,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            var volumeStack = VolumeManager.instance.stack;
            _volume = volumeStack.GetComponent<DigitalGlitchVolume>();

            UpdateNoiseTexture();
        }

        public void Dispose()
        {
            CoreUtils.Destroy(_glitchMaterial);
            CoreUtils.Destroy(_noiseTexture);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            if (!isActive) return;

            float r = (float)_random.NextDouble();
            if (r > Mathf.Lerp(0.9f, 0.5f, _volume.intensity.value))
            {
                UpdateNoiseTexture();
            }
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

                // Allocate temporary RTs
                cmd.GetTemporaryRT(_mainFrameID, descriptor);
                cmd.GetTemporaryRT(_trashFrame1ID, descriptor);
                cmd.GetTemporaryRT(_trashFrame2ID, descriptor);

                // Copy camera source
                cmd.Blit(source.rt, _mainFrameID);

                int frameCount = Time.frameCount;
                if (frameCount % 13 == 0) cmd.Blit(source.rt, _trashFrame1ID);
                if (frameCount % 73 == 0) cmd.Blit(source.rt, _trashFrame2ID);

                int selectedTrashID = (_random.NextDouble() > 0.5f) ? _trashFrame1ID : _trashFrame2ID;

                // Set shader properties
                cmd.SetGlobalFloat(IntensityID, _volume.intensity.value);
                cmd.SetGlobalTexture(NoiseTexID, _noiseTexture);
                cmd.SetGlobalTexture(MainTexID, _mainFrameID);
                cmd.SetGlobalTexture(TrashTexID, selectedTrashID);

                // Final glitch effect
                cmd.Blit(_mainFrameID, source.rt, _glitchMaterial);

                // Cleanup
                cmd.ReleaseTemporaryRT(_mainFrameID);
                cmd.ReleaseTemporaryRT(_trashFrame1ID);
                cmd.ReleaseTemporaryRT(_trashFrame2ID);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        void UpdateNoiseTexture()
        {
            Color color = randomColor;

            for (int y = 0; y < _noiseTexture.height; y++)
            {
                for (int x = 0; x < _noiseTexture.width; x++)
                {
                    float r = (float)_random.NextDouble();
                    if (r > 0.89f)
                        color = randomColor;

                    _noiseTexture.SetPixel(x, y, color);
                }
            }

            _noiseTexture.Apply();
        }

        Color randomColor
        {
            get
            {
                return new Color(
                    (float)_random.NextDouble(),
                    (float)_random.NextDouble(),
                    (float)_random.NextDouble(),
                    (float)_random.NextDouble()
                );
            }
        }
    }
}
