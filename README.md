# Naukri Library

## `Better Attribute`

`PropertyDrawer` 輔助開發框架，透過在 `BetterPropertyDrawer` 使用 `BetterGUILayout` 下的函式，將 `EditorGUI` 模擬為 `EditorGUILayout`。

這可以讓 `PropertyDrawer` 下的 `GetPropertyHeight()` 和 `OnGUI()` 描述合二唯一，使之維護更為便利。

### 擴充屬性

- `DisplayName` 改變欄位名稱
- `DisplayUnityObjectFields` 在子欄位顯示目標 `UnityObject` 欄位
  
  > ⚠️ 若目標的欄位帶有 `DisplayUnityObjectFields` 將無法正確渲染
- `DisplayWhenFieldEqual` 當條件成立時才顯示欄位
- `DisplayWhenFieldNotEqual` 當條件不成立時才顯示欄位
- `ElementName` 改變陣列元素前綴
- `ForkName` 依照不同條件顯示不同欄位名稱
- `ReadOnly` 使欄位唯讀
