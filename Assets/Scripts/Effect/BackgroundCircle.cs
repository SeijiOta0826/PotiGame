using UnityEngine;

public class BackgroundCircle : MonoBehaviour
{
    // ── 上昇・回転 ──
    private const float SPEED_MIN = 0.5f;
    private const float SPEED_MAX = 2.5f;
    private const float ROTATE_SPEED_MIN = -30f;
    private const float ROTATE_SPEED_MAX = 30f;
    private const float SCALE_MIN = 0.3f;
    private const float SCALE_MAX = 1.8f;

    // ── 横ドリフト（サイン波で左右に揺れる） ──
    private const float DRIFT_SPEED_MIN = -0.3f;
    private const float DRIFT_SPEED_MAX = 0.3f;
    private const float DRIFT_FREQ_MIN = 0.2f;
    private const float DRIFT_FREQ_MAX = 0.8f;

    // ── スケール脈動（サイン波でサイズが呼吸するように変化する） ──
    private const float PULSE_FREQ_MIN = 0.3f;
    private const float PULSE_FREQ_MAX = 1.2f;
    private const float PULSE_AMP_MIN = 0.05f;
    private const float PULSE_AMP_MAX = 0.2f;

    // ── フェードイン・アウト ──
    private const float ALPHA_MIN = 0.08f;
    private const float ALPHA_MAX = 0.25f;
    private const float FADE_SPEED_MIN = 0.5f;
    private const float FADE_SPEED_MAX = 1.5f;
    private const float FADE_OUT_INTERVAL_MIN = 3f;
    private const float FADE_OUT_INTERVAL_MAX = 8f;

    // 画面外に出てリセットされるまでのマージンと、下から再出現する際のオフセット
    private const float RESET_TOP_MARGIN = 2f;
    private const float RESET_BOTTOM_OFFSET = 1f;

    private SpriteRenderer spriteRenderer;
    private float screenHeight;
    private float screenWidth;

    private float speed;
    private float rotateSpeed;
    private float driftSpeed;
    private float driftFrequency;
    
    // 各円ごとに揺れのタイミング（位相）をずらし、全体の動きをバラバラにする
    private float driftPhase;
    
    private float targetAlpha;
    private float currentAlpha;
    private float fadeSpeed;
    private bool  fadingOut;
    private float fadeOutTimer;
    private float fadeOutInterval;
    private float baseScale;
    private float pulseFrequency;
    
    // 各円のスケール脈動のタイミングをずらし、不自然なシンクロを防ぐ
    private float pulsePhase;
    private float pulseAmplitude;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // カメラの正投影サイズからワールド空間における画面の高さと幅を計算する
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth  = screenHeight * Camera.main.aspect;

        // 各種ランダムパラメータを初期化し、円ごとに固有の動きを設定する
        speed        = Random.Range(SPEED_MIN,       SPEED_MAX);
        rotateSpeed  = Random.Range(ROTATE_SPEED_MIN, ROTATE_SPEED_MAX);

        // 基本サイズを決定し、脈動の基準となる初期スケールを適用する
        baseScale    = Random.Range(SCALE_MIN, SCALE_MAX);
        transform.localScale = Vector3.one * baseScale;

        // 横揺れの速度・周波数・位相をランダムに設定する
        driftSpeed     = Random.Range(DRIFT_SPEED_MIN, DRIFT_SPEED_MAX);
        driftFrequency = Random.Range(DRIFT_FREQ_MIN,  DRIFT_FREQ_MAX);
        driftPhase     = Random.Range(0f, Mathf.PI * 2f);

        // 呼吸するようなスケール脈動の周波数・位相・振幅をランダムに設定する
        pulseFrequency = Random.Range(PULSE_FREQ_MIN, PULSE_FREQ_MAX);
        pulsePhase     = Random.Range(0f, Mathf.PI * 2f);
        pulseAmplitude = Random.Range(PULSE_AMP_MIN,  PULSE_AMP_MAX);

        // フェード後の目標アルファ値とフェード速度を決定する
        targetAlpha      = Random.Range(ALPHA_MIN,             ALPHA_MAX);
        currentAlpha     = 0f;
        fadeSpeed        = Random.Range(FADE_SPEED_MIN,         FADE_SPEED_MAX);
        
        fadingOut        = false;
        fadeOutInterval  = Random.Range(FADE_OUT_INTERVAL_MIN,  FADE_OUT_INTERVAL_MAX);
        
        // 初期タイマーをランダムにずらし、全円が同時にフェードアウトしないようにする
        fadeOutTimer     = Random.Range(0f, fadeOutInterval);

        // 画面内のランダムな位置を初期座標として決定する
        float x = Random.Range(-screenWidth  / 2f, screenWidth  / 2f);
        float y = Random.Range(-screenHeight / 2f, screenHeight / 2f);
        transform.position = new Vector3(x, y, 0f);

        // 出現時は必ず透明からスタートさせ、フェードイン処理につなげる
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // 直進的な上昇移動を適用する
        transform.position += Vector3.up * speed * dt;

        // サイン波を用いて横方向に揺らし、driftPhase で各円の波形タイミングをずらす
        float drift = driftSpeed * Mathf.Sin(driftFrequency * Time.time + driftPhase);
        transform.position += Vector3.right * drift * dt;

        // 指定された回転速度でZ軸回りに回転させる
        transform.Rotate(0f, 0f, rotateSpeed * dt);

        // サイン波で 1 ± amplitude の範囲を求め、基準サイズに乗算してスケールを脈動させる
        float pulse = 1f + pulseAmplitude * Mathf.Sin(pulseFrequency * Time.time + pulsePhase);
        transform.localScale = Vector3.one * baseScale * pulse;

        // フェードアウト開始までのタイマーを進行させる
        fadeOutTimer += dt;

        // 待機時間を超えたら、フェードアウト状態へ移行するフラグを立てる
        if (fadeOutTimer >= fadeOutInterval && !fadingOut)
        {
            fadingOut = true;
        }

        if (fadingOut)
        {
            // フェード速度に応じて徐々に透明にしていく
            currentAlpha -= fadeSpeed * dt;
            if (currentAlpha <= 0f)
            {
                // 完全に透明になったら画面下部へリセットし、再びフェードインさせる準備をする
                currentAlpha    = 0f;
                fadingOut       = false;
                fadeOutTimer    = 0f;
                fadeOutInterval = Random.Range(FADE_OUT_INTERVAL_MIN, FADE_OUT_INTERVAL_MAX);
                ResetPosition();
            }
        }
        else
        {
            // 目標のアルファ値に達するまで徐々に不透明にしていく
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * dt);
        }

        // 計算された最新のアルファ値をスプライトカラーに反映する
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = currentAlpha;
            spriteRenderer.color = c;
        }

        // 画面上端の余裕分を超えて見えなくなったら、画面下部へワープさせる
        if (transform.position.y > screenHeight / 2f + RESET_TOP_MARGIN)
        {
            ResetPosition();
            currentAlpha = 0f;
            fadingOut    = false;
            fadeOutTimer = 0f;
        }
    }

    void ResetPosition()
    {
        // X座標はランダム、Y座標は画面の下端よりさらに下に配置し、見えない位置から再スタートさせる
        float x = Random.Range(-screenWidth / 2f, screenWidth / 2f);
        transform.position = new Vector3(x, -screenHeight / 2f - RESET_BOTTOM_OFFSET, 0f);

        // パラメータを再抽選し、次に上昇してくるときは違う動き・見た目にする
        speed          = Random.Range(SPEED_MIN,        SPEED_MAX);
        rotateSpeed    = Random.Range(ROTATE_SPEED_MIN, ROTATE_SPEED_MAX);
        baseScale      = Random.Range(SCALE_MIN,        SCALE_MAX);
        targetAlpha    = Random.Range(ALPHA_MIN,        ALPHA_MAX);
        driftSpeed     = Random.Range(DRIFT_SPEED_MIN,  DRIFT_SPEED_MAX);
        driftFrequency = Random.Range(DRIFT_FREQ_MIN,   DRIFT_FREQ_MAX);
        driftPhase     = Random.Range(0f, Mathf.PI * 2f);
        pulseAmplitude = Random.Range(PULSE_AMP_MIN,    PULSE_AMP_MAX);
        pulseFrequency = Random.Range(PULSE_FREQ_MIN,   PULSE_FREQ_MAX);
    }
}
