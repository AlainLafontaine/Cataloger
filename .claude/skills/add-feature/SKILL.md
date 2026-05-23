---
name: add-feature
description: Add a new screen/feature following the Cataloger MVP pattern (Composante + View interface + Presenter + optional Service). Use when the user asks to add a new form, screen, or UI feature.
---

When the user asks to add a new screen or feature, follow these steps in order. Ask for the feature name and URL slug if not provided.

## Step 1 — View interface

Create `Cataloger/Views/I<Feature>View.cs`:

```csharp
using Cataloger.Core.Entities.<Domain>.Dto;

namespace Cataloger.Views
{
    public interface I<Feature>View : ICatalogerView
    {
        // Declare only what the Presenter needs to read/write on the Composante
        // Load methods: void Load<Something>(IEnumerable<SomeDto> items);
        // Properties: SomeDto? SelectedItem { get; set; }
    }
}
```

## Step 2 — Composante (View)

Create `Cataloger/Composants/<Domain>/<Feature>.cs` (and its `.Designer.cs`).
In Visual Studio: Add → New Item → User Control (WinForms). Then manually change the base class.

```csharp
using BaseWinform.Attributes;
using Cataloger.Views;
using Cataloger.Composantes;

namespace Cataloger.Composants
{
    public partial class <Feature> : CatalogerComposante, I<Feature>View
    {
        public <Feature>()
        {
            InitializeComponent();
        }

        // Implement interface members here
        // UI controls MUST be DevExpress — use TextEdit, ComboBoxEdit, GridControl, etc.
        // NOT TextBox, ComboBox, DataGridView
    }
}
```

**Important**: DI registration is automatic via reflection in `Startup.cs`. Do NOT add a manual `services.AddTransient(...)` for the Composante.

## Step 3 — Presenter

Create `Cataloger/Presenters/<Domain>/<Feature>Presenter.cs`:

```csharp
using BaseWinform.Attributes;
using Cataloger.Presenters.Bases;
using Cataloger.Views;

namespace Cataloger.Presenters.<Domain>
{
    [PresenterURL("<url-slug>")]
    public class <Feature>Presenter : CatalogerPresenter<I<Feature>View>
    {
        // Inject services via constructor
        public <Feature>Presenter(
            I<Feature>View view
            // , MyService myService  ← inject services here
        ) : base(view)
        {
        }

        public override void InitPresenter(object? sender, EventArgs? e)
        {
            base.InitPresenter(sender, e);
            // Load initial data, populate composante
        }

        public override void ReleasePresenter()
        {
            base.ReleasePresenter();
        }
    }
}
```

**Important**: The `[PresenterURL]` attribute is how the navigation system finds this presenter. The slug must be unique across all presenters. DI registration is automatic.

## Step 4 — Service (if the feature needs data access)

Create `Cataloger/Services/<Feature>Service.cs`:

```csharp
using BaseWinform.AccesAction;
using Cataloger.Core.Entities.<Domain>.Dto;

namespace Cataloger.Presenters
{
    public class <Feature>Service : PresenterDirectAccessAction
    {
        public <Feature>Service() {}

        public IEnumerable<<Feature>Dto> GetList<Feature>()
        {
            IEnumerable<<Feature>Dto>? dtos = null;
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            get("<url-slug>", out dtos);
            return dtos!;
        }

        // Add Create/Modify/Delete methods following SystemParameterService as the model
    }
}
```

## Step 5 — Navigate to the new screen

To add a navigation entry point (e.g., a menu button), call:
```csharp
navigationService.Naviguer("<url-slug>");
```

## Checklist

- [ ] `I<Feature>View.cs` created in `Cataloger/Views/`
- [ ] `<Feature>.cs` (Composante) created in `Cataloger/Composants/<Domain>/` — extends `CatalogerComposante`, implements `I<Feature>View`
- [ ] `<Feature>Presenter.cs` created in `Cataloger/Presenters/<Domain>/` — decorated with `[PresenterURL]`
- [ ] Service created in `Cataloger/Services/` if data access is needed
- [ ] All UI controls are DevExpress (no standard WinForms controls)
- [ ] No manual DI registration added in `Startup.cs`
- [ ] URL slug is unique across all `[PresenterURL]` attributes
