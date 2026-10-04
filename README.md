# Bludoku — refactor notes

gfork for unity tset task

---

## added

| What | Why |
| --- | --- |
| **ConfigSystem** (`ConfigService`, `ConfigItem`, settings assets under `Resources/Configs`) | Central place for tunables (combo, save keys, analytics). Logic reads typed settings instead of hard-coded constants scattered across scripts. |
| **SaveSystem** (`SaveService` + `SaveSettings`) | Single save API over keys. No one touch `PlayerPrefs` directly, so the backend can later swap to JSON / bundles / cloud without rewriting every consumer. |
| **MonoPool** (`MonoPool<T>`, `MonoPoolableItem`, `IPoolableItem`) | Generic MonoBehaviour pool (`GetFreeElement` / `SetFreeElement`) reused for score crumbs — no Instantate/Destroy spam. |
| **AnalyticsService** (`AnalyticsKey(s)`, providers, `AnalyticsSettings`) | Lightweight track API with named keys + optional query params, so events stay consistent and provider-swappable (console now, real SDK later). |
| **Combo settings + clear VFX** (`ComboSettings`, score crumbs, camera bump) | Designer-tunable combo juice without baking numbers into views/effects. |
| **ScoreData** | Concrete serializable blob for score / high score / combo — data separated from `ScoreSystem` logic. |

---

## refactored

| Script | Why |
| --- | --- |
| `MainMenu/SaveSystem.cs` | level progress now goes thru `SaveService` + `SaveSettings` keys. |
| `MainMenu/Settings/SettingsManager.cs` | sound/music/vibration prefs now thru `SaveService` (no direct `PlayerPrefs`). |
| `Blocks/FiguresSaveLoad.cs` | save/load via `SaveService` instead of raw prefs. |
| `Score/ScoreSystem.cs` | persists `ScoreData` thru `SaveService` + scoring logic stays here, state is in data. |
| `Score/ScoreData.cs` | just plain data (better to understand + divided logic more drastically). |
| `Score/ScoreMediator.cs` | wired combo/analytics and made stuff de-coupld |
| `Score/ScoreView.cs` | score UI updated along `ScoreData` / mediator flow |
| `UI/BestScoreText.cs` | best score now reads from unified score data path. |
| `UI/GameOver.cs` | game-over score/high-score display aligned with new score data. |
| `Boards/Board.cs` | more thicc `ClearResult` for scoring/effects/analytics. |
| `Boards/ClearResult.cs` | now carries clear count, positions, place coords, figure id (analytics and overall great to have). |
| `Core/FiguresController.cs` | wired placements and analytics events with params. |
| `Effects/EffectsManager.cs` | wired vfx - camera bump, pooled crumbs. |

---

## what i also wanna change but kinda lazy to do atm

- **Pool:** reuse a lot of objects instead of Spawn/Destroy loops (huge optimization spikes) (currently only crumbs use so)
- **Data vs logic:** a lot of static fields and overall hardcoded stuff, move everything to configs so they can be either loaded in future (for example level pattern can be loaded or figures), or just be changed easier without looking in code (for example managers/designers stuff could do so, not only programmers)
- **Single Entry Point:** currently a lot of stuff are just stickd with hope, and managed each on their own scene which can lead to **repeating** your code/other stuff, + if project grows this shit is gonnaa be rly haard to expand. **DI** could help here but itss not worth the hussle with containers
- **not many prefabs:** i saw like 2 prefabs at best which are not really do the job especially if many ppl work with UI on different branches (it would straight up broke scene assets lol)
- **zero base packets**: no base menus, no base tiles, no base vfx, nothing. All i saw is "Panel" class but its also not optimized. Project lacks **"FOUNDATION"** and proper structure, it currently spams Singletons which are not great at all
- **services/DI/service locator/collection:** would help with this singleton spam and properly divide logic/views
- **remove all the GetComponent<>:** even in Awake/Start, i know its not much but its a lot of small components like texts which in compound give like 10-20fps drop at startup which are significant on low end mobiles, plus there is no need in them. We can assign everything in [SerializeField] variables at start anyway, literaally no point in GetCommponents, the only justified use is on physics but u can create a HitScanMap there too so yeah

not so important:
- **proper code style:** no comments here, .prettier or smth could help (or cloud save of settings if u use Idea IDE). currently code style not unified but this have no point for bussines prespective so doesnt matter that much
