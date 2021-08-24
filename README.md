# Naukri-Library

## AwaitCoroutine

可異步等候協程，透過為 `YieldInstruction` 新增 `GetAwaiter()` 並建立 Awaiter 使其支援異步等候。

使用範例

```cs
private async void Demo()
{
    // 透過新增 YieldInstruction 建立等候者
    await new WaitForEndOfFrame();
    // 透過 Awaiter 工廠建立等候者
    await Awaiter.WaitForEndOfFrame();
}
```

### 支援的等候選項

- `WaitForUpdate` 等候直到下一幀 Update 結束
- `WaitForEndOfFrame` 等候直到下一幀渲染結束
- `WaitForFixedUpdate` 等候直到下一次 FixedUpdate 結束
- `WaitForSeconds` 等候 Unity 時間 (秒)
- `WaitForSecondsRealtime` 等候現實時間 (秒)
- `WaitUntil` 等候直到條件成立
- `WaitWhile` 等候直到條件不成立

## BetterAttribute

`PropertyDrawer` 輔助開發框架，透過在 `BetterPropertyDrawer` 使用 `BetterGUILayout` 下的函式，將 `EditorGUI` 模擬為 `EditorGUILayout`。這可以讓 `BetterPropertyDrawer` 下的 `GetPropertyHeight()` 和 `OnGUI()` 描述合二唯一，使之維護更為便利。並且解放了單一欄位只能對應一個 `PropertyDrawer` 的限制，基於 `BetterPropertyDrawer` 所實現的自定義屬性可以在不衝突的前提下同時作用於對應欄位。

- `OnInit()` 初始化 `BetterPropertyDrawer`
- `OnGUILayout()` 繪製欄位，如果完成繪製及回傳 true ， 若為 false 則會依 `PropertyAttribute` 的 order 順延至下一個 `BetterPropertyDrawer` 直到回傳 true 中止，若都沒有完成繪製則會使用預設欄位進行繪製
- `OnBeforeGUILayout()` 用以修改對這個欄位的 GUI 參數及開啟 GUI Scope
- `OnAfterGUILayout()` 用以回復 `OnBeforeGUILayout()` 所修改的 GUI 參數及關閉 GUI Scope
- `LayoutWrapper()` 用以包裝沒有被寫進 `BetterGUILayout` 的 EditorGUIField

### 擴充屬性

- `CustomObjectField` 自訂 ObjectField 之 Object Selector 可選擇的目標型態
- `DisplayName` 改變欄位名稱
- `DisplayObjectFields` 在子欄位顯示目標 `UnityObject` 欄位
- `DisplayWhenFieldEqual` 當任一條件成立時才顯示欄位
- `DisplayWhenFieldNotEqual` 當所有條件皆不成立時才顯示欄位
- `ElementName` 改變陣列中元素的前綴
- `ExpandElement` 直接展開在陣列中的元素
- `ForkName` 依照不同條件顯示不同欄位名稱
- `ReadOnly` 使欄位唯讀

## BetterInspector

基於 `Editor` 的開發框架，新增一個繼承自 `BetterInspectorEditor` 的類別並使用 `CustomEditor` 標註目標屬性，即可使用本框架。

透過 `InspectorAttribute` 標記繪製器並使用 `InspectorMemberDrawer` 和 `CustomInspectorDrawer` 實作，便能夠讓屬性及方法獲得類似序列化的欄位繪製在 Inspecotr 上的功能。

### 擴充屬性 (只作用使用 BetterInspector 繪製的類別)

- `DefaultInspector` 使用原生方式繪製 (在類別上標記)
- `DisplayProperty` 顯示屬性欄位

    ⚠️ 自動實作屬性會因為沒有被序列化而無法儲存資料。可以使用以下方法取得類似的效果，但要注意此時他會被視為欄位需使用 `PropertyDrawer` 來自定義

    ```cs
        [field: SerializeField]
        public int AutoImplementedProperty { get; set; }
    ```

- `DisplayMethod` 顯示方法欄位，並能且透過 Invoke 按鈕觸發方法

### 兼容屬性

- `ReadOnly` 使欄位唯讀

## Collections

自定義集合

- `KeyList` 可同時使用索引和鍵值查詢的列表
- `SerializableDictionary` 可序列化字典
- `SerializableHashSet` 可序列化唯一集合

## Extensions

擴充函式

- `DeconstructMethods`
- `DelegateMethods`
- `EnumMethods`
- `EnumerableMethods`
- `IListMethods`
- `StringMethods`
- `TypeMethods`

## Factory

一些工廠函式

- `ScriptFactory` 腳本工場，透過定義 ScriptTemplate 可以自定義腳本模板，同時能解決因 Unity 預設編碼為 Big5 所導致的中文亂碼錯誤。
- `TextureFactory` 紋理工廠

## Helper

基於 UnityEditor 的輔助工具，在 MenuItem/Naukri 下可以找到並使用。

- `MissingScriptCleaner` 清除所有選擇的 `GameObject` 中遺失腳本的 `MonoBehaviour`

## Reflection

### `CastTo`

透過使用

```cs
T dst = CastTo<T>.From(src);
```

將 src 轉型成 T，在編譯器無法準確判定型態的泛型方法中很好用。

### `FastReflection`

透過委派來加速反射對特定物件欄位的存取速度，具體效能約為

> 直接存取 1 : 快速反射 2 : 一般反射 391

使用範例

```cs
public class Demo
{
    public int src { get; set; } = 1;

    FastGetter<Demo, int> srcGetter;

    FastSetter<Demo, int> srcSetter;

    private void Example()
    {
        var type = GetType();
        srcGetter = type.GetProperty(nameof(src)).CreateFastGetter<Demo, int>();
        srcSetter = type.GetProperty(nameof(src)).CreateFastSetter<Demo, int>();
        srcSetter.Invoke(this, 2);
        var dst = srcGetter.Invoke(this); // dst will be 2
    }
}
```

⚠️ 保留快速反射的委派而不是每次反射時再建立一次，否則效能會比一般反射差上數倍。

## SceneManagement

輔助控制、排程載入 Unity 場景

- `AdditiveSceneLoader` 在目標移動到指定範圍時自動載入、卸載場景
- `SceneObject` 透過儲存 `SceneAsset` 的 name 輔助存取場景資產

## Singleton

適用於 Unity 的單例模式，會在腳本建置時在目標位置自動生成所需的 `.asset` 檔，也可以點擊 `MenuItem/Naukri/Create Singleton Asset` 手動生成。

### `SingletonBehaviour`

基於 `MonoBehaviour` 的單例，會優先搜索場景中是否存在對應的單例，若無則建立之。

### `SingletonResource`

基於 `ScriptableObject` 的單例，使用 `AssetPathAttribute` 指定單例物件生成/讀取位置，路徑中須包含 `Resources` 資料夾才能正確被識別。

### `SingletonAsset`

類似於 [`SingletonResource`](#SingletonResource) 但由於只作用於 UnityEditor，所以路徑中不須包含 `Resources` 資料夾。

## Timer

計時器

- `CountdownTimer` 倒數計時器
- `StopwatchTimer` 正數計時器

## Toast

在螢幕的特定角落生成提示訊息。

使用 `Toaster.Create()` 可以使用快速生成預設風格的訊息

也可以透過建立 `Toast` 作為模板，並使用 `ToastManager` 來動態生成自定義風格的訊息，具體流程請參考預製物 `DefaultToast` 和 `DefaultToastManager`

---

## `EventInvoker`

可以在 Inspector 加入 `EventInvoker` 後於面板中透過 Invoke 按鈕觸發對應的 `UnityEvent`，也可以透過定義的快捷鍵觸發觸發事件。

## `Flag`

用以增加 EnumFlag 的可讀性

```cs
using Naukri;
using System;

[Flags]
public enum DemoFlag
{
    None = Flag.NONE,
    Flag1 = Flag.BIT00,
    Flag2 = Flag.BIT01,
}
```

## `Interpolation`

一些擴充的插值，可以直接調用對應的函式。也可以透過在 `HandleByType()` 傳入目標 `InterpolationType` 動態選擇對應插值。

## `NaukriBehaviour`

綁定好 `BetterInspector` 的 `MonoBehaviour`

- `GetComponentsRecursive` 取得特定深度下所有子物件的 Component

## `NaukriScriptableObject`

綁定好 `BetterInspector` 的 `ScriptableObject`

## `UnityPath` & `EditorUnityPath`

取得基於 Unity 專案的相對路徑

提醒 / 建議

- 只會序列化 public 欄位 (包含自動實作屬性)
- 可以使用 `YamlIgnore` 排除不想序列化的 public 欄位
- 可以透過 `YamlMember` 改變一些基礎屬性
- 建立一個序列化專用的物件並映射到目標物件上，而不是直接序列化目標物件
- 引用 `YamlDotNet-11.1.1` 函式庫
