---
name: add-entity
description: Add a new database entity end-to-end: DB class with ORM annotations, DTO, repository interface + SQLite impl, Business CRUD actions, and Service. Use when the user asks to add a new table or domain entity.
---

When the user asks to add a new entity or table, follow these steps. Ask for the entity name, table name, and required fields if not provided.

Use `SystemParameter` as the reference implementation throughout.

## Step 1 — DB entity (Cataloger.Core)

Create `Cataloger.Core/Entities/<Domain>/<Entity>Db.cs`:

```csharp
using Zzz.App.Core.Donnees;

namespace Cataloger.Core.Entities.<Domain>
{
    [TableBd("CAT_<TABLE>")]
    public class <Entity>Db
    {
        [ClePrimaire]
        [ChampBd("<PREFIX>_NO_SEQ")]
        [SequenceBd("CAT_<PREFIX>_NO_SEQ")]
        public long <Entity>Id { get; set; }

        [ChampBd("<PREFIX>_FIELD")]
        public string Field { get; set; } = string.Empty;

        // nullable fields use null defaults:
        [ChampBd("<PREFIX>_VAL_STR")]
        public string? ValString { get; set; } = null;
    }
}
```

Naming conventions:
- Table name: `CAT_` prefix + uppercase snake_case (e.g., `CAT_SYSTE_PARAM`)
- Column prefix: short abbreviation in uppercase (e.g., `SPM_` for SystemParameter)
- Primary key sequence: `CAT_<PREFIX>_NO_SEQ`

## Step 2 — DTO (Cataloger.Core)

Create `Cataloger.Core/Entities/<Domain>/Dto/<Entity>Dto.cs`:

```csharp
using System.ComponentModel.DataAnnotations;

namespace Cataloger.Core.Entities.<Domain>.Dto
{
    public class <Entity>Dto
    {
        public long <Entity>Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Field { get; set; } = string.Empty;

        // Mirror the DB entity fields, using .NET types
    }
}
```

## Step 3 — Repository interface (Cataloger.Core)

Create `Cataloger.Core/Repositories/I<Entity>Repository.cs`:

```csharp
using Zzz.App.Core.Donnees;
using Cataloger.Core.Entities.<Domain>;

namespace Cataloger.Core.Repositories
{
    public interface I<Entity>Repository : IRepositoryCRUDBase<<Entity>Db>
    {
    }
}
```

## Step 4 — Repository implementation (Cataloger.DataAccess.Sqlite)

Create `Cataloger.DataAccess.Sqlite/Repositories/<Entity>Repository.cs`:

```csharp
using Cataloger.Core.Entities.<Domain>;
using Cataloger.Core.Repositories;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sqlite.Sql;

namespace Cataloger.DataAccess.Sqlite.Repositories
{
    public class <Entity>Repository : RepositorySqliteCRUDBase<<Entity>Db>, I<Entity>Repository
    {
        public <Entity>Repository(AppConnexion connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }
}
```

**Important**: `services.AddRepositories(typeof(<Entity>Repository).Assembly)` in `Startup.cs` already auto-registers all repositories in this assembly. No manual DI registration needed.

## Step 5 — Business actions (Cataloger.Business)

Create one file per action in `Cataloger.Business/<Domain>/`. Every file must be wrapped in `#if __INCLUS_THIS_ACTION__`. Model each action after the existing `SystemsParameters` actions.

Typical CRUD set:
- `Get<Entity>Action.cs` → `[GetApi("<entity-slug>/sections/{section}/keys/{key}", "...")]`
- `GetList<Entity>Action.cs` → `[GetApi("<entity-slug>", "...")]`
- `Create<Entity>Action.cs` → `[PostApi("<entity-slug>", "...")]`
- `Modify<Entity>Action.cs` → `[PutApi("<entity-slug>/{id}", "...")]`
- `Delete<Entity>Action.cs` → `[DeleteApi("<entity-slug>/{id}", "...")]`

Each action file structure:
```csharp
#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.<Domain>.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.<Domain>
{
    public class Get<Entity>Requete : Requete
    {
        // URL parameters become properties here
    }

    public class Get<Entity>Reponse : Reponse
    {
        [HttpBody]
        public <Entity>Dto <Entity> { get; set; } = default(<Entity>Dto)!;
    }

    [GetApi("<entity-slug>/{id}", "Description")]
    public class Get<Entity>Action : SecureActionBase<Get<Entity>Requete, Get<Entity>Reponse>
    {
        private readonly I<Entity>Repository <entity>Repository;

        public Get<Entity>Action(ILogger logger, IGestionnaireSecurite gs, I<Entity>Repository <entity>Repository)
            : base(logger, gs)
        {
            this.<entity>Repository = <entity>Repository;
        }

        public override bool VerifierPermissions(Get<Entity>Requete requete) => true;

        protected override Get<Entity>Reponse ExecuterSiAutorise(Get<Entity>Requete requete)
        {
            var reponse = this.CreerReponse();
            reponse.<Entity> = this.<entity>Repository.Obtenir<<Entity>Dto>(new { Id = requete.Id });
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
```

**Important**: Actions are auto-registered via `services.AddActions(typeof(Get<SomeExistingAction>).Assembly)` in `Startup.cs`. No manual DI needed.

## Step 6 — Service (Cataloger)

Create `Cataloger/Services/<Entity>Service.cs` extending `PresenterDirectAccessAction`. Use `SystemParameterService` as the exact model.

## Step 7 — SQLite table

Add the `CREATE TABLE` statement to the SQLite database files:
- `Cataloger.DataAccess.Sqlite/catalog_initiale.sqlite` — base schema
- `Cataloger.DataAccess.Sqlite/catalog_test_data.sqlite` — test data

Use a SQLite tool (e.g., DB Browser for SQLite) or the `sqlite3` CLI to run the DDL.

## Checklist

- [ ] `<Entity>Db.cs` in `Cataloger.Core/Entities/<Domain>/` with `[TableBd]`, `[ChampBd]`, `[ClePrimaire]`, `[SequenceBd]`
- [ ] `<Entity>Dto.cs` in `Cataloger.Core/Entities/<Domain>/Dto/`
- [ ] `I<Entity>Repository.cs` in `Cataloger.Core/Repositories/`
- [ ] `<Entity>Repository.cs` in `Cataloger.DataAccess.Sqlite/Repositories/` — extends `RepositorySqliteCRUDBase`
- [ ] Business action files in `Cataloger.Business/<Domain>/` — each wrapped in `#if __INCLUS_THIS_ACTION__`
- [ ] `<Entity>Service.cs` in `Cataloger/Services/` — extends `PresenterDirectAccessAction`
- [ ] SQLite table created in both `.sqlite` files
- [ ] No manual DI registration added
