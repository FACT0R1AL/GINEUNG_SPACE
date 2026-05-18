using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixUI : MonoBehaviour
{
    [Header("필수 요소")]
    public Slider _Slider;
    public Button _goStopButton;

    private RectTransform _rectRangeBox;
    private RectTransform _rectSlider;

    private bool _IsSuccessful = false;
    private bool _IsMove = false;

    [Header("슬라이더 구동 설정")]
    public float speed = 100f; // 슬라이더 왕복 속도
    private bool _movingUp = true;

    [Header("★ 범위 상자 크기 조절 (0~100 기준 퍼센트)")]
    [Range(1f, 100f)] public float minSizePercent = 15f; // 인스펙터에서 조절할 최소 크기
    [Range(1f, 100f)] public float maxSizePercent = 30f; // 인스펙터에서 조절할 최대 크기

    // 내부에서 사용할 성공 범위 저장 변수 (0 ~ 100 기준)
    private float _rangeMin = 0f;
    private float _rangeMax = 0f;

    public FixType fixType;

    private void Awake()
    {
        if (_rectRangeBox == null)
        {
            _rectRangeBox = _Slider.transform.GetChild(1).GetComponent<RectTransform>();
        }
        _rectRangeBox.gameObject.SetActive(false);
        _rectSlider = _Slider.GetComponent<RectTransform>();

        // UI 정렬 강제 초기화 (왼쪽 정렬, 왼쪽 피벗)
        _rectRangeBox.anchorMin = new Vector2(0f, 0.5f);
        _rectRangeBox.anchorMax = new Vector2(0f, 0.5f);
        _rectRangeBox.pivot = new Vector2(0f, 0.5f);

        _goStopButton.interactable = false;

        _Slider.minValue = 0f;
        _Slider.maxValue = 100f;

        gameObject.SetActive(false);
    }

    public void Show(FixType fixType)
    {
        gameObject.SetActive(true);
        OnStartSC();
        this.fixType = fixType;
    }

    private void Update()
    {
        if (!_IsMove) return;

        // 슬라이더 바 구동 (0 ~ 100 무한 왕복)
        if (_movingUp)
        {
            _Slider.value += speed * Time.deltaTime;
            if (_Slider.value >= _Slider.maxValue)
            {
                _Slider.value = _Slider.maxValue;
                _movingUp = false;
            }
        }
        else
        {
            _Slider.value -= speed * Time.deltaTime;
            if (_Slider.value <= _Slider.minValue)
            {
                _Slider.value = _Slider.minValue;
                _movingUp = true;
            }
        }
    }

    public void OnStartSC()
    {
        _goStopButton.interactable = true;
        _Slider.value = 0f;
        _movingUp = true;

        // 예외 방지: 만약 최솟값이 최댓값보다 크게 설정되어 있다면 자동으로 보정
        if (minSizePercent > maxSizePercent)
        {
            float temp = minSizePercent;
            minSizePercent = maxSizePercent;
            maxSizePercent = temp;
        }

        // 전체 슬라이더의 실제 픽셀 가로 크기
        float sliderWidth = _rectSlider.rect.width;

        // 1. 인스펙터에서 설정한 최소/최대 범위 사이에서 랜덤하게 크기를 결정합니다.
        float sizePercent = Random.Range(minSizePercent, maxSizePercent);

        // 2. 최대 100을 넘지 않도록, 배치 가능한 최대 시작점을 제한 (100 - 상자크기)
        float maxStartPercent = 100f - sizePercent;

        // 3. 안전한 범위 내에서 시작 퍼센트 결정 (절대 우측 이탈 불가)
        _rangeMin = Random.Range(0f, maxStartPercent);
        _rangeMax = _rangeMin + sizePercent;

        // 4. 결정된 0~100 기준 값을 실제 UI 픽셀 크기로 변환하여 적용
        float pixelPositionX = sliderWidth * (_rangeMin / 100f);
        float pixelWidth = sliderWidth * (sizePercent / 100f);

        _rectRangeBox.anchoredPosition = new Vector2(pixelPositionX, 0f);
        _rectRangeBox.sizeDelta = new Vector2(pixelWidth, _rectRangeBox.sizeDelta.y);

        _rectRangeBox.gameObject.SetActive(true);
        _IsMove = true;
    }

    public void OnClickStopButton()
    {
        if (!_IsMove) return;

        _IsMove = false;

        // 현재 슬라이더 값이 미니멈과 맥시멈 사이에 있는가?
        _IsSuccessful = (_Slider.value >= _rangeMin) && (_Slider.value <= _rangeMax);

        if (_IsSuccessful)
        {
            Debug.Log($"<color=green>[미니게임 성공]</color> 멈춘 위치: {_Slider.value:F2} (정답 범위: {_rangeMin:F2} ~ {_rangeMax:F2})");
            FixManager.Instance.Fix(fixType);
            StartCoroutine(Hide());
        }
        else
        {
            Debug.Log($"<color=red>[미니게임 실패]</color> 멈춘 위치: {_Slider.value:F2} (정답 범위: {_rangeMin:F2} ~ {_rangeMax:F2})");
            StartCoroutine(Hide());
        }

        _goStopButton.interactable = false;
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(1f); // 1초 대기 후 숨김
        gameObject.SetActive(false);
    }
}