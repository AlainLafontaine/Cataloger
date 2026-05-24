# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Developer role

See [.claude/memory/roles/developpeur.md](.claude/memory/roles/developpeur.md) for the expected developer profile, technical expertise, and communication style to apply in this project.

## Solution structure

Multi-project .NET 8.0 solution (`Cataloger.sln`). Each project has a fixed role:

| Project | Role |
|---|---|
| `Cataloger` | WinForms app (WinExe) — UI layer: Composantes, Presenters, Services, Views |
| `Cataloger.Business` | Business actions (CRUD) called by the service layer |
| `Cataloger.Core` | DB entities, DTOs, repository interfaces |
| `Cataloger.DataAccess.Sqlite` | SQLite repository implementations |
| `Zzz.App.Core.AccesDonnees.Sqlite.Sql` | SQLite/SQL provider (framework-level, rarely touched) |
| `BaseWinform` | MVP framework library — see `BaseWinform/CLAUDE.md` for details |

## Build and run

```
dotnet build Cataloger.sln
dotnet run --project Cataloger
```

Both `dotnet` CLI and Visual Studio work. There are no custom build scripts.

## MVP architecture

Every screen follows the same three-part pattern:

1. **View interface** (`Cataloger/Views/I<Feature>View.cs`) — extends `ICatalogerView`. Declares only what the Presenter needs to read or write on the Composante.
2. **Composante** (`Cataloger/Composants/<Domain>/<Feature>.cs`) — extends `CatalogerComposante`, implements `I<Feature>View`. Contains only UI logic (binding, event wiring). No business logic here.
3. **Presenter** (`Cataloger/Presenters/<Domain>/<Feature>Presenter.cs`) — extends `CatalogerPresenter<I<Feature>View>`. Decorated with `[PresenterURL("url-slug")]`. Contains all business/presentation logic. `InitPresenter()` is the entry point called on load.

**Auto-registration**: Composantes and Presenters are registered in the DI container automatically by `Startup.cs` via reflection. Do NOT add manual `services.AddTransient(...)` calls for new Composantes or Presenters.

## URL-based navigation

Navigation is URL-driven. To navigate to a screen:
```csharp
navigationService.Naviguer("url-slug");
navigationService.Naviguer("url-slug", data, permetPrecedent: true);  // with back support
```

The `[PresenterURL("url-slug")]` attribute on the Presenter is the sole registration mechanism. The URL in the attribute must match exactly what is passed to `Naviguer`.

## Business actions (Cataloger.Business)

Each action is a class in `Cataloger.Business` that follows this structure:

```csharp
#if __INCLUS_THIS_ACTION__
// ... Requete, Reponse, and Action classes
#endif // __INCLUS_THIS_ACTION__
```

The `#if __INCLUS_THIS_ACTION__` symbol is always defined (both Debug and Release). Every business action file must be wrapped in this directive.

Actions inherit `SecureActionBase<TRequete, TReponse>` and are decorated with `[GetApi]`, `[PostApi]`, `[PutApi]`, or `[DeleteApi]`. They are auto-registered via `services.AddActions(...)` — no manual DI needed.

## Service layer (PresenterDirectAccessAction)

Services in `Cataloger/Services/` extend `PresenterDirectAccessAction` and call business actions using typed HTTP-style methods:

```csharp
get("url", out IEnumerable<MyDto>? dtos);
post<MyDto>("url", body, out _, Succes: (dto) => { ... return displayMsg; });
put<MyDto>("url/id", body, out _, Succes: (dto) => { ... return displayMsg; });
delete("url/id", Succes: () => { ... return displayMsg; });
```

## Database entities (Cataloger.Core)

DB entities use ORM annotations from `Zzz.App.Core.Donnees`:

```csharp
[TableBd("CAT_TABLE_NAME")]
public class MyEntityDb
{
    [ClePrimaire]
    [ChampBd("COL_NO_SEQ")]
    [SequenceBd("CAT_COL_NO_SEQ")]
    public long MyEntityId { get; set; }

    [ChampBd("COL_FIELD")]
    public string Field { get; set; } = string.Empty;
}
```

Table name prefix: `CAT_`. Column prefix: abbreviated to match existing conventions (e.g., `SPM_` for SystemParameter). Repositories extend `RepositorySqliteCRUDBase<TDb>` and implement `IRepositoryCRUDBase<TDb>`. They are auto-registered via `services.AddRepositories(...)`.

## Naming conventions

- **Domain layer** (entities, DTOs, actions, Requete/Reponse classes): French names are acceptable and follow existing patterns (e.g., `Requete`, `Reponse`, `SystemParameterDb` column prefixes).
- **Infrastructure layer** (services, repositories, presenters, composantes): English names.
- Always use `CatalogerComposante` as the base for top-level screens; `CatalogerChildComposante` for embedded child screens.
- Presenter names end in `Presenter`; view interfaces start with `I` and end in `View`.

## UI controls

Use DevExpress controls exclusively. Do not introduce `System.Windows.Forms` controls like `TextBox`, `ComboBox`, `DataGridView`, etc. Use their DevExpress equivalents (`TextEdit`, `ComboBoxEdit`, `GridControl`, etc.).

## Testing

There are no unit test projects. Testing is done manually via the `Test` composante (`Cataloger/Composants/Tests/Test.cs`) inside the running application. When adding a new feature, verify behavior by running the app.

## Configuration

`Cataloger/appsettings.json` controls environment, database location, and AWS settings. The `ModeTest` flag switches the app to test mode. `securite.config` handles security configuration.
