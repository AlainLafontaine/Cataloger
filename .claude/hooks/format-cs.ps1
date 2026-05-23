$raw = [Console]::In.ReadToEnd()
$json = $raw | ConvertFrom-Json
$f = $json.tool_input.file_path
if ($null -ne $f -and $f -match '\.cs$') {
    dotnet format Cataloger.sln --include "$f" 2>$null
}
