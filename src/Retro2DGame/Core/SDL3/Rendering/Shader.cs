using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class Shader
{
    public nint Handle { get; }

    private Shader(nint handle)
    {
        Handle = handle;
    }

    private static void GenerateDataFromBackend(string backend, out SDL.SDL_GPUShaderFormat shaderFormat, out string extension, out string entryPointName)
    {
        switch (backend)
        {
            case "vulkan":
                shaderFormat = SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_SPIRV;
                extension = "spv";
                entryPointName = "main";
                break;

            case "metal":
                shaderFormat = SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_MSL;
                extension = "msl";
                entryPointName = "main0";
                break;

            case "direct3d11":
                shaderFormat = SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_DXBC;
                extension = "dxbc";
                entryPointName = "main";
                break;

            case "direct3d12":
                shaderFormat = SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_DXIL;
                extension = "dxil";
                entryPointName = "main";
                break;

            default:
                throw new ArgumentException("This shouldn't happen!");
        }
    }

    public static Shader Load(GraphicsDevice graphicsDevice, string shaderName)
    {
        GenerateDataFromBackend(graphicsDevice.Backend, out SDL.SDL_GPUShaderFormat shaderFormat, out string extension, out string entryPointName);

        var fileText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "resources\\shaders\\compiled\\", $"{shaderName}.{extension}"));

        var shaderCreateInfo = new SDL.SDL_GPUShaderCreateInfo
        {
            code = SDL.SDL_StringToPointer(fileText),
            code_size = (nuint)fileText.Length,

            entrypoint = entryPointName,
            stage = shaderName[^4..] switch
            {
                "frag" => SDL.SDL_GPUShaderStage.SDL_GPU_SHADERSTAGE_FRAGMENT,
                "vert" => SDL.SDL_GPUShaderStage.SDL_GPU_SHADERSTAGE_VERTEX,
                _ => throw new Exception("Tried to create a shader with an invalid stage!"),
            },
            format = shaderFormat
        };


        var handle = SDL.SDL_CreateGPUShader(graphicsDevice.Handle, shaderCreateInfo);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create Shader: {SDL.SDL_GetError()}");
        }
        var shader = new Shader(handle);

        return shader;
    }

    public void Release(GraphicsDevice graphicsDevice)
    {
        SDL.SDL_ReleaseGPUShader(graphicsDevice.Handle, Handle);
    }
}
