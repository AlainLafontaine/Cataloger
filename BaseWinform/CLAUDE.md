# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Rôle du projet

`BaseWinform` est la bibliothèque framework de l'application. Elle fournit l'infrastructure MVP (Model-View-Presenter), la navigation par URL, et les services WinForms de base. Elle est indépendante du domaine IMA.

Cible : `net8.0-windows10.0.22000.0`, x64.

## Patron Composante / Presenter

### Composante (View)
- `BaseComposante` hérite de `NavigationCtrl` (UserControl DevExpress) et implémente `IBaseComposante`
- `ChildComposante` est un UserControl enfant embarqué dans un `BaseComposante`
- Les éditeurs DevExpress (`BaseEdit`) dans la composante déclenchent automatiquement `IsDirty = true` via `WireAllEditors()`
- `RemiseAZeroIsDirty()` / `AcceptChanges()` réinitialisent l'état dirty après enregistrement

### Presenter
- `BasePresenter<I>` est la classe de base; `I` est l'interface de la Composante associée
- `InitPresenter()` est déclenché par l'événement `InitComposante2` de la Composante lors du `Load`
- `MajEtatControl()` est appelé en arrière-plan pour mettre à jour l'état des contrôles
- `ReleasePresenter()` libère les références et les ChildPresenters

### ChildComposante / ChildPresenter
Lors de l'ajout d'une `ChildComposante` dans le `BaseControlCollection`, le mécanisme automatique :
1. Détecte l'interface spécifique implémentée par la ChildComposante (ex. `IMonChildComposante`)
2. Cherche le type `ChildPresenter` associé dans le dictionnaire `BaseComposante.childPresenters`
3. Crée le ChildPresenter via `IFactory`, l'injecte avec `InjectionComposante()`, et l'initialise

Les ChildPresenters s'enregistrent dans ce dictionnaire au démarrage via `Startup.cs` (côté `Ima.Windows`).

## Navigation par URL

`NavigationService` maintient un dictionnaire `URL → Type Presenter` construit à l'initialisation par réflexion sur les attributs `[PresenterURL("mon-url")]`. 

- `Naviguer(url)` : remplace le Presenter courant
- `Naviguer(url, data, permetPrecedent: true)` : permet un retour arrière
- `Precedent(data)` : revient au Presenter précédent
- `ShowPremierePage(url)` : charge le premier écran

Un guard `GuardSiDirty` demande confirmation si `IsDirty` est vrai avant toute navigation.

## Accès aux Actions (PresenterDirectAccessAction)

Les Presenters qui héritent de `PresenterDirectAccessAction` peuvent appeler le backend Affaire via des méthodes typées :

```csharp
get<MaReponseDto>("ma/url", out var dto);
post<MaReponseDto>("ma/url", body, out var dto);
put("ma/url", body);
delete("ma/url");
```

`PresenterDirectAccessAction.Init("Ima.Affaire.dll")` doit être appelé au démarrage pour charger le dictionnaire URL → Action par réflexion sur les attributs `[GetApi]`, `[PostApi]`, `[PutApi]`, `[DeleteApi]`.

## Services

- `NavigationService` — gestion de la navigation URL
- `DevExpressRestoreService` — restauration de l'état des formulaires DevExpress
- `GeoLocalisateurService` — localisation GPS (modes : Simulateur, GeoLocator, NMEA)
- `IsDesignModeService` — détecte si l'application tourne en mode design VS
- `CommandeService` — gestion des commandes (pattern Command)

## Interfaces clés

| Interface | Usage |
|---|---|
| `IBaseComposante` | Contrat minimal de toute Composante |
| `IBasePresenter<I>` | Contrat minimal de tout Presenter |
| `IChildComposante` | Marqueur pour les composantes enfants |
| `IChildPresenter` | Contrat pour les Presenters enfants |
| `IDirtyPresenter` | Tracking des modifications non sauvegardées |
| `ITransfertData` | Données passées lors d'une navigation |
| `IGeoLocalisateur` | Abstraction du GPS |
