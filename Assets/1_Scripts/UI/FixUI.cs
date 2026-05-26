using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class FixUI : MonoBehaviour
{
    [Header("�ʼ� ���")]
    public Slider _Slider;
    public Button _goStopButton;
    public Transform itemInfoPanel;
    public GameObject itemInfoPrefab;
    public GameObject fixStartPanel;

    public Text nameText;

    private RectTransform _rectRangeBox;
    private RectTransform _rectSlider;

    private bool _IsSuccessful = false;
    private bool _IsMove = false;

    [Header("�����̴� ���� ����")]
    public float speed = 100f; // �����̴� �պ� �ӵ�
    private bool _movingUp = true;

    [Header("�� ���� ���� ũ�� ���� (0~100 ���� �ۼ�Ʈ)")]
    [Range(1f, 100f)] public float minSizePercent = 15f; // �ν����Ϳ��� ������ �ּ� ũ��
    [Range(1f, 100f)] public float maxSizePercent = 30f; // �ν����Ϳ��� ������ �ִ� ũ��

    // ���ο��� ����� ���� ���� ���� ���� (0 ~ 100 ����)
    private float _rangeMin = 0f;
    private float _rangeMax = 0f;

    public FixType fixType;
    public FixInfo fixInfo;

    public GameObject SussecUI;
    public GameObject failUI;
    public Animator sussecAnimator;
    public Animator failAnimator;

    private void Awake()
    {
        if (_rectRangeBox == null)
        {
            _rectRangeBox = _Slider.transform.GetChild(1).GetComponent<RectTransform>();
        }
        _rectRangeBox.gameObject.SetActive(false);
        _rectSlider = _Slider.GetComponent<RectTransform>();

        // UI ���� ���� �ʱ�ȭ (���� ����, ���� �ǹ�)
        _rectRangeBox.anchorMin = new Vector2(0f, 0.5f);
        _rectRangeBox.anchorMax = new Vector2(0f, 0.5f);
        _rectRangeBox.pivot = new Vector2(0f, 0.5f);

        _Slider.minValue = 0f;
        _Slider.maxValue = 100f;
        
        SussecUI.SetActive(false);
        failUI.SetActive(false);

        gameObject.SetActive(false);
    }

    public void Init()
    {
        _rectRangeBox.anchorMin = new Vector2(0f, 0.5f);
        _rectRangeBox.anchorMax = new Vector2(0f, 0.5f);
        _rectRangeBox.pivot = new Vector2(0f, 0.5f);
        fixStartPanel.SetActive(true);
    }

    public void fixStart()
    {
        bool isFix = true;


        foreach (var fixData in fixInfo.fixData)
        {
            var itemData = fixData.itemdata;

            int count = InventoryManager.Instance.GetItemCount(itemData.itemType);

            if (count <= 0)
            {
                isFix = false;
                break;
            }
            else if (count < itemData.count)
            {
                isFix = false;
                break;
            }

        }

        if (isFix)
        {
            fixStartPanel.SetActive(false);
            OnStartSC();
        }
        else
        {
           GameManager.instance.SendMessage("재료가 부족합니다!", Color.red);
        }
    }

    public void Show(FixType fixType)
    {
        gameObject.SetActive(true);
        Init();
        this.fixType = fixType;
        FixInfo info = FixManager.Instance.GetFixInfo(fixType);
        fixInfo = info;
        nameText.text = info.fixName + " 수리";
        for (int i = itemInfoPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(itemInfoPanel.GetChild(i).gameObject);
        }
        foreach (var itemData in info.fixData)
        {
            var itemInfoObj = Instantiate(itemInfoPrefab, itemInfoPanel);
            var itemInfo = itemInfoObj.GetComponent<ItemFixSlot>();
            var sprite = GameManager.instance.GetSprite(itemData.itemdata.itemType);
            int level = 0;
            switch (itemData.itemdata.itemType)
            {
                case ItemType.IronLv1:
                case ItemType.CopperLv1:
                case ItemType.PlasticLv1:
                    level = 1;
                    break;
                case ItemType.IronLv2:
                    case ItemType.CopperLv2:
                    case ItemType.PlasticLv2:
                    level = 2;
                    break;
                    case ItemType.IronLv3:
                    case ItemType.CopperLv3:
                    case ItemType.PlasticLv3:
                    level = 3;
                    break;
            }
            itemInfo.SetItem(sprite, level, itemData.itemdata.count);
        }
    }

    private void Update()
    {
        if (!_IsMove) return;

        // �����̴� �� ���� (0 ~ 100 ���� �պ�)
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

        // ���� ����: ���� �ּڰ��� �ִ񰪺��� ũ�� �����Ǿ� �ִٸ� �ڵ����� ����
        if (minSizePercent > maxSizePercent)
        {
            float temp = minSizePercent;
            minSizePercent = maxSizePercent;
            maxSizePercent = temp;
        }

        // ��ü �����̴��� ���� �ȼ� ���� ũ��
        float sliderWidth = _rectSlider.rect.width;

        // 1. �ν����Ϳ��� ������ �ּ�/�ִ� ���� ���̿��� �����ϰ� ũ�⸦ �����մϴ�.
        float sizePercent = Random.Range(minSizePercent, maxSizePercent);

        // 2. �ִ� 100�� ���� �ʵ���, ��ġ ������ �ִ� �������� ���� (100 - ����ũ��)
        float maxStartPercent = 100f - sizePercent;

        // 3. ������ ���� ������ ���� �ۼ�Ʈ ���� (���� ���� ��Ż �Ұ�)
        _rangeMin = Random.Range(0f, maxStartPercent);
        _rangeMax = _rangeMin + sizePercent;

        // 4. ������ 0~100 ���� ���� ���� UI �ȼ� ũ��� ��ȯ�Ͽ� ����
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

        // ���� �����̴� ���� �̴ϸذ� �ƽø� ���̿� �ִ°�?
        _IsSuccessful = (_Slider.value >= _rangeMin) && (_Slider.value <= _rangeMax);

        if (_IsSuccessful)
        {
            Debug.Log($"<color=green>[�̴ϰ��� ����]</color> ���� ��ġ: {_Slider.value:F2} (���� ����: {_rangeMin:F2} ~ {_rangeMax:F2})");
            SussecUI.SetActive(true);
            sussecAnimator.Rebind();
            sussecAnimator.Play(0);
            FixManager.Instance.Fix(fixType);
            foreach (var fixData in fixInfo.fixData)
            {
                int count = InventoryManager.Instance.GetItemCount(fixData.itemdata.itemType);
                if (count >= fixData.itemdata.count)
                {
                    count -= fixData.itemdata.count;

                    int index = (int)fixData.itemdata.itemType;

                    for (int i = 0; i < InventoryManager.Instance.inventoryItems.Count; i++)
                    {
                        if (InventoryManager.Instance.inventoryItems[i].itemType == fixData.itemdata.itemType)
                        {
                            var item = InventoryManager.Instance.inventoryItems[i];
                            item.count = count;
                            InventoryManager.Instance.inventoryItems[i] = item;
                        }
                    }
                }
            } 
            StartCoroutine(Hide());
        }
        else
        {
            Debug.Log($"<color=red>[�̴ϰ��� ����]</color> ���� ��ġ: {_Slider.value:F2} (���� ����: {_rangeMin:F2} ~ {_rangeMax:F2})");
            failUI.SetActive(true);
            failAnimator.Rebind();
            failAnimator.Play(0);
            StartCoroutine(Hide());
        }

        _goStopButton.interactable = false;
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(1f); // 1�� ��� �� ����
        SussecUI.SetActive(false);
        failUI.SetActive(false);
        gameObject.SetActive(false);
    }
}