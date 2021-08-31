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

`PropertyDrawer` 輔助開發框架，透過 `BetterGUIWrapper` 包裝將 `GetPropertyHeight()` 和 `OnGUI()` 描述合二唯一成 `OnGUILayout`，使之維護更為便利。並且解放了單一欄位只能對應一個 `PropertyDrawer` 的限制，基於 `BetterPropertyDrawer` 所定義屬性可以在不衝突的前提下同時作用於對應欄位。

- 在繪製任何欄位時皆需要在 `BetterGUILayout` 下以包裝好的欄位或使用 `BetterGUILayout.Wrapper()` 做包裝，並用 `yield return` 回傳給框架
- 原本 `OnGUI()` 所傳遞的參數 `position` 會被 `BetterPropertyDrawer.position` 所替代
- `position.y` 會在每次繪製後結束後自動換行 (+18F)
- `position.height` 永遠為 18F 只有在繪製包裝器的匿名函式時會變為包裝器所定義的高度
- `position.x` 、`position.width` 永遠與 `OnGUI()` 所傳遞的參數 `position` 相同
- 框架只會在優先序最高的 Drawer 中展開，其餘 Drawer 則會使用預設繪製器進行繪製 (避免 Unity 指定的繪製器不是優先序最高的)
- 框架展開後會取得包含自己在內的所有指定 Drawer 的克隆體，並使用這些克龍體產生實際繪製行為
- `OnInit()` 初始化 `BetterPropertyDrawer`
- `OnGUILayout()` 使用 `yield return BetterGUIWrapper` 繪製欄位、或用 `yield break` 中斷繪製，如果繪製器尚未完成過繪製，會嘗試繪製下一個優先序的 `BetterPropertyDrawer` 直到繪製完成或沒有其他的繪製器，若直到結束都沒有完成過繪製則會使用預設方法進行繪製
- `OnBeforeGUILayout()` 用以修改對這個欄位的 GUI 參數及開啟 GUI Scope
- `OnAfterGUILayout()` 用以回復 `OnBeforeGUILayout()` 所修改的 GUI 參數及關閉 GUI Scope

### 擴充屬性

- `CustomObjectField` 自訂 ObjectField 之 Object Selector 可選擇的目標型態
- `DisplayName` 改變欄位名稱
- `DisplayObjectFields` 在子欄位顯示目標 `UnityObject` 欄位
- `DisplayWhenFieldEqual` 當任一條件成立時才顯示欄位
- `DisplayWhenFieldNotEqual` 當所有條件皆不成立時才顯示欄位
- `DisplayWhenHasFlag` 當擁有定應旗標時顯示欄位
- `ElementName` 改變陣列中元素的前綴
- `ExpandElement` 直接展開在陣列中的元素
- `ForkName` 依照不同條件顯示不同欄位名稱
- `LongFlag` 將 `long` 欄位繪製成對應的 `Enum`
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

- `Heap` 堆積的抽象類別，可以使用預設的 `MaxHeap` 、 `MinHeap` 或繼承並實作自定義堆積
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
- `LinqMethods`
- `StringMethods`
- `TypeMethods`
- `UnityMethods`

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
  ⚠️ `AdditiveSceneLoader` 會 disable 目標 `Collider` 並將其設為 Trigger 以提升效能
- `SceneObject` 透過儲存 `SceneAsset` 的 name 輔助存取場景資產

## Scope

配合 using 在區域進入及離開時產生特定行為

使用範例

```cs
    int value = 0;

    void Test()
    {
        using (Scope.TempReplace(()=> ref value, 10))
        {
            // now value is 10
            value = 5;
            // now value is 5
        }
        // now value is 0;
    }
```

- `Event()` 在進入、離開區域時將調用目標函式
- `SetValue()` 在進入、離開區域時將目標參考設為對應的值
- `TempReplace()` 在進入區域時將目標參考設為對應的值，並在離開時還原

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

用來定義 EnumFlag 以增加其可讀性

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

- `Flag` 用以定義基底類型為 `int` 、 `uint` 的類型，其基底類型為 `uint`
- `LongFlag` 用以定義基底類型為 `long` 、 `ulong` 的類型，其基底類型為 `ulong`

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

## Utility

通用的輔助函式
