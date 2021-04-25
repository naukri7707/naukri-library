# Naukri-Library

## Better Attribute

`PropertyDrawer` 輔助開發框架，透過在 `BetterPropertyDrawer` 使用 `BetterGUILayout` 下的函式，將 `EditorGUI` 模擬為 `EditorGUILayout`。

這可以讓 `PropertyDrawer` 下的 `GetPropertyHeight()` 和 `OnGUI()` 描述合二唯一，使之維護更為便利。

### 擴充屬性

- `DisplayName` 改變欄位名稱
- `DisplayUnityObjectFields` 在子欄位顯示目標 `UnityObject` 欄位
  
  ⚠️ 若目標的欄位帶有 `DisplayUnityObjectFields` 將無法正確渲染
- `DisplayWhenFieldEqual` 當條件成立時才顯示欄位
- `DisplayWhenFieldNotEqual` 當條件不成立時才顯示欄位
- `ElementName` 改變陣列元素前綴
- `ForkName` 依照不同條件顯示不同欄位名稱
- `ReadOnly` 使欄位唯讀

## Singleton

適用於 Unity 的單例模式

### SingletonBehaviour

基於 `MonoBehaviour` 的單例，會優先搜索場景中是否存在對應的單例，若無則建立之。

### SingletonResource

基於 `ScriptableObject` 的單例，使用 `AssetPathAttribute` 指定單例物件生成/讀取位置，路徑中須包含 `Resources` 資料夾才能正確被識別。

### SingletonAsset

類似於 [`SingletonResource`](#SingletonResource) 但由於只作用於 UnityEditor，所以路徑中不須包含 `Resources` 資料夾。
