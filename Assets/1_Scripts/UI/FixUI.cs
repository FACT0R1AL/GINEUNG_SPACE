using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixUI : MonoBehaviour
{
    [Header("필수 요소")]
    public Slider _Slider;

    // 범위 상자
    private RectTransform _rectRangeBox;
    private RectTransform _rectSlider;

    public bool _IsSuccessful = false;
    private bool _IsMove = false;

    // LeanTween 대체용 변수들
    [Header("설정")]
    public float speed = 100f; // 슬라이더가 움직이는 속도
    private bool _movingUp = true; // 현재 슬라이더 값이 증가 중인지 여부

    private void Awake()
    {
        // 삼항 연산자 대신 안전하게 null 체크 후 할당
        if (_rectRangeBox == null)
        {
            _rectRangeBox = _Slider.transform.GetChild(1).GetComponent<RectTransform>();
        }
        _rectRangeBox.gameObject.SetActive(false);

        _rectSlider = _Slider.GetComponent<RectTransform>();

        // 슬라이더의 기본 최소/최대값 강제 지정 (안전장치)
        _Slider.minValue = 0f;
        _Slider.maxValue = 100f;
    }

    public void Start()
    {
        OnStartSC();
    }

    private void Update()
    {
        // 게임이 시작되었고, 움직여야 하는 상태일 때만 실행
        if (!_IsMove) return;

        // LeanTween.setLoopPingPong()을 대체하는 핑퐁 로직
        if (_movingUp)
        {
            _Slider.value += speed * Time.deltaTime;
            if (_Slider.value >= _Slider.maxValue)
            {
                _Slider.value = _Slider.maxValue;
                _movingUp = false; // 감소 방향으로 전환
            }
        }
        else
        {
            _Slider.value -= speed * Time.deltaTime;
            if (_Slider.value <= _Slider.minValue)
            {
                _Slider.value = _Slider.minValue;
                _movingUp = true; // 증가 방향으로 전환
            }
        }
    }

    public void OnStartSC()
    {

        float fotRangeValue = Random.Range(5f, 85f); // 끝에 걸치지 않게 최대 범위를 조금 줄임
        float fotSize = Random.Range(20f, 100f);

        // 범위 상자 활성화
        _rectRangeBox.gameObject.SetActive(true);

        // 1. 범위 상자의 위치 설정 (슬라이더 가로 길이 기준 비율 계산)
        float sliderWidth = _rectSlider.rect.width;
        _rectRangeBox.anchoredPosition = new Vector2(sliderWidth * (fotRangeValue / 100f), 0);
        _rectRangeBox.sizeDelta = new Vector2(fotSize, _rectRangeBox.sizeDelta.y);

        // 2. 값 초기화 및 움직임 시작
        _Slider.value = 0f;
        _movingUp = true;
        _IsMove = true;
    }

    public void OnClickStopButton()
    {
        if (!_IsMove) return;

        _IsMove = false; // Update문 중지

        // 훨씬 직관적이고 정확한 성공 여부 판단 (값 자체로 비교)
        _IsSuccessful = IsValueInRange();

    }

    private bool IsValueInRange()
    {
        // 슬라이더의 전체 가로 길이
        float sliderWidth = _rectSlider.rect.width;
        if (sliderWidth <= 0) return false;

        // 범위 상자의 시작과 끝 위치가 전체 길이에서 차지하는 '비율(0~100%)' 계산
        float boxMinPercent = (_rectRangeBox.anchoredPosition.x / sliderWidth) * 100f;
        float boxMaxPercent = ((_rectRangeBox.anchoredPosition.x + _rectRangeBox.sizeDelta.x) / sliderWidth) * 100f;

        // 현재 슬라이더의 value가 그 비율(범위) 안에 들어와 있는지 검사
        // (+-2 정도의 약간의 판정 보정값(보너스 범위)을 주었습니다)
        float currentVal = _Slider.value;
        return (currentVal >= boxMinPercent - 2f) && (currentVal <= boxMaxPercent + 2f);
    }
}