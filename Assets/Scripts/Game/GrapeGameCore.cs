
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GrapeGameCore : MonoBehaviour {

    #region Component

    /// <summary>
    /// 水果工廠
    /// </summary>
    [SerializeField]
    private FruitFactory fruitFactory;
    public FruitFactory FruitFactory { get => fruitFactory; }

    /// <summary>
    /// 水果堆疊上限碰撞箱觸發器
    /// </summary>
    [SerializeField]
    private StackLimitTrigger stackLimitTrigger;

    #endregion
    #region UI物件參照區

    [SerializeField, Header("UI整個頁面CanvasGroup")]
    private CanvasGroup CanvasGroup_Page;

    #region  -> 遊戲頁面

    [SerializeField, Header("遊戲所有物件的根物件")]
    private GameObject Obj_GameSceneObjectsRoot;

    [SerializeField, Header("離開遊戲按鈕")]
    private Button Button_StopGame;

    [SerializeField, Header("NEXT圖片")]
    private Image Image_Next;

    [SerializeField, Header("前三名分數文字")]
    private Text[] Text_LeaderboardScores;

    [SerializeField, Header("自己分數文字")]
    private Text Text_YouScore;

    [SerializeField, Header("右側進化列表標題文字")]
    private Text Text_Evolution;

    [SerializeField, Header("右側進化列表物件")]
    private List<FruitEvolutionListObject> fruitEvolutionListObjects = new List<FruitEvolutionListObject>();

    [SerializeField, Header("新水果特效")]
    private Animator Animator_NEW_SweetPotato;

    #endregion
    #region  -> 結算畫面

    [SerializeField, Header("結算畫面UI")]
    private CanvasGroupWindow CanvasGroupWindow_Result;

    [SerializeField, Header("2ㄏ圖片")]
    private Image Image_HealingUhhuh;
    [SerializeField, Header("愛心圖片")]
    private Image Image_HealingHeart;
    [SerializeField, Header("截圖放置區")]
    private RawImage RawImage_ScreenShot_Result;
    [SerializeField, Header("結算分數文字")]
    private Text Text_ScoreValue_Result;
    [SerializeField, Header("新紀錄文字")]
    private Text Text_NewRecord_Result;
    [SerializeField, Header("再玩一次按鈕")]
    private Button Button_Retry_Result;
    [SerializeField, Header("回到標題按鈕")]
    private Button Button_ToTitle_Result;

    #endregion
    #region  -> 提交高分紀錄頁面

    [SerializeField, Header("提交高分紀錄頁面UI")]
    private CanvasGroup CanvasGroup_WindowPage_NewRecordSubmit;

    [SerializeField, Header("新紀錄副標題")]
    private Text Text_NewRecordSubmitSubtitle;
    [SerializeField, Header("新紀錄分數")]
    private Text Text_ScoreValue_NewRecordSubmit;
    [SerializeField, Header("新紀錄名字輸入")]
    private InputField InputField_Name_NewRecordSubmit;
    [SerializeField, Header("儲存分數按鈕")]
    private Button Button_SaveScore;

    #endregion

    #endregion
    #region 容器-水果配對

    private struct PendingFruitsPair {
        public FruitObject fruit1;
        public FruitObject fruit2;
    }

    #endregion
    #region 遊戲物件參照區

    [Header("游標邊界")]
    /// <summary>
    /// 移動游標左側邊界
    /// </summary>
    [SerializeField]
    private Transform Trans_LeftBound;

    /// <summary>
    /// 移動游標右側邊界
    /// </summary>
    [SerializeField]
    private Transform Trans_RightBound;

    /// <summary>
    /// 水果重生點
    /// </summary>
    [SerializeField, Header("水果重生點")]
    private Transform spawnPoint;

    /// <summary>
    /// 水果重生點顯示水果的位置
    /// </summary>
    [SerializeField, Header("水果重生點顯示水果的位置")]
    private Transform spawnPointContainer;

    /// <summary>
    /// 水果物件放置容器
    /// </summary>
    [SerializeField, Header("水果物件放置容器")]
    private Transform trans_FruitContainer;

    /// <summary>
    /// 露諾圖片
    /// </summary>
    [SerializeField, Header("露諾圖片")]
    private SpriteRenderer SpriteRenderer_Runo;

    #endregion
    #region 參數參考區

    /// <summary>
    /// 遊戲正在進行中
    /// </summary>
    private bool gamePlaying = false;

    /// <summary>
    /// 水果碰到天花板了
    /// </summary>
    private bool fruitTouchedLimitTrigger = false;

    private Vector3 spawnPointOriginal;

    private float moveSpeed = 3f;

    //分數
    private int nowYouScore = 0;

    //在重生游標上的水果
    private FruitObject fruitOnSpawnpointCursor = null;
    //下一個水果種類
    private FruitType nextSpawnFruitType = FruitType.Grape;

    //目前在場上已生成的水果
    private readonly List<FruitObject> fruitsInScene = new List<FruitObject>();

    //已申請合成的水果列表
    private readonly List<PendingFruitsPair> pendingCombines = new List<PendingFruitsPair>();
    //每次檢查時，已經被合成過的水果的HashSet
    private readonly HashSet<FruitObject> appliedCombinedFruits = new HashSet<FruitObject>();

    //水果合成次數，在生成Joker後會歸零
    private int jokerMeter_FruitCombine;

    #endregion
    #region Unity內建方法

    #region  -> Awake

    void Awake() {
        //初始化重生點位置
        spawnPointOriginal = spawnPoint.position;
        //重設遊戲
        ResetGame();
        //按鈕設定
        Button_StopGame.onClick.AddListener(OnClick_Button_StopGame);
        //水果觸碰天花板設定
        stackLimitTrigger.onFruitTouch.AddListener(OnTrigger_StackLimit);
        //預設關閉結算畫面
        CanvasGroupWindow_Result.SetEnableCanvasGroup(false);
        //結算畫面按鈕
        Button_Retry_Result.onClick.AddListener(OnClick_Button_Retry_Result);
        Button_ToTitle_Result.onClick.AddListener(OnClick_Button_ToTitle_Result);
        //新紀錄頁面關閉
        CanvasGroup_WindowPage_NewRecordSubmit.SetEnable(false);
        //提交分數頁面按鈕
        Button_SaveScore.onClick.AddListener(OnClick_Button_SaveScore_NewRecordSubmit);
    }

    #endregion
    #region  -> Update

    void Update() {
        if (!gamePlaying || fruitTouchedLimitTrigger) { return; }

        UpdateCheckPlayerInput();

        if (Input.GetKeyDown(KeyCode.Equals)) {
            //刪除水果
            if (fruitsInScene.Count > 0) {
                var toDispose = fruitsInScene[0];
                DisposeFruit(toDispose);
            }
        }
    }

    #endregion
    #region  -> FixedUpdate

    private void FixedUpdate() {
        if (!gamePlaying || fruitTouchedLimitTrigger) { return; }
        ExecuteAllCombineApplies();
    }

    #endregion

    #endregion
    #region 開關頁面

    /// <summary>
    /// 開關頁面
    /// </summary>
    public void SetEnableGamePage(bool value) {
        CanvasGroup_Page.SetEnable(value);
        Obj_GameSceneObjectsRoot.SetActive(value);
    }

    #endregion
    #region 水果結合相關

    #region  -> 檢查並實際處理所有結合要求

    /// <summary>
    /// 檢查並實際處理所有結合要求
    /// </summary>
    private void ExecuteAllCombineApplies() {
        //先清空已經被合成過的水果
        appliedCombinedFruits.Clear();
        //檢查是否有任何水果組合需要處理
        if (pendingCombines.Count > 0) {
            for (int i = 0; i < pendingCombines.Count; i++) {
                var pair = pendingCombines[i];
                //檢查這兩個水果是否有任一個已經被處理過了
                bool isApplied = (appliedCombinedFruits.Contains(pair.fruit1) || appliedCombinedFruits.Contains(pair.fruit2));
                if (isApplied) {
                    //已經有被處理過了，跳過此要求
                    continue;
                }
                //處理這兩個水果
                //先檢查是否真的可以結合
                if (pair.fruit1.CanCombineWith(pair.fruit2)) {
                    //先將這兩者加入已經被合成的列表上
                    appliedCombinedFruits.Add(pair.fruit1);
                    appliedCombinedFruits.Add(pair.fruit2);
                    //檢查下一階水果
                    int nextPhaseFruitId = (int)FruitObject.GetNormalTypeFromObjects(pair.fruit1, pair.fruit2);
                    if (nextPhaseFruitId != (int)FruitType.Disappear) {
                        nextPhaseFruitId += 1;
                    }
                    FruitType nextPhaseFruitType = (FruitType)nextPhaseFruitId;
                    if (nextPhaseFruitType != FruitType.Disappear) {
                        //水果並不是單純消失
                        //在這兩個水果的位置中間生成一個下一階的水果
                        Vector3 nextFruitPos = (pair.fruit1.transform.position + pair.fruit2.transform.position) / 2;
                        FruitObject newFruit = SpawnFruit(
                            type: nextPhaseFruitType,
                            parent: trans_FruitContainer,
                            worldPosition: nextFruitPos
                        );
                        newFruit.SetEnablePhysics(true);
                    }
                    //移除這兩個水果
                    DisposeFruit(pair.fruit1);
                    DisposeFruit(pair.fruit2);
                    //播放音效
                    Core.Instance.audioComponent.PlaySound(SoundId.Fruit_Combine);
                    //執行加分
                    int scoreAdd = (nextPhaseFruitId * (nextPhaseFruitId - 1)) / 2;
                    nowYouScore += scoreAdd;
                    UpdateDisplay_MyScore();
                    //增加Joker計數器
                    jokerMeter_FruitCombine++;
                    //檢查是否要處理地瓜特效
                    if (nextPhaseFruitType == FruitType.HealingLuka_Uhhuh && !PlayerPrefHelper.GetInfo_ShowSweetPotato()) {
                        //下一級是地瓜，並且右邊列表還沒有地瓜時
                        //設定成要顯示
                        PlayerPrefHelper.SetInfo_ShowSweetPotato(true);
                        //更新進化列表物件
                        UpdateDisplay_FruitEvolutionList();
                        //播放NEW特效
                        Animator_NEW_SweetPotato.SetTrigger("Play");
                        //播放特殊音效
                        Core.Instance.audioComponent.PlaySound(SoundId.FirstTimeSweetPotato);
                    }
                } else {
                    Log.Error($"以下兩個水果不能融合卻被加入到了合成請求中：\n{pair.fruit1.name} 與 {pair.fruit2.name}");
                    continue;
                }
            }
        }
        //最後清除所有需求
        pendingCombines.Clear();
    }

    #endregion
    #region  -> 外部申請結合

    /// <summary>
    /// 申請結合
    /// </summary>
    public void ApplyForFruitCombine(FruitObject self, FruitObject target) {
        pendingCombines.Add(new PendingFruitsPair() {
            fruit1 = self,
            fruit2 = target
        });
    }

    #endregion

    #endregion
    #region 水果生成刪除相關

    /// <summary>
    /// 生成一個新的隨機水果種類(可以手放生成的)
    /// </summary>
    /// <returns></returns>
    private FruitType NewRandomFruitType() {
        int jokerRan = Random.Range(0, 100);
        int jokerChance = jokerMeter_FruitCombine / 5;
        jokerChance.FixValueInRange(0, 10);
        if (jokerRan < jokerChance) {
            //計數器歸零
            jokerMeter_FruitCombine = 0;
            //生成Joker
            return FruitType.Joker;
        }
        int typeId = Random.Range((int)FruitType.Seed, (int)FruitType.Apple);
        return (FruitType)typeId;
    }

    /// <summary>
    /// 從水果工廠生成指定種類水果
    /// </summary>
    /// <param name="type"></param>
    /// <param name="parent"></param>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    private FruitObject SpawnFruit(FruitType type, Transform parent, Vector3 worldPosition) {
        var newFruit = fruitFactory.SpawnFruit(type);
        newFruit.transform.SetParent(parent: parent, worldPositionStays: true);
        newFruit.transform.position = worldPosition;
        newFruit.ClearTouchedInfo();
        newFruit.SetSpriteColor(Color.white);
        fruitsInScene.Add(newFruit);
        Quaternion newRotation;
        switch (type) {
            case FruitType.Seed:
            case FruitType.Papaya:
                newRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                break;
            default:
                newRotation = Quaternion.identity;
                break;
        }
        newFruit.transform.rotation = newRotation;
        FruitSpriteType fruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType();
        Sprite newSprite = fruitFactory.GetFruitSprite(fruitSpriteType, type);
        newFruit.SetSprite(newSprite);
        return newFruit;
    }

    /// <summary>
    /// 刪除指定水果
    /// </summary>
    /// <param name="toDispose"></param>
    private void DisposeFruit(FruitObject toDispose) {
        fruitsInScene.Remove(toDispose);
        fruitFactory.DisposeFruit(toDispose);
    }

    /// <summary>
    /// 刪除全部水果
    /// </summary>
    private void DisposeAllFruit() {
        for (int i = 0; i < fruitsInScene.Count; i++) {
            fruitFactory.DisposeFruit(fruitsInScene[i]);
        }
        fruitsInScene.Clear();
    }

    #endregion
    #region 水果觸碰天花板相關

    /// <summary>
    /// 當水果觸碰到天花板時
    /// </summary>
    private void OnTrigger_StackLimit() {
        if (gamePlaying && !fruitTouchedLimitTrigger) {
            //只有在遊戲還正在進行中、以及還沒碰到天花板時才處理
            fruitTouchedLimitTrigger = true;
            //遊戲結束表現
            GameOver();
        }
    }

    #endregion
    #region 遊戲結束

    /// <summary>
    /// 遊戲結束(觸碰到天花板之後)
    /// </summary>
    private async void GameOver() {
        //禁止按下離開遊戲
        Button_StopGame.interactable = false;
        //遊戲結束表現
        for (int i = 0; i < fruitsInScene.Count; i++) {
            fruitsInScene[i].SetEnablePhysics(false);
        }
        //處理截圖
        TaskCompletionSource<Texture2D> tcs_Texture2D = new TaskCompletionSource<Texture2D>();
        StartCoroutine(CaptureScreenshot(tcs_Texture2D));
        Texture2D resultScreenshot = await tcs_Texture2D.Task; //暫存截圖
        for (int i = 0; i < fruitsInScene.Count; i++) {
            fruitsInScene[i].SetSpriteColor(Color.gray);
            Core.Instance.audioComponent.PlaySound(SoundId.Fruit_Gray);
            await Task.Delay(50);
        }
        await Task.Delay(700);
        //顯示結算畫面
        bool isRefreshHighScore = Core.Instance.leaderboardDataComponent.IsRefreshHighScore(nowYouScore, out int rank); //確認是否刷新
        Image_HealingUhhuh.enabled = !isRefreshHighScore;
        Image_HealingHeart.enabled = isRefreshHighScore;
        RawImage_ScreenShot_Result.texture = resultScreenshot;
        Text_ScoreValue_Result.text = nowYouScore.ToString();
        Text_NewRecord_Result.enabled = isRefreshHighScore;
        if (isRefreshHighScore) {
            Text_NewRecordSubmitSubtitle.text = $"你刷新了排行第 {rank} 名的紀錄";
            Text_ScoreValue_NewRecordSubmit.text = nowYouScore.ToString();
            Core.Instance.audioComponent.PlaySound(SoundId.Result_Win);
        } else {
            Core.Instance.audioComponent.PlaySound(SoundId.Result_Lose);
        }
        CanvasGroup_WindowPage_NewRecordSubmit.SetEnable(isRefreshHighScore);
        CanvasGroupWindow_Result.SetShowWindow(true);
    }

    #endregion
    #region IEnumerator-截圖

    /// <summary>
    /// IEnumerator-截圖
    /// </summary>
    /// <param name="taskCompletionSource"></param>
    /// <returns></returns>
    private IEnumerator CaptureScreenshot(TaskCompletionSource<Texture2D> taskCompletionSource) {
        yield return new WaitForEndOfFrame();
        Texture2D texture2D = ScreenCapture.CaptureScreenshotAsTexture();
        taskCompletionSource.TrySetResult(texture2D);
    }

    #endregion
    #region Task-刷新下一個水果

    /// <summary>
    /// 刷新下一個水果
    /// </summary>
    private async void CoroutineNextFruit() {
        //等待0.5秒再生成水果與刷新下一個NEXT
        await Task.Delay(500);
        //生成水果
        fruitOnSpawnpointCursor = SpawnFruit(
            type: nextSpawnFruitType,
            parent: spawnPointContainer,
            worldPosition: spawnPointContainer.position
        );
        fruitOnSpawnpointCursor.SetEnablePhysics(false);
        //下一個
        nextSpawnFruitType = NewRandomFruitType();
        //刷新NEXT圖片
        UpdateDisplay_NextImage();
    }

    #endregion
    #region 外部方法-重設遊戲 

    /// <summary>
    /// 重設遊戲
    /// </summary>
    public void ResetGame() {
        //關閉遊戲狀態
        gamePlaying = false;
        fruitTouchedLimitTrigger = false;
        //清除所有水果
        DisposeAllFruit();
        //清除目前分數
        nowYouScore = 0;
        UpdateDisplay_MyScore();
        //初始化重生點位置
        spawnPoint.position = spawnPointOriginal;
    }

    #endregion
    #region 外部方法-開始遊戲

    /// <summary>
    /// 準備開始遊戲(播放過場前，用以預處理視覺上一定要有的部分)
    /// </summary>
    public void PrepareStart() {
        //生成一顆水果在重生點
        FruitType spawnpointFruitType = NewRandomFruitType();
        fruitOnSpawnpointCursor = SpawnFruit(
            type: spawnpointFruitType,
            parent: spawnPointContainer,
            worldPosition: spawnPointContainer.position
        );
        fruitOnSpawnpointCursor.SetEnablePhysics(false);
        //預先隨機NEXT
        nextSpawnFruitType = NewRandomFruitType();
        //刷新NEXT圖片
        UpdateDisplay_NextImage();
        //檢查夢見露諾是否要顯示
        SpriteRenderer_Runo.enabled = PlayerPrefHelper.GetSetting_FruitSpriteType() != FruitSpriteType.Normal;
        //抓取前三名分數並顯示
        UpdateDisplay_Leaderboard();
        //更新右側進化列表物件
        UpdateDisplay_FruitEvolutionList();
    }

    /// <summary>
    /// 開始遊戲(播放過場後)
    /// </summary>
    public void StartGame() {
        //開啟遊戲狀態
        gamePlaying = true;
        fruitTouchedLimitTrigger = false;
        //允許按下離開遊戲
        Button_StopGame.interactable = true;
    }

    #endregion
    #region 外部方法-停止遊戲

    /// <summary>
    /// 停止遊戲
    /// </summary>
    public void StopGame() {
        //關閉遊戲狀態
        gamePlaying = false;
        //關閉所有水果物理
        for (int i = 0; i < fruitsInScene.Count; i++) {
            fruitsInScene[i].SetEnablePhysics(false);
        }
    }

    #endregion
    #region 遊戲使用者操作邏輯-按鍵輸入

    /// <summary>
    /// 遊戲使用者操作邏輯-按鍵輸入
    /// </summary>
    private void UpdateCheckPlayerInput() {
        if (!gamePlaying || fruitTouchedLimitTrigger) { return; }

        OptionsPage optionsPage = Core.Instance.optionsPage;
        //允許重生點水果物理、處理下一個水果
        if (Input.GetKeyDown(optionsPage.keycode_PutFruit)) {
            if (fruitOnSpawnpointCursor != null) {
                //音效
                Core.Instance.audioComponent.PlaySound(SoundId.Fruit_Put);
                //放水果
                fruitOnSpawnpointCursor?.SetEnablePhysics(true);
                fruitOnSpawnpointCursor?.transform.SetParent(trans_FruitContainer, worldPositionStays: true);
                fruitOnSpawnpointCursor = null;
                CoroutineNextFruit();
            }
        }
        //移動重生點游標
        //左
        if (Input.GetKey(optionsPage.keycode_MoveLeft)) {
            float newX = spawnPoint.position.x;
            newX -= (Time.deltaTime * moveSpeed);
            newX = Mathf.Max(newX, Trans_LeftBound.position.x);
            spawnPoint.position = new Vector3(newX, spawnPointOriginal.y, spawnPointOriginal.z);
        }
        //右
        if (Input.GetKey(optionsPage.keycode_MoveRight)) {
            float newX = spawnPoint.position.x;
            newX += (Time.deltaTime * moveSpeed);
            newX = Mathf.Min(newX, Trans_RightBound.position.x);
            spawnPoint.position = new Vector3(newX, spawnPointOriginal.y, spawnPointOriginal.z);
        }
    }

    #endregion
    #region UI更新事件-更新NEXT圖片

    /// <summary>
    /// UI更新事件-更新NEXT圖片
    /// </summary>
    private void UpdateDisplay_NextImage() {
        FruitSpriteType fruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType();
        Sprite nextSprite = fruitFactory.GetFruitSprite(fruitSpriteType, nextSpawnFruitType);
        Image_Next.sprite = nextSprite;
        Vector2 newSize;
        if (nextSpawnFruitType == FruitType.Pineapple) {
            newSize = new Vector2(200, 200);
        } else {
            newSize = new Vector2(100, 100);
        }
        var rectTransform = Image_Next.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSize.y);
    }

    #endregion
    #region UI更新事件-更新左下角自己分數事件

    /// <summary>
    /// 更新左下角自己分數事件
    /// </summary>
    private void UpdateDisplay_MyScore() {
        Text_YouScore.text = nowYouScore.ToString();
    }

    #endregion
    #region UI更新事件-更新左下角記分板前三名分數事件

    /// <summary>
    /// 更新左下角記分板前三名分數事件
    /// </summary>
    private void UpdateDisplay_Leaderboard() {
        LeaderboardDataComponent leaderboardDataComponent = Core.Instance.leaderboardDataComponent;
        for (int i = LeaderboardDataComponent.FirstRank; i <= LeaderboardDataComponent.LastRank; i++) {
            Text textScore = Text_LeaderboardScores[i - 1];
            if (leaderboardDataComponent.TryGetLeaderboardData(i, out var data)) {
                textScore.text = data.Score.ToString();
            } else {
                textScore.text = "------";
            }
        }
    }

    #endregion
    #region UI更新事件-更新右側進化列表物件

    /// <summary>
    /// 更新右側進化列表物件
    /// </summary>
    private void UpdateDisplay_FruitEvolutionList() {
        bool show_HealingLuka_Uhhuh = PlayerPrefHelper.GetInfo_ShowSweetPotato();
        FruitSpriteType fruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType(); //從遊戲存檔讀取現在是選哪一個
        //處理列表標題
        string titleText;
        switch (fruitSpriteType) {
            default:
            case FruitSpriteType.Normal:
                titleText = "水果進化論";
                break;
            case FruitSpriteType.TofuSkin:
                titleText = "豆皮演化史";
                break;
        }
        Text_Evolution.text = titleText;
        //處理水果列表
        for (int i = 0; i < fruitEvolutionListObjects.Count; i++) {
            var obj = fruitEvolutionListObjects[i];
            var fruitType = obj.fruitType;
            obj.SetSprite(fruitFactory.GetFruitSprite(fruitSpriteType, fruitType));
            obj.SetFruitName(fruitFactory.GetFruitName(fruitSpriteType, fruitType));
            obj.SetImageScale((fruitType == FruitType.Pineapple) ? new Vector3(2, 2, 2) : Vector3.one); //鳳梨特製大小
            if (fruitType == FruitType.HealingLuka_Uhhuh) {
                obj.gameObject.SetActive(show_HealingLuka_Uhhuh);
            }
        }
    }

    #endregion
    #region UI按鈕事件

    /// <summary>
    /// 按下離開遊戲按鈕
    /// </summary>
    private async void OnClick_Button_StopGame() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Close);
        StopGame();
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Show();
        }
        ResetGame();
        SetEnableGamePage(false);
        Core.Instance.titlePage.SetEnableTitlePage(true);
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Hide();
        }
    }

    /// <summary>
    /// 結算畫面按下再玩一次
    /// </summary>
    private async void OnClick_Button_Retry_Result() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_StartGame);
        StopGame();
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Show();
        }
        ResetGame();
        CanvasGroupWindow_Result.SetShowWindow(false);
        PrepareStart();
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Hide();
        }
        StartGame();
    }

    /// <summary>
    /// 結算畫面按下回到標題
    /// </summary>
    private async void OnClick_Button_ToTitle_Result() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        StopGame();
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Show();
        }
        ResetGame();
        CanvasGroupWindow_Result.SetShowWindow(false);
        SetEnableGamePage(false);
        Core.Instance.titlePage.SetEnableTitlePage(true);
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Hide();
        }
    }

    /// <summary>
    /// 提交新紀錄按鈕
    /// </summary>
    private void OnClick_Button_SaveScore_NewRecordSubmit() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        Core.Instance.leaderboardDataComponent.SaveNewRecord(nowYouScore, InputField_Name_NewRecordSubmit.text);
        CanvasGroup_WindowPage_NewRecordSubmit.SetEnable(false);
    }

    #endregion

}
