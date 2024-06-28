var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.ConnectionClose = true;

if (args.Length > 1 && Uri.TryCreate(args[1], UriKind.Absolute, out var uri))
    return await httpClient.GetAsync(uri).ContinueWith(r => r.Result.IsSuccessStatusCode) ? 0 : 1;

throw new ArgumentException("A valid URI must be given as first argument");