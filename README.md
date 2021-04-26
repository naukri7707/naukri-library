# Naukri-Library

## Reflection

### `Assembly`

一些需要透過存取 Assembly 才能實現的操作，比如 `GetDerivedTypesOf<T>()` 取得某類別的所有子類別。

### `CastTo`

透過使用

```cs
T dst = CastTo<T>.From(src);
```

來將 src 轉型成 T，在編譯器無法準確判定型態的泛型方法中很好用。

### `FastReflection`

透過委派來加速反射對特定物件欄位的存取速度，具體效能約為

> 直接存取 1 : 快速反射 2 : 一般反射 391

#### 使用範例

```cs
public class Demo
{
    public int src { get; set; } = 1;

    GetterDelegate<Demo, int> srcGetter;

    SetterDelegate<Demo, int> srcSetter;

    private void Example()
    {
        var type = GetType();
        srcGetter = type.GetProperty(nameof(src)).CreateGetterDelegate<Demo, int>();
        srcSetter = type.GetProperty(nameof(src)).CreateSetterDelegate<Demo, int>();
        srcSetter.Invoke(this, 2);
        var dst = srcGetter.Invoke(this); // dst will be 2
    }
}
```

⚠️ 保留快速反射的委派而不是每次反射時再建立一次，否則效能會比一般反射差上數倍。

---

## BetterAttribute

`PropertyDrawer` 輔助開發框架，透過在 `BetterPropertyDrawer` 使用 `BetterGUILayout` 下的函式，將 `EditorGUI` 模擬為 `EditorGUILayout`。

這可以讓 `BetterPropertyDrawer` 下的 `GetPropertyHeight()` 和 `OnGUI()` 描述合二唯一，使之維護更為便利。

### 擴充屬性

- `DisplayName` 改變欄位名稱
- `DisplayUnityObjectFields` 在子欄位顯示目標 `UnityObject` 欄位
- `DisplayWhenFieldEqual` 當條件成立時才顯示欄位
- `DisplayWhenFieldNotEqual` 當條件不成立時才顯示欄位
- `ElementName` 改變陣列元素前綴
- `ForkName` 依照不同條件顯示不同欄位名稱
- `ReadOnly` 使欄位唯讀

## EventInvoker

可以在 `EventInvoker` 面板中透過 Invoke 按鈕觸發對應的 `UnityEvent`，也可以設定熱鍵後在 runtime 透過熱鍵觸發事件。

## Factory

一些工廠函式

- `ScriptFactory` 腳本工場，透過定義 ScriptTemplate 可以自定義腳本模板，同時能解決因 Unity 預設編碼為 Big5 所導致的中文亂碼錯誤。

- `TextureFactory` 紋理工廠

## Interpolation

一些擴充的插值，可以直接調用對應的函式。也可以透過 `InterpolationType` 使用 `ByMethod()` 動態選擇對應插值。

## Serializable

適用於 Unity 可序列化的資料結構，透過 `ISerializationCallbackReceiver` 介面自動進行與 `List` 的轉換

- `SerializableDictionary` 可序列化字典
- `SerializableHashSet` 可序列化唯一集合

## Singleton

適用於 Unity 的單例模式

### `SingletonBehaviour`

基於 `MonoBehaviour` 的單例，會優先搜索場景中是否存在對應的單例，若無則建立之。

### `SingletonResource`

基於 `ScriptableObject` 的單例，使用 `AssetPathAttribute` 指定單例物件生成/讀取位置，路徑中須包含 `Resources` 資料夾才能正確被識別。

### `SingletonAsset`

類似於 [`SingletonResource`](#SingletonResource) 但由於只作用於 UnityEditor，所以路徑中不須包含 `Resources` 資料夾。

## Toast

在螢幕的特定角落生成提示訊息。

使用 `Toaster.Create()` 可以使用快速生成預設風格的訊息

也可以透過建立 `Toast` 作為模板，並使用 `ToastManager` 來動態生成自定義風格的訊息，具體流程請參考預製物 `DefaultToast` 和 `DefaultToastManager`
