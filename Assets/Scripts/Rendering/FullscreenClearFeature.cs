using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class FullscreenClearFeature : ScriptableRendererFeature
{
    class ClearPass : ScriptableRenderPass
    {
        [System.Obsolete("Using legacy ScriptableRenderPass.Execute override")]
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("FullscreenClear");
            cmd.ClearRenderTarget(true, true, Color.black);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            // Required for URP 14+, but unused
        }
    }

    ClearPass pass;

    public override void Create()
    {
        pass = new ClearPass
        {
            // IMPORTANT: clear AFTER camera renders
            renderPassEvent = RenderPassEvent.AfterRendering
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}