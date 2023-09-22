using Microsoft.JSInterop;

namespace Blazor.WASM.PWA.Client.Data; 

public class LocalForecastStore : IAsyncDisposable {
    private readonly HttpClient httpClient;
    private readonly IJSRuntime js;
    private Lazy<IJSObjectReference> accessorJsRef = new();

    public LocalForecastStore(HttpClient httpClient, IJSRuntime js)
    {
        this.httpClient = httpClient;
        this.js = js;
    }
    
    public async Task InitializeAsync()
    {
        await WaitForReference();
        await accessorJsRef.Value.InvokeVoidAsync("initialize");
    }
    
    public async Task<T> GetAsync<T>(string collectionName, int id)
    {
        await WaitForReference();
        var result = await accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);

        return result;
    }
    
    public async Task<T> GetAllAsync<T>(string collectionName)
    {
        await WaitForReference();
        var result = await accessorJsRef.Value.InvokeAsync<T>("getAll", collectionName);

        return result;
    }

    public async Task SetAsync<T>(string collectionName, T value)
    {
        await WaitForReference();
        await accessorJsRef.Value.InvokeVoidAsync("set", collectionName, value);
    }
    
    private async Task WaitForReference()
    {
        if (accessorJsRef.IsValueCreated is false)
        {
            accessorJsRef = new(await js.InvokeAsync<IJSObjectReference>("import", "/localForecastStore.js"));
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        if (accessorJsRef.IsValueCreated)
        {
            await accessorJsRef.Value.DisposeAsync();
        }
    }
}